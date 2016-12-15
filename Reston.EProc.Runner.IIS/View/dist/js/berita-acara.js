
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
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/addBeritaAcara",
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
  
    var myDropzoneBeritaAcaraPenilaian = new Dropzone("#BeritaAcaraPenilaian",
             {
                 url: $("#BeritaAcaraPenilaian").attr("action") + "&id=" + $("#pengadaanId").val(),
                 maxFilesize: 10,
                 acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx,.docx",
                 accept: function (file, done) {
                     if ($("#isPIC").val() == 1) {
                         var cekRekananCheck = 0;
                         $(".checkbox-pilih-kandidat").each(function () {
                             if ($(this).prop('checked') == true) {
                                 cekRekananCheck = cekRekananCheck + 1;
                             }
                         });
                         if (cekRekananCheck > 0) {
                             if ($("#State").val() == 6)
                                 done();
                                 //BootstrapDialog.show({
                                 //    title: 'Konfirmasi',
                                 //    message: 'Dengan mengklik "Lanjutkan" Berarti State akan Berlanjut Ke state Berikutnya!',
                                 //    buttons: [{
                                 //        label: 'Lanjutkan',
                                 //        action: function (dialog) {
                                 //            done();
                                 //            //nextState("klarifikasi");
                                 //            dialog.close();
                                 //        }
                                 //    }, {
                                 //        label: 'Batal',
                                 //        action: function (dialog) {
                                 //            myDropzoneBeritaAcaraPenilaian.removeFile(file);
                                 //            dialog.close();

                                 //        }
                                 //    }]
                                 //});
                             else {
                                 if ($("#State").val() > 6)
                                    done();
                             } 
                         }
                         else {
                             BootstrapDialog.show({
                                 title: 'Konfirmasi',
                                 message: 'Anda Belum Memilih Kandidat untuk Maju Ketahap Klarifikasi',
                                 buttons: [{
                                     label: 'Close',
                                     action: function (dialog) {
                                         myDropzoneBeritaAcaraPenilaian.removeFile(file);
                                         dialog.close();

                                     }
                                 }]
                             });
                         }
                     } else {
                         BootstrapDialog.show({
                             title: 'Konfirmasi',
                             message: 'Anda Tidak Memiliki Akses!',
                             buttons: [{
                                 label: 'Close',
                                 action: function (dialog) {
                                     myDropzoneBeritaAcaraPenilaian.removeFile(file);
                                     dialog.close();
                                 }
                             }]
                         });
                     }
                 },
                 dictDefaultMessage: "Tidak Ada Dokumen",
                 init: function () {
                     this.on("addedfile", function (file) {
                         file.previewElement.addEventListener("click", function () {
                             var id = 0;
                             if (file.Id != undefined)
                                 id = file.Id
                             else
                                 id = $.parseJSON(file.xhr.response)
                             //viewFile(data.Id);
                             $("#HapusFile").show();
                             $("#konfirmasiFile").attr("attr1", "BeritaAcaraPenilaian");
                             $("#konfirmasiFile").attr("FileId", id);
                             $("#konfirmasiFile").modal("show");
                         });
                     });
                     this.on("success", function (file, responseText) {
                         //if($("#State").val()==6)
                          //  nextState("klarifikasi");
                     });
                 }
             }
         );

    renderDokumenDropzone(myDropzoneBeritaAcaraPenilaian, "BeritaAcaraPenilaian");
    Dropzone.options.BeritaAcaraPenilaian = false;

    var myDropzoneBerkasBeritaAcaraPenilaian = new Dropzone("#BerkasBeritaAcaraPenilaian",
          {
              url: $("#BerkasBeritaAcaraPenilaian").attr("action") + "&id=" + $("#pengadaanId").val(),
              maxFilesize: 10,
              acceptedFiles: "",
              clickable: false,
              dictDefaultMessage: "Tidak Ada Dokumen",
              init: function () {
                  this.on("addedfile", function (file) {
                      file.previewElement.addEventListener("click", function () {
                          var id = 0;
                          if (file.Id != undefined)
                              id = file.Id
                          else
                              id = $.parseJSON(file.xhr.response)
                          //viewFile(data.Id);
                          $("#HapusFile").hide();
                          $("#konfirmasiFile").attr("attr1", "BeritaAcaraPenilaian");
                          $("#konfirmasiFile").attr("FileId", id);
                          $("#konfirmasiFile").modal("show");
                      });
                  });
              }
          }
      );

    renderDokumenDropzone(myDropzoneBerkasBeritaAcaraPenilaian, "BeritaAcaraPenilaian");
    Dropzone.options.BerkasBeritaAcaraPenilaian = false;
    
    getListBeritaAcara();
});


function getListBeritaAcara() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetBeritaAcara?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $.each(data, function (index, value) {
                if (value.Tipe == 3) 
                    $("#input-ba-aanwijzing").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 5)
                    $("#input-ba-bukaamplop").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 6)
                    $("#input-ba-penilaian").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 7)
                    $("#input-ba-klarifikasi").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 8)
                    $("#input-ba-nota").val(moment(value.tanggal).format("DD MMMM YYYY"));
                if (value.Tipe == 12)
                    $("#input-ba-spk").val(moment(value.tanggal).format("DD MMMM YYYY"));
            });
        }
    });
}