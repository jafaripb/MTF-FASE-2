
$(function () {
    getNotifikasi();
});
function getNotifikasi() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/getRiwayatDokumenVendor" ,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {        
            $("#totalNotif").html(data.length);
            $("#headerNotif").html("Ada " + data.length + " Notifikasi");
            var lstHtml = "";
            for(var index in data){
                lstHtml = lstHtml + '<li>' +
                        '<a href="#">' +
                          '<h4>' +
                             data[index].JudulPengadaan +
                          '</h4>' +
                          '<p> Status Pengadaan: ' + data[index].Status + '</p>' +
                        '</a>' +
                      '</li>';
            }
            $("#listNotif").html(lstHtml);
        }
    });
   
}
