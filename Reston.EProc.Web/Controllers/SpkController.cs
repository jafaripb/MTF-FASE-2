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


namespace Reston.Pinata.WebService.Controllers
{
    public class SpkController : BaseController
    {
        private ISpkRepo _repository;
        private IWorkflowRepository _workflowrepo;
        private IPengadaanRepo _repoPengadaan;
        private string DocumentType = "SPK";
        private string FILE_DOKUMEN_SPK_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_DOKUMEN_SPK_PATH"];
        
        public SpkController()
        {
            _repository = new SpkRepo(new JimbisContext());
            _repoPengadaan = new PengadaanRepo(new JimbisContext());
            _workflowrepo = new WorkflowRepository(new HelperContext());
        }

        public SpkController(SpkRepo repository)
        {
            _repository = repository;
        }
       
      [HttpPost]
      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public IHttpActionResult List()
      {
          try
          {
              int start = Convert.ToInt32(System.Web.HttpContext.Current.Request["start"].ToString());
              string search = System.Web.HttpContext.Current.Request["search"].ToString();
              int length = Convert.ToInt32(System.Web.HttpContext.Current.Request["length"].ToString());
              string klasifikasi = System.Web.HttpContext.Current.Request["klasifikasi"].ToString();
              var data = _repository.List(search, start, length, klasifikasi);
              return Json(data);
          }
          catch (Exception ex)
          {
              return Json(new DataTablePksTemplate());
          }
      }

      [HttpPost]
      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public IHttpActionResult ListPks()
      {
          try
          {
              int start = Convert.ToInt32(System.Web.HttpContext.Current.Request["start"].ToString());
              string search = System.Web.HttpContext.Current.Request["search"].ToString();
              int length = Convert.ToInt32(System.Web.HttpContext.Current.Request["length"].ToString());
              string klasifikasi = System.Web.HttpContext.Current.Request["klasifikasi"].ToString();
              var data = _repository.ListPks(search, start, length, klasifikasi);
              return Json(data);
          }
          catch (Exception ex)
          {
              return Json(new DataTableSpkTemplate());
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
              var oPks = _repository.detail(Id, UserId());
              return Json(oPks);
          }
          catch (Exception ex)
          {
              return Json(new VWPks());
          }
      }


      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public IHttpActionResult Save(VWSpk vspk)
      {
          try
          {
              var spk = new Spk();
              spk.PksId = vspk.PksId;
              spk.Id = vspk.Id;
              spk.NilaiSPK = vspk.NilaiSPK;
              if (!string.IsNullOrEmpty(vspk.TanggalSPKStr)) spk.TanggalSPK = Common.ConvertDate(vspk.TanggalSPKStr, "dd/MM/yyyy HH:mm");
              return Json(_repository.save(spk, UserId()));
          }
          catch (Exception ex)
          {
              return Json(new VWSpk());
          }
      }

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public async Task<IHttpActionResult> UploadFile( Guid id)
      {
          var oSpk = _repository.get(id);

          if (oSpk.StatusSpk != StatusSpk.Draft)
              return Json("00000000-0000-0000-0000-000000000000");

          var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
          bool isSavedSuccessfully = true;
          string filePathSave = FILE_DOKUMEN_SPK_PATH;//+id ;
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
                  DokumenSpk dokumen = new DokumenSpk
                  {
                      File = fileName,
                      ContentType = contentType,
                      SpkId = id,
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
      public List<VWDokumenSPK> getDokumens( Guid Id)
      {
          return _repository.GetListDokumenSpk(Id);
      }

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                         IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                          IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public ResultMessage deleteDokumenPks(Guid Id)
      {
          try
          {
              int Approver = 0;
              var oSpk = _repository.getDokSpk(Id);
              var result = _repository.deleteDokumenSpk(Id, UserId(), Approver);
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
                      message = "error",
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
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public IHttpActionResult ChangeSatus(Guid Id,StatusSpk status)
      {
          _repository.ChangeStatus(Id, status, UserId());
          _repository.AddRiwayatDokumenSpk(new RiwayatDokumenSpk()
          {
              ActionDate=DateTime.Now,
              Status="Berubah Status: "+status.ToString()
          }, UserId());
          return Json(_repository.ChangeStatus(Id, status, UserId()));
            
      }
     
      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                       IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                        IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public ResultMessage delete(Guid Id)
      {
          return _repository.deleteSpk(Id, UserId());
      }

      

      
    }
    
}
