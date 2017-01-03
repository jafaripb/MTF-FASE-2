var id_pengadaan = window.location.hash.replace("#", "");
var table;
$(function () {
    if (isGuid(id_pengadaan)) {
        $("#pengadaanId").val(id_pengadaan);
        loadData(id_pengadaan);
    }
    else {
        if (isGuid($("#pengadaanId").val())) {
            window.location.hash = $("#pengadaanId").val();
            loadData($("#pengadaanId").val());
        }
        else {            
            window.location.replace("http://" + window.location.host + "/pengadaan-rekanan.html");
        }
    }

    $("#tab-pelakasanaan").on("click", function () {
        $("#side-kanan").find(".rk").hide();
        $("#side-kanan").find(".pl").show();
    });
    $("#tab-berkas").on("click", function () {
        $("#side-kanan").find(".rk").hide();
        $("#side-kanan").find(".pl").hide();
    });
    $("#tab-rk").on("click", function () {
        $("#side-kanan").find(".rk").show();
        $("#side-kanan").find(".pl").hide();
    });

    $("#side-kanan").find(".pl").hide();

    $("#myNav").affix({
        offset: {
            top: 100
        }
    });
    $(".fa-download").click(function () {
        $("#view-siup").modal('show');
    });

    $(".tab-content").show();

    $("#side-kanan").on("click", "li", function () {
        $("#side-kanan").find("li").each(function () {
            $(this).removeClass("active");
        });
        $(this).addClass("active");
    });
        
   

    $(".tab-content").show();
    table = $('#example1').DataTable({
        "paging": false,
        "lengthChange": false,
        "searching": false,
        "ordering": false,
        "info": false,
        "autoWidth": true,
        //responsive: true,
        "ajax": "Api/PengadaanE/getRksKlarifikasiLanjutanRekanan?id=" + $("#pengadaanId").val(),
        "columns": [
            { "data":null },
            { "data": "item" },
            { "data": "keteranganItem" },
            { "data": "satuan" },            
            { "data": "jumlah" ,"className": "tengah" },
            { "data": null },
            { "data": null, "className": "rata_kanan" },
            { "data": null }
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    if (row.level == 0) return row.judul;
                    else return "";
                },
                "targets": 0,
                "orderable": false
            },
                {
                    "render": function (data, type, row) {
                        //<input type="text" class="auto"  data-a-sign="€ " data-v-max="10000000000000000">
                        if (row.jumlah > 0) {
                            var harga = "0";

                            var num = row.harga.toString().replace(",", ".");

                            if (num > 0) {
                                harga = accounting.formatNumber(num, { thousand: ".", decimal: ",", precision: 2 });
                            }
                            return '<input type="text" class="form-control harga-rekanan" value="' + harga + '"  attrId="' + row.Id + '" />';

                        }
                        else return '<input type="hidden" class="harga-rekanan" attrId="' + row.Id + '" />';

                        // $('input.harga-rekanan').autoNumeric({ aSep: '.', aDec: ',' });
                    },
                    "targets": 5,
                    "orderable": false
                },
                {
                    "render": function (data, type, row) {
                        if (row.jumlah != null) {
                            var num = row.harga.toString().replace(",", ".");
                            if (row.jumlah > 0 && num > 0) {
                                return accounting.formatNumber(num * row.jumlah, { thousand: ".", decimal: ",", precision: 2 });
                            }
                            else return 0;
                        }
                        else return "";
                    },
                    "targets": 6,
                    "orderable": false
                },
                {
                    "render": function (data, type, row) {
                        if (row.jumlah != null) {
                            if (row.jumlah > 0) {
                                return '<textarea class="form-control undangan" attrId="' + row.Id + '">' + row.keterangan + '</textarea>';
                            } else return "";
                        }
                        else return "";
                    },
                    "targets": 7,
                    "orderable": false
                }
        ],
        "rowCallback": function (row, data, index) {
            hitungHargaItem();
        },
        "initComplete": function (oSettings, json) {    
            $(".harga-rekanan").number(true, 0, ",", ".");
            hitungHargaItem();
        }
    });
    //,vMax:'1000000000000'}); });
    $('#example1').on("click", ".harga-rekanan", function () {
        var baris = $(this).parent().closest("tr").index();
        var oldRowData = table.row(baris).data();
        $(this).focusout(function () {           
            var newData = {};
            newData = oldRowData;
            newData.harga = UnformatFloat($(this).val());
            updateData(baris, newData);
            hitungHargaItem();
        });        
    });
    $('#example1').on("click", ".undangan", function () {
        var baris = $(this).parent().closest("tr").index();
        var oldRowData = table.row(baris).data();
        $(this).focusout(function () {
            var newData = {};
            newData = oldRowData;
            newData.keterangan = $(this).val();
            updateData(baris, newData);
            hitungHargaItem();
        });
    });

    $(".save-harga").on("click", function () {
        var data = datatableToJson(table);
        var arrData = [];
        $.each(data, function (index,value) {
            var objData = {};
            objData.Id = value.Id;
            if (value.HargaRekananId != "00000000-0000-0000-0000-000000000000")
                objData.HargaRekananId = value.HargaRekananId;
            if(value.harga!="" &&value.harga!=null)
                objData.harga = value.harga;
            arrData.push(objData);
        });
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/addHargaKlarifikasiLanjutanRekanan?PengadaanId=" + $("#pengadaanId").val(),
            dataType: "json",
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8',
            success: function (xdata) {
                waitingDialog.hideloading();
                if (xdata.Id == "1") {
                    BootstrapDialog.show({
                        title: 'Konfirmasi',
                        message: 'Harga Tawaran Berhasil di Kirim',
                        buttons: [{
                            label: 'Kembali',
                            action: function (dialog) {
                                window.location.replace("http://" + window.location.host + "/rekanan-side-detail-pengadaan.html#" + $("#pengadaanId").val());
                            }
                        },
                                {
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
                        message: 'Penewaran di tutup',
                        buttons: [{
                            label: 'Kembali',
                            action: function (dialog) {
                                window.location.replace("http://" + window.location.host + "/rekanan-side-detail-pengadaan.html#" + $("#pengadaanId").val());
                            }
                        },
                                {
                                    label: 'Close',
                                    action: function (dialog) {
                                        dialog.close();
                                    }
                                }]
                    });
                }
            },
            error: function (errormessage) {
                waitingDialog.hideloading();
                BootstrapDialog.show({
                    title: 'Error!!',
                    message: errormessage,
                    buttons: [{
                        label: 'Kembali',
                        action: function (dialog) {
                            window.location.replace("http://" + window.location.host + "/rekanan-side-detail-pengadaan.html#" + $("#pengadaanId").val());
                        }
                    },
                            {
                                label: 'Close',
                                action: function (dialog) {
                                    dialog.close();
                                }
                            }]
                });
            }
        })
    });
});

