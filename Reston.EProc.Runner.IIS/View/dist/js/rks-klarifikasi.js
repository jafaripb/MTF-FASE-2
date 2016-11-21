var id_pengadaan = window.location.hash.replace("#", "");

var table;

var model = { showed: [], colIndex: -1, colOrder: [], locked:[] };
var firstIndexProcessed = 5;//0-4 to be skipped on sorting col

function sortCol(e, idx) {
    var sortDir = 0;
    $e = $(e).children("span");
    if ($e.hasClass("fa-sort")) {
        $(".s-sort-symbol").removeClass('fa-sort-asc').removeClass('fa-sort-desc').addClass('fa-sort');
        $e.removeClass("fa-sort").addClass("fa-sort-asc"); //direction dont change
        $(table.row(idx).nodes()).addClass('highlight-row');
    }
    else if ($e.hasClass("fa-sort-asc")){
        $e.removeClass("fa-sort-asc").addClass("fa-sort-desc");
        sortDir = 1;//go descending
        $(table.row(idx).nodes()).addClass('highlight-row');
    }
    else if ($e.hasClass("fa-sort-desc")) { 
        $e.removeClass("fa-sort-desc").addClass("fa-sort");
        table.colReorder.reset();
        $(table.row(idx).nodes()).removeClass('highlight-row');
        //saving model
        model.colIndex = -1;
        model.colOrder = [];
        return;
    }

    var idx_arr = firstIndexProcessed;
    var row_data = table.rows().data()[idx];
    var row_data_processed = row_data.slice(firstIndexProcessed, row_data.length);
    var row_index = [];
    for (var i in row_data_processed) row_index.push(idx_arr++);
    var oldOrder = table.colReorder.order();
    for (var i in oldOrder)
        if ($.inArray(oldOrder[i],model.locked) != -1) oldOrder[i] = -1;
    console.log(oldOrder);
    bubbleSortodif(row_data_processed, sortDir, row_index); //0 asc, else desc
    var newOrder = [0, 1, 2, 3, 4];
    for (var i in row_index) newOrder.push(row_index[i]);
    table.colReorder.order(newOrder);
    //updating saving model
    model.colIndex = idx;
    model.colOrder = newOrder;
}

function showHideVendor(el,i){
    $e = $(el);
    var index = i+firstIndexProcessed;
    if ($e.is(':checked')) {
        table.column(index).visible(true);
    }
    else {
        table.column(index).visible(false);
    }
}

function lockUnlockVendor(el, i) {
    //console.log('wehew');
    $e = $(el);
    $indicator = $e.prev();
    $box = $e.parent().parent().parent();
    var index = i + firstIndexProcessed;
//    console.log("lock/unlock index: " + i);
    if ($e.is(':checked')) {
        model.locked.push(index);
        $indicator.removeClass('fa-lock').addClass('fa-unlock');
        $box.addClass('list-hidden');
        $( table.column( index ).nodes() ).addClass( 'highlight-col' );
    }
    else {
        arrayRemoveValue(model.locked, index);
        $indicator.removeClass('fa-unlock').addClass('fa-lock');
        $box.removeClass('list-hidden');
        $(table.column(index).nodes()).removeClass('highlight-col');
    }
}

function arrayRemoveValue(array, value) {
    for (var i = array.length - 1; i >= 0; i--) {
        if (array[i] === value) {
            array.splice(i, 1);
        }
    }
}

function gotoVendor() {
    //alert("gog");
    window.location.href = 'rekanan-detail.html';
}

function setShowedColModels() {
    model.showed = [];
    var e = $(".s-checkbox:checked");//.is('checked');
    //console.log(e);
    //for (var i in e) {
    //    model.showed.push($(e[i]).data('idx'));
    //}
    //console.log(model.showed);
}

