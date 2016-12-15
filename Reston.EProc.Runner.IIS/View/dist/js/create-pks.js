var PengadaanId = gup("id");
var VendorId = gup("VendorId");

$(function () {
    //$("#pengadaanId").val(PengadaanId);
    //$("#VendorId").val(VendorId);
    if(PengadaanId==""||VendorId=="" )
        window.location.replace("http://" + window.location.host + "/pks.html");
    loadDetail(PengadaanId, VendorId);



    var myDropzonePKS = new Dropzone("#dok-pks",
             {
                 url: $("#dok-pks").attr("action") + "&id=" + PengadaanId,
                 maxFilesize: 10,
                 acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
                 accept: function (file, done) {
                     var jumFile = myDropzonePKS.files.length;
                     if ($("#pksId").val() == "") {
                         BootstrapDialog.show({
                             title: 'Konfirmasi',
                             message: 'Simpan Dahulu Dokumen Sebelum Upload File',
                             buttons: [{
                                 label: 'Close',
                                 action: function (dialog) {
                                     myDropzonePKS.removeFile(file);
                                     dialog.close();

                                 }
                             }]
                         });

                     } else {
                         if (jumFile > 1) {
                             BootstrapDialog.show({
                                 title: 'Konfirmasi',
                                 message: 'Berkas Sudah Ada ',
                                 buttons: [{
                                     label: 'Close',
                                     action: function (dialog) {
                                         myDropzonePKS.removeFile(file);
                                         dialog.close();
                                     }
                                 }]
                             });
                         } else {
                             done();
                         }
                     }
                     
                 },
                 init: function () {
                     this.on("addedfile", function (file) {
                         file.previewElement.addEventListener("click", function () {
                             var id = 0;
                             if (file.Id != undefined)
                                 id = file.Id;
                             else
                                 id = $.parseJSON(file.xhr.response);
                             $("#HapusFile").show();
                             $("#konfirmasiFile").attr("attr1", "PKS");
                             $("#konfirmasiFile").attr("FileId", id);
                             $("#konfirmasiFile").modal("show");
                         });
                     });
                     this.on("complete", function (file) {
                         if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                             isSpkUploaded();
                         }
                     });
                     this.on("error", function (file) {
                         myDropzoneSuratPerintahKerja.removeFile(file);
                     });
                     this.on("success", function (file, responseText) {
                         if (responseText == "00000000-0000-0000-0000-000000000000" || responseText == null) {
                             myDropzonePKS.removeFile(file);
                         }
                     });
                 }
             }
         );

    renderDokumenDropzone(myDropzonePKS, "PKS");
    Dropzone.options.PKS = false;

    $("#HapusFile").on("click", function () {
        var tipe = $(this).parent().parent().parent().parent().attr("attr1");
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/deleteDokumenPelaksanaan?Id=" + FileId
        }).done(function (data) {
            if (data.Id == "1") {

                if (tipe == "PKS") {
                    $.each(myDropzonePKS.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzonePKS.removeFile(item);
                        }
                    });
                }
            }
            $("#konfirmasiFile").modal("hide");
        });
    });

});

function loadDetail(PengadaanId, VendorId){
    $.ajax({
        url: "Api/pks/detail?PengadaanId=" + PengadaanId + "&VendorId=" + VendorId
      }).done(function (data) {
          $("#judul").val(data.Judul);
          $("#no-pengadaan").val(data.NoPengadaan);
          $("#no-spk").val(data.NoSpk);
          $("#pelaksana").val(data.Vendor);
      });
  
}

function renderDokumenDropzone(myDropzone, tipe) {
    $.ajax({
        url: "Api/PengadaanE/getDokumens?Id=" + $("#pengadaanId").val() + "&tipe=" + tipe,
        success: function (data) {
            for (var key in data) {
                var file = {
                    Id: data[key].Id, name: data[key].File, accepted: true,
                    status: Dropzone.SUCCESS, processing: true, size: data[key].SizeFile
                };
                //thisDropzone.options.addedfile.call(thisDropzone, file);
                myDropzone.emit("addedfile", file);
                myDropzone.emit("complete", file);
                myDropzone.files.push(file);
            }
        },
        error: function (errormessage) {

            //location.reload();

        }
    });
}


