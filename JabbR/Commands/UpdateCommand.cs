using JabbR.Models;

namespace JabbR.Commands
{
    [Command("update", "Type /update to force a refresh for everyone. Only administrators can use this command.")]
    public class UpdateCommand : AdminCommand
    {        
        public override void ExecuteAdminOperation(CommandContext context, CallerContext callerContext, ChatUser callingUser, string[] args)
        {
            context.NotificationService.ForceUpdate();
        }
    }
}