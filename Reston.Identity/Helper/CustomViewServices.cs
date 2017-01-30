using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.ViewModels;
using IdentityServer3.Core.Validation;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using Reston.Helper.Model;
using Reston.Helper;

namespace Reston.Identity.Helper
{
    public class CustomViewService : IViewService
    {
        IClientStore clientStore;
        public CustomViewService(IClientStore clientStore)
        {
            this.clientStore = clientStore;
        }
        public string GetCaptca()
        {
            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //HttpResponseMessage reply = await client.GetAsync(
            //       string.Format("{0}/{1}", IdLdapConstants.Proc.Url, "api/Registrasi/GetCaptcha"));
            //if (reply.IsSuccessStatusCode)
            //{
            //    string captca = await reply.Content.ReadAsStringAsync();
            //    var masterData = JsonConvert.DeserializeObject<string>(captca);

            //    return masterData;
            //}
            //else
            //{
            //    Byte[] bytes = new Byte[16];
            //    Guid guidEmpty = new Guid(bytes);
            //    return guidEmpty.ToString();
            //}

            CaptchaHelper captcha=new CaptchaHelper();
            try
            {
                CaptchaViewModel Generate = captcha.Generate();
                return Generate.Id.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            
        }

        public virtual async Task<System.IO.Stream> Login(LoginViewModel model, SignInMessage message)
        {
            var client = await clientStore.FindClientByIdAsync(message.ClientId);
            var name = client != null ? client.ClientName : null;
           // string[] split = model.LoginUrl.ToString().Split('?');
          //  model.LoginUrl = "../home/login?" + split[1];
            
            model.Username = GetCaptca();
            return await Render(model, "login", name);
        }

        public Task<Stream> Logout(LogoutViewModel model, SignOutMessage message)
        {
            return Render(model, "logout");
        }

        public Task<Stream> LoggedOut(LoggedOutViewModel model, SignOutMessage message)
        {
            model.RedirectUrl = IdLdapConstants.IDM.Url;
            return Render(model, "loggedOut");
            //return Task.FromResult(StringToStream(LoadHtml("logoutj")));
        //    var client = await clientStore.FindClientByIdAsync(IdLdapConstants.IDM.ClientId);
        //    var name = client != null ? client.ClientName : null;
        //    return await Render(new LoginViewModel(), "login", name);
        }

        public Task<Stream> Consent(ConsentViewModel model, ValidatedAuthorizeRequest authorizeRequest)
        {
            return Render(model, "consent");
        }

        public Task<Stream> ClientPermissions(ClientPermissionsViewModel model)
        {
            return Render(model, "permissions");
        }

        public virtual Task<System.IO.Stream> Error(ErrorViewModel model)
        {
            return Render(model, "error");
        }

        protected virtual Task<System.IO.Stream> Render(CommonViewModel model, string page, string clientName = null)
        {
            var request = HttpContext.Current.Request;
            var address = "";

            if (IdLdapConstants.Id.DnsSafe.ToLower().Equals("true"))
            {
                address = string.Format("{0}://{1}", request.Url.Scheme, request.Url.DnsSafeHost);
            }
            else
            {
                address = string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority);
            }


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });

            string html = LoadHtml(page);
            html = Replace(html, new
            {
                siteUrl = Microsoft.Security.Application.Encoder.HtmlEncode(address),
                siteName = Microsoft.Security.Application.Encoder.HtmlEncode(model.SiteName),
                model = Microsoft.Security.Application.Encoder.HtmlEncode(json),
                clientName = clientName,                
                procUrl=IdLdapConstants.Proc.Url
            });

            return Task.FromResult(StringToStream(html));
        }

        private string LoadHtml(string name)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"content\app");
            file = Path.Combine(file, name + ".html");
            return File.ReadAllText(file);
        }

        string Replace(string value, IDictionary<string, object> values)
        {
            foreach (var key in values.Keys)
            {
                var val = values[key];
                val = val ?? String.Empty;
                if (val != null)
                {
                    value = value.Replace("{" + key + "}", val.ToString());
                }
            }
            return value;
        }

        string Replace(string value, object values)
        {
            return Replace(value, Map(values));
        }

        IDictionary<string, object> Map(object values)
        {
            var dictionary = values as IDictionary<string, object>;

            if (dictionary == null)
            {
                dictionary = new Dictionary<string, object>();
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
                {
                    dictionary.Add(descriptor.Name, descriptor.GetValue(values));
                }
            }

            return dictionary;
        }

        Stream StringToStream(string s)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(s);
            sw.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}