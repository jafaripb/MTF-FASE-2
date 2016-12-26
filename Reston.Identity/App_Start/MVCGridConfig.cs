[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(IdLdap.MVCGridConfig), "RegisterGrids")]

namespace IdLdap
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using System.DirectoryServices.AccountManagement;
    using IdLdap.Configuration;
    using Reston.Identity.Helper;
    using IdLdap.Models;
    using Reston.Identity.Repository.Identity;

    public static class MVCGridConfig 
    {
        static readonly ILdapRepository _LdapRepository;
        static readonly UserManager _UserManager;
        static readonly RoleManager _RoleManager;

        static MVCGridConfig()
        {
            _LdapRepository = new LdapRepository(new PrincipalContext(
                                    ContextType.ApplicationDirectory,
                                    IdLdapConstants.LdapConfiguration.Host,
                                    IdLdapConstants.LdapConfiguration.ContextNaming,
                                    IdLdapConstants.LdapConfiguration.Username,
                                    IdLdapConstants.LdapConfiguration.Password));

            _UserManager = new UserManager(new UserStore(new IdentityContext()));
            _RoleManager = new RoleManager(new RoleStore(new IdentityContext()));
        }

        public static void RegisterGrids()
        {

            ColumnDefaults colDefauls = new ColumnDefaults()
            {
                EnableSorting = true
            };


            MVCGridDefinitionTable.Add("ListRole", new MVCGridBuilder<Role>(colDefauls)
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .AddColumns(cols =>
               {
                   cols.Add("Id").WithSorting(false)
                       .WithValueExpression(p => p.Id.ToString());
                   cols.Add("NameRole").WithSorting(false).WithHeaderText("NameRole")
                       .WithValueExpression(p => p.Name)
                       .WithFiltering(true);
                   cols.Add("RoleDescripton").WithSorting(false).WithHeaderText("RoleDescription")
                       .WithValueExpression(p => p.RoleDescription)
                       .WithFiltering(true);
                   cols.Add("Delete").WithHtmlEncoding(false)
                        .WithSorting(false)
                        .WithHeaderText("Action")
                        .WithValueExpression((p, c) => '"' + p.Name + '"')
                        .WithValueTemplate("<button onclick='DeleteRole({Value});' class='btn btn-primary' role='button'>Delete</button>");

               })
               .WithSorting(true, "Id")
               .WithPaging(true, 10, true, 100)
               .WithFiltering(true)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   var rolename = options.GetFilterString("NameRole") == null ? "" : options.GetFilterString("NameRole");
                   var result = (new RoleManager(new RoleStore(new IdentityContext()))).Roles.Where(x => x.Name.Contains(rolename));
                   var total = result.Count();
                   result.Skip(options.GetLimitOffset().GetValueOrDefault()).Take(options.GetLimitRowcount().GetValueOrDefault());

                   return new QueryResult<Role>()
                   {
                       Items = result,
                       TotalRecords = total
                   };
               })
           );


            MVCGridDefinitionTable.Add("UserLdapFiltering", new MVCGridBuilder<UserLdap>(colDefauls)
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .AddColumns(cols =>
               {
                   //cols.Add("Guid").WithSorting(false)
                   //    .WithValueExpression(p => p.Guid.ToString());
                   cols.Add("Name").WithSorting(false).WithHeaderText("Name")
                       .WithValueExpression(p => p.Name)
                       .WithFiltering(true);
                   cols.Add("SamAccountName").WithSorting(false).WithHeaderText("SAMAccountName")
                       .WithValueExpression(p => p.SamAccountName)
                       .WithFiltering(true);
                   cols.Add("UserPrincipalName").WithSorting(false).WithHeaderText("UserPrincipalName")
                       .WithValueExpression(p => p.UserPrincipalName)
                       .WithFiltering(true);
                   cols.Add("GivenName").WithSorting(false).WithHeaderText("GivenName")
                       .WithValueExpression(p => p.GivenName)
                       .WithFiltering(false);
                   cols.Add("Mail").WithSorting(false).WithHeaderText("Mail")
                       .WithValueExpression(p => p.Mail)
                       .WithFiltering(false);
                   cols.Add("IsAccountLockedOut").WithSorting(false)
                       .WithHeaderText("Status")
                       .WithValueExpression(p => p.IsAccountLockedOut ? "Inactive" : "Active")
                       .WithFiltering(false);
                   cols.Add("Linked").WithSorting(false).WithSortColumnData("Linked")
                       .WithHeaderText("Linked")
                       .WithValueExpression(p => p.IsLinked.GetValueOrDefault() ? "Linked" : "Not Linked")
                       .WithFiltering(false);
                   cols.Add("Edit").WithHtmlEncoding(false)
                        .WithSorting(false)
                        .WithHeaderText("Action")
                        .WithValueExpression((p, c) => '"'+p.UserPrincipalName+'"'  )
                        .WithCellCssClassExpression(p => p.IsLinked.GetValueOrDefault() ? "hiddentd" : "")
                        .WithValueTemplate("<button onclick='LinkedAccount({Value});' class='btn btn-primary' role='button'>Link</button>");
                    
               })
               .WithSorting(true, "SamAccountName")
               .WithPaging(true, 10, true, 100)
               .WithFiltering(true)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   List<UserLdap> items;
                   
                       var username = options.GetFilterString("UserPrincipalName") == null ? "*" : "*"+options.GetFilterString("UserPrincipalName")+"*";
                       var result = _LdapRepository.GetUsersByUsername(username, options.PageIndex.GetValueOrDefault(), options.ItemsPerPage.GetValueOrDefault());
                       
                       items = result.Users.Select(x => new UserLdap
                       {
                           Name = x.Name,
                           Guid = x.Guid.ToString(),
                           Mail = x.EmailAddress,
                           SamAccountName = x.SamAccountName,
                           UserPrincipalName = x.UserPrincipalName,
                           GivenName = x.GivenName,
                           Surname = x.Surname,
                           IsAccountLockedOut = x.IsAccountLockedOut(),
                           IsLinked = false,
                           DisplayName = x.DisplayName
                       }).ToList();
                       
                       List<string> guidSearch = items.Select(x=>x.Guid).ToList();
                       var usersIdentity = (new UserManager(new UserStore(new IdentityContext()))).Users.Where(x => guidSearch.Contains(x.Id)).Select(x => x.Id).ToList();
                       //items.Where(c => usersIdentity.Contains(c.Guid)).ToList().ForEach(x =>  { x.IsLinked = true; } );

                       items.Update(x => x.IsLinked = usersIdentity.Contains(x.Guid) ? true : false);
                        
                       return new QueryResult<UserLdap>()
                       {
                           Items = items,
                           //TotalRecords = items.Count()
                           TotalRecords = result.Length
                       };
               })
           );


            MVCGridDefinitionTable.Add("UserId", new MVCGridBuilder<User>(colDefauls)
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .AddColumns(cols =>
               {
                   //cols.Add("Guid").WithSorting(false)
                   //    .WithValueExpression(p => p.Id.ToString());
                   cols.Add("UserName").WithSorting(false).WithHeaderText("Username")
                       .WithValueExpression(p => p.UserName)
                       .WithFiltering(true);
                   cols.Add("DisplayName").WithSorting(false).WithHeaderText("DisplayName")
                       .WithValueExpression(p => p.DisplayName)
                       .WithFiltering(true);
                   cols.Add("Email").WithSorting(false).WithHeaderText("Email")
                       .WithValueExpression(p => p.Email)
                       .WithFiltering(true);
                   cols.Add("Position").WithSorting(false).WithHeaderText("Position")
                       .WithValueExpression(p => p.Position)
                       .WithFiltering(true);
                   cols.Add("IsLdapUser").WithSorting(false).WithHeaderText("User Ldap")
                       .WithValueExpression(p => p.IsLdapUser ? "User Ldap": "Non Ldap")
                       .WithFiltering(false);
                   cols.Add("LockoutEnabled").WithSorting(false)
                       .WithHeaderText("Status")
                       .WithValueExpression(p => p.LockoutEnabled ? "Inactive" : "Active")
                       .WithFiltering(false);
                   cols.Add("Edit").WithHtmlEncoding(false)
                        .WithSorting(false)
                        .WithHeaderText("Action")
                        .WithValueExpression((p, c) => c.UrlHelper.Action("DetailUser", "Admin", new { id = p.UserName }))
                        .WithValueTemplate("<a href='{Value}' class='btn btn-primary' role='button'>View</a>");
                   cols.Add("Delete").WithHtmlEncoding(false)
                        .WithSorting(false)
                        .WithHeaderText("Action")
                        .WithValueExpression((p, c) => '"' + p.UserName + '"')
                        .WithValueTemplate("<button onclick='DeleteAccount({Value});' class='btn btn-primary' role='button'>Delete</button>");

               })
               .WithSorting(true, "SamAccountName")
               .WithPaging(true, 10, true, 100)
               .WithFiltering(true)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;

                   var username = options.GetFilterString("UserName") == null ? "" : options.GetFilterString("UserName");

                   var result = (new UserManager(new UserStore(new IdentityContext()))).Users.Where(x => x.UserName.Contains(username)||x.DisplayName.Contains(username));
                   var total = result.Count();    
                   //result.Skip(options.GetLimitOffset().GetValueOrDefault()).Take(options.GetLimitRowcount().GetValueOrDefault());
                   int skip = options.PageIndex.GetValueOrDefault() * options.ItemsPerPage.GetValueOrDefault();
                   int take = options.ItemsPerPage.GetValueOrDefault();
                   result = result.OrderBy(x=>x.Id).Skip(skip).Take(take); 

                   return new QueryResult<User>()
                   {
                       Items = result,
                       TotalRecords = total
                   };
               })
           );


            

        }

        public static void Update<TSource>(this IList<TSource> outer, Action<TSource> updator)
        {
            foreach (var item in outer)
            {
                updator(item);
            }
        }

        
    }
}