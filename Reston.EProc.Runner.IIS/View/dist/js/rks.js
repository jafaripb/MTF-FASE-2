var id_rks = window.location.hash.replace("#", "");
var status;
var table;
$(function () {

    //loadData();
    if (isGuid(id_rks)) {
        $("#pengadaanId").val(id_rks);
        loadData(id_rks);
        loadDataRks(id_rks);
    }
    else {
        if (isGuid($("#pengadaanId").val())) {
            window.location.hash = $("#pengadaanId").val();
            loadData($("#pengadaanId").val());
            loadDataRks($("#pengadaanId").val());
        }
        else {

          //  window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
            //$(location).attr('href', window.location.origin + "pengadaan-list.html");
        }
    }

    $("#cetak-rks").on("click", function () {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Pilih Tipe Dokumen <a id="CetakWord" class="btn btn-app bg-blue"><i class="fa fa-file-word-o"></i> Word</a> <a id="CetakXls" class="btn btn-app bg-green"><i class="fa fa-file-excel-o"></i> Excel</a>',
            buttons: [{
                label: 'Close',
                action: function (dialog) {

                    dialog.close();
                }
            }]
        });
    });

    $("body").on("click", "#CetakWord", function () {
        downloadFileUsingForm("/api/report/CetakHPS?Id=" + $("#pengadaanId").val());
    });

    $("body").on("click", "#CetakXls", function () {
        downloadFileUsingForm("/api/report/CetakHPSXLS?Id=" + $("#pengadaanId").val());
    });
    $("#myNav").affix({
        offset: {
            top: 100
        }
    });
    table = $('#example1').DataTable({
        "paging": false,
        "lengthChange": false,
        "searching": false,
        "ordering": false,
        "info": false,
        "autoWidth": true,
        //responsive: true,
        "ajax": ($("#pengadaanId").val() == "") ? "Api/PengadaanE/getRks?id=" + id_rks : "Api/PengadaanE/getRks?id=" + $("#pengadaanId").val(),
        //"ajax": "data/rks2.txt",
        "columns": [
             { "data": "judul" },
             { "data": "item" },
             { "data": "satuan" },
             { "data": "jumlah", "className": "tengah" },
             { "data": "hps", "className": "rata_kanan" },
             { "data": "total", "className": "rata_kanan" },
             { "data": "keterangan", "className": "rata_kiri" },
             { "data": "null" }
        ],
        "columnDefs": [
            {
                "render": function (data, type, row, meta) {
                    if (row.level == 0) {
                        //$("#example1 tr:eq(" + (meta.row+1) + ") td:eq(0)").attr("colspan", "7");
                        //$("#example1 tr:eq(" + (meta.row +1)+ ") td").not(":eq(0)").remove();
                        return '<input type="text" class="form-control item namaJudul" value="' + row.judul + '" style="font-weight:600; text-align:left; width:160px;">';
                    }
                    else return '';
                },
                "targets": 0,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    if (row.level == 1) {
                        return '<input type="text" class="form-control item namaItem" value="' + row.item + '">';
                    }
                    else return '';
                },
                "targets": 1,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    if (row.level == 1) {
                        return '<input type="text" class="form-control item satuan" value="' + row.satuan + '" style="text-align:center; width:120px;">';
                    }
                    else return '';
                },
                "targets": 2,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    if (row.level == 1) {
                        return '<input type="text" class="form-control item jumlah" value="' + row.jumlah + '" style="width:100px; text-align:center;">';
                    }
                    else return "";
                },
                "targets": 3,
                "orderable": false
            },
             {
                 "render": function (data, type, row) {
                     if (row.level == 1) {
                         return '<input type="text" class="form-control item hps" value="' + row.hps + '" style="width:150px;">';
                     }
                     else if (row.level == 2) {
                         return "Sub Total";
                     }
                     else return "";
                 },
                 "targets": 4,
                 "orderable": false
             },
             {
                 "render": function (data, type, row) {
                     if (row.level == 0) {
                         return "";
                     }
                     if (row.level == 2) {
                         return accounting.formatNumber(row.total, { thousand: ".", decimal: ",", precision: 2 });
                     }
                     if (row.hps == "" || row.jumlah == "" || row.jumlah == null || row.hps == null) {
                         return row.total;
                     }
                     else {
                         var xtoot = "";
                         if (row.total != null) {
                             xtoot = row.total.toString().replace(".", "");
                             xtoot = xtoot.toString().replace(",", ".");
                         }
                         if ($.isNumeric(xtoot) && xtoot != "") {
                             return accounting.formatNumber(xtoot, { thousand: ".", decimal: ",", precision: 2 });
                         }
                         var xJum = row.jumlah.toString().replace(".", "");
                         xJum = row.jumlah.toString().replace(",", ".");
                         var xHps = row.hps.toString().replace(".", "");
                         xHps = row.hps.toString().replace(",", ".");
                         var tot = xJum * xHps;

                         return accounting.formatNumber(tot, { thousand: ".", decimal: ",", precision: 2 });
                     }
                 },
                 "targets": 5,
                 "orderable": false
             },
             {
                 "render": function (data, type, row) {
                     if (row.level == 1) {
                         return '<textarea class="form-control keterangan" >' + row.keterangan + ' </textarea>';
                     }
                     else return "";
                 },
                 "targets": 6,
                 "orderable": false
             },
             {
                 "render": function (data, type, row) {
                     var elSisipAtas = ' <a class="btn btn-xs btn-primary sisip-item-atas" attrJudul="' + row.judul + '" title="Tambah Item Atas"><span class="fa fa-hand-o-up"></span></a> ';
                     var elSisipBawah = ' <a class="btn btn-xs btn-success sisip-item-bawah" attrJudul="' + row.judul + '" title="Tambah Item Bawah"><span class="fa fa-hand-o-down"></span></a> ';
                     var elRemoveITem = ' <a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a> ';
                     var Html = "";
                     if (row.level == 0) {
                         Html = elSisipBawah + elRemoveITem;
                     }
                     if (row.level == 1) {
                         Html = elSisipAtas + elSisipBawah + elRemoveITem;
                     }
                     if (row.level == 2) {
                         Html = elSisipAtas  + elRemoveITem;
                     }
                     return Html;
                 },
                 "targets": 7,
                 "orderable": false
             }
        ],
        "rowCallback": function (row, data, index) {
            hitungHargaItemAwal();
            Hitung();
        }
    });

    $(".datepicker").datepicker({
        showOtherMonths: true,
        format: "dd/mm/yy",
        changeYear: true,
        changeMonth: true,
        yearRange: "-90:+4", //+4 Years from now
        onSelect: function (dateStr) {
            $('#form_reg').validate().element(this);
        }
    });
    $().UItoTop({ easingType: 'easeOutQuart' });

    $(".add-judul").on("click", function () {
        addNewJudul(parseInt($(this).attr("attr")));
        $(this).attr("attr", parseInt($(this).attr("attr")) + 1);
    });

    function addNewJudul(lastgrup) {
        table.row.add({
            "judul": '',
            "item": '',
            "satuan": '',
            "jumlah": '',
            "hps": '',
            "total": 0,
            "keterangan": '',
            "level": 0,
            "grup": lastgrup + 1
        }).draw();
        var jumRow = table.data().length;
        table.cell((jumRow - 1), 7).data(' <a class="btn btn-xs btn-success sisip-item" title="Tambah Item"><span class="fa fa-plus"></span></a> ' +
                ' <a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a> ').draw();
        // $("#example1 tbody tr:eq('" + (jumRow - 1) + "') td:eq(0)").find("input").focus();
        subTotal(lastgrup);
    }

    function subTotal(lastgrup) {
        var newData = {};
        newData.RKSHeaderId = $("#idRks").val();
        newData.judul = '';
        newData.item = '';
        newData.satuan = '';
        newData.jumlah = '';
        newData.hps = 'Sub Total';
        newData.total = '0';
        newData.keterangan = '';
        newData.action = '';
        newData.level = 2;
        newData.grup = lastgrup + 1;
        var currentPage = table.page();
        var baris = $(this).parent().closest("tr").index();
        table.row.add(newData).draw();
        var currentPage = table.page();
    }

    $("#example1").on("change", ".item", function () {
        var elRow = $(this).parent().closest("tr");
        var oldRowData = table.row(elRow.index()).data();
        var totalHarga = (parseFloat($(elRow).find(".jumlah").val()) * parseFloat($(elRow).find(".hps").val()));
        var newRowData = {};
        newRowData.judul = '' + $(elRow).find(".namaJudul").val();
        if (oldRowData.item == "") {
            newRowData.item = "";
            newRowData.satuan = "";
            newRowData.jumlah = "";
            newRowData.hps = "";
            newRowData.keterangan = "";
        }
        else if (newRowData.judul == "undefined" || newRowData.judul == "") {
            if ($(elRow).find(".jumlah").val() == "") {
            }
            else if ($(elRow).find(".hps").val() == "0") {
                newRowData.hps = $(elRow).find(".hps").val();
                newRowData.jumlah = $(elRow).find(".jumlah").val();
                newRowData.keterangan = $(elRow).find(".keterangan").val();
                newRowData.total = totalHarga;
            }
            else if ($(elRow).find(".jumlah").val() != "" || $(elRow).find(".hps").val() != "0") {
                newRowData.jumlah = $(elRow).find(".jumlah").val();
                newRowData.hps = $(elRow).find(".hps").val();
                newRowData.keterangan = $(elRow).find(".keterangan").val();
                newRowData.total = totalHarga;
            }
        }
        else {
            newRowData.item = $(elRow).find(".namaItem").val();
            newRowData.satuan = $(elRow).find(".satuan").val();
            newRowData.jumlah = $(elRow).find(".jumlah").val();
            newRowData.hps = $(elRow).find(".hps").val();
            newRowData.keterangan = $(elRow).find(".keterangan").val();
            newRowData.total = totalHarga;
        }
        addNewData(elRow.index(), newRowData);
        Hitung();
    });

    $("#example1").on('click.autocomplete', ".namaItem", function () {
        $(this).autocomplete({
            //source: "data/item.txt",
            //source:"api/Produk/GetAllProduk",
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: 'api/PengadaanE/GetItemByRegion?Region=' + $("#region").val(),
                    data: request,
                    success: function (data) {
                        //var ParsedObject = $.parseJSON(data);
                        response($.map(data.aaData, function (item) {
                            return {
                                ItemId: item.Id,
                                label: item.Nama,
                                region: item.Region,
                                satuan: item.Satuan,
                                harga: item.Price,
                                spesifikasi: item.Spesifikasi
                            }
                        }))
                    }
                });
            },
            select: function (event, ui) {
                var baris = $(this).parent().closest("tr").index();
                var oldRowData = table.row(baris).data();

                $(this).focus();
                var newData = {};

                newData.Id = oldRowData.Id;

                newData.judul = ui.item.judul;

                newData.ItemId = ui.item.ItemId;
                newData.RKSHeaderId = $("#idRks").val();
                newData.hps = ui.item.harga;
                newData.item = ui.item.label;
                newData.jumlah = 1;
                newData.satuan = ui.item.satuan;
                newData.total = ui.item.harga;
                newData.keterangan = ui.item.spesifikasi;
                addNewData(baris, newData);

                Hitung();

                return false;
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            var html = '<div class="vendor">' +
                    '<div class="box-typehead-content">' +
                      '<span class="box-typehead-title-auto">' + item.label + '</span>' +
                      '<span class="box-typehead-desk-auto"> ' + item.region + ' /</span>' +
                      '<span class="box-typehead-desk-auto"> ' + item.satuan + ' /</span>' +
                      '<span class="box-typehead-desk-auto"> ' + item.harga + '</span>' +
                    '</div>' +
                '</div>'
            return $("<li class='item'>")
                .data("item.autocomplete", item)
                .append(html)
                .appendTo(ul);

        };

    });

    function Hitung() {
        var total = 0;
        var index = 0;
        var subtotal = 0;
        var before_grup = 0;
        table.rows().every(function () {
            var d = this.data();
            var current_grup = d.grup;
           
            if (before_grup != 0 && before_grup == current_grup && d.level == 1) {
                subtotal = subtotal + parseFloat(d.total);
                total = total + parseFloat(d.total);
            }
            else {
                if (before_grup == 0 && d.level == 1) {
                    subtotal = subtotal + parseFloat(d.total);  
                }
                else {
                    if (d.level == 2) {
                        var oldRowData = table.row(index).data();
                        oldRowData.total = subtotal;
                        addNewData(index, oldRowData);
                        subtotal = 0;
                    }
                }
            }
            index++;
            before_grup = current_grup;
            this.invalidate(); // invalidate the data DataTables has cached for this row       
        });
        $(".add-judul").attr("attr", before_grup + 1);
        $('#totalRKS').text(accounting.formatNumber(total, { thousand: ".", decimal: ",", precision: 2 }));
    }

    $("body").on("keydown.autocomplete", ".list-rks", function () {
        $(this).autocomplete({
            appendTo: ".eventInsForm",
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: 'api/Rks/GetRks?klarifikasi=' + $("#Klasifikasi option:selected").val(),
                    data: request,
                    success: function (data) {
                        response($.map(data.aaData, function (item) {
                            return {
                                Title: item.Title,
                                Id: item.Id
                            }
                        }))
                    }
                });
            },
            select: function (event, ui) {
                $(this).val(ui.item.Title);
                $(this).attr("idrks", ui.item.Id);
                return false;
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            var html = '<div class="vendor">' +
                    '<div class="box-typehead-content">' +
                      '<span class="box-typehead-title-auto">' + item.Title + '</span>' +
                    '</div>' +
                '</div>'
            return $("<li class='item'>")
                .data("item.autocomplete", item)
                .append(html)
                .appendTo(ul);

        };
    });

  /*  $(".load-get-rks").on("click", function () {
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/saveRksFromTemplate?RksId=" + $(".list-rks").attr("idrks") + "&PengadaanId=" + $("#pengadaanId").val(),
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                waitingDialog.hideloading();
                window.location.reload();
            }
        });
    });
    */
    $("#example1").on("click", ".sisip-item-atas", function () {
        var judul = $(this).attr('attrJudul');
        var newData = {};
        newData.RKSHeaderId = $("#idRks").val();
        newData.judul = judul;
        newData.item = '';
        newData.satuan = '';
        newData.jumlah = '';
        newData.hps = '';//ui.item.satuan;
        newData.total = 0;
        newData.keterangan = '';
        newData.action = '';
        newData.level = 1;
        var currentPage = table.page();
        var baris = $(this).parent().closest("tr").index();
        var beforeRowData = table.row(baris).data();
        newData.grup = beforeRowData.grup;
        //baris = baris;
        table.row.add(newData).draw();

        var currentPage = table.page();

        //move added row to desired index (here the row we clicked on)
        var index = baris,
            rowCount = table.data().length - 1,
            insertedRow = table.row(rowCount).data(),
            tempRow;

        for (var i = rowCount; i > index; i--) {
            tempRow = table.row(i - 1).data();
            table.row(i).data(tempRow);
            table.row(i - 1).data(insertedRow);
        }
        //refresh the page
        table.page(currentPage).draw(false);
    });

    $("#example1").on("click", ".sisip-item-bawah", function () {
        var judul = $(this).attr('attrJudul');
        var newData = {};
        newData.RKSHeaderId = $("#idRks").val();
        newData.judul = judul;
        newData.item = '';
        newData.satuan = '';
        newData.jumlah = '';
        newData.hps = '';//ui.item.satuan;
        newData.total = 0;
        newData.keterangan = '';
        newData.action = '';
        newData.level = 1;
        var currentPage = table.page();
        var baris = $(this).parent().closest("tr").index();
        var beforeRowData = table.row(baris).data();
        newData.grup = beforeRowData.grup;
        //baris = baris;
        table.row.add(newData).draw();

        var currentPage = table.page();

        //move added row to desired index (here the row we clicked on)
        var index = baris,
            rowCount = table.data().length - 1,
            insertedRow = table.row(rowCount).data(),
            tempRow;

        for (var i = rowCount; i > index + 1; i--) {
            tempRow = table.row(i - 1).data();
            table.row(i).data(tempRow);
            table.row(i - 1).data(insertedRow);
        }
        //refresh the page
        table.page(currentPage).draw(false);
    });

    $("#example1").on("click", ".remove-item", function () {
        var vl = $(this).closest('tr')[0];
        table.rows(vl).remove().draw();
        hitungHargaItem();
    });

    $("#example1").on("input propertychange paste", ".jmlItem", function (e) {
        var vl = $(this).closest('tr')[0];
        var rowIndex = $('#example1 tr').index(vl);
        var jum = $(this).val();
        table.cell(rowIndex - 1, 1).data('<input type="text" class="form-control jmlItem" value="' + jum + '" placeholder="Jumlah">').draw();
        hitungHargaItem();
    });

    $(".take").on("click", function () {
        datatableToJson(table);
    });

    $("#example1").on('keydown.autocomplete', ".namaItem", function () {
        $(this).autocomplete({
            //source: "data/item.txt",
            //source:"api/Produk/GetAllProduk",
            minLength: 1,
            source: function (request, response) {
                $.ajax({
                    url: 'api/PengadaanE/GetItemByRegion?Region=' + $("#region").val(),
                    data: request,
                    success: function (data) {
                        //var ParsedObject = $.parseJSON(data);
                        response($.map(data.aaData, function (item) {
                            //return {
                            //    label: item.UserName,
                            //    value: item.UserId
                            //};
                            return {
                                ItemId: item.Id,
                                label: item.Nama,
                                region: item.Region,
                                satuan: item.Satuan,
                                harga: item.Price,
                                spesifikasi: item.Spesifikasi
                            }
                        }))
                        //return data.aaData;
                    }
                });
            },
            select: function (event, ui) {

                var baris = $(this).parent().closest("tr").index();
                var oldRowData = table.row(baris).data();
                //$(this).focus();
                var newData = {};
                newData.Id = oldRowData.Id;
                newData.ItemId = ui.item.ItemId;
                newData.RKSHeaderId = $("#idRks").val();
                newData.hps = ui.item.harga;
                newData.item = ui.item.label;
                newData.jumlah = 1;
                newData.satuan = ui.item.satuan;
                newData.total = ui.item.harga;
                newData.keterangan = ui.item.spesifikasi;
                addNewData(baris, newData);
                return false;
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            var html = '<div class="vendor">' +
                    '<div class="box-typehead-content">' +
                      '<span class="box-typehead-title-auto">' + item.label + '</span>' +
                      '<span class="box-typehead-desk-auto"> ' + item.region + ' /</span>' +
                      '<span class="box-typehead-desk-auto"> ' + item.satuan + ' /</span>' +
                      '<span class="box-typehead-desk-auto"> ' + item.harga + '</span>' +
                    '</div>' +
                '</div>'
            return $("<li class='item'>")
                .data("item.autocomplete", item)
                .append(html)
                .appendTo(ul);

        };

    });


    $("#example1").on('click', ".remove-item", function () {
        var vl = $(this).closest('tr')[0];
        table.row(vl.rowIndex).remove().draw();
    });

    $(".load-rks").on("click", function () {
        $("#modal-load-rks").modal("show");
        
    });


    $(".save-template-hps").on("click", function () {

    });

    $("#simpan").on("click", function () {
        if ($(this).attr("attr2") != "save") {
            BootstrapDialog.show({
                title: 'Informasi',
                message: 'Klik Button <span class="fa  fa-edit"></span> Jika Ingin Menyimpan RKS',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            return false;
        }
        $(this).hide();
        $("#loader").show();
        var objRKSHeader = {};
        if ($("#idRks").val() != "") objRKSHeader.Id = $("#idRks").val();
        objRKSHeader.PengadaanId = $("#pengadaanId").val();
        objRKSHeader.RKSDetails = datatableToJson(table);

        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/saveRks",
            dataType: "json",
            data: JSON.stringify(objRKSHeader),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#loader").hide();
                $("#simpan").show();
                waitingDialog.hideloading();

                //  table.draw();//message
                //addHeaderAfterSave();

                $("#idRks").val(data.Id);
                $("#konten-simpan").text(data.message);
                $("#modal-simpan").modal("show");
            },
            complate: function () {
                $("#loader").hide();
                $("#simpan").show();
            }
        });
    });
    $("#kembali").on("click", function () {
        if ($("#aturan-pengadaan").val()=="Pengadaan Terbuka")
            window.location.replace("http://" + window.location.host + "/pengadaan_add_terbuka.html#" + $("#pengadaanId").val());
        else window.location.replace("http://" + window.location.host + "/pengadaan-add.html#" + $("#pengadaanId").val());
    });

    $(".refresh").on("click", function () {
        //window.location.reload();
        window.location.href = "http://" + window.location.host + "/rks.html#" + $("#pengadaanId").val();
        window.location.reload();
        // window.location.replace("http://" + window.location.host + "/rks.html#"+$("#pengadaanId").val());
        //window.location.reload("http://" + window.location.host + "/rks.html#" + $("#pengadaanId").val());
    });
});

