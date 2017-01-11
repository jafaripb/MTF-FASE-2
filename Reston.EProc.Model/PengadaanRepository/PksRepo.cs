using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using Model.Helper;
using Reston.Pinata.Model.PengadaanRepository.View;
using System.Configuration;
using Reston.Pinata.Model.JimbisModel;
using Newtonsoft.Json;
using System.Net;
using Reston.Helper.Util;


namespace Reston.Pinata.Model.PengadaanRepository
{
    public interface IPksRepo
    {
        DataTablePksTemplate List(string search, int start, int limit, string klasifikasi);
        DataTablePksTemplate ListPengadaan(string search, int start, int limit, string klasifikasi);
        VWPks detail(Guid Id, Guid UserId);
        ResultMessage save(Pks pks, Guid UserId);
        ResultMessage ChangeStatus(Guid Id,StatusPks status, Guid UserId);
        ResultMessage saveDokumen(DokumenPks dokPks, Guid UserId);
        ResultMessage deletePks(Guid Id, Guid UserId);
        int deleteDokumenPks(Guid Id, Guid UserId, int approver);
        ResultMessage TolakPks(Guid Id,Guid UserId);
        ResultMessage SetujuiPks(Guid Id,string NoPks, Guid UserId);
        Pks get(Guid id);
        DokumenPks getDokPks(Guid id);
        RiwayatDokumenPks AddRiwayatDokumenPks(RiwayatDokumenPks dtRiwayatDokumenPks, Guid UserId);

        List<VWDokumenPks> GetListDokumenPks(Guid Id,TipeBerkas tipe);
        ResultMessage saveCatatan(CatatanPks data);
        List<VWCatatanPks> ListCatatanPKs(Guid Id);
    }
    public class PksRepo : IPksRepo
    {
        JimbisContext ctx;
        public PksRepo(JimbisContext j)
        {
            ctx = j;
            //ctx.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = true;
        }

        ResultMessage msg = new ResultMessage();

        public void Save()
        {
            ctx.SaveChanges();
        }

        public DataTablePksTemplate List(string search, int start, int limit, string klasifikasi)
        {
            search = search == null ? "" : search;
            DataTablePksTemplate dtTable = new DataTablePksTemplate();
            if (limit > 0)
            {

                var data = ctx.Pks.AsQueryable();
                dtTable.recordsTotal = data.Count();
                if (!string.IsNullOrEmpty(klasifikasi))
                {
                    data = data.Where(d => d.PemenangPengadaan.Pengadaan.JenisPekerjaan == klasifikasi);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    data = data.Where(d => d.Title.Contains(search));
                }
                dtTable.recordsFiltered = data.Count();
                data = data.OrderByDescending(d => d.CreateOn).Skip(start).Take(limit);
                dtTable.data = data.Select(d => new VWPks
                {
                    Id = d.Id,
                    PemenangPengadaanId = d.PemenangPengadaanId,
                    NoPks=d.NoDokumen,
                    Judul = d.PemenangPengadaan.Pengadaan.Judul,
                    JenisPekerjaan = d.PemenangPengadaan.Pengadaan.JenisPekerjaan,
                    Vendor = d.PemenangPengadaan.Vendor.Nama,
                    AturanPengadaan = d.PemenangPengadaan.Pengadaan.AturanPengadaan,
                    VendorId = d.PemenangPengadaan.VendorId,
                    PengadaanId = d.PemenangPengadaan.Pengadaan.Id,
                    StatusPks=d.StatusPks,
                    StatusPksName=d.StatusPks.ToString(),
                    WorkflowId=d.WorkflowId,
                    CreateBy=d.CreateBy,
                    HPS = d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault() == null ? null :
                            d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().RKSDetails.Where(dd =>
                                dd.RKSHeaderId == d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().Id)
                                .Sum(dx => dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault() == null ? 0 : dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault().harga*dx.jumlah)
                }).ToList();

            }
            return dtTable;
        }

