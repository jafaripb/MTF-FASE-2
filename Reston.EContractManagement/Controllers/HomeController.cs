using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DotNetXmlHttpRequest;
using Reston.Eproc.Client.Helper;
using Reston.EProc.Client;


namespace Reston.EContractManagement.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
       

        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<ActionResult> test()
        {
            var tokenRespones = ClientTokenManagement.GetToken();

            var clientHttp = HttpClientHelper.GetPROCHttpClient(tokenRespones);
            HttpResponseMessage dataResponse = await clientHttp.GetAsync("api/Header/GetMenu");

            var ss= dataResponse;
           var ssxx = Request.Cookies[".AspNet.Cookies"];
            var xhr = new XMLHttpRequest();
            xhr.Open(XMLHttpRequest.EnumMethod.Get, "http://localhost:49559/api/Header/GetMenu");

            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create("http://localhost:49559/api/Header/GetMenu");
            rq.Accept = "Accept=application/json, text/plain, */*";
            rq.Host = "http://localhost:49559";
           //"OpenIdConnect.nonce.KfZvPNHK0wmwyzPf82SPoxpTkcC%2BjoXhITR%2Bwjdf77o%3D=bThweVU1REV4eHlhQzdObS1BNDdjRTg0RGJwWjFKRFpZNHBlYk5HZFQweEFMc3VaU3U1QlczSGwwU2ZReUNTU1JCSkdtd2diNjZPY1cwU0JGd2x6TnlKdmhRMzlSalhSWWFydk9lT1NBWElpUTZJV3ZNeUY1UFczc0RoNUF3czBZU0xlSnNzQXJTaUdNYWdLdWtvZUdkWGR6RDVMWkttQ3d5aklNZFVwbTRueW11TTlseTlSWjZjQ0x5VHkyUkxnR1J2S2JLQ0VkYjRsVE5lWS1kQ3p5cGk0U2R2TzBUMUtMdWxiSFlnN19fbw%3D%3D; OpenIdConnect.nonce.sBsYuphIZKDc2tI06A6AocYENMI4f%2BiNXsqE%2F88iPO0%3D=N2kxQUFIZXIwSU5QNXVSMkRSdWc2amJFQ1ZDNm54ZXg1dUJRc3hBbE9NdEhTWTRKQ0RuVklyTWViNUk3bHhBeEM3TWQ0QXZOcGlPTEVRS3ZwZmg2NDg1aEdJVGpmM0kycklQdVc5S05kd252Ml9MOFhFOTk0cDE2ekpsN0pJZ01yajRwdzNqU2h3X1ItUndmVFFGWDhBN3piN3hTSmNPZGFBdlI4Z2s4NDIwTXMyeUFISFZaV2h3TjZGRHlGdkJSeWE1elhnU1NrLVRRMEJPbWRpQzBsSVlWOWRBZ2FVRTNMMmVBU1Z0cGZiZw%3D%3D; .AspNet.Cookies=odcBptmRSGrFNNeSeMlJsd0bWMQ4pIpqWE1_FFoxJvg3ivKxez1icXPvuxkgiV9KuThZjTK4POHiy7JUQsfsWAvOdkpXsD6N5IW3NRMCBroMIAaIw0BQTWNu_JikYLb3kJ3sTffK8limIoQ9Jsfcf5E8C4lZqB-Pz2K0Ygq6n7QERKNq23ThuYfj2rdvadDtE3M_5RUTPlP4LBqN32BF_1PcbiUQzTOBx-wkziBSfNx2qore0Ynujt1qsG0jm_VmdZRPrvDR5OAXbbasqS-b4kVRqpesu9HLiNcromHzhqhv_c8Z-u-F3kFYZq7DqSsi1Ze37SYf6I7r2AVgQGs3YBh9_iDkR9HnU5Q6ZMlpwp0GKpV2EiZU3CjiII_F5XA1l-Y_qeOtufRNe2PL1H9aNTYAs9_BURUS_S1qTHUxPvbHCL-RzSIZ5vo5yGMMMksxiPNR9prTw966FD_DbJX4Qn_JCcTMhqq1ys498tqbA9rqRnEX841d77hf_ImAdpLc4KRj_zTkrCYN17of6n-mkNxH4jUSgidIUDXEnH2hmWow45KC8WiGV1lhixMPOcba-6rPeLSMr0zXcdmcCfP2Kdpnyj43PJ0bIJwKQpUPYGtyjw-hQe921175IQpz8KDf-ZeJL9_xhF1zRYLOjnPk0UmPYpAhvqQHy2ZHEKiG7Yl-Zf6yIWta2DzK2VsmEDUEGZawpW7KsEY20rYJKpb2mxgUNHLDB62j5wbHEViYlx7HjHN1OrtC4xxh6jmcS4c98yeBwSGyUu2LOI4X_F9y1cm87nv6xA3GeeXXDKeEceeHZI9Q7-Ei_U_vYkaQZ1GOiFes9E7zOvUiKDhuIc-srCBIfyd5efsDwWE7vr4-SueUL6KNulLRBmLMXozKyjcNnd4mXnSD3MpXcceMEuGN8ZK9ih75MJanimWuyhMmYYV5GCAY-si5FNoRYznxXSgwyE5CPKLCF0sGqgqsQRlX24Bax9jsOmQFqZ9SYuWXFvCSbwwN_0QuwKwNXnwBhqGaLlpbdmGdux_p2WIL1n5C_N1V3s1gBUoRYWyk_9d0UAFmtqCift9d7mcxY6wZpJDfWVs0e9TFgwRibKt7NPp0rFmrbzWp7Hu2dF1yixwnPlc3X1moYJQiE7L8YbhAMxJluNhkMQFDCIkaHXI2qD0x1Nwurx5uHaAqgvQWvs3GHYuTbX2wOh3tpkOYMEc593FfopitM1_KpCSO_QPL-7qniFaD_p8pAM5eeSUpxUXHh-YRdDB1xKSBy7XZokwx7Cs29PHJWxQw3j8m7t1-EK7d4eXBoLkzOhJmKqoXdBimpZWh8FisIPKAveNtL8YR4swTbBF0gC_67HWzW1MOwOAht7Gl-dBnOTR5QPrMwK4OIXNS61lNI37-gSgZSURBQoG3n09TZE4cC_yqHUXsubMeBBJMLW9ZFDKgptNxR5oY5VnQHGvE6iOjA0E2IWBD02C2Svpzgqb3cTEOzBctunkKjipaaMpU1jEGWo5DAAhfMMHygev8DbnBMKdvMxDH6c971EXh54lJSRa-gBoFqE8UGxlqEYONdmgvWcvj-K6dnVwKvKT4sNS2565Zyv5JKxyhKVsJKs8ae96x6m7-LCAChF1XDLq76zh3lpPVL0NasZ6E9Eg9zSHvoZMy2FfE0hCqmdmcTTAWvJFZl5wbzII-z07QIlNF0fkRc4fL5hwLaYFgxozBImu8rby0_2zEAnhzSICyVtd6-MUZUo88waFW5EixvGhZpdWUCcK5TjTYimgEeuYO3s-va9evHlWb-JcvQyROfSkstYnheQwZBqmwmpL1xuNzmZYl1CXCjDASjRk";

            HttpWebResponse resp = (HttpWebResponse)rq.GetResponse();
            using (Stream s = resp.GetResponseStream())
            {
                // Read the result here
            }
            
            return View();
        }
	}
}