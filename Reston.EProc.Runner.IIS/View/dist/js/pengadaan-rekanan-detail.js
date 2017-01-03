var id_pengadaan = window.location.hash.replace("#", "");

$(function () {
    if (isGuid(id_pengadaan)) {
        $("#pengadaanId").val(id_pengadaan);
        loadData(id_pengadaan);
        loadStatus(id_pengadaan);
    }
    else {
        console.log("inguid");
        if (isGuid($("#pengadaanId").val())) {
            window.location.hash = $("#pengadaanId").val();
            loadData($("#pengadaanId").val());
            loadStatus(id_pengadaan);
        }
        else {            
            window.location.replace("http://"+window.location.host + "/pengadaan-rekanan.html");
        }
    }

    $("#tab-pelakasanaan").on("click", function () {
        $("#side-kanan").find(".rk").hide();
        $("#side-kanan").find(".pl").show();
    });
    $("#tab-berkas").on("click", function () {
        $("#side-kanan").find(".rk").hide();
        $("#side-kanan").find(".pl").hide();
    });
    $("#tab-rk").on("click", function () {
        $("#side-kanan").find(".rk").show();
        $("#side-kanan").find(".pl").hide();
    });

    $("#side-kanan").find(".pl").hide();

    $("#myNav").affix({
        offset: {
            top: 100
        }
    });
    $(".fa-download").click(function () {
        $("#view-siup").modal('show');
    });

    $(".tab-content").show();

    $("#side-kanan").on("click", "li", function () {
        $("#side-kanan").find("li").each(function () {
            $(this).removeClass("active");
        });
        $(this).addClass("active");
    });
        
    //dropzone
    var myDropzoneBerkasRujukanLain = new Dropzone("#BerkasRujukanLain",
            {
                url: $("#BerkasRujukanLain").attr("action") + "&id=" + $("#pengadaanId").val(),
                maxFilesize: 5,
                acceptedFiles: "",
                clickable: false,
                dictDefaultMessage: "Tidak Ada Dokumen",
				init: function () {
                    this.on("addedfile", function (file) {
                        file.previewElement.addEventListener("click", function () {
                            var id = 0;
                            if (file.Id != undefined)
                                id = file.Id;
                            else
                                id = $.parseJSON(file.xhr.response);
                            console.log(id);
							$("#HapusFile").hide();
                            $("#konfirmasiFile").attr("attr1", "BerkasRujukanLain");
                            $("#konfirmasiFile").attr("FileId", id);
                            $("#konfirmasiFile").modal("show");
                        });
                    });
                }
            }
        );
    renderDokumenDropzone(myDropzoneBerkasRujukanLain, "BerkasRujukanLain");
    Dropzone.options.BerkasRujukanLain = false;

    var myDropzoneBerkasRekanan = new Dropzone("#BerkasRekanan",
            {
                url: $("#BerkasRekanan").attr("action") + "&id=" + $("#pengadaanId").val(),
                maxFilesize: 5,
                acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.docx,.xlsx",
                dictDefaultMessage: "Tidak Ada Dokumen",
                init: function () {
                    this.on("addedfile", function (file) {
                        file.previewElement.addEventListener("click", function () {
                            var id = 0;
                            if (file.Id != undefined)
                                id = file.Id;
                            else
                                id = $.parseJSON(file.xhr.response);
                            console.log(id);
                            $("#konfirmasiFile").attr("attr1", "BerkasRekanan");
                            $("#konfirmasiFile").attr("FileId", id);
                            $("#konfirmasiFile").modal("show");
                        });
                    });
                    //this.on("removedfile", function (file, responseText) {
                    //    myDropzoneBerkasRekanan.removeFile(file);
                    //});
                    this.on("success", function (file, responseText) {
                        if (responseText == 0) {
                            myDropzoneBerkasRekanan.removeFile(file);
                            BootstrapDialog.show({
                                title: 'Konfirmasi',
                                message: 'Waktu Penawaran Sudah Habis',
                                buttons: [{
                                    label: 'Close',
                                    action: function (dialog) {
                                        dialog.close();
                                    }
                                }]
                            });
                        }
                    });
                }
            }
        );
    renderDokumenDropzone(myDropzoneBerkasRekanan, "BerkasRekanan");
    Dropzone.options.BerkasRekanan = false;

    var myDropzoneBerkasRekananKlarifikasi = new Dropzone("#BerkasRekananKlarifikasi",
            {
                url: $("#BerkasRekananKlarifikasi").attr("action") + "&id=" + $("#pengadaanId").val(),
                maxFilesize: 5,
                acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.docx,.xlsx",
                dictDefaultMessage: "Tidak Ada Dokumen",
                init: function () {
                    this.on("addedfile", function (file) {
                        file.previewElement.addEventListener("click", function () {
                            var id = 0;
                            if (file.Id != undefined)
                                id = file.Id;
                            else
                                id = $.parseJSON(file.xhr.response);
                            console.log(id);
                            $("#konfirmasiFile").attr("attr1", "BerkasRekananKlarifikasi");
                            $("#konfirmasiFile").attr("FileId", id);
                            $("#konfirmasiFile").modal("show");
                        });
                    });
                    this.on("success", function (file, responseText) {
                        if (responseText == 0) {
                            myDropzoneBerkasRekananKlarifikasi.removeFile(file);
                            BootstrapDialog.show({
                                title: 'Konfirmasi',
                                message: 'Waktu Penawaran Sudah Habis',
                                buttons: [{
                                    label: 'Close',
                                    action: function (dialog) {
                                        myDropzoneBerkasRekananKlarifikasi.removeFile(file);
                                        dialog.close();
                                    }
                                }]
                            });
                        }
                    });
                }
            }
        );
    renderDokumenDropzone(myDropzoneBerkasRekananKlarifikasi, "BerkasRekananKlarifikasi");
    Dropzone.options.BerkasRekananKlarifikasi = false;

    var myDropzoneBerkasRekananKlarifikasiLanjutan = new Dropzone("#BerkasRekananKlarifikasiLanjutan",
          {
              url: $("#BerkasRekananKlarifikasiLanjutan").attr("action") + "&id=" + $("#pengadaanId").val(),
              maxFilesize: 5,
              acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.docx,.xlsx",
              dictDefaultMessage: "Tidak Ada Dokumen",
              init: function () {
                  this.on("addedfile", function (file) {
                      file.previewElement.addEventListener("click", function () {
                          var id = 0;
                          if (file.Id != undefined)
                              id = file.Id;
                          else
                              id = $.parseJSON(file.xhr.response);

                          $("#konfirmasiFile").attr("attr1", "BerkasRekananKlarifikasiLanjutan");
                          $("#konfirmasiFile").attr("FileId", id);
                          $("#konfirmasiFile").modal("show");
                      });
                  });
                  this.on("success", function (file, responseText) {
                      if (responseText == 0) {
                          myDropzoneBerkasRekananKlarifikasiLanjutan.removeFile(file);
                          BootstrapDialog.show({
                              title: 'Konfirmasi',
                              message: 'Waktu Penawaran Sudah Habis',
                              buttons: [{
                                  label: 'Close',
                                  action: function (dialog) {
                                      myDropzoneBerkasRekananKlarifikasiLanjutan.removeFile(file);
                                      dialog.close();
                                  }
                              }]
                          });
                      }
                  });
              }
          }
      );
    renderDokumenDropzone(myDropzoneBerkasRekananKlarifikasiLanjutan, "BerkasRekananKlarifikasiLanjutan");
    Dropzone.options.BerkasRekananKlarifikasiLanjutan = false;
   
    $("#penawaran").on("click", function () {

        window.open("http://" + window.location.host + "/rekanan-rks.html#" + $("#pengadaanId").val());
    });

    $("#penawaran-klarifikasi").on("click", function () {
        window.open("http://" + window.location.host + "/rekanan-klarifikasi-harga.html#" + $("#pengadaanId").val());
    });
    $("#penawaran-klarifikasi-lanjutan").on("click", function () {
        window.open("http://" + window.location.host + "/rekanan-klarifikasi-lanjutan-harga.html#" + $("#pengadaanId").val());
    });

    $("#downloadFile").on("click", function () {
        var tipe = $(this).parent().parent().parent().parent().attr("attr1");
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");
        downloadFileUsingForm("/api/pengadaane/OpenFile?Id=" + FileId);
        $("#konfirmasiFile").modal("hide");
    });

    $("#HapusFile").on("click", function () {
        var tipe = $(this).parent().parent().parent().parent().attr("attr1");
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");
        $.ajax({
            method: "POST",
            url: "Api/VendorAction/deleteDokumenPelaksanaan?Id=" + FileId
        }).done(function (data) {
            if (data.Id == "1") {
                if (tipe == "BerkasRekanan") {
                    $.each(myDropzoneBerkasRekanan.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzoneBerkasRekanan.removeFile(item);
                        }
                    });
                }
            }
            if (data.Id == "1") {
                if (tipe == "BerkasRekananKlarifikasi") {
                    $.each(myDropzoneBerkasRekananKlarifikasi.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzoneBerkasRekananKlarifikasi.removeFile(item);
                        }
                    });
                }
            }
            $("#konfirmasiFile").modal("hide");
        });
    });
});

