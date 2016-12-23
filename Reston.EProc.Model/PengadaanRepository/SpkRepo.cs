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
    public interface ISpkRepo
    {
        int Delete(Guid Id, Guid UserId);

        DataTableSpkTemplate List(string search, int start, int limit, string klasifikasi);
        DataTablePksTemplate ListPks(string search, int start, int limit, string klasifikasi);
        VWSpk detail(Guid Id, Guid UserId);
        Spk saveSpkPertam(Spk spk, Guid UserId);
        ResultMessage save(Spk spk, Guid UserId);
        ResultMessage ChangeStatus(Guid Id,StatusSpk status, Guid UserId);
        ResultMessage saveDokumen(DokumenSpk dokPks, Guid UserId);
        ResultMessage deleteSpk(Guid Id, Guid UserId);
        int deleteDokumenSpk(Guid Id, Guid UserId, int approver);
        ResultMessage TolakPks(Guid Id,Guid UserId);
        ResultMessage SetujuiPks(Guid Id,string NoPks, Guid UserId);
        Spk get(Guid id);
        DokumenSpk getDokSpk(Guid id);
        RiwayatDokumenSpk AddRiwayatDokumenSpk(RiwayatDokumenSpk dtRiwayatDokumenSpk, Guid UserId);
        List<VWDokumenSPK> GetListDokumenSpk(Guid Id);

    }
    public class SpkRepo : ISpkRepo
    {
        JimbisContext ctx;

        public SpkRepo(JimbisContext j)
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

        public Spk saveSpkPertam(Spk spk, Guid UserId)
        {
            var oSpk = ctx.Spk.Where(d => d.NoSPk == spk.NoSPk && d.PksId == null).FirstOrDefault();
            if (oSpk == null)
            {
                var TotalHargaKandidat = spk.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault() == null ? null :
                           spk.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().RKSDetails.Where(dd =>
                               dd.RKSHeaderId == spk.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().Id)
                               .Sum(dx => dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault() == null ? 0 : dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault().harga * dx.jumlah);
                spk.CreateOn = DateTime.Now;
                spk.CreateBy = UserId;
                spk.NilaiSPK = TotalHargaKandidat;
                ctx.Spk.Add(spk);
            }
            else
            {
                var TotalHargaKandidat = spk.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault() == null ? null :
                           spk.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().RKSDetails.Where(dd =>
                               dd.RKSHeaderId == spk.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().Id)
                               .Sum(dx => dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault() == null ? 0 : dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault().harga * dx.jumlah);
                spk.NilaiSPK = TotalHargaKandidat;
                oSpk.NoSPk = spk.NoSPk;
                oSpk.Note = spk.Note;
                oSpk.PemenangPengadaanId = spk.PemenangPengadaanId;
                oSpk.StatusSpk = spk.StatusSpk;
                oSpk.Title = spk.Title;
                oSpk.WorkflowId = spk.WorkflowId;
                oSpk.ModifiedBy = UserId;
                oSpk.ModifiedOn = DateTime.Now;
            }
            ctx.SaveChanges();
            return oSpk;
        }

        public int Delete(Guid Id, Guid UserId)
        {
            try
            {
                var oSpk = ctx.Spk.Find(Id);
                if (oSpk != null)
                {
                    ctx.Spk.Remove(oSpk);
                }
                ctx.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }


        }
        
        public DataTableSpkTemplate List(string search, int start, int limit, string klasifikasi)
        {
            search = search == null ? "" : search;
            DataTableSpkTemplate dtTable = new DataTableSpkTemplate();
            if (limit > 0)
            {
                var data = ctx.Spk.AsQueryable();
                dtTable.recordsTotal = data.Count();
                if (!string.IsNullOrEmpty(klasifikasi))
                {
                    data = data.Where(d => d.PemenangPengadaan.Pengadaan.JenisPekerjaan == klasifikasi);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    data = data.Where(d => d.Title == d.Title);
                }
                dtTable.recordsFiltered = data.Count();
                data = data.OrderByDescending(d => d.CreateOn).Skip(start).Take(limit);
                var lol = data.ToList();
                dtTable.data = data.Select(d => new VWSpk
                {
                    Id = d.Id,
                    PemenangPengadaanId = d.PemenangPengadaanId,
                    NoSpk = d.NoSPk,
                    Judul = d.PemenangPengadaan.Pengadaan.Judul,
                    JenisPekerjaan = d.PemenangPengadaan.Pengadaan.JenisPekerjaan,
                    Vendor = d.PemenangPengadaan.Vendor.Nama,
                    AturanPengadaan = d.PemenangPengadaan.Pengadaan.AturanPengadaan,
                    VendorId = d.PemenangPengadaan.VendorId,
                    PengadaanId = d.PemenangPengadaan.Pengadaan.Id,
                    StatusSpk=d.StatusSpk,
                    StatusSpkName=d.StatusSpk.ToString(),
                    HPS = d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault() == null ? null :
                           d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().RKSDetails.Where(dd =>
                               dd.RKSHeaderId == d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().Id)
                               .Sum(dx => dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault() == null ? 0 : dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault().harga * dx.jumlah)
                }).ToList();
            }
            return dtTable;
        }

        public DataTablePksTemplate ListPks(string search, int start, int limit, string klasifikasi)
        {
            search = search == null ? "" : search;
            DataTablePksTemplate dtTable = new DataTablePksTemplate();
            if (limit > 0)
            {

                var data = ctx.Pks.Where(d=>d.StatusPks==StatusPks.Approve).AsQueryable();
                dtTable.recordsTotal = data.Count();
                if (!string.IsNullOrEmpty(klasifikasi))
                {
                    data = data.Where(d => d.PemenangPengadaan.Pengadaan.JenisPekerjaan == klasifikasi);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    data = data.Where(d => d.Title == d.Title);
                }
                dtTable.recordsFiltered = data.Count();
                data = data.OrderByDescending(d => d.CreateOn).Skip(start).Take(limit);
                dtTable.data = data.Select(d => new VWPks
                {
                    Id = d.Id,
                    PemenangPengadaanId = d.PemenangPengadaanId,
                    NoPks = d.NoDokumen,
                    NoPengadaan=d.PemenangPengadaan.Pengadaan.NoPengadaan,
                    Judul = d.PemenangPengadaan.Pengadaan.Judul,
                    JenisPekerjaan = d.PemenangPengadaan.Pengadaan.JenisPekerjaan,
                    Vendor = d.PemenangPengadaan.Vendor.Nama,
                    AturanPengadaan = d.PemenangPengadaan.Pengadaan.AturanPengadaan,
                    VendorId = d.PemenangPengadaan.VendorId,
                    PengadaanId = d.PemenangPengadaan.Pengadaan.Id,
                    StatusPks = d.StatusPks,
                    StatusPksName = d.StatusPks.ToString(),
                    WorkflowId = d.WorkflowId,
                    HPS = d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault() == null ? null :
                            d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().RKSDetails.Where(dd =>
                                dd.RKSHeaderId == d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().Id)
                                .Sum(dx => dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault() == null ? 0 : dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault().harga * dx.jumlah)
                }).ToList();

            }
            return dtTable;
        }

        public ResultMessage save(Spk spk, Guid UserId)
        {
            try
            {
                if (spk.Id != Guid.Empty && spk.Id != null)
                {
                    var oldPks = ctx.Spk.Find(spk.Id);
                    if (oldPks == null || oldPks.CreateBy!=UserId) return new ResultMessage();
                    oldPks.Note = spk.Note;
                    oldPks.Title = spk.Title;
                    oldPks.PksId = spk.PksId;
                    oldPks.ModifiedBy = UserId;
                    oldPks.ModifiedOn = DateTime.Now;
                    ctx.SaveChanges(UserId.ToString());
                    return new ResultMessage()
                    {
                        Id = spk.Id.ToString(),
                        message = Common.UpdateSukses(),
                        status = HttpStatusCode.OK
                    };
                }
                else
                {
                    if (spk.PksId == null) return new ResultMessage()
                    {
                        message = HttpStatusCode.MethodNotAllowed.ToString(),
                        status = HttpStatusCode.MethodNotAllowed
                    };
                    spk.CreateBy = UserId;
                    spk.CreateOn = DateTime.Now;
                    ctx.Spk.Add(spk);
                    ctx.SaveChanges(UserId.ToString());
                    return new ResultMessage()
                    {
                        Id = spk.Id.ToString(),
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

        public Spk get(Guid id)
        {
            return ctx.Spk.Find(id);
        }

        public ResultMessage deleteSpk(Guid Id, Guid UserId)
        {
            try
            {
                var oldData = ctx.Spk.Find(Id);
                ctx.Spk.Remove(oldData);
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

        public VWSpk detail(Guid Id,Guid UserId)
        {
            return ctx.Spk.Where(d => d.Id == Id).Select(d => new VWSpk()
            {
                Id = d.Id,
                JenisPekerjaan = d.PemenangPengadaan.Pengadaan.JenisPekerjaan,
                PemenangPengadaanId = d.PemenangPengadaan.Id,
                Judul = d.PemenangPengadaan.Pengadaan.Judul,
                NoPengadaan = d.PemenangPengadaan.Pengadaan.NoPengadaan,
                Keterangan = d.PemenangPengadaan.Pengadaan.Keterangan,
                HPS =  d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault() == null ? null :
                           d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().RKSDetails.Where(dd =>
                               dd.RKSHeaderId == d.PemenangPengadaan.Pengadaan.RKSHeaders.FirstOrDefault().Id)
                               .Sum(dx => dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault() == null ? 0 : dx.HargaKlarifikasiRekanan.Where(ddx => ddx.RKSDetailId == dx.Id).FirstOrDefault().harga * dx.jumlah),
                NoSpk = d.NoSPk,
                Vendor = d.PemenangPengadaan.Vendor.Nama,
                StatusSpk = d.StatusSpk,
                StatusSpkName = d.StatusSpk.ToString(),
                isOwner=d.CreateBy==UserId?1:0,
                Note=d.Note,
                WorkflowId=d.WorkflowId,
                TanggalSPK=d.TanggalSPK,
                NilaiSPK=d.NilaiSPK
            }).FirstOrDefault();
        }

        public ResultMessage saveDokumen(DokumenSpk dokSpk, Guid UserId)
        {
            try
            {
                var oData = ctx.Spk.Find(dokSpk.SpkId);
                if (oData == null) return new ResultMessage
                {
                    status = HttpStatusCode.Forbidden,
                    message = Common.Forbiden()
                };
                dokSpk.CreateOn = DateTime.Now;
                dokSpk.CreateBy = UserId;
                ctx.DokumenSpk.Add(dokSpk);
                ctx.SaveChanges();
                return new ResultMessage
                {
                    status = HttpStatusCode.OK,
                    message = Common.SaveSukses(),
                    Id = dokSpk.Id.ToString()
                };
            }
            catch(Exception ex){
                return new ResultMessage()
                {
                    status = HttpStatusCode.NotImplemented,
                    message = ex.ToString()
                };
            }
        }

        public List<VWDokumenSPK> GetListDokumenSpk(Guid Id)
        {
            return ctx.DokumenSpk.Where(d => d.SpkId == Id ).Select(d => new VWDokumenSPK()
            {
                Id=d.Id,
                SpkId = d.SpkId,
                SizeFile=d.SizeFile,
                ContentType=d.ContentType,
                File=d.File
            }).ToList();
        }

        public int deleteDokumenSpk(Guid Id, Guid UserId,int approver)
        {
            try
            {
                DokumenSpk doku = ctx.DokumenSpk.Find(Id);
                int isMine = doku.CreateBy == UserId ? 1 : 0;
                if (isMine == 1)
                {
                    ctx.DokumenSpk.Remove(doku);
                    ctx.SaveChanges(UserId.ToString());
                    return 1;
                }
                return 0;
            }
            catch { return 0; }
        }

        public RiwayatDokumenSpk AddRiwayatDokumenSpk(RiwayatDokumenSpk dtRiwayatDokumenSpk, Guid UserId)
        {
            try
            {
                ctx.RiwayatDokumenSpk.Add(dtRiwayatDokumenSpk);
                ctx.SaveChanges(UserId.ToString());
                return dtRiwayatDokumenSpk;
            }
            catch
            {
                return new RiwayatDokumenSpk();
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

        public DokumenSpk getDokSpk(Guid id)
        {
            return ctx.DokumenSpk.Find(id);
        }

        public ResultMessage ChangeStatus(Guid Id, StatusSpk status, Guid UserId)
        {
            try
            {
                var oData = ctx.Spk.Find(Id);
                oData.StatusSpk = status;
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

    }
}


