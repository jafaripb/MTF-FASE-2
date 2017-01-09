var Id = gup("id");
var tableitem;

$(function () {
    //$("#pengadaanId").val(PengadaanId);
    //$("#VendorId").val(VendorId);
    if (Id != "" && Id != null) loadDetail(Id);
    else SetListBank("");
    //if ($("#Id").val() != "" && (Id == null || Id == "")) loadDetail($("#Id").val());
       //window.location.href("http://" + window.location.host + "/pks.html");

    $("#HapusFile").on("click", function () {
        var tipe = $(this).parent().parent().parent().parent().attr("attr1");
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");
        $.ajax({
            method: "POST",
            url: "Api/PO/deleteDokumenPO?Id=" + FileId
        }).done(function (data) {
            if (data.Id == "1") {
                    $.each(myDropzonePO.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzonePO.removeFile(item);
                        }
                    });
                }            
            $("#konfirmasiFile").modal("hide");
        });
    });

    var myDropzonePO = new Dropzone("#DOKPO",
             {
                 maxFilesize: 10,
                 acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
                 accept: function (file, done) {
                     this.options.url = $("#DOKPO").attr("action") + "?id=" + $("#Id").val();
                     done();
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
                         myDropzonePO.removeFile(file);
                     });
                     this.on("success", function (file, responseText) {
                         if (responseText == "00000000-0000-0000-0000-000000000000" || responseText == null) {
                             myDropzonePO.removeFile(file);
                         }
                     });
                 }
             }
         );

    renderDokumenDropzone(myDropzonePO, "DOKPO");
    Dropzone.options.DOKPO = false;
});

$(function () {
    // $("#bingkai-upload-legal").hide();
    if (Id != "" && Id != null) loadDetail(Id);
    if ($("#Id").val() != "" && (Id == null || Id == "")) Id = $("#Id").val();
    tableitem = $('#table-podetail').DataTable({
        "serverSide": true,
        "searching": false,
        "ajax": {
            "url": 'api/PO/ListItem',
            "type": 'POST',
            "data": function (d) {
                d.PoId = Id;
                d.search = "";
            }
        },
        "columns": [
            { "data": "Kode", "width": "5%" },
            { "data": "NamaBarang", "width": "40%" },
            { "data": "Banyak", "width": "10%" },
            { "data": "Satuan", "width": "10%" },
            { "data": null,"width": "5%"  },
            { "data": null, "width": "10%" },
            { "data": null,"width":"20%" }
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    return accounting.formatNumber(row.Banyak, { thousand: ".", decimal: ",", precision: 2 });
                },

                "targets": 2,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    return accounting.formatNumber(row.Harga , { thousand: ".", decimal: ",", precision: 2 });
                },

                "targets": 4,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    return accounting.formatNumber(row.Harga * row.Banyak, { thousand: ".", decimal: ",", precision: 2 });
                },

                "targets": 5,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    var objData = JSON.stringify(row);
                    return "<a attrData='" + objData + "' class='btn btn-success btn-sm edit-item '>Edit </a> <a attrId='" + row.Id + "' class='btn btn-danger btn-sm delete-item '>Delete </a>";
                },

                "targets": 6,
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
    $(".add-item").on("click", function () {        
        if (!$("#Id").val().trim()) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Harap Simpan Dahulu PO!',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
        }
        else {
            $("#item-modal").modal("show");
        }
    });

    $(".close-item-form").on("click", function () {
        $("#Id").val("");
    });

    $("#table-podetail").on("click", ".pilih-pks", function () {
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
        waitingDialog.hideloading();
    });

    $(".Simpan").on("click", function () {
        var data = {};
        data.Id = $("#Id").val();
        data.Prihal = $("#prihal").val();
        data.Vendor = $("#vendor").val();
        data.NoPO = $("#no-po").val();
        data.TanggalPOstr = moment($("#tanggal-po").val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");
        data.NilaiPO = $("#nilai-po").val();
        data.UP = $("#up").val();
        data.PeriodeDaristr = moment($("#periode-dari").val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");
        data.PeriodeSampaistr = moment($("#periode-sampai").val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");
        data.NamaBank = $("[name='BankInfo.Nama']").val();
        data.AtasNama = $("#atas-nama").val();
        data.NoRekening = $("#no-rekening").val();
        data.AlamatPengirimanBarang = $("#alamat-pengiriman-barang").val();
        data.UPPengirimanBarang = $("#up-pengiriman-barang").val();
        data.TelpPengirimanBarang = $("#telp-pengiriman-barang").val();
        data.AlamatKwitansi = $("#alamat-kwitansi").val();
        data.NPWP = $("#npwp").val();
        data.AlamatPengirimanKwitansi = $("#alamat-pengiriman-kwitansi").val();
        data.UPPengirimanKwitansi = $("#up-pengiriman-kwitansi").val();
        data.Ttd1 = $("#ttd1").val();
        data.Ttd2 = $("#ttd2").val();
        data.Discount = $("#discount").val();
        if ($('#ppn').is(':checked')) 
            data.ppn = 10;
        else 
            data.ppn = 0;
        data.pph = $("#pph").val();
        data.dpp = $("#dpp").val();
        save(data);
        console.log(data.ppn);
    });   

    $(".Hapus").on("click", function () {
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            url: "Api/PO/delete?Id=" + $("#Id").val(),
            method: "POST",
        }).done(function (data) {
            window.location.replace("http://" + window.location.host + "/PO.html");
            waitingDialog.hideloading();
        });

    });
    
    $(".date").datetimepicker({
        format: "DD MMMM YYYY",
        locale: 'id',
        useCurrent: true
    });

    $("#banyak").on("change", function () {
        if (parseFloat($("#banyak").val()) > 0 && parseFloat($("#harga").val()) > 0) {
            $("#jumlah").val(parseFloat($("#banyak").val()) * parseFloat($("#harga").val()));
        }
    });
    $("#harga").on("change", function () {
        if (parseFloat($("#banyak").val()) > 0 && parseFloat($("#harga").val()) > 0) {
            $("#jumlah").val(parseFloat($("#banyak").val()) * parseFloat($("#harga").val()));
        }
    });
});

