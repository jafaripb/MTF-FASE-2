var SpkId = gup("id");
var myDropzoneSPK;
$(function () {
    //$("#pengadaanId").val(PengadaanId);
    //$("#VendorId").val(VendorId);
    if (SpkId != "" && SpkId != null) loadDetail(SpkId);
    if ($("#pksId").val() != "" && (SpkId == null || SpkId == "")) loadDetail($("#spkId").val());
       //window.location.href("http://" + window.location.host + "/pks.html");
   

    $("#HapusFile").on("click", function () {
        var tipe = $(this).parent().parent().parent().parent().attr("attr1");
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");
        $.ajax({
            method: "POST",
            url: "Api/Spk/deleteDokumenSpk?Id=" + FileId
        }).done(function (data) {
            if (data.Id == "1") {
                    $.each(myDropzoneSPK.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzoneSPK.removeFile(item);
                        }
                    });
                }            
            $("#konfirmasiFile").modal("hide");
        });
    });

     myDropzoneSPK = new Dropzone("#dokspk",
             {
                 maxFilesize: 10,
                 acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
                 accept: function (file, done) {
                     this.options.url = $("#dokspk").attr("action") + "?id=" + $("#spkId").val();
                     if ($("#isOwner").val() != 1)
                         BootstrapDialog.show({
                             title: 'Konfirmasi',
                             message: 'Anda Tidak Punya Akses ',
                             buttons: [{
                                 label: 'Close',
                                 action: function (dialog) {
                                     myDropzoneSPK.removeFile(file);
                                     dialog.close();
                                 }
                             }]
                         });
                     else {
                         var jumFile = myDropzoneSPK.files.length;
                         if ($("#pksId").val() == "") {
                             BootstrapDialog.show({
                                 title: 'Konfirmasi',
                                 message: 'Simpan Dahulu Dokumen Sebelum Upload File',
                                 buttons: [{
                                     label: 'Close',
                                     action: function (dialog) {
                                         myDropzoneSPK.removeFile(file);
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
                                             myDropzoneSPK.removeFile(file);
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
                             if ($("#isOwner").val() == 1 && $("#StatusSpk").val() == 0) {
                                 $("#HapusFile").show();
                             } else { $("#HapusFile").hide() }
                         });
                     });

                     this.on("error", function (file) {
                         myDropzoneSPK.removeFile(file);
                     });
                     this.on("success", function (file, responseText) {
                         if (responseText == "00000000-0000-0000-0000-000000000000" || responseText == null) {
                             myDropzoneSPK.removeFile(file);
                         }
                     });
                 }
             }
         );

    renderDokumenDropzone(myDropzoneSPK, "dokspk");
    Dropzone.options.dokspk = false;
});

$(function () {
   // $("#bingkai-upload-legal").hide();
    table = $('#tbl-PKS').DataTable({
        "serverSide": true,
        "searching": false,
        "ajax": {
            "url": 'api/Spk/ListPks',
            "type": 'POST',
            "data": function (d) {
                d.search = $("#title").val();
                d.klasifikasi = $("#klasifikasi option:selected").val();
            }
        },
        "columns": [
            { "data": "Judul" },
            { "data": "NoPks"},
            { "data": "NoPengadaan" },
            { "data": "JenisPekerjaan" },
            { "data": "Vendor" },
            { "data": null }
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    var objData = JSON.stringify(data);
                    return " <a obj='" + objData + "' class='btn btn-success btn-sm pilih-pks '> Pilih </a> ";
                },

                "targets": 5,
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
    $(".cari-pks").on("click", function () {
        $("#pks_modal").modal("show");
    });

    $("#tbl-PKS").on("click", ".pilih-pks", function () {
        console.log($(this).attr("obj"));
        var obj = jQuery.parseJSON($(this).attr("obj"));       
        waitingDialog.showloading("Proses Harap Tunggu");
        $("#judul").val(obj.Judul);
        $("#no-pengadaan").val(obj.NoPengadaan);
        $("#no-spk").val(obj.NoSpk);
        $("#no-pks").val(obj.NoPks);
        $("#pelaksana").val(obj.Vendor);
        $("#PemenangPengadaanId").val(obj.PemenangPengadaanId);
        $("#pks_modal").modal("hide");
        $("#pksId").val(obj.Id);
        //$(".ajukan").hide();
       // $(".Hapus").hide();
        waitingDialog.hideloading();
    });

    $(".Simpan").on("click", function () {
        var spk = {};
        spk.PksId = $("#pksId").val();
        spk.TanggalSPKStr = moment($("#tanggal-spk").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
        spk.NilaiSPK = $("#nilai-spk").val();
        console.log(spk);
        if ($("#isOwner").val() == 1 || $("#spkId").val() == "")
            save(spk);
    });

    if ($("#spkId").val() == "") {
        $(".Simpan").show();
    }
    $(".Hapus").on("click", function () {
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            url: "Api/Spk/delete?Id=" + $("#spkId").val(),
            method: "POST",
        }).done(function (data) {
            window.location.replace("http://" + window.location.host + "/spk.html");
            waitingDialog.hideloading();

        });

    });
    
    $("[name=status]").on("change", function () {
        change();
    });

    $(".date").datetimepicker({
        format: "DD MMMM YYYY",
        locale: 'id',
        useCurrent: false,
        minDate: Date.now()

    });

    $("#downloadFile").on("click", function () {
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");
        downloadFileUsingForm("/api/Spk/OpenFile?Id=" + FileId);
    });
});

function loadDetail(Id) {
    $.ajax({
        url: "Api/spk/detail?Id=" + Id
    }).done(function (data) {
        $("#judul").val(data.Judul);
        $("#no-pengadaan").val(data.NoPengadaan);
        $("#no-spk").val(data.NoSpk);
        $("#pelaksana").val(data.Vendor);
        $("#note").val(data.Note);
        $("#title-pks").val(data.Title);
        $("#pksId").val(data.PksId);
        $("#spkId").val(data.Id);
        $("#isOwner").val(data.isOwner);
        $("#Approver").val(data.Approver);
        $("#no-pks").val(data.NoPks);
        $("[name=status][value=" + data.StatusSpk + "]").prop('checked', true);
        $("#PemenangPengadaanId").val(data.PemenangPengadaanId);
        $("#StatusSpk").val(data.StatusSpk);//$("[name=status]:checked").val()
        $("#tanggal-spk").val(moment(data.TanggalSPK).format("DD MMMM YYYY HH:mm"));
        $("#nilai-spk").val(data.NilaiSPK);
        $("#Status").text(data.StatusSpkName);
        if ($("#isOwner").val() == 0 || data.StatusSpk == 1 || data.StatusSpk == 2) {
            $(".Simpan").remove();
            $(".Hapus").remove();
            $(" .input-spk").attr("disabled", true);
            if ($("#isOwner").val() == 1) {
                $("[name=status][value=1]").attr("disabled", false);
                $("[name=status][value=2]").attr("disabled", false);
            }
        }
        if (data.isOwner == 1 && (data.StatusSpk == 0)) {
            if (data.StatusSpk == 0) {
                $(".Simpan").show();
                $(".ajukan").show();
                $(".Hapus").show();
                $(".input-spk").attr("disabled", false);
                $(".Edit").hide();
            }
        }
        



        $("#Status").text(data.StatusPksName);
    });

}

function renderDokumenDropzone(myDropzone) {
    var rSpkId = SpkId;
    if ($("#spkId").val() !== '') rSpkId = $("#spkId").val();
    $.ajax({
        url: "Api/Spk/getDokumens?Id=" + rSpkId,
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

function save(spk) {
    
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: "Api/spk/Save" ,
        method: "POST",
        data: spk ///JSON.stringify(pks)
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

$("#template-spk").on("click", function () {
    //var tanggal = moment($("#tanggal-spk").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    downloadFileUsingForm("Api/Report/BerkasSPK2?Judul=" + $("#judul").val() + "&Tanggal_SPK=" + $("#tanggal-spk").val() + "&Vendor= " + $("#pelaksana").val() + "&Nilai_SPK=" + $("#nilai-spk").val());
    alert("bgafugsafgsg")
});

function change() {
    var status = $("[name=status]:checked").val();
    
    if (myDropzoneSPK.files.length == 0 && (status == 1 || status==2)) {
        BootstrapDialog.show({
            title: 'Infomasi',
            message: "Harap Upload Dokumen SPK dahulu",
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        $("[name=status][value=0]").prop('checked', true);
        return;
    }
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: "Api/Spk/ChangeSatus?Id=" + $("#spkId").val() + "&status=" + status,
        method: "POST",
    }).done(function (data) {
        loadDetail($("#spkId").val());
        if (data.Id != "") {
            if (status == 1) {
                $(".Hapus").remove();
                $(".Simpan").remove();
              
            }
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
