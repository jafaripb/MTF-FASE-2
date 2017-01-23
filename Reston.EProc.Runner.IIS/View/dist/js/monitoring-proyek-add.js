var table;
var table_pekerjaan;
var table_pembayaran;
var table_dokumen_pekerjaan;
var table_dokumen_pembayaran;

// Tabel Tahapan Pekerjaan
$(function () {
    var PengadaanID = gup("id-pengadaan");
    $("#pengadaanId").val(PengadaanID);
    table_pekerjaan = $('#tahapan-pekerjaan').DataTable({
        "serverSide": true,
        "searching": false,
        "ajax": {
            "url": 'api/Proyek/TampilTahapanPekerjaan?Id=' + PengadaanID,
            "type": 'POST',
            "data": function (d) {
                Id = d.Id;
            },
            "error": function (jqXHR, textStatus, errorThrown) {
            }
        },
        "columns": [
            { "data": "NamaTahapan" },
            { "data": "BobotPekerjaan" },
            { "data": "TanggalMulai" },
            { "data": "TanggalSelesai" },
            { "data": "null" },
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    return moment(row.TanggalMulai).format("DD MMMM YYYY");
                },
                "targets": 2,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    return moment(row.TanggalSelesai).format("DD MMMM YYYY");
                },
                "targets": 3,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    return ' <a  tahapId="' + row.Id + '"  class="btn btn-xs btn-info tambah-dokumen-pekerjaan" title="Dokumen Pekerjaan"><span class="fa fa-cloud-upload"></span> Dokumen </a> ' +
                    ' <a tahapId="' + row.Id + '" class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a>';
                },
                "targets": 4,
                "orderable": false
            },
        ],
        "paging": true,
        "lengthChange": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true
    });

    $("body").on("click", ".remove-person", function () {
        hapusPersonil($(this).parent().find("input").attr("attrid"), $(this));
    });
});

function hapusPersonil(Id, _this) {
    $.ajax({
        method: "POST",
        url: "Api/Proyek/deletePIC?Id=" + Id

    }).done(function (data) {
        if (data.status == 200) {
            //_this.parent().parent().parent().remove();
            _this.parent().remove();
        }
        else {
            alert("error");
        }
    });
}

// Tabel Tahapan Pembayaran
$(function () {
    var PengadaanID = gup("id-pengadaan");
    $("#pengadaanId").val(PengadaanID);
    table_pembayaran = $('#tahapan-pembayaran').DataTable({
        "serverSide": true,
        "searching": false,
        "ajax": {
            "url": 'api/Proyek/TampilTahapanPembayaran?Id=' + PengadaanID,
            "type": 'POST',
            "data": function (d) {
                Id = d.Id;
            },
            "error": function (jqXHR, textStatus, errorThrown) {
            }
        },
        "columns": [
            { "data": "NamaTahapan" },
            { "data": "TanggalMulai" },
            { "data": "TanggalSelesai" },
            { "data": "null" },
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    return moment(row.TanggalMulai).format("DD MMMM YYYY");
                },
                "targets": 1,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    return moment(row.TanggalSelesai).format("DD MMMM YYYY");
                },
                "targets": 2,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    return ' <a  tahapId="' + row.Id + '"  class="btn btn-xs btn-info tambah-dokumen-pembayaran" title="Dokumen Pembayaran"><span class="fa fa-cloud-upload"></span> Dokumen </a> ' +
                    '<a tahapId="' + row.Id + '" class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a>';
                },
                "targets": 3,
                "orderable": false
            },
        ],
        "paging": true,
        "lengthChange": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true
    });
});