function updateData(baris, newdata) {
    $.extend(table.row(baris).data(), newdata);
    table.row(baris).invalidate();
}

function hitungHargaItem() {
    var totalHarga = 0;
    table.rows().every(function () {
        var d = this.data();

        var numHarga = parseFloat(d.harga.toString().replace(",", "."));

        if (numHarga > 0) {

            var numJumlah = parseFloat(d.jumlah.toString().replace(",", "."));
            console.log(numHarga);
            var harga = numHarga * numJumlah;

            d.counter++;
            //harga = parseInt(UnformatFloat(harga));
            this.invalidate(); // invalidate the data DataTables has cached for this row

            totalHarga = totalHarga + harga;
        }
    });

    $("#total").text(accounting.formatNumber(totalHarga, { thousand: "." ,decimal: ",", precision: 2}));
}

function datatableToJson(table) {
    var data = [];
    table.rows().every(function () {
        var itemObj = {};
        var d = this.data();
        d.harga = d.harga.toString().replace(",", ".");
        itemObj = d;
        data.push(itemObj);
    });
    return data;
    //console.log(JSON.stringify(data));
}

function loadData(pengadaanId) {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/detailPengadaanForRekanan?Id=" + pengadaanId,
        dataType: "json"
    }).done(function (data) {
        $("#judul").text(data.Judul);
        $("#deskripsi").text( data.AturanPengadaan + ", " + data.AturanBerkas + ", " + data.AturanPenawaran);        
        $("#keterangan").text(data.Keterangan);
        $("#MataUang").text(data.MataUang);
        $("#UnitKerjaPemohon").text(data.UnitKerjaPemohon);
        $("#Region").text(data.Region);
        $("#Provinsi").text(data.Provinsi);
        $("#JenisPekerjaan").text(data.JenisPekerjaan);
        $("#pengadaanId").val(data.Id);
        if (data.StatusName != "KLARIFIKASILANJUTAN") {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Anda Tidak Masuk ke Tahap Klarifikasi Lanjutan',
                buttons: [{
                    label: 'Kembali',
                    action: function (dialog) {
                        window.location.replace("http://" + window.location.host + "/pengadaan-rekanan.html");

                    }
                }]
            });
        }
        if (data.StatusName != "KLARIFIKASILANJUTAN")
            window.location.replace("http://" + window.location.host + "/rekanan-side-detail-pengadaan.html#" + pengadaanId);        
    });
}

