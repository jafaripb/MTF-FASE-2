
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

    $("#report_pks").on("click", function () {
        var dari = moment($("#report_pks_dari").val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");
        var sampai = moment($("#report_pks_sampai").val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");

        downloadFileUsingForm("/api/report/ReportPKS?dari=" + dari + "&sampai=" + sampai);
    });

    $("#report_spk").on("click", function () {
        var dari = moment($("#report_spk_dari").val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");
        var sampai = moment($("#report_spk_sampai").val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");

        downloadFileUsingForm("/api/report/ReportSPK?dari=" + dari + "&sampai=" + sampai);
    });

    $("#report_po").on("click", function () {
        var dari = moment($("#report_po_dari").val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");
        var sampai = moment($("#report_po_sampai").val(), ["D MMMM YYYY"], "id").format("DD/MM/YYYY");

        downloadFileUsingForm("/api/report/ReportPO?dari=" + dari + "&sampai=" + sampai);
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




