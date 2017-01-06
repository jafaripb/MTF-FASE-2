

var dataPengadaanPErhatian;
$(function () {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/isCreatePengadaan"
    }).done(function (data) {
        if (data == 1) $("#tambah").show();
    });
    $("#myNav").affix({
        offset: {
            top: 100
        }
    });
    //$("#downloadFile").on("click", function () {
    //    downloadFileUsingForm("/api/report/OpenFile");
    //});
    //$(".fa-download").click(function () {
    //    $("#view-siup").modal('show');
    //});
    $().UItoTop({ easingType: 'easeOutQuart' });
    $(".datepicker").datepicker({
        showOtherMonths: true,
        format: "dd/mm/yy",
        changeYear: true,
        changeMonth: true,
        yearRange: "-90:+4", //+4 Years from now

    });
    var table = $('#example1').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "columnDefs": [
          { className: "rata_kanan", "targets": [2] }
        ]
    });
    $(".remove-vendor").on("click", function () {
        var vl = $(this).closest('tr')[0];
        $("#row_index").val(vl.rowIndex);
        $("#ModalHapus").modal("show");
    });
    $("#eveHapus").on("click", function () {
        var index = $("#row_index").val();
        //var el=$('#example1 tr').eq(index);
        table.row(index).remove().draw();
        $("#ModalHapus").modal("hide");
    });
    $("#example1_filter").hide();
    $("#cari").on("click", function () {
        var kategori = $("#kategori option:selected").text();
        var subKategori = $("#sub_kategori option:selected").text();
        var nama_item = $("#nama_item").val();
        table.columns(0).search(nama_item).draw();
        //table.columns(1).search( kategori ).draw();
        //table.columns(2).search( subKategori ).draw();
    });
    $("body").on("click", "#add", function () {
        location.href = 'purchase-requests-add.html';
    });
    $(".perhatian").on("click", function () {
        $("#rpelaksanaan").hide();
        $("#rterjadwal").hide();
        $("#rarsip").hide();
        var start = $(this).attr("start");
        var count = $(".list-arsip").children().length;
        var nextStrart = count;
        $.ajax({
            url: "Api/PengadaanE/getPengadaanList?start=" + count + "&length=5&group=ARSIP&search=" + $("#search").val()
        }).done(function (data) {
            renderData(data.data);
            cekPaging(data.TotalRecord, ".list-arsip", "#arsip");
        });
        $("#pelaksanaan").html("Berikutnya");
        $("#terjadwal").html("Berikutnya");
    });
    $(".pelaksanaan").on("click", function () {
        $("#perlu_perhatian").hide();
        $("#rterjadwal").hide();
        $("#rarsip").hide();
        var start = $(this).attr("start");
        var count = $(".dalam-pelaksanaa").children().length;
        var nextStrart = count;
        $.ajax({
            url: "Api/PengadaanE/getPengadaanList?start=" + nextStrart + "&length=5&group=DALAMPELAKSANAAN&search=" + $("#search").val()
        }).done(function (data) {
            renderData(data.data);
            cekPaging(data.TotalRecord, ".dalam-pelaksanaa", "#pelaksanaan");
        });
        $("#arsip").html("Berikutnya");
        $("#terjadwal").html("Berikutnya");
    });
    $(".terjadwal").on("click", function () {
        $("#rpelaksanaan").hide();
        $("#perlu_perhatian").hide();
        $("#rarsip").hide();
        var start = $(this).attr("start");
        var count = $(".belum-terjadwal").children().length;
        var nextStrart = count;
        $.ajax({
            url: "Api/PengadaanE/getPengadaanList?start=" + nextStrart + "&length=5&group=BELUMTERJADWAL&search=" + $("#search").val()
        }).done(function (data) {
            renderData(data.data);
            cekPaging(data.TotalRecord, ".belum-terjadwal", "#terjadwal");
        });
        $("#pelaksanaan").html("Berikutnya");
        $("#arsip").html("Berikutnya");
    });
    $(".arsip").on("click", function () {
        $("#perlu_perhatian").hide();
        $("#rpelaksanaan").hide();
        $("#rterjadwal").hide();
        var start = $(this).attr("start");
        var count = $(".list-arsip").children().length;
        var nextStrart = count;
        $.ajax({
            url: "Api/PengadaanE/getPengadaanList?start="+count+"&length=5&group=ARSIP&search=" + $("#search").val()
        }).done(function (data) {
            renderData(data.data);
            cekPaging(data.TotalRecord, ".list-arsip", "#arsip");
        });
        $("#pelaksanaan").html("Berikutnya");
        $("#terjadwal").html("Berikutnya");
    });
    //keydown //input propertychange paste
    $("#header").on("keydown", "#search", function (e) {
        var keyCode = e.keyCode || e.which;

        var search = $(this).val().toLowerCase
        if (keyCode == 13) {
            $(".dalam-pelaksanaa").html("");
            $(".belum-terjadwal").html("");
            $(".list-arsip").html("");
            $.ajax({
                url: "Api/PengadaanE/getPengadaanList?start=0&length=5&group=DALAMPELAKSANAAN&search=" + $("#search").val()
            }).done(function (data) {
                renderData(data.data);
                cekPaging(data.TotalRecord, ".dalam-pelaksanaa", "#pelaksanaan");
            });
            $.ajax({
                url: "Api/PengadaanE/getPengadaanList?start=0&length=5&group=BELUMTERJADWAL&search=" + $("#search").val()
            }).done(function (data) {
                renderData(data.data);
                cekPaging(data.TotalRecord, ".belum-terjadwal", "#terjadwal");
            });
            $.ajax({
                url: "Api/PengadaanE/getPengadaanList?start=0&length=5&group=ARSIP&search=" + $("#search").val()
            }).done(function (data) {
                renderData(data.data);
                cekPaging(data.TotalRecord, ".list-arsip", "#arsip");
            });
        }
        //$(".item-pengadaan").hide();
        //$(".item-pengadaan").each(function () {
        //    var string = $(this).find("div.box").find("div.box-header h3").text().toLowerCase();
        //    var count = string.split(search).length;
        //    if (count > 1) {
        //        $(this).show();
        //    }
        //});
    });
    $(".remove-item-pengadaan").on("click", function () {
        $(this).parent().parent().parent().parent().parent().parent().remove();
    });
    $.ajax({
        url: "Api/PengadaanE/getPerhatianPengadaanList?start=0&length=5&search=" + $("#search").val()
    }).done(function (data) {
        $(".perlu-perhatian").html("");
        renderDataPerluPerhatian(data);
        //renderData(data);
    });
    $.ajax({
        url: "Api/PengadaanE/getPengadaanList?start=0&length=5&group=DALAMPELAKSANAAN&search=" + $("#search").val()
    }).done(function (data) {
        $(".dalam-pelaksanaa").html("");
        renderData(data.data);
        cekPaging(data.TotalRecord, ".dalam-pelaksanaa", "#pelaksanaan");
    });
    $.ajax({
        url: "Api/PengadaanE/getPengadaanList?start=0&length=5&group=BELUMTERJADWAL&search=" + $("#search").val()
    }).done(function (data) {
        $(".belum-terjadwal").html("");
        renderData(data.data);
        cekPaging(data.TotalRecord, ".belum-terjadwal", "#terjadwal");
    });
    $.ajax({
        url: "Api/PengadaanE/getPengadaanList?start=0&length=5&group=ARSIP&search=" + $("#search").val()
    }).done(function (data) {
        $(".list-arsip").html("");
        renderData(data.data);
        cekPaging(data.TotalRecord, ".list-arsip", "#arsip");
    });
    $(".belum-terjadwal").on("click", ".HapusPengadaan", function () {
        var pengadaanId = $(this).attr("attrId");
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Apakah Anda Yakin Ingin Mengahapus Pengadaan Ini?',
            buttons: [{
                label: 'Lanjutkan',
                action: function (dialog) {
                    waitingDialog.showloading("Proses Harap Tunggu");
                    $.ajax({
                        //method: "POST",
                        url: "Api/PengadaanE/deletePengadaan?Id=" + pengadaanId,
                        error: function () {
                            waitingDialog.hideloading();
                        }

                    }).done(function (data) {
                        waitingDialog.hideloading();
                        if (data.status == 200) {
                            location.reload();
                        }
                        else alert("Hapus Gagal");
                    });

                    dialog.close();
                },
            }, {
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                },
            }]
        });

    });
    $("body").on("click", ".detail-pengadaan", function () {
        var id = $(this).attr("attrId");
        $(location).attr('href', '/pengadaan-detail.html#' + id);
    });
    $(".perlu-perhatian").on("click", ".setujui", function () {
        var id = $(this).attr("attrId");
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Dengan menekan tombol "setujui" berarti pengadaan ini memiliki ketetapan akan dilaksanakan oleh sistem',
            buttons: [{
                label: 'Lanjutkan',
                action: function (dialog) {
                    waitingDialog.showloading("Proses Harap Tunggu");
                    $.ajax({
                        method: "POST",
                        url: "Api/PengadaanE/persetujuan?Id=" + id,
                        success: function (data) {
                            if (data.status == 200) {
                                //  $(location).attr('href', window.location.host + "/pengadaan-list.html");
                                window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
                                waitingDialog.hideloading();
                            }
                        },
                        error: function (errormessage) {
                            alert("gagal");
                            waitingDialog.hideloading();
                        }
                    });
                    dialog.close();
                }
            }, {
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
    });

    $(".perlu-perhatian").on("click", ".tolak", function () {
        var id = $(this).attr("attrId");
        
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: '<p>Anda Yakin Menolak Pengadaan Ini?</p><p>Alasan Penolakan:</p><textarea class="form-control" id="keterangan_penolakan"></textarea>',
            buttons: [{
                label: 'Lanjutkan',
                action: function (dialog) {
                    var alasan = $.trim($("#keterangan_penolakan").val());
                    if (alasan == "") {
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: "Alasan Penolakan Harus DiIsi!",
                            buttons: [{
                                label: 'Close',
                                action: function (dialogx) {
                                    dialogx.close();
                                }
                            }]
                        });
                        return false;
                    }
                    waitingDialog.showloading("Proses Harap Tunggu");
                    var objData = {};
                    objData.PenolakanId = id;
                    objData.AlasanPenolakan = alasan;
                    $.ajax({
                        method: "POST",
                        dataType: "json",
                        data: JSON.stringify(objData),
                        contentType: 'application/json; charset=utf-8',
                        url: "Api/PengadaanE/tolakPengadaan",
                        success: function (data) {
                            if (data.status == 200) {
                                //  $(location).attr('href', window.location.host + "/pengadaan-list.html");
                                window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
                            }
                        },
                        error: function (errormessage) {
                            alert("gagal");
                            waitingDialog.hideloading();
                        }
                    });
                    dialog.close();
                }
            }, {
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
    });

    $("body").on("click", ".download-dokumen", function () {
        downloadFileUsingForm("/api/pengadaane/OpenFile?Id=" + $(this).attr("attr1"));
    });
});

