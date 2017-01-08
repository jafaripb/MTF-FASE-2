using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.PengadaanRepository;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using Reston.Eproc.Model.Monitoring.Entities;

namespace Reston.Pinata.Model
{
    public class SysLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string RecordID { get; set; }
        public string UserID { get; set; }
        public string ColumnName { get; set; }
        public string CurrentValue { get; set; }
        public string BeforeValue { get; set; }
        public Nullable<DateTime> EventDateUTC { get; set; }
        public string EventType { get; set; }
    }

    public class JimbisContext:DbContext
    {
        public const string VENDOR_SCHEMA_NAME = "vendor";
        public const string VENDORREG_SCHEMA_NAME = "vendorreg";
        public const string CATALOG_SCHEMA_NAME = "catalog";
        public const string MASTER_SCHEMA_NAME = "master";
        public const string PENGADAAN_SCHEMA_NAME = "pengadaan";
        public const string MONITORING_SCHEMA_NAME = "monitoring";
        public const string WORKFLOW_SCHEMA_NAME = "workflow";
        public const string PROYEK_SCHEMA_NAME = "proyek";
        public const string PO_SCHEMA_NAME = "po"; 

        public JimbisContext()
            : base("name=JimbisEntities")
        {
            //Configuration.ProxyCreationEnabled = false;
        }
		
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<BankInfo> BankInfos { get; set; }
        public virtual DbSet<VendorPerson> VendorPersons { get; set; }
        public virtual DbSet<RiwayatPengajuanVendor> RiwayatPengajuans { get; set; }
        public virtual DbSet<Dokumen> Dokumens { get; set; }
        public virtual DbSet<DokumenDetail> DokumenDetails { get; set; }
        public virtual DbSet<AktaDokumenDetail> AktaDokumenDetails { get; set; }
        public virtual DbSet<IzinUsahaDokumenDetail> IzinUsahaDokumenDetails { get; set; }
        public virtual DbSet<RegVendor> RegVendors { get; set; }
        public virtual DbSet<RegBankInfo> RegBankInfos { get; set; }
        public virtual DbSet<RegVendorPerson> RegVendorPersons { get; set; }
        public virtual DbSet<RegRiwayatPengajuanVendor> RegRiwayatPengajuanVendors { get; set; }
        public virtual DbSet<RegDokumen> RegDokumens { get; set; }
        public virtual DbSet<RegDokumenDetail> RegDokumenDetails { get; set; }
        public virtual DbSet<RegAktaDokumenDetail> RegAktaDokumenDetails { get; set; }
        public virtual DbSet<RegIzinUsahaDokumenDetail> RegIzinUsahaDokumenDetails { get; set; }
        public virtual DbSet<Produk> Produks { get; set; }
        public virtual DbSet<KategoriSpesifikasi> KategoriSpesifikasis { get; set; }
        public virtual DbSet<AtributSpesifikasi> AtributSpesifikasis { get; set; }
        public virtual DbSet<RiwayatHarga> RiwayatHargas { get; set; }
        public virtual DbSet<Pengadaan> Pengadaans { get; set; }
        public virtual DbSet<DokumenPengadaan> DokumenPengadaans { get; set; }
        public virtual DbSet<KandidatPengadaan> KandidatPengadaans { get; set; }
        public virtual DbSet<JadwalPengadaan> JadwalPengadaans { get; set; }
        public virtual DbSet<PersonilPengadaan> PersonilPengadaans { get; set; }
        public virtual DbSet<RiwayatPengadaan> RiwayatPengadaans { get; set; }
        public virtual DbSet<BintangPengadaan> BintangPengadaans { get; set; }
        public virtual DbSet<RKSHeader> RKSHeaders { get; set; }
        public virtual DbSet<RKSDetail> RKSDetails { get; set; }
        public virtual DbSet<MessagePengadaan> MessagePengadaans { get; set; }
        public virtual DbSet<ReferenceData> ReferenceDatas { get; set; }
        public virtual DbSet<PelaksanaanAanwijzing> PelaksanaanAanwijzings { get; set; }
        public virtual DbSet<KehadiranKandidatAanwijzing> KehadiranKandidatAanwijzings { get; set; }
        public virtual DbSet<PelaksanaanSubmitPenawaran> PelaksanaanSubmitPenawarans { get; set; }
        public virtual DbSet<PelaksanaanBukaAmplop> PelaksanaanBukaAmplops { get; set; }
        public virtual DbSet<PersetujuanBukaAmplop> PersetujuanBukaAmplops { get; set; }
        public virtual DbSet<PelaksanaanPenilaianKandidat> PelaksanaanPenilaianKandidats { get; set; }
        public virtual DbSet<PemenangPengadaan> PemenangPengadaans { get; set; }
        public virtual DbSet<PelaksanaanKlarifikasi> PelaksanaanKlarifikasis { get; set; }
        public virtual DbSet<JadwalPelaksanaan> JadwalPelaksanaans { get; set; }
        public virtual DbSet<KualifikasiKandidat> KualifikasiKandidats { get; set; }
        public virtual DbSet<HargaRekanan> HargaRekanans { get; set; }
        public virtual DbSet<PelaksanaanPemilihanKandidat> PelaksanaanPemilihanKandidats { get; set; }
        public virtual DbSet<HargaKlarifikasiRekanan> HargaKlarifikasiRekanans { get; set; }
        public virtual DbSet<CatatanPengadaan> CatatanPengadaans { get; set; }
        public virtual DbSet<KreteriaPembobotan> KreteriaPembobotans { get; set; }
        public virtual DbSet<PembobotanPengadaan> PembobotanPengadaans { get; set; }
        public virtual DbSet<PembobotanPengadaanVendor> PembobotanPengadaanVendors { get; set; }
        public virtual DbSet<NoDokumenGenerator> NoDokumenGenerators { get; set; }
        public virtual DbSet<BeritaAcara> BeritaAcaras { get; set; }
        public virtual DbSet<ReportPengadaan> ReportPengadaans { get; set; }
        public virtual DbSet<CaptchaRegistration> CaptchaRegistration { get; set; }
        public virtual DbSet<PembatalanPengadaan> PembatalanPengadaans { get; set; }
        public virtual DbSet<PenolakanPengadaan> PenolakanPengadaans { get; set; }
        public virtual DbSet<RiwayatDokumen> RiwayatDokumens { get; set; }
        public virtual DbSet<RKSHeaderTemplate> RKSHeaderTemplate { get; set; }
        public virtual DbSet<RKSDetailTemplate> RKSDetailTemplate { get; set; }
        public virtual DbSet<MonitoringPekerjaan> MonitoringPekerjaans { get; set; }
        public virtual DbSet<DetailPekerjaan> DetailPekerjaans { get; set; }
        public virtual DbSet<JadwalProyek> JadwalProyeks { get; set; }

        public virtual DbSet<HargaKlarifikasiLanLanjutan> HargaKlarifikasiLanLanjutans { get; set; }
        
        //----------------------------------------------------------------------------------
        // Proyek
        public virtual DbSet<TahapanProyek> TahapanProyeks { get; set; }
        public virtual DbSet<RencanaProyek> RencanaProyeks { get; set; }
        public virtual DbSet<PICProyek> PICProyeks { get; set; }
        public virtual DbSet<DokumenProyek> DokumenProyeks { get; set; }
        
        public virtual DbSet<PersetujuanPemenang> PersetujuanPemenangs { get; set; }
        public virtual DbSet<Pks> Pks { get; set; }
		public virtual DbSet<PenilaianVendorHeader> PenilaianVendorHeaders { get; set; }
        public virtual DbSet<PenilaianVendorDetail> PenilaianVendorDetails { get; set; }
        public virtual DbSet<DokumenPks> DokumenPks { get; set; }
         public virtual DbSet<RiwayatDokumenPks> RiwayatDokumenPks { get; set; }
        public virtual DbSet<Spk> Spk { get; set; }
        public virtual DbSet<DokumenSpk> DokumenSpk { get; set; }
        public virtual DbSet<RiwayatDokumenSpk> RiwayatDokumenSpk { get; set; }
        public virtual DbSet<COA> COAs { get; set; }
        
        public virtual DbSet<PO> POs { get; set; }
        public virtual DbSet<PODetail> PODetails { get; set; }
        public virtual DbSet<DokumenPO> DokumenPO { get; set; }
        public virtual DbSet<LewatTahapan> LewatTahapans { get; set; }
        
        //persetujuan tahapan

        public virtual DbSet<PersetujuanTahapan> PersetujuanTahapans { get; set; }
        //
        public virtual DbSet<PersetujuanTerkait> PersetujuanTerkait { get; set; }
        

        public virtual DbSet<SysLog> SysLogs { get; set; }
        
        public void ValidateReferenceData()
        {
            //Qualifier dan Code tidak bisa diubah
            foreach (var entry in this.ChangeTracker.Entries<ReferenceData>().Where(a => a.State == EntityState.Modified))
            {
                entry.Property(a => a.Qualifier).IsModified = false;
                entry.Property(a => a.Code).IsModified = false;
            }

            //untuk reference data yang baru dimasukkan, cek reference data
            foreach (var entry in this.ChangeTracker.Entries<ReferenceData>().Where(a => a.State == EntityState.Added))
            {
                //cari yang qualifier dan kodenya sama
                var duplicates = this.ReferenceDatas.Where(a =>
                    a.Qualifier == entry.Entity.Qualifier
                    && a.Code == entry.Entity.Code).ToList();

                //gunakan prefix untuk menghindari duplikat pada kode yang sudah dihapus
                if (duplicates.Count > 0)
                {
                    entry.Entity.Code += duplicates.Count;
                }
            }
        }

        #region Audit Log
        public int SaveChanges(string userId)
        {
            try
            {
                // Get all Added/Deleted/Modified entities (not Unmodified or Detached)
                foreach (var ent in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
                {
                    // For each changed record, get the audit record entries and add them
                    foreach (SysLog x in GetAuditRecordsForChange(ent, userId))
                    {
                        this.SysLogs.Add(x);
                    }
                }
            }
            catch
            {
            }

            // Call the original SaveChanges(), which will save both the changes made and the audit records
            return base.SaveChanges();
        }

        private string GetPrimaryKeyName(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntitySet.ElementType.KeyMembers.Single().Name;
        }

        private List<SysLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, string userId)
        {
            List<SysLog> result = new List<SysLog>();

            DateTime changeTime = DateTime.Now;
            TableAttribute tableAttr = null;
            string tableName = string.Empty;
            string keyName = string.Empty;

            try
            {
                // Get the Table() attribute, if one exists
                tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

                // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
                tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

                // Get primary key value (If you have more than one key column, this will need to be adjusted)
                keyName = GetPrimaryKeyName(dbEntry);


                if (dbEntry.State == EntityState.Added)
                {
                    // For Inserts, just add the whole record
                    result.Add(new SysLog()
                    {
                        UserID = userId,
                        EventDateUTC = changeTime,
                        EventType = "A", // Added
                        TableName = tableName,
                        RecordID = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                        ColumnName = "*ALL",
                        CurrentValue = DescribeEntity(dbEntry.CurrentValues.ToObject())
                    }
                        );
                }
                else if (dbEntry.State == EntityState.Deleted)
                {
                    // Same with deletes, do the whole record
                    result.Add(new SysLog()
                    {
                        UserID = userId,
                        EventDateUTC = changeTime,
                        EventType = "D", // Deleted
                        TableName = tableName,
                        RecordID = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                        ColumnName = "*ALL",
                        BeforeValue = DescribeEntity(dbEntry.OriginalValues.ToObject())
                    }
                        );
                }
                else if (dbEntry.State == EntityState.Modified)
                {
                    foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                    {
                        // For updates, we only want to capture the columns that actually changed
                        if (!object.Equals(dbEntry.GetDatabaseValues().GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                        {
                            result.Add(new SysLog()
                            {
                                UserID = userId,
                                EventDateUTC = changeTime,
                                EventType = "M",    // Modified
                                TableName = tableName,
                                RecordID = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                                ColumnName = propertyName,
                                BeforeValue = dbEntry.GetDatabaseValues().GetValue<object>(propertyName) == null ? null : dbEntry.GetDatabaseValues().GetValue<object>(propertyName).ToString(),
                                CurrentValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                            }
                                );
                        }
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        private string DescribeEntity(object obj)
        {
            string returnValue = string.Empty;
            returnValue = "{";
            var properties = from p in obj.GetType().GetProperties()
                             where p.CanRead && p.CanWrite
                             select p;
            foreach (var property in properties)
            {
                var value = property.GetValue(obj, null) == null ? string.Empty : property.GetValue(obj, null).ToString();
                returnValue += property.Name + ":" + "\"" + value + "\" , ";

            }
            returnValue += "}";
            return returnValue;
        }
        #endregion


    }
}