        public DataTablePksTemplate ListPengadaan(string search, int start, int limit, string klasifikasi)
        {
            search = search == null ? "" : search;
            DataTablePksTemplate dtTable = new DataTablePksTemplate();
            if (limit > 0)
            {
                var data = ctx.DokumenPengadaans.Where(d => d.Tipe == TipeBerkas.SuratPerintahKerja && d.Pengadaan.BeritaAcaras.Where(dd => dd.VendorId == d.VendorId).Count() > 0);
                dtTable.recordsTotal = data.Count();
                if (!string.IsNullOrEmpty(klasifikasi))
                {
                    data = data.Where(d => d.Pengadaan.JenisPekerjaan == klasifikasi);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    data = data.Where(d => d.Pengadaan.Judul.Contains(search) || d.Pengadaan.NoPengadaan.Contains(search));
                }
                dtTable.recordsFiltered = data.Count();
                data = data.OrderByDescending(d => d.CreateOn).Skip(start).Take(limit);
                dtTable.data = data.Select(d => new VWPks
                {
                    Id = d.Id,
                    PemenangPengadaanId = d.Pengadaan.PemenangPengadaans.Where(dd => dd.VendorId == d.VendorId).FirstOrDefault().Id,
                    NoSpk = d.Pengadaan.BeritaAcaras.Where(dd => dd.VendorId == d.VendorId).FirstOrDefault().NoBeritaAcara,
                    NoPengadaan = d.Pengadaan.NoPengadaan,
                    Judul = d.Pengadaan.Judul,
                    JenisPekerjaan = d.Pengadaan.JenisPekerjaan,
                    Vendor = d.Vendor == null ? "" : d.Vendor.Nama,
                    AturanPengadaan = d.Pengadaan.AturanPengadaan,
                    VendorId = d.VendorId,
                    PengadaanId = d.PengadaanId,
                    HPS = d.Pengadaan.RKSHeaders.FirstOrDefault() == null ? null : d.Pengadaan.RKSHeaders.FirstOrDefault().RKSDetails.Where(dd => dd.RKSHeaderId == d.Pengadaan.RKSHeaders.FirstOrDefault().Id).Sum(dx => dx.jumlah * dx.hps == null ? 0 : dx.jumlah * dx.hps)
                }).ToList();
            }
            return dtTable;
        }

