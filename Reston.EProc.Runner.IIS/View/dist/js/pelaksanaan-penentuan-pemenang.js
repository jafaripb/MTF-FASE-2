﻿$(function () {
    $(".ajukan-pemenang").on("click", function () {
        BootstrapDialog.show({
            title: 'Ajukan Pemenang',
            message: 'Dengan menekan tombol "lanjut" dokumen penentuan pemenang ini akan diajukan pada Departemen Head Pengadaaan untuk dimintai persetujuan',
            buttons: [{
                label: 'Lanjutkan',
                action: function (dialog) {
                    ajukanPemenang();
                    dialog.close();
                }
            },{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });

    });
});

function ajukanPemenang() {
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: "Api/PengadaanE/ajukanDokPemenang?Id=" + $("#pengadaanId").val(),
        success: function (data) {    
            window.location.reload();
            waitingDialog.hideloading();
        },
        error: function (errormessage) {
            

        }
    });

}