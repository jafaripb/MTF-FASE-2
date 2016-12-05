var id_pengadaan = window.location.hash.replace("#", "");

$(function () {
    if (isGuid(id_pengadaan)) {
        $("#pengadaanId").val(id_pengadaan);
        loadData(id_pengadaan);
    }
    else {
        if (isGuid($("#pengadaanId").val())) {
            window.location.hash = $("#pengadaanId").val();
            loadData($("#pengadaanId").val());
        }
        else {
            
            window.location.replace("http://"+window.location.host + "/pengadaan-list.html");
            //$(location).attr('href', window.location.origin + "pengadaan-list.html");
        }
    }

    $(".box-folder-vendor").on("click", function () {
        window.location.replace("http://localhost:49559/rekanan-detail.html");
    });
    
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
    //$(".fa-download").click(function () {
    //    $("#view-siup").modal('show');
    //});

    $(".download-berkas").on("click", function () {
        if ($(this).attr("attr1") == "berkas-aanwijzing")
            downloadFileUsingForm("/api/report/BerkasAanwzjing?Id=" + $("#pengadaanId").val());
        if ($(this).attr("attr1") == "berkas-buka-amplop")
            downloadFileUsingForm("/api/report/BerkasBukaAmplop?Id=" + $("#pengadaanId").val());
        if ($(this).attr("attr1") == "berkas-klarifikasi")
            downloadFileUsingForm("/api/report/BerkasKlarfikasi?Id=" + $("#pengadaanId").val());
        if ($(this).attr("attr1") == "berkas-pemenang")
            downloadFileUsingForm("/api/report/BerkasPemenang?Id=" + $("#pengadaanId").val());
        if ($(this).attr("attr1") == "daftar-hadir")
            downloadFileUsingForm("/api/report/BerkasDaftarRapat?Id=" + $("#pengadaanId").val());
        if ($(this).attr("attr1") == "lembar-disposisi")
            downloadFileUsingForm("/api/report/LembarDisposisi?Id=" + $("#pengadaanId").val());
        if ($(this).attr("attr1") == "berkas-penilaian")
            downloadFileUsingForm("/api/report/BerkasPenilaian?Id=" + $("#pengadaanId").val());
        if ($(this).attr("attr1") == "lembar-spk")
            downloadFileUsingForm("/api/report/BerkasSPK?Id=" + $("#pengadaanId").val());
    });
   
    

    $(".tab-content").show();

    $("#side-kanan").on("click", "li", function () {
        $("#side-kanan").find("li").each(function () {
            $(this).removeClass("active");
        });
        $(this).addClass("active");
    });
    $(".klarifikasi").on("click", function () {
        if (!confirm("Yakin vendor ini dieleminasi?"))
            return false;
        $(this).parent().remove();
    });
    $(".penentuan-pemenang").on("click", function () {
        if (!confirm("Yakin vendor ini sebagai pemenang?"))
            return false;
        var conten = $(this).parent().html();
        $(".pemenang").html(conten);
    });
        
    //dropzone
    var myDropzoneDokumenNotaInternal = new Dropzone("#DokumenNotaInternal",
            {
                url: $("#DokumenNotaInternal").attr("action") + "&id=" + $("#pengadaanId").val(),
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
                            $("#konfirmasiFile").attr("attr1", "DokumenNotaInternal");
                            $("#konfirmasiFile").attr("FileId", id);
                            $("#konfirmasiFile").modal("show");
                        });
                    });
                }
            }
        );
    renderDokumenDropzone(myDropzoneDokumenNotaInternal, "NOTA");
    Dropzone.options.DokumenNotaInternal = false;

    //dropzone
    var myDropzoneBerkasDokumenNotaInternal = new Dropzone("#BerkasDokumenNotaInternal",
            {
                url: $("#DokumenNotaInternal").attr("action") + "&id=" + $("#pengadaanId").val(),
                maxFilesize: 5,
                acceptedFiles: "",
                clickable: false,
                dictDefaultMessage: "Tidak Ada Dokumen",
                init: function () {
                    this.on("addedfile", function (file) {
                        file.previewElement.addEventListener("click", function () {
                            var id = 0;
                            if (file.Id != undefined)
                                id = file.Id
                            else
                                id = $.parseJSON(file.xhr.response)
                            //viewFile(data.Id);
                            $("#konfirmasiFile").attr("attr1", "DokumenNotaInternal");
                            $("#konfirmasiFile").attr("FileId", id);
                            $("#konfirmasiFile").modal("show");
                        });
                    });
                }
            }
        );
    renderDokumenDropzone(myDropzoneBerkasDokumenNotaInternal, "NOTA");
    Dropzone.options.BerkasDokumenNotaInternal = false;

    var myDropzoneDokumenLain = new Dropzone("#DokumenLain",
           {
               url: $("#DokumenLain").attr("action") + "&id=" + $("#pengadaanId").val(),
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
                           $("#HapusFile").hide();
                           $("#konfirmasiFile").attr("attr1", "DokumenLain");
                           $("#konfirmasiFile").attr("FileId", id);
                           $("#konfirmasiFile").modal("show");
                       });
                   });
               }
           }
       );
    renderDokumenDropzone(myDropzoneDokumenLain, "DOKUMENLAIN");
    Dropzone.options.DokumenLain = false;

    var myDropzoneBerkasDokumenLain = new Dropzone("#BerkasDokumenLain",
           {
               url: $("#DokumenLain").attr("action") + "&id=" + $("#pengadaanId").val(),
               maxFilesize: 5,
               acceptedFiles: "",
               clickable: false,
               dictDefaultMessage: "Tidak Ada Dokumen",
               init: function () {
                   this.on("addedfile", function (file) {
                       file.previewElement.addEventListener("click", function () {
                           var id = 0;
                           if (file.Id != undefined)
                               id = file.Id
                           else
                               id = $.parseJSON(file.xhr.response)
                           //viewFile(data.Id);
                           $("#HapusFile").hide();
                           $("#konfirmasiFile").attr("attr1", "DOKUMENLAIN");
                           $("#konfirmasiFile").attr("FileId", id);
                           $("#konfirmasiFile").modal("show");
                       });
                   });
               }
           }
       );
    renderDokumenDropzone(myDropzoneBerkasDokumenLain, "DokumenLain");
    Dropzone.options.BerkasDokumenLain = false;

    var myDropzoneBerkasRujukanLain = new Dropzone("#BerkasRujukanLain",
            {
                url: $("#BerkasRujukanLain").attr("action") + "&id=" + $("#pengadaanId").val(),
                maxFilesize: 5,
                acceptedFiles: "",
                dictDefaultMessage: "Tidak Ada Dokumen",
                clickable: false,
                init: function () {
                    this.on("addedfile", function (file) {
                        file.previewElement.addEventListener("click", function () {
                            var id = 0;
                            if (file.Id != undefined)
                                id = file.Id;
                            else
                                id = $.parseJSON(file.xhr.response);
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

    var myDropzoneBerkasBerkasRujukanLain = new Dropzone("#BerkasBerkasRujukanLain",
        {
            url: $("#BerkasBerkasRujukanLain").attr("action") + "&id=" + $("#pengadaanId").val(),
            maxFilesize: 5,
            acceptedFiles: "",
            dictDefaultMessage: "Tidak Ada Dokumen",
            clickable: false,
            init: function () {
                this.on("addedfile", function (file) {
                    file.previewElement.addEventListener("click", function () {
                        var id = 0;
                        if (file.Id != undefined)
                            id = file.Id
                        else
                            id = $.parseJSON(file.xhr.response)
                        //viewFile(data.Id);
                        $("#konfirmasiFile").attr("attr1", "BerkasRujukanLain");
                        $("#konfirmasiFile").attr("FileId", id);
                        $("#konfirmasiFile").modal("show");
                    });
                });
            }
        }
    );
    renderDokumenDropzone(myDropzoneBerkasBerkasRujukanLain, "BerkasRujukanLain");
    Dropzone.options.BerkasBerkasRujukanLain = false;
    
    $("#ajukan").on("click", function () {
        if ($("[name=TitleDokumenNotaInternal]").val() == "" || myDropzoneDokumenNotaInternal.files.length == 0) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Dokumen Nota Internal Wajib Diisi!',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            return false;
        }
        if ($("#AturanPengadaan").val().trim() != "Pengadaan Terbuka") {
            if ($("div.listkandidat .col-md-3").length == 0) {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Kandidat Wajib diisi!',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
                return false;
            }
        }
        var cek = 1;
        
            $.each($(".jadwal"), function (index, el) {
                if ($(el).hasClass("jadwal-pendaftaran-terbuka")) {
                    if ($("#AturanPengadaan").val().trim() != "Pengadaan Tertutup") {
                        if ($(el).text() == "") {
                            BootstrapDialog.show({
                                title: 'Konfirmasi',
                                message: 'Semua Jadwal Wajib diisi!',
                                buttons: [{
                                    label: 'Close',
                                    action: function (dialog) {
                                        dialog.close();
                                    }
                                }]
                            });
                            cek = 0;
                            return false;
                        }
                    }
                }
                else {
                    if ($(el).text() == "") {
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Semua Jadwal Wajib diisi!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    dialog.close();
                                }
                            }]
                        });
                        cek = 0;
                        return false;
                    }
                }
            });
        

        if (cek == 0) return false;
        if ($('.ready-checkbox').not(':checked').length > 0) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Semua Personil Wajib Menceklis Kesiapan',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            return false;
        }

        if ($("div.listperson-pic .btn-app").length == 0) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'PIC Wajib diisi!',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            return false;
        }

        if ($("div.listperson-tim .btn-app").length == 0) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Tim Wajib diisi!',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            return false;
        }

        if ($("div.listperson-staff .btn-app").length == 0) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'User Wajib diisi!',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            return false;
        }

        if ($("div.listperson-controller .btn-app").length == 0) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Controller Wajib diisi!',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            return false;
        }

        if ($("div.listperson-compliance .btn-app").length == 0) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Compliance Wajib diisi!',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            return false;
        }
        $("#modal-persetujuan").modal("show");
    });
    $("#anjukan-lanjutkan").on("click", function () {
        $("#modal-persetujuan").modal("hide");
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            method:"POST",
            url: "Api/PengadaanE/ajukan?Id=" + $("#pengadaanId").val(),
            success: function (response) {
                window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
                waitingDialog.hideloading();
            },
            error: function (errormessage) {
                waitingDialog.hideloading();
            }
        });
    });

    $("#edit").on("click", function () {
        window.location.replace("http://" + window.location.host + "/pengadaan-add.html#" + $("#pengadaanId").val());
    });
    $(".Setujui").on("click", function () {
        $("#modal-setujui").modal("show");
    });
    $(".Tolak").on("click", function () {
        $("#modal-ditolak").modal("show");
    });
    $("#lanjut-tolak").on("click", function () {
        if ($.trim($("#keterangan_penolakan").val()) == "") {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Alasan Penolakan Harus DiIsi',
                buttons: [
                        {
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
            });
            return false;
        }

        waitingDialog.showloading("Proses Harap Tunggu");
        var objData = {};
        objData.PenolakanId=$("#pengadaanId").val();
        objData.AlasanPenolakan = $("#keterangan_penolakan").val();

        $.ajax({
            method: "POST",
            dataType: "json",
            data: JSON.stringify(objData),
            contentType: 'application/json; charset=utf-8',
            url: "Api/PengadaanE/tolakPengadaan" ,
            success: function (data) {
                if (data.status == 200) {
                    window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
                }
                waitingDialog.hideloading();
            },
            error: function (errormessage) {
                //alert("gagal");
                waitingDialog.hideloading();
                $("#modal-ditolak").modal("hide");
            }
        });

    });

    $("#lanjut-setujui-aja").on("click", function () {
        $("#modal-setujui").modal("hide");
         waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/persetujuan?Id=" + $("#pengadaanId").val(),
            success: function (data) {
                if (data.status == 200) {
                    window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
                }
                waitingDialog.hideloading();
                //el.show();
            },
            error: function (errormessage) {
                // waitingDialog.hideloading();
                //el.show();
            }
        });
    });
    $("body").on("click", ".box-rekanan", function () {
        var id = $(this).attr("vendorId");
        var pengadaanId = $("#pengadaanId").val();
        BootstrapDialog.show({
            title: 'Konfirmasi',
            buttons: [{
                label: 'Lihat Informasi Rekanan',
                action: function (dialog) {
                    window.open("http://" + window.location.host + "/rekanan-detail.html?id=" + id);
                    dialog.close();
                }
            }, {
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
    });

    $("body").on("click", ".ready-checkbox", function () {
        var sendCheck = 0;
        if ($(this).is(':checked')) {
            sendCheck = 1;
        }
        var _this = $(this);
        $.ajax({
            url: "Api/PengadaanE/SaveReadyPersonil?Id=" + $("#pengadaanId").val() + "&ready=" + sendCheck,
            method: "POST"
        }).done(function (data) {
            if ((data.Id == null || data.Id == "") && sendCheck == 1) {
                _this.prop('checked', false);
            }
            if ((data.Id == null || data.Id == "") && sendCheck == 0) {
                _this.prop('checked', true);
            }
        });
    });
});

function loadData(pengadaanId) {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/detailPengadaan?Id=" + pengadaanId,
        dataType: "json"
    }).done(function (data) {        
        $("#judul").text(data.Judul);
        $("#deskripsi").text((data.NoPengadaan == null ? "" : (data.NoPengadaan + ", ")) + data.AturanPengadaan + ", " + data.AturanBerkas + ", " + data.AturanPenawaran);
        if (data.AturanPengadaan == "Pengadaan Terbuka") $("#jadwal_pendaftaran").show();
        $("#AturanPengadaan").val(data.AturanPengadaan);
        $("#keterangan").text(data.Keterangan);
        $("#MataUang").text(data.MataUang);
        $("#PeriodeAnggaran").text(data.PeriodeAnggaran);
        $("#JenisPembelanjaan").text(data.JenisPembelanjaan);
        $("#UnitKerjaPemohon").text(data.UnitKerjaPemohon);
        $("#Region").text(data.Region);
        $("#Provinsi").text(data.Provinsi);
        $("#KualifikasiRekan").text(data.KualifikasiRekan);
        $("#JenisPekerjaan").text(data.JenisPekerjaan);
        $("#TitleDokumenNotaInternal").text(data.TitleDokumenNotaInternal);

        $("#TitleDokumenLain").text(data.TitleDokumenLain);
        $("#TitleBerkasRujukanLain").text(data.TitleBerkasRujukanLain);
        $("#pengadaanId").val(data.Id);
        $("#isPIC").val(data.isPIC);
        $("#isTEAM").val(data.isTEAM);
        $("#isPersonil").val(data.isPersonil);
        $("#isPersonil").val(data.isPersonil);
        $("#State").val(data.Status);
        isPemenangApproved(pengadaanId);
        StatusPemenang(pengadaanId);
        if (data.isPIC == 0) {
            $(".action-pelaksanaan").attr("disabled", "disabled"); 
            $("button.action-pelaksanaan").remove();
        }

        if (data.isPIC == 1) {
            //$(".addPerson").show();
        }

        if (data.GroupPengadaan == 3) {
            $(".action-pelaksanaan").remove();
            $(".dateJadwal").remove();
            $("textarea").attr("disabled", "disabled");
            $("input").attr("disabled", "disabled");
            $("#collapseOne").addClass("in");
            $("#tab-anwijzing").attr("data-toggle", "collapse");
            $("#tab-submit-penawaran").attr("data-toggle", "collapse");
            $("#tab-buka-amplop").attr("data-toggle", "collapse");
            $("#tab-penilaian-kandidat").attr("data-toggle", "collapse");
            $("#tab-klarifikasi").attr("data-toggle", "collapse");
            $("#tab-penentu-pemenang").attr("data-toggle", "collapse");
            $(".delete-klarifikasi-kandidat").hide();
            $("#HapusFile").hide();
            $("input[type='checkbox']").attr('disabled', 'disabled')

        }

        loadListKandidat(data.Id);
        hitungHPS($("#pengadaanId").val(), data.AturanPenawaran);
        loadJadwal(data.JadwalPengadaans);
        LoadListPersonil(data.PersonilPengadaans, data.isPIC);
        loadKualifikas(data.KualifikasiKandidats);
        $("#lihatHps").attr("href", "rks.html#" + data.Id);
        if (data.AturanPenawaran == "Open Price") $("#lihatHps").remove();
        ///generateUndangan(data);
        if (data.Status == 0) {
            $("#Status").text("Status Pengadaan : Draft");
            if (data.isTEAM == 1 || data.isCreated==1) {
                $("#edit").show();
				if(data.isPIC==1)
					$("#ajukan").show();
            }
            $("#tab-pelakasanaan").hide();
            $("#tab-berkas").hide();
        }
        if (data.isTEAM == 0) {
            $("#HapusFile").remove();
        }

        if (data.GroupPengadaan == 3) {
            $("#arsipkan").remove();
            $("#dibatalkan").remove();
        }

        if (data.isUser == 1 && data.isTEAM==0) {
            $("#tab-klarifikasi").parent().parent().parent().remove();
           // $("#collapse5").remove();
        }

        if (data.Status ==10) {
            $("#Status").text("Status Pengadaan : Pengajuan Ditolak");
            if (data.isTEAM == 1 || data.isCreated == 1) {
                $("#edit").show();
                if (data.isPIC == 1)
                    $("#ajukan").show();
            }
            $("#tab-pelakasanaan").hide();
            $("#tab-berkas").hide();
            $("#ajukan").remove();
            loadKeteranganDitolak(data.Id);
        }
        if (data.Status == 1) {
            $("#Status").text("Status Pengadaan : Dalam Pengajuan");
            $("#tab-pelakasanaan").hide();
            $("#tab-berkas").hide();
        }
        if (data.Status > 1 && data.Status != 10 ) {
            $("#tab-rk").attr("data-toggle", "tab");
            $("#tab-pelakasanaan").attr("data-toggle", "tab");//aria-expanded="true"
            $("#tab-berkas").attr("data-toggle", "tab");
            $("#tab-pelakasanaan").show();
            $("#myTab li:eq(1)").addClass("active");
            $("#myTab li:eq(0)").removeClass("active");
            $("#tab-berkas").show();
            if (data.isPIC == 1) {
                $("#dibatalkan").show();
            }
            $("#rencana_kerja").removeClass("active");
            $("#perlaksanaan").addClass("active");
            $("#tab-rk").removeClass("active");
            $("#tab-pelakasanaan").addClass("active");

            if (data.Status == 11) $("#dibatalkan").hide();
        }
        if (data.Status == 2) {
            $("#Status").text("Status Pengadaan : Disetujui");
           // cekState("Disetujui");
            $("#collapseOne").addClass("in");
            $("#tab-anwijzing").attr("data-toggle", "collapse");      
            
        }
        if (data.Status == 3) {
            $("#Status").text("Status Pengadaan : Aanwijzing");
           // cekState("Aanwijzing");
            $("#collapseOne").addClass("in");
            $("#tab-anwijzing").attr("data-toggle", "collapse");
        }
        if (data.Status == 4) {
            //cekState("pengisian_harga");
            $("#collapseTwo").addClass("in");
            $("#Status").text("Status Pengadaan : Submit Penawaran");

            $("#tab-anwijzing").attr("data-toggle", "collapse");
            $("#tab-submit-penawaran").attr("data-toggle", "collapse");

			 $(".jadwal-aanwijzing").remove();
        }
        if (data.Status == 5) {
           // cekState("buka_amplop");
            $("#collapseThree").addClass("in");
            $("#Status").text("Status Pengadaan : Buka Amplop");

            $("#tab-anwijzing").attr("data-toggle", "collapse");
            $("#tab-submit-penawaran").attr("data-toggle", "collapse");
            $("#tab-buka-amplop").attr("data-toggle", "collapse");

            $(".jadwal-aanwijzing").remove();
            $(".jadwal-submit").remove();
        }
        if (data.Status == 6) {
           // cekState("penilaian");
            $("#collapse4").addClass("in");
            $("#Status").text("Status Pengadaan : Penilaian Kandidat");

            $("#tab-anwijzing").attr("data-toggle", "collapse");
            $("#tab-submit-penawaran").attr("data-toggle", "collapse");
            $("#tab-buka-amplop").attr("data-toggle", "collapse");
            $("#tab-penilaian-kandidat").attr("data-toggle", "collapse");

            $(".jadwal-aanwijzing").remove();
            $(".jadwal-submit").remove();
            $(".jadwal-buka-amplop").remove();
        }
        if (data.Status == 7) {
           // cekState("klarifikasi");
            $("#collapse5").addClass("in");
            $("#Status").text("Status Pengadaan : Klarifikasi");

            $("#tab-anwijzing").attr("data-toggle", "collapse");
            $("#tab-submit-penawaran").attr("data-toggle", "collapse");
            $("#tab-buka-amplop").attr("data-toggle", "collapse");
            $("#tab-penilaian-kandidat").attr("data-toggle", "collapse");
            $("#tab-klarifikasi").attr("data-toggle", "collapse");

            if (data.isUser == 1 && data.isTEAM == 0) {
                $("#tab-klarifikasi").parent().parent().parent().remove();
                //$("#collapse5").remove();                
            }
            $(".jadwal-aanwijzing").remove();
            $(".jadwal-submit").remove();
            $(".jadwal-buka-amplop").remove();
            $(".jadwal-penilaian").remove();
        }
        if (data.Status == 8) {
            //cekState("penentuanpemenang");
            $("#collapse6").addClass("in");
            
            //getPemenangVendor();
            $("#Status").text("Status Pengadaan : Penentuan Pemenang");

            $("#tab-anwijzing").attr("data-toggle", "collapse");
            $("#tab-submit-penawaran").attr("data-toggle", "collapse");
            $("#tab-buka-amplop").attr("data-toggle", "collapse");
            $("#tab-penilaian-kandidat").attr("data-toggle", "collapse");
            $("#tab-klarifikasi").attr("data-toggle", "collapse");
            $("#tab-penentu-pemenang").attr("data-toggle", "collapse");

            $(".jadwal-aanwijzing").remove();
            $(".jadwal-submit").remove();
            $(".jadwal-buka-amplop").remove();
            $(".jadwal-penilaian").remove();
            $(".jadwal-klarifikasi").remove();
            if (data.isPIC == 1) {
                isSpkUploaded();
            }
        }
        if (data.Status == 11) {
            $("#Status").text("Status Pengadaan : DiBatalkan");
            loadKeteranganDiBatalkan(data.Id);
        }

        if (data.Approver == 1) {
            $(".Setujui").show();
            $(".Tolak").show();
        }
        

        //$("#lihatHps").on("click", function () {
        //    window.open("http://" + window.location.host + "/rks.html#" + $("#pengadaanId").val());
        //}); 
    });
}
function getPemenangVendor() {
    var result;
    $.ajax({
        url: "Api/PengadaanE/GetPemenangVendor?Id=" + $("#pengadaanId").val(),
        success: function (data) {
            if (data == null)
                $("#Status").text("Status Pengadaan : Proses Penentuan Pemenang");
            else {
                $("#dibatalkan").hide();
                $("#Status").text("Status Pengadaan : " + data.Nama + " sebagai Pemenang");
            }
        },
        error: function (errormessage) {
            return 0;
        }
    });
}
function isSpkUploaded() {
    var result;
    $.ajax({
        url: "Api/PengadaanE/isSpkUploaded?Id=" + $("#pengadaanId").val(),
        success: function (data) {            
            if (data == 1) $("#arsipkan").show();
            else $("#arsipkan").hide();
        },
        error: function (errormessage) {
            return 0;
        }
    });
}
function renderDokumenDropzone(myDropzone, tipe) {
    $.ajax({
        url: "Api/PengadaanE/getDokumens?Id=" + $("#pengadaanId").val() + "&tipe=" + tipe,
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

            //location.reload();

        }
    });
}