// Tabel Dokumen Pekerjaan
function TampilDokumenPekerjaan() {
    var id_tahapan = $('#id_tahapModalPekerjaan').val();
    console.log(id_tahapan);
    table_dokumen_pekerjaan = $('#list-dokumen-pekerjaan').DataTable({
        "serverSide": false,
        "searching": false,
        "ajax": {
            "url": 'api/Proyek/TampilDokumenPekerjaan?Id=' + id_tahapan,
            "type": 'POST',
            "data": function (d) {
                Id = d.Id;
            },
            "error": function (jqXHR, textStatus, errorThrown) {
            }
        },
        "columns": [

            { "data": "NamaDokumen" },
            { "data": "null" },
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    return ' <a tahapDokId="' + row.Id + '" class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a>';
                },
                "targets": 1,
                "orderable": false
            },
        ],
        "pageLength": 5,
        "paging": true,
        "lengthChange": false,
        "ordering": false,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "destroy": true,
    })
}

// Tabel Dokumen Pembayaran
function TampilDokumenPembayaran() {
    var id_tahapan = $('#id_tahapModalPembayaran').val();
    table_dokumen_pembayaran = $('#list-dokumen-pembayaran').DataTable({
        "serverSide": true,
        "searching": false,
        "ajax": {
            "url": 'api/Proyek/TampilDokumenPembayaran?Id=' + id_tahapan,
            "type": 'POST',
            "data": function (d) {
                Id = d.Id;
            },
            "error": function (jqXHR, textStatus, errorThrown) {
            }
        },
        "columns": [

            { "data": "NamaDokumen" },
            { "data": "null" },
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    return ' <a tahapDokId="' + row.Id + '" class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a>';
                },
                "targets": 1,
                "orderable": false
            },
        ],
        "paging": true,
        "lengthChange": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "destroy": true,
        "iDisplayLength": 3
    })
}

// Menampilkan Konfirmasi Delete Pekerjaan
function showDeleteConfirmationPekerjaan(idd) {
    $("#tobeDeletePekerjaan").val(idd);
    $("#DeleteModalPekerjaan").modal('show');
}

// Menampilkan Konfirmasi Delete Pembayaran
function showDeleteConfirmationPembayaran(idd) {
    $("#tobeDeletePembayaran").val(idd);
    $("#DeleteModalPembayaran").modal('show');
}

// Menampilkan Konfirmasi Delete Dokumen Pekerjaan
function showDeleteConfirmationDokumenPekerjaan(idd) {
    $("#tobeDeleteDokumenPekerjaan").val(idd);
    $("#DeleteModalDokumenPekerjaan").modal('show');
}

// Menampilkan Konfirmasi Delete Dokumen Pembayaran
function showDeleteConfirmationDokumenPembayaran(idd) {
    $("#tobeDeleteDokumenPembayaran").val(idd);
    $("#DeleteModalDokumenPembayaran").modal('show');
}

// Fungsi Delete Pekerjaan
function DeletePekerjaan(idd) {
    $("#DeleteModalPekerjaan").modal('hide');
    var v = 0;
    if (!idd) {
        v = $("#tobeDeletePekerjaan").val();
    }
    else v = idd
    $.ajax({
        url: "/api/Proyek/delete?id=" + v,
        success: function (data) {
            table_pekerjaan.draw();
        },
        complete: function (xhr, textStatus) {
            ajaxCompleteProcess(xhr);
        }
    });
}

// Fungsi Delete Pembayaran
function DeletePembayaran(idd) {
    $("#DeleteModalPembayaran").modal('hide');
    var v = 0;
    if (!idd) {
        v = $("#tobeDeletePembayaran").val();
    }
    else v = idd
    $.ajax({
        url: "/api/Proyek/delete?id=" + v,
        success: function (data) {
            table_pembayaran.draw();
        },
        complete: function (xhr, textStatus) {
            ajaxCompleteProcess(xhr);
        }
    });
}

// Fungsi Delete Dokumen Pekerjaan
function DeleteDokumenPekerjaan(idd) {
    $("#DeleteModalDokumenPekerjaan").modal('hide');
    var v = 0;
    if (!idd) {
        v = $("#tobeDeleteDokumenPekerjaan").val();
    }
    else v = idd
    $.ajax({
        url: "/api/Proyek/deleteDok?id=" + v,
        success: function (data) {
            table_dokumen_pekerjaan.ajax.reload();
        },
        complete: function (xhr, textStatus) {
            ajaxCompleteProcess(xhr);
        }
    });
}

