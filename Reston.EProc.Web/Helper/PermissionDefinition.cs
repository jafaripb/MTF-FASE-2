using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.EProc.Web.Helper
{
    public static class PermissionDefinition
    {
        public static class Role
        {
            public const string User = "user_directory";
            public const string Admin = "admin_directory";
        }
        public static class Claims
        {
            public const string Role = "role";
        }

        public static class Action
        {
            public const string Read = "read";
            public const string Create = "create";
            public const string Update = "update";
            public const string Delete = "delete"; 
        }

        public static class Resources
        {
            public static class MasterData
            {
                public const string OrganisationUnit = "organisationunit"; 

            }

            public static class ProfileUser
            {
                public const string BasicInformation = "basicinformation"; 
            }
        }
    }
}