function bubbleSortodif(arr, dir, arr_idx) {
    var temp=0;
    var temp_idx=0;
    for (i = 0; i < arr.length - 1; i++) {
        if (((dir == 0 && arr[i] > arr[i + 1]) || (dir != 0 && arr[i] < arr[i + 1])) && $.inArray(arr_idx[i],model.locked) == -1) {
            //console.log(model.locked);
            //console.log(arr_idx[i]);
            if ($.inArray(arr_idx[i + 1], model.locked) == -1) {
                arrayValueSwap(arr, i, i + 1);
                arrayValueSwap(arr_idx, i, i + 1);

                i = i - 2;
            }
            else {
                console.log(arr_idx[i+1]);
                var j = i+1;
                while (j < arr.length && $.inArray(arr_idx[j], model.locked) != -1) {
                    j++;
                }
                if (j < arr.length) {
                    arrayValueSwap(arr, i, j);
                    arrayValueSwap(arr_idx, i, j);
                    i = i - 2;// (1 + (j - i));
                }
            }
        }
        /*http://codereview.stackexchange.com/questions/87869/bubble-sort-algorithm-in-javascript*/
    }
}

function arrayValueSwap(arr,i,j) {
    var temp = arr[j];
    arr[j] = arr[i];
    arr[i] = temp;
}

function tambahCatatan() {
    var catatan = $("#note").val();
    var date = new Date();
    if (catatan.length > 0) {
        //$("#note-list").append("<p style='margin-left:10px;'>" + catatan + "</p><p style='text-align:right;'><span class='text-muted'> " + date.toDateString() + " - " + date.toLocaleTimeString() + "|by: me</span></p><hr>");
        var str = '<div class="col-md-6 col-md-offset-3"><div class="box"><div class="user-block"><div class="img-profile">'+
        '<img alt="user image" src="dist/img/avatar04.png" class="img-circle img-bordered-sm "></div>'+
                                              '<span class="username profile-detail">' +
                                              catatan+
                                                  '</span>'+
                                                  '<span class="text-muted profile-detail paddingb10"><b>'+
                                                  'Saya' +
                                                  '</b> | ' +
                                                  date.toDateString() +' - '+ date.toLocaleTimeString()+
                                                  '</span></div></div></div>';
        $("#note-list").append(str);
    }
}
var bcde;
function generateTable(pengadaanId) {
    $.ajax({
        // url: 'data/rks-penilaian.json',
        url: 'Api/PengadaanE/getRKSKlarifikasi?PId=' + pengadaanId,
        dataType: 'json',
        success: function (data) {
            bcde = data;
            //header table
            var str_row = "<thead style='background-color:#11C2D7'><tr>" +
                '<th width="2%"></th>' +
                '<th>Item</th>' +
                '<th>Satuan</th>' +
                '<th>Jumlah</th>' +
                '<th>HPS</th>';
            for (var j in data.vendors) {
                str_row += '<th>' + data.vendors[j].nama + '</th>';
            }
            str_row += "</tr></thead><tbody>";

            //content table
            for (var i in data.hps) {//accounting.formatNumber(totalHarga, { thousand: "." })
                str_row += "<tr>" +
                    '<td>' + '<button data-idx="' + i + '" onclick="sortCol(this,' + i + ')" class="btn btn-xs btn-warning"><span class="fa fa-sort s-sort-symbol"></span></button>' + '</td>' +
                    '<td>' + data.hps[i].item + '</td>';
                if (data.hps[i].harga > 0) {
                    if (data.hps[i].satuan !=null) str_row += '<td>' + data.hps[i].satuan + '</td>';
                    else str_row += '<td></td>';
                    if (data.hps[i].jumlah > 0) str_row += '<td>' + accounting.formatNumber(data.hps[i].jumlah, { thousand: ".", decimal: ",", precision: 2 }) + '</td>';
                    else str_row += '<td></td>';
                    if (data.hps[i].harga > 0) str_row += '<td class="rata_kanan">' + accounting.formatNumber(data.hps[i].harga, { thousand: ".", decimal: ",", precision: 2 }) + '</td>';
                    else str_row += '<td></td>';
                    
                }
                else {
                    str_row += '<td></td>' +
                    '<td></td>' +
                    '<td></td>';
                }
                for (var j in data.vendors) {
                     var totItemInVendor = data.vendors[j].items.length;
                    if (i >= totItemInVendor)
                        str_row += '<td></td>';
                    else
                        if (data.hps[i].harga > 0 && data.vendors[j].items[i].harga > 0) {
                            str_row += '<td class="rata_kanan">' + accounting.formatNumber(data.vendors[j].items[i].harga, { thousand: ".", decimal: ",", precision: 2 }) + '</td>';
                        }
                        else {
                            str_row += '<td></td>';
                        }
                    //str_row += '<td>' + accounting.formatNumber(data.vendors[j].items[i].harga, { thousand: "." })+ '</td>';
                }
                str_row += "</tr>";
            }
            str_row += "</tbody>";
            $("#example1").append(str_row);
            setListHargaHPS(data.hps[0]);
            setListHargaVendor(data.vendors);

            table = $('#example1').DataTable({
                "scrollX": true,
                "paging": false,
                "lengthChange": false,
                "searching": false,
                "ordering": false,
                "info": false,
                "autoWidth": true,
                //responsive: true,
                colReorder: true

            });
            smallestValueInRowTable(table);
        }
    });
}

