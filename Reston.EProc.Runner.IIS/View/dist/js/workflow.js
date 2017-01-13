

var tbl_user_approver;
var tbl_apporval_pemenang;

$(function () {

    $(".add-user-terkait").on("click", function () {
        $("#users_modal").modal("show");

    });
    

    tbl_apporval_pemenang = $("#tbl_apporval_pemenang").DataTable({
        "serverSide": false,
        "searching": false,
        "ajax": {
            "url": 'api/Workflow/List',
            "type": 'POST',
            "data": function (d) {
                d.Id = $("#pengadaanId").val();
            }
        },
        "columns": [
            
            { "data": "Nama" },
            { "data": "jabatan" },
            { "data": null }
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    return '<button type="button" class="btn button delete-user-pemenang" attrId="' + row.Id + '">Delete</button>';
                },

                "targets": 2,
                "orderable": false
            }
        ],
        "paging": false,
        "lengthChange": false,
        "ordering": false,
        "info": true,
        "autoWidth": false,
        "responsive": true,
    });

    tbl_user_approver = $("#tbl-user-approver").DataTable({
        "serverSide": true,
        "searching": true,
        "ajax": {
            "url": 'api/Workflow/ListUser',
            "type": 'POST',
            "data": function (d) {
                d.tipe = "approver";
            }
        },
        "columns": [
            { "data": "Nama" },
            { "data": "jabatan" },
            { "data": null }
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    var objData = JSON.stringify(data);
                    return "<button type='button' class='btn button pilih-user-pemenang' odata='" + objData + "'>Pilih</button>";
                },

                "targets": 2,
                "orderable": false
            }
        ],
        "paging": true,
        "lengthChange": false,
        "ordering": false,
        "info": true,
        "autoWidth": false,
        "responsive": true,
    });
});



$(function () {

    $("body").on("click", ".pilih-user-pemenang", function () {

        var data = $(this).attr("odata");
        var obj = jQuery.parseJSON(data);
        tbl_apporval_pemenang.row.add(obj).draw();
        console.log(obj);
        

        
    });
    $("body").on("click", ".delete-user-pemenang", function () {
        var vl = $(this).closest('tr')[0];
        

        tbl_apporval_pemenang.row(vl.rowIndex-1).remove().draw();
       
    });

    $(".pilih-user-approver-pemenang").on("click", function () {
        $("#user-approver").modal("show");
    });

    $(".save-workflow-pemenang").on("click", function () {
        var data = datatableToJson(tbl_apporval_pemenang);
        
        SaveWorkflowPemenang(data, $("#pengadaanId").val());
    });

        
});

function datatableToJson(table) {
    var data = [];
    var beforeJudul = "";
    table.rows().every(function () {
        data.push(this.data());
    });

    return data;
}


function SaveWorkflowPemenang(data, Id) {
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        method: "POST",
        url: "Api/Workflow/save?Id="+Id,
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            waitingDialog.hideloading();
            var msg = data.message;
            if (data.Id == 0) {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: msg,
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
            }
            waitingDialog.hideloading();
        }
    });
}