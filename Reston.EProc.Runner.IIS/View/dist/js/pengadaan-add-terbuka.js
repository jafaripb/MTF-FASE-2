var id_rks = window.location.hash.replace("#", "");

function getListKandidat() {
    arrKandidatPengadaan = [];
    $(".list-kandidat").each(function () {
        objKandidatPengadaan = {};
        objKandidatPengadaan.VendorId = $(this).val();
        arrKandidatPengadaan.push(objKandidatPengadaan);
    });
    return arrKandidatPengadaan;
}

function loadListKandidat(Id, status,xisTEam) {
    $(".listkandidat").html("");
        $.ajax({
            method:"POST",
            url: "Api/PengadaanE/GetKandidats?PId=" + Id
        }).done(function (data) {
            $.each(data, function (index, item) {
                var html = '<div class="col-md-3"><div class="box box-rekanan">';
                if (status == 0 && xisTEam==1) {
                    html = html+'<div class="box-tools pull-right">' +
                            '<span class="badge bg-red remove-vendor" attr="' + item.Id + '">' +
                               '<i class="fa fa-remove"></i>' +
                          '</span>' +
                            '</div>';                   
                }
               html=html+ '<div class="box-body box-profile">' +
                 '<input type="hidden" class="list-kandidat" value="' + item.VendorId + '" />' +
                 '<p class="profile-username title-header">' + item.Nama + '</p>' +
                 //'<p class="text-muted text-center deskripsi">' + item.kontak + '</p>' +
                 '<p class="text-muted text-center deskripsi">' + item.Telepon + '</p>' +
               '</div>' +
             '</div></div>';
            
                $(".listkandidat").append(html);
            });
        });
    
}

function getListPersonil() {
    arrPersonilPengadaan = [];
    $(".list-personil").each(function () {
        objPersonilPengadaan = {};
        objPersonilPengadaan.PersonilId = $(this).val();
        objPersonilPengadaan.tipe = $(this).attr("attr1");
        objPersonilPengadaan.Nama = $(this).attr("attr2");
        objPersonilPengadaan.Jabatan = $(this).attr("attr3");
        arrPersonilPengadaan.push(objPersonilPengadaan);
    });
    return arrPersonilPengadaan;
}

function LoadListPersonil(Personil,Status,xisTeam) {
    $(".listperson-pic").html("");
    $(".listperson-staff").html("");
    $(".listperson-controller").html("");
    $(".listperson-compliance").html("");
    $(".listperson-tim").html("");
    for (var item in Personil) {
        if (Personil[item].tipe == "pic")
            addLoadPersonil(Personil[item], ".listperson-pic", Status, xisTeam);
        if (Personil[item].tipe == "staff")
            addLoadPersonil(Personil[item], ".listperson-staff", Status, xisTeam);
        if (Personil[item].tipe == "tim")
            addLoadPersonil(Personil[item], ".listperson-tim", Status, xisTeam);
        if (Personil[item].tipe == "controller")
            addLoadPersonil(Personil[item], ".listperson-controller", Status, xisTeam);
        if (Personil[item].tipe == "compliance")
            addLoadPersonil(Personil[item], ".listperson-compliance", Status, xisTeam);
    }
}

