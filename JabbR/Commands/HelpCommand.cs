namespace JabbR.Commands
{
    [Command("help", "Type /help to show the list of commands")]
    public class HelpCommand : ICommand
    {
        public void Execute(CommandContext context, CallerContext callerContext, string[] args)
        {
            // TODO: Discover all commands and pass then here, instead of hardcoding them
            context.NotificationService.ShowHelp();
        }
    }
}