function hitungHPS(rksId,aturanPenawaran) {
    if (aturanPenawaran == "Price Matching") {
        $.ajax({
            url: "Api/PengadaanE/getRks?Id=" + rksId
        }).done(function (data) {
            var rksdetail = data.data;
            var total = 0;
            for (var key in rksdetail) {
                if (rksdetail[key].hps != null && rksdetail[key].hps != "") {
                    var jumlah = rksdetail[key].jumlah;
                    var hps = rksdetail[key].hps;
                    var totalPerItem = jumlah * hps;
                    total = total + totalPerItem;
                }
            }
            $("#hps").text(accounting.formatNumber(total, { thousand: "." }));
        });
    }
    else {
        $.ajax({
            url: "Api/PengadaanE/getTotalHps?Id=" + rksId,
            method: "POST"
        }).done(function (data) {
            if (data.Total != null || data.Total != "")
                $("#hps").text(accounting.formatNumber(data.Total, { thousand: "." }));
        });
    }
}

function loadListKandidat(Id) {
    $(".listkandidat").html("");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetKandidats?PId=" + Id
    }).done(function (data) {
        $(".listkandidat").html("");
        $.each(data, function (index, item) {
            var html = '<div class="col-md-3"><div class="box box-rekanan" vendorId="' + item.VendorId + '">' +
             '<div class="box-tools pull-right">' +
               '</div>' +
              '<div class="box-body box-profile">' +
               '<p class="profile-username title-header">' + item.Nama + '</p>' +
               //'<p class="text-muted text-center deskripsi">' + item.kontak + '</p>' +
               '<p class="text-muted text-center deskripsi">' + item.Telepon + '</p>' +
             '</div>' +
           '</div></div>';
            $(".listkandidat").append(html);
        });
    });

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
            if (data[i].tipe == "pendaftaran") {                
                if (data[i].Mulai != null)
                    $("#pendaftaran").text(tgl);
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

function LoadListPersonil(Personil,isPic) {
    $(".listperson-pic").html("");
    $(".listperson-staff").html("");
    $(".listperson-controller").html("");
    $(".listperson-compliance").html("");
    $(".listperson-tim").html("");
    for (var item in Personil) {
        if (Personil[item].tipe == "pic")
            addLoadPersonil(Personil[item], ".listperson-pic", isPic);
        if (Personil[item].tipe == "tim")
            addLoadPersonil(Personil[item], ".listperson-tim", isPic);
        if (Personil[item].tipe == "staff")
            addLoadPersonil(Personil[item], ".listperson-staff", isPic);
        if (Personil[item].tipe == "controller")
            addLoadPersonil(Personil[item], ".listperson-controller", isPic);
        if (Personil[item].tipe == "compliance")
            addLoadPersonil(Personil[item], ".listperson-compliance", isPic);
    }
}

function addLoadPersonil(item, el,ispic) {
    var peran = el.replace(".listperson-", "");
    var removeEL = '';
    if (ispic == 1) {        
    }
    if (item.isReady == 1) {
        if (item.isMine == 1 && item.Status==0)
            removeEL = removeEL + '<span class="badge-left check-person"><input type="checkbox" class="ready-checkbox" checked/></span>';
        else removeEL = removeEL + '<span class="badge-left check-person"><input type="checkbox" class="ready-checkbox" checked disabled /></span>';
    }
    else {
        if (item.isMine == 1 && item.Status == 0)
            removeEL = removeEL + '<span class="badge-left check-person"><input type="checkbox" class="ready-checkbox"/></span>';
        else removeEL = removeEL + '<span class="badge-left check-person"><input type="checkbox" class="ready-checkbox" disabled/></span>';
    }
    html = '<a class="btn btn-app">' +
        '<input type="hidden" class="list-personil" attrId="'
                       + item.Id + '" attr1="' + peran + '" attr2="' + item.Nama + '" attr3="'
                       + item.Jabatan + '" value="' + item.PersonilId + '" />' +
                   removeEL +
                   '<i class="fa fa-user"></i>' +
                   item.Nama +
                 '</a>';
    $(el).append(html);
}

function cekState(tipe) {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/cekState?Id=" + $("#pengadaanId").val()+"&tipe="+tipe,
        success: function (data) {
            if (data == 1) {
                location.reload();
                //window.location.replace("http://" + window.location.host + "/pengadaan-detail.html#" + $("#pengadaanId").val());
            }
        },
        error: function (errormessage) {
           // alert("gagal");
        }
    });
}

