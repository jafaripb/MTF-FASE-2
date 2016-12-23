var PksId = gup("id");

$(function () {
    //$("#pengadaanId").val(PengadaanId);
    //$("#VendorId").val(VendorId);
    if (PksId != "") loadDetail(PksId);
       //window.location.href("http://" + window.location.host + "/pks.html");
   

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
    $(".cari-pengadaan").on("click", function () {
        $("#pengadaan_modal").modal("show");
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
    $(".Hapus").on("click", function () {
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            url: "Api/Pks/delete?Id=" + $("#pksId").val(),
            method: "POST",
        }).done(function (data) {
            loadDetail($("#pksId").val());
            window.location.replace("http://" + window.location.host + "/pks.html");
            waitingDialog.hideloading();

        });

    });
    

    $(".date").datetimepicker({
        format: "DD MMMM YYYY HH:mm",
        locale: 'id',
        useCurrent: false,
        minDate: Date.now()

    })
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
