var id_pengadaan = window.location.hash.replace("#", "");
var app = angular.module("app", []);

function getListKandidat() {
    arrKandidatPengadaan = [];
    $(".list-kandidat").each(function () {
        objKandidatPengadaan = {};
        objKandidatPengadaan.VendorId = $(this).val();
        arrKandidatPengadaan.push(objKandidatPengadaan);
    });
    return arrKandidatPengadaan;
}

function getListPersonil() {
    arrPersonilPengadaan = [];
    $(".list-personil").each(function () {
        objPersonilPengadaan = {};
        objPersonilPengadaan.PersonilId = $(this).val();
        objPersonilPengadaan.tipe = $(this).attr("attr1");
        arrPersonilPengadaan.push(objPersonilPengadaan);
    });
    return arrPersonilPengadaan;
}

function getJadwal() {
    arrJadwalPengadaan = [];
    $(".jadwal").each(function () {
        objJadwalPengadaan = {};
        objJadwalPengadaan.tipe = $(this).attr("attr1");
        objJadwalPengadaan.Mulai = $(this).attr("attr2");
        objJadwalPengadaan.Sampai = $(this).attr("attr3");
        arrJadwalPengadaan.push(objJadwalPengadaan);
    });
    console.log(arrJadwalPengadaan);
    return arrJadwalPengadaan;
}

function save(pengadaanHeader, attr1, status) {
    //var pengadaan = pengadaanHeader.Pengadaan;
    pengadaanHeader.VWKandidatPengadaans = getListKandidat();
    pengadaanHeader.VWPersonilPengadaans = getListPersonil();
    pengadaanHeader.VWJadwalPengadaans = getJadwal();

    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/save?status=" + status,
        dataType: "json",
        data: JSON.stringify(pengadaanHeader),
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (attr1 == "showmodal") {
            $("#format-rks").attr("href", "rks.html#" + data.Id);
            $("#hpsmodal-list").modal("show");
        } else {
            $("#pengadaanId").val(data.Id);
            alert(data.message);
        }

        // alert(data.status);
    });
}

function getHeaderPengadaan() {
    var viewPengadaan = {};
    viewPengadaan.Judul = $("[name=Judul]").val();
    viewPengadaan.Keterangan = $("[name=Keterangan]").val();
    viewPengadaan.AturanPengadaan = $("[name=AturanPengadaan]").val();
    viewPengadaan.AturanBerkas = $("[name=AturanBerkas]").val();
    viewPengadaan.AturanPenawaran = $("[name=AturanPenawaran]").val();
    viewPengadaan.MataUang = $("[name=MataUang]").val();
    viewPengadaan.PeriodeAnggaran = $("[name=PeriodeAnggaran]").val();
    viewPengadaan.JenisPembelanjaan = $("[name=JenisPembelanjaan]").val();
    viewPengadaan.HpsId = $("[name=HpsId]").val();
    viewPengadaan.TitleDokumenNotaInternal = $("[name=TitleDokumenNotaInternal]").val();
    viewPengadaan.IdDokumenNotaInternal = $("[name=IdDokumenNotaInternal]").val();
    viewPengadaan.UnitKerjaPemohon = $("[name=UnitKerjaPemohon]").val();
    viewPengadaan.TitleDokumenLain = $("[name=TitleDokumenLain]").val();
    viewPengadaan.IdDokumenLain = $("[name=IdDokumenLain]").val();
    viewPengadaan.ContentBerkasDokumenLain = $("[name=IdDokumenLain]").attr("attr1");
    viewPengadaan.Region = $("[name=Region]").val();
    viewPengadaan.Provinsi = $("[name=Provinsi]").val();
    viewPengadaan.KualifikasiRekan = $("[name=KualifikasiRekan]").val();
    viewPengadaan.JenisPekerjaan = $("[name=JenisPekerjaan]").val();
    viewPengadaan.TitleBerkasRujukanLain = $("[name=TitleBerkasRujukanLain]").val();
    viewPengadaan.ContentBerkasRujukanLain = $("[name=TitleBerkasRujukanLain]").attr("attr1");
    viewPengadaan.IdBerkasRujukanLain = $("[name=IdBerkasRujukanLain]").val();
    viewPengadaan.ContentTypeDokumenNotaInternal = $("[name=IdBerkasRujukanLain]").attr("attr1");

    if ($("#pengadaanId").val() != "") viewPengadaan.Id = $("#pengadaanId").val();
    return viewPengadaan;
}