$(function () {

    $(".save-template-hps").on("click", function () {
        $("#modal-save-rks").modal("show");       
    });

    $(".save-repo-rks").on("click", function () {
        var objRKSHeader = {};
        if ($("#idrepoRks").val() != "") objRKSHeader.Id = $("#idrepoRks").val();
        objRKSHeader.Title = $("#title").val();
        objRKSHeader.Description = $("#deskripsi-repo").val();
        objRKSHeader.Klasifikasi = $("#Klasifikasi option:selected").val();
        objRKSHeader.Region = $("#region").val();
        objRKSHeader.RKSDetailTemplate = datatableRepoRksToJson(table);
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            method: "POST",
            url: "Api/Rks/save",
            dataType: "json",
            data: JSON.stringify(objRKSHeader),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                waitingDialog.hideloading();
                $("#idrepoRks").val(data.Id);
                BootstrapDialog.show({
                    title: 'Informasi',
                    message: data.message,
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {

                            dialog.close();
                        }
                    }]
                });
            },
            complate: function () {
                $("#loader").hide();
                $("#simpan").show();
            }
        });
        $("#modal-save-rks").modal("hide");
    });


});

function datatableRepoRksToJson(table) {
    var data = [];
    var beforeJudul = "";
    table.rows().every(function () {
        data.push(this.data());
    });
    return data;
}

