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
                ndata.Prihal = data.Prihal;
                ndata.Vendor = data.Vendor;
                ndata.NoPO = data.NoPO;
                ndata.NilaiPO = data.NilaiPO;
                ndata.UP = data.UP;
                ndata.NamaBank = data.NamaBank;
                ndata.AtasNama = data.AtasNama;
                ndata.NoRekening = data.NoRekening;
                ndata.AlamatPengirimanBarang = data.AlamatPengirimanBarang;
                ndata.UPPengirimanBarang = data.UPPengirimanBarang;
                ndata.TelpPengirimanBarang = data.TelpPengirimanBarang;
                ndata.AlamatKwitansi = data.AlamatKwitansi;
                ndata.NPWP = data.NPWP;
                ndata.AlamatPengirimanKwitansi = data.AlamatPengirimanKwitansi;
                ndata.UPPengirimanKwitansi = data.UPPengirimanKwitansi;
                ndata.Ttd1 = data.Ttd1;
                ndata.Ttd2 = data.Ttd2;
                ndata.Ttd3 = data.Ttd3;
                ndata.Discount = data.Discount;
                ndata.PPN = data.PPN;
                ndata.PPH = data.PPH;

                if (!string.IsNullOrEmpty(data.TanggalPOstr))
                {
                    try
                    {
                        ndata.TanggalPO = Common.ConvertDate(data.TanggalPOstr, "dd/MM/yyyy");
                    }
                    catch { }
                }
                if (!string.IsNullOrEmpty(data.PeriodeDaristr))
                {
                    try
                    {
                        ndata.PeriodeDari = Common.ConvertDate(data.PeriodeDaristr, "dd/MM/yyyy");
                    }
                    catch { }
                }
                if (!string.IsNullOrEmpty(data.PeriodeSampaistr))
                {
                    try
                    {
                        ndata.PeriodeSampai = Common.ConvertDate(data.PeriodeSampaistr, "dd/MM/yyyy");
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
            VWPOReport data1 = new VWPOReport() {
                Id = po.Id,
                Prihal = po.Prihal,
                Vendor = po.Vendor,
                UP = po.UP,
                NoPO = po.NoPO,
                TanggalPO = po.TanggalPO != null ? po.TanggalPO.Value.Day + " " + Common.ConvertNamaBulan(po.TanggalPO.Value.Month) + " " + po.TanggalPO.Value.Year : "",
                TanggalPOstr = po.TanggalPO != null ? po.TanggalPO.Value.Day + " " + Common.ConvertNamaBulan(po.TanggalPO.Value.Month) + " " + po.TanggalPO.Value.Year : "",
                NilaiPO = po.PODetail == null ? "" : po.PODetail.Sum(d => d.Harga * d.Banyak).Value.ToString("C", MyConverter.formatCurrencyIndo()),
                AlmatBarangUp = po.AlamatPengirimanBarang,
                UpPengirimanBarang = po.UPPengirimanBarang,
                Rekening = po.NoRekening,
                AtasNama = po.AtasNama,
                Bank = po.NamaBank,
                TelpBarang = po.TelpPengirimanBarang,
                KwitansiUp = po.UPPengirimanKwitansi,
                Total = "",
                TTD1 = po.Ttd1,
                TTD2 = po.Ttd2,
                TTD3 = po.Ttd3,
                TTD4 = po.UP,
                PeriodeDari = po.PeriodeDari != null ? po.PeriodeDari.Value.Day + " " + Common.ConvertNamaBulan(po.PeriodeDari.Value.Month) + " " + po.PeriodeDari.Value.Year : "-",
                PeriodeDaristr = po.PeriodeDari != null ? po.PeriodeDari.Value.Day + " " + Common.ConvertNamaBulan(po.PeriodeDari.Value.Month) + " " + po.PeriodeDari.Value.Year : "-",
                PeriodeSampai = po.PeriodeSampai != null ? po.PeriodeSampai.Value.Day + " " + Common.ConvertNamaBulan(po.PeriodeSampai.Value.Month) + " " + po.PeriodeSampai.Value.Year : "-",
                PeriodeSampaistr = po.PeriodeSampai != null ? po.PeriodeSampai.Value.Day + " " + Common.ConvertNamaBulan(po.PeriodeSampai.Value.Month) + " " + po.PeriodeSampai.Value.Year : "-",
                AlamatKwitansi = po.AlamatKwitansi,
                NPWP = po.NPWP,
                AlamatPengirimanKwitansi = po.AlamatPengirimanKwitansi
            };
           List<VWPOReport> lstdata1 = new List<VWPOReport>();
           lstdata1.Add(data1);
           List< VWPODetailReport> lstdata2 = new List< VWPODetailReport>();
           if (po.PODetail != null)
           {
                var nilaipo = po.PODetail == null ? 0 : po.PODetail.Sum(d => d.Harga * d.Banyak);
                decimal? hitungdiscount = (po.Discount / 100 == null?0: (po.Discount / 100)) * (nilaipo == null ? 0 : nilaipo);
                decimal nilaiPosetelahdiskon = (nilaipo == null ? 0 : nilaipo.Value) - (hitungdiscount == null ? 0 : hitungdiscount.Value);
                decimal hitungppn = nilaiPosetelahdiskon * (po.PPN==null?0:po.PPN.Value) / 100;

                //decimal? hitungppn = (po.PPN / 100 == null ? 0 : (po.PPN / 100)) * (nilaipo == null ? 0 : nilaipo);
                decimal hitungpph = (po.PPH==null?0:po.PPH.Value / 100 ) * nilaiPosetelahdiskon;
                
                //decimal? nilaidiscount = (nilaitotal == null ? 0 : nilaitotal.Value) - (hitungdiscount == null ? 0 : hitungdiscount.Value);
                decimal? nilaidiscount = hitungdiscount == null ? 0 : hitungdiscount.Value;
                decimal? nilaippn =hitungppn;
                decimal? nilaipph =  hitungpph;
                decimal? nilaidpp = nilaiPosetelahdiskon;
                decimal? nilaitotal = (nilaiPosetelahdiskon+ nilaippn) - (nilaipph);
                lstdata2 = po.PODetail.Select(d => new VWPODetailReport()
                {
                    Id = d.Id,
                    Banyak = Convert.ToInt32(d.Banyak).ToString(),
                    Deskripsi = d.Deskripsi,
                    Harga = d.Banyak == null ? "" : d.Harga.Value.ToString("C", MyConverter.formatCurrencyIndo()),
                    Jumlah = d.Banyak == null ? "" : (d.Harga.Value * d.Banyak.Value).ToString("C", MyConverter.formatCurrencyIndo()),
                    Kode = d.Kode,
                    NamaBarang = d.NamaBarang,
                    Satuan = d.Satuan,
                    SubTotal = po.PODetail == null ? "" : po.PODetail.Sum(dd => dd.Harga * dd.Banyak).Value.ToString("C", MyConverter.formatCurrencyIndo()),
                    Discount = Convert.ToInt32(po.Discount).ToString() == null ? "" : Convert.ToInt32(po.Discount).ToString(),
                    NilaiDiscount = nilaidiscount.Value.ToString("C", MyConverter.formatCurrencyIndo()),
                    PPN = Convert.ToInt32(po.PPN).ToString() == null ? "" : Convert.ToInt32(po.PPN).ToString(),
                    NilaiPPN = nilaippn.Value.ToString("C", MyConverter.formatCurrencyIndo()),
                    PPH = Convert.ToInt32(po.PPH).ToString() == null ? "" : Convert.ToInt32(po.PPH).ToString(),
                    NilaiPPH = nilaipph.Value.ToString("C", MyConverter.formatCurrencyIndo()),
                    NilaiDPP = nilaidpp.Value.ToString("C", MyConverter.formatCurrencyIndo()),
                    Total = nilaitotal.Value.ToString("C", MyConverter.formatCurrencyIndo()),
                    Keterangan = d.Keterangan == null?"":d.Keterangan
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
          "  <PageWidth>15.267in</PageWidth>" +
          "  <PageHeight>20.000in</PageHeight>" +
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
