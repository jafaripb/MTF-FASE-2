var id_rks = window.location.hash.replace("#", "");
var status;
var table;
//var app = angular.module("app", []);
//app.controller('side-menu', ['$scope', '$http', function ($scope, $http) {
//    $scope.menus = [];
//    $http.get("data/menu.json")
//       .then(function (response) {
//           $scope.menus = response.data;
//       });
//}]);

//app.controller('user', ['$scope', '$http', function ($scope, $http) {
//    $scope.user = {};
//    $http.get("data/user.json")
//       .then(function (response) {
//           $scope.user = response.data;
//       });
//}]);
//var dataHps;
//$.ajax({
//    url: "Api/Produk/GetAllProduk"
//}).done(function (data) {
//    dataHps = data.aaData;
//});
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

            window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
            //$(location).attr('href', window.location.origin + "pengadaan-list.html");
        }
    }
    $("#cetak-rks").on("click", function () {
        downloadFileUsingForm("/api/report/CetakHPS?Id=" + $("#pengadaanId").val());
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
            { "data": "item" },
            { "data": "satuan" },
            { "data": "jumlah", "className": "tengah" },
            { "data": "hps", "className": "rata_kanan" },
            { "data": "total", "className": "rata_kanan" },
            { "data": "null", "width": "3%" }
        ],
        "columnDefs": [
                {
                    "render": function (data, type, row) {

                        if (row.hps != null) {
                            var str = "'" + row.hps + "'";
                            var arrstr = str.split(" ");
                            if (arrstr.length > 1)
                                return row.hps;
                            else {

                                var zz = accounting.formatNumber(row.hps, { thousand: "." });
                                if (UnformatFloat(zz) > 0)
                                    return zz;
                                else return "";
                            }
                        }
                        else return row.hps;
                    },
                    "targets": 3,
                    "orderable": false
                },
                {
                    "render": function (data, type, row) {
                        if (row.hps == "" || row.jumlah == "" || row.jumlah == null || row.hps == null)
                            return "";
                        else
                            return accounting.formatNumber(row.hps * row.jumlah, { thousand: "." });
                    },
                    "targets": 4,
                    "orderable": false
                },
                {
                    "render": function (data, type, row) {
                        return '<a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a>';
                    },
                    "targets": 5,
                    "orderable": false
                }
        ],
        "rowCallback": function (row, data, index) {
            hitungHargaItem();
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

    $("#example1 thead tr").dblclick(function () {
        var jumRow = table.data().length;
        var noItem = 1;
        addNewItem();
        var index = table.row(jumRow).index();
        //$("#example1 tbody tr:eq('"+index+"')").attr("parent",1);
        clearInputInRow(index);
    });
    $("#example1").on('keydown', '.item', function (e) {
        var keyCode = e.keyCode || e.which;
        var JumKolom = 6;//$(this).parent().parent().children().length;
       

            var tr = $(this).closest('tr')[0];
            var rowIndex = $('#example1 tr').index(tr);
            var colIndex = $(this).parent().parent().children().index($(this).parent());
            var val = $(this).val();
            if (keyCode == 9 || keyCode == 13) {
                $(".ui-autocomplete").remove();
                if (colIndex + 1 == 1) {
                    table.cell(rowIndex - 1, colIndex).data("<p>" + val + "</p>").draw();
                    $("#example1 tbody tr:eq('" + (rowIndex - 1) + "')").find("td:eq(" + (colIndex) + ") p").attr("padding-left", "0");
                }
                else {
                    table.cell(rowIndex - 1, colIndex).data(val).draw();
                }

                if (keyCode == 9) {
                    e.preventDefault();
                    if (colIndex <3) {
                        var valEl = $("#example1 tbody tr:eq('" + (rowIndex - 1) + "')").find("td:eq(" + (colIndex + 1) + ")").text();
                        if(valEl!="") valEl = UnformatFloat(valEl);
                        table.cell(rowIndex - 1, colIndex + 1).data('<input type="text" value="' + valEl + '"  class="form-control item" >').draw();
                        $("#example1 tbody tr:eq(" + (rowIndex - 1) + ") td:eq(" + (colIndex + 1) + ")").find("input").focus();
                        console.log($("#example1 tbody tr:eq(" + (rowIndex - 1) + ") td:eq(" + (colIndex + 1) + ")").find("input"));
                    }
                    else addNewItem();
                }
                //hitung total per baris
                var jumlah = $("#example1 tbody tr:eq('" + (rowIndex - 1) + "')").find("td:eq(" + (2) + ")").text();
                var hps = $("#example1 tbody tr:eq('" + (rowIndex - 1) + "')").find("td:eq(" + (3) + ")").text();
                hps = UnformatFloat(hps);
                if ($.isNumeric(jumlah) || $.isNumeric(hps)) {
                    var totalBaris = parseFloat(jumlah) * parseFloat(hps);
                    var totalBarisFormat = accounting.formatNumber(totalBaris, { thousand: "." });
                    table.cell(rowIndex - 1, 4).data(totalBarisFormat).draw();
                }
               // hitungHargaItem();
            }
    });
    $("#example1").on('dblclick', 'td', function (e) {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('td.selected').removeClass('selected');
            $(this).addClass('selected');
        }
        clearInputInRow2();
    });

    $("#edit").on("click", function () {
        var tdtEl = $("#example1").find("td.selected");
        var val = tdtEl.text();
        var rowIndex = $('#example1 tr').index($(tdtEl).closest('tr')[0]);
        clearInputInRow(rowIndex);
        var colIndex = $(tdtEl).parent().children().index($(tdtEl));
        if (colIndex + 1 == 2) {
            table.cell(rowIndex - 1, colIndex).data('<input type="text" value="' + val + '" class="form-control item satuan" >').draw();
        }
        else if (colIndex + 1 == 1) {
            table.cell(rowIndex - 1, colIndex).data('<input type="text" value="' + val + '" class="form-control item namaItem" >').draw();
        }
        else {
            table.cell(rowIndex - 1, colIndex).data('<input type="text" value="' + val + '" class="form-control item" >').draw();
        }
        $(tdtEl).removeClass('selected');
        //item_initialize();
    });

    $("#right").on("click", function () {
        var tdtEl = $("#example1").find("td.selected");

        var nextPadding = $(tdtEl).children().attr("padding-left");
        var intPadd=15;
        if (typeof nextPadding != "undefined") {
            intPadd = parseInt(nextPadding) + 15;
        }
        $(tdtEl).children().css("padding-left", intPadd + "px");
        $(tdtEl).children().attr("padding-left", intPadd);
        var rowIndex = $('#example1 tr').index($(tdtEl).closest('tr')[0]);
        var colIndex = $(tdtEl).parent().children().index($(tdtEl));
        var val = $(tdtEl).html();
       // console.log(val);
        table.cell(rowIndex - 1, colIndex).data(val).draw();
    });
    $("#left").on("click", function () {
        var tdtEl = $("#example1").find("td.selected");
        var nextPadding = $(tdtEl).children().attr("padding-left");
        var intPadd = 0;
        if (typeof nextPadding != "undefined") {
            intPadd = parseInt(nextPadding) - 15;
        }
        if (parseInt(nextPadding) > 0) {           
            $(tdtEl).children().css("padding-left", intPadd + "px");
            $(tdtEl).children().attr("padding-left", intPadd);
        }
        var rowIndex = $('#example1 tr').index($(tdtEl).closest('tr')[0]);
        var colIndex = $(tdtEl).parent().children().index($(tdtEl));
        var val = $(tdtEl).html();
        table.cell(rowIndex - 1, colIndex).data(val).draw();
    });
    $("#take").on("click", function () {
        datatableToJson(table);
    });

    $("#example1").on('keydown.autocomplete', ".namaItem", function () {

        $(this).autocomplete({
            //source: "data/item.txt",
            //source:"api/Produk/GetAllProduk",
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: 'api/PengadaanE/GetAllProduk',
                    data: request,
                    success: function (data) {
                        //var ParsedObject = $.parseJSON(data);
                        response($.map(data.aaData, function (item) {
                            //return {
                            //    label: item.UserName,
                            //    value: item.UserId
                            //};
                            return {
                                ItemId:item.Id,
                                label: item.Nama,
                                region: item.Region,
                                satuan: item.Satuan,
                                harga: item.Price
                            }
                        }))
                        //console.log(data.aaData);
                        //return data.aaData;
                    }
                });
            },
            select: function (event, ui) {
              
                var baris = $(this).parent().closest("tr").index();
                var oldRowData=table.row(baris).data();
                //$(this).focus();
                 var newData = {};
                 newData.Id = oldRowData.Id;
                 newData.ItemId = ui.item.ItemId;
                 newData.RKSHeaderId = $("#idRks").val();
                 newData.hps = ui.item.harga;
                 newData.item = ui.item.label; //'<input type="text" value="' + ui.item.label + '"  class="form-control item namaItem " >';
                 newData.jumlah = 1;
                 newData.satuan = '<input type="text" value="' + ui.item.satuan + '"  class="form-control item namaItem " >';//ui.item.satuan;
                 newData.total = ui.item.harga;
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
  

    $("#simpan").on("click", function () {
        $(this).hide();
        $("#loader").show();
        var objRKSHeader = {};
        if ($("#idRks").val()!="") objRKSHeader.Id = $("#idRks").val();
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

                table.draw();//message
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
        window.location.replace("http://" + window.location.host + "/pengadaan-add.html#"+$("#pengadaanId").val());
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
function addNewItem() {
    table.row.add({
        "item": '<input type="text" class="form-control item namaItem " >',
        "satuan": "",
        "jumlah": "",
        "hps": "",
        "total": ""
    }).draw();
    var jumRow = table.data().length;
    table.cell((jumRow - 1), 5).data('<a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a>').draw();
    $("#example1 tbody tr:eq('" + (jumRow - 1) + "') td:eq(0)").find("input").focus();

}

function addNewData(baris, newdata) {
    $.extend( table.row(baris).data(), newdata );
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
        var inputs = $("#example1 tbody tr:eq('" + (indexTr - 1) + "')").find("input");
        inputs.each(function () {
            var val = $(this).val();
            var colIndex = $(this).parent().parent().children().index($(this).parent());
            table.cell((indexTr - 1), colIndex).data(val).draw();
        });
    }
}
function clearInputInRow2() {
    //console.log($("#example1 tbody td").find("input"));
    var inputs = $("#example1 tbody td").find("input");
    inputs.each(function () {
        var val = $(this).val();
        var vlRow = $(this).parent().closest('tr')[0];
        //var rowIndex=vlRow
        //console.log(vlRow.rowIndex);
        var colIndex = $(this).parent().parent().children().index($(this).parent());
        table.cell((vlRow.rowIndex - 1), colIndex).data(val).draw();
    });

}
function hitungHargaItem() {
    var totalHarga = 0;
    
    table.rows().every(function () {
        var d = this.data();       
        hps = UnformatFloat(d.hps);
        if ($.isNumeric(d.jumlah) || $.isNumeric(hps)) {
            totalHarga = totalHarga + parseFloat(d.jumlah) * parseFloat(hps);
        }
        d.counter++; // update data source for the row;
        this.invalidate(); // invalidate the data DataTables has cached for this row
       
    });
    var estimasiCost = accounting.formatNumber(totalHarga, { thousand: "." });
    $("#totalRKS").text(estimasiCost);
   
}
function datatableToJson(table) {
    var data = [];
    table.rows().every(function () {
        var itemObj = {};
        var d = this.data();
        itemObj.Id=d.Id
        itemObj.item = d.item;
        itemObj.satuan = d.satuan;
        itemObj.jumlah = d.jumlah;
        itemObj.hps = d.hps;
        data.push(itemObj);
       // console.log(d);
    });

    //console.log(JSON.stringify(data));
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
        $("#keterangan").text(data.Keterangan);
        $("#judul-page").html("RKS " + data.Judul);
       // $("#idRks").val(data.RKSHeaderId);
        if (data.isTEAM == 1 || data.isCreated==1) {
            $("#simpan").show();
        }
        if (data.isTEAM != 1 && data.isCreated != 1) {
            $("#simpan").remove();
            $("#edit").remove();
            $("#right").remove();
            $("#left").remove();
        }
        if (data.Status != 0) {
            var column = table.column(5);
            column.visible(false);//!column.visible()
            $("#simpan").remove();
            $("#edit").remove();
            $("#right").remove();
            $("#left").remove();
        }
        $("#pengadaanId").val(data.Id);
    });
}

//console.log(id_rks);