function addHeaderAfterSave() {
    var tr = $("#example1 tbody tr");
    tr.each(function () {
        var baris = $(this).index();
        var oldRowData = table.row(baris).data();
        var newData = {};
        newData = oldRowData;
        newData.RKSHeaderId = $("#idRks").val();
        addNewData(baris, newData);
    });
    hitungHargaItemAwal();
}

function item_initialize() {
    var item = new Bloodhound({
        datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.text); },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        //limit:Number.MAX_VALUE, // I do not know how to get total number ...
        remote: {
            url: 'data/item.txt'
        }
    });

    item.initialize();
    $('.namaItem').typeahead(null, {
        name: 'item',
        display: 'label',
        source: item.ttAdapter(),
        templates: {
            empty: [
              '<div class="empty-message">',
                'NOT FOUND!!!',
              '</div>'
            ].join('\n'),
            suggestion: Handlebars.compile(
                '<div class="vendor">' +
                      '<div class="box-typehead-content">' +
                        '<span class="box-typehead-title" style="font-size:1.2em">{{label}}</span>' +
                        '<span class="box-typehead-desk">{{satuan}}</span>' +
                        '<span class="box-typehead-desk">{{harga}}</span>' +
                      '</div>' +
                  '</div>'
                )
        }
    }).on('typeahead:selected', function (obj, datum) {
        $(this).closest("td").next().html(datum.satuan);
        $(this).closest("td").next().next().next().html(datum.harga);

    });
}

