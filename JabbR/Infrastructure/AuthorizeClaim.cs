using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.AspNet.SignalR.Owin;
using JabbR.Auth;

namespace JabbR.Infrastructure
{
    public class AuthorizeClaim : AuthorizeAttribute
    {
        private readonly string _claimType;
        public AuthorizeClaim(string claimType)
        {
            _claimType = claimType;
        }

        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            var secureDataFormat = new TicketDataFormat(DataProtectorSingleton.GetInstance().Protector);
            // authenticate by using bearer token in query string
            var token = request.QueryString.Get(SignalRTokenAuth.QueryStringKey);
            var ticket = secureDataFormat.Unprotect(token);
            if (ticket != null && ticket.Identity != null && ticket.Identity.IsAuthenticated)
            {
                // set the authenticated user principal into environment so that it can be used in the future
                request.Environment["server.User"] = new ClaimsPrincipal(ticket.Identity);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var connectionId = hubIncomingInvokerContext.Hub.Context.ConnectionId;
            // check the authenticated user principal from environment
            var environment = hubIncomingInvokerContext.Hub.Context.Request.Environment;
            var principal = environment["server.User"] as ClaimsPrincipal;
            if (principal != null && principal.Identity != null && principal.Identity.IsAuthenticated)
            {
                // create a new HubCallerContext instance with the principal generated from token
                // and replace the current context so that in hubs we can retrieve current user identity
                hubIncomingInvokerContext.Hub.Context = new HubCallerContext(new ServerRequest(environment), connectionId);
                return true;
            }
            else
            {
                return false;
            }
        }


        //protected override bool UserAuthorized(IPrincipal user)
        //{
        //    var claimsPrincipal = user as ClaimsPrincipal;

        //    return claimsPrincipal != null && claimsPrincipal.HasClaim(_claimType);
        //}
    }
}