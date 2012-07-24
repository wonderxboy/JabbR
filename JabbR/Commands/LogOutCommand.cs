using JabbR.Models;

namespace JabbR.Commands
{
    [Command("logout", "Type /logout - To logout from this client (chat cookie will be removed).")]
    public class LogOutCommand : UserCommand
    {
        public override void Execute(CommandContext context, CallerContext callerContext, ChatUser callingUser, string[] args)
        {
            context.NotificationService.LogOut(callingUser, callerContext.ClientId);
        }
    }
}