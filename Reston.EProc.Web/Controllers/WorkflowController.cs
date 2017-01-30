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
    public class VWWOrflowPersetujuan
    {
        public Guid Id { get; set; }
        List<VWWorkflowDetail> data { get; set; }
    }

    public class VWWorkflowDetail
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string PersonilId { get; set; }
        public string jabatan { get; set; }
    }
    public class DataTableWorkflowDetail    
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<VWWorkflowDetail> data { get; set; }
    }
    public class WorkflowController : BaseController
    {
        private IWorkflowRepository _repository;
        private IPengadaanRepo _repopengdaan;
        public WorkflowController()
        {
            _repository = new WorkflowRepository(new Reston.Helper.HelperContext());
            _repopengdaan = new PengadaanRepo(new JimbisContext());
        }

        public WorkflowController(WorkflowRepository repository)
        {
            _repository = repository;
        }
       

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager,
                                           IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_direksi)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public async Task< IHttpActionResult> List()
      {
          try
          {
              //int start = Convert.ToInt32(System.Web.HttpContext.Current.Request["start"].ToString());
              //string search = System.Web.HttpContext.Current.Request["search"].ToString();
              //int length = Convert.ToInt32(System.Web.HttpContext.Current.Request["length"].ToString());
              string Id = System.Web.HttpContext.Current.Request["Id"].ToString();
              int Idx=0;
              try{
                  Guid idPengdaan = new Guid(Id);
                  var penge = _repopengdaan.GetPengadaanByiD(idPengdaan);
                  if (penge.PersetujuanPemenangs.FirstOrDefault() != null)
                  {
                      Idx = penge.PersetujuanPemenangs.FirstOrDefault().WorkflowId.Value;
                  }
              }
              catch{}

              var data = _repository.ListWorkflowDetails(Idx);

              List<VWWorkflowDetail> lstdt = new List<VWWorkflowDetail>();
              foreach (var item in data)
              {
                  //var jabatan = _repository.JabatanForApprovalPelaksanaan(item.Id);
                  VWWorkflowDetail dt = new VWWorkflowDetail();
                  Userx userdetail =await userDetail(item.UserId.ToString());
                  dt.jabatan = userdetail.jabatan;
                  dt.Nama = userdetail.Nama;
                  dt.Id = item.Id;
                  dt.PersonilId = item.UserId.ToString();
                  lstdt.Add(dt);
              }
              DataTableWorkflowDetail datatable = new DataTableWorkflowDetail();
              datatable.data = lstdt;
              datatable.recordsTotal = data.Count();
              datatable.recordsFiltered = data.Count();
              return Json(datatable);
          }
          catch (Exception ex)
          {
              return Json(new DataTableWorkflowDetail());
          }
      }

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public IHttpActionResult save(List<VWWorkflowDetail> data, Guid Id)
      {
          var vwpengadaan = _repopengdaan.GetPengadaanByiD(Id);
          WorkflowMasterTemplate datax = new WorkflowMasterTemplate();
          if (vwpengadaan.PersetujuanPemenangs.FirstOrDefault() != null)
              datax.Id = vwpengadaan.PersetujuanPemenangs.FirstOrDefault().WorkflowId.Value;
            datax.CreateOn = DateTime.Now;
            datax.CreateBy = UserId();
            datax.NameValue = "Approver Pemenang Pengdaan:" + vwpengadaan.Judul;
            datax.DescValue = "Approver Pemenang Pengdaan:" + vwpengadaan.Judul;
            datax.ApprovalType = ApprovalType.BERTINGKAT;
            List<WorkflowMasterTemplateDetail> lstData = new List<WorkflowMasterTemplateDetail>();
           int i=1;
            foreach (var item in data)
            {
                WorkflowMasterTemplateDetail dt = new WorkflowMasterTemplateDetail();
                dt.UserId =new Guid(item.PersonilId);
                dt.SegOrder = i;
                i++;
                lstData.Add(dt);
            }
            datax.WorkflowMasterTemplateDetails = lstData;
          

          var resultTemplate = _repository.SaveWorkFlow(datax, UserId());
          if (vwpengadaan.PersetujuanPemenangs.FirstOrDefault() == null)
          {
              int TemplateId = 0;
              try { TemplateId = Convert.ToInt32(resultTemplate.Id); }
              catch { }
              if (TemplateId != 0)
              {
                  PersetujuanPemenang ndata = new PersetujuanPemenang()
                  {
                      CreatedOn = DateTime.Now,
                      CreatedBy = UserId(),
                      PengadaanId = Id,
                      WorkflowId = TemplateId
                  };
                  _repopengdaan.SavePersetujuanPemenang(ndata, UserId());
              }
          }

          return Json(resultTemplate);
      }

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public async Task< IHttpActionResult> saveHeader(Guid Id)
      {
        var pengadaan = _repopengdaan.GetPengadaanByiD(Id);

        WorkflowMasterTemplate data = new WorkflowMasterTemplate()
        {
            NameValue = "Approver Pemenang Pengdaan:" + pengadaan.Judul,
            DescValue = "Approver Pemenang Pengdaan:" + pengadaan.Judul,
            ApprovalType=ApprovalType.BERTINGKAT
        };
        var result = _repository.SaveHeader(data, UserId());
        if (result.status == HttpStatusCode.OK)
        {
            pengadaan.WorkflowId = Convert.ToInt32(result.Id);
             _repopengdaan.AddPengadaan(pengadaan, UserId(), await listGuidManager());
        }
        return Json(_repository.SaveHeader(data, UserId()));
      }

      //[ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
      //                                     IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
      //                                      IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      //[System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      //public async Task<IHttpActionResult> saveDetail(WorkflowMasterTemplateDetail data)
      //{
      //    data.
      //    var result = _repository.SaveDetail(data);
      //    if (result.status == HttpStatusCode.OK)
      //    {
      //        pengadaan.WorkflowId = Convert.ToInt32(result.Id);
      //        _repopengdaan.AddPengadaan(pengadaan, UserId(), await listGuidManager());
      //    }
      //    return Json(_repository.SaveHeader(data, UserId()));
      //}

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager,
                                           IdLdapConstants.Roles.pRole_compliance,
                                            IdLdapConstants.Roles.pRole_direksi)]
      public async Task<IHttpActionResult> ListUser()// ListUser(int start, int limit)
      {
          var client = new HttpClient();
          int start = Convert.ToInt32(System.Web.HttpContext.Current.Request["start"].ToString());
          string search = System.Web.HttpContext.Current.Request["search[value]"].ToString();
          int length = Convert.ToInt32(System.Web.HttpContext.Current.Request["length"].ToString());
          string tipe = System.Web.HttpContext.Current.Request["tipe"].ToString();


          DataTableWorkflowDetail data = new DataTableWorkflowDetail();
          client.DefaultRequestHeaders.Accept.Clear();
          client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

          HttpResponseMessage reply = await client.GetAsync(
                  string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/ListUser?start=" + start + "&limit=" + length + "&filter=" + tipe));
          if (reply.IsSuccessStatusCode)
          {
              string masterDataContent = await reply.Content.ReadAsStringAsync();
              var masterData = JsonConvert.DeserializeObject<DataPageUsers>(masterDataContent);
              data.data = masterData.Users.Select(d=>new VWWorkflowDetail(){
                  PersonilId=d.PersonilId,Nama=d.Nama,jabatan=d.jabatan
              }).ToList();
              data.recordsFiltered = masterData.totalRecord.Value;
              data.recordsTotal = masterData.totalRecord.Value;
          }
          return Json(data);

      }

     [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager,
                                           IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_direksi)]
      public async Task<IHttpActionResult> ListHistoryPersetujuanPemenang(Guid Id)
      {
          var pengadaan = _repopengdaan.GetPengadaanByiD(Id);
          List<VWWorkflowApproval> lstdt = new List<VWWorkflowApproval>();
          if (pengadaan.PersetujuanPemenangs.FirstOrDefault() != null)
          {
              var data = _repository.ListWorkflowApprovalByWorkflowId(pengadaan.PersetujuanPemenangs.FirstOrDefault().WorkflowId.Value, 10, 1);
              
             
              foreach(var item in data.data){
                  VWWorkflowApproval dt=new VWWorkflowApproval();
                  var user=await userDetail(item.UserId.ToString());
                  dt.ActionDate=item.ActionDate;
                  dt.Comment=item.Comment;
                  dt.Id=item.Id;
                  dt.UserId = item.UserId;
                  if (user != null)
                  {
                      dt.UserName = user.Nama;
                      dt.Jabatan = user.jabatan;
                  }
                  lstdt.Add(dt);
              }              
          }
         return Json(lstdt);


      }
    }
    
}