function cekPaging(totalRecord, elBox, elPaging) {
    if ($(elBox).find("li").length >= totalRecord) $(elPaging).html("");
    else $(elPaging).html("Berikutnya");
    if ($(elBox).find("li").length == 0) $(elBox).html("<i>Kosong</i>");

}


function renderData(data) {
    for (var i in data) {
        var html = '<li class="item">' +
                 '<div class="product-img pegadaan-bintang">' +
                     '<span ><a href="#"><i class="fa fa-fw fa-star-o" ></i></a></span>' +
                 '</div>' +
                 '<div class="product-info pegadaan-info">';
        if (data[i].GroupPengadaan == 1) {
            if (data[i].AturanPengadaan == "Pengadaan Terbuka") {
                 html = html + '<a href="pengadaan-terbuka-detail.html#' + data[i].Id + '" class="product-title text-blue-mtf">' + data[i].Judul + ' </a>';
                }
            else {
                html = html + '<a href="pengadaan-detail.html#' + data[i].Id + '" class="product-title text-blue-mtf">' + data[i].Judul + ' </a>';
             }
         }

        if (data[i].GroupPengadaan == 2) {
            if (data[i].AturanPengadaan == "Pengadaan Terbuka") {
                html = html + '<a href="pengadaan-terbuka-detail.html#' + data[i].Id + '" class="product-title text-blue-mtf">' + data[i].Judul + ' </a>';
            }
            else {
                html = html + '<a href="pengadaan-detail.html#' + data[i].Id + '" class="product-title text-blue-mtf ">' + data[i].Judul + ' </a>';
            }
            if (data[i].Status == 0)
                if (data[i].isCreated == 1 || data[i].isPIC == 1) {
                    html = html + '<span attrId="' + data[i].Id + '" class="label label-danger pull-right action HapusPengadaan">Hapus</span>';
                }
            html = html + '<span attrId="' + data[i].Id + '" class="label label-primary pull-right action detail-pengadaan">Detail </span>';
        }

        if (data[i].GroupPengadaan == 3) {
            if (data[i].AturanPengadaan == "Pengadaan Terbuka") {
                html = html + '<a href="pengadaan-terbuka-detail.html#' + data[i].Id + '" class="product-title text-blue-mtf">' + data[i].Judul + ' </a>';
            }
            else {
                html = html + '<a href="pengadaan-detail.html#' + data[i].Id + '" class="product-title text-blue-mtf">' + data[i].Judul + ' </a>';
            }
       }


        html = html + '<span class="product-description pegadaan-description">' +
                            (data[i].NoPengadaan != null ? data[i].NoPengadaan + "," : "") +
                            data[i].AturanPengadaan + ',' + data[i].AturanBerkas + ',' + data[i].AturanPenawaran + '.' +
                    '</span>';



        if (data[i].KandidatPengadaans.length > 0) {
            html = html + '<span class="product-description pegadaan-item">' +
                  '<i class="fa fa-fw fa-users"></i>';
            var jum = data[i].KandidatPengadaans.length;
            $.each(data[i].KandidatPengadaans, function (index, value) {
                if (index+1 == jum)
                    html = html + value.Nama;
                else html = html + value.Nama + ', ';
                
            });
            //for (var key in data[i].KandidatPengadaans) {
               
            //}
            html = html + '</span>';
        }        
        if (data[i].JadwalPengadaans.length > 0) {
            html = html + '<span class="product-description pegadaan-item">' +
                  '<i class="fa fa-fw fa-calendar"></i>';
            var Aanwijzing = "Aanwijzing :";
            var pengisian_harga = "Pengisian Harga :";
            var klarifikasi = "Klarifikasi :";
            var penentuan_pemenang = "Penentuan Pemenang :";
            $.each(data[i].JadwalPengadaans, function (index, val) {

                if (val.tipe == "Aanwijzing") {
                    if (moment(val.Mulai).format("DD/MM/YYYY") != "Invalid date") {
                        Aanwijzing = Aanwijzing + moment(val.Mulai).format("DD/MM/YYYY");
                    }
                }
                if (val.tipe == "pengisian_harga") {
                    if (moment(val.Mulai).format("DD/MM/YYYY") != "Invalid date") {
                        pengisian_harga = pengisian_harga + moment(val.Mulai).format("DD/MM/YYYY");
                    }
                    if (moment(val.Sampai).format("DD/MM/YYYY") != "Invalid date") {
                        pengisian_harga = pengisian_harga + " s/d " + moment(val.Sampai).format("DD/MM/YYYY");
                    }
                }
                if (val.tipe == "klarifikasi") {
                    if (moment(val.Mulai).format("DD/MM/YYYY") != "Invalid date") {
                        klarifikasi = klarifikasi + moment(val.Mulai).format("DD/MM/YYYY");
                    }
                    if (moment(val.Sampai).format("DD/MM/YYYY") != "Invalid date") {
                        klarifikasi = klarifikasi + " s/d " + moment(val.Sampai).format("DD/MM/YYYY");
                    }
                }
                if (val.tipe == "penentuan_pemenang") {

                    if (moment(val.Mulai).format("DD/MM/YYYY") != "Invalid date") {
                        penentuan_pemenang = penentuan_pemenang + moment(val.Mulai).format("DD/MM/YYYY");
                    }
                }
            });


            html = html + Aanwijzing + "," + pengisian_harga + "," + klarifikasi + "," + penentuan_pemenang + '</span>';
        }
        if (data[i].DokumenPengadaans.length > 0) {
            html = html + '<span class="product-description pegadaan-item">' +
                  '<i class="fa fa-fw fa-files-o" ></i>';
            var nota = data[i].TitleDokumenNotaInternal + " : ";
            var doklain = data[i].TitleDokumenLain + " : ";
            var berkas = data[i].TitleBerkasRujukanLain + " : ";
            for (var key in data[i].DokumenPengadaans) {

                if (data[i].DokumenPengadaans[key].Tipe == 0) {
                    nota = nota + '<a class="download-dokumen" attr1="' + data[i].DokumenPengadaans[key].Id + '" ><i class="fa fa-fw  fa-download"></i></a> ';
                }
                if (data[i].DokumenPengadaans[key].Tipe == 1) {
                    doklain = doklain + '<a class="download-dokumen" attr1="' + data[i].DokumenPengadaans[key].Id + '" ><i class="fa fa-fw  fa-download"></i></a> ';
                }
                if (data[i].DokumenPengadaans[key].Tipe == 1) {
                    berkas = berkas + '<a class="download-dokumen" attr1="' + data[i].DokumenPengadaans[key].Id + '" ><i class="fa fa-fw  fa-download"></i></a> ';
                }
            }

            html = html + nota + "," + doklain + "," + berkas + '</span>';
        }
        if (data[i].PersonilPengadaans.length > 0) {
            html = html + '<span class="product-description pegadaan-item">';
            for (var key in data[i].PersonilPengadaans) {
                //html = html + '<img  src="dist/img/avatar04.png" height="18px" class="user-image" alt="User Image">' + data[i].PersonilPengadaans[key].Nama + ' ';
                html = html + '<i class="fa fa-fw fa-user" ></i>' + data[i].PersonilPengadaans[key].Nama + ' ';
            }
        }
        html = html + '</span>';
        html = html +
        '</div>' +
        '</li>';
        if (data[i].GroupPengadaan == 2) {
            $(".belum-terjadwal").append(html);
        }
        else if (data[i].GroupPengadaan == 1) {
            $(".dalam-pelaksanaa").append(html);
        }
        else {
            $(".list-arsip").append(html);
        }
    }
}

