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
    public class PksController : BaseController
    {
        private IPksRepo _repository;
        private IWorkflowRepository _workflowrepo;
        private IPengadaanRepo _repoPengadaan;
        private string DocumentType = "PKS";
        private string FILE_DOKUMEN_PKS_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_DOKUMEN_PKS_PATH"];
        
        public PksController()
        {
            _repository = new PksRepo(new JimbisContext());
            _repoPengadaan = new PengadaanRepo(new JimbisContext());
            _workflowrepo = new WorkflowRepository(new HelperContext());
        }

        public PksController(PksRepo repository)
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
                string klasifikasi = System.Web.HttpContext.Current.Request["klasifikasi"].ToString();
                var data = _repository.List(search, start, length, klasifikasi);

                foreach(var item in data.data)
                {
                    if (item.WorkflowId != null)
                    {
                        List<Reston.Helper.Model.ViewWorkflowModel> getDoc = _workflowrepo.ListDocumentWorkflow(UserId(), item.WorkflowId.Value, Reston.Helper.Model.DocumentStatus.PENGAJUAN, DocumentType, 0, 0);
                        if (getDoc.Where(d => d.CurrentUserId == UserId()).FirstOrDefault() != null) item.Approver = 1;
                        //item.lastApprover = _workflowrepo.isLastApprover(item.Id, item.WorkflowTemplateId.Value).Id;
                    }
                    var user = await userDetail(item.CreateBy.ToString());
                    if(user!=null)
                        item.CreatedName = user.Nama;
                }
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
        public IHttpActionResult ListPengadaan()
        {
            try
            {
                int start = Convert.ToInt32(System.Web.HttpContext.Current.Request["start"].ToString());
                string search = System.Web.HttpContext.Current.Request["search"].ToString();
                int length = Convert.ToInt32(System.Web.HttpContext.Current.Request["length"].ToString());
                string klasifikasi = System.Web.HttpContext.Current.Request["klasifikasi"].ToString();
                var data = _repository.ListPengadaan(search, start, length, klasifikasi);
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
        public IHttpActionResult Detail(Guid Id)
        {
            try
            {
                var oPks = _repository.detail(Id, UserId());
                if (oPks.WorkflowId != null)
                {
                    oPks.Approver= _workflowrepo.isThisUserLastApprover(oPks.WorkflowId.Value, UserId());
                    //item.lastApprover = _workflowrepo.isLastApprover(item.Id, item.WorkflowTemplateId.Value).Id;
                }
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
        public async Task<IHttpActionResult> ajukan(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                var pks = _repository.get(Id);
                var Legal = await listUser(IdLdapConstants.App.Roles.IdLdaplegal_admin);

                #region BuatAtauUpdateTamplate

                var WorkflowMasterTemplateDetails = new List<WorkflowMasterTemplateDetail>(){
                                //new WorkflowMasterTemplateDetail()
                                //    {
                                //        NameValue="Gen.By.System",
                                //        SegOrder=1,
                                //        UserId=pks.PemenangPengadaan.Pengadaan.PersonilPengadaans.Where(d=>d.tipe=="staff").FirstOrDefault().PersonilId
                                //    },
                                 new WorkflowMasterTemplateDetail()
                                    {
                                        NameValue="Gen.By.System",
                                        SegOrder=1,
                                        UserId=Legal[0]
                                    }
                            };
                WorkflowMasterTemplate MasterTemplate = new WorkflowMasterTemplate()
                {
                    ApprovalType = ApprovalType.BERTINGKAT,
                    CreateBy = UserId(),
                    CreateOn = DateTime.Now,
                    DescValue = "WorkFlow PKS Pengadaan=> " + pks.PemenangPengadaan.Pengadaan.Judul,
                    NameValue = "Generate By System ",
                    WorkflowMasterTemplateDetails = WorkflowMasterTemplateDetails
                };
                var resultTemplate = _workflowrepo.SaveWorkFlow(MasterTemplate, UserId());
                pks.WorkflowId = Convert.ToInt32(resultTemplate.Id);
                #endregion

                if (pks.WorkflowId != null)
                {
                    pks.StatusPks = StatusPks.Pending;
                    var savePks = _repository.save(pks, UserId());
                    var resultx = _workflowrepo.PengajuanDokumen(new Guid(savePks.Id), Convert.ToInt32(resultTemplate.Id), DocumentType);
                    if (string.IsNullOrEmpty(resultx.Id))
                    {
                        pks.StatusPks = StatusPks.Draft;
                        _repository.save(pks, UserId());
                        return Json(resultx);
                    }

                    _repository.AddRiwayatDokumenPks(new RiwayatDokumenPks()
                    {
                        ActionDate=DateTime.Now,
                        //Comment = Note,
                        Status="Pengajuan Dokumen Pks",
                        PksId=pks.Id,
                        UserId=UserId()
                    }, UserId());
                    return Json(new ResultMessage()
                    {
                        Id = resultx.Id,
                        message="Berhasil",
                        status=HttpStatusCode.OK
                    });

                    
                }
                else
                {
                    return Json(new ResultMessage() { 
                        message="GAGAL BUAT WORKFLOW APPROVAL",
                        status=HttpStatusCode.NotImplemented
                    });
                }

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            return Json(new ResultMessage());
        }

       
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult Save(Pks pks)
        {
            try
            {
                return Json(_repository.save(pks, UserId()));
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
        public async Task<IHttpActionResult> UploadFile( Guid id,string tipe)
        {
            var oPks = _repository.get(id);
            TipeBerkas t = (TipeBerkas)Enum.Parse(typeof(TipeBerkas), tipe);
            if ( t == TipeBerkas.FinalLegalPks)
            {
                if (oPks.WorkflowId == null) return Json(new ResultMessage()
                {
                    status = HttpStatusCode.Forbidden,
                    message = Common.Forbiden()
                });
                //List<Reston.Helper.Model.ViewWorkflowModel> getDoc =
                //    _workflowrepo.ListDocumentWorkflow(UserId(), oPks.WorkflowId.Value, Reston.Helper.Model.DocumentStatus.PENGAJUAN, DocumentType, 0, 0);
              
                //if (getDoc.Where(d => d.CurrentUserId == UserId()).FirstOrDefault() == null )
                //    return Json(new ResultMessage()
                //    {
                //        status = HttpStatusCode.Forbidden,
                //        message = Common.Forbiden()
                //    });
                
            }
            if (t == TipeBerkas.DraftPKS && oPks.StatusPks==StatusPks.Approve)
            {
                return Json(new ResultMessage()
                {
                    status = HttpStatusCode.Forbidden,
                    message = Common.Forbiden()
                });

            }
            if (t == TipeBerkas.AssignedPks && oPks.CreateBy != UserId())
            {
                return Json(new ResultMessage()
                {
                    status = HttpStatusCode.Forbidden,
                    message = Common.Forbiden()
                });
            }


                var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            bool isSavedSuccessfully = true;
            string filePathSave = FILE_DOKUMEN_PKS_PATH;//+id ;
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
                    DokumenPks dokumen = new DokumenPks
                    {
                        File = fileName,
                        ContentType = contentType,
                        PksId = id,
                        SizeFile = sizeFile,
                        Tipe=t
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
        public List<VWDokumenPks> getDokumens( Guid Id,TipeBerkas tipe)
        {
            return _repository.GetListDokumenPks(Id,tipe);
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
                var oPks = _repository.getDokPks(Id);
                if (oPks.Pks.WorkflowId != null)
                {
                    List<Reston.Helper.Model.ViewWorkflowModel> getDoc =
                        _workflowrepo.ListDocumentWorkflow(UserId(), oPks.Pks.WorkflowId.Value, Reston.Helper.Model.DocumentStatus.PENGAJUAN, DocumentType, 0, 0);
                    if (getDoc.Where(d => d.CurrentUserId == UserId()).FirstOrDefault() == null) Approver = 1;
                }
                var result = _repository.deleteDokumenPks(Id, UserId(), Approver);
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
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage delete(Guid Id)
        {
            return _repository.deletePks(Id, UserId());
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult Tolak(Guid Id,string Note)
        {
            var result = new Reston.Helper.Util.ResultMessageWorkflowState();
            try
            {               
                result = _workflowrepo.ApproveDokumen(Id, UserId(), Note, Reston.Helper.Model.WorkflowStatusState.REJECTED);
                if (result.data != null)
                {
                    if (result.data.DocumentStatus == Reston.Helper.Model.DocumentStatus.REJECTED)
                    {
                        ///_repository.ChangeStatusPersetujuanPemenang(Id, StatusPengajuanPemenang.REJECTED, UserId());
                        var resultx = _repository.TolakPks(Id, UserId());
                    }

                    _repository.AddRiwayatDokumenPks(new RiwayatDokumenPks()
                    {
                        ActionDate = DateTime.Now,
                        Comment = Note,
                        Status = "Dokumen Pks DiTolak Oleh: " + CurrentUser.UserName,
                        UserId = UserId()
                    }, UserId());
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new Reston.Helper.Util.ResultMessageWorkflowState()
                {
                    status=HttpStatusCode.NotImplemented,
                    message=ex.ToString()
                });
            }
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public Reston.Helper.Util.ResultMessageWorkflowState Setujui(Guid id, string Note,string NoPks)
        {
            var result = new Reston.Helper.Util.ResultMessageWorkflowState();
            try
            {
                result = _workflowrepo.ApproveDokumen(id, UserId(), "", Reston.Helper.Model.WorkflowStatusState.APPROVED);
                if (!string.IsNullOrEmpty(result.Id))
                {
                    RiwayatDokumenPks nRiwayatDokumen = new RiwayatDokumenPks();
                    nRiwayatDokumen.Status = "Dokumen Pengadaan DiSetujui";
                    nRiwayatDokumen.Comment = Note;
                    nRiwayatDokumen.PksId = id;
                    nRiwayatDokumen.UserId = UserId();
                    nRiwayatDokumen.ActionDate = DateTime.Now;
                    _repository.AddRiwayatDokumenPks(nRiwayatDokumen,UserId());
                    ViewWorkflowState oViewWorkflowState = _workflowrepo.StatusDocument(id);
                    if (oViewWorkflowState.DocumentStatus == DocumentStatus.APPROVED)
                    {
                        _repository.SetujuiPks(id,NoPks, UserId());
                    }
                }
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult Edit(Guid Id)
        {
            
            return Json(_repository.ChangeStatus(Id,StatusPks.Draft, UserId()));
            
        }

        public HttpResponseMessage OpenFile(Guid Id)
        {
            var data = _repository.getDokPks(Id);
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + FILE_DOKUMEN_PKS_PATH + data.File;
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


        public IHttpActionResult Pending(Guid Id, string note)
        {
            var change = _repository.ChangeStatus(Id, StatusPks.Pending, UserId());
            if (!string.IsNullOrEmpty(change.Id))
            {
                CatatanPks nCatatan = new CatatanPks()
                {
                    PksId = Id,
                    Catatan = note,
                    CreatedBy = UserId(),
                    CreatedOn = DateTime.Now
                };
                 _repository.saveCatatan(nCatatan);
            }
            return Json(change); 

        }

        public IHttpActionResult ListCatatan(Guid Id)
        {
           
            return Json(_repository.ListCatatanPKs(Id));

        }
    }
    
}