function getJadwal() {
    arrJadwalPengadaan = [];
    //$(".jadwal").each(function () {
        objJadwalPengadaan = {};
        objJadwalPengadaan.tipe = "pendaftaran";
        if (moment($("#pendaftaran").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Mulai = moment($("#pendaftaran").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm"); //$("#pengisian_harga").val();
        if (moment($("#pendaftaran_sampai").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Sampai = moment($("#pendaftaran_sampai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm"); //$("#pengisian_harga_sampai").val();
        arrJadwalPengadaan.push(objJadwalPengadaan);


        objJadwalPengadaan = {};
        objJadwalPengadaan.tipe = "Aanwijzing";
        if (moment($("#aanwijzing").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Mulai = moment($("#aanwijzing").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");//$("#aanwijzing").val();
        arrJadwalPengadaan.push(objJadwalPengadaan);
        
        objJadwalPengadaan = {};
        objJadwalPengadaan.tipe = "pengisian_harga";
        if (moment($("#pengisian_harga").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Mulai = moment($("#pengisian_harga").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm"); //$("#pengisian_harga").val();
        if (moment($("#pengisian_harga_sampai").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Sampai = moment($("#pengisian_harga_sampai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm"); //$("#pengisian_harga_sampai").val();
        arrJadwalPengadaan.push(objJadwalPengadaan);

        objJadwalPengadaan = {};
        objJadwalPengadaan.tipe = "buka_amplop";
        if (moment($("#buka_amplop").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Mulai = moment($("#buka_amplop").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm"); //$("#buka_amplop").val();
        if (moment($("#buka_amplop_sampai").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Sampai = moment($("#buka_amplop_sampai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm"); //$("#buka_amplop_sampai").val();
        arrJadwalPengadaan.push(objJadwalPengadaan);

        objJadwalPengadaan = {};
        objJadwalPengadaan.tipe = "penilaian";
        if (moment($("#penilaian").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Mulai = moment($("#penilaian").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm"); //$("#buka_amplop").val();
        if (moment($("#penilaian_sampai").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Sampai = moment($("#penilaian_sampai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm"); //$("#buka_amplop_sampai").val();
        arrJadwalPengadaan.push(objJadwalPengadaan);


        objJadwalPengadaan = {};
        objJadwalPengadaan.tipe = "klarifikasi";
        if (moment($("#klarifikasi").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Mulai = moment($("#klarifikasi").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm"); //$("#klarifikasi").val();
        if (moment($("#klarifikasi_sampai").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Sampai = moment($("#klarifikasi_sampai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");// $("#klarifikasi_sampai").val();
        arrJadwalPengadaan.push(objJadwalPengadaan);

        objJadwalPengadaan = {};
        objJadwalPengadaan.tipe = "penentuan_pemenang";
        if (moment($("#penentuan_pemenang").val(), 'D MMMM YYYY HH:mm', 'id', true).isValid())
            objJadwalPengadaan.Mulai = moment($("#penentuan_pemenang").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm"); //$("#penentuan_pemenang").val();
        arrJadwalPengadaan.push(objJadwalPengadaan);
    //});
    return arrJadwalPengadaan;
}

function loadJadwal(Jadwal,xIsTeam) {
    for (var item in Jadwal) {
        if (Jadwal[item].tipe == "pendaftaran") {
            if ((Jadwal[item].Mulai != null || Jadwal[item].Mulai != "") && moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
                // $('.jadwal[attr1="Pengisian Harga"]').attr("attr2", moment(Jadwal[item].Mulai).format("DD/MM/YYYY"));
                $(".pendaftaran[attr1='dari']").val(moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm"));
                $(".pendaftaran[attr1='dari']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".pendaftaran[attr1='dari").removeAttr("disabled");
                    $(".pendaftaran[attr1='sampai").removeAttr("disabled");
                }
            } else $(".pendaftaran[attr1='dari").attr("disabled");
            if ((Jadwal[item].Sampai != null || Jadwal[item].Sampai != "") && moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
                // $('.jadwal[attr1="Pengisian Harga"]').attr("attr3", moment(Jadwal[item].Sampai).format("DD/MM/YYYY"));
                $(".pendaftaran[attr1='sampai']").val(moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm"));
                $(".pendaftaran[attr1='sampai']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".pendaftaran[attr1='sampai").removeAttr("disabled");
                    $(".Aanwijzing[attr1='dari").removeAttr("disabled");
                }
            } else $(".pendaftaran[attr1='sampai']").attr("disabled");
        }
        if (Jadwal[item].tipe == "Aanwijzing") {
            if ((Jadwal[item].Mulai != null || Jadwal[item].Sampai != "")
                && moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
                //$('.jadwal[attr1="Aanwijzing"]').attr("attr2", moment(Jadwal[item].Mulai).format("DD/MM/YYYY"));
                $(".Aanwijzing[attr1='dari']").val(moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm"));
                $(".Aanwijzing[attr1='dari']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".Aanwijzing[attr1='dari").removeAttr("disabled");
                    $(".pengisian_harga[attr1='dari").removeAttr("disabled");
                }
            }
        }
        if (Jadwal[item].tipe == "pengisian_harga") {
            if ((Jadwal[item].Mulai != null || Jadwal[item].Mulai != "") && moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
               // $('.jadwal[attr1="Pengisian Harga"]').attr("attr2", moment(Jadwal[item].Mulai).format("DD/MM/YYYY"));
                $(".pengisian_harga[attr1='dari']").val(moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm"));
                $(".pengisian_harga[attr1='dari']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".pengisian_harga[attr1='dari").removeAttr("disabled");
                    $(".pengisian_harga[attr1='sampai").removeAttr("disabled");
                }
            } else $(".pengisian_harga[attr1='dari").attr("disabled");
            if ((Jadwal[item].Sampai != null || Jadwal[item].Sampai != "") && moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
               // $('.jadwal[attr1="Pengisian Harga"]').attr("attr3", moment(Jadwal[item].Sampai).format("DD/MM/YYYY"));
                $(".pengisian_harga[attr1='sampai']").val(moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm"));
                $(".pengisian_harga[attr1='sampai']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".pengisian_harga[attr1='sampai").removeAttr("disabled");
                    $(".buka_amplop[attr1='dari").removeAttr("disabled");
                }
            } else $(".pengisian_harga[attr1='sampai']").attr("disabled");
        }
        if (Jadwal[item].tipe == "buka_amplop") {
            if ((Jadwal[item].Mulai != null || Jadwal[item].Mulai != "") && moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
                //$('.jadwal[attr1="Buka Amplop"]').attr("attr2", moment(Jadwal[item].Mulai).format("DD/MM/YYYY"));
                $(".buka_amplop[attr1='dari']").val(moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm"));
                $(".buka_amplop[attr1='dari']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".buka_amplop[attr1='dari").removeAttr("disabled");
                    $(".buka_amplop[attr1='sampai").removeAttr("disabled");
                }
            }else $(".buka_amplop[attr1='dari']").attr("disabled");
            if ((Jadwal[item].Sampai != null || Jadwal[item].Sampai != "") && moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
               // $('.jadwal[attr1="Buka Amplop"]').attr("attr3", moment(Jadwal[item].Sampai).format("DD/MM/YYYY"));
                $(".buka_amplop[attr1='sampai']").val(moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm"));
                $(".buka_amplop[attr1='sampai']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".buka_amplop[attr1='sampai").removeAttr("disabled");
                    $(".penilaian[attr1='dari").removeAttr("disabled");
                }
            }else $(".buka_amplop[attr1='sampai']").attr("disabled");
        }
        if (Jadwal[item].tipe == "penilaian") {
            if ((Jadwal[item].Mulai != null || Jadwal[item].Mulai != "") && moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
                // $('.jadwal[attr1="Pengisian Harga"]').attr("attr2", moment(Jadwal[item].Mulai).format("DD/MM/YYYY"));
                $(".penilaian[attr1='dari']").val(moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm"));
                $(".penilaian[attr1='dari']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".penilaian[attr1='dari").removeAttr("disabled");
                    $(".penilaian[attr1='sampai").removeAttr("disabled");
                }
            } else $(".penilaian[attr1='dari']").attr("disabled");
            if ((Jadwal[item].Sampai != null || Jadwal[item].Sampai != "") && moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
                // $('.jadwal[attr1="Pengisian Harga"]').attr("attr3", moment(Jadwal[item].Sampai).format("DD/MM/YYYY"));
                $(".penilaian[attr1='sampai']").val(moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm"));
                $(".penilaian[attr1='sampai']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".penilaian[attr1='sampai").removeAttr("disabled");
                    $(".klarifikasi[attr1='dari").removeAttr("disabled");
                }
            } else $(".penilaian[attr1='sampai']").attr("disabled");
        }
        if (Jadwal[item].tipe == "klarifikasi") {
            if ((Jadwal[item].Mulai != null || Jadwal[item].Mulai != "") && moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
                //$('.jadwal[attr1="Klarifikasi"]').attr("attr2", moment(Jadwal[item].Mulai).format("DD/MM/YYYY"));
                $(".klarifikasi[attr1='dari']").val(moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm"));
                $(".klarifikasi[attr1='dari']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".klarifikasi[attr1='dari").removeAttr("disabled");
                    $(".klarifikasi[attr1='sampai").removeAttr("disabled");
                }
            } else $(".klarifikasi[attr1='dari']").attr("disabled");
            if ((Jadwal[item].Sampai != null || Jadwal[item].Sampai != "") && moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
                //$('.jadwal[attr1="Klarifikasi"]').attr("attr3", moment(Jadwal[item].Sampai).format("DD/MM/YYYY"));
                $(".klarifikasi[attr1='sampai']").val(moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm"));
                $(".klarifikasi[attr1='sampai']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".klarifikasi[attr1='sampai").removeAttr("disabled");
                    $(".penentuan_pemenang[attr1='dari").removeAttr("disabled");
                }
            } else $(".klarifikasi[attr1='sampai']").attr("disabled");
        }
        if (Jadwal[item].tipe == "penentuan_pemenang") {
            if ((Jadwal[item].Mulai != null || Jadwal[item].Mulai != "") && moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
                //$('.jadwal[attr1="Penentuan Pemenang"]').attr("attr2", moment(Jadwal[item].Mulai).format("DD/MM/YYYY"));
                $(".penentuan_pemenang[attr1='dari']").val(moment(Jadwal[item].Mulai).format("DD MMMM YYYY HH:mm"));
                $(".penentuan_pemenang[attr1='dari']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".penentuan_pemenang[attr1='dari").removeAttr("disabled");
                    $(".penentuan_pemenang[attr1='sampai").removeAttr("disabled");
                }
            } else $(".penentuan_pemenang[attr1='dari']").attr("disabled");
            if ((Jadwal[item].Sampai != null || Jadwal[item].Sampai != "") && moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm") != "Invalid date") {
                ///$('.jadwal[attr1="Penentuan Pemenang"]').attr("attr2", moment(Jadwal[item].Sampai).format("DD/MM/YYYY"));
                $(".penentuan_pemenang[attr1='sampai']").val(moment(Jadwal[item].Sampai).format("DD MMMM YYYY HH:mm"));
                $(".penentuan_pemenang[attr1='sampai']").attr("attr2", Jadwal[item].Id);
                if (xIsTeam == 1) {
                    $(".penentuan_pemenang[attr1='sampai").removeAttr("disabled");
                }
            } else $(".penentuan_pemenang[attr1='sampai']").attr("disabled");
        }
    }
}

function getBerkas() {
    var arrBerkas = [];
    var Berkas = {};
    Berkas.Title = $("[name=TitleDokumenNotaInternal]").val();
    Berkas.File = $("[name=FileDokumenNotaInternal]").val();
    Berkas.ContentType = $("[name=FileDokumenNotaInternal]").attr("attr1");
    if ($("[name=FileDokumenNotaInternal]").attr("attr2") != "") Berkas.Id = $("[name=FileDokumenNotaInternal]").attr("attr2");
    Berkas.Tipe = "NOTA";
    arrBerkas.push(Berkas);

    Berkas = {};
    Berkas.Title = $("[name=TitleDokumenLain]").val();
    Berkas.File = $("[name=FileDokumenLain]").val();
    Berkas.ContentType = $("[name=FileDokumenLain]").attr("attr1");
    if ($("[name=FileDokumenLain]").attr("attr2") != "") Berkas.Id = $("[name=FileDokumenNotaInternal]").attr("attr2");
    Berkas.Tipe = "DOKUMENLAIN";
    arrBerkas.push(Berkas);
    Berkas = {};
    Berkas.Title = $("[name=TitleBerkasRujukanLain]").val();
    Berkas.File = $("[name=FileBerkasRujukanLain]").val();
    if ($("[name=FileBerkasRujukanLain]").attr("attr2") != "") Berkas.Id = $("[name=FileDokumenNotaInternal]").attr("attr2");
    Berkas.ContentType = $("[name=FileBerkasRujukanLain]").attr("attr1");
    Berkas.Tipe = "BerkasRujukanLain";
    arrBerkas.push(Berkas);
   
    return arrBerkas;
}

function silensave(pengadaanHeader,attr1) {
    var data = {};
    data.Pengadaan = pengadaanHeader;
    pengadaanHeader.Status = "Draft";
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/save?status=Draft",
        dataType: "json",
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.status == 200) {
            $("#pengadaanId").val(data.Id);
            window.location.hash = data.Id;
           // window.location.reload();
        }
        else {
            alert("error");
        }
    });
}

function saveawal(pengadaanHeader, attr1) {
    var data = {};
    data.Pengadaan = pengadaanHeader;
    pengadaanHeader.Status = "Draft";
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/save?status=Draft",
        dataType: "json",
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        waitingDialog.hideloading();
        if (attr1 == "showmodal") {
            $("#format-rks").attr("href", "rks.html#" + data.Id);
            $("#hpsmodal-list").modal("show");
            if (data.status == 200) {
                $("#pengadaanId").val(data.Id);
                window.location.hash = data.Id;
                loadData(data.Id);
            }
        } else {
            //$("#pengadaanId").val(data.Id);
            if (data.status == 200) {
                $("#pengadaanId").val(data.Id);
                window.location.hash = data.Id;
            }
            if (status == "Draft") {
                $("#TitleKonfirmasi").html("Info");
                $("#KontenConfirmasi").html(data.message);
                $("#konfirmasi").modal("show");
                loadData(data.Id);
            }
            else if (status == "Ajukan") {
                window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
            }
        }
    });
}

function save(pengadaanHeader,attr1,status) {
    var data = {};
    data.Pengadaan = pengadaanHeader;
    data.Jadwal = getJadwal();
    pengadaanHeader.Status = status;
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/save?status=" + status,
        dataType: "json",
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        waitingDialog.hideloading();
        if (attr1 == "showmodal") {
            $("#format-rks").attr("href", "rks.html#" + data.Id);
            $("#hpsmodal-list").modal("show");
        } else {
            //$("#pengadaanId").val(data.Id);
            if (data.status == 200) {                
                window.location.hash = data.Id;
                if (!$("#pengadaanId").val().trim()) {
                    window.location.reload();
                }
                $("#pengadaanId").val(data.Id);
            }
            if (status == "Draft") {
                $("#TitleKonfirmasi").html("Info");
                $("#KontenConfirmasi").html(data.message);
                $("#konfirmasi").modal("show");
                loadData(data.Id);
            }
            else if (status == "Ajukan") {
                window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
            }
        }
    });
    if ($("[name=AturanPenawaran]").val() == "Open Price") {
        var totalHps = $("[name=HpsId]").val();
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/saveTotalHps?Id=" + $("#pengadaanId").val() + "&Total=" + totalHps,
            dataType: "json",
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            
        });
    }
}


function getHeaderPengadaan() {
    var viewPengadaan = {};
    viewPengadaan.Judul = $("[name=Judul]").val();
    viewPengadaan.Keterangan = $("[name=Keterangan]").val();
    viewPengadaan.AturanPengadaan = $("[name=AturanPengadaan]").val();
    viewPengadaan.AturanBerkas = $("[name=AturanBerkas]").val();
    viewPengadaan.AturanPenawaran = $("[name=AturanPenawaran]").val();
    viewPengadaan.MataUang = $("[name=MataUang]").val();
    viewPengadaan.PeriodeAnggaran = $("[name=PeriodeAnggaran]").val();
    viewPengadaan.JenisPembelanjaan = $("[name=JenisPembelanjaan]").val();
    viewPengadaan.HpsId = $("[name=HpsId]").val();
    viewPengadaan.TitleDokumenNotaInternal = $("[name=TitleDokumenNotaInternal]").val();
    viewPengadaan.TitleDokumenLain = $("[name=TitleDokumenLain]").val();
    viewPengadaan.TitleBerkasRujukanLain = $("[name=TitleBerkasRujukanLain]").val();
    viewPengadaan.UnitKerjaPemohon = $("[name=UnitKerjaPemohon]").val();
    viewPengadaan.Pagu = $("[name=Pagu]").val();
    viewPengadaan.Region = $("[name=Region]").val();
    viewPengadaan.Provinsi = $("[name=Provinsi]").val();
    viewPengadaan.KualifikasiRekan = $("[name=KualifikasiRekan]").val();
    viewPengadaan.JenisPekerjaan = $("[name=JenisPekerjaan]").val();
    if ($("#pengadaanId").val() != "") viewPengadaan.Id = $("#pengadaanId").val();
    return viewPengadaan;
}

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
                    '<span class="badge-left check-person"><input  class="ready-checkbox" type="checkbox"/></span>' +
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

function loadHeaderPengadaan(viewPengadaan) {
    var xPermision = viewPengadaan.isCreated == 1 ? 1 : viewPengadaan.isTEAM == 1 ? 1 : 0;
    LoadKriteriaPembobotan(viewPengadaan.Id, xPermision);
    if (viewPengadaan.Status == 0 || viewPengadaan.Status == 10) {
        if (viewPengadaan.isPIC == 1 && viewPengadaan.Status != 10) {
            $(".SimpanAjukan").show();
            
        }
        $("#isTeam").val(viewPengadaan.isTEAM);
        $("#isCreated").val(viewPengadaan.isCreated);
        if (viewPengadaan.isTEAM == 1 || viewPengadaan.isCreated == 1)
            $(".Simpan").show();
        if (viewPengadaan.isTEAM == 0 && viewPengadaan.isCreated==0) {
            $("input").attr("disabled", "disabled");
            $("select").attr("disabled", "disabled");
            $("checkbox").attr("disabled", "disabled");
            $("#buat_hps").text("Lihat HPS");
            $("textarea").attr("disabled", "disabled");
            $("#HapusFile").hide();
            $("#addVendorItem").hide();
            $("#addPerson-pic").parent().text("PIC");
            $("#addPerson-tim").parent().text("Tim");
            $("#addPerson-staff").parent().text("User");
            $("#addPerson-controller").parent().text("Controller");
            $("#addPerson-complaince").parent().text("Complaince");
        }
        if ((viewPengadaan.isPIC == 1 || viewPengadaan.isCreated)) {
            $(".Hapus").show();
        }
    }

    if (viewPengadaan.Status == 1) {
        //buat_hps
        $("input").attr("disabled", "disabled");
        $("select").attr("disabled", "disabled");
        $("textarea").attr("disabled", "disabled");
        $("#buat_hps").text("Lihat HPS");
        $("#buat_hps").attr("status", viewPengadaan.Status);
        $(".SimpanAjukan").show();
    }

    if (viewPengadaan.Status == 10) {
        
        $(".SimpanAjukan").hide();
    }

    if (viewPengadaan.Approver == 1) {
        $(".Setujui").show();
        $(".Tolak").show();
    }

    if (viewPengadaan.AturanPenawaran == "Price Matching") {
        $("[name=HpsId]").attr("disabled", "disabled");
        $("#buat_hps").show();
    }

    if (viewPengadaan.AturanPenawaran == "Open Price") {
        $("#buat_hps").hide();
        $("[name=HpsId]").removeAttr("disabled");
    }
        
    $("[name=Judul]").val(viewPengadaan.Judul);
    $("[name=Keterangan]").val(viewPengadaan.Keterangan) ;
    $("[name=AturanPengadaan]").val(viewPengadaan.AturanPengadaan);
    
    $("[name=AturanBerkas]").val(viewPengadaan.AturanBerkas) ;
    $("[name=AturanPenawaran]").val(viewPengadaan.AturanPenawaran);
    $("[name=MataUang]").val(viewPengadaan.MataUang);
    $("[name=PeriodeAnggaran]").val(viewPengadaan.PeriodeAnggaran);
    $("[name=JenisPembelanjaan]").val(viewPengadaan.JenisPembelanjaan);
    $("[name=TitleDokumenNotaInternal]").val(viewPengadaan.TitleDokumenNotaInternal);
    $("[name=TitleDokumenLain]").val(viewPengadaan.TitleDokumenLain);
    $("[name=TitleBerkasRujukanLain]").val(viewPengadaan.TitleBerkasRujukanLain);
    $("[name=UnitKerjaPemohon]").val(viewPengadaan.UnitKerjaPemohon);

    $("[name=Pagu]").val(viewPengadaan.Pagu);
    $("[name=Region]").val(viewPengadaan.Region);
    $("[name=Provinsi]").val(viewPengadaan.Provinsi);
    $("[name=KualifikasiRekan]").val(viewPengadaan.KualifikasiRekan);
    $("[name=JenisPekerjaan]").val(viewPengadaan.JenisPekerjaan);
    $("#pengadaanId").val(viewPengadaan.Id);

    aturanPenawaranView();
    aturanPengadaanView();

    loadListKandidat(viewPengadaan.Id, viewPengadaan.Status, xPermision);
    loadJadwal(viewPengadaan.JadwalPengadaans, xPermision);
    LoadListPersonil(viewPengadaan.PersonilPengadaans, viewPengadaan.Status, xPermision);
    loadKualifikas(viewPengadaan.KualifikasiKandidats)
    hitungHPS(id_rks);

}

function loadData(id_rks) {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/detailPengadaan?Id=" + id_rks
    }).done(function (data) {        
        loadHeaderPengadaan(data);
        
    });
}

function addVendor(item) {
    var kandidat = {};
    kandidat.PengadaanId = $("#pengadaanId").val();
    kandidat.VendorId = item.id;
    if (isRekananExsis(item.id) == 1) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Kandidat Sudah Ada!',
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
        url: "Api/PengadaanE/saveKandidat",
        dataType: "json",
        data: JSON.stringify(kandidat),
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {

        if (data.status == 200) {
            
            var html = '<div class="col-md-3"><div class="box box-rekanan">' +
             '<div class="box-tools pull-right">';

            if ($("#isTeam").val() == "1" || $("#isCreated").val()=="1") {
                html = html + '<span class="badge bg-red remove-vendor" attr="' + data.Id + '">' +
                         '<i class="fa fa-remove"></i>' +
                    '</span>';
            }
            
                //'<button class="remove-vendor btn btn-box-tool" attr="' + data.Id + '"><i class="fa fa-times"></i></button>' +
            html = html + '</div>' +
               '<div class="box-body box-profile">' +
                '<input type="hidden" class="list-kandidat" value="' + item.id + '" />' +
                '<p class="profile-username title-header">' + item.Nama + '</p>' +
                //'<p class="text-muted text-center deskripsi">' + item.kontak + '</p>' +
                '<p class="text-muted text-center deskripsi">' + item.Telepon + '</p>' +
              '</div>' +
            '</div></div>';
            $(".listkandidat").append(html);
        }
        else {
            alert("error");
        }
    });
}

function isRekananExsis(rekananId) {
    var status = 0;
    $(".list-kandidat").each(function (index, el) {
        if ($.trim($(el).val()) == $.trim(rekananId)) {
            status = 1;
            return false;
        }
    });
    return status;
}

function hapusKandidat(Id,_this) {

    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/deleteKandidat?Id="+Id
       
    }).done(function (data) {
        if (data.status == 200) {
            _this.parent().parent().parent().remove();
        }
        else {
            alert("error");
        }
    });
}

function hapusPersonil(Id, _this) {

    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/deletePersonil?Id=" + Id

    }).done(function (data) {
        if (data.status == 200) {
            // _this.parent().parent().parent().remove();
            _this.parent().remove();
        }
        else {
            alert("error");
        }
    });
}

function addVendorLoad(item, kandidat) {
    //var html = '<div class="col-md-4"><div class="box box-primary">' +
    //        '<div class="box-tools pull-right">' +
    //            '<button class="remove-vendor btn btn-box-tool" attr="' + kandidat.Id + '"><i class="fa fa-times"></i></button>' +
    //          '</div>' +
    //         '<div class="box-body box-profile">' +
    //          '<input type="hidden" class="list-kandidat" value="' + item.id + '" />' +
    //          '<p class="profile-username title-header">' + item.Nama + '</p>' +
    //          //'<p class="text-muted text-center deskripsi">' + item.kontak + '</p>' +
    //          '<p class="text-muted text-center deskripsi">' + item.Telepon + '</p>' +
    //        '</div>' +
    //      '</div></div>';
    var html = '<div class="col-md-3"><div class="box box-rekanan">' +
            '<div class="box-tools pull-right">' +
                   '<span class="badge bg-red remove-vendor" attr="' + kandidat.Id + '">' +
                        '<i class="fa fa-remove"></i>' +
                   '</span>' +
                //'<button class="remove-vendor btn btn-box-tool" attr="' + data.Id + '"><i class="fa fa-times"></i></button>' +
              '</div>' +
             '<div class="box-body box-profile">' +
              '<input type="hidden" class="list-kandidat" value="' + item.id + '" />' +
              '<p class="profile-username title-header">' + item.Nama + '</p>' +
              //'<p class="text-muted text-center deskripsi">' + item.kontak + '</p>' +
              '<p class="text-muted text-center deskripsi">' + item.Telepon + '</p>' +
            '</div>' +
          '</div></div>';
    $(".listkandidat").append(html);
}

function addLoadPersonil(item, el, status, xisTeam) {
    var peran = el.replace(".listperson-", "");
    var removeEL = '';
    if (status == 0 && xisTeam == 1) {
        //removeEL = '<a class="pull-right btn-box-tool remove-person"><i class="fa fa-times"></i></a>';
        removeEL = '<span class="badge bg-red remove-person"><i class="fa fa-remove"></i></span>';
    }
    if (item.isReady == 1) {
        if (item.isMine == 1 && item.Status == 0)
            removeEL = removeEL + '<span class="badge-left check-person"><input type="checkbox" class="ready-checkbox" checked/></span>';
        else removeEL = removeEL + '<span class="badge-left check-person"><input type="checkbox" class="ready-checkbox" checked disabled /></span>';
    }
    else {
        if (item.isMine == 1 && item.Status == 0)
            removeEL = removeEL + '<span class="badge-left check-person"><input type="checkbox" class="ready-checkbox"/></span>';
        else removeEL = removeEL + '<span class="badge-left check-person"><input type="checkbox" class="ready-checkbox" disabled/></span>';
    } html = '<a class="btn btn-app">' +
                   '<input type="hidden" class="list-personil" attrId="'
                       + item.Id + '" attr1="' + peran + '" attr2="' + item.Nama + '" attr3="'
                       + item.Jabatan + '" value="' + item.PersonilId + '" />' +
                   removeEL +
                   '<i class="fa fa-user"></i>' +
                   item.Nama +
                 '</a>';
    $(el).append(html);
}

function isPersonileEksis(idPersonil,elm) {
    var status = 0;
    $(".list-personil").each(function (index, el) {
        if ($.trim($(el).val()) == $.trim(idPersonil)) {
            status = 1;
            return false;
        }
    });
    return status;
}

function isPersonileEksisPerPeran(idPersonil, elm) {
    var status = 0;
    $(elm).find(".list-personil").each(function (index, el) {
        if ($.trim($(el).val()) == $.trim(idPersonil)) {
            status = 1;
            return false;
        }
    });
    return status;
}


function viewFile(File) {
    $.ajax({
        url: "Api/PengadaanE/ViewFile?file=" + File,
        success: function (response) {
            $("#docViewFrame").attr("src", response);
            $("#view-doc-modal").modal("show");
        }
    });
}

function viewDocDetail(File) {
    $.ajax({
        url: "api/PengadaanE/OpenFile?file=" + File
    });
}

function loadKualifikas(kualifikasiKandidat) {
    $(".checkbox-kualifikasi").removeAttr("checked");
    $.each(kualifikasiKandidat, function (index, value) {
        $(".checkbox-kualifikasi[value='" + value.kualifikasi + "']").prop("checked", "true");
        $(".checkbox-kualifikasi[value='" + value.kualifikasi + "']").attr("attrId", value.Id);
    });
}

function LoadKriteriaPembobotan(PengadaanId,xIsTeam) {
    $("#kreteriaPembobotan").html("");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/getKriteriaPembobotan?PengadaanId=" + PengadaanId,
        success:function(data){
            $.each(data, function (index, val) {
                html = '<div class="row">' +
			        '<div class="form-group col-md-4">' +
				        '<label class="title">' + val.NamaKreteria + ':</label>';
                if (xIsTeam == 1) html = html + '<input id="bobot-harga" attrId="' + val.Id + '" type="text" class="form-control input-bobot-pengadaan" value=' + val.Bobot + ' >';
                else html = html + '<input id="bobot-harga" disabled="disabled" attrId="' + val.Id + '" type="text" class="form-control input-bobot-pengadaan" value=' + val.Bobot + ' >';
                html=html+'</div>' +
		         '</div>';	
				$("#kreteriaPembobotan").append(html);
            });
        }
    });

    
}

function addPembobotanPengadaan(el) {
    var totalBobot = 0;
    $(".input-bobot-pengadaan").each(function () {
        totalBobot += $.isNumeric($(this).val()) == true ? parseInt($(this).val()) : 0;        
    });

    if (totalBobot > 100) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Total Bobot Tidak Boleh Lebih dari 100%',
            buttons: [ {
                label: 'Batal',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        $(el).val(0);
        return false;
    }
    var oData = {};
    oData.KreteriaPembobotanId = el.attr("attrId");
    oData.Bobot = el.val();
    oData.PengadaanId = $("#pengadaanId").val();
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/addPembobotanPengadaan",
        dataType: "json",
        data: JSON.stringify(oData),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if(data==0)
                el.val(0);
        }
    });
}

function addLstPembobotanPengadaan() {
    var totalBobot = 0;
    var lstData = [];
    $(".input-bobot-pengadaan").each(function () {
        totalBobot += $.isNumeric($(this).val()) == true ? parseInt($(this).val()) : 0;
        var oData = {};
        oData.KreteriaPembobotanId = $(this).attr("attrId");
        oData.Bobot = $(this).val();
        oData.PengadaanId = $("#pengadaanId").val();
        lstData.push(oData);
    });

    if (totalBobot > 100) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Total Bobot Tidak Boleh Lebih dari 100%',
            buttons: [{
                label: 'Batal',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        $(".input-bobot-pengadaan").val(0);
        return false;
    }

    
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/addLstPembobotanPengadaan",
        dataType: "json",
        data: JSON.stringify(lstData),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data == 0)
                $(".input-bobot-pengadaan").val(0);
        }
    });
}

$(function () {    
    SetListRegion("[name=Region]");
    SetListProvinsi("#listProvinsi");
    SetListPeriode("[name=PeriodeAnggaran]");
    SetListUnitKerja("[name=UnitKerjaPemohon]");
    SetListJenisPekerjaan("[name=JenisPekerjaan]");
    SetListJenisPembelanjaan("[name=JenisPembelanjaan]");
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
            if (parseInt($(this).val())>100) {
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

    $(".listkandidat").on("click", ".remove-vendor", function () {
        var Id = $(this).attr("attr");
        hapusKandidat(Id, $(this));
    });

    $("body").on("click", ".remove-person", function () {
        hapusPersonil($(this).parent().find("input").attr("attrid"), $(this));
    });

    $("body").on("click", ".Hapus", function () {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Apakah Anda Yakin Ingin Mengahapus Pengadaan Ini?',
            buttons: [{
                label: 'Lanjutkan',
                action: function (dialog) {
                    waitingDialog.showloading("Proses Harap Tunggu");
                    $.ajax({
                        method: "POST",
                        url: "Api/PengadaanE/deletePengadaan?Id=" + $("#pengadaanId").val()
                    }).done(function (data) {
                        window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
                        waitingDialog.hideloading();
                    });
                    
                    dialog.close();
                },
            }, {
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                },
            }]
        });
       
    });



    $(".dateJadwal").datetimepicker({
        format: "DD MMMM YYYY HH:mm",
        locale: 'id',
        useCurrent: false,
        minDate: Date.now()

    }).on('dp.change', function (e) {
        var jadwal = $(this).attr("name");
        var nextJadwal = $(this).attr("nextJadwal");
        var beforeJadwal = $(this).attr("beforeJadwal");
        var thisJadwal = moment($(this).val(), ["D MMMM YYYY HH:mm"], "id");
        if (nextJadwal != "") {
            var jadwalNext = moment($(nextJadwal).val(), ["D MMMM YYYY HH:mm"], "id");//.format("DD/MM/YYYY HH:mm");
            $(nextJadwal).data("DateTimePicker").minDate(e.date);
            $(nextJadwal).removeAttr("disabled");

            var jadwalNext = moment($(nextJadwal).val(), ["D MMMM YYYY HH:mm"], "id");
            var diff = thisJadwal.diff(jadwalNext);

            if($(nextJadwal).val()!="")
                if (diff >=0) {
                    BootstrapDialog.show({
                        title: 'Konfirmasi',
                        message: 'Jadwal Tidak Boleh Lebih Besar dari Jadwal Berikutnya!',
                        buttons: [{
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
                    });
                    $(this).val("");
                }

        }
        if (beforeJadwal != "") {
            var jadwalBefore = moment($(beforeJadwal).val(), ["D MMMM YYYY HH:mm"], "id");
            var diff = thisJadwal.diff(jadwalBefore);

            if (diff <=0) {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Jadwal Tidak Boleh Lebih Kecil!',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
                $(this).val("");
            }
            
            if ($(beforeJadwal).val() == "") {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Isi Jadwal Sesuai Urutan!',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
                $(this).val("");
            }
        }
    });

    $(".Simpan").on("click", function () {  
        save(getHeaderPengadaan(), "", "Draft");
        addLstPembobotanPengadaan();
    });

    $(".SimpanAjukan").on("click", function () {
        if ($("[name=TitleDokumenNotaInternal]").val() == ""||myDropzoneDokumenNotaInternal.files.length==0) {
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
        if ($("div.listkandidat .col-md-3").length == 0&&$("#AturanPengadaan").val()=="Pengadaan Tertutup") {
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
        var cek = 1;
        $.each($(".dateJadwal"), function (index, el) {            
            if ($(el).val() == "") {
                if ($("[name=AturanPengadaan]").val() == 'Pengadaan Tertutup') {
                    if (index > 1) {
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
                else {
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
        // save(getHeaderPengadaan(), "", "Ajukan");
    });

    $("#anjukan-lanjutkan").on("click", function () {
        $("#modal-persetujuan").modal("hide");
        save(getHeaderPengadaan(), "", "Ajukan");
    });

    $("#buat_hps").on("click", function () {
        
        if ($(this).attr("status") == 0) {
            $("#hpsmodal").modal("show");
        }
        else window.location.replace("http://" + window.location.host + "/rks.html#" + $("#pengadaanId").val());
    });

    $("#simpanhps").on("click", function () {
        $("#hpsmodal").modal("hide");
        var dataPengadaan = getHeaderPengadaan();
        if (dataPengadaan.hasOwnProperty("Id")) {
            save(dataPengadaan, "showmodal", "Draft");
        }
        else {
            saveawal(dataPengadaan, "showmodal");
        }
        //save(getHeaderPengadaan(), "showmodal", "Draft");
    });

    if (isGuid(id_rks)) {
        $("#pengadaanId").val(id_rks);
        loadData(id_rks);
    }
    else {
        if (isGuid($("#pengadaanId").val())) {
            window.location.hash = $("#pengadaanId").val();
           
        } else {
            //$(".SimpanAjukan").show();
            $(".Simpan").show();
            //$(".Hapus").show();
           // silensave(getHeaderPengadaan());//save dulu
        }
    }
   // }

    $(".buat-baru").on("click", function () {
        window.location.replace("http://" + window.location.host + window.location.pathname);
    });
    var acceptedFiles = ".png,.jpg,.pdf,.xls,.xlsx,.jpeg,.doc,.docx";
    //dropzone
    var myDropzoneDokumenNotaInternal = new Dropzone("#DokumenNotaInternal",
            {
                url: $("#DokumenNotaInternal").attr("action") + "&id=" + $("#pengadaanId").val(),
                maxFilesize: 5,
                acceptedFiles: acceptedFiles,
                accept: function (file, done) {
                    var pengadaanId = $("#pengadaanId").val();                    
                    if (pengadaanId.trim()) {
                        done();
                    }
                    else {
                        myDropzoneDokumenNotaInternal.removeFile(file);
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Save Dokumen Sebelum Upload File!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {

                                    dialog.close();
                                }
                            }]
                        });
                    }
                },
                init: function () {
                    this.on("addedfile", function (file) {
                        file.previewElement.addEventListener("click", function () {
                            var id = 0;
                            if (file.Id != undefined)
                                id = file.Id
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

    var myDropzoneDokumenLain = new Dropzone("#DokumenLain",
            {
                url: $("#DokumenLain").attr("action") + "&id=" + $("#pengadaanId").val(),
                maxFilesize: 5,
                acceptedFiles: acceptedFiles,
                accept: function (file, done) {
                   // var pengadaanId = $("#pengadaanId").val();
                    if ($("#pengadaanId").val().trim()) {
                        done();
                    }
                    else {
                        myDropzoneDokumenLain.removeFile(file);
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Save Dokumen Sebelum Upload File!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {

                                    dialog.close();
                                }
                            }]
                        });
                    }
                },
                init: function () {
                    this.on("addedfile", function (file) {
                        file.previewElement.addEventListener("click", function () {
                            var id = 0;
                            if (file.Id != undefined)
                                id = file.Id
                            else
                                id = $.parseJSON(file.xhr.response);
                            //viewFile(data.Id);
                            $("#konfirmasiFile").attr("attr1", "DokumenLain");
                            $("#konfirmasiFile").attr("FileId", id);
                            $("#konfirmasiFile").modal("show");

                        });
                    });
                }
            }
        );
    renderDokumenDropzone(myDropzoneDokumenLain,"DOKUMENLAIN");
    Dropzone.options.DokumenLain = false;

    var myDropzoneBerkasRujukanLain = new Dropzone("#BerkasRujukanLain",
            {
                url: $("#BerkasRujukanLain").attr("action") + "&id=" + $("#pengadaanId").val(),
                maxFilesize: 5,
                acceptedFiles: acceptedFiles,
                accept: function (file, done) {
                    var pengadaanId = $("#pengadaanId").val();
                    if (pengadaanId.trim()) {
                        done();
                    }
                    else {
                        myDropzoneBerkasRujukanLain.removeFile(file);
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Save Dokumen Sebelum Upload File!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {

                                    dialog.close();
                                }
                            }]
                        });
                    }
                },
                init: function () {
                    this.on("addedfile", function (file) {
                        file.previewElement.addEventListener("click", function () {
                            var id = 0;
                            if (file.Id != undefined)
                                id = file.Id
                            else
                                id = $.parseJSON(file.xhr.response);
                            //viewFile(data.Id);
                            $("#konfirmasiFile").attr("attr1", "BerkasRujukanLain");
                            $("#konfirmasiFile").attr("FileId", id);
                            $("#konfirmasiFile").modal("show");

                        });
                    });
                }
            }
        );
    renderDokumenDropzone(myDropzoneBerkasRujukanLain,"BerkasRujukanLain");
    Dropzone.options.BerkasRujukanLain = false;

    $("#HapusFile").on("click", function () {
        var tipe = $(this).parent().parent().parent().parent().attr("attr1");
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");
       
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/deleteDokumen?Id=" + FileId
        }).done(function (data) {
            if (data.Id == "1") {
                if (tipe == "DokumenNotaInternal") {

                    $.each(myDropzoneDokumenNotaInternal.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }
                        
                        if (id == FileId) {
                            myDropzoneDokumenNotaInternal.removeFile(item);
                            $("#konfirmasiFile").modal("hide");
                        }
                    });
                }
                if (tipe == "DOKUMENLAIN") {
                    $.each(myDropzoneDokumenLain.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzoneDokumenLain.removeFile(item);
                            $("#konfirmasiFile").modal("hide");
                        }
                    });
                }
                if (tipe == "BerkasRujukanLain") {
                    $.each(myDropzoneBerkasRujukanLain.files, function (index, item) {
                        myDropzoneBerkasRujukanLain.removeFile(item);
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzoneBerkasRujukanLain.removeFile(item);
                            $("#konfirmasiFile").modal("hide");
                        }
                    });
                }
            }

        });
    });

    $(".Setujui").on("click", function () {
        $.ajax({
            method:"POST",
            url: "Api/PengadaanE/persetujuan?Id=" + $("#pengadaanId").val() ,
            success: function (data) {
                if (data.status == 200) {                    
                  //  $(location).attr('href', window.location.host + "/pengadaan-list.html");
                    window.location.replace(window.location.host + "/pengadaan-list.html");
                }
            },
            error: function (errormessage) {
                alert("gagal");
            }
        });
    });

    $(".checkbox-kualifikasi").on("click", function () {
        var ell = $(this);
        if ($(this).is(':checked')) {
            var data = {};
            data.kualifikasi = $(this).val();
            data.PengadaanId = $("#pengadaanId").val();           
            $.ajax({
                method: "POST",
                url: "Api/PengadaanE/saveKualifikasiKandidat",
                dataType: "json",
                data: JSON.stringify(data),
                contentType: 'application/json; charset=utf-8',
                success: function (xdata) {
                    $(ell).attr("attrId", xdata.Id);
                },
                error: function (errormessage) {
                    alert("gagal");
                }

            })

        }
        else {            
            $.ajax({
                method: "POST",
                url: "Api/PengadaanE/deleteKualifikasiKandidat?Id="+$(this).attr("attrId"),
                success: function (data) {
                    $(ell).attr("attrId", "");
                },
                error: function (errormessage) {
                    alert("gagal");
                }

            });
        }
    });

    $("[name=AturanPenawaran]").on('change', function () {
        aturanPenawaranView();
    });

    $("#downloadFile").on("click", function () {
        var tipe = $(this).parent().parent().parent().parent().attr("attr1");
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");
        downloadFileUsingForm("/api/pengadaane/OpenFile?Id=" + FileId);
    });
    
    $("#AturanPengadaan").on("change", function () {
        aturanPengadaanView();
    });
});

function aturanPengadaanView() {
    if ($("#AturanPengadaan").val() == "Pengadaan Terbuka") {
        //$("#BingkaiHps").hide();
        //$("#BingkaiKandidat").hide();
        $("#jadwal_pendaftaran").show();
        $("#aanwijzing").attr("disabled", "disabled");

    }
    else {
        // $("#BingkaiHps").show();
        $("#BingkaiKandidat").show();
        $("#jadwal_pendaftaran").hide();
        $("#aanwijzing").removeAttr("disabled");
    }
}

function aturanPenawaranView() {
    if ($("[name=AturanPenawaran]").val() == 'Price Matching') {
        $("[name=HpsId]").attr("disabled", "disabled");
        $("#buat_hps").show();
    } else {
        $("[name=HpsId]").removeAttr("disabled");
        $("#buat_hps").hide();
    }
}

function renderDokumenDropzone(myDropzone,tipe) {
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

function hitungHPS(rksId) {
    if ($("[name=AturanPenawaran]").val() == "Price Matching") {
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

            $("[name=HpsId]").val(total);
        });
    }
    else {
        $.ajax({
            url: "Api/PengadaanE/getTotalHps?Id=" + rksId,
            method:"POST"
        }).done(function (data) {            
            if (data.Total != null || data.Total != "")
                $("[name=HpsId]").val(data.Total);
        });
    }
}




