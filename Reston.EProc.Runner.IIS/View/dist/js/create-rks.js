var id_rks = window.location.hash.replace("#", "");
var status;
var table;
$(function () {

    //loadData();
    if (isGuid(id_rks)) {
        $("#rksId").val(id_rks);
        loadDataRks(id_rks);
    }
    else {
        if (isGuid($("#rksId").val())) {
            window.location.hash = $("#rksId").val();
            loadDataRks($("#rksId").val());
        }
        else {

            //window.location.replace("http://" + window.location.host + "/repository-rks.html");
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
        downloadFileUsingForm("/api/report/CetakHPSNew?Id=" + $("#rksId").val());
    });

    $("body").on("click", "#CetakXls", function () {
        downloadFileUsingForm("/api/report/CetakHPSXLSNew?Id=" + $("#rksId").val());
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
        "ajax": ($("#rksId").val() == "") ? "Api/Rks/getRks?id=" + id_rks : "Api/Rks/getRks?id=" + $("#rksId").val(),
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
                            return '<input type="text" class="form-control item hps" value="' + row.hps + '" style="width:150px; text-align:right;">';
                        }
                        else if(row.level==2){
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
                     if (row.level == 0) {
                         return '<a class="btn btn-xs btn-success sisip-item-bawah" attrJudul="' + row.judul + '" title="Tambah Item Bawah"><span class="fa fa-hand-o-down"></span></a>' +
                                ' <a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a> ';
                     }
                     if (row.level == 2) {
                         return ' <a class="btn btn-xs btn-primary sisip-item-atas" attrJudul="' + row.judul + '" title="Tambah Item Atas"><span class="fa fa-hand-o-up"></span></a> ' +
                                ' <a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a> ';
                     }
                     return ' <a class="btn btn-xs btn-primary sisip-item-atas" attrJudul="'+row.judul+'" title="Tambah Item Atas"><span class="fa fa-hand-o-up"></span></a> ' +
                         ' <a class="btn btn-xs btn-success sisip-item-bawah" attrJudul="' + row.judul + '" title="Tambah Item Bawah"><span class="fa fa-hand-o-down"></span></a> ' +
                         ' <a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a> ' +
                         ' <a class="btn btn-xs btn-warning hitung" title="Clear Row"><span class="fa fa-pencil"></span></a>';
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

    function numberWithCommas(x) {
        var parts = x.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return parts.join(".");
    }

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

    $("#example1").on("click", ".hitung", function () {
        alert("OK");
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

    $(".add-judul").on("click", function () {        
        addNewJudul(parseInt($(this).attr("attr")));
        $(this).attr("attr", parseInt($(this).attr("attr")) + 1);

    });

    SetListRegion("[name=Region]");
    
    $("#example1").on("change", ".item", function () {
        var elRow = $(this).parent().closest("tr");
        var oldRowData = table.row(elRow.index()).data();
        var newRowData = {};
        newRowData.judul = '' + $(elRow).find(".namaJudul").val();
        if (oldRowData.item == "") {
            newRowData.item = "";
            newRowData.satuan = "";
            newRowData.jumlah = "";
            newRowData.hps = "";
            newRowData.keterangan = "";
        }
        else if (newRowData.judul == "undefined" || newRowData.judul=="") {
            var totalHarga = (parseFloat($(elRow).find(".jumlah").val()) * parseFloat($(elRow).find(".hps").val()));
            if ($(elRow).find(".jumlah").val() == "") {
                newRowData.jumlah = '1';
                newRowData.total = totalHarga;
            }
            else if ($(elRow).find(".hps").val() == "0") {
                newRowData.hps = $(elRow).find(".hps").val();
                newRowData.jumlah = $(elRow).find(".jumlah").val();
                newRowData.total = totalHarga;
            }
            else {
                newRowData.item = $(elRow).find(".namaItem").val();
                newRowData.satuan = $(elRow).find(".satuan").val();
                newRowData.jumlah = $(elRow).find(".jumlah").val();
                newRowData.hps = $(elRow).find(".hps").val();
                newRowData.total = totalHarga;
            }
        }
        addNewData(elRow.index(), newRowData);
        
        Hitung();
    });

    function Hitung() {
        var total = 0;
        var index = 0;
        var subtotal = 0;
        var before_group = 0;
            table.rows().every(function () {
                var d = this.data();
                var current_group = d.group;
                if (before_group != 0 && before_group == current_group && d.level ==1) {
                    subtotal = subtotal + parseFloat(d.total);
                    total = total + parseFloat(d.total);
                   
                }
                else {
                    if (before_group == 0 && d.level==1) {
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
                before_group = current_group;

                this.invalidate(); // invalidate the data DataTables has cached for this row       
            });
            $(".add-judul").attr("attr", before_group+1);
            $('#totalRKS').text(accounting.formatNumber(total, { thousand: ".", decimal: ",", precision: 2 }));
    }

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
        newData.group = beforeRowData.group;
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
        newData.group = beforeRowData.group;
        //baris = baris;
        table.row.add(newData).draw();
        
        var currentPage = table.page();

        //move added row to desired index (here the row we clicked on)
        var index = baris,
            rowCount = table.data().length - 1,
            insertedRow = table.row(rowCount).data(),
            tempRow;

        for (var i = rowCount; i > index+1; i--) {
            tempRow = table.row(i - 1).data();
            table.row(i).data(tempRow);
            table.row(i - 1).data(insertedRow);
        }
        //refresh the page
        table.page(currentPage).draw(false);
    });

    $("#example1").on("click", ".sisip-judul", function () {
        var newData = {};
        newData.RKSHeaderId = $("#idRks").val();
        newData.judul = '';
        newData.item = '';
        newData.satuan = '';
        newData.jumlah = '';
        newData.hps = '';//ui.item.satuan;
        newData.total = '';
        newData.keterangan = '';
        newData.action = '';
        var currentPage = table.page();
        var baris = $(this).parent().closest("tr").index();
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

    $(".refresh").on("click", function () {
        window.location.href = "http://" + window.location.host + "/create-rks.html#" + $("#rksId").val();
        window.location.reload();
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

    $("#example1").on('click', ".remove-item", function () {
        var vl = $(this).closest('tr')[0];
        table.row(vl.rowIndex).remove().draw();
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
        objRKSHeader.Title = $("#title").val();
        objRKSHeader.Description = $("#deskripsi").val();
        objRKSHeader.Klasifikasi = $("#Klasifikasi option:selected").val();
        objRKSHeader.Region = $("#region option:selected").val();
        objRKSHeader.RKSDetailTemplate = datatableToJson(table);
        console.log(datatableToJson(table));
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            method: "POST",
            url: "Api/Rks/save",
            dataType: "json",
            data: JSON.stringify(objRKSHeader),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#loader").hide();
                $("#simpan").show();
                waitingDialog.hideloading();

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
        window.location.replace("http://" + window.location.host + "/repository-rks.html");
    });
});



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

function addNewJudul(lastgroup) {
    table.row.add({
        "judul": '',
        "item": '',
        "satuan": '',
        "jumlah": '',
        "hps": '',
        "total": 0,
        "keterangan": '',
        "level": 0,
        "group":lastgroup+1
    }).draw();
    var jumRow = table.data().length;
    table.cell((jumRow - 1), 7).data(' <a class="btn btn-xs btn-success sisip-item" title="Tambah Item"><span class="fa fa-plus"></span></a> ' +
            ' <a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a> ').draw();
   // $("#example1 tbody tr:eq('" + (jumRow - 1) + "') td:eq(0)").find("input").focus();
    subTotal(lastgroup);
}

function subTotal(lastgroup) {
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
    newData.group = lastgroup + 1;
    var currentPage = table.page();
    var baris = $(this).parent().closest("tr").index();
    table.row.add(newData).draw();
    var currentPage = table.page();
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
    table.cell((jumRow - 1), 7).data('<a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a>').draw();
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
        //  console.log(inputs);
        inputs.each(function () {
            var val = $(this).find("input").val();
            table.cell(indexTr, $(this).index()).data(val).draw();
        });
    }
}

function clearInputInRow2() {
    //console.log($("#example1 tbody td").find("input"));
    var inputs = $("#example1 tbody td").find("input");
    inputs.each(function () {
        var val = $(this).val();
        var vlRow = $(this).parent().closest('tr')[0];
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
    });
    //console.log(JSON.stringify(data));
    return data;
}

function loadDataRks(id) {
    $.ajax({
        url: "Api/Rks/getHeaderRks?Id=" + $("#rksId").val()
    }).done(function (data) {
        $("#idRks").val(data.Id);
        $("#title").val(data.Title);
        $("#deskripsi").val(data.Deskripsi);
        $("#Klasifikasi").val(data.Klasifikasi);
        $("#region").val(data.Region);
    });
}

//console.log(id_rks);