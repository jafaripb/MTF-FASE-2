using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Model.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Reston.Pinata.Model.PengadaanRepository.View;
using Reston.Pinata.WebService.Helper;
using Webservice.Helper.Util;

namespace Reston.Pinata.WebService
{
	public class BaseController : ApiController
	{
       
        // GET: Base
        public BaseController()
        {
            // Thread.CurrentThread.CurrentCulture = new CultureInfo("en-AU"); 
        }

        public AppUser CurrentUser
        {
            get
            {
                return new AppUser(this.User as ClaimsPrincipal);
            }
        }

        public bool UserInRole(string role) {
            if (!this.User.Identity.IsAuthenticated) return false;
            return CurrentUser.Roles.Contains(role);
        }

        public string AksesToken()
        {
            return CurrentUser.AccessToken;
        }

        public bool UserInRole(string[] role)
        {
            if (!this.User.Identity.IsAuthenticated) return false;
            foreach (string r in role) {
                if (CurrentUser.Roles.Contains(r))
                    return true;
            }
            return false;
        }

        public List<string> Roles()
        {
            return CurrentUser.Roles;
        }

        public Guid UserId()
        {
            return new Guid(CurrentUser.Subject);
        }

        public async Task<List<Userx>> getManager()
        {
            var client = new HttpClient();
           // var tokenRespones = await Reston.Identity.Client.Api.ClientTokenManagement.GetIdEPROCAPITokenAsync();

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpTokenManagement.GetToken());

            //original base address using appmgt instead
            //client.BaseAddress = new Uri("http://localhost:53080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage reply = await client.GetAsync(
                    string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/GetManager"));
            if (reply.IsSuccessStatusCode)
            {
                string masterDataContent = await reply.Content.ReadAsStringAsync();
                var masterData = JsonConvert.DeserializeObject<List<Userx>>(masterDataContent);
                return masterData;
            }
            return new List<Userx>();
        }

        public async Task<List<Guid>> listGuidManager()
        {
            var oData = await getManager();
            List<Guid> oGuid=new List<Guid>();
            foreach (var item in oData)
            {
                oGuid.Add(new Guid(item.PersonilId));
            }
            return oGuid;
        }

        public async Task<List<Guid>> listHead()
        {
            var client = new HttpClient();
            string filter = IdLdapConstants.App.Roles.IdLdapProcurementHeadRole;
           // var tokenRespones = await Reston.Identity.Client.Api.ClientTokenManagement.GetIdEPROCAPITokenAsync();

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer ",  AksesToken());

            //original base address using appmgt instead
            //client.BaseAddress = new Uri("http://localhost:53080/");
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AksesToken());
            //client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage reply = await client.GetAsync(
                   string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/ListUser?start=" + 0 + "&limit=" + 10 + "&filter=" + filter));

            if (reply.IsSuccessStatusCode)
            {
                string masterDataContent = await reply.Content.ReadAsStringAsync();
                var masterData = JsonConvert.DeserializeObject<DataPageUsers>(masterDataContent);
                var oData = await getManager();
                List<Guid> oGuid = new List<Guid>();
                foreach (var item in masterData.Users)
                {
                    oGuid.Add(new Guid(item.PersonilId));
                }
                return oGuid;
            }
            else
            {
                return new List<Guid>();
            }            
        }

        public async Task<List<Guid>> listUser(string role)
        {
            var client = new HttpClient();
            string filter = role;
            // var tokenRespones = await Reston.Identity.Client.Api.ClientTokenManagement.GetIdEPROCAPITokenAsync();

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer ",  AksesToken());

            //original base address using appmgt instead
            //client.BaseAddress = new Uri("http://localhost:53080/");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AksesToken());
            //client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage reply = await client.GetAsync(
                   string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/ListUser?start=" + 0 + "&limit=" + 10 + "&filter=" + filter));

            if (reply.IsSuccessStatusCode)
            {
                string masterDataContent = await reply.Content.ReadAsStringAsync();
                var masterData = JsonConvert.DeserializeObject<DataPageUsers>(masterDataContent);
                var oData = await getManager();
                List<Guid> oGuid = new List<Guid>();
                foreach (var item in masterData.Users)
                {
                    oGuid.Add(new Guid(item.PersonilId));
                }
                return oGuid;
            }
            else
            {
                return new List<Guid>();
            }
        }

        public async Task<int> isApprover()
        {
            var oUser =await getManager();
            foreach (var item in oUser)
            {
                if (new Guid(item.PersonilId) == UserId())
                {
                    return 1;
                }
            }
            return 0;
        }

        public async Task<Userx> userDetail(string Id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage reply = await client.GetAsync(
                   string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/GetUser?Id=" + Id));

            if (reply.IsSuccessStatusCode)
            {
                string masterDataContent = await reply.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Userx>(masterDataContent);                 
            }
            else
            {
                return new Userx();
            }
        }

    }
}