
$(function () {
    $(".addPerson").on("click", function () {
        $("#title-modal").html("Daftar Personil")
        $("#tipe-person-list").val($(this).attr("attr1"));
        $("#personilModal").modal("show");
        $(".item-bg-blue-light").each(function () {
            $(this).removeClass("item-bg-blue-light");
        });
    });
  
});

app.controller('PersonCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.persons = [];
    //$http.get(LOGIN_PAGE + "admin/ListUser?start=0&limit=5&name=")
    $http.get("api/pengadaane/getUsers?start=0&limit=5&name=")
       .then(function (response) {
           $scope.persons = response.data.Users;
       });
    $scope.getPerson = function ($event, person) {
        if ($($event.currentTarget).hasClass("item-bg-blue-light")) {
            $($event.currentTarget).removeClass("item-bg-blue-light");
        }
        else {
            $($event.currentTarget).addClass("item-bg-blue-light");
            addPersonil(person, $("#tipe-person-list").val());
        }
    }
    $scope.searchPerson = function () {
        $http.get("api/pengadaane/getUsers?start=0&limit=5&name=" + $("#search").val())
          .then(function (response) {
              $scope.persons = response.data.Users;
              if (jum >= response.data.totalRecord) $("#pop-personil-next").html("");
              else $("#pop-personil-next").html("Berikutnya");
          });
    }
    $scope.nextPerson = function () {
        var jum = $(".pop-personil-list").find("li").length;
        $http.get("api/pengadaane/getUsers?start=" + jum + "&limit=5&name=" + $("#search").val())
          .then(function (response) {

              $.each(response.data.Users, function (index, value) {
                  $scope.persons.push(value);
                  jum = jum + 1;
              });
              if (jum >= response.data.totalRecord) $("#pop-personil-next").html("");

          });
    }
}]);



function addPersonil(item, el) {
    var peran = el.replace(".listperson-", "");
    var objPersonilPengadaan = {};
    objPersonilPengadaan.PersonilId = item.PersonilId;
    objPersonilPengadaan.tipe = peran;
    objPersonilPengadaan.Nama = item.Nama;
    objPersonilPengadaan.Jabatan = item.Jabatan;
    objPersonilPengadaan.PengadaanId = $("#pengadaanId").val();

    if (peran == "pic" && $(el).children().length > 0) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'PIC Sudah Ada!',
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        return false;
    }
    if (isPersonileEksisPerPeran(item.PersonilId, el) == 1) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'User yang Ingin ditambahkan Sudah Ada!',
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        return false;
    }

    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/savePersonil",
        dataType: "json",
        data: JSON.stringify(objPersonilPengadaan),
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.status == 200) {
            html = '<a class="btn btn-app">' +
                    '<input type="hidden" class="list-personil" attrId="'
                        + data.Id + '" attr1="' + peran + '" attr2="' + item.Nama + '" attr3="'
                        + item.Jabatan + '" value="' + item.PersonilId + '" />' +
                    '<span class="badge bg-red remove-person"><i class="fa fa-remove"></i></span>' +
                    '<i class="fa fa-user"></i>' +
                    item.Nama +
                  '</a>';
            $(el).append(html);
        }
        else {
            alert("error");
        }
    });

}
