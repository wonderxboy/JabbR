using System;
using JabbR.Infrastructure;
using JabbR.Models;

namespace JabbR.Commands
{
    [Command("resetinvitecode", "Type /resetinvitecode - To reset the current invite code. This will render the previous invite code invalid")]
    public class ResetInviteCodeCommand : UserCommand
    {
        public override void Execute(CommandContext context, CallerContext callerContext, Models.ChatUser callingUser, string[] args)
        {
            ChatRoom room = context.Repository.VerifyUserRoom(context.Cache, callingUser, callerContext.RoomName);

            context.Service.SetInviteCode(callingUser, room, RandomUtils.NextInviteCode());

            context.NotificationService.PostNotification(room, callingUser, String.Format("Invite Code for this room: {0}", room.InviteCode));
        }
    }
}