function loadData(pengadaanId) {
    $.ajax({
        method: "POST",
        url: "Api/VendorAction/detailPengadaanForRekanan?Id=" + pengadaanId,
        dataType: "json"
    }).done(function (data) {
        $("#judul").text(data.Judul);
        $("#deskripsi").text( data.AturanPengadaan + ", " + data.AturanBerkas + ", " + data.AturanPenawaran);        
        $("#keterangan").text(data.Keterangan);
        $("#MataUang").text(data.MataUang);
        $("#UnitKerjaPemohon").text(data.UnitKerjaPemohon);
        $("#Region").text(data.Region);
        $("#Provinsi").text(data.Provinsi);
        $("#JenisPekerjaan").text(data.JenisPekerjaan);
        $("#pengadaanId").val(data.Id);
        hitungTawaranRekanan($("#pengadaanId").val(), data.AturanPenawaran);
        loadJadwal(data.JadwalPengadaans);
        loadKualifikas(data.KualifikasiKandidats);
        if (data.Status<7 && data.Status>=4) {
            //$("#Status").text("Submit Penawaran");
            $("#tab-penawaran-rekanan").attr("data-toggle", "collapse");
            //$("#Status").text("Submit Penawaran");
            $("#collapseOne").addClass("in");
        }

       

        if (data.Status == 7) {
            //$("#Status").text("Submit Penawaran");
            $("#tab-klarifikasi-rekanan").attr("data-toggle", "collapse");
            $("#tab-penawaran-rekanan").attr("data-toggle", "collapse");
            //$("#Status").text("Submit Penawaran");
            $("#collapseTwo").addClass("in");
        }

        if (data.Status == 12) {
            //$("#Status").text("Submit Penawaran");
            $("#tab-klarifikasi-rekanan").attr("data-toggle", "collapse");
            $("#tab-penawaran-rekanan").attr("data-toggle", "collapse");
            $("#tab-klarifikasi-lanjutan-rekanan").attr("data-toggle", "collapse");
           
            $("#panel-klarifikasi-lanjutan").addClass("in");
        }

       // if (data.AturanPenawaran == "Price Matching") {
            $("#total_penawaran").attr("disabled", "disabled");
            $("#row_penawaran_open_price").remove();           
            $("#total_penawaran-klarifikasi").attr("disabled", "disabled");
        //}
        //if (data.AturanPenawaran == "Open Price") {
        //    $("#total_penawaran").removeAttr("disabled");
        //    $("#row_penawaran").remove();
        //    $("#total_penawaran-klarifikasi").removeAttr("disabled");
        //    $("#penawaran-klarifikasi").attr("disabled", "disabled");
        //}
      
    });
    getDateSubmitPenawaran();
    getKlarifikasi();
}