        public ResultMessage save(Pks pks, Guid UserId)
        {
            try
            {
                if (pks.Id != Guid.Empty && pks.Id != null)
                {
                    var oldPks = ctx.Pks.Find(pks.Id);
                    if (oldPks == null || oldPks.CreateBy!=UserId) return new ResultMessage();
                    oldPks.Note = pks.Note;
                    oldPks.Title = pks.Title;
                    oldPks.PemenangPengadaanId = pks.PemenangPengadaanId;
                    oldPks.ModifiedBy = UserId;
                    oldPks.ModifiedOn = DateTime.Now;
                    ctx.SaveChanges(UserId.ToString());
                    return new ResultMessage()
                    {
                        Id = pks.Id.ToString(),
                        message = Common.UpdateSukses(),
                        status = HttpStatusCode.OK
                    };
                }
                else
                {
                    pks.CreateBy = UserId;
                    pks.CreateOn = DateTime.Now;
                    ctx.Pks.Add(pks);
                    ctx.SaveChanges(UserId.ToString());
                    return new ResultMessage()
                    {
                        Id = pks.Id.ToString(),
                        message = Common.SaveSukses(),
                        status = HttpStatusCode.OK
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultMessage()
                {
                    message = ex.ToString(),
                    status = HttpStatusCode.ExpectationFailed
                };

            }
        }

        public Pks get(Guid id)
        {
            return ctx.Pks.Find(id);
        }

        public ResultMessage deletePks(Guid Id, Guid UserId)
        {
            try
            {

                var oldData = ctx.Pks.Find(Id);
                if (oldData.StatusPks != StatusPks.Draft || oldData.CreateBy!=UserId) return new ResultMessage()
                {
                    message = HttpStatusCode.MethodNotAllowed.ToString(),
                    status = HttpStatusCode.MethodNotAllowed
                };                
                ctx.Pks.Remove(oldData);
                ctx.SaveChanges(UserId.ToString());
                msg.status = HttpStatusCode.OK;
                msg.message = "Sukses";
            }
            catch (Exception ex)
            {
                msg.status = HttpStatusCode.ExpectationFailed;
                msg.message = ex.ToString();
            }
            return msg;
        }

        public VWPks detail(Guid Id,Guid UserId)
        {
            return ctx.Pks.Where(d => d.Id == Id).Select(d => new VWPks()
            {
                Id = d.Id,
                JenisPekerjaan = d.PemenangPengadaan.Pengadaan.JenisPekerjaan,
                PemenangPengadaanId = d.PemenangPengadaan.Id,
                Judul = d.PemenangPengadaan.Pengadaan.Judul,
                NoPengadaan = d.PemenangPengadaan.Pengadaan.NoPengadaan,
                Keterangan = d.PemenangPengadaan.Pengadaan.Keterangan,
                HPS = d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().RKSDetails.Sum(dd => dd.hps * dd.jumlah == null ? 0 : dd.hps * dd.jumlah).Value,
               // NoSpk = d.PemenangPengadaan.Pengadaan.BeritaAcaras.Where(dd => dd.VendorId == d.PemenangPengadaan.VendorId).FirstOrDefault() == null ? "" : d.PemenangPengadaan.Pengadaan.BeritaAcaras.Where(dd => dd.VendorId == d.PemenangPengadaan.VendorId).FirstOrDefault().NoBeritaAcara,
                Vendor = d.PemenangPengadaan.Vendor.Nama,
                NoPks=d.NoDokumen,
                StatusPks=d.StatusPks,
                StatusPksName=d.StatusPks.ToString(),
                isOwner=d.CreateBy==UserId?1:0,
                Title=d.Title,
                Note=d.Note,
                WorkflowId=d.WorkflowId
            }).FirstOrDefault();
        }

        public VWPks detailxx(Guid PengadaanId, int VendorId)
        {
            return ctx.Pengadaans.Where(d => d.Id == PengadaanId).Select(d => new VWPks()
            {
                Id = PengadaanId,
                JenisPekerjaan = d.JenisPekerjaan,
                PemenangPengadaanId = d.PemenangPengadaans.Where(dd => dd.VendorId == VendorId).FirstOrDefault().Id,
                Judul = d.Judul,
                NoPengadaan = d.NoPengadaan,
                Keterangan = d.Keterangan,
                HPS = d.RKSHeaders.FirstOrDefault().RKSDetails.Sum(dd => dd.hps * dd.jumlah == null ? 0 : dd.hps * dd.jumlah).Value,
                NoSpk = d.BeritaAcaras.Where(dd => dd.VendorId == VendorId).FirstOrDefault() == null ? "" : d.BeritaAcaras.Where(dd => dd.VendorId == VendorId).FirstOrDefault().NoBeritaAcara,
                Vendor = d.BeritaAcaras.Where(dd => dd.VendorId == VendorId).FirstOrDefault() == null ? "" : d.BeritaAcaras.Where(dd => dd.VendorId == VendorId).FirstOrDefault().Vendor.Nama
            }).FirstOrDefault();
        }

        public ResultMessage saveDokumen(DokumenPks dokPks,Guid UserId)
        {
            try
            {
                var oData = ctx.Pks.Find(dokPks.PksId);
                if (oData == null) return new ResultMessage
                {
                    status = HttpStatusCode.Forbidden,
                    message = Common.Forbiden()
                };
              /*  if (oData.CreateBy != UserId && (dokPks.Tipe == TipeBerkas.DraftPKS||dokPks.Tipe==TipeBerkas.FinalLegalPks)) return new ResultMessage
                {
                    status = HttpStatusCode.Forbidden,
                    message = Common.Forbiden()
                };*/
                dokPks.CreateOn = DateTime.Now;
                dokPks.CreateBy = UserId;
                ctx.DokumenPks.Add(dokPks);
                ctx.SaveChanges();
                return new ResultMessage
                {
                    status = HttpStatusCode.OK,
                    message = Common.SaveSukses(),
                    Id=dokPks.Id.ToString()
                };
            }
            catch(Exception ex){
                return new ResultMessage()
                {
                    status = HttpStatusCode.ExpectationFailed,
                    message = ex.ToString()
                };
            }
        }

        public List<VWDokumenPks> GetListDokumenPks(Guid Id,TipeBerkas tipe)
        {
            return ctx.DokumenPks.Where(d => d.PksId == Id && d.Tipe==tipe).Select(d => new VWDokumenPks()
            {
                Id=d.Id,
                PksId=d.PksId,
                SizeFile=d.SizeFile,
                ContentType=d.ContentType,
                File=d.File
            }).ToList();
        }

        public int deleteDokumenPks(Guid Id, Guid UserId,int approver)
        {
            try
            {
                DokumenPks MdokPks = ctx.DokumenPks.Find(Id);
                int isMine = MdokPks.CreateBy == UserId ? 1 : 0;


               /* if ((approver == 1 && MdokPks.Tipe == TipeBerkas.AssignedPks) ||
                    (isMine == 1 && (MdokPks.Tipe == TipeBerkas.DraftPKS && MdokPks.Tipe == TipeBerkas.FinalLegalPks)))
                {*/
                    ctx.DokumenPks.Remove(MdokPks);
                    ctx.SaveChanges(UserId.ToString());
                    return 1;
               // }
               // return 0;
            }
            catch { return 0; }
        }

        public RiwayatDokumenPks AddRiwayatDokumenPks(RiwayatDokumenPks dtRiwayatDokumenPks, Guid UserId)
        {
            try
            {
                ctx.RiwayatDokumenPks.Add(dtRiwayatDokumenPks);
                ctx.SaveChanges(UserId.ToString());
                return dtRiwayatDokumenPks;
            }
            catch
            {
                return new RiwayatDokumenPks();
            }
        }

        public ResultMessage TolakPks(Guid Id, Guid UserId)
        {
            try
            {
                var oldData = ctx.Pks.Find(Id);
                oldData.StatusPks = StatusPks.Reject;
                ctx.SaveChanges(UserId.ToString());
                return new ResultMessage(){
                    Id=Id.ToString(),
                    message=Common.SaveSukses(),
                    status=HttpStatusCode.OK
                };

            }
            catch(Exception ex)
            {
                return new ResultMessage()
                {
                    Id = "0",
                    message = ex.ToString(),
                    status = HttpStatusCode.NotImplemented
                };
            }
        }

        public ResultMessage SetujuiPks(Guid Id,string NoPks, Guid UserId)
        {
            try
            {
                var oldData = ctx.Pks.Find(Id);
                oldData.NoDokumen = NoPks;
                oldData.StatusPks = StatusPks.Approve;
                ctx.SaveChanges(UserId.ToString());
                return new ResultMessage()
                {
                    Id = Id.ToString(),
                    message = Common.SaveSukses(),
                    status = HttpStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new ResultMessage()
                {
                    Id = "0",
                    message = ex.ToString(),
                    status = HttpStatusCode.NotImplemented
                };
            }
        }

        public DokumenPks getDokPks(Guid id)
        {
            return ctx.DokumenPks.Find(id);
        }

        public ResultMessage ChangeStatus(Guid Id, StatusPks status, Guid UserId)
        {
            try
            {
                var oData = ctx.Pks.Find(Id);
                oData.StatusPks = status;
                ctx.SaveChanges(UserId.ToString());
                return new ResultMessage()
                {
                    Id=Id.ToString(),message=Common.SaveSukses(),status=HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ResultMessage()
                {
                    message=ex.ToString(),status=HttpStatusCode.NotImplemented
                };
            }
        }

        
        public List<VWCatatanPks> ListCatatanPKs(Guid Id)
        {
            return ctx.RiwayatDokumenPks.Where(d => d.PksId == Id).Select(d => new VWCatatanPks() { 
                Id=d.Id,
                PksId=d.PksId,
                Status=d.Status,
                Catatan=d.Comment.ToString(),
                CreatedOn=d.ActionDate.Value,
                CreatedBy=d.UserId.Value            
            }).ToList();
        }

        public ResultMessage saveCatatan(CatatanPks data)
        {
            try
            {
                ctx.CatatanPks.Add(data);
                ctx.SaveChanges();
                return new ResultMessage()
                {
                    Id = data.Id.ToString(),
                    message = Common.SaveSukses(),
                    status = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ResultMessage()
                {
                    message = ex.ToString(),
                    status = HttpStatusCode.NotImplemented
                };
            }


        }


    }
}