$(function () {
    //edit-item
    $(".save-item").on("click", function () {
        var data = {};
        if ($("#itemId").val() != "") data.Id = $("#itemId").val();
        data.POId = $("#Id").val();
        data.NamaBarang = $("#nama-barang").val();
        data.Kode = $("#kode").val();
        data.Banyak = $("#banyak").val();
        data.Satuan = $("#satuan").val();
        data.Harga = $("#harga").val();
        saveItem(data);

    });

    $("#table-podetail").on("click", ".edit-item", function () {
        var data = jQuery.parseJSON($(this).attr("attrData"));
        $("#itemId").val(data.Id);
        $("#nama-barang").val(data.NamaBarang);
        $("#kode").val(data.Kode);
        $("#banyak").val(data.Banyak);
        $("#satuan").val(data.Satuan);
        $("#harga").val(data.Harga);
        $("#item-modal").modal("show");
    });

    $("#table-podetail").on("click",".delete-item", function () {
        deleteItem($(this).attr("attrId"));
    });

    $(".ge-no-po").on("click", function () {
        generateNoPO();
    });

    $("#downloadFile").on("click", function () {
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");

        downloadFileUsingForm("/api/PO/OpenFile?Id=" + FileId);
    });
    //downloadFileUsingForm("/api/report/BerkasAanwzjing?Id=" + $("#pengadaanId").val());
});

function SetListBank(namabank) {
    //console.log("SetListBank");
    $.ajax({
        url: "/api/ReferenceData/GetAllBank",
        success: function (data) {
            for (var i in data) {
                if (namabank == data[i].Name) {
                    $("[name='BankInfo.Nama']").append("<option value='" + data[i].Name + "' selected>" + data[i].Name + "</option>");
                }
                else {
                    $("[name='BankInfo.Nama']").append("<option value='" + data[i].Name + "'>" + data[i].Name + "</option>");
                }
            }
        }
    });
}

function loadDetail(Id) {
    $.ajax({
        url: "Api/PO/detail?Id=" + Id
    }).done(function (data) {
        $("#Id").val(data.Id);
        $("#prihal").val(data.Prihal);
        $("#vendor").val(data.Vendor);
        $("#no-po").val(data.NoPO);
        $("#tanggal-po").val(moment(data.TanggalPO).format("DD MMMM YYYY"));
        $("#nilai-po").val(accounting.formatNumber(data.NilaiPO, { thousand: ".", decimal: ",", precision: 2 }));
        $("#up").val(data.UP);
        $("#periode-dari").val(moment(data.PeriodeDari).format("DD MMMM YYYY"));
        $("#periode-sampai").val(moment(data.PeriodeSampai).format("DD MMMM YYYY"));
        $("#atas-nama").val(data.AtasNama);
        $("#no-rekening").val(data.NoRekening);
        $("#alamat-pengiriman-barang").val(data.AlamatPengirimanBarang);
        $("#up-pengiriman-barang").val(data.UPPengirimanBarang);
        $("#telp-pengiriman-barang").val(data.TelpPengirimanBarang);
        $("#alamat-kwitansi").val(data.AlamatKwitansi);
        $("#npwp").val(data.NPWP);
        $("#alamat-pengiriman-kwitansi").val(data.AlamatPengirimanKwitansi);
        $("#up-pengiriman-kwitansi").val(data.UPPengirimanKwitansi);
        $("#ttd1").val(data.Ttd1);
        $("#ttd2").val(data.Ttd2);
        $("#discount").val(data.Discount);
        if (data.PPN != "")
            $("#ppn").attr("checked", true);
        $("#pph").val(data.PPH);
        $("#dpp").val(data.DPP);
        SetListBank(data.NamaBank);
    });

}

function renderDokumenDropzone(myDropzone) {
    var rId = Id;
    if ($("#Id").val() !== '') rId = $("#Id").val();
    $.ajax({
        url: "Api/PO/getDokumens?Id=" + rId,
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

function save(po) {    
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: "Api/po/Save" ,
        method: "POST",
        data: po
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

function saveItem(item) {
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: "Api/po/SaveItem",
        method: "POST",
        data: item
    }).done(function (data) {
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
        tableitem.draw();
        $("#item-modal").modal("hide");
    });
}

function deleteItem(Id) {
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: "Api/po/DeleteItem",
        method: "GET",
    }).done(function (data) {
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

function generateNoPO() {
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: "Api/po/GenerateNoPO?Id="+$("#Id").val(),
        method: "GET",
    }).done(function (data) {
        loadDetail(data.Id)
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
        tableitem.draw();
    });
}

$(function () {
    $("#cetak-po").on("click", function () {
        downloadFileUsingForm("Api/po/Report?Id=" + Id);
    });
});

