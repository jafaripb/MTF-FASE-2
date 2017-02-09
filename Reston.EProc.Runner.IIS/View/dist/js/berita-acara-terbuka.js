
$(function () {
    
    $(".dateJadwalNoTime").datetimepicker({
        format: "DD MMMM YYYY",
        locale: 'id'
    });


    $(".save-tgl-ba").on("click", function () {
        var oBeritaAcara = {};
        oBeritaAcara.tanggal = moment($("#" + $(this).attr("attr1")).val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");//"dd/MM/yyyy"
        oBeritaAcara.Tipe = $(this).attr("attr2");
        oBeritaAcara.PengadaanId = $("#pengadaanId").val();
        waitingDialog.showloading("Proses Harap Tunggu");
        var url = "Api/PengadaanE/addBeritaAcara";
        if (oBeritaAcara.Tipe == "SuratPerintahKerja") url = "Api/PengadaanE/addBeritaAcaraSpk";
        //if (oBeritaAcara.Tipe == "BeritaAcaraPenentuanPemenang") url = "Api/PengadaanE/addBeritaAcaraNota";
        $.ajax({
            method: "POST",
            url: url,
            dataType: "json",
            data: JSON.stringify(oBeritaAcara),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //$("#aanwijzing_pelaksanaan").val(moment(data.Mulai).format("DD/MM/YYYY"));
                $(this).attr('checked', true);
                getKandidatPemenang();
                waitingDialog.hideloading();
                waitingDialog.hideloading();
            },
            error: function (errormessage) {
                $(this).attr('checked', false);
                waitingDialog.hideloading();
            }
        });
    });
  
    getListBeritaAcara();
});


function getListBeritaAcara() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetBeritaAcara?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $.each(data, function (index, value) {
                if (value.Tipe == 13)
                    $("#input-ba-pendaftaran").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 3)
                    $("#input-ba-aanwijzing").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 5)
                    $("#input-ba-bukaamplop").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 6)
                    $("#input-ba-penilaian").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 7)
                    $("#input-ba-klarifikasi").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 17)
                    $("#input-ba-klarifikasi-lanjutan").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 8)
                    $("#input-ba-nota").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 12)
                    $("#input-ba-spk").val(moment(value.tanggal).format("DD MMMM YYYY"));
            });
        }
    });
}