function smallestValueInRowTable(table) {

    table.rows().every(function (x, y) {
        var d = this.data();
        //var newD = d.splice(5, 5);
        var arr = [];
        $.each(d, function (index, val) {
            if (index > 4) {
                arr.push(val);
            }
        });
        var vals = $.map(arr, function (i) {
            return parseInt(UnformatFloat(i), 10) ? parseInt(UnformatFloat(i), 10) : null;
        });
        var min = Math.min.apply(Math, vals);
        var textMin = accounting.formatNumber(min, { thousand: ".", decimal: ",", precision: 2 });
        //var tableRow = $("td").filter(function () {
        //    return $(this).text() == textMin;
        //}).closest($('#example1 tr:eq("' + (x + 1) + '")'));
        $('#example1 tr:eq("' + (x + 1) + '")').find("td").each(function () {
            if ($(this).text() == textMin) {
                var colIndex = $(this).index();
                //table.cell(x, colIndex).data(textMin).draw();
                $(this).addClass("selected");
            }
        });

    });
}


function setListHargaHPS(hps) {
    var str_hps = '<div class="col-md-2"><div class="box box-primary box-folder-vendor"><div class="box-tools pull-right vendor-check-box"></div>' +
                            '<div class="box-body box-profile"  style="margin-top:3px">' +
                            '<p class="profile-username title-header"><a onclick="#">HPS</a></p>' +
                            '<p class="text-muted text-center deskripsi">Rp. ' + accounting.formatNumber(hps.harga, { thousand: ".", decimal: ",", precision: 2 }) + '</p></div></div></div>';
    $("#listHargaVendor").append(str_hps);
}

function setListHargaVendor(vendor) {
    var str_vendor = "";
    for (var i in vendor) { 
        str_vendor += '<div class="col-md-2"><div class="box box-primary box-folder-vendor"><div class="box-tools pull-right vendor-check-box"><div class="row" style="margin-right:3px;"><span class="fa fa-eye"></span> ' +
                            '<input class="s-checkbox" data-idx="'+i+'" type="checkbox" title="Tampil/Sembunyi" value="" checked onclick="showHideVendor(this,'+i+')" />' +
                            ' <span class="fa fa-lock unlocked"></span> ' +
                            '<input class="s-checkbox" data-idx="'+i+'" type="checkbox" title="Kunci/Lepas kunci" value="" onclick="lockUnlockVendor(this,'+i+')" /></div></div>' +
                            '<div class="box-body box-profile" style="margin-top:3px">' +
                            '<p class="profile-username title-header"><a onclick="gotoVendor(0)">'+vendor[i].nama+'</a></p>' +
                            '<p class="text-muted text-center deskripsi">Rp. ' + accounting.formatNumber(vendor[i].items[0].harga, { thousand: ".", decimal: ",", precision: 2 }) + '</p>' +
                            '<p class="text-muted text-center deskripsi">Bobot Nilai ' + (vendor[i].NIlaiKriteria == null ? 0 : vendor[i].NIlaiKriteria) + '</p>' +
                            '</div></div></div>';
    }
    $("#listHargaVendor").append(str_vendor);
}