// Fungsi Delete Pekerjaan
function DeleteDokumenPembayaran(idd) {
    $("#DeleteModalDokumenPembayaran").modal('hide');
    var v = 0;
    if (!idd) {
        v = $("#tobeDeleteDokumenPembayaran").val();
    }
    else v = idd
    $.ajax({
        url: "/api/Proyek/deleteDok?id=" + v,
        success: function (data) {
            table_dokumen_pembayaran.ajax.reload();
        },
        complete: function (xhr, textStatus) {
            ajaxCompleteProcess(xhr);
        }
    });
}

// Tombol Hapus Tahapan Pekerjaan di Klik
$("#tahapan-pekerjaan").on("click", ".remove-item", function () {
    showDeleteConfirmationPekerjaan($(this).attr("tahapId"));
});

// Tombol Hapus Tahapan Pembayaran di Klik
$("#tahapan-pembayaran").on("click", ".remove-item", function () {
    showDeleteConfirmationPembayaran($(this).attr("tahapId"));
});

// Tombol Hapus Tahapan Dokumen Pekerjaan di Klik
$("#list-dokumen-pekerjaan").on("click", ".remove-item", function () {
    showDeleteConfirmationDokumenPekerjaan($(this).attr("tahapDokId"));
});

// Tombol Hapus Tahapan Dokumen Pembayaran di Klik
$("#list-dokumen-pembayaran").on("click", ".remove-item", function () {
    showDeleteConfirmationDokumenPembayaran($(this).attr("tahapDokId"));
});

// Tambah Tahapan Pekerjaan
function TambahTahapanPekerjaan() {
    var nPengadaanId = $("#pengadaanId").val();
    var nNamaTahapanPekerjaan = $("#nama-tahapan-pekerjaan").val();
    var nTanggalMulai = moment($("#tanggal-pekerjaan-mulai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    var nTanggalSelesai = moment($("#tanggal-pekerjaan-selesai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    var nBobotPekerjaan = $("#bobot-pekerjaan").val();
    var nJenisTahapan = 'Pekerjaan';
    $.ajax({
        method: "post",
        url: "api/Proyek/SimpanTahapanPekerjaan",
        datatype: "json",
        data: {
            aPengdaanId: nPengadaanId,
            aNamaTahapanPekerjaan: nNamaTahapanPekerjaan,
            aTanggalMulai: nTanggalMulai,
            aTanggalSelesai: nTanggalSelesai,
            aBobotPekerjaan: nBobotPekerjaan,
            aJenisTahapan: nJenisTahapan
        },
        success: function (d) {
            $("#tahapanup").modal("hide");
            $("#nama-tahapan-pekerjaan").val("");
            $("#tanggal-pekerjaan-mulai").val("");
            $("#tanggal-pekerjaan-selesai").val("");
            $("#bobot-pekerjaan").val("");

                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: d.message,
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });

                table_pekerjaan.draw();
        }
    })
}