function renderDataPerluPerhatian(data) {
    if (data.length == 0) {
        $(".perlu-perhatian").html("<i>Kosong</i>");
    }
    for (var i in data) {
        var html = '<li class="item">' +
                 '<div class="product-img pegadaan-bintang">' +
                     '<span ><a href="#"><i class="fa fa-fw fa-star-o" ></i></a></span>' +
                 '</div>' +
                 '<div class="product-info pegadaan-info">';
        if (data[i].AturanPengadaan == "Pengadaan Terbuka") {
            html = html + '<a href="pengadaan-terbuka-detail.html#' + data[i].Id + '" class="product-title text-blue-mtf">' + data[i].Judul + ' </a>';
        }
        else {
            html = html + '<a href="pengadaan-detail.html#' + data[i].Id + '" class="product-title text-blue-mtf">' + data[i].Judul + ' </a>';
        }
        if (data[i].Status == 1) {
            if (data[i].Approver == 1) {
                html = html + '<span class="label label-danger pull-right action tolak" attrId="' + data[i].Id + '">Tolak</span>' +
                '<span class="label label-success pull-right action setujui" attrId="' + data[i].Id + '">Setujui</span>';
            }
            else {
                html = html + '<span class="label label-primary pull-right action">Dalam Pengajuan</span>';
            }

        }
        if (data[i].Status == 10) {

            html = html + '<span class="label label-primary pull-right action">Pengajuan Ditolak!</span>';


        }
        if (data[i].Status == 8) {
            html = html + '<span class="label label-primary pull-right action">Pengadaan Ini Ditolak!</span>';
        }


        if (data[i].Status == 3) {
            html = html + '<a href="pengadaan-arsip.html#' + data[i].Id + '" class="product-title text-blue-mtf">' + data[i].Judul + ' </a>';
        }

        html = html + '<span class="product-description pegadaan-description">' +
                         data[i].AturanPengadaan + ',' + data[i].AturanBerkas + ',' + data[i].AturanPenawaran + '.' +
                    '</span>';
        if (data[i].KandidatPengadaans.length > 0) {
            html = html + '<span class="product-description pegadaan-item">' +
                  '<i class="fa fa-fw fa-users"></i>';
            for (var key in data[i].KandidatPengadaans) {
                html = html + data[i].KandidatPengadaans[key].Nama + ' ';
            }
            html = html + '</span>';
        }
        if (data[i].JadwalPengadaans.length > 0) {
            html = html + '<span class="product-description pegadaan-item">' +
                  '<i class="fa fa-fw fa-calendar"></i>';
            var Aanwijzing = "Aanwijzing :";
            var pengisian_harga = "Pengisian Harga :";
            var klarifikasi = "Klarifikasi :";
            var penentuan_pemenang = "Penentuan Pemenang :";
            $.each(data[i].JadwalPengadaans, function (index, val) {
                if (val.tipe == "Aanwijzing") {
                    if (moment(val.Mulai).format("DD/MM/YYYY") != "Invalid date") {
                        Aanwijzing = Aanwijzing + moment(val.Mulai).format("DD/MM/YYYY");
                    }
                }
                if (val.tipe == "pengisian_harga") {
                    if (moment(val.Mulai).format("DD/MM/YYYY") != "Invalid date") {
                        pengisian_harga = pengisian_harga + moment(val.Mulai).format("DD/MM/YYYY");
                    }
                    if (moment(val.sampai).format("DD/MM/YYYY") != "Invalid date") {
                        pengisian_harga = pengisian_harga + " s/d " + moment(val.sampai).format("DD/MM/YYYY");
                    }
                }
                if (val.tipe == "klarifikasi") {
                    if (moment(val.Mulai).format("DD/MM/YYYY") != "Invalid date") {
                        klarifikasi = klarifikasi + moment(val.Mulai).format("DD/MM/YYYY");
                    }
                    if (moment(val.sampai).format("DD/MM/YYYY") != "Invalid date") {
                        klarifikasi = klarifikasi + " s/d " + moment(val.sampai).format("DD/MM/YYYY");
                    }
                }
                if (val.tipe == "penentuan_pemenang") {

                    if (moment(val.Mulai).format("DD/MM/YYYY") != "Invalid date") {
                        penentuan_pemenang = penentuan_pemenang + moment(val.Mulai).format("DD/MM/YYYY");
                    }
                }
            });


            html = html + Aanwijzing + "," + pengisian_harga + "," + klarifikasi + "," + penentuan_pemenang + '</span>';
        }
        if (data[i].DokumenPengadaans.length > 0) {
            html = html + '<span class="product-description pegadaan-item">' +
                  '<i class="fa fa-fw fa-files-o" ></i>';
            var nota = data[i].TitleDokumenNotaInternal + " : ";
            var doklain = data[i].TitleDokumenLain + " : ";
            var berkas = data[i].TitleBerkasRujukanLain + " : ";
            for (var key in data[i].DokumenPengadaans) {

                if (data[i].DokumenPengadaans[key].Tipe == 0) {
                    nota = nota + '<a  class="download-dokumen" attr1="' + data[i].DokumenPengadaans[key].Id + '" ><i class="fa fa-fw  fa-download"></i></a> ';
                }
                if (data[i].DokumenPengadaans[key].Tipe == 1) {
                    doklain = doklain + '<a   class="download-dokumen" attr1="' + data[i].DokumenPengadaans[key].Id + '" ><i class="fa fa-fw  fa-download"></i></a> ';
                }
                if (data[i].DokumenPengadaans[key].Tipe == 1) {
                    berkas = berkas + '<a  class="download-dokumen" attr1="' + data[i].DokumenPengadaans[key].Id + '" ><i class="fa fa-fw  fa-download"></i></a> ';
                }
            }

            html = html + nota + "," + doklain + "," + berkas + '</span>';
        }
        if (data[i].PersonilPengadaans.length > 0) {
            html = html + '<span class="product-description pegadaan-item">';
            for (var key in data[i].PersonilPengadaans) {
                //html = html + '<img  src="dist/img/avatar04.png" height="18px" class="user-image" alt="User Image">' + data[i].PersonilPengadaans[key].Nama + ' ';
                html = html + '<i class="fa fa-fw fa-user" ></i>' + data[i].PersonilPengadaans[key].Nama + ' ';
            }
        }
        html = html + '</span>';
        html = html +
        '</div>' +
        '</li>';
        if (data[i].Status == 1) {
            $(".perlu-perhatian").append(html);
        }
        if (data[i].Status == 10) { //&& data[i].Approver == 0
            $(".perlu-perhatian").append(html);
        }
    }
}


$("#pilih-tipe-pengadaan").on("click", function () {
    BootstrapDialog.show({
        title: 'Pilih Pengadaan',
        message: '<a id="add_tertutup" href="pengadaan-add.html" class="btn bg-navy btn-flat margin">Pengadaan Tertutup</a> <a id="add_terbuka" href="pengadaan_add_terbuka.html" class="btn bg-orange btn-flat margin">Pengadaan Terbuka</a>',
        buttons: [{
            label: 'Close',
            action: function (dialog) {

                dialog.close();
            }
        }]
    });
});