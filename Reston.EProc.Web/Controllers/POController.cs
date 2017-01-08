using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Model.Helper;
using Newtonsoft.Json;
using Reston.Pinata.Model;
//using Reston.Pinata.Model.Helper;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.PengadaanRepository;
using Reston.Pinata.Model.PengadaanRepository.View;
using Reston.Pinata.WebService.Helper;
using Reston.Pinata.WebService.ViewModels;
using Webservice.Helper.Util;
using Reston.Helper.Repository;
using Reston.Helper;
using Reston.Helper.Util;
using Reston.Helper.Model;
using System.Web;
using Reston.Eproc.Model.Monitoring.Entities;
using Microsoft.Reporting.WebForms;


namespace Reston.Pinata.WebService.Controllers
{
    public class POController : BaseController
    {
        private IPORepo _repository;
        private string FILE_DOKUMEN_PO_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_DOKUMEN_PO_PATH"];
        private string FILE_REPORT_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_REPORT_PATH"];
        
        public POController()
        {
            _repository = new PORepo(new JimbisContext());
        }

        public POController(PORepo repository)
        {
            _repository = repository;
        }
       
      [HttpPost]
      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public async Task< IHttpActionResult> List()
      {
          try
          {
              int start = Convert.ToInt32(System.Web.HttpContext.Current.Request["start"].ToString());
              string search = System.Web.HttpContext.Current.Request["search"].ToString();
              int length = Convert.ToInt32(System.Web.HttpContext.Current.Request["length"].ToString());
              string NoPO = System.Web.HttpContext.Current.Request["NoPO"].ToString();
              var data = _repository.List(search, start, length, NoPO);

              foreach (var item in data.data)
              {
                  Userx userdetail =await userDetail(item.CreatedId.ToString());
                  item.Created = userdetail.Nama;
              }
              return Json(data);
          }
          catch (Exception ex)
          {
              return Json(new DataTablePO());
          }
      }

      [HttpPost]
      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public IHttpActionResult ListItem()
      {
          try
          {
              int start = Convert.ToInt32(System.Web.HttpContext.Current.Request["start"].ToString());
              string search = System.Web.HttpContext.Current.Request["search"].ToString();
              int length = Convert.ToInt32(System.Web.HttpContext.Current.Request["length"].ToString());
              Guid PoId = new Guid();
              try
              {
                  PoId = new Guid(System.Web.HttpContext.Current.Request["PoId"].ToString());
              }
              catch { }
              var data = _repository.ListItem(search, start, length, PoId);
              return Json(data);
          }
          catch (Exception ex)
          {
              return Json(new DataTablePODetail() { data=new List<VWPODetail>()});
          }
      }


