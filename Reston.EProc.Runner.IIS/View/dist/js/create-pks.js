var PksId = gup("id");

$(function () {
    //$("#pengadaanId").val(PengadaanId);
    //$("#VendorId").val(VendorId);
    if (PksId != "") loadDetail(PksId);
       //window.location.href("http://" + window.location.host + "/pks.html");
    
    var myDropzonePKS = new Dropzone("#DraftPKS",
             {
                 maxFilesize: 10,
                 acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
                 accept: function (file, done) {
                     this.options.url = $("#DraftPKS").attr("action") + "&id=" + $("#pksId").val();
                     if ($("#isOwner").val() != 1)
                         BootstrapDialog.show({
                             title: 'Konfirmasi',
                             message: 'Anda Tidak Punya Akses ',
                             buttons: [{
                                 label: 'Close',
                                 action: function (dialog) {
                                     myDropzonePKS.removeFile(file);
                                     dialog.close();
                                 }
                             }]
                         });
                     else {
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
                    
                     this.on("error", function (file) {
                         myDropzonePKS.removeFile(file);
                     });
                     this.on("success", function (file, responseText) {
                         if (responseText == "00000000-0000-0000-0000-000000000000" || responseText == null) {
                             myDropzonePKS.removeFile(file);
                         }
                     });
                 }
             }
         );

    renderDokumenDropzone(myDropzonePKS, "DraftPKS");
    Dropzone.options.DraftPKS = false;

    var myDropzoneFinalPKS = new Dropzone("#FinalLegalPks",
             {
                 maxFilesize: 10,
                 acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
                 accept: function (file, done) {
                     this.options.url = $("#FinalLegalPks").attr("action") + "&id=" + $("#pksId").val();
                     if ($("#Approver").val() != 1)
                         BootstrapDialog.show({
                             title: 'Konfirmasi',
                             message: 'Anda Tidak Punya Akses ',
                             buttons: [{
                                 label: 'Close',
                                 action: function (dialog) {
                                     myDropzonePKS.removeFile(file);
                                     dialog.close();
                                 }
                             }]
                         });
                     else {
                         var jumFile = myDropzoneFinalPKS.files.length;
                         if ($("#pksId").val() == "") {
                             BootstrapDialog.show({
                                 title: 'Konfirmasi',
                                 message: 'Simpan Dahulu Dokumen Sebelum Upload File',
                                 buttons: [{
                                     label: 'Close',
                                     action: function (dialog) {
                                         myDropzoneFinalPKS.removeFile(file);
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
                                             myDropzoneFinalPKS.removeFile(file);
                                             dialog.close();
                                         }
                                     }]
                                 });
                             } else {
                                 done();
                             }
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

                     this.on("error", function (file) {
                         myDropzoneFinalPKS.removeFile(file);
                     });
                     this.on("success", function (file, responseText) {
                         if (responseText == "00000000-0000-0000-0000-000000000000" || responseText == null) {
                             myDropzoneFinalPKS.removeFile(file);
                         }
                     });
                 }
             }
         );

    renderDokumenDropzone(myDropzoneFinalPKS, "FinalLegalPks");
    Dropzone.options.FinalLegalPks = false;

    var myDropzoneAssignedPks = new Dropzone("#AssignedPks",
             {
                 url: $("#AssignedPks").attr("action") + "&id=" + $("#pksId").val(),
                 maxFilesize: 10,
                 acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
                 accept: function (file, done) {
                     this.options.url = $("#FinalLegalPks").attr("action") + "&id=" + $("#pksId").val();
                     if ($("#isOwner").val() != 1)
                         BootstrapDialog.show({
                             title: 'Konfirmasi',
                             message: 'Anda Tidak Punya Akses ',
                             buttons: [{
                                 label: 'Close',
                                 action: function (dialog) {
                                     myDropzonePKS.removeFile(file);
                                     dialog.close();
                                 }
                             }]
                         });
                     else {
                         var jumFile = myDropzoneAssignedPks.files.length;
                         if ($("#pksId").val() == "") {
                             BootstrapDialog.show({
                                 title: 'Konfirmasi',
                                 message: 'Simpan Dahulu Dokumen Sebelum Upload File',
                                 buttons: [{
                                     label: 'Close',
                                     action: function (dialog) {
                                         myDropzoneAssignedPks.removeFile(file);
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
                                             myDropzoneAssignedPks.removeFile(file);
                                             dialog.close();
                                         }
                                     }]
                                 });
                             } else {
                                 done();
                             }
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

                     this.on("error", function (file) {
                         myDropzoneAssignedPks.removeFile(file);
                     });
                     this.on("success", function (file, responseText) {
                         if (responseText == "00000000-0000-0000-0000-000000000000" || responseText == null) {
                             myDropzoneAssignedPks.removeFile(file);
                         }
                     });
                 }
             }
         );

    renderDokumenDropzone(myDropzoneAssignedPks, "AssignedPks");
    Dropzone.options.AssignedPks = false;

    $("#HapusFile").on("click", function () {
        var tipe = $(this).parent().parent().parent().parent().attr("attr1");
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");
        $.ajax({
            method: "POST",
            url: "Api/Pks/deleteDokumenPks?Id=" + FileId
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

$(function () {
   // $("#bingkai-upload-legal").hide();
    table = $('#tbl-pengadaan').DataTable({
        "serverSide": true,
        "searching": false,
        "ajax": {
            "url": 'api/Pks/ListPengadaan',
            "type": 'POST',
            "data": function (d) {
                d.search = $("#title").val();
                d.klasifikasi = $("#klasifikasi option:selected").val();
            }
        },
        "columns": [
            { "data": "Judul" },
            { "data": "HPS", "className": "rata_kanan" },
            { "data": "JenisPekerjaan" },
            { "data": "Vendor" },
            { "data": null }
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    if (data != null)
                        return accounting.formatNumber(data, { thousand: ".", decimal: ",", precision: 2 })
                    else return "";
                },

                "targets": 1,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    var objData = JSON.stringify(data);
                    return " <a obj='" + objData + "' class='btn btn-success btn-sm pilih-pengadaan '> Pilih </a> ";
                },

                "targets": 4,
                "orderable": false
            }
        ],
        "paging": true,
        "lengthChange": false,
        "ordering": false,
        "info": true,
        "autoWidth": false,
        "responsive": true,
    });
});

$(function () {
    $(".cari-pengadaan").on("click", function () {
        $("#pengadaan_modal").modal("show");
    });

    $("#tbl-pengadaan").on("click", ".pilih-pengadaan", function () {
        console.log($(this).attr("obj"));
        var obj = jQuery.parseJSON($(this).attr("obj"));
        waitingDialog.showloading("Proses Harap Tunggu");
        $("#judul").val(obj.Judul);
        $("#no-pengadaan").val(obj.NoPengadaan);
        $("#no-spk").val(obj.NoSpk);
        $("#pelaksana").val(obj.Vendor);
        $("#PemenangPengadaanId").val(obj.PemenangPengadaanId);
        $("#pengadaan_modal").modal("hide");
        $("#pksId").val("");
        $(".ajukan").hide();
        $(".Hapus").hide();
        waitingDialog.hideloading();
    });

    $(".Simpan").on("click", function () {
        var pks = {};
        pks.Note = $("#note").val();
        pks.Title = $("#title-pks").val();
        pks.PemenangPengadaanId = $("#PemenangPengadaanId").val();
        pks.Id = $("#pksId").val();
        if ($("#isOwner").val() == 1 || $("#pksId").val()=="")
            save(pks);
    });

    $(".ajukan").on("click", function () {
        var pks = {};
        pks.Note = $("#note").val();
        pks.Title = $("#title-pks").val();
        pks.PemenangPengadaanId = $("#PemenangPengadaanId").val();
        pks.Id = $("#pksId").val();
        if ($("#StatusPks").val() == "0")
            ajukan();
    });

    $(".Edit").on("click", function () {
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            url: "Api/Pks/Edit?Id=" + $("#pksId").val(),
            method: "POST",
        }).done(function (data) {
            loadDetail($("#pksId").val());
            waitingDialog.hideloading();
        });

    });
    if ($("#pksId").val() == "") {
        $(".Simpan").show();
    }
    
});

function loadDetail(Id) {
    $.ajax({
        url: "Api/pks/detail?Id=" + Id
    }).done(function (data) {
        $("#judul").val(data.Judul);
        $("#no-pengadaan").val(data.NoPengadaan);
        $("#no-spk").val(data.NoSpk);
        $("#pelaksana").val(data.Vendor);
        $("#note").val(data.Note);
        $("#title-pks").val(data.Title);
        $("#pksId").val(Id);
        $("#isOwner").val(data.isOwner);
        $("#Approver").val(data.Approver);
        
        $("#PemenangPengadaanId").val(data.PemenangPengadaanId);
        $("#StatusPks").val(data.StatusPks);
        if ($("#isOwner").val() == 0) {
            $(".Simpan").remove();
            $(".ajukan").remove();
            $(".Hapus").remove();
            $(".input-pks").attr("disabled", true);
            $(".input-pks").attr("disabled", true);

        }
        if (data.isOwner == 1 && (data.StatusPks == 0 || data.StatusPks == 3)) {
            $("#bingkai-upload-legal").hide();
            if (data.StatusPks == 0) {
                $(".Simpan").show();
                $(".ajukan").show();
                $(".Hapus").show();
                $(".input-pks").attr("disabled", false);
                $(".Edit").hide();
            }
            if (data.StatusPks == 3) {
                $(".Edit").show();
            }
        }
        if (data.StatusPks == 0 || data.StatusPks == 3)
            $("#bingkai-upload-legal").hide();
        if(data.StatusPks != 0 && data.StatusPks != 3) {
            $("#HapusFile").remove();
            $(".input-pks").attr("disabled", true);
            $(".cari-pengadaan").remove();
            $(".Simpan").remove();
            $(".ajukan").remove();
            $(".Hapus").remove();
            if (data.StatusPks == 2) { //kalo udah approve 
                $("#bingkai-upload-legal").show();
            }
        }


        $("#Status").text(data.StatusPksName);
    });

}

function renderDokumenDropzone(myDropzone, tipe) {
    $.ajax({
        url: "Api/Pks/getDokumens?Id=" + PksId + "&tipe="+tipe,
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

function save(pks) {
    
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: "Api/Pks/Save" ,
        method: "POST",
        data:pks ///JSON.stringify(pks)
    }).done(function (data) {
        loadDetail(data.Id);
        var msg = data.message;
        waitingDialog.hideloading();
        BootstrapDialog.show({
            title: 'Infomasi',
            message: msg,
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        
    });
}

function ajukan() {
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: "Api/Pks/ajukan?Id=" + $("#pksId").val(),
        method: "POST",
    }).done(function (data) {
        if (data.Id != "") {
            $(".ajukan").remove();
            $(".Hapus").remove();
            $(".Simpan").remove();
            $("input").attr("disabled", true);
            $("textarea").attr("disabled", true);
        }
        var msg = data.message;
        waitingDialog.hideloading();
        BootstrapDialog.show({
            title: 'Infomasi',
            message: msg,
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });

    });
}