$(function () {
    if (isGuid(id_pengadaan)) {
        $("#pengadaanId").val(id_pengadaan);
        loadData(id_pengadaan);
        generateTable(id_pengadaan);
    }
    else {
        if (isGuid($("#pengadaanId").val())) {
            window.location.hash = $("#pengadaanId").val();
            loadData($("#pengadaanId").val());
            generateTable(id_pengadaan);
        }
        else {

            window.location.replace("http://" + window.location.host);
            //$(location).attr('href', window.location.origin + "pengadaan-list.html");
        }
    }
       

    $("#cetak-rks-klarifikasi").on("click", function () {
       
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
        downloadFileUsingForm("/api/report/CetakRKSKlarfikasi?Id=" + $("#pengadaanId").val());
    });

    $("body").on("click", "#CetakXls", function () {
        downloadFileUsingForm("/api/report/CetakRKSKlarfikasiXls?Id=" + $("#pengadaanId").val());
    });
        
    
        $("#myNav").affix({
            offset: {
                top: 100
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
        $('#kreteriaPembobotan').on('change', ".input-bobot-pengadaan", function () {
            if (!$.isNumeric($(this).val())) {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Inputan Harus Angka dan Kurang dari 100',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
                $(this).val(0);
                return false;
            } else {
                if (parseInt($(this).val()) > 100) {
                    BootstrapDialog.show({
                        title: 'Konfirmasi',
                        message: 'Inputan Harus Angka dan Kurang dari 100',
                        buttons: [{
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
                    });
                    $(this).val(0);
                    return false;
                }
            }
            addPembobotanPengadaan($(this));
        });

        $(".edit-bobot-kriteria").on("click", function () {
            $("#kriteria-loader").show();
            $(".edit-bobot-kriteria").hide();
            var lstKriterai = [];
            var totalBobot = 0;
            var i = 1;
            $(".value-edit-bobot").each(function (index, val) {
                var objKriteria = {};
                objKriteria.PengadaanId = $("#pengadaanId").val();
                objKriteria.KreteriaPembobotanId = $(this).attr("attrId");
                var i = 1;
                if (!$.isNumeric($(this).val())) {
                    BootstrapDialog.show({
                        title: 'Konfirmasi',
                        message: 'Inputan Harus Angka dan Kurang dari 100',
                        buttons: [{
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
                    });
                    $(this).val(0);
                    i = 0;
                    $("#kriteria-loader").hide();
                    $(".edit-bobot-kriteria").show();
                    return false;
                }
                if (i == 0) {
                    $("#kriteria-loader").hide();
                    $(".edit-bobot-kriteria").show();
                    return false;
                }
                objKriteria.Bobot = $(this).val();
                lstKriterai.push(objKriteria);
                totalBobot += parseInt($(this).val());
            });

            if (i == 0) {
                $("#kriteria-loader").hide();
                $(".edit-bobot-kriteria").show();
                return false;
            }

            if (totalBobot > 100) {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Total Bobot Tidak Boleh Lebih dari 100%',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
                $("#kriteria-loader").hide();
                $(".edit-bobot-kriteria").show();
                return false;
            }
            $.ajax({
                method: "POST",
                url: "Api/PengadaanE/addLstPembobotanPengadaan",
                dataType: "json",
                data: JSON.stringify(lstKriterai),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data == 0) {
                        BootstrapDialog.show({
                            title: 'Error',
                            message: 'Gagal Menyimpan!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    dialog.close();
                                }
                            }]
                        });
                    }
                    else {
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Berhasil Menyimpan!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    dialog.close();
                                    window.location.reload();
                                }
                            }]
                        });
                    }
                    $("#kriteria-loader").hide();
                    $(".edit-bobot-kriteria").show();
                }
            });
        });
        $(".input-nilai-kriteria-vendor").on("click", function () {
            $("#nilai-vendor-loader").show();
            $(".input-nilai-kriteria-vendor").hide();
            var lstNilaiVendor = [];
            $(".input-nilai-vendor").each(function (index, val) {
                var objNilaiVendor = {};
                objNilaiVendor.PengadaanId = $("#pengadaanId").val();
                objNilaiVendor.VendorId = $(this).attr("vendorId");
                objNilaiVendor.KreteriaPembobotanId = $(this).attr("attrId");
                objNilaiVendor.Nilai = $(this).val();
                lstNilaiVendor.push(objNilaiVendor);
            });
            $.ajax({
                method: "POST",
                url: "Api/PengadaanE/addLstNilaiKriteriaVendor",
                dataType: "json",
                data: JSON.stringify(lstNilaiVendor),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data == 0) {
                        BootstrapDialog.show({
                            title: 'Error',
                            message: 'Gagal Menyimpan!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    dialog.close();
                                }
                            }]
                        });
                    }
                    else {
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Berhasil Menyimpan!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    dialog.close();
                                    window.location.reload();
                                }
                            }]
                        });
                    }
                    $("#nilai-vendor-loader").hide();
                    $(".input-nilai-kriteria-vendor").show();
                }
            });

        });

    });

   