      [HttpPost]
      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public IHttpActionResult Detail(Guid Id)
      {
          try
          {
              return Json(_repository.detail(Id, UserId()));
          }
          catch (Exception ex)
          {
              return Json(new VWPO());
          }
      }


      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public IHttpActionResult Save(VWPO data)
      {
          try
          {
              var ndata = new PO();
              ndata.Id = data.Id;
              ndata.Keterangan = data.Keterangan;
              ndata.Prihal = data.Prihal;
              ndata.UP = data.UP;
              ndata.Vendor = data.Vendor;

              if (!string.IsNullOrEmpty(data.TanggalPOstr) )
              {
                  try
                  {
                      ndata.TanggalPO = Common.ConvertDate(data.TanggalPOstr, "dd/MM/yyyy");
                  }
                  catch { }
                 
              }
              return Json(_repository.save(ndata, UserId()));
          }
          catch (Exception ex)
          {
              return Json(new VWPO());
          }
      }

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public IHttpActionResult SaveItem(PODetail data)
      {
          try
          {
              return Json(_repository.saveItem(data, UserId()));
          }
          catch (Exception ex)
          {
              return Json(new VWPO());
          }
      }

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public async Task<IHttpActionResult> UploadFile( Guid id)
      {
          var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
          bool isSavedSuccessfully = true;
          string filePathSave = FILE_DOKUMEN_PO_PATH;//+id ;
          string fileName = "";

          var s = await Request.Content.ReadAsStreamAsync();
          var provider = new MultipartMemoryStreamProvider();

          await Request.Content.ReadAsMultipartAsync(provider);
          string contentType = "";
          Guid newGuid = Guid.NewGuid();
          long sizeFile = 0;
          foreach (var file in provider.Contents)
          {
              string filename = file.Headers.ContentDisposition.FileName.Trim('\"');
              string extension = filename.Substring(filename.IndexOf(".") + 1, filename.Length - filename.IndexOf(".") - 1);
              byte[] buffer = await file.ReadAsByteArrayAsync();
              contentType = file.Headers.ContentType.ToString();
              sizeFile = buffer.Length;
              filePathSave +=  newGuid.ToString() + "." + extension;
              fileName += newGuid.ToString() + "." + extension;
              // var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase; //new PhysicalFileSystem(@"..\Reston.Pinata\WebService\Upload\Vendor\Dokumen\");

              try
              {
                  FileStream fs = new FileStream(uploadPath.ToString() + filePathSave, FileMode.CreateNew);
                  await fs.WriteAsync(buffer, 0, buffer.Length);

                  fs.Close();

                  isSavedSuccessfully = true;
              }
              catch (Exception e)
              {
                  return InternalServerError();
              }
          }
          Guid DokumenId = Guid.NewGuid();
            
            
          if (isSavedSuccessfully)
          {
              try
              {
                  DokumenPO dokumen = new DokumenPO
                  {
                      File = fileName,
                      ContentType = contentType,
                      POId = id,
                      SizeFile = sizeFile
                  };
                  return Json( _repository.saveDokumen(dokumen, UserId()).Id);
              }
              catch (Exception ex)
              {
                  return Json("00000000-0000-0000-0000-000000000000");
              }
          }
          return Json("00000000-0000-0000-0000-000000000000");
      }

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public List<VWDokumenPO> getDokumens( Guid Id)
      {
          return _repository.GetListDokumenPO(Id);
      }

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                         IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                          IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public ResultMessage deleteDokumenPO(Guid Id)
      {
          try
          {
              var result = _repository.deleteDokumenPO(Id, UserId());
              if (result == 1)
              {
                  return new ResultMessage()
                  {
                      status = HttpStatusCode.OK,
                      message =Common.DeleteSukses(),
                      Id = "1"
                  };
              }
              else
              {
                  return new ResultMessage()
                  {
                      status = HttpStatusCode.NotImplemented,
                      message = HttpStatusCode.NotImplemented.ToString(),
                      Id = "0"
                  };
              }
          }
          catch(Exception ex)
          {
              return new ResultMessage()
              {
                  status = HttpStatusCode.NotImplemented,
                  message =ex.ToString() ,
                  Id = "0"
              };
          }
      }
      
     
      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                       IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                        IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public ResultMessage delete(Guid Id)
      {
          return  _repository.Delete(Id, UserId());
      }

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                       IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                        IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public ResultMessage deleteItem(Guid Id)
      {
          return _repository.DeleteItem(Id, UserId());
      }

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                       IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                        IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public ResultMessage GenerateNoPO(Guid Id)
      {

          var noPO = _repository.GenerateNoPO(UserId());
          var data = _repository.get(Id);
          if(string.IsNullOrEmpty( data.NoPO))
             data.NoPO = noPO;
          return _repository.save(data,UserId());
      }