function loadData() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/detailPengadaan?Id=" + id_pengadaan,
        dataType: "json"
    }).done(function (data) {
        $("#judul").val(data.Judul);
       // $("#deskripsi").text(data.Id + ", " + data.AturanPengadaan + ", " + data.AturanBerkas + ", " + data.AturanPenawaran);
        $("#keterangan").val(data.Keterangan);
        $("#PeriodeAnggaran").text(data.PeriodeAnggaran);
        $("#JenisPembelanjaan").text(data.JenisPembelanjaan);
        $("#UnitKerjaPemohon").text(data.UnitKerjaPemohon);
        $("#Region").text(data.Region);
        $("#Provinsi").text(data.Provinsi);
        $("#KualifikasiRekan").text(data.KualifikasiRekan);
        $("#JenisPekerjaan").text(data.JenisPekerjaan);
        loadDokumen(data.IdDokumenNotaInternal, $("#TitleDokumenNotaInternal"), $("#urlDokumenNotaInternal"));
        loadDokumen(data.IdDokumenLain, $("#TitleDokumenLain"), $("#urlDokumenLain"));
        loadDokumen(data.IdBerkasRujukanLain, $("#BerkasRujukanLain"), $("#urlBerkasRujukanLain"));

    });
}

function loadDokumen(Id, eltitle, elurl) {
    $.ajax({
        url: "Api/PengadaanE/getDokumenPengadaan?Id=" + Id
    }).done(function (data) {
        eltitle.text(data.title);
        elurl.attr("attr1", data.DocumentId)
    });
}

function loadKandidat() {
    $.ajax({
        url: "Api/PengadaanE/getKandidatPengadaanList?Id=" + id_pengadaan
    }).done(function (data) {

    });
}

function loadJadwal() {
    $.ajax({
        url: "Api/PengadaanE/getJadwalList?Id=" + id_pengadaan
    }).done(function (data) {
        for (var i in data) {
            var tgl = "";
            var dateMulai;
            var dateSampai;
            if (data[i].Mulai != null) {
                dateMulai = new Date(parseInt(data[i].Mulai.substr(6)));
                dateMulai = dateMulai.getDate() + "/" + (dateMulai.getMonth() + 1) + "/" + dateMulai.getFullYear();
                tgl = tgl + data[i].Mulai;
            }
            if (data[i].Sampai != null) {
                dateSampai = new Date(parseInt(data[i].Sampai.substr(6)));
                dateSampai = dateSampai.getDate() + "/" + (dateSampai.getMonth() + 1) + "/" + dateSampai.getFullYear();
                tgl = tgl + " s/d " + data[i].Sampai;
            }
            console.log(tgl);
            if (data[i].tipe == "Aanwijzing") {
                if (data[i].Mulai != null)
                    $("#Aanwijzing").text(tgl);
            }
            if (data[i].tipe == "Pengisian Harga") {
                $("#PengisianHarga").text(tgl);
            }
            if (data[i].tipe == "Buka Amplop") {
                $("#BukaAmplop").text(tgl);
            }
            if (data[i].tipe == "Klarifikasi") {
                $("#Klarifikasi").text(tgl);
            }
            if (data[i].tipe == "Penentuan Pemenang") {
                $("#PenentuanPemenang").text(tgl);
            }
        }
    });
}

function loadPersonil() {
    $.ajax({
        url: "Api/PengadaanE/getKandidatPengadaanList?Id=" + id_pengadaan
    }).done(function (data) {

    });
}

function loadStatusPEngadaan() {
    $.ajax({
        url: "Api/PengadaanE/getStatusPengadaan?Id=" + id_pengadaan
    }).done(function (data) {
        if (data.status = "Draft") {
            $("#ajukan").show();
            $("#edit").show();
            var href = $("#edit").attr("href") + "#" + id_pengadaan;
            $("#edit").attr("href", href);
            $("#tab-pelakasanaan").removeAttr("data-toggle");
            $("#tab-berkas").removeAttr("data-toggle");
        }
    });
}
$(function () {
    loadJadwal();
    $(".dateJadwal").datepicker({
        showOtherMonths: true,
        format: "dd/mm/yyyy",
        changeYear: true,
        changeMonth: true,
        yearRange: "-90:+4", //+4 Years from now
        //onSelect: function (dateStr) {
        //    alert("sdsd");
        //    console.log(this);
        //    $('.Simpan').validate().element(this);
        //}
    }).on('changeDate', function (ev) {
        var attr = $(this).attr("attr1");
        if (attr == "dari")
            $(this).parent().parent().find(".jadwal").attr("attr2", $(this).val());
        else if (attr == "sampai")
            $(this).parent().parent().find(".jadwal").attr("attr3", $(this).val());
    });

    //$("#formPengadaan").submit(function (event) {
    //    console.log($(this).serializeArray());
    //    event.preventDefault();
    //});

    $(".Simpan").on("click", function () {
        //var str = $("#formPengadaan").serializeArray()();
        //console.log(str);       
        save(getHeaderPengadaan(), "", "Draft");
    });
    $(".SimpanAjukan").on("click", function () {
        save(getHeaderPengadaan(), "", "Ajukan");
    });
    $("#buat_hps").on("click", function () {
        $("#hpsmodal").modal("show");
    });
    $("#simpanhps").on("click", function () {
        $("#hpsmodal").modal("hide");
        save(getHeaderPengadaan(), "showmodal");

    });
});