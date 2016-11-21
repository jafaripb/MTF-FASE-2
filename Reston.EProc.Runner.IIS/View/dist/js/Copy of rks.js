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
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Pilih Tipe Dokumen <a id="CetakWord" class="btn btn-app bg-blue"><i class="fa fa-file-word-o"></i> Word</a>',
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
            { "data": "keterangan", "className": "rata_kiri" },
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
                        if (row.hps == "" || row.jumlah == "" || row.jumlah == null || row.hps == null) {
                            return row.total;
                        }
                        else {
                            
                            if ($.isNumeric(UnformatFloat(row.total))) {
                                return accounting.formatNumber(UnformatFloat(row.total), { thousand: "." });
                            }
                            return accounting.formatNumber(row.jumlah * row.hps, { thousand: "." });
                            
                        }
                        //if(row.total!="")
                        //    return accounting.formatNumber(row.total, { thousand: "." });
                        //else return "";
                    },
                    "targets": 4,
                    "orderable": false
                },
                {
                    "render": function (data, type, row) {
                        return '<a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a>';
                    },
                    "targets": 6,
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

    $("#add").on("click", function () {
        if ($("#simpan").attr("attr2") != "unsave") return false;
        var jumRow = table.data().length;
        var noItem = 1;
        addNewItem();
        var index = table.row(jumRow).index();
        var jumlah = $("#example1 tbody tr:eq('" + (index) + "')").find("td:eq('2') input").val();
        var hps = $("#example1 tbody tr:eq('" + (index) + "')").find("td:eq('3') input").val();
        var Item = $("#example1 tbody tr:eq('" + (index) + "')").find("td:eq('0') input").val();
        var satuan = $("#example1 tbody tr:eq('" + (index) + "')").find("td:eq('1') input").val();
        var keterangan = $("#example1 tbody tr:eq('" + (index) + "')").find("td:eq('5') textarea").val();
        console.log();
    });
    //$("#example1 thead tr").dblclick(function () {
    //    if ($("#simpan").attr("attr2") != "unsave") return false;
    //    var jumRow = table.data().length;
    //    var noItem = 1;
    //    addNewItem();
    //    var index = table.row(jumRow).index();
    //});
    $("#example1").on('keydown', '.item', function (e) {
        var keyCode = e.keyCode || e.which;
        var JumKolom = 6;//$(this).parent().parent().children().length;       

            var tr = $(this).closest('tr')[0];
            var rowIndex = $('#example1 tr').index(tr);
            var colIndex = $(this).parent().parent().children().index($(this).parent());
            var val = $(this).val();
            if (keyCode == 9 || keyCode == 13) {
                $(".ui-autocomplete").remove();
                if (keyCode == 9) {
                    e.preventDefault();
                    if (colIndex < 3) {
                        console.log(colIndex);
                        $("#example1 tbody tr:eq(" + (rowIndex - 1) + ") td:eq(" + (colIndex + 1) + ")").find("input").focus();
                    }
                    else {
                        var jumlah = $("#example1 tbody tr:eq('" + (rowIndex - 1) + "')").find("td:eq('2') input").val();
                        var hps = $("#example1 tbody tr:eq('" + (rowIndex - 1) + "')").find("td:eq('3') input").val();
                        var Item = $("#example1 tbody tr:eq('" + (rowIndex - 1) + "')").find("td:eq('0') input").val();
                        var satuan = $("#example1 tbody tr:eq('" + (rowIndex - 1) + "')").find("td:eq('1') input").val();
                        var keterangan = $("#example1 tbody tr:eq('" + (rowIndex - 1) + "')").find("td:eq('5') textarea").val();
                        

                        hps = UnformatFloat(hps);
                        if ($.isNumeric(jumlah) || $.isNumeric(hps)) {
                            var totalBaris = parseFloat(jumlah) * parseFloat(hps);
                            var baris = $(this).parent().closest("tr").index();
                            var oldRowData = table.row(baris).data();
                            //$(this).focus();
                            var newData = {};
                            newData.Id = oldRowData.Id;
                            newData.ItemId = oldRowData.ItemId;
                            newData.RKSHeaderId = $("#idRks").val();
                            newData.hps = '<input type="text" value="' + hps + '" class="form-control item" >';
                            newData.item = '<input type="text" value="' + Item + '" class="form-control item" >'; //'<input type="text" value="' + ui.item.label + '"  class="form-control item namaItem " >';
                            newData.jumlah = '<input type="text" class="form-control item" value="' + jumlah + '"  >';
                            newData.satuan = '<input type="text" value="' + satuan + '"  class="form-control item" >';//ui.item.satuan;
                            newData.total = accounting.formatNumber(totalBaris, { thousand: "." });
                            newData.keterangan = '<textarea class="form-control keterangan" >' + keterangan + '</textarea>';
                            addNewData(baris, newData);
                            //var totalBarisFormat = accounting.formatNumber(totalBaris, { thousand: "." });
                            //console.log(rowIndex + " " + totalBarisFormat);
                           // table.cell(rowIndex - 1, 4).data(totalBaris).draw();
                        }
                        else {
                            var baris = $(this).parent().closest("tr").index();
                            var oldRowData = table.row(baris).data();
                            //$(this).focus();
                            var newData = {};
                            newData.Id = oldRowData.Id;
                            newData.ItemId = oldRowData.ItemId;
                            newData.RKSHeaderId = $("#idRks").val();
                            newData.hps = '<input type="text" value="" class="form-control item" >';
                            newData.item = '<input type="text" value="' + Item + '" class="form-control item" >'; //'<input type="text" value="' + ui.item.label + '"  class="form-control item namaItem " >';
                            newData.jumlah = '<input type="text" class="form-control item" value=""  >';
                            newData.satuan = '<input type="text" value=""  class="form-control item" >';//ui.item.satuan;
                            newData.total = "";
                            newData.keterangan = '<textarea class="form-control keterangan" >' + keterangan + '</textarea>';
                            addNewData(baris, newData);
                            //table.cell(rowIndex - 1, 0).data('<input type="text" class="form-control item namaItem value="' + Input1 + '" " >').draw();
                        }
                        addNewItem();                        
                    }
                }
                //hitung total per baris
               
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
        //clearInputInRow2();
    });

    $("#edit").on("click", function () {
        if ($("#simpan").attr("attr2") == "unsave") {           
            $("#edit").children().removeClass("fa-edit");
            $("#edit").children().addClass("fa-check");
            $("#simpan").attr("attr2", "save");
            colectData();
        }
        else if ($("#simpan").attr("attr2") == "save") {
            $("#edit").children().removeClass("fa-check");
            $("#edit").children().addClass("fa-edit");
            $("#simpan").attr("attr2", "unsave");
            reInputData();
        }
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
                                harga: item.Price,
                                spesifikasi:item.Spesifikasi
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
                 newData.hps = '<input type="text" value="' + ui.item.harga + '" class="form-control item hps" >';
                 newData.item = '<input type="text" value="' + ui.item.label + '" class="form-control item namaItem" >'; //'<input type="text" value="' + ui.item.label + '"  class="form-control item namaItem " >';
                 newData.jumlah = '<input type="text" class="form-control item jumlah" value="1"  >';
                 newData.satuan = '<input type="text" value="' + ui.item.satuan + '"  class="form-control item satuan" >';//ui.item.satuan;
                 newData.total = ui.item.harga;
                 newData.keterangan = '<textarea class="form-control keterangan" >' + ui.item.spesifikasi + '</textarea>';
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

    $("#example1").on('keydown.autocomplete', ".satuan", function () {

        $(this).autocomplete({
            //source: "data/item.txt",
            //source:"api/Produk/GetAllProduk",
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: 'api/PengadaanE/GetAllSatuan',
                    data: request,
                    success: function (data) {
                        //var ParsedObject = $.parseJSON(data);
                        response($.map(data.aaData, function (item) {
                            //return {
                            //    label: item.UserName,
                            //    value: item.UserId
                            //};
                            return {
                                satuan: item.Satuan
                            }
                        }))
                        //console.log(data.aaData);
                        //return data.aaData;
                    }
                });
            },
            select: function (event, ui) {
                $(this).val(ui.item.satuan);
                return false;
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            var html = '<div class="vendor">' +
                    '<div class="box-typehead-content">' +
                      '<span class="box-typehead-title-auto">' + item.satuan + '</span>' +
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
                addHeaderAfterSave();
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
        window.location.replace("http://" + window.location.host + "/pengadaan-add.html#"+$("#pengadaanId").val());
    });  
});

function addHeaderAfterSave() {
    var tr = $("#example1 tbody tr");
    tr.each(function () {
        var baris = $(this).index();
        var oldRowData = table.row(baris).data();
        var newData = {};
        newData = oldRowData;
        newData.RKSHeaderId = $("#idRks").val();
        //console.log(newData)
        addNewData(baris, newData);
    });
}

function colectData() {
    var tr = $("#example1 tbody tr");
    tr.each(function () {
        var baris = $(this).index();
        var oldRowData = table.row(baris).data();
        var newData = {};
        newData.Id = oldRowData.Id;
        newData.ItemId = oldRowData.ItemId;
        newData.RKSHeaderId = $("#idRks").val();
        newData.item ="<p>"+ $(this).find("td:eq(0) input").val()+"</p>";
        newData.satuan = $(this).find("td:eq(1) input").val(); 
        newData.jumlah = $(this).find("td:eq(2) input").val();
        newData.hps = $(this).find("td:eq(3) input").val();//ui.item.satuan;
        newData.keterangan = $(this).find("td:eq(5) textarea").val();
        if ($.isNumeric(newData.jumlah) && $.isNumeric(newData.hps)) {
            newData.total = UnformatFloat(newData.jumlah * newData.hps);
        } else newData.total = "";
        
        addNewData(baris, newData);        
    });

    hitungHargaItem();
}

function reInputData() {
    var tr = $("#example1 tbody tr");
    if (tr.find("td").length < 1) return false;
    var regex = /(<([^>]+)>)/ig;
    tr.each(function () {
        var baris = $(this).index();
        var oldRowData = table.row(baris).data();
        var newData = {};
        var item = oldRowData.item.replace(regex, "");
        newData.Id = oldRowData.Id;
        newData.ItemId = oldRowData.ItemId;
        newData.RKSHeaderId = $("#idRks").val();
        newData.item = '<input type="text" value="' + item + '" class="form-control item namaItem " >';
        newData.satuan = '<input type="text" value="' + (oldRowData.satuan == null ? "" : oldRowData.satuan) + '" class="form-control item satuan" >'; //'<input type="text" value="' + ui.item.label + '"  class="form-control item namaItem " >';
        newData.jumlah = '<input type="text" value="' + (oldRowData.jumlah == null ? "" : oldRowData.jumlah) + '" class="form-control item jumlah" >';
        newData.hps = '<input type="text" value="' + (oldRowData.hps == null ? "" : oldRowData.hps) + '" class="form-control item hps" >';//ui.item.satuan;
        newData.keterangan = '<textarea class="form-control keterangan" >' + oldRowData.keterangan+ ' </textarea>';
        var total = 0;
        if (oldRowData.jumlah != null && oldRowData.hps != null)
            total = oldRowData.jumlah * oldRowData.hps;
        if (total > 0)
            newData.total = accounting.formatNumber(total, { thousand: "." });
        else newData.total = "";
        //console.log(newData)
        addNewData(baris, newData);
    });
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
        var inputs = $("#example1 tbody tr:eq('" + (indexTr - 1) + "')").find("input").parent();
        console.log(inputs);
        inputs.each(function () {
            var val = $(this).find("input").val();
            //var colIndex = $(this).parent().parent().children().index($(this).parent());
            //colIndex = $(this).parent().index();
            //console.log($(this));
            //console.log($(this).parent());
            //console.log($(this).parent().index());
            //console.log(colIndex + " " + indexTr);
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
        if ($.isNumeric(d.jumlah) && $.isNumeric(hps)) {
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
        itemObj.keterangan = d.keterangan;
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
            var column = table.column(6);
            column.visible(false);//!column.visible()
            $("#simpan").remove();
            $("#edit").remove();
            $("#right").remove();
            $("#left").remove();
            $("#add").remove();
        }
        if (data.Status > 0) {
            var column = table.column(6);
            column.visible(false);//!column.visible()
            $("#simpan").remove();
            $("#edit").remove();
            $("#right").remove();
            $("#left").remove();
            $("#add").remove();
        }
        $("#pengadaanId").val(data.Id);
    });
}

//console.log(id_rks);