function loadKeteranganDitolak(Id) {
    $.ajax({
        method: "GET",
        url: "Api/PengadaanE/getAlasanPenolakan?Id="+Id,
        success: function (data) {
                $("#AlasanPenolakan").text("Alasan Penolakan: " + (data==""?"-":data));
                $("#AlasanPenolakan").show();
            },
        error: function (errormessage) {
        }
    });
}

function loadKeteranganDiBatalkan(Id) {
    $.ajax({
        method: "GET",
        url: "Api/PengadaanE/getAlasanDiBatalkan?Id=" + Id,
        success: function (data) {
            $("#AlasanDiBatalkan").text("Alasan DiBatalkan: " + (data == "" ? "-" : data));
            $("#AlasanDiBatalkan").show();
        },
        error: function (errormessage) {
        }
    });
}

function isPemenangApproved(Id) {
    $.ajax({
        url: "Api/PengadaanE/isApprovePemenang?Id=" + Id,
        success: function (data) {
            $("#isPemenangApproved").val(data);
            if (data == 1) {
                $(".bingkai-spk").show();
                $(".bingkai-pengajuan-pemenang").remove();
            }
            else {
                $(".bingkai-spk").remove();
                $(".bingkai-pengajuan-pemenang").show();
            }
            if ($("#isPIC").val() != 1) $("#ajukan-pemenang").remove();
        },
        error: function (errormessage) {
            $("#isPemenangApproved").val(data);
        }
    });
    if ($("#isPIC").val() != 1) $("#ajukan-pemenang").remove();
}

function StatusPemenang(Id) {
    $.ajax({
        url: "Api/PengadaanE/StatusPemenang?pengadaanId=" + Id,
        success: function (data) {
            if (data == 0) {
                $(".status-persetujuan-pemenang").text("Dokumen Pemenang Belum Diajukan");
            }
            if (data > 0) {
                $(".ajukan-pemenang").attr("disabled", "disabled");
                if (data == 1) $(".status-persetujuan-pemenang").text("Dokumen Pemenang Sedang Diajukan");
                if (data == 2) $(".status-persetujuan-pemenang").text("Dokumen Pemenang Telah Disetujui");
                if (data == 3) {
                    $(".status-persetujuan-pemenang").text("Dokumen Pemenang  Ditolak");
                    $(".ajukan-pemenang").removeAttr("disabled");
                }
            }
        },
        error: function (errormessage) {

        }
    });
}