function loadStatus(pengadaanId) {
    $.ajax({
        method: "POST",
        url: "Api/VendorAction/statusVendor?Id=" + pengadaanId,
        dataType: "json"
    }).done(function (data) {
        $("#Status").html("Status: " + data);

    });
}

function renderDokumenDropzone(myDropzone, tipe) {
    $.ajax({
        url: "Api/VendorAction/getDokumens?Id=" + $("#pengadaanId").val() + "&tipe=" + tipe,
        success: function (data) {
            for (var key in data) {
                var file = {
                    Id: data[key].Id, name: data[key].File, accepted: true,
                    status: Dropzone.SUCCESS, processing: true, size: data[key].SizeFile
                };
                //thisDropzone.options.addedfile.call(thisDropzone, file);
                myDropzone.emit("addedfile", file);
                myDropzone.emit("complete", file);
                myDropzone.files.push(file);
            }
        },
        error: function (errormessage) {
        }
    });
}

function hitungTawaranRekanan(pengadaanId,aturanPenawaran) {
   // if (aturanPenawaran == "Price Matching") {
        $.ajax({
            url: "Api/VendorAction/getRksRekanan?id=" + pengadaanId,
        }).done(function (data) {
            var rksdetail = data.data;
            var total = 0;
            for (var key in rksdetail) {

                //if (rksdetail[key].jumlah != null && rksdetail[key].jumlah != "" && rksdetail[key].harga != null && rksdetail[key].harga != "") {
                if (rksdetail[key].harga > 0 && rksdetail[key].jumlah > 0) {

                    var jumlah = rksdetail[key].jumlah;
                    var harga = rksdetail[key].harga;
                    var totalPerItem = jumlah * harga;
                    total = total + totalPerItem;
                }
            }
            $("#total_penawaran").val(accounting.formatNumber(total, { thousand: ".", decimal: ",", precision: 2 }));
        });
        $.ajax({
            url: "Api/VendorAction/getRKSForKlarifikasiRekanan?id=" + pengadaanId,
        }).done(function (data) {
            var rksdetail = data.data;
            var total = 0;
            for (var key in rksdetail) {
                if (rksdetail[key].harga > 0 && rksdetail[key].jumlah > 0) {
                    var jumlah = rksdetail[key].jumlah;
                    var harga = rksdetail[key].harga;
                    var totalPerItem = jumlah * harga;
                    total = total + totalPerItem;
                }
            }
            $("#total_penawaran-klarifikasi").val(accounting.formatNumber(total, { thousand: ".", decimal: ",", precision: 2 }));
        });
        $.ajax({
            url: "Api/VendorAction/getRKSForKlarifikasiLanjutanRekanan?Id=" + pengadaanId,
        }).done(function (data) {
            var rksdetail = data.data;
            var total = 0;
            for (var key in rksdetail) {
                if (rksdetail[key].harga > 0 && rksdetail[key].jumlah > 0) {
                    var jumlah = rksdetail[key].jumlah;
                    var harga = rksdetail[key].harga;
                    var totalPerItem = jumlah * harga;
                    total = total + totalPerItem;
                }
            }
            $("#total-penawaran-klarifikasi-lanjutan").val(accounting.formatNumber(total, { thousand: ".", decimal: ",", precision: 2 }));
        });
   // }
}



