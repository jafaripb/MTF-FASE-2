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
using Reston.Pinata.Model.Helper;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.PengadaanRepository;
using Reston.Pinata.Model.PengadaanRepository.View;
using Reston.Pinata.WebService.Helper;
using Reston.Pinata.WebService.ViewModels;
using Novacode;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Drawing;
using Microsoft.Reporting.WebForms;
using System.Reflection;
using System.Xml.Linq;
using System.Web;

namespace Reston.Pinata.WebService.Controllers
{
    public class ReportController : BaseController
    {
        private IPengadaanRepo _repository;
        private IPORepo _porepo;
        private IRksRepo _rksrepo;
        internal ResultMessage result = new ResultMessage();
        private string FILE_TEMP_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_TEMP"];
        private string FILE_DOKUMEN_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_DOC"];
        public ReportController()
        {
            _repository = new PengadaanRepo(new JimbisContext());
            _porepo = new PORepo(new JimbisContext());
            _rksrepo = new RksRepo(new JimbisContext());
        }

        public ReportController(PengadaanRepo repository)
        {
            _repository = repository;
        }

        [Authorize]
        public async Task<IHttpActionResult> UploadFile(string tipe, Guid id)
        {
            var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            bool isSavedSuccessfully = true;
            string filePathSave = FILE_DOKUMEN_PATH;//+id ;
            string fileName = tipe;
            if (Directory.Exists(uploadPath + filePathSave) == false)
            {
                Directory.CreateDirectory(uploadPath + filePathSave);
            }

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
                filePathSave += tipe + "-" + newGuid.ToString() + "." + extension;
                fileName += "-" + newGuid.ToString() + "." + extension;
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
            TipeBerkas t = (TipeBerkas)Enum.Parse(typeof(TipeBerkas), tipe);
            DokumenPengadaan dokumen = new DokumenPengadaan
            {
                File = fileName,
                Tipe = t,
                ContentType = contentType,
                PengadaanId = id,
                SizeFile = sizeFile
            };

            if (isSavedSuccessfully)
            {
                try
                {
                    DokumenPengadaan dokumenUpdate = _repository.saveDokumenPengadaan(dokumen, UserId());
                    if (t == TipeBerkas.BeritaAcaraAanwijzing)
                    {
                        //  int x = _repository.UpdateStatus(id, EStatusPengadaan.SUBMITPENAWARAN);
                    }
                    return Json(dokumen.Id);
                }
                catch (Exception ex)
                {
                    return Json(0);
                }
            }

            return Json(dokumen.Id);
        }

        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task< HttpResponseMessage> BerkasAanwzjing(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalAanwijzing = _repository.getPelaksanaanAanwijing(Id);
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\Berita Acara Pemberian Penjelasan.docx";

            string outputFileName = "BA-Aanwizjing-" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;
            // Create a document in memory:
            //string outputFileName =
            //        string.Format(fileName, "BAcoooooooooooooot", DateTime.Now.ToString("dd-MM-yy"));
            var streamx = new FileStream(fileName, FileMode.Open);
            try
            {
                var BeritaAcara = _repository.getBeritaAcaraByTipe(Id, TipeBerkas.BeritaAcaraAanwijzing, UserId());
                var doc = DocX.Load(streamx);//.Create(OutFileNama);
                doc.ReplaceText("{pengadaan_name}", pengadaan.Judul == null ? "" : pengadaan.Judul);
                doc.ReplaceText("{pengadaan_name_judul}", pengadaan.Judul == null ? "" : pengadaan.Judul.ToUpper());
                doc.ReplaceText("{nomor_berita_acara}", BeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara);
                doc.ReplaceText("{pengadaan_unit_pemohon}", pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);

                //var list = doc.AddList(listType: ListItemType.Numbered, startNumber: 1);

                //doc.AddListItem(list, "Number 1");
                //doc.AddListItem(list, "Number 2");
                var kandidat = _repository.getKandidatPengadaan(Id, UserId());
                //list.InsertParagraphAfterSelf("{vendor1}");
                //doc.FindAll("{tabel}").ForEach(index => );
                var table = doc.AddTable(kandidat.Count(), 1);
                int rowIndex = 0;
                foreach (var item in kandidat)
                {
                    table.Rows[rowIndex].Cells[0].Paragraphs.First().Append((rowIndex + 1) + ". " + "....................   Mewakili: " + item.NamaVendor);
                    table.Rows[rowIndex].Cells[0].Paragraphs.First().FontSize(11).Font(new FontFamily("Calibri"));
                    table.Rows[rowIndex].Cells[0].Width = 500;
                    rowIndex++;
                }

                //table.Rows[0].Cells[0].
                //table.Rows[0].Cells[0].MarginTop = 0;


                table.Alignment = Alignment.center;
                //table.AutoFit = AutoFit.Contents;

                foreach (var paragraph in doc.Paragraphs)
                {
                    paragraph.FindAll("{tabel}").ForEach(index => paragraph.InsertTableBeforeSelf(table));

                }
                doc.ReplaceText("{tabel}", "");

                if (jadwalAanwijzing != null)
                {
                    doc.ReplaceText("{pengadaan_jadwal_hari}", Common.ConvertHari((int)jadwalAanwijzing.Mulai.Value.DayOfWeek));
                    doc.ReplaceText("{pengadaan_jadwal_tanggal}", jadwalAanwijzing.Mulai.Value.Day.ToString() +
                        " " + Common.ConvertNamaBulan(jadwalAanwijzing.Mulai.Value.Month) +
                        " " + jadwalAanwijzing.Mulai.Value.Year.ToString());
                    doc.ReplaceText("{tempat_tanggal}", ".............," + jadwalAanwijzing.Mulai.Value.Day.ToString() +
                        " " + Common.ConvertNamaBulan(jadwalAanwijzing.Mulai.Value.Month) +
                        " " + jadwalAanwijzing.Mulai.Value.Year.ToString());
                }
                else
                {
                    doc.ReplaceText("{pengadaan_jadwal_hari} ", " - ");
                    doc.ReplaceText("{pengadaan_jadwal_tanggal}", "");
                    doc.ReplaceText("{tempat_tanggal}", "...............,...........................");
                }

                var listPersonil = _repository.getListPersonilPengadaan(Id);
                var NamePic = listPersonil.Where(d => d.tipe == "pic").FirstOrDefault().Nama;
                doc.ReplaceText("{nama_pic}", NamePic);
                

                //tambah tabel persetujuan tahapan
                var table2 = await getTablePersetujuan(pengadaan.Id, EStatusPengadaan.AANWIJZING, doc);

                table2.Alignment = Alignment.center; 
                //table.AutoFit = AutoFit.Contents;

                foreach (var paragraph in doc.Paragraphs)
                {
                    paragraph.FindAll("{tabel2}").ForEach(index => paragraph.InsertTableBeforeSelf(table2));

                }
                doc.ReplaceText("{tabel2}", "");
                //end

                doc.SaveAs(OutFileNama);
                streamx.Close();
            }
            catch
            {
                streamx.Close();
            }
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }

        private async Task<Table> getTablePersetujuan(Guid PengadaanId, EStatusPengadaan status ,DocX doc)
        {
            var personilPersetujuan = _repository.GetPersetujuanTahapan(PengadaanId, status);
            var table2 = doc.AddTable(personilPersetujuan.Count() + 1, 3);
            Border WhiteBorder = new Border(BorderStyle.Tcbs_single, BorderSize.four, 1, Color.Black);
            table2.SetBorder(TableBorderType.Bottom, WhiteBorder);
            table2.SetBorder(TableBorderType.Left, WhiteBorder);
            table2.SetBorder(TableBorderType.Right, WhiteBorder);
            table2.SetBorder(TableBorderType.Top, WhiteBorder);
            table2.SetBorder(TableBorderType.InsideV, WhiteBorder);
            table2.SetBorder(TableBorderType.InsideH, WhiteBorder);
            int rowIndex = 0;

            table2.Rows[rowIndex].Cells[0].Paragraphs.First().Append("Nama");
            table2.Rows[rowIndex].Cells[0].Paragraphs.First().FontSize(11).Font(new FontFamily("Calibri"));
            table2.Rows[rowIndex].Cells[0].Width = 500;
            table2.Rows[rowIndex].Cells[1].Paragraphs.First().Append("Tanggal");
            table2.Rows[rowIndex].Cells[1].Paragraphs.First().FontSize(11).Font(new FontFamily("Calibri"));
            table2.Rows[rowIndex].Cells[1].Width = 500;
            table2.Rows[rowIndex].Cells[2].Paragraphs.First().Append("Status");
            table2.Rows[rowIndex].Cells[2].Paragraphs.First().FontSize(11).Font(new FontFamily("Calibri"));
            table2.Rows[rowIndex].Cells[2].Width = 500;
            rowIndex++;
            foreach (var item in personilPersetujuan)
            {
                var user = await userDetail(item.UserId.ToString());
                table2.Rows[rowIndex].Cells[0].Paragraphs.First().Append(user.Nama);
                table2.Rows[rowIndex].Cells[0].Paragraphs.First().FontSize(11).Font(new FontFamily("Calibri"));
                table2.Rows[rowIndex].Cells[0].Width = 500;
                table2.Rows[rowIndex].Cells[1].Paragraphs.First().Append(item.CreatedOn == null ? "" : item.CreatedOn.Value.Day.ToString() +
                    " " + Common.ConvertNamaBulan(item.CreatedOn.Value.Month) +
                    " " + item.CreatedOn.Value.Year.ToString());
                table2.Rows[rowIndex].Cells[1].Paragraphs.First().FontSize(11).Font(new FontFamily("Calibri"));
                table2.Rows[rowIndex].Cells[1].Width = 500;
                table2.Rows[rowIndex].Cells[2].Paragraphs.First().Append(item.Status.ToString());
                table2.Rows[rowIndex].Cells[2].Paragraphs.First().FontSize(11).Font(new FontFamily("Calibri"));
                table2.Rows[rowIndex].Cells[2].Width = 500;
                rowIndex++;
            }

            return table2;
        }

        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage BerkasDaftarRapat(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalAanwijzing = _repository.getPelaksanaanAanwijing(Id);
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\Daftar Rapat.docx";

            string outputFileName = "Daftar-Rapat-" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;
            // Create a document in memory:
            //string outputFileName =
            //        string.Format(fileName, "BAcoooooooooooooot", DateTime.Now.ToString("dd-MM-yy"));
            var streamx = new FileStream(fileName, FileMode.Open);
            try
            {
                var doc = DocX.Load(streamx);//.Create(OutFileNama);
                doc.SaveAs(OutFileNama);
                streamx.Close();
            }
            catch
            {
                streamx.Close();
            }
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }

        public HttpResponseMessage ReportPengadaan(string dari, string sampai)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Report\BukaAmplop.rdlc";
                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }
                else
                {
                    //return View("Index");
                }
                var oDari = Common.ConvertDate(dari, "dd/MM/yyyy");
                var oSampai = Common.ConvertDate(sampai, "dd/MM/yyyy");

                var oKandidatPengadaan = _repository.GetRepotPengadan(oDari, oSampai, UserId());

                ReportDataSource rd = new ReportDataSource("DataSet2", oKandidatPengadaan);
                lr.DataSources.Add(rd);
                string param1 = "";
                string filename = "";
                string param2 = "";
                string paramSemester = "";
                string paramTahunAjaran = "";


                string reportType = "doc";
                string mimeType;
                string encoding;
                string fileNameExtension;


                string[] streamids = null;
                String extension = null;
                Byte[] bytes = null;
                Warning[] warnings;

                bytes = lr.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                Stream stream = new MemoryStream(bytes);

