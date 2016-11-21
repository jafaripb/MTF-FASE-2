using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace EOffice.Directory.App.Helper
{
    public class APIAuthorizationManager : ResourceAuthorizationManager
    {

        public bool UserHasClaim(ResourceAuthorizationContext context)
        {
            return context.Principal.HasClaim(PermissionDefinition.Claims.Role, PermissionDefinition.Role.User);
        }

        public bool AdminHasClaim(ResourceAuthorizationContext context)
        {
            return context.Principal.HasClaim(PermissionDefinition.Claims.Role, PermissionDefinition.Role.User);
        }

        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            switch (context.Resource.First().Value)
            {
                case PermissionDefinition.Resources.MasterData.OrganisationUnit:
                    return AuthorizeMasterData(context);
                case PermissionDefinition.Resources.ProfileUser.BasicInformation:
                    return AuthorizeProfileUser(context);
                default:
                    return Nok();
            }

        }

        private Task<bool> AuthorizeMasterData(ResourceAuthorizationContext context)
        {
            switch (context.Action.First().Value)
            {
                case PermissionDefinition.Action.Read:
                    return Eval(AdminHasClaim(context) || UserHasClaim(context));
                case PermissionDefinition.Action.Create:
                    return Eval(AdminHasClaim(context));
                case PermissionDefinition.Action.Update:
                    return Eval(AdminHasClaim(context));
                case PermissionDefinition.Action.Delete:
                    return Eval(AdminHasClaim(context));
                default:
                    return Nok();
            }
        }

        private Task<bool> AuthorizeProfileUser(ResourceAuthorizationContext context)
        {
            switch (context.Action.First().Value)
            {
                case PermissionDefinition.Action.Read:
                    return Eval(AdminHasClaim(context) || UserHasClaim(context));
                case PermissionDefinition.Action.Create:
                    return Eval(AdminHasClaim(context));
                case PermissionDefinition.Action.Update:
                    return Eval(AdminHasClaim(context));
                case PermissionDefinition.Action.Delete:
                    return Eval(AdminHasClaim(context));
                default:
                    return Nok();
            }
        }
    }
}
