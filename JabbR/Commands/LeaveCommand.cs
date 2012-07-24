using System.Web;
using JabbR.Models;

namespace JabbR.Commands
{
    [Command("leave", "Type /leave to leave the current room. Type /leave [room name] to leave a specific room.")]
    public class LeaveCommand : UserCommand
    {
        public override void Execute(CommandContext context, CallerContext callerContext, ChatUser callingUser, string[] args)
        {
            ChatRoom room = null;
            if (args.Length  == 0)
            {
                room = context.Repository.VerifyUserRoom(context.Cache, callingUser, callerContext.RoomName);                
            }
            else
            {
                string roomName = HttpUtility.HtmlDecode(args[0]);

                room = context.Repository.VerifyRoom(roomName, mustBeOpen: false);
            }

            context.Service.LeaveRoom(callingUser, room);

            context.NotificationService.LeaveRoom(callingUser, room);

            context.Repository.CommitChanges();
        }
    }
}