function loadKualifikas(kualifikasiKandidat) {
    $(".checkbox-kualifikasi").removeAttr("checked");
    $.each(kualifikasiKandidat, function (index, value) {
        $(".checkbox-kualifikasi[value='" + value.kualifikasi + "']").prop("checked", "true");
        $(".checkbox-kualifikasi[value='" + value.kualifikasi + "']").attr("attrId", value.Id);
    });
}

function loadJadwal(data) {   
        for (var i in data) {
            var tgl = "";
            var dateMulai;
            var dateSampai;
            if (data[i].Mulai != null && moment(data[i].Mulai).format("DD/MM/YYYY") != "Invalid date") {
                tgl = tgl + moment(data[i].Mulai).format("DD/MM/YYYY");
            }
            if (data[i].Sampai != null && moment(data[i].Sampai).format("DD/MM/YYYY") != "Invalid date") {
                tgl = tgl + " s/d " + moment(data[i].Sampai).format("DD/MM/YYYY");
            }
            if (data[i].tipe == "Aanwijzing") {
                if (data[i].Mulai != null)
                    $("#Aanwijzing").text(tgl);
            }
            if (data[i].tipe == "pengisian_harga") {
                $("#PengisianHarga").text(tgl);
            }
            if (data[i].tipe == "buka_amplop") {
                $("#BukaAmplop").text(tgl);
            }
            if (data[i].tipe == "penilaian") {
                $("#penilaian").text(tgl);
            }
            if (data[i].tipe == "klarifikasi") {
                $("#Klarifikasi").text(tgl);
            }
            if (data[i].tipe == "penentuan_pemenang") {
                $("#PenentuanPemenang").text(tgl);
            }
        }
}

function getDateSubmitPenawaran() {
    $.ajax({
        method: "POST",
        url: "Api/VendorAction/GetSubmitPenawran?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $("#pengisian_harga_aktual").html("( " + moment(data.Mulai).format("DD MMMM YYYY HH:mm") + " s/d " + moment(data.Sampai).format("DD MMMM YYYY HH:mm") + " )");

        },
        error: function (errormessage) {
        }
    });
}

function getKlarifikasi() {
    $.ajax({
        method: "POST",
        url: "Api/VendorAction/GetKlarifikasi?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $("#klarifikasi_aktual").html("( " + moment(data.Mulai).format("DD MMMM YYYY HH:mm") + " s/d " + moment(data.Sampai).format("DD MMMM YYYY HH:mm") + " )");
        },
        error: function (errormessage) {
        }
    });
}


