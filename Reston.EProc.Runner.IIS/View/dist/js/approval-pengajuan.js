

$(function () {

    $("#myNav").affix({
        offset: {
            top: 100
        }
    });
    $().UItoTop({ easingType: 'easeOutQuart' });

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
});

