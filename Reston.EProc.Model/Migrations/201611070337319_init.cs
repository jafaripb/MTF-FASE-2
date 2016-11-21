namespace Reston.Eproc.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "vendor.Dokumen",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TipeDokumen = c.Int(nullable: false),
                        File = c.String(maxLength: 1000),
                        ContentType = c.String(),
                        Active = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "vendor.Vendor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TipeVendor = c.Int(nullable: false),
                        NomorVendor = c.String(maxLength: 10),
                        Nama = c.String(maxLength: 255),
                        Alamat = c.String(maxLength: 1000),
                        Provinsi = c.String(maxLength: 100),
                        Kota = c.String(maxLength: 100),
                        KodePos = c.String(maxLength: 6),
                        Website = c.String(maxLength: 100),
                        Email = c.String(maxLength: 150),
                        Telepon = c.String(maxLength: 20),
                        StatusAkhir = c.Int(nullable: false),
                        Owner = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "vendor.BankInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NamaBank = c.String(maxLength: 100),
                        Cabang = c.String(maxLength: 100),
                        NomorRekening = c.String(maxLength: 50),
                        NamaRekening = c.String(maxLength: 255),
                        Active = c.Boolean(),
                        Vendor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendor.Vendor", t => t.Vendor_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "vendor.RiwayatPengajuanVendor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Waktu = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Metode = c.Int(nullable: false),
                        Komentar = c.String(maxLength: 1000),
                        Urutan = c.Int(nullable: false),
                        Vendor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendor.Vendor", t => t.Vendor_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "vendor.VendorPerson",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nama = c.String(maxLength: 100),
                        Jabatan = c.String(maxLength: 100),
                        Telepon = c.String(maxLength: 20),
                        Email = c.String(maxLength: 150),
                        Active = c.Boolean(),
                        Vendor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendor.Vendor", t => t.Vendor_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "catalog.AtributSpesifikasi",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nama = c.String(maxLength: 100),
                        Nilai = c.String(maxLength: 255),
                        Grup = c.String(),
                        KategoriSpesifikasi_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("catalog.KategoriSpesifikasi", t => t.KategoriSpesifikasi_Id)
                .Index(t => t.KategoriSpesifikasi_Id);
            
            CreateTable(
                "pengadaan.BeritaAcara",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        tanggal = c.DateTime(),
                        Tipe = c.Int(),
                        NoBeritaAcara = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.BintangPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        UserId = c.Guid(nullable: false),
                        StatusBintang = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.Pengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Judul = c.String(maxLength: 255),
                        Keterangan = c.String(maxLength: 500),
                        AturanPengadaan = c.String(maxLength: 50),
                        AturanBerkas = c.String(maxLength: 50),
                        AturanPenawaran = c.String(maxLength: 50),
                        MataUang = c.String(maxLength: 50),
                        PeriodeAnggaran = c.String(maxLength: 50),
                        JenisPembelanjaan = c.String(maxLength: 50),
                        HpsId = c.Guid(),
                        TitleDokumenNotaInternal = c.String(),
                        UnitKerjaPemohon = c.String(maxLength: 50),
                        TitleDokumenLain = c.String(),
                        Region = c.String(maxLength: 50),
                        Provinsi = c.String(maxLength: 50),
                        KualifikasiRekan = c.String(maxLength: 50),
                        JenisPekerjaan = c.String(maxLength: 50),
                        Status = c.Int(),
                        GroupPengadaan = c.Int(),
                        TitleBerkasRujukanLain = c.String(),
                        NoPengadaan = c.String(),
                        CreatedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedOn = c.DateTime(),
                        ModifiedBy = c.Guid(),
                        TanggalMenyetujui = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.DokumenPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        File = c.String(maxLength: 1000),
                        ContentType = c.String(maxLength: 255),
                        Title = c.String(maxLength: 255),
                        Tipe = c.Int(),
                        SizeFile = c.Long(),
                        VendorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.JadwalPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        tipe = c.String(),
                        Mulai = c.DateTime(),
                        Sampai = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.KandidatPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        VendorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.PersonilPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        PersonilId = c.Guid(),
                        Nama = c.String(),
                        Jabatan = c.String(),
                        tipe = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.RKSHeader",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        Total = c.Decimal(precision: 18, scale: 2),
                        CreateOn = c.DateTime(),
                        CreateBy = c.Guid(),
                        ModifiedOn = c.DateTime(),
                        ModifiedBy = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.RKSDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RKSHeaderId = c.Guid(),
                        ItemId = c.Guid(),
                        item = c.String(),
                        satuan = c.String(),
                        jumlah = c.Decimal(precision: 18, scale: 2),
                        hps = c.Decimal(precision: 18, scale: 2),
                        keterangan = c.String(),
                        CreateOn = c.DateTime(),
                        CreateBy = c.Guid(),
                        ModifiedOn = c.DateTime(),
                        ModifiedBy = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.RKSHeader", t => t.RKSHeaderId)
                .Index(t => t.RKSHeaderId);
            
            CreateTable(
                "vendorreg.CaptchaRegistration",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Text = c.String(maxLength: 10),
                        ExpiredDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.CatatanPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        UserId = c.Int(),
                        Komentar = c.String(),
                        tipeCatatan = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.HargaKlarifikasiRekanan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RKSDetailId = c.Guid(),
                        VendorId = c.Int(),
                        harga = c.Decimal(precision: 18, scale: 2),
                        keterangan = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.HargaRekanan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RKSDetailId = c.Guid(),
                        VendorId = c.Int(),
                        harga = c.Decimal(precision: 18, scale: 2),
                        hargaEncrypt = c.String(),
                        keterangan = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.JadwalPelaksanaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        statusPengadaan = c.Int(),
                        Mulai = c.DateTime(),
                        Sampai = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "catalog.KategoriSpesifikasi",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nama = c.String(maxLength: 150),
                        Deskripsi = c.String(maxLength: 255),
                        ParentKategori_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("catalog.KategoriSpesifikasi", t => t.ParentKategori_Id)
                .Index(t => t.ParentKategori_Id);
            
            CreateTable(
                "pengadaan.KehadiranKandidatAanwijzing",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        VendorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.KreteriaPembobotan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        NamaKreteria = c.String(),
                        Bobot = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.KualifikasiKandidat",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        kualifikasi = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.MessagePengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        Waktu = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Message = c.String(maxLength: 1000),
                        Urutan = c.Int(nullable: false),
                        UserTo = c.Guid(),
                        FromTo = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.NoDokumenGenerator",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.String(),
                        tipe = c.Int(),
                        CreateOn = c.DateTime(),
                        CreateBy = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.PelaksanaanAanwijzing",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        Mulai = c.DateTime(),
                        IsiUndangan = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.PelaksanaanBukaAmplop",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        Mulai = c.DateTime(),
                        Sampai = c.DateTime(),
                        DokomenPengadaanId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.DokumenPengadaan", t => t.DokomenPengadaanId)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId)
                .Index(t => t.DokomenPengadaanId);
            
            CreateTable(
                "pengadaan.PelaksanaanKlarifikasi",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        Mulai = c.DateTime(),
                        Sampai = c.DateTime(),
                        DokomenPengadaanId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.DokumenPengadaan", t => t.DokomenPengadaanId)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId)
                .Index(t => t.DokomenPengadaanId);
            
            CreateTable(
                "pengadaan.PelaksanaanPemilihanKandidat",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        VendorId = c.Int(),
                        CreatedBy = c.Guid(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.PelaksanaanPenilaianKandidat",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        Mulai = c.DateTime(),
                        Sampai = c.DateTime(),
                        DokomenPengadaanId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.DokumenPengadaan", t => t.DokomenPengadaanId)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId)
                .Index(t => t.DokomenPengadaanId);
            
            CreateTable(
                "pengadaan.PelaksanaanSubmitPenawaran",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        Mulai = c.DateTime(),
                        Sampai = c.DateTime(),
                        DokomenPengadaanId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.DokumenPengadaan", t => t.DokomenPengadaanId)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId)
                .Index(t => t.DokomenPengadaanId);
            
            CreateTable(
                "pengadaan.PembatalanPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        Keterangan = c.String(),
                        CreateOn = c.DateTime(nullable: false),
                        CreateBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.PembobotanPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        KreteriaPembobotanId = c.Guid(),
                        Bobot = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.PembobotanPengadaanVendor",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        VendorId = c.Int(),
                        KreteriaPembobotanId = c.Guid(),
                        Nilai = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.PemenangPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        VendorId = c.Int(),
                        CreatedBy = c.Guid(),
                        CreateOn = c.DateTime(),
                        ModifiedBy = c.Guid(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "pengadaan.PenolakanPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        Keterangan = c.String(),
                        status = c.Int(),
                        CreateOn = c.DateTime(nullable: false),
                        CreateBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.PersetujuanBukaAmplop",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "catalog.Produk",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nama = c.String(maxLength: 150),
                        Deskripsi = c.String(maxLength: 255),
                        Satuan = c.String(maxLength: 20),
                        KategoriSpesifikasi_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("catalog.KategoriSpesifikasi", t => t.KategoriSpesifikasi_Id)
                .Index(t => t.Nama)
                .Index(t => t.KategoriSpesifikasi_Id);
            
            CreateTable(
                "catalog.RiwayatHarga",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tanggal = c.DateTime(nullable: false),
                        Harga = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.String(maxLength: 4),
                        Region = c.String(),
                        Sumber = c.String(maxLength: 500),
                        User = c.String(maxLength: 50),
                        Produk_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("catalog.Produk", t => t.Produk_Id)
                .Index(t => t.Produk_Id);
            
            CreateTable(
                "master.ReferenceData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Qualifier = c.String(nullable: false, maxLength: 26),
                        Code = c.String(nullable: false, maxLength: 30),
                        LocalizedName = c.String(nullable: false, maxLength: 100),
                        LocalizedDesc = c.String(maxLength: 256),
                        StringAttr1 = c.String(maxLength: 256),
                        StringAttr2 = c.String(maxLength: 256),
                        StringAttr3 = c.String(maxLength: 256),
                        IntAttr1 = c.Int(),
                        IntAttr2 = c.Int(),
                        IntAttr3 = c.Int(),
                        FlagAttr1 = c.Boolean(),
                        FlagAttr2 = c.Boolean(),
                        FlagAttr3 = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Qualifier, t.Code }, unique: true, name: "IndexQualifierCode");
            
            CreateTable(
                "vendorreg.RegDokumen",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TipeDokumen = c.Int(nullable: false),
                        File = c.String(maxLength: 1000),
                        ContentType = c.String(),
                        Active = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "vendorreg.RegVendor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NoPengajuan = c.String(maxLength: 12),
                        TipeVendor = c.Int(nullable: false),
                        Nama = c.String(maxLength: 255),
                        Alamat = c.String(maxLength: 1000),
                        Provinsi = c.String(maxLength: 100),
                        Kota = c.String(maxLength: 100),
                        KodePos = c.String(maxLength: 6),
                        Website = c.String(maxLength: 100),
                        Email = c.String(maxLength: 150),
                        Telepon = c.String(maxLength: 20),
                        StatusAkhir = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "vendorreg.RegBankInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NamaBank = c.String(maxLength: 100),
                        Cabang = c.String(maxLength: 100),
                        NomorRekening = c.String(maxLength: 50),
                        NamaRekening = c.String(maxLength: 255),
                        Active = c.Boolean(),
                        RegVendor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendorreg.RegVendor", t => t.RegVendor_Id)
                .Index(t => t.RegVendor_Id);
            
            CreateTable(
                "vendorreg.RegRiwayatPengajuanVendor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Waktu = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Metode = c.Int(nullable: false),
                        Komentar = c.String(maxLength: 1000),
                        Urutan = c.Int(nullable: false),
                        RegVendor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendorreg.RegVendor", t => t.RegVendor_Id)
                .Index(t => t.RegVendor_Id);
            
            CreateTable(
                "vendorreg.RegVendorPerson",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nama = c.String(maxLength: 100),
                        Jabatan = c.String(maxLength: 100),
                        Telepon = c.String(maxLength: 20),
                        Email = c.String(maxLength: 150),
                        Active = c.Boolean(),
                        RegVendor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendorreg.RegVendor", t => t.RegVendor_Id)
                .Index(t => t.RegVendor_Id);
            
            CreateTable(
                "pengadaan.ReportPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        Judul = c.String(),
                        User = c.String(),
                        hps = c.Decimal(precision: 18, scale: 2),
                        realitas = c.Decimal(precision: 18, scale: 2),
                        efisiensi = c.Decimal(precision: 18, scale: 2),
                        Pemenang = c.String(),
                        Aanwjzing = c.DateTime(),
                        PembukaanAmplop = c.DateTime(),
                        Klasrifikasi = c.DateTime(),
                        Scoring = c.DateTime(),
                        NotaPemenang = c.DateTime(),
                        SPK = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.RiwayatDokumen",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.Guid(),
                        PengadaanId = c.Guid(),
                        ActionDate = c.DateTime(),
                        Comment = c.String(maxLength: 500),
                        Status = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "pengadaan.RiwayatPengadaan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PengadaanId = c.Guid(),
                        Waktu = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Komentar = c.String(maxLength: 1000),
                        Urutan = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("pengadaan.Pengadaan", t => t.PengadaanId)
                .Index(t => t.PengadaanId);
            
            CreateTable(
                "dbo.DokumenVendors",
                c => new
                    {
                        Dokumen_Id = c.Guid(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Dokumen_Id, t.Vendor_Id })
                .ForeignKey("vendor.Dokumen", t => t.Dokumen_Id, cascadeDelete: true)
                .ForeignKey("vendor.Vendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.Dokumen_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "dbo.RegDokumenRegVendors",
                c => new
                    {
                        RegDokumen_Id = c.Guid(nullable: false),
                        RegVendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RegDokumen_Id, t.RegVendor_Id })
                .ForeignKey("vendorreg.RegDokumen", t => t.RegDokumen_Id, cascadeDelete: true)
                .ForeignKey("vendorreg.RegVendor", t => t.RegVendor_Id, cascadeDelete: true)
                .Index(t => t.RegDokumen_Id)
                .Index(t => t.RegVendor_Id);
            
            CreateTable(
                "vendor.AktaDokumenDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nomor = c.String(maxLength: 100),
                        order = c.Int(nullable: false),
                        Tanggal = c.DateTime(),
                        Notaris = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendor.Dokumen", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "vendor.DokumenDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nomor = c.String(maxLength: 100),
                        MasaBerlaku = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendor.Dokumen", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "vendor.IzinUsahaDokumenDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nomor = c.String(maxLength: 100),
                        MasaBerlaku = c.DateTime(),
                        Instansi = c.String(maxLength: 100),
                        Klasifikasi = c.String(maxLength: 100),
                        Kualifikasi = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendor.Dokumen", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "vendorreg.RegAktaDokumenDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nomor = c.String(maxLength: 100),
                        order = c.Int(nullable: false),
                        Tanggal = c.DateTime(),
                        Notaris = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendorreg.RegDokumen", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "vendorreg.RegDokumenDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nomor = c.String(maxLength: 100),
                        MasaBerlaku = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendorreg.RegDokumen", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "vendorreg.RegIzinUsahaDokumenDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nomor = c.String(maxLength: 100),
                        MasaBerlaku = c.DateTime(),
                        Instansi = c.String(maxLength: 100),
                        Klasifikasi = c.String(maxLength: 100),
                        Kualifikasi = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vendorreg.RegDokumen", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("vendorreg.RegIzinUsahaDokumenDetail", "Id", "vendorreg.RegDokumen");
            DropForeignKey("vendorreg.RegDokumenDetail", "Id", "vendorreg.RegDokumen");
            DropForeignKey("vendorreg.RegAktaDokumenDetail", "Id", "vendorreg.RegDokumen");
            DropForeignKey("vendor.IzinUsahaDokumenDetail", "Id", "vendor.Dokumen");
            DropForeignKey("vendor.DokumenDetail", "Id", "vendor.Dokumen");
            DropForeignKey("vendor.AktaDokumenDetail", "Id", "vendor.Dokumen");
            DropForeignKey("pengadaan.RiwayatPengadaan", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("vendorreg.RegVendorPerson", "RegVendor_Id", "vendorreg.RegVendor");
            DropForeignKey("vendorreg.RegRiwayatPengajuanVendor", "RegVendor_Id", "vendorreg.RegVendor");
            DropForeignKey("dbo.RegDokumenRegVendors", "RegVendor_Id", "vendorreg.RegVendor");
            DropForeignKey("dbo.RegDokumenRegVendors", "RegDokumen_Id", "vendorreg.RegDokumen");
            DropForeignKey("vendorreg.RegBankInfo", "RegVendor_Id", "vendorreg.RegVendor");
            DropForeignKey("catalog.RiwayatHarga", "Produk_Id", "catalog.Produk");
            DropForeignKey("catalog.Produk", "KategoriSpesifikasi_Id", "catalog.KategoriSpesifikasi");
            DropForeignKey("pengadaan.PersetujuanBukaAmplop", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.PemenangPengadaan", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.PelaksanaanSubmitPenawaran", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.PelaksanaanSubmitPenawaran", "DokomenPengadaanId", "pengadaan.DokumenPengadaan");
            DropForeignKey("pengadaan.PelaksanaanPenilaianKandidat", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.PelaksanaanPenilaianKandidat", "DokomenPengadaanId", "pengadaan.DokumenPengadaan");
            DropForeignKey("pengadaan.PelaksanaanPemilihanKandidat", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.PelaksanaanKlarifikasi", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.PelaksanaanKlarifikasi", "DokomenPengadaanId", "pengadaan.DokumenPengadaan");
            DropForeignKey("pengadaan.PelaksanaanBukaAmplop", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.PelaksanaanBukaAmplop", "DokomenPengadaanId", "pengadaan.DokumenPengadaan");
            DropForeignKey("pengadaan.PelaksanaanAanwijzing", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.MessagePengadaan", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.KualifikasiKandidat", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.KehadiranKandidatAanwijzing", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("catalog.KategoriSpesifikasi", "ParentKategori_Id", "catalog.KategoriSpesifikasi");
            DropForeignKey("catalog.AtributSpesifikasi", "KategoriSpesifikasi_Id", "catalog.KategoriSpesifikasi");
            DropForeignKey("pengadaan.JadwalPelaksanaan", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.BintangPengadaan", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.RKSDetail", "RKSHeaderId", "pengadaan.RKSHeader");
            DropForeignKey("pengadaan.RKSHeader", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.PersonilPengadaan", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.KandidatPengadaan", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.JadwalPengadaan", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("pengadaan.DokumenPengadaan", "PengadaanId", "pengadaan.Pengadaan");
            DropForeignKey("vendor.VendorPerson", "Vendor_Id", "vendor.Vendor");
            DropForeignKey("vendor.RiwayatPengajuanVendor", "Vendor_Id", "vendor.Vendor");
            DropForeignKey("dbo.DokumenVendors", "Vendor_Id", "vendor.Vendor");
            DropForeignKey("dbo.DokumenVendors", "Dokumen_Id", "vendor.Dokumen");
            DropForeignKey("vendor.BankInfo", "Vendor_Id", "vendor.Vendor");
            DropIndex("vendorreg.RegIzinUsahaDokumenDetail", new[] { "Id" });
            DropIndex("vendorreg.RegDokumenDetail", new[] { "Id" });
            DropIndex("vendorreg.RegAktaDokumenDetail", new[] { "Id" });
            DropIndex("vendor.IzinUsahaDokumenDetail", new[] { "Id" });
            DropIndex("vendor.DokumenDetail", new[] { "Id" });
            DropIndex("vendor.AktaDokumenDetail", new[] { "Id" });
            DropIndex("dbo.RegDokumenRegVendors", new[] { "RegVendor_Id" });
            DropIndex("dbo.RegDokumenRegVendors", new[] { "RegDokumen_Id" });
            DropIndex("dbo.DokumenVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.DokumenVendors", new[] { "Dokumen_Id" });
            DropIndex("pengadaan.RiwayatPengadaan", new[] { "PengadaanId" });
            DropIndex("vendorreg.RegVendorPerson", new[] { "RegVendor_Id" });
            DropIndex("vendorreg.RegRiwayatPengajuanVendor", new[] { "RegVendor_Id" });
            DropIndex("vendorreg.RegBankInfo", new[] { "RegVendor_Id" });
            DropIndex("master.ReferenceData", "IndexQualifierCode");
            DropIndex("catalog.RiwayatHarga", new[] { "Produk_Id" });
            DropIndex("catalog.Produk", new[] { "KategoriSpesifikasi_Id" });
            DropIndex("catalog.Produk", new[] { "Nama" });
            DropIndex("pengadaan.PersetujuanBukaAmplop", new[] { "PengadaanId" });
            DropIndex("pengadaan.PemenangPengadaan", new[] { "PengadaanId" });
            DropIndex("pengadaan.PelaksanaanSubmitPenawaran", new[] { "DokomenPengadaanId" });
            DropIndex("pengadaan.PelaksanaanSubmitPenawaran", new[] { "PengadaanId" });
            DropIndex("pengadaan.PelaksanaanPenilaianKandidat", new[] { "DokomenPengadaanId" });
            DropIndex("pengadaan.PelaksanaanPenilaianKandidat", new[] { "PengadaanId" });
            DropIndex("pengadaan.PelaksanaanPemilihanKandidat", new[] { "PengadaanId" });
            DropIndex("pengadaan.PelaksanaanKlarifikasi", new[] { "DokomenPengadaanId" });
            DropIndex("pengadaan.PelaksanaanKlarifikasi", new[] { "PengadaanId" });
            DropIndex("pengadaan.PelaksanaanBukaAmplop", new[] { "DokomenPengadaanId" });
            DropIndex("pengadaan.PelaksanaanBukaAmplop", new[] { "PengadaanId" });
            DropIndex("pengadaan.PelaksanaanAanwijzing", new[] { "PengadaanId" });
            DropIndex("pengadaan.MessagePengadaan", new[] { "PengadaanId" });
            DropIndex("pengadaan.KualifikasiKandidat", new[] { "PengadaanId" });
            DropIndex("pengadaan.KehadiranKandidatAanwijzing", new[] { "PengadaanId" });
            DropIndex("catalog.KategoriSpesifikasi", new[] { "ParentKategori_Id" });
            DropIndex("pengadaan.JadwalPelaksanaan", new[] { "PengadaanId" });
            DropIndex("pengadaan.RKSDetail", new[] { "RKSHeaderId" });
            DropIndex("pengadaan.RKSHeader", new[] { "PengadaanId" });
            DropIndex("pengadaan.PersonilPengadaan", new[] { "PengadaanId" });
            DropIndex("pengadaan.KandidatPengadaan", new[] { "PengadaanId" });
            DropIndex("pengadaan.JadwalPengadaan", new[] { "PengadaanId" });
            DropIndex("pengadaan.DokumenPengadaan", new[] { "PengadaanId" });
            DropIndex("pengadaan.BintangPengadaan", new[] { "PengadaanId" });
            DropIndex("catalog.AtributSpesifikasi", new[] { "KategoriSpesifikasi_Id" });
            DropIndex("vendor.VendorPerson", new[] { "Vendor_Id" });
            DropIndex("vendor.RiwayatPengajuanVendor", new[] { "Vendor_Id" });
            DropIndex("vendor.BankInfo", new[] { "Vendor_Id" });
            DropTable("vendorreg.RegIzinUsahaDokumenDetail");
            DropTable("vendorreg.RegDokumenDetail");
            DropTable("vendorreg.RegAktaDokumenDetail");
            DropTable("vendor.IzinUsahaDokumenDetail");
            DropTable("vendor.DokumenDetail");
            DropTable("vendor.AktaDokumenDetail");
            DropTable("dbo.RegDokumenRegVendors");
            DropTable("dbo.DokumenVendors");
            DropTable("pengadaan.RiwayatPengadaan");
            DropTable("pengadaan.RiwayatDokumen");
            DropTable("pengadaan.ReportPengadaan");
            DropTable("vendorreg.RegVendorPerson");
            DropTable("vendorreg.RegRiwayatPengajuanVendor");
            DropTable("vendorreg.RegBankInfo");
            DropTable("vendorreg.RegVendor");
            DropTable("vendorreg.RegDokumen");
            DropTable("master.ReferenceData");
            DropTable("catalog.RiwayatHarga");
            DropTable("catalog.Produk");
            DropTable("pengadaan.PersetujuanBukaAmplop");
            DropTable("pengadaan.PenolakanPengadaan");
            DropTable("pengadaan.PemenangPengadaan");
            DropTable("pengadaan.PembobotanPengadaanVendor");
            DropTable("pengadaan.PembobotanPengadaan");
            DropTable("pengadaan.PembatalanPengadaan");
            DropTable("pengadaan.PelaksanaanSubmitPenawaran");
            DropTable("pengadaan.PelaksanaanPenilaianKandidat");
            DropTable("pengadaan.PelaksanaanPemilihanKandidat");
            DropTable("pengadaan.PelaksanaanKlarifikasi");
            DropTable("pengadaan.PelaksanaanBukaAmplop");
            DropTable("pengadaan.PelaksanaanAanwijzing");
            DropTable("pengadaan.NoDokumenGenerator");
            DropTable("pengadaan.MessagePengadaan");
            DropTable("pengadaan.KualifikasiKandidat");
            DropTable("pengadaan.KreteriaPembobotan");
            DropTable("pengadaan.KehadiranKandidatAanwijzing");
            DropTable("catalog.KategoriSpesifikasi");
            DropTable("pengadaan.JadwalPelaksanaan");
            DropTable("pengadaan.HargaRekanan");
            DropTable("pengadaan.HargaKlarifikasiRekanan");
            DropTable("pengadaan.CatatanPengadaan");
            DropTable("vendorreg.CaptchaRegistration");
            DropTable("pengadaan.RKSDetail");
            DropTable("pengadaan.RKSHeader");
            DropTable("pengadaan.PersonilPengadaan");
            DropTable("pengadaan.KandidatPengadaan");
            DropTable("pengadaan.JadwalPengadaan");
            DropTable("pengadaan.DokumenPengadaan");
            DropTable("pengadaan.Pengadaan");
            DropTable("pengadaan.BintangPengadaan");
            DropTable("pengadaan.BeritaAcara");
            DropTable("catalog.AtributSpesifikasi");
            DropTable("vendor.VendorPerson");
            DropTable("vendor.RiwayatPengajuanVendor");
            DropTable("vendor.BankInfo");
            DropTable("vendor.Vendor");
            DropTable("vendor.Dokumen");
        }
    }
}
