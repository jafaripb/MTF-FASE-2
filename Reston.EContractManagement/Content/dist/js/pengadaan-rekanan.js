
$(function () {
    $("#myNav").affix({
        offset: {
            top: 100
        }
    });
    $().UItoTop({ easingType: 'easeOutQuart' });
    var jum = $(".dalam-pelaksanaan li").length;
    $.ajax({
        url: "Api/PengadaanE/getPengadaanForRekananList?start=0&length=5&group=DALAMPELAKSANAAN"
    }).done(function (data) {
        $(".dalam-pelaksanaan").html("");
        renderList(data);
    });

    $.ajax({
        url: "Api/PengadaanE/getPengadaanForRekananList?start=0&length=5&group=ARSIP"
    }).done(function (data) {
        if (data.length > 0) {
            $(".arsip").html("");
            renderList(data);
        }
    });
    
    $("#next-dalam-pelaksanaan").on("click", function () {
        var jum = $(".dalam-pelaksanaan li").length;
        $.ajax({
            url: "Api/PengadaanE/getPengadaanForRekananList?start=" + jum + "&length=5&group=DALAMPELAKSANAAN"
        }).done(function (data) {
            if (data.length > 0)
                renderList(data);
            else {
                $("#next-dalam-pelaksanaan").hide();
            }
        });
    });
});


function renderList(data) {
    var html = "";    
    $.each(data, function (index, value) {       
        html = '<li class="item">' +
            '<div class="product-img pegadaan-bintang">' +
            '<span><a href="#"><i class="fa fa-fw fa-star-o"></i></a></span>' +
            '</div>' +
            '<div class="product-info pegadaan-info">' +
            '<a href="rekanan-side-detail-pengadaan.html#' + value.Id + '" class="product-title text-blue-mtf">' + value.Judul + '</a>' +
            '<span class="label label-danger pull-right action"></span>' +
            '<span class="label label-success pull-right action"></span>' +
            '<span class="product-description pegadaan-description">' + (value.NoPengadaan!=null?value.NoPengadaan+",":"")+
                 value.AturanPengadaan + ',' + value.AturanBerkas + ',' + value.AturanPenawaran + '.' +
            '</span>';
        if (value.JadwalPengadaans.length > 0) {
            html = html + '<span class="product-description pegadaan-item">' +
                  '<i class="fa fa-fw fa-calendar"></i>';
            var Aanwijzing = "Aanwijzing :";
            var pengisian_harga = "Pengisian Harga :";
            var klarifikasi = "Klarifikasi :";
            var penentuan_pemenang = "Penentuan Pemenang :";
            $.each(value.JadwalPengadaans, function (index, val) {

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
        html = html +
            '<span class="product-description pegadaan-item">' +
                '<i class="fa fa-fw fa-location-arrow"></i>' + value.Region + ', ' + value.Provinsi + ' <a href="#"><i class="fa fa-fw  fa-map-marker"></i></a>' +
            '</span>' +
        '</div>' +
        '</li>';
        
        if (value.GroupPengadaan == 1) {
            $(".dalam-pelaksanaan").append(html);
        }
        else {
            $(".arsip").append(html);
        }
        //$(".dalam-pelaksanaan").append(html);
    });
    if (html == "") {
        $(".dalam-pelaksanaan").append("Kosong");
    }
}
