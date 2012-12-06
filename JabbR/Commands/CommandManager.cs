﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using JabbR.Models;
using JabbR.Services;
using JabbR.Commands.Exceptions;

namespace JabbR.Commands
{
    public class CommandManager
    {
        private readonly string _clientId;
        private readonly string _userAgent;
        private readonly string _userId;
        private readonly string _roomName;
        private readonly INotificationService _notificationService;
        private readonly IChatService _chatService;
        private readonly ICache _cache;
        private readonly IJabbrRepository _repository;

        private static Dictionary<string, ICommand> _commandCache;
        private static readonly Lazy<IList<ICommand>> _commands = new Lazy<IList<ICommand>>(GetCommands);

        public CommandManager(string clientId,
                              string userId,
                              string roomName,
                              IChatService service,
                              IJabbrRepository repository,
                              ICache cache,
                              INotificationService notificationService)
            : this(clientId, null, userId, roomName, service, repository, cache, notificationService)
        {
        }

        public CommandManager(string clientId,
                              string userAgent,
                              string userId,
                              string roomName,
                              IChatService service,
                              IJabbrRepository repository,
                              ICache cache,
                              INotificationService notificationService)
        {
            _clientId = clientId;
            _userAgent = userAgent;
            _userId = userId;
            _roomName = roomName;
            _chatService = service;
            _repository = repository;
            _cache = cache;
            _notificationService = notificationService;
        }

        public string ParseCommand(string commandString, out string[] args)
        {
            var parts = commandString.Substring(1).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            args = parts.Skip(1).ToArray();
            return parts[0];
        }
        public bool TryHandleCommand(string command)
        {
            command = command.Trim();
            if (!command.StartsWith("/"))
            {
                return false;
            }

            string[] args;
            var commandName = ParseCommand(command, out args);
            return TryHandleCommand(commandName, args);
        }

        public bool TryHandleCommand(string commandName, string[] args)
        {
            commandName = commandName.Trim();
            if (commandName.StartsWith("/"))
            {
                return false;
            }

            var context = new CommandContext
            {
                Cache = _cache,
                NotificationService = _notificationService,
                Repository = _repository,
                Service = _chatService
            };

            var callerContext = new CallerContext
            {
                ClientId = _clientId,
                UserId = _userId,
                UserAgent = _userAgent,
                RoomName = _roomName,
            };

            ICommand command;
            try
            {
                MatchCommand(commandName, out command);
            }
            catch (CommandNotFoundException)
            {
                throw new InvalidOperationException(String.Format("'{0}' is not a valid command.", commandName));
            }
            catch (CommandAmbiguityException e)
            {
                throw new InvalidOperationException(String.Format("'{0}' is ambiguous: {1}.", commandName, e.Ambiguities.Aggregate((s, r) => s += ", " + r)));
            }

            command.Execute(context, callerContext, args);

            return true;
        }

        private void MatchCommand(string commandName, out ICommand command)
        {
            if (_commandCache == null)
            {
                var commands = from c in _commands.Value
                               let commandAttribute = c.GetType()
                                                       .GetCustomAttributes(true)
                                                       .OfType<CommandAttribute>()
                                                       .FirstOrDefault()
                               where commandAttribute != null
                               select new
                               {
                                   Name = commandAttribute.CommandName,
                                   Command = c
                               };

                _commandCache = commands.ToDictionary(c => c.Name,
                                                      c => c.Command,
                                                      StringComparer.OrdinalIgnoreCase);
            }

            ExpandCommand(_commandCache.Keys, ref commandName);

            if (!_commandCache.TryGetValue(commandName, out command))
            {
                throw new CommandNotFoundException();
            }
        }

        private static IList<ICommand> GetCommands()
        {
            // Use MEF to locate the content providers in this assembly
            var catalog = new AssemblyCatalog(typeof(CommandManager).Assembly);
            var compositionContainer = new CompositionContainer(catalog);
            return compositionContainer.GetExportedValues<ICommand>().ToList();
        }

        public static IEnumerable<CommandMetaData> GetCommandsMetaData()
        {
            var commands = from c in _commands.Value
                           let commandAttribute = c.GetType()
                                                   .GetCustomAttributes(true)
                                                   .OfType<CommandAttribute>()
                                                   .FirstOrDefault()
                           where commandAttribute != null
                           select new CommandMetaData
                           {
                               Name = commandAttribute.CommandName,
                               Description = commandAttribute.Description,
                               Arguments = commandAttribute.Arguments,
                               Group = commandAttribute.Group
                           };
            return commands;
        }

        private static void ExpandCommand(IEnumerable<string> commandNames, ref string commandName)
        {
            var inputCommand = commandName;
            var extended = commandNames.Where(comm => comm.StartsWith(inputCommand));

            if (extended.Count() == 0) return;

            try
            {
                commandName = extended.Single();
            }
            catch (InvalidOperationException)
            {
                throw new CommandAmbiguityException(extended);
            }
        }
    }
}