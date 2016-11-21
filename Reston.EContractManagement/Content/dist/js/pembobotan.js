

$(function () {
    $('#kreteriaPembobotan').on('change', ".input-bobot-pengadaan", function () {
        if (!$.isNumeric($(this).val())) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Inputan Harus Angka dan Kurang dari 100',
                buttons: [{
                    label: 'Batal',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            $(this).val(0);
            return false;
        } else {
            if (parseInt($(this).val()) > 100) {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Inputan Harus Angka dan Kurang dari 100',
                    buttons: [{
                        label: 'Batal',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
                $(this).val(0);
                return false;
            }
        }
        addPembobotanPengadaan($(this));
    });

});

function LoadKriteriaPembobotan(PengadaanId) {
    $("#kreteriaPembobotan").html("");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/getKriteriaPembobotan?PengadaanId=" + PengadaanId,
        success: function (data) {
            $.each(data, function (index, val) {
                html = '<div class="row">' +
			        '<div class="form-group col-md-4">' +
				        '<label style="font-size:small">' + val.NamaKreteria + '</label>' +
				        '<input id="bobot-harga" attrId="' + val.Id + '" type="text" class="form-control input-bobot-pengadaan" value=' + val.Bobot + ' >' +
			        '</div>' +
		         '</div>';
                $("#kreteriaPembobotan").append(html);


            });
        }
    });
}


