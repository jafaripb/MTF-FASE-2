var id_rks = window.location.hash.replace("#", "");
var status = "";
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
var dataHps;
$.ajax({
    url: "Api/Produk/GetAllProduk"
}).done(function (data) {
    dataHps = data.aaData;
});
$(function () {

    //loadData();
    loadDataRks();
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
    table = $('#example1').DataTable({
        "paging": false,
        "lengthChange": false,
        "searching": false,
        "ordering": false,
        "info": false,
        "autoWidth": true,
        //responsive: true,
        "ajax": "Api/PengadaanE/getRks?id=" + id_rks,
        //"ajax": "data/rks2.txt",
        "columns": [
            { "data": "item" },
            { "data": "satuan" },
            { "data": "jumlah", "className": "tengah" },
            { "data": "hps", "className": "rata_kanan" },
            { "data": "total", "className": "rata_kanan" }
        ],
        "columnDefs": [
                {
                    "render": function (data, type, row) {
                        
                        if (row.hps != null ) {
                            var str ="'"+ row.hps+"'";
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
                        if (row.hps == "" || row.jumlah == "" || row.jumlah == null || row.hps==null)
                            return "";
                        else
                            return accounting.formatNumber(row.hps*row.jumlah, { thousand: "." });
                    },
                    "targets": 4,
                    "orderable": false
                }
        ],
        "rowCallback": function (row, data, index) {
            if (status == 1) {
                var column = table.column(5);
                column.visible(false);//!column.visible()
            }
        }

    });

   

   
    if (isGuid(id_rks)) {
        $("#pengadaanId").val(id_rks);
        loadData(id_rks);
    }
    else {
        console.log("inguid");
        if (isGuid($("#pengadaanId").val())) {
            window.location.hash = $("#pengadaanId").val();
            loadData($("#pengadaanId").val());
        }
        else {

            window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
            //$(location).attr('href', window.location.origin + "pengadaan-list.html");
        }
    }
});
function loadData() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/detailPengadaan?Id=" + id_rks,
        dataType: "json"
    }).done(function (data) {
        $("#judul").text(data.Judul);
        $("#judul-page").html("RKS " + data.Judul);
        $("#pengadaanId").val(data.Id);
    });
}

//console.log(id_rks);