                result.Content = new StreamContent(stream);

                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Report-Monitoring" + UserId() + DateTime.Now.ToString("dd-MM-yy") + ".xls"
                };

                return result;
            }
            catch (ReflectionTypeLoadException ex)
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                result.Content = new StringContent(sb.ToString());

                return result;
                //Display or log the error based on your application.
            }
        }

      

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<HttpResponseMessage> BerkasBukaAmplop(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalBukaAmplop = _repository.getPelaksanaanBukaAmplop(Id);
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\BERITA ACARA BUKA AMPLOP.docx";
            var BeritaAcara = _repository.getBeritaAcaraByTipe(Id, TipeBerkas.BeritaAcaraBukaAmplop, UserId());
            string outputFileName = "BA-Buka-Amplop-" + (BeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara.Replace("/", "-")) + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var streamx = new FileStream(fileName, FileMode.Open);
            try
            {

                var doc = DocX.Load(streamx);//.Create(OutFileNama);
                doc.ReplaceText("{pengadaan_name}", pengadaan.Judul == null ? "" : pengadaan.Judul);
                doc.ReplaceText("{pengadaan_name_judul}", pengadaan.Judul == null ? "" : pengadaan.Judul.ToUpper());
                doc.ReplaceText("{nomor_berita_acara}", BeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara);
                doc.ReplaceText("{pengadaan_unit_pemohon}", pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);

                doc.ReplaceText("{tempat_tanggal}", BeritaAcara == null ? "" : "...............," + BeritaAcara.tanggal == null ? "................" :
                       BeritaAcara.tanggal.Value.Day + " " + Common.ConvertNamaBulan(BeritaAcara.tanggal.Value.Month) + " " +
                       BeritaAcara.tanggal.Value.Year);

                doc.ReplaceText("{pengadaan_jadwal_hari}", BeritaAcara == null ? "" : BeritaAcara.tanggal == null ? "" :
                       Common.ConvertHari(BeritaAcara.tanggal.Value.Day));
                doc.ReplaceText("{pengadaan_jadwal_tanggal}", BeritaAcara == null ? "" : BeritaAcara.tanggal.Value.Day + " " + Common.ConvertNamaBulan(BeritaAcara.tanggal.Value.Month) +
                      " " + BeritaAcara.tanggal.Value.Year);
                var kandidat = _repository.getKandidatPengadaan(Id, UserId());
                var table = doc.AddTable(kandidat.Count(), 1);
                Border WhiteBorder = new Border(BorderStyle.Tcbs_none, 0, 0, Color.White);
                table.SetBorder(TableBorderType.Bottom, WhiteBorder);
                table.SetBorder(TableBorderType.Left, WhiteBorder);
                table.SetBorder(TableBorderType.Right, WhiteBorder);
                table.SetBorder(TableBorderType.Top, WhiteBorder);
                table.SetBorder(TableBorderType.InsideV, WhiteBorder);
                table.SetBorder(TableBorderType.InsideH, WhiteBorder);
                int rowIndex = 0;
                foreach (var item in kandidat)
                {
                    table.Rows[rowIndex].Cells[0].Paragraphs.First().Append((rowIndex + 1) + ". " + item.NamaVendor);
                    table.Rows[rowIndex].Cells[0].Paragraphs.First().FontSize(11).Font(new FontFamily("Calibri"));
                    table.Rows[rowIndex].Cells[0].Width = 500;
                    rowIndex++;
                }

                table.Alignment = Alignment.center;
                foreach (var paragraph in doc.Paragraphs)
                {
                    paragraph.FindAll("{vendor}").ForEach(index => paragraph.InsertTableBeforeSelf(table));

                }
                doc.ReplaceText("{vendor}", "");

                var panitia = _repository.getPersonilPengadaan(Id);
                var tablePanitia = doc.AddTable(panitia.Count(), 1);
                WhiteBorder = new Border(BorderStyle.Tcbs_none, 0, 0, Color.White);
                tablePanitia.SetBorder(TableBorderType.Bottom, WhiteBorder);
                tablePanitia.SetBorder(TableBorderType.Left, WhiteBorder);
                tablePanitia.SetBorder(TableBorderType.Right, WhiteBorder);
                tablePanitia.SetBorder(TableBorderType.Top, WhiteBorder);
                tablePanitia.SetBorder(TableBorderType.InsideV, WhiteBorder);
                tablePanitia.SetBorder(TableBorderType.InsideH, WhiteBorder);
                rowIndex = 0;
                foreach (var item in panitia)
                {
                    tablePanitia.Rows[rowIndex].Cells[0].Paragraphs.First().Append((rowIndex + 1) + ". " + item.Nama);
                    tablePanitia.Rows[rowIndex].Cells[0].Paragraphs.First().FontSize(11).Font(new FontFamily("Calibri"));
                    tablePanitia.Rows[rowIndex].Cells[0].Width = 500;
                    rowIndex++;
                }

                tablePanitia.Alignment = Alignment.center;
                foreach (var paragraph in doc.Paragraphs)
                {
                    paragraph.FindAll("{panitia}").ForEach(index => paragraph.InsertTableBeforeSelf(tablePanitia));

                }
                doc.ReplaceText("{panitia}", "");

                //tambah tabel persetujuan tahapan
                var table3 = await getTablePersetujuan(pengadaan.Id, EStatusPengadaan.BUKAAMPLOP, doc);

                table3.Alignment = Alignment.center;
                //table.AutoFit = AutoFit.Contents;

                foreach (var paragraph in doc.Paragraphs)
                {
                    paragraph.FindAll("{table3}").ForEach(index => paragraph.InsertTableBeforeSelf(table3));

                }
                doc.ReplaceText("{table3}", "");
                //end

                if (_repository.CekBukaAmplop(Id) == 1)
                {

                    var oVWRKSVendors = _repository.getRKSPenilaian2Report(pengadaan.Id, UserId());
                    var table2 = doc.AddTable(oVWRKSVendors.hps.Count() + 1, (oVWRKSVendors.vendors.Count*2) + 6);
                    Border BlankBorder = new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.Black);
                    table2.SetBorder(TableBorderType.Bottom, BlankBorder);
                    table2.SetBorder(TableBorderType.Left, BlankBorder);
                    table2.SetBorder(TableBorderType.Right, BlankBorder);
                    table2.SetBorder(TableBorderType.Top, BlankBorder);
                    table2.SetBorder(TableBorderType.InsideV, BlankBorder);
                    table2.SetBorder(TableBorderType.InsideH, BlankBorder);
                    int no = 1;

                    int indexRow = 0;
                    table2.Rows[indexRow].Cells[0].Paragraphs.First().Append("NO");
                    table2.Rows[indexRow].Cells[0].Width = 10;
                    table2.Rows[indexRow].Cells[1].Paragraphs.First().Append("Item");
                    table2.Rows[indexRow].Cells[2].Paragraphs.First().Append("Satuan");
                    table2.Rows[indexRow].Cells[3].Paragraphs.First().Append("Jumlah");
                    table2.Rows[indexRow].Cells[4].Paragraphs.First().Append("Keterangan Item");
                    table2.Rows[indexRow].Cells[5].Paragraphs.First().Append("Harga HPS");
                    int headerCol = 6;
                    foreach (var item in oVWRKSVendors.vendors)
                    {
                        table2.Rows[indexRow].Cells[headerCol].Paragraphs.First().Append(item.nama);
                        headerCol++;
                        table2.Rows[indexRow].Cells[headerCol].Paragraphs.First().Append("Total ("+item.nama+")");
                        headerCol++;
                    }
                    indexRow++;
                    var itemlast = oVWRKSVendors.hps.Last();
                    foreach (var item in oVWRKSVendors.hps)
                    {
                        if (item.Equals(itemlast))
                        {
                            table2.Rows[indexRow].Cells[0].Paragraphs.First().Append("");
                            table2.Rows[indexRow].Cells[0].Width = 10;
                        }
                        else
                        {

                            if (item.jumlah > 0)
                            {
                                table2.Rows[indexRow].Cells[0].Paragraphs.First().Append(no.ToString());
                                table2.Rows[indexRow].Cells[0].Width = 10;
                                no++;
                            }
                            else
                            {
                                table2.Rows[indexRow].Cells[0].Paragraphs.First().Append("");
                                table2.Rows[indexRow].Cells[0].Width = 10;
                            }
                        }
                        //Regex example #1 "<.*?>"
                        string dekripsi = Regex.Replace(item.item, @"<.*?>", string.Empty);
                        //Regex example #2
                        // string result2 = Regex.Replace(dekripsi, @"<[^>].+?>", "");
                        table2.Rows[indexRow].Cells[1].Paragraphs.First().Append(dekripsi);
                        var info = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        info.NumberFormat.CurrencyDecimalDigits = 0;
                        info.NumberFormat.CurrencySymbol = "Rp ";
                        info.NumberFormat.CurrencyGroupSeparator = ".";
                        info.NumberFormat.CurrencyDecimalSeparator = ",";

                        table2.Rows[indexRow].Cells[2].Paragraphs.First().Append(item.satuan);
                        table2.Rows[indexRow].Cells[3].Paragraphs.First().Append(item.jumlah.ToString());
                        table2.Rows[indexRow].Cells[4].Paragraphs.First().Append(item.keteranganItem);
                        if (item.harga != null && item.jumlah != null)
                        {
                            //decimal harga = item.harga.Value * item.jumlah.Value;
                            table2.Rows[indexRow].Cells[5].Paragraphs.First().Append(item.harga.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                        }
                        else table2.Rows[indexRow].Cells[5].Paragraphs.First().Append("");

                        int nexCol = 6;
                        foreach (var itemx in oVWRKSVendors.vendors)
                        {
                            if (itemx.items.Where(d => d.Id == item.Id) != null)
                            {
                                var itemxx = itemx.items.Where(d => d.Id == item.Id).FirstOrDefault();
                                var harga = itemxx == null ? "" : itemxx.harga == null ? "" : itemxx.harga.Value.ToString("C", MyConverter.formatCurrencyIndo());
                                table2.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append(harga);
                                nexCol++;
                                var jumlah = itemxx == null ? "" : itemxx.jumlah == null ? "" : itemxx.jumlah.Value.ToString();
                                if (jumlah != string.Empty && harga != string.Empty)
                                {
                                    var total = itemxx.jumlah * itemxx.harga;
                                    table2.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append(total.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                                }
                                else table2.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append("");
                                
                            }
                            else table2.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append("");

                            nexCol++;
                        }

                        indexRow++;
                    }

                    System.IO.MemoryStream ms2 = new System.IO.MemoryStream();
                    DocX doc2 = DocX.Create(ms2);

                    doc2.PageLayout.Orientation = Novacode.Orientation.Landscape;
                    Paragraph p = doc2.InsertParagraph();
                    p.Append("Lampiran").Bold();
                    p.Alignment = Alignment.left;
                    p.AppendLine();
                    Table t = doc2.InsertTable(table2);
                    doc.InsertSection();
                    doc.InsertDocument(doc2);
                    //doc.InsertTable(table2);

                }
                doc.SaveAs(OutFileNama);
                streamx.Close();
            }
            catch
            {
                streamx.Close();
            }
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            if (_repository.CekBukaAmplop(Id) == 1)
            {
                var stream = new FileStream(OutFileNama, FileMode.Open);
                result.Content = new StreamContent(stream);
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = outputFileName
                };
            }
            else result.Content = new StringContent("Anda Tidak Memiliki Akses");
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task< HttpResponseMessage> BerkasKlarfikasi(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\BERITA ACARA RAPAT KLARIFIKASI DAN NEGOSIASI.docx";
            var BeritaAcara = _repository.getBeritaAcaraByTipe(Id, TipeBerkas.BeritaAcaraKlarifikasi, UserId());
            string outputFileName = "Berkas-Klarifikasi-" + (BeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara.Replace("/", "-")) + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var streamx = new FileStream(fileName, FileMode.Open);

            var doc = DocX.Load(streamx);

            try
            {


                doc.ReplaceText("{pengadaan_name}", pengadaan.Judul == null ? "" : pengadaan.Judul);
                doc.ReplaceText("{pengadaan_name_judul}", pengadaan.Judul == null ? "" : pengadaan.Judul.ToUpper());
                doc.ReplaceText("{nomor_berita_acara}", BeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara);
                doc.ReplaceText("{pengadaan_unit_pemohon}", pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);

                doc.ReplaceText("{tempat_tanggal}", "...............," + (BeritaAcara == null ? "................" :
                        (BeritaAcara.tanggal.Value.Day + " " + Common.ConvertNamaBulan(BeritaAcara.tanggal.Value.Month) + " " +
                        BeritaAcara.tanggal.Value.Year)));

                doc.ReplaceText("{pengadaan_jadwal_hari}", BeritaAcara == null ? "" :
                       Common.ConvertHari(BeritaAcara.tanggal.Value.Day));
                doc.ReplaceText("{pengadaan_jadwal_tanggal}", BeritaAcara == null ? "" : BeritaAcara.tanggal.Value.Day + " " + Common.ConvertNamaBulan(BeritaAcara.tanggal.Value.Month) +
                      " " + BeritaAcara.tanggal.Value.Year);
                var kandidat = _repository.GetVendorsKlarifikasiByPengadaanId(Id);
                if (kandidat.Count() > 0)
                {
                    var table = doc.AddTable(kandidat.Count(), 1);
                    Border BlankBorder = new Border(BorderStyle.Tcbs_none, 0, 0, Color.White);
                    table.SetBorder(TableBorderType.Bottom, BlankBorder);
                    table.SetBorder(TableBorderType.Left, BlankBorder);
                    table.SetBorder(TableBorderType.Right, BlankBorder);
                    table.SetBorder(TableBorderType.Top, BlankBorder);
                    table.SetBorder(TableBorderType.InsideV, BlankBorder);
                    table.SetBorder(TableBorderType.InsideH, BlankBorder);

                    int rowIndex = 0;
                    foreach (var item in kandidat)
                    {
                        table.Rows[rowIndex].Cells[0].Paragraphs.First().Append((rowIndex + 1) + ". " + item.Nama);
                        table.Rows[rowIndex].Cells[0].Paragraphs.First().FontSize(11).Font(new FontFamily("Calibri"));
                        table.Rows[rowIndex].Cells[0].Width = 550;
                        rowIndex++;
                    }

                    table.Alignment = Alignment.center;
                    foreach (var paragraph in doc.Paragraphs)
                    {
                        paragraph.FindAll("{vendor}").ForEach(index => paragraph.InsertTableBeforeSelf(table));

                    }
                    doc.ReplaceText("{vendor}", "");
                }

                //tambah tabel persetujuan tahapan
                var table3 = await getTablePersetujuan(pengadaan.Id, EStatusPengadaan.KLARIFIKASI, doc);

                table3.Alignment = Alignment.center;
                //table.AutoFit = AutoFit.Contents;

                foreach (var paragraph in doc.Paragraphs)
                {
                    paragraph.FindAll("{table3}").ForEach(index => paragraph.InsertTableBeforeSelf(table3));

                }
                doc.ReplaceText("{table3}", "");
                //end

                //var listPersonil = _repository.getListPersonilPengadaan(Id);
                //var NamePic = listPersonil.Where(d => d.tipe == "pic").FirstOrDefault().Nama.ToString();
                //doc.ReplaceText("{nama_pic}", NamePic);
                //var NameController = listPersonil.Where(d => d.tipe == "controller").FirstOrDefault().Nama.ToString();
                //doc.ReplaceText("{nama_controller}", NameController);
                //var waktu = "........, " + " " + DateTime.Now.Day + " " +
                //    Common.ConvertNamaBulan(DateTime.Now.Month) + " " + DateTime.Now.Year;
                //doc.ReplaceText("{waktu}", waktu);
                //var oVWRKSVendors = _repository.getRKSKlarifikasiPenilaian(pengadaan.Id, UserId());
                //var table = doc.AddTable(oVWRKSVendors.hps.Count() + 1, oVWRKSVendors.vendors.Count + 3);
                //int no = 1;

                //int indexRow = 0;
                //table.Alignment = Alignment.center;
                //table.Rows[indexRow].Cells[0].Paragraphs.First().Append("NO");
                //table.Rows[indexRow].Cells[0].Width = 10;
                //table.Rows[indexRow].Cells[1].Paragraphs.First().Append("Deskripsi");
                //table.Rows[indexRow].Cells[2].Paragraphs.First().Append("Harga HPS");
                //int headerCol = 3;
                //foreach (var item in oVWRKSVendors.vendors)
                //{
                //    table.Rows[indexRow].Cells[headerCol].Paragraphs.First().Append(item.nama);
                //    headerCol++;
                //}
                //indexRow++;
                //var itemlast = oVWRKSVendors.hps.Last();
                //foreach (var item in oVWRKSVendors.hps)
                //{
                //    if (item.Equals(itemlast))
                //    {
                //        table.Rows[indexRow].Cells[0].Paragraphs.First().Append("");
                //        table.Rows[indexRow].Cells[0].Width = 10;
                //    }
                //    else
                //    {

                //        if (item.jumlah > 0)
                //        {
                //            table.Rows[indexRow].Cells[0].Paragraphs.First().Append(no.ToString());
                //            table.Rows[indexRow].Cells[0].Width = 10;
                //            no++;
                //        }
                //        else
                //        {
                //            table.Rows[indexRow].Cells[0].Paragraphs.First().Append("");
                //            table.Rows[indexRow].Cells[0].Width = 10;
                //        }
                //    }
                //    //Regex example #1 "<.*?>"
                //    string dekripsi = Regex.Replace(item.item, @"<.*?>", string.Empty);
                //    //Regex example #2
                //    // string result2 = Regex.Replace(dekripsi, @"<[^>].+?>", "");
                //    table.Rows[indexRow].Cells[1].Paragraphs.First().Append(dekripsi);

                //    table.Rows[indexRow].Cells[2].Paragraphs.First().Append(item.harga.ToString());
                //    int nexCol = 3;
                //    foreach (var itemx in oVWRKSVendors.vendors)
                //    {
                //        if (itemx.items.Where(d => d.Id == item.Id) != null)
                //        {
                //            table.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append(itemx.items.Where(d => d.Id == item.Id).FirstOrDefault().harga.ToString());

                //        }
                //        else table.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append("");

                //        nexCol++;
                //    }

                //    indexRow++;
                //}
                //// Insert table at index where tag #TABLE# is in document.
                ////doc.InsertTable(table);
                //foreach (var paragraph in doc.Paragraphs)
                //{

                //    paragraph.FindAll("{tabel}").ForEach(index => paragraph.InsertTableAfterSelf((table)));
                //}
                ////Remove tag
                //doc.ReplaceText("{tabel}", "");

                doc.SaveAs(OutFileNama);
                streamx.Close();
            }
            catch
            {
                streamx.Close();
            }
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<HttpResponseMessage> BerkasKlarfikasiLanjutan(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\BERITA ACARA RAPAT KLARIFIKASI DAN NEGOSIASI LANJUTAN.docx";
            var BeritaAcara = _repository.getBeritaAcaraByTipe(Id, TipeBerkas.BeritaAcaraKlarifikasiLanjutan, UserId());
            string outputFileName = "Berkas-Klarifikasi-lanjutan-" + (BeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara.Replace("/", "-")) + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var streamx = new FileStream(fileName, FileMode.Open);

            var doc = DocX.Load(streamx);

            try
            {


                doc.ReplaceText("{pengadaan_name}", pengadaan.Judul == null ? "" : pengadaan.Judul);
                doc.ReplaceText("{pengadaan_name_judul}", pengadaan.Judul == null ? "" : pengadaan.Judul.ToUpper());
                doc.ReplaceText("{nomor_berita_acara}", BeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara);
                doc.ReplaceText("{pengadaan_unit_pemohon}", pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);

                doc.ReplaceText("{tempat_tanggal}", "...............," + (BeritaAcara == null ? "................" :
                        (BeritaAcara.tanggal.Value.Day + " " + Common.ConvertNamaBulan(BeritaAcara.tanggal.Value.Month) + " " +
                        BeritaAcara.tanggal.Value.Year)));

                doc.ReplaceText("{pengadaan_jadwal_hari}", BeritaAcara == null ? "" :
                       Common.ConvertHari(BeritaAcara.tanggal.Value.Day));
                doc.ReplaceText("{pengadaan_jadwal_tanggal}", BeritaAcara == null ? "" : BeritaAcara.tanggal.Value.Day + " " + Common.ConvertNamaBulan(BeritaAcara.tanggal.Value.Month) +
                      " " + BeritaAcara.tanggal.Value.Year);
                var kandidat = _repository.GetVendorsKlarifikasiByPengadaanId(Id);
                var table = doc.AddTable(kandidat.Count(), 1);
                Border BlankBorder = new Border(BorderStyle.Tcbs_none, 0, 0, Color.White);
                table.SetBorder(TableBorderType.Bottom, BlankBorder);
                table.SetBorder(TableBorderType.Left, BlankBorder);
                table.SetBorder(TableBorderType.Right, BlankBorder);
                table.SetBorder(TableBorderType.Top, BlankBorder);
                table.SetBorder(TableBorderType.InsideV, BlankBorder);
                table.SetBorder(TableBorderType.InsideH, BlankBorder);

                int rowIndex = 0;
                foreach (var item in kandidat)
                {
                    table.Rows[rowIndex].Cells[0].Paragraphs.First().Append((rowIndex + 1) + ". " + item.Nama);
                    table.Rows[rowIndex].Cells[0].Paragraphs.First().FontSize(11).Font(new FontFamily("Calibri"));
                    table.Rows[rowIndex].Cells[0].Width = 550;
                    rowIndex++;
                }

                table.Alignment = Alignment.center;
                foreach (var paragraph in doc.Paragraphs)
                {
                    paragraph.FindAll("{vendor}").ForEach(index => paragraph.InsertTableBeforeSelf(table));

                }
                doc.ReplaceText("{vendor}", "");


                //tambah tabel persetujuan tahapan
                var table3 = await getTablePersetujuan(pengadaan.Id, EStatusPengadaan.KLARIFIKASILANJUTAN, doc);

                table3.Alignment = Alignment.center;
                //table.AutoFit = AutoFit.Contents;

                foreach (var paragraph in doc.Paragraphs)
                {
                    paragraph.FindAll("{table3}").ForEach(index => paragraph.InsertTableBeforeSelf(table3));

                }
                doc.ReplaceText("{table3}", "");
                //end

                doc.SaveAs(OutFileNama);
                streamx.Close();
            }
            catch
            {
                streamx.Close();
            }
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }




        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task< HttpResponseMessage> BerkasPenilaian(Guid Id)
        {
            ViewPengadaan pengadaan = this._repository.GetPengadaan(Id, base.UserId(), 0);
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\Berita Acara Penilaian.docx";
            string str2 = "BA-Penilaian-" + base.UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";
            string filename = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + str2;
            FileStream stream = new FileStream(path, FileMode.Open);
            try
            {
                BeritaAcara acara = this._repository.getBeritaAcaraByTipe(Id, TipeBerkas.BeritaAcaraPenilaian, base.UserId());
                DocX cx = DocX.Load(stream);
                cx.ReplaceText("{pengadaan_name}", (pengadaan.Judul == null) ? "" : pengadaan.Judul, false, RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                cx.ReplaceText("{pengadaan_name_judul}", (pengadaan.Judul == null) ? "" : pengadaan.Judul.ToUpper(), false, RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                cx.ReplaceText("{nomor_berita_acara}", (acara == null) ? "" : acara.NoBeritaAcara, false, RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                cx.ReplaceText("{pengadaan_unit_pemohon}", (pengadaan.UnitKerjaPemohon == null) ? "" : pengadaan.UnitKerjaPemohon, false, RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                cx.ReplaceText("{tempat_tanggal}", "...............," + (acara == null ? "................" : Common.ConvertNamaBulan(acara.tanggal.Value.Month)));
                cx.ReplaceText("{pengadaan_jadwal_hari}", (acara == null) ? "" : Common.ConvertHari(acara.tanggal.Value.Day), false, RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                cx.ReplaceText("{pengadaan_jadwal_tanggal}", (acara == null) ? "" : string.Concat(new object[] { acara.tanggal.Value.Day, " ", Common.ConvertNamaBulan(acara.tanggal.Value.Month), " ", acara.tanggal.Value.Year }), false, RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                List<VWPembobotanPengadaan> list = this._repository.getKriteriaPembobotan(Id);
                string text = ((from d in list
                                where d.NamaKreteria.Contains("Harga")
                                select d).FirstOrDefault<VWPembobotanPengadaan>() == null) ? "0" : (from d in list
                                                                                                    where d.NamaKreteria.Contains("Harga")
                                                                                                    select d).FirstOrDefault<VWPembobotanPengadaan>().Bobot.ToString();
                cx.ReplaceText("{harga}", text + "%", false, RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                string str5 = ((from d in list
                                where d.NamaKreteria.Contains("Teknis")
                                select d).FirstOrDefault<VWPembobotanPengadaan>() == null) ? "0" : (from d in list
                                                                                                    where d.NamaKreteria.Contains("Teknis")
                                                                                                    select d).FirstOrDefault<VWPembobotanPengadaan>().Bobot.ToString();
                cx.ReplaceText("{teknis}", str5 + "%", false, RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                List<VWRekananPenilaian> source = this._repository.getKandidatPengadaan2(Id, base.UserId());
                Table table = cx.AddTable(4, source.Count<VWRekananPenilaian>() + 3);
                Border BlankBorder = new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.Black);
                table.SetBorder(TableBorderType.Bottom, BlankBorder);
                table.SetBorder(TableBorderType.Left, BlankBorder);
                table.SetBorder(TableBorderType.Right, BlankBorder);
                table.SetBorder(TableBorderType.Top, BlankBorder);
                table.SetBorder(TableBorderType.InsideV, BlankBorder);
                table.SetBorder(TableBorderType.InsideH, BlankBorder);

                table.Rows[0].Cells[0].Paragraphs.First<Paragraph>().Append("No");
                table.Rows[0].Cells[0].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                table.Rows[0].Cells[0].Width = 10.0;
                table.Rows[0].Cells[1].Paragraphs.First<Paragraph>().Append("Faktor");
                table.Rows[0].Cells[1].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                table.Rows[0].Cells[2].Paragraphs.First<Paragraph>().Append("Bobot");
                table.Rows[0].Cells[2].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                int num = 3;
                foreach (VWRekananPenilaian penilaian in source)
                {
                    table.Rows[0].Cells[num].Paragraphs.First<Paragraph>().Append("Nilai " + penilaian.NamaVendor);
                    table.Rows[0].Cells[num].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                    num++;
                }
                table.Rows[1].Cells[0].Paragraphs.First<Paragraph>().Append("1");
                table.Rows[1].Cells[0].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                table.Rows[1].Cells[0].Width = 10.0;
                table.Rows[1].Cells[1].Paragraphs.First<Paragraph>().Append("Teknis");
                table.Rows[1].Cells[1].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                table.Rows[1].Cells[2].Paragraphs.First<Paragraph>().Append(str5);
                table.Rows[1].Cells[2].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                num = 3;
                foreach (VWRekananPenilaian penilaian in source)
                {
                    VWPembobotanPengadaanVendor vendor = (from d in this._repository.getPembobtanPengadaanVendor(Id, penilaian.VendorId.Value, base.UserId())
                                                          where d.NamaKreteria.Contains("Teknis")
                                                          select d).FirstOrDefault<VWPembobotanPengadaanVendor>();
                    table.Rows[1].Cells[num].Paragraphs.First<Paragraph>().Append((vendor == null) ? "-" : vendor.Nilai.Value.ToString());
                    table.Rows[1].Cells[num].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                    num++;
                }
                table.Rows[2].Cells[0].Paragraphs.First<Paragraph>().Append("2");
                table.Rows[2].Cells[0].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                table.Rows[2].Cells[0].Width = 10.0;
                table.Rows[2].Cells[1].Paragraphs.First<Paragraph>().Append("Harga");
                table.Rows[2].Cells[1].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                table.Rows[2].Cells[2].Paragraphs.First<Paragraph>().Append(text);
                table.Rows[2].Cells[2].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                num = 3;
                foreach (VWRekananPenilaian penilaian in source)
                {
                    VWPembobotanPengadaanVendor vendor2 = (from d in this._repository.getPembobtanPengadaanVendor(Id, penilaian.VendorId.Value, base.UserId())
                                                           where d.NamaKreteria.Contains("Harga")
                                                           select d).FirstOrDefault<VWPembobotanPengadaanVendor>();
                    table.Rows[2].Cells[num].Paragraphs.First<Paragraph>().Append((vendor2 == null) ? "-" : vendor2.Nilai.Value.ToString());
                    table.Rows[2].Cells[num].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                    num++;
                }
                table.Rows[3].Cells[0].Paragraphs.First<Paragraph>().Append("");
                table.Rows[3].Cells[0].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                table.Rows[3].Cells[0].Width = 10.0;
                table.Rows[3].Cells[1].Paragraphs.First<Paragraph>().Append("");
                table.Rows[3].Cells[1].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                table.Rows[3].Cells[2].Paragraphs.First<Paragraph>().Append("Score");
                table.Rows[3].Cells[2].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                num = 3;
                foreach (VWRekananPenilaian penilaian in source)
                {
                    List<VWPembobotanPengadaanVendor> list3 = this._repository.getPembobtanPengadaanVendor(Id, penilaian.VendorId.Value, base.UserId());
                    int num2 = ((from d in list3
                                 where d.NamaKreteria.Contains("Harga")
                                 select d).FirstOrDefault<VWPembobotanPengadaanVendor>() == null) ? 0 : (from d in list3
                                                                                                         where d.NamaKreteria.Contains("Harga")
                                                                                                         select d).FirstOrDefault<VWPembobotanPengadaanVendor>().Nilai.Value;
                    int num3 = ((from d in list3
                                 where d.NamaKreteria.Contains("Harga")
                                 select d).FirstOrDefault<VWPembobotanPengadaanVendor>() == null) ? 0 : (from d in list3
                                                                                                         where d.NamaKreteria.Contains("Harga")
                                                                                                         select d).FirstOrDefault<VWPembobotanPengadaanVendor>().Bobot.Value;
                    int num4 = ((from d in list3
                                 where d.NamaKreteria.Contains("Teknis")
                                 select d).FirstOrDefault<VWPembobotanPengadaanVendor>() == null) ? 0 : (from d in list3
                                                                                                         where d.NamaKreteria.Contains("Teknis")
                                                                                                         select d).FirstOrDefault<VWPembobotanPengadaanVendor>().Nilai.Value;
                    int num5 = ((from d in list3
                                 where d.NamaKreteria.Contains("Teknis")
                                 select d).FirstOrDefault<VWPembobotanPengadaanVendor>() == null) ? 0 : (from d in list3
                                                                                                         where d.NamaKreteria.Contains("Teknis")
                                                                                                         select d).FirstOrDefault<VWPembobotanPengadaanVendor>().Bobot.Value;
                    int num6 = (num2 * num3) / 100;
                    int num7 = (num4 * num5) / 100;
                    int num8 = num6 + num7;
                    table.Rows[3].Cells[num].Paragraphs.First<Paragraph>().Append(num8.ToString());
                    table.Rows[3].Cells[num].Paragraphs.First<Paragraph>().FontSize(11.0).Font(new FontFamily("Calibri"));
                    num++;
                }
                table.Alignment = Alignment.center;
                //using (IEnumerator<Paragraph> enumerator2 = cx.Paragraphs.GetEnumerator())
                //{
                //    while (enumerator2.MoveNext())
                //    {
                //        Action<int> action = null;
                //        Paragraph paragraph = enumerator2.Current;
                //        if (action == null)
                //        {
                //            action = delegate(int index)
                //            {
                //                paragraph.InsertTableBeforeSelf(table);
                //            };
                //        }
                //        paragraph.FindAll("{penilaian}").ForEach(action);
                //    }
                //}
                System.IO.MemoryStream ms2 = new System.IO.MemoryStream();
                DocX doc2 = DocX.Create(ms2);

                doc2.PageLayout.Orientation = Novacode.Orientation.Landscape;
                Paragraph p = doc2.InsertParagraph();
                p.Append("Lampiran").Bold();
                p.Alignment = Alignment.left;
                p.AppendLine();
                Table t = doc2.InsertTable(table);
                cx.InsertSection();
                cx.InsertDocument(doc2);
                //cx.ReplaceText("{penilaian}", "", false, RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
               
                //tambah tabel persetujuan tahapan
                var table3 = await getTablePersetujuan(pengadaan.Id, EStatusPengadaan.PENILAIAN, cx);

                table3.Alignment = Alignment.center;
                //table.AutoFit = AutoFit.Contents;

                foreach (var paragraph in cx.Paragraphs)
                {
                    paragraph.FindAll("{table3}").ForEach(index => paragraph.InsertTableBeforeSelf(table3));

                }
                cx.ReplaceText("{table3}", "");
                //end
                
                cx.SaveAs(filename);
                stream.Close();
            }
            catch
            {
                stream.Close();
            }
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            FileStream content = new FileStream(filename, FileMode.Open);
            message.Content = new StreamContent(content);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            ContentDispositionHeaderValue value2 = new ContentDispositionHeaderValue("attachment")
            {
                FileName = str2
            };
            message.Content.Headers.ContentDisposition = value2;
            return message;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task< HttpResponseMessage> BerkasPemenang(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());
            var jadwalPemenang = _repository.getPelaksanaanPemenang(Id, UserId());
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\NOTA BERSAMA Usulan Pemenang.docx";
           string outputFileName = "BA-Pemenang-" +  pengadaan.NoPengadaan.Replace("/", "-") + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;


            System.IO.MemoryStream ms2 = new System.IO.MemoryStream();
            var docM = DocX.Create(ms2);
            
            var pemenangx = _repository.getPemenangPengadaan(Id, UserId());
            foreach (var item in pemenangx)
            {
                var streamx = new FileStream(fileName, FileMode.Open);
                var BeritaAcara = _repository.getBeritaAcaraByTipeandVendor(Id, TipeBerkas.BeritaAcaraPenentuanPemenang,item.VendorId.Value, UserId());
            
                try
                {                   
                    var doc = DocX.Load(streamx);
                    var BeritaAcaraKlarifikasi = _repository.getBeritaAcaraByTipe(Id, TipeBerkas.BeritaAcaraKlarifikasi, UserId());

                    doc.ReplaceText("{pengadaan_name}", pengadaan.Judul == null ? "" : pengadaan.Judul);
                    doc.ReplaceText("{pengadaan_name_judul}", pengadaan.Judul == null ? "" : pengadaan.Judul.ToUpper());
                    doc.ReplaceText("{nomor_berita_acara}", BeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara);
                    doc.ReplaceText("{pengadaan_unit_pemohon}", pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);
                    doc.ReplaceText("{nomor_berita_acara_klarifikasi}", BeritaAcaraKlarifikasi == null ? "" : BeritaAcaraKlarifikasi.NoBeritaAcara);
                    doc.ReplaceText("{tempat_tanggal}", "...............," + (BeritaAcara == null ? "................" :
                            (BeritaAcara.tanggal.Value.Day + " " + Common.ConvertNamaBulan(BeritaAcara.tanggal.Value.Month) + " " +
                            BeritaAcara.tanggal.Value.Year)));
                    doc.ReplaceText("{tanggal_klarifikasi}", BeritaAcaraKlarifikasi == null ? "" : BeritaAcaraKlarifikasi.tanggal.Value.Day + " " + Common.ConvertNamaBulan(BeritaAcaraKlarifikasi.tanggal.Value.Month) +
                          " " + BeritaAcaraKlarifikasi.tanggal.Value.Year);

                    doc.ReplaceText("{pengadaan_jadwal_hari}", BeritaAcara == null ? "" : BeritaAcara.tanggal == null ? "" :
                           Common.ConvertHari(BeritaAcara.tanggal.Value.Day));
                    doc.ReplaceText("{pengadaan_jadwal_tanggal}", BeritaAcara == null ? "" : BeritaAcara.tanggal.Value.Day + " " + Common.ConvertNamaBulan(BeritaAcara.tanggal.Value.Month) +
                          " " + BeritaAcara.tanggal.Value.Year);



                    doc.ReplaceText("{kandidat_pemenang}", item.NamaVendor );
                    doc.ReplaceText("{total_pengadaan}",item.total==null?"": item.total.Value.ToString("C", MyConverter.formatCurrencyIndo()) );
                    docM.InsertSection();
                                        
                    docM.InsertDocument(doc); //doc.SaveAs(OutFileNama);
                    streamx.Close();
                }
                catch
                {
                    streamx.Close();
                }
            }

            //tambah tabel persetujuan tahapan
            var table3 = await getTablePersetujuan(pengadaan.Id, EStatusPengadaan.PEMENANG, docM);

            table3.Alignment = Alignment.center;
            //table.AutoFit = AutoFit.Contents;

            foreach (var paragraph in docM.Paragraphs)
            {
                paragraph.FindAll("{table3}").ForEach(index => paragraph.InsertTableBeforeSelf(table3));

            }
            docM.ReplaceText("{table3}", "");
            //end
            docM.SaveAs(OutFileNama);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage BerkasSPK(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());
            var jadwalPemenang = _repository.getPelaksanaanPemenang(Id, UserId());
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\SPK.docx";
            string outputFileName = "BA-SPK-" + pengadaan.NoPengadaan.Replace("/", "-") + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

           // var streamx = new FileStream(fileName, FileMode.Open);

           // var doc = DocX.Load(streamx);

             System.IO.MemoryStream ms2 = new System.IO.MemoryStream();
            var docM = DocX.Create(ms2);
            
            var pemenangx = _repository.getPemenangPengadaan(Id, UserId());
            foreach (var item in pemenangx)
            {
                var streamx = new FileStream(fileName, FileMode.Open);
                var BeritaAcara = _repository.getBeritaAcaraByTipeandVendor(Id, TipeBerkas.SuratPerintahKerja,item.VendorId.Value, UserId());
                
                try
                {
                    var doc = DocX.Load(streamx);
                    doc.ReplaceText("{pengadaan_name}", pengadaan.Judul == null ? "" : pengadaan.Judul);
                    doc.ReplaceText("{pengadaan_name_judul}", pengadaan.Judul == null ? "" : pengadaan.Judul.ToUpper());
                    doc.ReplaceText("{nomor_berita_acara}", BeritaAcara == null ? "" : BeritaAcara.NoBeritaAcara);
                    doc.ReplaceText("{pengadaan_unit_pemohon}", pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);
                    doc.ReplaceText("{tempat_tanggal}", "...............," + BeritaAcara == null ? "................" :
                            BeritaAcara.tanggal.Value.Day + " " + Common.ConvertNamaBulan(BeritaAcara.tanggal.Value.Month) + " " +
                            BeritaAcara.tanggal.Value.Year);

                    doc.ReplaceText("{pengadaan_jadwal_hari}", BeritaAcara.tanggal == null ? "" :
                           Common.ConvertHari(BeritaAcara.tanggal.Value.Day));
                    doc.ReplaceText("{pengadaan_jadwal_tanggal}", BeritaAcara.tanggal.Value.Day + " " + Common.ConvertNamaBulan(BeritaAcara.tanggal.Value.Month) +
                          " " + BeritaAcara.tanggal.Value.Year);


                   // var pemenang = _repository.getPemenangPengadaan(Id, UserId());
                    var vendor = _repository.GetVendorById(item.VendorId.Value);
                    doc.ReplaceText("{kandidat_pemenang}", item.NamaVendor );
                    doc.ReplaceText("{total_pengadaan}",item.total==null?"": item.total.Value.ToString("C", MyConverter.formatCurrencyIndo()) );
                    doc.ReplaceText("{alamat}",vendor.Alamat.ToString());
                    doc.ReplaceText("{terbilang}",item.total==null?"": MyConverter.Terbilang(item.total.Value.ToString()) + " Rupiah");
                   // doc.SaveAs(OutFileNama);
                    docM.InsertSection();

                    docM.InsertDocument(doc); //doc.SaveAs(OutFileNama);
                    streamx.Close();
                   // streamx.Close();
                }
                catch
                {
                    streamx.Close();
                }
            }
            docM.SaveAs(OutFileNama);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage LembarDisposisi(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());
            var jadwalPemenang = _repository.getPelaksanaanPemenang(Id, UserId());
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\LEMBAR DISPOSISI.docx";

            string outputFileName = "Lembar-Disposisi-" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var streamx = new FileStream(fileName, FileMode.Open);

            var doc = DocX.Load(streamx);

            try
            {
                doc.ReplaceText("{judul_pengadaan}", pengadaan.Judul == null ? "" : pengadaan.Judul);
                doc.ReplaceText("{no_pengadaan}", pengadaan.NoPengadaan == null ? "" : pengadaan.NoPengadaan);
                doc.ReplaceText("{unit_pemohon}", pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);
                doc.ReplaceText("{region}", pengadaan.Region == null ? "" : pengadaan.Region);

                if (jadwalPemenang != null)
                {
                    doc.ReplaceText("{tanggal}", jadwalPemenang.Mulai.Value.Day.ToString() +
                        " " + Common.ConvertNamaBulan(jadwalPemenang.Mulai.Value.Month) +
                        " " + jadwalPemenang.Mulai.Value.Year.ToString());
                }
                else
                {
                    doc.ReplaceText("{tanggal}", "");
                }


                doc.SaveAs(OutFileNama);
                streamx.Close();
            }
            catch
            {
                streamx.Close();
            }
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage CetakRKSKlarfikasi(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\Klarifikasi-Pemenang.docx";

            string outputFileName = "Cetak-RKS-Klarifikasi" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var streamx = new FileStream(fileName, FileMode.Open);

            var doc = DocX.Load(streamx);//.Create(OutFileNama);
            doc.ReplaceText("{pengadaan_name_judul}", pengadaan.Judul == null ? "" : pengadaan.Judul.ToUpper());
            doc.ReplaceText("{nomor_berita_acara}", pengadaan.NoPengadaan == null ? "" : pengadaan.NoPengadaan);
            doc.ReplaceText("{pengadaan_unit_pemohon}", pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);


            var oVWRKSVendors = _repository.getRKSKlarifikasiPenilaian(pengadaan.Id, UserId());
            var table = doc.AddTable(oVWRKSVendors.hps.Count() + 1, oVWRKSVendors.vendors.Count + 4);
            int no = 1;

            int indexRow = 0;
            table.Rows[indexRow].Cells[0].Paragraphs.First().Append("NO");
            table.Rows[indexRow].Cells[0].Width = 10;
            table.Rows[indexRow].Cells[1].Paragraphs.First().Append("Item");
            table.Rows[indexRow].Cells[2].Paragraphs.First().Append("Jumlah");
            table.Rows[indexRow].Cells[3].Paragraphs.First().Append("Harga HPS");
            int headerCol = 4;
            foreach (var item in oVWRKSVendors.vendors)
            {
                table.Rows[indexRow].Cells[headerCol].Paragraphs.First().Append(item.nama);
                headerCol++;
            }
            indexRow++;
            var itemlast = oVWRKSVendors.hps.Last();
            foreach (var item in oVWRKSVendors.hps)
            {
                if (item.Equals(itemlast))
                {
                    table.Rows[indexRow].Cells[0].Paragraphs.First().Append("");
                    table.Rows[indexRow].Cells[0].Width = 10;
                }
                else
                {

                    if (item.jumlah > 0)
                    {
                        table.Rows[indexRow].Cells[0].Paragraphs.First().Append(no.ToString());
                        table.Rows[indexRow].Cells[0].Width = 10;
                        no++;
                    }
                    else
                    {
                        table.Rows[indexRow].Cells[0].Paragraphs.First().Append("");
                        table.Rows[indexRow].Cells[0].Width = 10;
                    }
                }
                //Regex example #1 "<.*?>"
                string dekripsi = Regex.Replace(item.item, @"<.*?>", string.Empty);
                //Regex example #2
                // string result2 = Regex.Replace(dekripsi, @"<[^>].+?>", "");
                table.Rows[indexRow].Cells[1].Paragraphs.First().Append(dekripsi);
                table.Rows[indexRow].Cells[2].Paragraphs.First().Append(item.jumlah==null?"":item.jumlah.Value.ToString("C",MyConverter.formatCurrencyIndoTanpaSymbol()));
                table.Rows[indexRow].Cells[3].Paragraphs.First().Append(item.harga == null ? "" : item.harga.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                int nexCol = 4;
                foreach (var itemx in oVWRKSVendors.vendors)
                {
                    if (itemx.items.Where(d => d.Id == item.Id) != null)
                    {
                        table.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append(itemx.items.Where(d => d.Id == item.Id).FirstOrDefault() == null ? "" : itemx.items.Where(d => d.Id == item.Id).FirstOrDefault().harga.Value.ToString("C", MyConverter.formatCurrencyIndo()));

                    }
                    else table.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append("");

                    nexCol++;
                }

                indexRow++;
            }
            // Insert table at index where tag #TABLE# is in document.
            //doc.InsertTable(table);
            foreach (var paragraph in doc.Paragraphs)
            {
                paragraph.FindAll("{tabel}").ForEach(index => paragraph.InsertTableAfterSelf((table)));
            }
            //Remove tag
            doc.ReplaceText("{tabel}", "");

            doc.SaveAs(OutFileNama);
            streamx.Close();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage CetakRKSKlarfikasiXls(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());
            string outputFileName = "Cetak-RKS-Klarifikasi" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".xls";

            var Judul = pengadaan.Judul == null ? "" : pengadaan.Judul;
            var UnitPemohon = pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon;


            var oVWRKSVendors = _repository.getRKSKlarifikasiPenilaian(pengadaan.Id, UserId());
                         
             var ms = new System.IO.MemoryStream();
             try
             {
                 using (var sl = new SpreadsheetLight.SLDocument())
                 {
                     sl.SetCellValue(1, 1, "Proyek");
                     sl.SetCellValue(1, 2, UnitPemohon);
                     sl.SetCellValue(2, 1, "Judul");
                     sl.SetCellValue(2, 2, Judul);
                     sl.SetCellValue(3, 1, "Penawaran Harga Klarifikasi dan Negoisasi Rekanan sebagai berikut:");
                     var rowNum = 4;
                     //write header
                     sl.SetCellValue(rowNum, 1, "No");
                     sl.SetCellValue(rowNum, 2, "Item");
                     sl.SetCellValue(rowNum, 3, "Jumlah");
                     sl.SetCellValue(rowNum, 4, "Harga HPS");

                     int headerCol = 5;
                     foreach (var item in oVWRKSVendors.vendors)
                     {
                         sl.SetCellValue(rowNum, headerCol, item.nama);
                         headerCol++;
                     }
                     rowNum++;
                     //write data
                     var itemlast = oVWRKSVendors.hps.Last();
                     int no = 1;
                     foreach (var item in oVWRKSVendors.hps)
                     {
                         if (item.Equals(itemlast))
                         {
                             sl.SetCellValue(rowNum, 1, "");
                         }
                         else
                         {
                             if (item.jumlah > 0)
                             {
                                 sl.SetCellValue(rowNum, 1, no);
                                 no++;
                             }
                             else
                             {
                                 sl.SetCellValue(rowNum, 1, "");
                             }
                         }
                         string dekripsi = Regex.Replace(item.item, @"<.*?>", string.Empty);
                         sl.SetCellValue(rowNum, 2, dekripsi);
                         if (item.harga==null)sl.SetCellValue(rowNum, 3, "");
                         else sl.SetCellValue(rowNum, 3,item.harga.Value);
                         if (item.jumlah == null) sl.SetCellValue(rowNum, 4, "");
                         else sl.SetCellValue(rowNum, 4, item.jumlah.Value);
                         int nextCol = 5;
                         foreach (var itemx in oVWRKSVendors.vendors)
                         {
                             if (itemx.items.Where(d => d.Id == item.Id) != null)
                             {
                                var itemxx= itemx.items.Where(d => d.Id == item.Id).FirstOrDefault();
                                if (itemxx == null)
                                {
                                    sl.SetCellValue(rowNum, nextCol, "");
                                }
                                else
                                {
                                    if (itemxx.harga == null) sl.SetCellValue(rowNum, nextCol, "");
                                    else sl.SetCellValue(rowNum, nextCol, itemxx.harga.Value);
                                }
                             }
                             else sl.SetCellValue(rowNum, nextCol, "");

                             nextCol++;
                         }
                         rowNum++;
                     }
                     sl.SetColumnWidth(1, 10.0);

                     sl.SaveAs(ms);
                 }
             }
             catch { }

             ms.Position = 0;
             HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
             result.Content = new StreamContent(ms);
             result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

             result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
             {
                 FileName = outputFileName
             };
             return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage CetakRKSKlarfikasiVendor(Guid Id, int VendorId)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\Klarifikasi-Pemenang.docx";

            string outputFileName = "Cetak-RKS-Klarifikasi-" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var streamx = new FileStream(fileName, FileMode.Open);

            var doc = DocX.Load(streamx);//.Create(OutFileNama);
            doc.ReplaceText("{pengadaan_name_judul}", pengadaan.Judul == null ? "" : pengadaan.Judul.ToUpper());
            doc.ReplaceText("{nomor_berita_acara}", pengadaan.NoPengadaan == null ? "" : pengadaan.NoPengadaan);
            doc.ReplaceText("{pengadaan_unit_pemohon}", pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);


            var oVWRKSVendors = _repository.getRKSKlarifikasiPenilaianVendor2(pengadaan.Id, UserId(), VendorId);
            var table = doc.AddTable(oVWRKSVendors.hps.Count() + 1, oVWRKSVendors.vendors.Count + 4);
            int no = 1;

            int indexRow = 0;
            table.Rows[indexRow].Cells[0].Paragraphs.First().Append("NO");
            table.Rows[indexRow].Cells[0].Width = 10;
            table.Rows[indexRow].Cells[1].Paragraphs.First().Append("Item");
            table.Rows[indexRow].Cells[2].Paragraphs.First().Append("Jumlah");
            table.Rows[indexRow].Cells[3].Paragraphs.First().Append("Harga HPS");
            int headerCol = 4;
            foreach (var item in oVWRKSVendors.vendors)
            {
                table.Rows[indexRow].Cells[headerCol].Paragraphs.First().Append(item.nama);
                headerCol++;
            }
            indexRow++;
            var itemlast = oVWRKSVendors.hps.Last();
            foreach (var item in oVWRKSVendors.hps)
            {
                if (item.Equals(itemlast))
                {
                    table.Rows[indexRow].Cells[0].Paragraphs.First().Append("");
                    table.Rows[indexRow].Cells[0].Width = 10;
                }
                else
                {

                    if (item.jumlah > 0)
                    {
                        table.Rows[indexRow].Cells[0].Paragraphs.First().Append(no.ToString());
                        table.Rows[indexRow].Cells[0].Width = 10;
                        no++;
                    }
                    else
                    {
                        table.Rows[indexRow].Cells[0].Paragraphs.First().Append("");
                        table.Rows[indexRow].Cells[0].Width = 10;
                    }
                }
                //Regex example #1 "<.*?>"
                string dekripsi = Regex.Replace(item.item, @"<.*?>", string.Empty);
                //Regex example #2
                // string result2 = Regex.Replace(dekripsi, @"<[^>].+?>", "");
                table.Rows[indexRow].Cells[1].Paragraphs.First().Append(dekripsi);

                table.Rows[indexRow].Cells[2].Paragraphs.First().Append(item.harga == null ? "" : item.harga.Value.ToString("C", MyConverter.formatCurrencyIndo()));

                table.Rows[indexRow].Cells[3].Paragraphs.First().Append(item.jumlah == null ? "" : item.jumlah.Value.ToString("C",MyConverter.formatCurrencyIndoTanpaSymbol()));                

                int nexCol = 4;
                foreach (var itemx in oVWRKSVendors.vendors)
                {
                    if (itemx.items.Where(d => d.Id == item.Id) != null)
                    {
                        table.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append(itemx.items.Where(d => d.Id == item.Id).FirstOrDefault() == null ? "" : itemx.items.Where(d => d.Id == item.Id).FirstOrDefault().harga.Value.ToString("C", MyConverter.formatCurrencyIndo()));

                    }
                    else table.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append("");

                    nexCol++;
                }

                indexRow++;
            }
            // Insert table at index where tag #TABLE# is in document.
            //doc.InsertTable(table);
            foreach (var paragraph in doc.Paragraphs)
            {
                paragraph.FindAll("{tabel}").ForEach(index => paragraph.InsertTableAfterSelf((table)));
            }
            //Remove tag
            doc.ReplaceText("{tabel}", "");

            doc.SaveAs(OutFileNama);
            streamx.Close();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage CetakRKSXLSKlarfikasiVendor(Guid Id, int VendorId)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());

            string outputFileName = "Cetak-RKS-Klarifikasi-" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".xlsx";

            var Judul = pengadaan.Judul == null ? "" : pengadaan.Judul.ToUpper();
            var NoPengadaan= pengadaan.NoPengadaan == null ? "" : pengadaan.NoPengadaan;
            var UnitPemohon= pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon;


            var oVWRKSVendors = _repository.getRKSKlarifikasiPenilaianVendor2(pengadaan.Id, UserId(), VendorId);

             
             var ms = new System.IO.MemoryStream();
             try
             {
                 using (var sl = new SpreadsheetLight.SLDocument())
                 {
                     sl.SetCellValue(1, 1, "Proyek");
                     sl.SetCellValue(1, 2, UnitPemohon);
                     sl.SetCellValue(2, 1, "Judul");
                     sl.SetCellValue(2, 2, Judul);
                     sl.SetCellValue(3, 1, "Penawaran Harga Klarifikasi dan Negoisasi Rekanan sebagai berikut:");
                     var rowNum = 4;
                     //write header
                     sl.SetCellValue(rowNum, 1, "No");
                     sl.SetCellValue(rowNum, 2, "Item");
                     sl.SetCellValue(rowNum, 3, "Jumlah");
                     sl.SetCellValue(rowNum, 4, "Harga HPS");

                     int headerCol = 5;
                     foreach (var item in oVWRKSVendors.vendors)
                     {
                         sl.SetCellValue(rowNum, headerCol, item.nama);
                         headerCol++;
                     }
                     rowNum++;
                     //write data
                     var itemlast = oVWRKSVendors.hps.Last();
                     int no = 1;
                     foreach (var item in oVWRKSVendors.hps)
                     {
                         if (item.Equals(itemlast))
                         {
                             sl.SetCellValue(rowNum, 1, "");
                         }
                         else
                         {
                             if (item.jumlah > 0)
                             {
                                 sl.SetCellValue(rowNum, 1, no);
                                 no++;
                             }
                             else
                             {
                                 sl.SetCellValue(rowNum, 1, "");
                             }
                         }
                         string dekripsi = Regex.Replace(item.item, @"<.*?>", string.Empty);
                         sl.SetCellValue(rowNum, 2, dekripsi);
                         if (item.harga==null)sl.SetCellValue(rowNum, 3, "");
                         else sl.SetCellValue(rowNum, 3,item.harga.Value);
                         if (item.jumlah == null) sl.SetCellValue(rowNum, 4, "");
                         else sl.SetCellValue(rowNum, 4, item.jumlah.Value);
                         int nextCol = 5;
                         foreach (var itemx in oVWRKSVendors.vendors)
                         {
                             if (itemx.items.Where(d => d.Id == item.Id) != null)
                             {
                                var itemxx= itemx.items.Where(d => d.Id == item.Id).FirstOrDefault();
                                if (itemxx == null)
                                {
                                    sl.SetCellValue(rowNum, nextCol, "");
                                }
                                else
                                {
                                    if (itemxx.harga == null) sl.SetCellValue(rowNum, nextCol, "");
                                    else sl.SetCellValue(rowNum, nextCol, itemxx.harga.Value);
                                }
                             }
                             else sl.SetCellValue(rowNum, nextCol, "");

                             nextCol++;
                         }
                         rowNum++;
                     }
                     sl.SetColumnWidth(1, 10.0);

                     sl.SaveAs(ms);
                 }
             }
             catch { }

             ms.Position = 0;
             HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
             //  var stream = new FileStream(ms, FileMode.Open);
             result.Content = new StreamContent(ms);
             //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
             result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

             result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
             {
                 FileName = outputFileName
             };
             return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage CetakRKSPenilaianAll(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\Klarifikasi-Pemenang.docx";

            string outputFileName = "Cetak-RKS-Penilaian-" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var streamx = new FileStream(fileName, FileMode.Open);

            var doc = DocX.Load(streamx);//.Create(OutFileNama);
            doc.ReplaceText("{pengadaan_name_judul}", pengadaan.Judul == null ? "" : pengadaan.Judul.ToUpper());
            doc.ReplaceText("{nomor_berita_acara}", pengadaan.NoPengadaan == null ? "" : pengadaan.NoPengadaan);
            doc.ReplaceText("{pengadaan_unit_pemohon}", pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);


            var oVWRKSVendors = _repository.getRKSPenilaian2Report(pengadaan.Id, UserId());
            var table = doc.AddTable(oVWRKSVendors.hps.Count() + 1, oVWRKSVendors.vendors.Count + 4);
            int no = 1;

            int indexRow = 0;
            table.Rows[indexRow].Cells[0].Paragraphs.First().Append("NO");
            table.Rows[indexRow].Cells[1].Paragraphs.First().Append("Item");
            table.Rows[indexRow].Cells[2].Paragraphs.First().Append("Jumlah");
            table.Rows[indexRow].Cells[3].Paragraphs.First().Append("Harga HPS");
            int headerCol = 4;
            foreach (var item in oVWRKSVendors.vendors)
            {
                table.Rows[indexRow].Cells[headerCol].Paragraphs.First().Append(item.nama);
                headerCol++;
            }
            indexRow++;
            var itemlast = oVWRKSVendors.hps.Last();
            foreach (var item in oVWRKSVendors.hps)
            {
                if (item.Equals(itemlast))
                {
                    table.Rows[indexRow].Cells[0].Paragraphs.First().Append("");
                }
                else
                {

                    if (item.jumlah > 0)
                    {
                        table.Rows[indexRow].Cells[0].Paragraphs.First().Append(no.ToString());
                        no++;
                    }
                    else
                    {
                        table.Rows[indexRow].Cells[0].Paragraphs.First().Append("");
                    }
                }
                //Regex example #1 "<.*?>"
                string dekripsi = Regex.Replace(item.item, @"<.*?>", string.Empty);
                //Regex example #2
                // string result2 = Regex.Replace(dekripsi, @"<[^>].+?>", "");
                table.Rows[indexRow].Cells[1].Paragraphs.First().Append(dekripsi);
                table.Rows[indexRow].Cells[2].Paragraphs.First().Append(item.jumlah==null?"":item.jumlah.Value.ToString("C",MyConverter.formatCurrencyIndoTanpaSymbol()));
                table.Rows[indexRow].Cells[3].Paragraphs.First().Append(item.harga == null ? "" : item.harga.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                int nexCol = 4;
                foreach (var itemx in oVWRKSVendors.vendors)
                {
                    if (itemx.items.Where(d => d.Id == item.Id) != null)
                    {
                        table.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append(itemx.items.Where(d => d.Id == item.Id).FirstOrDefault() == null ? "" : itemx.items.Where(d => d.Id == item.Id).FirstOrDefault().harga.Value.ToString("C", MyConverter.formatCurrencyIndo()));

                    }
                    else table.Rows[indexRow].Cells[nexCol].Paragraphs.First().Append("");

                    nexCol++;
                }

                indexRow++;
            }
            // Insert table at index where tag #TABLE# is in document.
            //doc.InsertTable(table);
            foreach (var paragraph in doc.Paragraphs)
            {
                paragraph.FindAll("{tabel}").ForEach(index => paragraph.InsertTableAfterSelf((table)));
            }
            //Remove tag
            doc.ReplaceText("{tabel}", "");

            doc.SaveAs(OutFileNama);
            streamx.Close();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage CetakRKSPenilaianAllXls(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());

            string outputFileName = "Cetak-RKS-Penilaian-" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".xlsx";

            var Judul = pengadaan.Judul == null ? "" : pengadaan.Judul.ToUpper();
            var NoPengadaan = pengadaan.NoPengadaan == null ? "" : pengadaan.NoPengadaan;
            var UnitPemohon = pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon;


            var oVWRKSVendors = _repository.getRKSPenilaian2Report(pengadaan.Id, UserId());


            var ms = new System.IO.MemoryStream();
            try
            {
                using (var sl = new SpreadsheetLight.SLDocument())
                {
                    sl.SetCellValue(1, 1, "Proyek");
                    sl.SetCellValue(1, 2, UnitPemohon);
                    sl.SetCellValue(2, 1, "Judul");
                    sl.SetCellValue(2, 2, Judul);
                    sl.SetCellValue(3, 1, "Penawaran Harga Klarifikasi dan Negoisasi Rekanan sebagai berikut:");
                    var rowNum = 4;
                    //write header
                    sl.SetCellValue(rowNum, 1, "No");
                    sl.SetCellValue(rowNum, 2, "Item");
                    sl.SetCellValue(rowNum, 3, "Jumlah");
                    sl.SetCellValue(rowNum, 4, "Harga HPS");

                    int headerCol = 5;
                    foreach (var item in oVWRKSVendors.vendors)
                    {
                        sl.SetCellValue(rowNum, headerCol, item.nama);
                        headerCol++;
                    }
                    rowNum++;
                    //write data
                    var itemlast = oVWRKSVendors.hps.Last();
                    int no = 1;
                    foreach (var item in oVWRKSVendors.hps)
                    {
                        if (item.Equals(itemlast))
                        {
                            sl.SetCellValue(rowNum, 1, "");
                        }
                        else
                        {
                            if (item.jumlah > 0)
                            {
                                sl.SetCellValue(rowNum, 1, no);
                                no++;
                            }
                            else
                            {
                                sl.SetCellValue(rowNum, 1, "");
                            }
                        }
                        string dekripsi = Regex.Replace(item.item, @"<.*?>", string.Empty);
                        sl.SetCellValue(rowNum, 2, dekripsi);
                        if (item.harga == null) sl.SetCellValue(rowNum, 3, "");
                        else sl.SetCellValue(rowNum, 3, item.harga.Value);
                        if (item.jumlah == null) sl.SetCellValue(rowNum, 4, "");
                        else sl.SetCellValue(rowNum, 4, item.jumlah.Value);
                        int nextCol = 5;
                        foreach (var itemx in oVWRKSVendors.vendors)
                        {
                            if (itemx.items.Where(d => d.Id == item.Id) != null)
                            {
                                var itemxx = itemx.items.Where(d => d.Id == item.Id).FirstOrDefault();
                                if (itemxx == null)
                                {
                                    sl.SetCellValue(rowNum, nextCol, "");
                                }
                                else
                                {
                                    if (itemxx.harga == null) sl.SetCellValue(rowNum, nextCol, "");
                                    else sl.SetCellValue(rowNum, nextCol, itemxx.harga.Value);
                                }
                            }
                            else sl.SetCellValue(rowNum, nextCol, "");

                            nextCol++;
                        }
                        rowNum++;
                    }
                    sl.SetColumnWidth(1, 10.0);

                    sl.SaveAs(ms);
                }
            }
            catch { }

            ms.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            //  var stream = new FileStream(ms, FileMode.Open);
            result.Content = new StreamContent(ms);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };
            return result;
        }

        //Buat Word Create RKS New
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage CetakHPSNew(Guid Id)
        {
            var rkstemplate = _rksrepo.getRksTemplate(Id);
            //var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\template-hps-new.docx";

            string outputFileName = "Cetak-HPS" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var streamx = new FileStream(fileName, FileMode.Open);
            try
            {
                var doc = DocX.Load(streamx);//.Create(OutFileNama);
                doc.ReplaceText("{judul}", rkstemplate.Title);
                doc.ReplaceText("{deskripsi}", rkstemplate.Description);
                doc.ReplaceText("{klasifikasi}", rkstemplate.Klasifikasi.ToString());
                doc.ReplaceText("{region}", rkstemplate.Region);

                var oVWRKSDetail = rkstemplate.RKSDetailTemplate;
                var table = doc.AddTable(oVWRKSDetail.Count() + 1, 7);


                int indexRow = 0;
                table.Rows[indexRow].Cells[0].Paragraphs.First().Append("Nama");
                table.Rows[indexRow].Cells[1].Paragraphs.First().Append("Item");
                table.Rows[indexRow].Cells[2].Paragraphs.First().Append("Satuan");
                table.Rows[indexRow].Cells[3].Paragraphs.First().Append("Jumlah");
                table.Rows[indexRow].Cells[3].Width = 10;
                table.Rows[indexRow].Cells[4].Paragraphs.First().Append("Hps Satuan");
                table.Rows[indexRow].Cells[5].Paragraphs.First().Append("Total");
                table.Rows[indexRow].Cells[6].Paragraphs.First().Append("Keterangan");
                indexRow++;
                decimal subtotal = 0;
                decimal totalall = 0;
                foreach (var item in oVWRKSDetail)
                {
                    if (item.level == 0)
                    {
                        table.Rows[indexRow].Cells[0].Paragraphs.First().Append(item.judul == null ? "" : item.judul.ToString());

                    }
                    else if (item.level == 1)
                    {
                        table.Rows[indexRow].Cells[1].Paragraphs.First().Append(item.item == null ? "" : Regex.Replace(item.item.ToString(), @"<.*?>", string.Empty));
                        table.Rows[indexRow].Cells[2].Paragraphs.First().Append(item.satuan == null ? "" : item.satuan.ToString());
                        table.Rows[indexRow].Cells[3].Paragraphs.First().Append(item.jumlah == null ? "" : item.jumlah.Value.ToString());
                        table.Rows[indexRow].Cells[3].Width = 10;
                        table.Rows[indexRow].Cells[4].Paragraphs.First().Append(item.hps == null ? "" : item.hps.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                        decimal? total = item.jumlah * item.hps;
                        table.Rows[indexRow].Cells[5].Paragraphs.First().Append(total == null ? "" : total.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                        table.Rows[indexRow].Cells[6].Paragraphs.First().Append(item.keterangan);
                        subtotal = subtotal + total.Value;
                        totalall = totalall + total.Value;
                    }
                    else if (item.level == 2)
                    {
                        table.Rows[indexRow].Cells[4].Paragraphs.First().Append("Sub Total");
                        table.Rows[indexRow].Cells[5].Paragraphs.First().Append(subtotal.ToString("C", MyConverter.formatCurrencyIndo()));
                        subtotal = 0;
                    }
                    indexRow++;
                }

                doc.ReplaceText("{total}", totalall.ToString("C", MyConverter.formatCurrencyIndo()));

                // Insert table at index where tag #TABLE# is in document.
                //doc.InsertTable(table);
                foreach (var paragraph in doc.Paragraphs)
                {
                    paragraph.FindAll("{tabel}").ForEach(index => paragraph.InsertTableAfterSelf((table)));
                }
                //Remove tag
                doc.ReplaceText("{tabel}", "");

                doc.SaveAs(OutFileNama);
                streamx.Close();
            }
            catch
            {
                streamx.Close();
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }

        // Create RKS Excel New
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage CetakHPSXLSNew(Guid Id)
        {
            var rkstemplate = _rksrepo.getRksTemplate(Id);
            var spc = new System.Data.DataTable("Jimbis");
            string outputFileName = "Cetak-HPS" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".xlsx";
            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var ms = new System.IO.MemoryStream();
            try
            {
                var oVWRKSDetail = rkstemplate.RKSDetailTemplate;
                
                using (var sl = new SpreadsheetLight.SLDocument())
                {
                    sl.SetCellValue(1, 1, "Judul");
                    sl.SetCellValue(1, 2, rkstemplate.Title);
                    sl.SetCellValue(2, 1, "Deskripsi");
                    sl.SetCellValue(2, 2, rkstemplate.Description);
                    sl.SetCellValue(3, 1, "Klasifikasi");
                    sl.SetCellValue(3, 2, rkstemplate.Klasifikasi.ToString());
                    sl.SetCellValue(4, 1, "Region");
                    sl.SetCellValue(4, 2, rkstemplate.Region);

                    var rowNum = 6;
                    //write header
                    sl.SetCellValue(rowNum, 1, "Nama");
                    sl.SetCellValue(rowNum, 2, "Item");
                    sl.SetCellValue(rowNum, 3, "Satuan");
                    sl.SetCellValue(rowNum, 4, "Jumlah");
                    sl.SetCellValue(rowNum, 5, "Hps");
                    sl.SetCellValue(rowNum, 6, "Total");
                    sl.SetCellValue(rowNum, 7, "Keterangan");
                    rowNum++;
                    //write data
                    decimal subtotal = 0;
                    decimal totalall = 0;
                    foreach (var item in oVWRKSDetail)
                    {
                        if (item.level == 0)
                        {
                            sl.SetCellValue(rowNum, 1, (item.judul));
                        }
                        else if (item.level == 1)
                        {
                            sl.SetCellValue(rowNum, 2, Regex.Replace(item.item.ToString(), @"<.*?>", string.Empty));
                            sl.SetCellValue(rowNum, 3, item.satuan);
                            if (item.jumlah == null) sl.SetCellValue(rowNum, 4, "");
                            else sl.SetCellValue(rowNum, 4, item.jumlah.Value);
                            if (item.hps == null) sl.SetCellValue(rowNum, 5, "");
                            else sl.SetCellValue(rowNum, 5, item.hps.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                            decimal? total = item.jumlah * item.hps;
                            if (total == null) sl.SetCellValue(rowNum, 6, "");
                            else sl.SetCellValue(rowNum, 6, total.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                            sl.SetCellValue(rowNum, 7, item.keterangan);
                            subtotal = subtotal + total.Value;
                            totalall = totalall + total.Value;
                        }
                        else if (item.level == 2)
                        {
                            sl.SetCellValue(rowNum, 5, "Sub Total");
                            sl.SetCellValue(rowNum, 6, subtotal.ToString("C", MyConverter.formatCurrencyIndo()));
                            subtotal = 0;
                        }
                        rowNum++;
                    }

                    sl.SetCellValue(5, 1, "Total HPS");
                    sl.SetCellValue(5, 2, totalall.ToString("C", MyConverter.formatCurrencyIndo()));

                    //add filter
                    sl.SetColumnWidth(4, 12.0);
                    sl.SetColumnWidth(5, 30.0);
                    sl.SetColumnWidth(6, 30.0);
                    sl.SaveAs(ms);
                }

            }
            catch
            {

            }

            ms.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            //  var stream = new FileStream(ms, FileMode.Open);
            result.Content = new StreamContent(ms);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };
            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage CetakHPS(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            //var jadwalKlarifikasi = _repository.getPelaksanaanKlarifikasi(Id, UserId());
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\template-hps.docx";

            string outputFileName = "Cetak-HPS" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var streamx = new FileStream(fileName, FileMode.Open);
            try
            {
                var doc = DocX.Load(streamx);//.Create(OutFileNama);
                doc.ReplaceText("{pengadaan_name_judul}", pengadaan.Judul == null ? "" : pengadaan.Judul);
                doc.ReplaceText("{nomor_berita_acara}", pengadaan.NoPengadaan == null ? "" : pengadaan.NoPengadaan);
                doc.ReplaceText("{pengadaan_unit_pemohon}", pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);

                var oVWRKSDetail = _repository.getRKSDetails(pengadaan.Id, UserId());
                var table = doc.AddTable(oVWRKSDetail.Count() + 1, 7);

                int indexRow = 0;
                table.Rows[indexRow].Cells[0].Paragraphs.First().Append("Nama");
                table.Rows[indexRow].Cells[1].Paragraphs.First().Append("Item");
                table.Rows[indexRow].Cells[2].Paragraphs.First().Append("Satuan");
                table.Rows[indexRow].Cells[3].Paragraphs.First().Append("Jumlah");
                table.Rows[indexRow].Cells[3].Width = 10;
                table.Rows[indexRow].Cells[4].Paragraphs.First().Append("Hps Satuan");
                table.Rows[indexRow].Cells[5].Paragraphs.First().Append("Total");
                table.Rows[indexRow].Cells[6].Paragraphs.First().Append("Keterangan");
                indexRow++;
                decimal subtotal = 0;
                decimal totalall = 0;
                foreach (var item in oVWRKSDetail)
                {
                    if (item.level == 0)
                    {
                        table.Rows[indexRow].Cells[0].Paragraphs.First().Append(item.judul == null ? "" : item.judul.ToString());

                    }
                    else if (item.level == 1)
                    {
                        table.Rows[indexRow].Cells[1].Paragraphs.First().Append(item.item == null ? "" : Regex.Replace(item.item.ToString(), @"<.*?>", string.Empty));
                        table.Rows[indexRow].Cells[2].Paragraphs.First().Append(item.satuan == null ? "" : item.satuan.ToString());
                        table.Rows[indexRow].Cells[3].Paragraphs.First().Append(item.jumlah == null ? "" : item.jumlah.Value.ToString());
                        table.Rows[indexRow].Cells[3].Width = 10;
                        table.Rows[indexRow].Cells[4].Paragraphs.First().Append(item.hps == null ? "" : item.hps.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                        decimal? total = item.jumlah * item.hps;
                        table.Rows[indexRow].Cells[5].Paragraphs.First().Append(total == null ? "" : total.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                        table.Rows[indexRow].Cells[6].Paragraphs.First().Append(item.keterangan);
                        subtotal = subtotal + total.Value;
                        totalall = totalall + total.Value;
                    }
                    else if (item.level == 2)
                    {
                        table.Rows[indexRow].Cells[4].Paragraphs.First().Append("Sub Total");
                        table.Rows[indexRow].Cells[5].Paragraphs.First().Append(subtotal.ToString("C", MyConverter.formatCurrencyIndo()));
                        subtotal = 0;
                    }
                    indexRow++;
                }
                doc.ReplaceText("{total_hps}", totalall.ToString("C", MyConverter.formatCurrencyIndo()));
                // Insert table at index where tag #TABLE# is in document.
                //doc.InsertTable(table);
                foreach (var paragraph in doc.Paragraphs)
                {
                    paragraph.FindAll("{tabel}").ForEach(index => paragraph.InsertTableAfterSelf((table)));
                }
                //Remove tag
                doc.ReplaceText("{tabel}", "");

                doc.SaveAs(OutFileNama);
                streamx.Close();
            }
            catch
            {
                streamx.Close();
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage CetakHPSXLS(Guid Id)
        {
            var pengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            var spc = new System.Data.DataTable("Jimbis");
            string outputFileName = "Cetak-HPS" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".xlsx";
            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var ms = new System.IO.MemoryStream();
            try
            { 
                var oVWRKSDetail = _repository.getRKSDetails(pengadaan.Id, UserId());
                
                using (var sl = new SpreadsheetLight.SLDocument())
                {
                    sl.SetCellValue(1, 1, "Judul Pengadaan");
                    sl.SetCellValue(1, 2, pengadaan.Judul == null ? "" : pengadaan.Judul);
                    sl.SetCellValue(2, 1, "Nomor Pengadaan");
                    sl.SetCellValue(2, 2, pengadaan.NoPengadaan == null ? "" : pengadaan.NoPengadaan);
                    sl.SetCellValue(3, 1, "Unit Kerja");
                    sl.SetCellValue(3, 2, pengadaan.UnitKerjaPemohon == null ? "" : pengadaan.UnitKerjaPemohon);

                    var rowNum = 6;
                    //write header
                    sl.SetCellValue(rowNum, 1, "Nama");
                    sl.SetCellValue(rowNum, 2, "Item");
                    sl.SetCellValue(rowNum, 3, "Satuan");
                    sl.SetCellValue(rowNum, 4, "Jumlah");
                    sl.SetCellValue(rowNum, 5, "Hps");
                    sl.SetCellValue(rowNum, 6, "Total");
                    sl.SetCellValue(rowNum, 7, "Keterangan");
                    rowNum++;
                    //write data
                    decimal subtotal = 0;
                    decimal totalall = 0;
                    foreach (var item in oVWRKSDetail)
                    {
                        if (item.level == 0)
                        {
                            sl.SetCellValue(rowNum, 1, (item.judul));
                        }
                        else if (item.level == 1)
                        {
                            sl.SetCellValue(rowNum, 2, Regex.Replace(item.item.ToString(), @"<.*?>", string.Empty));
                            sl.SetCellValue(rowNum, 3, item.satuan);
                            if (item.jumlah == null) sl.SetCellValue(rowNum, 4, "");
                            else sl.SetCellValue(rowNum, 4, item.jumlah.Value);
                            if (item.hps == null) sl.SetCellValue(rowNum, 5, "");
                            else sl.SetCellValue(rowNum, 5, item.hps.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                            decimal? total = item.jumlah * item.hps;
                            if (total == null) sl.SetCellValue(rowNum, 6, "");
                            else sl.SetCellValue(rowNum, 6, total.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                            sl.SetCellValue(rowNum, 7, item.keterangan);
                            subtotal = subtotal + total.Value;
                            totalall = totalall + total.Value;
                        }
                        else if (item.level == 2)
                        {
                            sl.SetCellValue(rowNum, 5, "Sub Total");
                            sl.SetCellValue(rowNum, 6, subtotal.ToString("C", MyConverter.formatCurrencyIndo()));
                            subtotal = 0;
                        }
                        rowNum++;
                    }
                    sl.SetCellValue(4, 1, "Total HPS");
                    sl.SetCellValue(4, 2, totalall.ToString("C", MyConverter.formatCurrencyIndo()));

                    //add filter
                    sl.SetColumnWidth(4, 12.0);
                    sl.SetColumnWidth(5, 30.0);
                    sl.SetColumnWidth(6, 30.0);

                    sl.SaveAs(ms);
                }

            }
            catch
            {
                
            }

            ms.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            //  var stream = new FileStream(ms, FileMode.Open);
            result.Content = new StreamContent(ms);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };
            return result;
        }

        //Buat Word Create PO
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public HttpResponseMessage CetakPO(Guid Id)
        {
            var potemplate = _porepo.get(Id);

            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Template\template-po.docx";

            string outputFileName = "Cetak-PO" + UserId().ToString() + "-" + DateTime.Now.ToString("dd-MM-yy") + ".docx";

            string OutFileNama = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Download\Report\Temp\" + outputFileName;

            var streamx = new FileStream(fileName, FileMode.Open);
            try
            {
                var doc = DocX.Load(streamx);//.Create(OutFileNama);
                doc.ReplaceText("{no_po}", potemplate.NoPO);
                doc.ReplaceText("{vendor}", potemplate.Vendor);
                doc.ReplaceText("{up}", potemplate.UP == null ? "" : potemplate.UP);
                doc.ReplaceText("{perihal}", potemplate.Prihal);
                DateTime tgl = potemplate.TanggalPO.Value;
                string ctgl = tgl.ToString("dd MMMM yyyy");
                doc.ReplaceText("{tanggal-po}", ctgl);

                var oPODetail = potemplate.PODetail;
                var table = doc.AddTable(oPODetail.Count() + 1, 7);


                int indexRow = 0;
                table.Rows[indexRow].Cells[0].Paragraphs.First().Append("No");
                table.Rows[indexRow].Cells[1].Paragraphs.First().Append("Kode");
                table.Rows[indexRow].Cells[2].Paragraphs.First().Append("Nama Barang");
                table.Rows[indexRow].Cells[2].Width = 10;
                table.Rows[indexRow].Cells[3].Paragraphs.First().Append("Banyak");
                table.Rows[indexRow].Cells[4].Paragraphs.First().Append("Satuan");
                table.Rows[indexRow].Cells[5].Paragraphs.First().Append("Harga");
                table.Rows[indexRow].Cells[6].Paragraphs.First().Append("Jumlah");
                indexRow++;
                decimal subtotal = 0;
                decimal totalall = 0;
                foreach (var item in oPODetail)
                {
                    table.Rows[indexRow].Cells[0].Paragraphs.First().Append(indexRow.ToString());
                    table.Rows[indexRow].Cells[1].Paragraphs.First().Append(item.Kode == null ? "" : item.Kode.ToString());
                    table.Rows[indexRow].Cells[2].Paragraphs.First().Append(item.NamaBarang == null ? "" : item.NamaBarang.ToString());
                    table.Rows[indexRow].Cells[2].Width = 10;
                    table.Rows[indexRow].Cells[3].Paragraphs.First().Append(Convert.ToInt32(item.Banyak).ToString() == null ? "" : Convert.ToInt32(item.Banyak).ToString());
                    table.Rows[indexRow].Cells[4].Paragraphs.First().Append(item.Satuan == null ? "" : item.Satuan.ToString());
                    table.Rows[indexRow].Cells[5].Paragraphs.First().Append(item.Harga == null ? "" : item.Harga.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                    decimal? jumlah = item.Banyak * item.Harga;
                    table.Rows[indexRow].Cells[6].Paragraphs.First().Append(jumlah == null ? "" : jumlah.Value.ToString("C", MyConverter.formatCurrencyIndo()));
                    subtotal = subtotal + jumlah.Value;
                    totalall = totalall + jumlah.Value;
                    indexRow++;
                }
                doc.ReplaceText("{total}", totalall.ToString("C", MyConverter.formatCurrencyIndo()));

                // Insert table at index where tag #TABLE# is in document.
                //doc.InsertTable(table);
                foreach (var paragraph in doc.Paragraphs)
                {
                    paragraph.FindAll("{tabel}").ForEach(index => paragraph.InsertTableAfterSelf((table)));
                }
                //Remove tag
                doc.ReplaceText("{tabel}", "");

                doc.SaveAs(OutFileNama);
                streamx.Close();
            }
            catch
            {
                streamx.Close();
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(OutFileNama, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName
            };

            return result;
        }
    }
}



// Append some text.
//p.Append("Hello World").Font(new FontFamily("Arial")).Bold();

//p = p.InsertParagraphAfterSelf(String.Empty);
//p.Alignment = Alignment.center;
//p.Font(new FontFamily("Arial"));
//p.FontSize(12);

//p.Append("Some text").Bold();

//p.Append("I am ").Append("bold").Bold()
//.Append(" and I am ")
//.Append("italic").Italic().Append(".")
//.AppendLine("I am ")
//.Append("Arial Black")
//.Font(new FontFamily("Arial Black"))
//.Append(" and I am not.")
//.AppendLine("I am ")
//.Append("BLUE").Color(Color.Blue)
//.Append(" and I am")
//.Append("Red").Color(Color.Red).Append(".");