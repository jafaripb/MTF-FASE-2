using Reston.Pinata.Model;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.Repository;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using System.Net;
using Microsoft.Owin.FileSystems;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web;
using System.IO;
using System;
using System.Security.Claims;
using Model.Helper;


namespace Reston.Pinata.WebService
{
	public class ProcurementController : BaseController
	{
        private IProdukRepo _repository;
        public ProcurementController()
        {
            _repository = new ProdukRepo(new JimbisContext());
        }

        public ProcurementController(ProdukRepo repository)
        {
            _repository = repository;
        }

        [System.Web.Http.Authorize]
        [HttpGet]
        public IHttpActionResult jimbis() {
            return Json("kamu mau apa");
        }

        [System.Web.Http.Authorize]
		public Produk Get(int id)
		{
            return _repository.GetProduk(id);
		}

        [Authorize]
		public IEnumerable<string>Get()
		{
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var zz = ((ClaimsIdentity)User.Identity).Claims.First().Value;
			return new [] { "Something exciting", "more of it", "more" };
		}

        [AllowAnonymous]
        [HttpPost]
        public async Task<string> Upload2()
        {
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            // extract file name and file contents
            var fileNameParam = provider.Contents[0].Headers.ContentDisposition.Parameters
                .FirstOrDefault(p => p.Name.ToLower() == "filename");
            string fileName = (fileNameParam == null) ? "" : fileNameParam.Value.Trim('"');
            byte[] file = await provider.Contents[0].ReadAsByteArrayAsync();

            Stream stream = await provider.Contents[0].ReadAsStreamAsync();
            
            string root = new PhysicalFileSystem(@"..\Reston.Pinata\WebService\mockup").Root;
            var sss=provider.Contents[0];
           


            // Here you can use EF with an entity with a byte[] property, or
            // an stored procedure with a varbinary parameter to insert the
            // data into the DB

            var result
                = string.Format("Received '{0}' with length: {1}", fileName, file.Length);
            return result;

        }
        


        [AllowAnonymous]
        public Task<HttpResponseMessage> Upload()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = new PhysicalFileSystem(@"..\Reston.Pinata\WebService\mockup").Root;

           // string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/uploads");

            var provider = new MultipartFormDataStreamProvider(root);

            var task = request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(o =>
                {
                
                    //string file1 = provider.FileData.First().Headers.ContentDisposition.FileName; //provider.BodyPartFileNames.First().Value;
                    // this is the file name on the server where the file was saved 
                    var fileContent = provider.Contents.SingleOrDefault();
                    
                    if (fileContent != null)
                    {
                        var fileName = fileContent.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    }
                    
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("File uploaded.")
                    };
                      
                }
            );
            return task;
        } 

	}

}

