
$(function () {

    $(".date").datetimepicker({
        format: "DD MMMM YYYY",
        locale: 'id'
    }).on('changeDate', function (ev) {
    });

    $("#report").on("click", function () {
        var dari = moment($("#report_dari").val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");
        var sampai = moment($("#report_sampai").val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");

        downloadFileUsingForm("/api/report/ReportPengadaan?dari=" + dari + "&sampai=" + sampai);
    });
});

function downloadFileUsingForm(url) {
    var form = document.createElement("form");
    form.method = "post";
    form.action = url;
    document.body.appendChild(form);
    form.submit();
    document.body.removeChild(form);
}