// Tambah Tahapan Pembayaran
function TambahTahapanPembayaran() {
    var nPengadaanId = $("#pengadaanId").val();
    var nNamaTahapanPembayaran = $("#nama-tahapan-pembayaran").val();
    var nTanggalMulai = moment($("#tanggal-pembayaran-mulai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    var nTanggalSelesai = moment($("#tanggal-pembayaran-selesai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    var nJenisTahapan = 'Pembayaran';
    var nBobotPekerjaan = 0;

    $.ajax({
        method: "post",
        url: "api/Proyek/SimpanTahapanPembayaran",
        datatype: "json",
        data: {
            aPengdaanId: nPengadaanId,
            aNamaTahapanPekerjaan: nNamaTahapanPembayaran,
            aTanggalMulai: nTanggalMulai,
            aTanggalSelesai: nTanggalSelesai,
            aJenisTahapan: nJenisTahapan,
            aBobotPekerjaan: nBobotPekerjaan
        },
        success: function (d) {
            $("#tahapanuppem").modal("hide");
            $("#nama-tahapan-pembayaran").val("");
            $("#tanggal-pembayaran-mulai").val("");
            $("#tanggal-pembayaran-selesai").val("");
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Data Berhasil di Simpan',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            table_pembayaran.ajax.reload();
        }
    })
}

// Tambah Draf Perencanaan Proyek Pekerjaan(statusnya = draft)
function TambahProyekDrafPekerjaan() {
    var nPengadaanId = $("#pengadaanId").val();
    var nStartDate = moment($("#pendaftaran").val(), ["D MMMM YYYY "], "id").format("DD/MM/YYYY HH:mm");
    var nEnddate = moment($("#pendaftaran_sampai").val(), ["D MMMM YYYY "], "id").format("DD/MM/YYYY HH:mm");
    var nStatus = $("#status").val();

    $.ajax({
        method: "post",
        url: "api/Proyek/SimpanRencanaProyek",
        datatype: "json",
        data: {
            aPengadaanId: nPengadaanId,
            aStartDate: nStartDate,
            aEndDate: nEnddate,
            aStatus: nStatus
        },
        success: function (d) {
            //popuptahapan();

            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: d.message,
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
        }
    })
}

// Simpan No. Kontrak dan Update Status Rencana Proyek (Simpan Proyek)
function SimpanProyek() {
    var PengadaanId = $("#pengadaanId").val();
    var nNoKontrak = $("#no-spk").val();
    var nStatus = "Dijalankan";
    $.ajax({
        method: "post",
        url: "api/Proyek/UbahStatusRencanaProyek",
        datatype: "json",
        data: {
            Id: PengadaanId,
            NoKontrak: nNoKontrak,
            Status: nStatus
        },
        success: function (d) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Data Berhasil di Simpan',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
        }
    })
}

// Tambah Tahapan Dokumen
function TambahTahapanDokumen() {
    var nTahapanPekerjaanId = $("#tahapanpekerjaanId").val(); //Get dari Id Tahap Pekerjaan
    var nNamaDokumen = $("#nama-dokumen").val();
    var nTanggalPekerjaan = moment($("#tanggal-tahapan-pekerjaan").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    var nJenisTahapan = 'Pekerjaan';

    $.ajax({
        method: "post",
        url: "api/Proyek/SimpanTahapanDokumen",
        datatype: "json",
        data: {
            aPengdaanId: nPengadaanId,
            aNamaDokumen: nNamaDokumen,
            aTanggalPekerjaan: nTanggalPekerjaan,
            aJenisTahapan: nJenisTahapan
        },
        success: function (d) {
            $("#tahapanup").modal("hide");
            $("#tahapanuppem").modal("hide");
            $("#nama-tahapan-pekerjaan").val("");
            $("#tanggal-tahapan-pekerjaan").val("");
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Data Berhasil di Simpan',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            table_dokumen_pekerjaan.draw();
        }
    })
}

// Tambah Draf Perencanaan Proyek Pembayaran(statusnya = draft)
function TambahProyekDrafPembayaran() {
    var nPengadaanId = $("#pengadaanId").val();
    var nStartDate = moment($("#pendaftaran").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    var nEnddate = moment($("#pendaftaran_sampai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    var nStatus = $("#status").val();

    $.ajax({
        method: "post",
        url: "api/Proyek/SimpanRencanaProyek",
        datatype: "json",
        data: {
            aPengadaanId: nPengadaanId,
            aStartDate: nStartDate,
            aEndDate: nEnddate,
            aStatus: nStatus
        },
        success: function (d) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: d.message,
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
        }

    })
}

$("#SimpanJalankan").on("click", function () {
    SimpanProyek();
});

// pop up tahapan pekerjaan
function popuptahapan() {
    $("#tahapanup").modal("show");
}

// pop up tahapan pembayaran
function popuptahapanpembayaran() {
    $("#tahapanuppem").modal("show");
}

// pop up modal dokumen pekerjaan
function TampilModalDokumen(id_tahapan) {
    $("#id_tahapModalPekerjaan").val(id_tahapan);
    $("#dokumenupPekerjaan").modal('show');
    TampilDokumenPekerjaan();
}

// pop up modal dokumen pembayaran
function TampilModalDokumenPembayaran(id_tahapan) {
    $("#id_tahapModalPembayaran").val(id_tahapan);
    $("#dokumenupPembayaran").modal('show');
    TampilDokumenPembayaran();
}

// Fungsi Tambah Dokumen Tahapan Pekerjaan
function TambahDokumenPekerjaan(id_tahapan) {
    var id_tahapan = 0;
    Id_Tahapan = $("#id_tahapModalPekerjaan").val();
    Jenis_Dokumen = $("#jenis-tahapanModalPekerjaan").val();
    Nama_Dokumen = $("#nama-dokumen").val();

    $.ajax({
        method: "post",
        url: "api/Proyek/SimpanTahapanPekerjaanDokumen",
        datatype: "json",
        data: {
            aId_Tahapan: Id_Tahapan,
            aJenis_Tahapan: Jenis_Dokumen,
            aNama_Dokumen: Nama_Dokumen
        },
        success: function (d) {
            $("#list-dokumen-pekerjaan").modal("hide");
            Nama_Dokumen = $("#nama-dokumen").val("");
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Data Berhasil di Simpan',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            //table_dokumen_pekerjaan.draw();
            table_dokumen_pekerjaan.ajax.reload();
        }
    })
}

// Fungsi Tambah Dokumen Tahapan Pembayaran
function TambahDokumenPembayaran(id_tahapan) {
    var id_tahapan = 0;

    Id_Tahapan = $("#id_tahapModalPembayaran").val();
    Jenis_Dokumen = $("#jenis-tahapanModalPembayaran").val();
    Nama_Dokumen = $("#nama-dokumen-pembayaran").val();

    $.ajax({
        method: "post",
        url: "api/Proyek/SimpanTahapanPembayaranDokumen",
        datatype: "json",
        data: {
            aId_Tahapan: Id_Tahapan,
            aJenis_Tahapan: Jenis_Dokumen,
            aNama_Dokumen: Nama_Dokumen
        },
        success: function (d) {
            $("#list-dokumen-pembayaran").modal("hide");
            Nama_Dokumen = $("#nama-dokumen").val("");
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: 'Data Berhasil di Simpan',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
            table_dokumen_pembayaran.draw();
        }
    })
}

// JS untuk pop Up di dalam pop up
$(document).on('show.bs.modal', '.modal', function (event) {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
});

// JS untuk ambil id pengadaan
$(function () {
    var PengadaanID = gup("id-pengadaan");
    $("#pengadaanId").val(PengadaanID);
    $.ajax({
        method: "post",
        url: "api/Proyek/TampilJudul?Id=" + PengadaanID,
        success: function (d) {
            $("#judul").val(d.Judul);
            $("#no-pengadaan").val(d.NoPengadaan);
            $("#pelaksana").val(d.Pelaksana);
            $("#no-spk").val(d.NOSPK);
            $("#nilai-kontrak").val(numberWithCommas(d.NilaiKontrak));
            if (d.TanggalMulai != null)
                $("#pendaftaran").val(moment(d.TanggalMulai).format("DD MMMM YYYY"));
            if (d.TanggalSelesai != null)
            $("#pendaftaran_sampai").val(moment(d.TanggalSelesai).format("DD MMMM YYYY"));
            //$("#isPIC").val(d.PIC[0].Id);
            //LoadListPersonil(d.PIC);
        }
    })
})

// Fungsi Separator Thousand
function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

function loadData(PengadaanID) {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/detailPengadaan?Id=" + id_rks
    }).done(function (data) {
        loadHeaderPengadaan(data); // apa aja yang di tampilin
    });
}

// JS untuk personil
$(".addPerson").on("click", function () {
    $("#title-modal").html("Daftar Personil")
    $("#tipe-person-list").val($(this).attr("attr1"));
    $("#personilModal").modal("show");
    $(".item-bg-blue-light").each(function () {
        $(this).removeClass("item-bg-blue-light");
    });
});

// JS URL
function gup(name, url) {
    if (!url) url = location.href;
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(url);
    return results == null ? null : results[1];
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

function LoadListPersonil(PIC) {
    var PengadaanID = gup("id-pengadaan");
    $("#pengadaanId").val(PengadaanID);
    addLoadPersonil(PIC, ".listperson-pic", 0);
}

//Fungsi Untuk Tambah Personil
function addPersonil(item, el) {
    var peran = el.replace(".listperson-", "");
    var objPersonilProyek = {};
    objPersonilProyek.UserId = item.PersonilId;
    objPersonilProyek.tipe = peran;
    objPersonilProyek.Nama = item.Nama;
    objPersonilProyek.Jabatan = item.Jabatan;
    objPersonilProyek.PengadaanId = $("#pengadaanId").val();

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
        url: "Api/Proyek/savePersonil",
        dataType: "json",
        data: JSON.stringify(objPersonilProyek),
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
            $("#personilModal").modal("hide");
        }
        else {
            alert("error");
        }
    });
}