function addNewItem() {
    table.row.add({
        "item": '<input type="text" class="form-control item namaItem " >',
        "satuan": '<input type="text" class="form-control item satuan" >',
        "jumlah": '<input type="text" class="form-control item jumlah" >',
        "hps": '<input type="text" class="form-control item hps" >',
        "keterangan": '<textarea class="form-control keterangan"></textarea>'
    }).draw();
    var jumRow = table.data().length;
    table.cell((jumRow - 1), 6).data('<a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a>').draw();
    $("#example1 tbody tr:eq('" + (jumRow - 1) + "') td:eq(0)").find("input").focus();

}

function addNewData(baris, newdata) {
    $.extend(table.row(baris).data(), newdata);
    table.row(baris).invalidate();
}

function addItem(item) {
    var jumRow = table.data().length;
    var nextNo = 1;
    if (jumRow > 0) {
        var data = table.row(jumRow - 1).data();
        nextNo = parseInt(data[0]) + 1;
    }
    table.row.add([nextNo, item[1], item[2], '<a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a>']).draw();
}
function clearInputInRow(indexTr) {
    if (indexTr > 0) {
        var inputs = $("#example1 tbody tr:eq('" + (indexTr - 1) + "')").find("input").parent();
        inputs.each(function () {
            var val = $(this).find("input").val();
            //var colIndex = $(this).parent().parent().children().index($(this).parent());
            //colIndex = $(this).parent().index();
            table.cell(indexTr, $(this).index()).data(val).draw();
        });
    }
}
function clearInputInRow2() {
    var inputs = $("#example1 tbody td").find("input");
    inputs.each(function () {
        var val = $(this).val();
        var vlRow = $(this).parent().closest('tr')[0];
        //var rowIndex=vlRow
        var colIndex = $(this).parent().parent().children().index($(this).parent());
        table.cell((vlRow.rowIndex - 1), colIndex).data(val).draw();
    });

}
function hitungHargaItemAwal() {
    var totalHarga = 0;

    table.rows().every(function () {
        var d = this.data();
        hps = UnformatFloat(d.hps);
        if ($.isNumeric(d.jumlah) && $.isNumeric(d.hps)) {
            totalHarga = totalHarga + (d.jumlah * d.hps);
        }
        d.counter++; // update data source for the row;
        this.invalidate(); // invalidate the data DataTables has cached for this row       
    });
    var estimasiCost = accounting.formatNumber(totalHarga, { thousand: ".", decimal: ",", precision: 2 });
    $("#totalRKS").text(estimasiCost);
}

