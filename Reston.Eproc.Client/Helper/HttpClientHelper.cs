using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Reston.EProc.Client;

namespace Reston.Eproc.Client.Helper
{
    public class HttpClientHelper
    {
        public static HttpClient GetPROCHttpClient(String token)
        {
            HttpClient client = new HttpClient();
            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(IdLdapConstants.Proc.Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            }

            return null;
        }
    }
}