       [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                       IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                        IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      public HttpResponseMessage OpenFile(Guid Id)
      {
          var data = _repository.GetDokumenPO(Id);
          var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + FILE_DOKUMEN_PO_PATH + data.File;
          HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
          var stream = new FileStream(path, FileMode.Open);
          result.Content = new StreamContent(stream);
          //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
          result.Content.Headers.ContentType = new MediaTypeHeaderValue(data.ContentType);

          result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
          {
              FileName = data.File
          };

          return result;
      }

       public HttpResponseMessage Report(Guid Id)
       {
           LocalReport lr = new LocalReport();
           string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + FILE_REPORT_PATH;

            path = Path.Combine(path, "po.rdlc");
         

           if (System.IO.File.Exists(path))
           {
               lr.ReportPath = path;
           }

           var po = _repository.get(Id);
           
           VWPOReport data1=new VWPOReport(){
                Id=po.Id,
                Prihal=po.Prihal,
                Vendor=po.Vendor,
                UP=po.UP,
                NoPO=po.NoPO,
                TanggalPO=po.TanggalPO!=null?po.TanggalPO.Value.Day+" "+ Common.ConvertNamaBulan( po.TanggalPO.Value.Month) + " "+ po.TanggalPO.Value.Year:"",
                TanggalPOstr=po.TanggalPO!=null?po.TanggalPO.Value.Day+" "+ Common.ConvertNamaBulan( po.TanggalPO.Value.Month) + " "+ po.TanggalPO.Value.Year:"",
                NilaiPO=po.PODetail==null?"":po.PODetail.Sum(d=>d.Harga*d.Banyak).Value.ToString("C", MyConverter.formatCurrencyIndo()),
                Keterangan=po.Keterangan,
                AlmatBarangUp="",
                Rekening="",
                AtasNama="",
                Bank="",
                TelpBarang="",
                KwitansiUp="",
                Total="",
                TTD1="",
                TTD2="",
                TTD3="",
                TTD4=""
           };
           List<VWPOReport> lstdata1 = new List<VWPOReport>();
           lstdata1.Add(data1);
           List< VWPODetailReport> lstdata2 = new List< VWPODetailReport>();
           if (po.PODetail != null)
           {
               lstdata2 = po.PODetail.Select(d => new VWPODetailReport()
               {
                   Id=d.Id,
                   Banyak = d.Banyak.ToString() ,
                   Deskripsi=d.Deskripsi,
                   Harga = d.Banyak == null ? "" : d.Harga.Value.ToString("C", MyConverter.formatCurrencyIndo()),
                   Jumlah=d.Banyak==null?"":(d.Harga.Value*d.Banyak.Value).ToString("C", MyConverter.formatCurrencyIndo()),
                   Kode=d.Kode,
                   NamaBarang=d.NamaBarang,
                   Satuan=d.Satuan
               }).ToList();
           }

           ReportDataSource rd = new ReportDataSource("PoDs", lstdata1);
           lr.DataSources.Add(rd);
           ReportDataSource rd2 = new ReportDataSource("PoDetailDs", lstdata2);
           lr.DataSources.Add(rd2);

           string reportType = "pdf";
           string mimeType;
           string encoding;
           string fileNameExtension;



           string deviceInfo =

          "<DeviceInfo>" +
         "  <OutputFormat>PDF</OutputFormat>" +
         "  <PageWidth>14.267in</PageWidth>" +
         "  <PageHeight>18.692in</PageHeight>" +
         "  <MarginTop>0.25in</MarginTop>" +
         "  <MarginLeft>0.10in</MarginLeft>" +
         "  <MarginRight>0.10in</MarginRight>" +
         "  <MarginBottom>0.25in</MarginBottom>" +
         "</DeviceInfo>";

           Warning[] warnings;
           string[] streams;
           byte[] renderedBytes;

           renderedBytes = lr.Render(
               reportType,
               deviceInfo,
               out mimeType,
               out encoding,
               out fileNameExtension,
               out streams,
               out warnings);
           HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
           Stream stream = new MemoryStream(renderedBytes);

           result.Content = new StreamContent(stream);
           result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

           result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
           {
               FileName = po.Prihal+".pdf"
           };


           return result;
       }
       
    }
    
}