function hitungHargaItem() {
    var totalHarga = 0;

    table.rows().every(function () {
        var d = this.data();
        hps = UnformatFloat(d.hps);
        var xJum = d.jumlah.toString().replace(".", "");
        xJum = xJum.toString().replace(",", ".");

        var xHps = d.hps.toString().replace(".", "");
        xHps = xHps.toString().replace(",", ".");

        if ($.isNumeric(xJum) && $.isNumeric(xHps)) {
            totalHarga = totalHarga + (parseFloat(xJum) * parseFloat(xHps));
        }
        d.counter++; // update data source for the row;
        this.invalidate(); // invalidate the data DataTables has cached for this row       
    });
    var estimasiCost = accounting.formatNumber(totalHarga, { thousand: ".", decimal: ",", precision: 2 });
    $("#totalRKS").text(estimasiCost);
}

function datatableToJson(table) {
    var data = [];
    var beforeJudul = "";
    table.rows().every(function () {
        data.push(this.data());
        //var odt = this.data();
        //odt.grup = odt.grup;
        //data.push(odt);
    });
    return data;
}
function loadDataRks(id) {
    $.ajax({
        url: "Api/PengadaanE/getHeaderRks?Id=" + $("#pengadaanId").val()
    }).done(function (data) {
        $("#idRks").val(data.RKSHeaderId);
    });
}
function loadData() {
    $.ajax({
        method: "POST",
        url: ($("#pengadaanId").val() == "") ? "Api/PengadaanE/detailPengadaan?Id=" + id_rks : "Api/PengadaanE/detailPengadaan?Id=" + $("#pengadaanId").val(),
        dataType: "json"
    }).done(function (data) {
        $("#judul").text(data.Judul);
        $("#deskripsi").text(data.AturanPengadaan + ", " + data.AturanBerkas + ", " + data.AturanPenawaran);
        $("#aturan-pengadaan").val(data.AturanPengadaan);
        $("#keterangan").text(data.Keterangan);
        $("#judul-page").html("RKS " + data.Judul);
        // $("#idRks").val(data.RKSHeaderId);
        if (data.isTEAM == 1 || data.isCreated == 1) {
            $("#simpan").show();
        }
        if (data.isTEAM != 1 && data.isCreated != 1) {
            var column = table.column(6);
            column.visible(false);//!column.visible()
            $(".simpan").remove();
            $(".edit").remove();
            $(".right").remove();
            $(".left").remove();
            $(".add").remove();
        }
        if (data.Status > 0) {
            var column = table.column(6);
            column.visible(false);//!column.visible()
            $(".simpan").remove();
            $(".edit").remove();
            $(".right").remove();
            $(".left").remove();
            $(".add").remove();
        }
        $("#pengadaanId").val(data.Id);
        $("#region").val(data.Region)
    });
}