function loadData(pengadaanId) {
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/detailPengadaan?Id=" + pengadaanId,
            dataType: "json"
        }).done(function (data) {
            $("#judul").text(data.Judul);
            $("#judul").text(data.Judul);
            $("#deskripsi").text(data.AturanPengadaan + ", " + data.AturanBerkas + ", " + data.AturanPenawaran);
            $("#keterangan").text(data.Keterangan);
            LoadRekananPembobotan();
            LoadKriteriaPembobotan(pengadaanId)
        });
    }
function LoadKriteriaPembobotan(PengadaanId) {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/getKriteriaPembobotan?PengadaanId=" + PengadaanId,
        success: function (data) {
            $.each(data, function (index, val) {
                html =
                    '<div class="form-group col-md-2">' +
                        '<label style="font-size:small">' + val.NamaKreteria + '</label>' +
                        '<input  attrId="' + val.Id + '" type="text" class="form-control value-edit-bobot" value=' + (val.Bobot == null ? 0 : val.Bobot) + ' >' +
                    '</div>';
                $("#kreteriaPembobotan").append(html);
            });

        }
    });


}
function LoadRekananPembobotan() {
    $("#NilaiPembobotanRekanan").html("");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetRekananPenilaian?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $.each(data, function (index, val) {
                var valx = val;
                if (valx.total > 0)
                    $.ajax({
                        method: "POST",
                        url: "Api/PengadaanE/getPembobotanNilaiVendor?PengadaanId=" + $("#pengadaanId").val() + "&VendorId=" + valx.VendorId,
                        success: function (data) {
                            $.each(data, function (index, val) {
                                if (index == 0) {
                                    html =
                                        '<div class="form-group col-md-12">' +
                                            '<label style="font-size:small">' + valx.NamaVendor + '</label>' +
                                        '</div>';
                                }
                                html = html + '<div class="form-group col-md-2">' +
                                        '<label style="font-size:small">' + 'Nilai Kriteria ' + val.NamaKreteria + '</label>' +
                                        '<input attrId="' + val.Id + '" vendorId="' + valx.VendorId + '" type="text" class="form-control input-nilai-vendor" value=' + val.Nilai + ' placeholder="NIlai">' +
                                    '</div>';
                            });
                            $("#NilaiPembobotanRekanan").append(html);
                        }
                    });
            });
        }
    });

}