function isPersonileEksis(idPersonil, elm) {
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

function addLoadPersonil(item, el, ispic) {
    var peran = el.replace(".listperson-", "");
    var removeEL = '';
    html = '<a class="btn btn-app">' +
        '<input type="hidden" class="list-personil" attrId="'
                       + item[0].Id + '" attr1="' + item[0].Id + '" attr2="'
                       + item[0].NamaPIC + '" value="' + item[0].Id + '" />' +
                   '<span class="badge bg-red remove-person"><i class="fa fa-remove"></i></span>' +
                   '<i class="fa fa-user"></i>' +
                   item[0].NamaPIC +
                 '</a>';
    $(el).append(html);
}

// date picker
$(".dateJadwal").datetimepicker({
    format: "DD MMMM YYYY",
    locale: 'id',
    useCurrent: false

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

        if ($(nextJadwal).val() != "")
            if (diff >= 0) {
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

        if (diff <= 0) {
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

// bukan gw
function addItem(item) {
    var jumRow = table.data().length;
    var nextNo = 1;
    if (jumRow > 0) {
        var data = table.row(jumRow - 1).data();
        nextNo = parseInt(data[0]) + 1;
    }
    table.row.add([nextNo, item[1], item[2], '<a class="btn btn-xs btn-danger remove-item" title="Hapus"><span class="fa fa-trash-o"></span></a>']).draw();
}
function hitungHargaItem() {
    var totalHarga = 0;
    table.rows().every(function () {
        var d = this.data();
        var harga = d[2];
        d.counter++; // update data source for the row
        harga = parseInt(UnformatFloat(harga));
        this.invalidate(); // invalidate the data DataTables has cached for this row
        totalHarga = totalHarga + harga;

    });
    var estimasiCost = accounting.formatNumber(totalHarga, { thousand: "." });
    $("#estimated_cost").val(estimasiCost);
}


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

    //Tambah Dokumen Tahapan
    // Nangkep Action + tambah atribut
    $("#tahapan-pekerjaan").on("click", ".tambah-dokumen-pekerjaan", function () {
        TampilModalDokumen($(this).attr("tahapId"));
        //alert($(this).attr("tahapId"))
    });

    //Tambah Dokumen Tahapan
    // Nangkep Action + tambah atribut
    $("#tahapan-pembayaran").on("click", ".tambah-dokumen-pembayaran", function () {
        TampilModalDokumenPembayaran($(this).attr("tahapId"));
        //alert($(this).attr("tahapId"))
    });
}]);