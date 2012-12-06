using System;
using System.Linq;
using System.Web;
using JabbR.Models;

namespace JabbR.Commands
{
    [Command("delete", "Delete a room.", "room", "room")]
    public class DeleteCommand : UserCommand
    {
        public override void Execute(CommandContext context, CallerContext callerContext, ChatUser callingUser, string[] args)
        {
            if (args.Length > 1)
            {
                throw new InvalidOperationException("Room name cannot contain spaces.");
            }

            if (args.Length == 0)
            {
                throw new InvalidOperationException("No room specified.");
            }

            string roomName = HttpUtility.HtmlDecode(args[0]);
            if (String.IsNullOrWhiteSpace(roomName))
            {
                throw new InvalidOperationException("No room specified.");
            }

            ChatRoom room = context.Repository.GetRoomByName(roomName);

            if (room == null)
            {
                throw new InvalidOperationException(String.Format("The room '{0}' does not exist", roomName));
            }

            // Get the list of users in the room to notify in a moment
            // If the user initiating this command is not in the room, add them
            //now so that they still get a notification the room was deleted
            var users = room.Users.ToList();

            if (!users.Contains(callingUser))
                users.Add(callingUser);

            context.Service.DeleteRoom(callingUser, room);

            context.Repository.CommitChanges();

            context.NotificationService.DeleteRoom(users, room);
        }
    }
}