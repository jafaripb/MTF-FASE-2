﻿
$(function () {
    $(".dateJadwal").datetimepicker({
        format: "DD MMMM YYYY HH:mm",
        locale: 'id',
        useCurrent: false,
        minDate: Date.now()

    });
    getAanwijzing();
    var myDropzoneBeritaAcaraAanwijzing = new Dropzone("#BeritaAcaraAanwijzing",
            {
                url: $("#BeritaAcaraAanwijzing").attr("action") + "&id=" + $("#pengadaanId").val(),
                maxFilesize: 5,
                acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx,.docx",
                accept: function (file, done) {
                    if ($("#isPIC").val() == 1) {
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Dengan mengklik "Lanjutkan" Berarti State akan Berlanjut Ke state Berikutnya!',
                            buttons: [{
                                label: 'Lanjutkan',
                                action: function (dialog) {
                                    done();
                                    nextState("pengisian_harga");
                                    dialog.close();
                                }
                            }, {
                                label: 'Batal',
                                action: function (dialog) {
                                    myDropzoneBeritaAcaraAanwijzing.removeFile(file);
                                    dialog.close();

                                }
                            }]
                        });
                    } else {
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Anda Tidak Memiliki Akses!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    myDropzoneBeritaAcaraAanwijzing.removeFile(file);
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
                                id = $.parseJSON(file.xhr.response)
                            //viewFile(data.Id);
                            $("#HapusFile").show();
                            $("#konfirmasiFile").attr("attr1", "BeritaAcaraAanwijzing");
                            $("#konfirmasiFile").attr("FileId", id);
                            $("#konfirmasiFile").modal("show");
                        });
                    });
                }
            }
        );

    renderDokumenDropzone(myDropzoneBeritaAcaraAanwijzing, "BeritaAcaraAanwijzing");
    Dropzone.options.BeritaAcaraAanwijzing = false;

    var myDropzoneBerkasBeritaAcaraAanwijzing = new Dropzone("#BerkasBeritaAcaraAanwijzing",
           {
               url: $("#BerkasBeritaAcaraAanwijzing").attr("action") + "&id=" + $("#pengadaanId").val(),
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
                           $("#konfirmasiFile").attr("attr1", "BeritaAcaraAanwijzing");
                           $("#konfirmasiFile").attr("FileId", id);
                           $("#konfirmasiFile").modal("show");
                       });
                   });
               }
           }
       );
    
    renderDokumenDropzone(myDropzoneBerkasBeritaAcaraAanwijzing, "BeritaAcaraAanwijzing");
  
    Dropzone.options.BerkasBeritaAcaraAanwijzing = false;

    var myDropzoneBeritaAcaraBukaAmplop = new Dropzone("#BeritaAcaraBukaAmplop",
            {
                url: $("#BeritaAcaraBukaAmplop").attr("action") + "&id=" + $("#pengadaanId").val(),
                maxFilesize: 5,
                acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.docx,.xlsx",
                accept: function (file, done) {
                    var jumFile = myDropzoneBeritaAcaraAanwijzing.files.length;
                    if (jumFile > 1) {
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Berkas Sudah Adda',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    myDropzoneBeritaAcaraBukaAmplop.removeFile(file);
                                    dialog.close();
                                }
                            }]
                        });
                    } else {
                        var cekBukaAmplop = 1;
                        $(".persetujuan-buka-amplop").each(function () {
                            if ($(this).hasClass("btn-danger")) {
                                cekBukaAmplop = 0;
                                return false;
                            }
                        });
                        console.log(cekBukaAmplop);
                        if (cekBukaAmplop == 0) {
                            BootstrapDialog.show({
                                title: 'Konfirmasi',
                                message: 'Semua Personil Harus Membuka Amplop Sebelum Ke Tahap Berikutnya!',
                                buttons: [ {
                                    label: 'Close',
                                    action: function (dialog) {
                                        myDropzoneBeritaAcaraBukaAmplop.removeFile(file);
                                        dialog.close();
                                    }
                                }]
                            });
                        }

                        if ($("#isPIC").val() == 1 && cekBukaAmplop==1) {
                            BootstrapDialog.show({
                                title: 'Konfirmasi',
                                message: 'Dengan mengklik "Lanjutkan" Berarti State akan Berlanjut Ke state Berikutnya!',
                                buttons: [{
                                    label: 'Lanjutkan',
                                    action: function (dialog) {
                                        done();
                                        nextState("penilaian");
                                        dialog.close();
                                    }
                                }, {
                                    label: 'Batal',
                                    action: function (dialog) {
                                        myDropzoneBeritaAcaraBukaAmplop.removeFile(file);
                                        dialog.close();

                                    }
                                }]
                            });
                        }
                        else {
                            if (cekBukaAmplop == 1) {
                                BootstrapDialog.show({
                                    title: 'Konfirmasi',
                                    message: 'Anda Tidak Memiliki Akses!',
                                    buttons: [{
                                        label: 'Close',
                                        action: function (dialog) {
                                            myDropzoneBeritaAcaraBukaAmplop.removeFile(file);
                                            dialog.close();

                                        }
                                    }]
                                });
                            }
                        }
                    }
                },
                init: function () {
                    this.on("addedfile", function (file) {
                        file.previewElement.addEventListener("click", function () {
                            var id = 0;
                            if (file.Id != undefined)
                                id = file.Id;
                            else
                                id = $.parseJSON(file.xhr.response);
                            //viewFile(data.Id);
                            $("#HapusFile").show();
                            $("#konfirmasiFile").attr("attr1", "BeritaAcaraBukaAmplop");
                            $("#konfirmasiFile").attr("FileId", id);
                            $("#konfirmasiFile").modal("show");
                        });
                    });
                }
            }
        );

    renderDokumenDropzone(myDropzoneBeritaAcaraBukaAmplop, "BeritaAcaraBukaAmplop");
    Dropzone.options.BeritaAcaraBukaAmplop = false;
    
    var myDropzoneBerkasBeritaAcaraBukaAmplop = new Dropzone("#BerkasBeritaAcaraBukaAmplop",
          {
              url: $("#BerkasBeritaAcaraBukaAmplop").attr("action") + "&id=" + $("#pengadaanId").val(),
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
                          $("#konfirmasiFile").attr("attr1", "BeritaAcaraBukaAmplop");
                          $("#konfirmasiFile").attr("FileId", id);
                          $("#konfirmasiFile").modal("show");
                      });
                  });
              }
          }
      );

    renderDokumenDropzone(myDropzoneBerkasBeritaAcaraBukaAmplop, "BeritaAcaraBukaAmplop");
    Dropzone.options.BerkasBeritaAcaraBukaAmplop = false;

    var myDropzoneBeritaAcaraKlarifikasi = new Dropzone("#BeritaAcaraKlarifikasi",
           {
               url: $("#BeritaAcaraKlarifikasi").attr("action") + "&id=" + $("#pengadaanId").val(),
               maxFilesize: 5,
               acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
               accept: function (file, done) {
                   var jumFile = myDropzoneBeritaAcaraKlarifikasi.files.length;
                   if (jumFile > 1) {
                       BootstrapDialog.show({
                           title: 'Konfirmasi',
                           message: 'Berkas Sudah Adda',
                           buttons: [{
                               label: 'Close',
                               action: function (dialog) {
                                   myDropzoneBeritaAcaraKlarifikasi.removeFile(file);
                                   dialog.close();
                               }
                           }]
                       });
                   } else {
                       if ($("#isPIC").val() == 1) {
                           var cekRekananCheck = 0;
                           $(".checkbox-pilih-pemenang").each(function () {
                               if ($(this).prop('checked') == true) {
                                   cekRekananCheck = cekRekananCheck + 1;
                               }
                           });
                           if (cekRekananCheck > 0) {
                               BootstrapDialog.show({
                                   title: 'Konfirmasi',
                                   message: 'Dengan mengklik "Lanjutkan" Berarti State akan Berlanjut Ke state Berikutnya!',
                                   buttons: [{
                                       label: 'Lanjutkan',
                                       action: function (dialog) {
                                           done();
                                           nextState("penentuan_pemenang");
                                           dialog.close();
                                       }
                                   }, {
                                       label: 'Batal',
                                       action: function (dialog) {
                                           myDropzoneBeritaAcaraKlarifikasi.removeFile(file);
                                           dialog.close();

                                       }
                                   }]
                               });
                           }
                           else {
                               BootstrapDialog.show({
                                   title: 'Konfirmasi',
                                   message: 'Anda Belum Memilih Kandidat Pemenang',
                                   buttons: [{
                                       label: 'Close',
                                       action: function (dialog) {
                                           myDropzoneBeritaAcaraKlarifikasi.removeFile(file);
                                           dialog.close();

                                       }
                                   }]
                               });
                           }
                       }
                       else {
                           BootstrapDialog.show({
                               title: 'Konfirmasi',
                               message: 'Anda Tidak Memiliki Akses!',
                               buttons: [{
                                   label: 'Close',
                                   action: function (dialog) {
                                       myDropzoneBeritaAcaraBukaAmplop.removeFile(file);
                                       dialog.close();
                                   }
                               }]
                           });
                       }

                   }
               },
               init: function () {
                   this.on("addedfile", function (file) {
                       file.previewElement.addEventListener("click", function () {
                           var id = 0;
                           if (file.Id != undefined)
                               id = file.Id;
                           else
                               id = $.parseJSON(file.xhr.response);
                           //viewFile(data.Id);
                           $("#HapusFile").show();
                           $("#konfirmasiFile").attr("attr1", "BeritaAcaraKlarifikasi");
                           $("#konfirmasiFile").attr("FileId", id);
                           $("#konfirmasiFile").modal("show");
                       });
                   });
               }
           }
       );

    renderDokumenDropzone(myDropzoneBeritaAcaraKlarifikasi, "BeritaAcaraKlarifikasi");
    Dropzone.options.BeritaAcaraKlarifikasi = false;

    var myDropzoneBerkasBeritaAcaraKlarifikasi = new Dropzone("#BerkasBeritaAcaraKlarifikasi",
         {
             url: $("#BerkasBeritaAcaraKlarifikasi").attr("action") + "&id=" + $("#pengadaanId").val(),
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
                         $("#konfirmasiFile").attr("attr1", "BeritaAcaraKlarifikasi");
                         $("#konfirmasiFile").attr("FileId", id);
                         $("#konfirmasiFile").modal("show");
                     });
                 });
             }
         }
     );

    renderDokumenDropzone(myDropzoneBerkasBeritaAcaraKlarifikasi, "BeritaAcaraKlarifikasi");
    Dropzone.options.BerkasBeritaAcaraKlarifikasi = false;


    var myDropzoneBeritaAcaraPenentuanPemenang = new Dropzone("#BeritaAcaraPenentuanPemenang",
           {
               url: $("#BeritaAcaraPenentuanPemenang").attr("action") + "&id=" + $("#pengadaanId").val(),
               maxFilesize: 5,
               acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
               accept: function (file, done) {
                   var jumFile = myDropzoneBeritaAcaraPenentuanPemenang.files.length;
                   
                   if (jumFile > 1) {
                       BootstrapDialog.show({
                           title: 'Konfirmasi',
                           message: 'Berkas Sudah Adda',
                           buttons: [{
                               label: 'Close',
                               action: function (dialog) {
                                   myDropzoneBeritaAcaraPenentuanPemenang.removeFile(file);
                                   dialog.close();
                               }
                           }]
                       });
                   } else {
                       done();
                   }
               },
               init: function () {
                   this.on("addedfile", function (file) {
                       file.previewElement.addEventListener("click", function () {
                           var id = 0;
                           if (file.Id != undefined)
                               id = file.Id;
                           else
                               id = $.parseJSON(file.xhr.response);
                           $("#HapusFile").show();
                           $("#konfirmasiFile").attr("attr1", "BeritaAcaraPenentuanPemenang");
                           $("#konfirmasiFile").attr("FileId", id);
                           $("#konfirmasiFile").modal("show");
                       });
                   });
                   this.on("complete", function (file) {
                       if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                           isSpkUploaded();
                       }
                   });
               }
           }
       );

    renderDokumenDropzone(myDropzoneBeritaAcaraPenentuanPemenang, "BeritaAcaraPenentuanPemenang");
    Dropzone.options.BeritaAcaraPenentuanPemenang = false;

    var myDropzoneBerkasBeritaAcaraPenentuanPemenang = new Dropzone("#BerkasBeritaAcaraPenentuanPemenang",
             {
                 url: $("#BerkasBeritaAcaraPenentuanPemenang").attr("action") + "&id=" + $("#pengadaanId").val(),
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
                             $("#konfirmasiFile").attr("attr1", "BeritaAcaraPenentuanPemenang");
                             $("#konfirmasiFile").attr("FileId", id);
                             $("#konfirmasiFile").modal("show");
                         });
                     });
                 }
             }
         );
    renderDokumenDropzone(myDropzoneBerkasBeritaAcaraPenentuanPemenang, "BeritaAcaraPenentuanPemenang");
    Dropzone.options.BerkasBeritaAcaraPenentuanPemenang = false;

    //var myDropzoneLembarDisposisi = new Dropzone("#LembarDisposisi",
    //       {
    //           url: $("#LembarDisposisi").attr("action") + "&id=" + $("#pengadaanId").val(),
    //           maxFilesize: 5,
    //           acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
    //           accept: function (file, done) {
    //               var jumFile = myDropzoneLembarDisposisi.files.length;
    //               if (jumFile > 1) {
    //                   BootstrapDialog.show({
    //                       title: 'Konfirmasi',
    //                       message: 'Berkas Sudah Adda',
    //                       buttons: [{
    //                           label: 'Close',
    //                           action: function (dialog) {
    //                               myDropzoneLembarDisposisi.removeFile(file);
    //                               dialog.close();
    //                           }
    //                       }]
    //                   });
    //               } else {
    //                   done();
    //               }
    //           },
    //           init: function () {
    //               this.on("addedfile", function (file) {
    //                   file.previewElement.addEventListener("click", function () {
    //                       var id = 0;
    //                       if (file.Id != undefined)
    //                           id = file.Id;
    //                       else
    //                           id = $.parseJSON(file.xhr.response);
    //                       $("#HapusFile").show();
    //                       $("#konfirmasiFile").attr("attr1", "LembarDisposisi");
    //                       $("#konfirmasiFile").attr("FileId", id);
    //                       $("#konfirmasiFile").modal("show");
    //                   });
    //               });
    //           }
    //       }
    //   );

    //renderDokumenDropzone(myDropzoneLembarDisposisi, "LembarDisposisi");
    //Dropzone.options.LembarDisposisi = false;

    //var myDropzoneBerkasLembarDisposisi = new Dropzone("#BerkasLembarDisposisi",
    //      {
    //          url: $("#BerkasLembarDisposisi").attr("action") + "&id=" + $("#pengadaanId").val(),
    //          maxFilesize: 5,
    //          acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
    //          clickable: false,
    //          dictDefaultMessage: "Tidak Ada Dokumen",
    //          init: function () {
    //              this.on("addedfile", function (file) {
    //                  file.previewElement.addEventListener("click", function () {
    //                      var id = 0;
    //                      if (file.Id != undefined)
    //                          id = file.Id;
    //                      else
    //                          id = $.parseJSON(file.xhr.response);
    //                      $("#Hapus").hide();
    //                      $("#konfirmasiFile").attr("attr1", "BerkasLembarDisposisi");
    //                      $("#konfirmasiFile").attr("FileId", id);
    //                      $("#konfirmasiFile").modal("show");
    //                  });
    //              });
    //          }
    //      }
    //  );

    //renderDokumenDropzone(myDropzoneBerkasLembarDisposisi, "LembarDisposisi");
    //Dropzone.options.BerkasLembarDisposisi = false;

    var myDropzoneSuratPerintahKerja = new Dropzone("#SuratPerintahKerja",
          {
              url: $("#SuratPerintahKerja").attr("action") + "&id=" + $("#pengadaanId").val(),
              maxFilesize: 5,
              acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
              accept: function (file, done) {
                  var jumFile = myDropzoneSuratPerintahKerja.files.length;
                  if (jumFile > 1) {
                      BootstrapDialog.show({
                          title: 'Konfirmasi',
                          message: 'Berkas Sudah Adda',
                          buttons: [{
                              label: 'Close',
                              action: function (dialog) {
                                  myDropzoneSuratPerintahKerja.removeFile(file);
                                  dialog.close();
                              }
                          }]
                      });
                  } else {
                      done();
                  }
              },
              init: function () {
                  this.on("addedfile", function (file) {
                      file.previewElement.addEventListener("click", function () {
                          var id = 0;
                          if (file.Id != undefined)
                              id = file.Id;
                          else
                              id = $.parseJSON(file.xhr.response);
                          $("#HapusFile").show();
                          $("#konfirmasiFile").attr("attr1", "SuratPerintahKerja");
                          $("#konfirmasiFile").attr("FileId", id);
                          $("#konfirmasiFile").modal("show");
                      });
                  });
                  this.on("complete", function (file) {
                      if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                          isSpkUploaded();
                      }
                  });
              }
          }
      );

    renderDokumenDropzone(myDropzoneSuratPerintahKerja, "SuratPerintahKerja");
    Dropzone.options.SuratPerintahKerja = false;

    var myDropzoneBerkasSuratPerintahKerja = new Dropzone("#BerkasSuratPerintahKerja",
          {
              url: $("#BerkasSuratPerintahKerja").attr("action") + "&id=" + $("#pengadaanId").val(),
              maxFilesize: 5,
              acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
              accept: function (file, done) {
                  var jumFile = myDropzoneBerkasSuratPerintahKerja.files.length;
                  if (jumFile > 1) {
                      BootstrapDialog.show({
                          title: 'Konfirmasi',
                          message: 'Berkas Sudah Adda',
                          buttons: [{
                              label: 'Close',
                              action: function (dialog) {
                                  myDropzoneBerkasSuratPerintahKerja.removeFile(file);
                                  dialog.close();
                              }
                          }]
                      });
                  } else {
                      done();
                  }
              },
              init: function () {
                  this.on("addedfile", function (file) {
                      file.previewElement.addEventListener("click", function () {
                          var id = 0;
                          if (file.Id != undefined)
                              id = file.Id;
                          else
                              id = $.parseJSON(file.xhr.response);
                          $("#HapusFile").show();
                          $("#konfirmasiFile").attr("attr1", "SuratPerintahKerja");
                          $("#konfirmasiFile").attr("FileId", id);
                          $("#konfirmasiFile").modal("show");
                      });
                  });
              }
          }
      );

    renderDokumenDropzone(myDropzoneBerkasSuratPerintahKerja, "SuratPerintahKerja");
    Dropzone.options.BerkasSuratPerintahKerja = false;

    $("#HapusFile").on("click", function () {
        var tipe = $(this).parent().parent().parent().parent().attr("attr1");
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/deleteDokumenPelaksanaan?Id=" + FileId
        }).done(function (data) {
            if (data.Id == "1") {
                
                if (tipe == "BeritaAcaraAanwijzing") {
                    $.each(myDropzoneBeritaAcaraAanwijzing.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzoneBeritaAcaraAanwijzing.removeFile(item);                           
                        }
                    });
                }
                if (tipe == "BeritaAcaraBukaAmplop") {
                    $.each(myDropzoneBeritaAcaraBukaAmplop.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzoneBeritaAcaraBukaAmplop.removeFile(item);
                        }
                    });
                }
                if (tipe == "BeritaAcaraKlarifikasi") {
                    $.each(myDropzoneBeritaAcaraKlarifikasi.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzoneBeritaAcaraKlarifikasi.removeFile(item);
                        }
                    });
                }

                if (tipe == "BeritaAcaraPenentuanPemenang") {
                    
                    $.each(myDropzoneBeritaAcaraPenentuanPemenang.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }
                        
                        if (id == FileId) {
                            myDropzoneBeritaAcaraPenentuanPemenang.removeFile(item);
                        }
                    });
                }
                if (tipe == "LembarDisposisi") {
                    
                    $.each(myDropzoneLembarDisposisi.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzoneLembarDisposisi.removeFile(item);
                        }
                    });
                }
                if (tipe == "SuratPerintahKerja") {
                    isSpkUploaded();
                    $.each(myDropzoneSuratPerintahKerja.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzoneSuratPerintahKerja.removeFile(item);
                        }
                    });
                }
                if (tipe == "BeritaAcaraPenilaian") {
                    $.each(myDropzoneBeritaAcaraPenilaian.files, function (index, item) {
                        var id = 0;
                        if (item.Id != undefined) {
                            id = item.Id;
                        }
                        else {
                            id = $.parseJSON(item.xhr.response);
                        }

                        if (id == FileId) {
                            myDropzoneBeritaAcaraPenilaian.removeFile(item);
                        }
                    });
                }
            }
            $("#konfirmasiFile").modal("hide");
        });
    });

    $("#downloadFile").on("click", function () {
        var tipe = $(this).parent().parent().parent().parent().attr("attr1");
        var FileId = $(this).parent().parent().parent().parent().attr("FileId");

        downloadFileUsingForm("/api/pengadaane/OpenFile?Id=" + FileId);
    });


    $(".ubah-jadwal").on("click", function () {
        if ($($(this).attr("attr1")).attr("disabled") && $($(this).attr("attr3")).attr("disabled")) {
            var now = moment();
            var TimeMulai = moment($($(this).attr("attr1")).val(), ["D MMMM YYYY HH:mm"], "id");//.format("DD/MM/YYYY HH:mm");
            console.log(now);
            console.log(TimeMulai);
            if (TimeMulai != "Invalid date") {
                if(TimeMulai>now)
                    $($(this).attr("attr1")).removeAttr("disabled");
            }
            $($(this).attr("attr3")).removeAttr("disabled");
            $(this).html('<i class="fa fa-fw fa-calendar"></i>Submit');
        } else {
            $($(this).attr("attr1")).attr("disabled", "");
            $($(this).attr("attr3")).attr("disabled", "");
            $(this).html('<i class="fa fa-fw fa-calendar"></i>Ubah');
            if ($(this).attr("attr2") == "aanwijzing_pelaksanaan")
                rubahDateAanwijzing();
            if ($(this).attr("attr2") == "pengisian_harga")
                    rubahDateSubmitPenawaran();
            if ($(this).attr("attr2") == "buka_amplop")
                rubahDateBukaAmplop();
            if ($(this).attr("attr2") == "penilaian_kandidat")
                rubahPenilaian();
            if ($(this).attr("attr2") == "pelaksanaan-klarifikasi")
                rubahKlarifikasi();
            if ($(this).attr("attr2") == "pelaksanaan-pemenang")
                rubahPemenang();
            //rubahDateSubmitPenawaran();
        }
    });

    $(".ubah-jadwal-xxx").on("click", function () {
        if ($($(this).attr("attr1")).attr("disabled")) {
            $($(this).attr("attr1")).removeAttr("disabled");
            $(this).html('<i class="fa fa-fw fa-calendar"></i>Submit');
        } else {
            $($(this).attr("attr1")).attr("disabled", "");
            $(this).html('<i class="fa fa-fw fa-calendar"></i>Rubah');
            if ($(this).attr("attr2") == "aanwijzing_pelaksanaan")
                rubahDateAanwijzing();
            //if ($(this).attr("attr2") == pengisian_harga)
            //    rubahDateSubmitPenawaran();
            if($(this).attr("attr2")=="buka_amplop")
                rubahDateBukaAmplop();

        }
    });

    getListKandidatPelaksanaan();

    $(".kehadiran-kandidat").on("click", ".absen-kandidat", function () {
        $(this).attr("disabled");
        var el = $(this);
        if ($(this).is(':checked')) {
            //save kehadiran
            $.ajax({
                method: "POST",
                url: "Api/PengadaanE/saveKehadiranAanwjzing?KandidatId=" + $(this).attr("vid"),
                success: function (data) {
                    $(this).removeAttr("disabled");
                    if (data.Id == "00000000-0000-0000-0000-000000000000") {
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Anda Tidak Memiliki Akses!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    dialog.close();
                                }
                            }]
                        });
                        $(el).attr('checked', false);
                    }
                },
                error: function (errormessage) {
                    $(el).removeAttr("disabled");
                    $(el).attr('checked', false);
                }
            });
        }
        else {
            //delete kehadiran
            $.ajax({
                method: "POST",
                url: "Api/PengadaanE/deleteKehadiranAanwjzing?Id=" + $(this).attr("vid"),
                success: function (data) {
                    $(el).removeAttr("disabled");
                    $(el).attr('checked', false);
                },
                error: function (errormessage) {
                    $(el).removeAttr("disabled");
                    $(el).attr('checked', true);
                }
            });
        }
    });

    $("#nextaanwijzing").on("click", function () {
        updateStatus(4);
    });

    getDateSubmitPenawaran();
    getBukaAmplop();
    getPersetujuanBukaAmplop();
    getListSubmitRekanan();
    getListPenilainRekanan();
    getPenilaian();
    getKandidatPemenang();

    $(".persetujuan-buka-amplop").on("click", function () {
        $(".persetujuan-buka-amplop").attr("disabled");
        if (!$(this).hasClass("btn-success")) {
            waitingDialog.showloading("Proses Harap Tunggu");
            $.ajax({
                method: "POST",
                url: "Api/PengadaanE/addPersetujuanBukaAmplop?Id=" + $("#pengadaanId").val(),
                success: function (data) {
                    waitingDialog.hideloading();
                    getPersetujuanBukaAmplop();
                    if (data.Id == "00000000-0000-0000-0000-000000000000") {
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Anda Tidak Memiliki Akses!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    dialog.close();
                                }
                            }]
                        });
                    }
                },
                error: function (errormessage) {
                    waitingDialog.hideloading();
                }
            });
        }

    });

    $(".list-submit-rekanan").on("click", ".vendor-detail", function () {
        window.open("http://" + window.location.host + "/rekanan-detail.html?id=" + $(this).attr("attrId"));
    });

    $(".lihat-penilaian").on("click", function () {
        window.open("http://" + window.location.host + "/rks-penilaian.html#" + $("#pengadaanId").val());
    });

    $(".lihat-klarifikasi").on("click", function () {
        window.open("http://" + window.location.host + "/rks-klarifikasi.html#" + $("#pengadaanId").val());
    });
    
    $(".list-rekanan-penilaian").on("click", ".checkbox-pilih-kandidat", function () {
        var objData = {};
        objData.PengadaanId = $("#pengadaanId").val();
        objData.VendorId = $(this).attr("vendorId");
        waitingDialog.showloading("Proses Harap Tunggu");
        if ($(this).is(':checked')) {
            $.ajax({
                method: "POST",
                url: "Api/PengadaanE/addPilihKandidat",
                dataType: "json",
                data: JSON.stringify(objData),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    //$("#aanwijzing_pelaksanaan").val(moment(data.Mulai).format("DD/MM/YYYY"));
                    $(this).attr('checked', true);
                    waitingDialog.hideloading();
                },
                error: function (errormessage) {
                    $(this).attr('checked', false);
                    waitingDialog.hideloading();
                }
            });
        }
        else {
            $.ajax({
                method: "POST",
                url: "Api/PengadaanE/deletePilihKandidat",
                dataType: "json",
                data: JSON.stringify(objData),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    //$("#aanwijzing_pelaksanaan").val(moment(data.Mulai).format("DD/MM/YYYY"));
                    $(this).attr('checked', false);
                    waitingDialog.hideloading();
                },
                error: function (errormessage) {
                    $(this).attr('checked', true);
                    waitingDialog.hideloading();
                }
            });
        }

    });

    $(".list-rekanan-klarifikasi-penilaian").on("click", ".delete-klarifikasi-kandidat", function () {

        var objData = {};
        objData.PengadaanId = $("#pengadaanId").val();
        objData.VendorId = $(this).attr("vendorId");
        var el = $(this);
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Apakah Anda Yakin Mengeliminasi Kandidat Ini! ',
            buttons: [{
                label: 'Lanjutkan',
                action: function (dialog) {
                    waitingDialog.showloading("Proses Harap Tunggu");
                    $.ajax({
                        method: "POST",
                        url: "Api/PengadaanE/deletePilihKandidat",
                        dataType: "json",
                        data: JSON.stringify(objData),
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            waitingDialog.hideloading();
                            el.parent().parent().parent().remove();
                        },
                        error: function (errormessage) {
                            waitingDialog.hideloading();
                        }
                    });
                    dialog.close();
                }
            }, {
                label: 'Batal',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });        
    });

    $("body").on("click", ".box-klarifikasi-rekanan", function () {
        var id=$(this).attr("vendorId");
        var pengadaanId = $("#pengadaanId").val();
        BootstrapDialog.show({
            title: 'Konfirmasi',
            buttons: [{
                label: 'Lihat Informasi Rekanan',
                action: function (dialog) {
                    window.open("http://" + window.location.host + "/rekanan-detail.html?id=" + id);
                    dialog.close();
                }
            },
            {
                label: 'Lihat Harga Penawaran Kandidat Ini',
                action: function (dialog) {
                    window.open("http://" + window.location.host + "/rks-klarifikasi-penilaian.html#" + pengadaanId + "&" + id);
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

    $(".arsipkan").on("click", function () {
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            url: "Api/PengadaanE/arsipkan?Id=" + $("#pengadaanId").val(),
            success: function (data) {
                waitingDialog.hideloading();
                if (data.Id == 1) {
                    window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
                } else {
                    BootstrapDialog.show({
                        title: 'Konfirmasi',
                        message: 'Anda Tidak Memiliki Akses!',
                        buttons: [{
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
                    });
                }
            },
            error: function (errormessage) {
                waitingDialog.hideloading();
            }
        });
    });

    $(".kirim-undangan").on("click", function () {
        waitingDialog.showloading("Proses Harap Tunggu");
        var odata = { PId: $("#pengadaanId").val(), Surat: $("#undangan").val() }
        $.ajax({
            //data: odata,
            method:"POST",
            url: "Api/PengadaanE/sendMail",
            data: { PengadaanId: $("#pengadaanId").val(), Surat: $("#undangan").val() },
            dataType: "text",
            success: function (data) {
                waitingDialog.hideloading();
                if (data== 1) {
                    BootstrapDialog.show({
                        title: 'Konfirmasi',
                        message: 'Undangan Berhasil Terkirim!',
                        buttons: [{
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
                    });
                } else {
                    BootstrapDialog.show({
                        title: 'Konfirmasi',
                        message: 'Anda Tidak Memiliki Akses!',
                        buttons: [{
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
                    });
                }
            },
            error: function (errormessage) {
                waitingDialog.hideloading();
            }
        });
    });

    $(".kirim-undangan-klarifikasi").on("click", function () {
        waitingDialog.showloading("Proses Harap Tunggu");
        var odata = { PId: $("#pengadaanId").val(), Surat: $("#mKlarifikasi").val() }
        $.ajax({
            //data: odata,
            method: "POST",
            url: "Api/PengadaanE/sendMailKlarifikasi",
            data: { PengadaanId: $("#pengadaanId").val(), Surat: $("#mKlarifikasi").val() },
            dataType: "text",
            success: function (data) {
                waitingDialog.hideloading();
                if (data == 1) {
                    BootstrapDialog.show({
                        title: 'Konfirmasi',
                        message: 'Undandangan Berhasil Terkirim!',
                        buttons: [{
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
                    });
                } else {
                    BootstrapDialog.show({
                        title: 'Konfirmasi',
                        message: 'Anda Tidak Memiliki Akses!',
                        buttons: [{
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
                    });
                }
            },
            error: function (errormessage) {
                waitingDialog.hideloading();
            }
        });
    });


    $(".list-rekanan-klarifikasi-penilaian").on("click", ".checkbox-pilih-pemenang", function () {        
        if ($(this).attr("checked")) {
            $(this).prop('checked', true);
            return false;
        }
        $(".checkbox-pilih-pemenang").prop('checked',false);
        var elTHis = $(this);
        var objData = {};
        objData.PengadaanId = $("#pengadaanId").val();
        objData.VendorId = $(this).attr("vendorid");
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/addPemenang",
            dataType: "json",
            data: JSON.stringify(objData),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                elTHis.prop('checked', true);
                waitingDialog.hideloading();
                if (data.Id==0) {
                    BootstrapDialog.show({
                        title: 'Konfirmasi',
                        message: 'Anda Tidak Memiliki Akses!',
                        buttons: [{
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
                    });
                }
            },
            error: function (errormessage) {
                waitingDialog.hideloading();
                BootstrapDialog.show({
                    title: 'Error',
                    message: errormessage,
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();

                        }
                    }]
                });
            }
        });
    });

    generateUndangan();
    getKlarifikasi();
    getPemenang();
    getListSubmitKlarifikasiRekanan();
    getListKlarifikasiRekanan();
    getAllKandidatPengadaan();
});

$(function () {
    $(".dibatalkan").on("click", function () {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: '<p>Apa Anda Yakin Ingin Membatalkan Pengadaan ini!</p><p>Keterangan:</p><textarea class="form-control" id="batal_keterangan"  rows="5"></textarea>',
            buttons: [{
                label: 'Lanjutkan',
                action: function (dialog) {
                    if ($("#batal_keterangan").val() == "") {
                        BootstrapDialog.show({
                            title: 'Konfirmasi',
                            message: 'Anda Harus Mengisi Keterangan!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    dialog.close();
                                }
                            }]
                        });
                    }
                    else {
                        batalkanPengadaan($("#batal_keterangan").val());
                        dialog.close();
                    }
                }
            }, {
                label: 'Batal',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
    });
});

function batalkanPengadaan(keterangan) {
    waitingDialog.showloading("Proses Harap Tunggu");
    var odata = {};
    odata.PengadaanId = $("#pengadaanId").val();
    odata.Keterangan = keterangan;
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/pembatalan",
        dataType: "json",
        data: JSON.stringify(odata),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            waitingDialog.hideloading();
            if (data == "0") {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Anda Tidak Memiliki Akses!',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
            } else {
                window.location.replace("http://" + window.location.host + "/pengadaan-list.html");
            }
        },
        error: function (errormessage) {
            waitingDialog.hideloading();
            BootstrapDialog.show({
                title: 'Error',
                message: errormessage,
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

function downloadFileUsingForm(url) {
    var form = document.createElement("form");
    form.method = "post";
    form.action = url;
    document.body.appendChild(form);
    form.submit();
    document.body.removeChild(form);
}

function getAanwijzing() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetPelaksanaanAanwijzings?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $("#aanwijzing_pelaksanaan").val(moment(data.Mulai).format("DD MMMM YYYY HH:mm"));
            $("#aanwijzing_aktual").html("( " + moment(data.Mulai).format("DD MMMM YYYY HH:mm") + " )");
            loadData($("#pengadaanId").val());
            generateUndangan();
            //if (data.Id != "00000000-0000-0000-0000-000000000000") {
            //    if (isGuid(data.Id)) {
            //        $("#aanwijzingPId").val(data.Id);
            //    }
            //}
        },
        error: function (errormessage) {
            alert("gagal");
        }
    });

}

function cekPerubahanJadwal(jadwal1, jadwal2) {
    var thisJadwal = moment($(jadwal1).val(), ["D MMMM YYYY HH:mm"], "id");
    var nextJadwal = moment($(jadwal2).val(), ["D MMMM YYYY HH:mm"], "id");
    var diff = nextJadwal.diff(thisJadwal);
    return diff > 0 ? 1 : 0;
}

function rubahDateAanwijzing() {
    var objPAanwijzing = {};
    objPAanwijzing.Mulai = moment($("#aanwijzing_pelaksanaan").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");//"dd/MM/yyyy"
    objPAanwijzing.PengadaanId = $("#pengadaanId").val();
    if (cekPerubahanJadwal("#aanwijzing_pelaksanaan", "#tgl_pengisian_harga_re") == 0) {       
        BootstrapDialog.show({
            title: 'Konfirmasi',
            //message: 'Jadwal yang Anda Masukan Melebih Jadwal Berikutnya!!<br/> Apakah Andah Ingin Merubah Jadwal Berikutnya Juga?',
            message: 'Jadwal ini Tidak Boleh Lebih dari Jadwal Berikutnya!',
            buttons: [ {
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        getAanwijzing();
        return false;
    }

    waitingDialog.showloading("Proses Harap Tunggu");    
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/saveAanwjzing",
        dataType: "json",
        data: JSON.stringify(objPAanwijzing),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            waitingDialog.hideloading();
            getAanwijzing();
            if (data.Id == "00000000-0000-0000-0000-000000000000") {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Anda Tidak Memiliki Akses!',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
            }
        },
        error: function (errormessage) {
            waitingDialog.hideloading();
            BootstrapDialog.show({
                title: 'Error',
                message: errormessage,
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

function rubahDateSubmitPenawaran() {
    var objSubmitPenawaran = {};
    objSubmitPenawaran.Mulai = moment($("#tgl_pengisian_harga_re").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");//"dd/MM/yyyy"
    objSubmitPenawaran.Sampai = moment($("#tgl_pengisian_harga_sampai_re").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    objSubmitPenawaran.PengadaanId = $("#pengadaanId").val();
    if (cekPerubahanJadwal("#tgl_pengisian_harga_re", "#tgl_pengisian_harga_sampai_re") == 0) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Jadwal ini Tidak Boleh Lebih dari Jadwal Berikutnya!',
            buttons: [{
                label: 'Close',
                action: function (dialog) {                    
                    dialog.close();
                }
            }]
        });
        getDateSubmitPenawaran();
        return false;
    }
    if (cekPerubahanJadwal("#tgl_pengisian_harga_sampai_re", "#buka_amplop_sampai_re") == 0) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Jadwal ini Tidak Boleh Lebih dari Jadwal Berikutnya!',
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        getDateSubmitPenawaran();
        return false;
    }
    waitingDialog.showloading("Proses Harap Tunggu");

    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/updateSubmitPenawran",
        dataType: "json",
        data: JSON.stringify(objSubmitPenawaran),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            waitingDialog.hideloading();
            getDateSubmitPenawaran();
            if (data.Id == "00000000-0000-0000-0000-000000000000") {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Anda Tidak Memiliki Akses!',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
            }
        },
        error: function (errormessage) {
            waitingDialog.hideloading();
            BootstrapDialog.show({
                title: 'Error',
                message: errormessage,
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

function getDateSubmitPenawaran() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetSubmitPenawran?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $("#tgl_pengisian_harga_re").val(moment(data.Mulai).format("DD MMMM YYYY HH:mm"));
            $("#tgl_pengisian_harga_sampai_re").val(moment(data.Sampai).format("DD MMMM YYYY HH:mm"));
            $("#pengisian_harga_aktual").html("( " + moment(data.Mulai).format("DD MMMM YYYY HH:mm") + " s/d " + moment(data.Sampai).format("DD MMMM YYYY HH:mm") + " )");
            //if (data.Id != "00000000-0000-0000-0000-000000000000") {
            //    if (isGuid(data.Id)) {
            //        $("#aanwijzingPId").val(data.Id);
            //    }
            //}
        },
        error: function (errormessage) {
            alert("gagal");
        }
    });
}

function rubahDateBukaAmplop() {
    var obj = {};
    obj.Sampai = moment($("#buka_amplop_sampai_re").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");//"dd/MM/yyyy"
    obj.Mulai = moment($("#buka_amplop_re").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    obj.PengadaanId = $("#pengadaanId").val();
    if (cekPerubahanJadwal("#buka_amplop_re", "#buka_amplop_sampai_re") == 0) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Jadwal ini Tidak Boleh Lebih dari Jadwal Berikutnya!',
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        getBukaAmplop();
        return false;
    }
    if (cekPerubahanJadwal("#buka_amplop_sampai_re", "#jadwal_penilaian_kandidat") == 0) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Jadwal ini Tidak Boleh Lebih dari Jadwal Berikutnya!',
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        getBukaAmplop();
        return false;
    }
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/updateBukaAmplop",
        dataType: "json",
        data: JSON.stringify(obj),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            getBukaAmplop();
            waitingDialog.hideloading();
            if (data.Id == "00000000-0000-0000-0000-000000000000") {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Anda Tidak Memiliki Akses!',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
            }
        },
        error: function (errormessage) {
            waitingDialog.hideloading();
            BootstrapDialog.show({
                title: 'Error',
                message: errormessage,
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

function getBukaAmplop() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetBukaAmplop?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $("#buka_amplop_re").val(moment(data.Mulai).format("DD MMMM YYYY HH:mm"));
            $("#buka_amplop_sampai_re").val(moment(data.Sampai).format("DD MMMM YYYY HH:mm"));
            $("#buka_amplop_aktual").html("( " + moment(data.Mulai).format("DD MMMM YYYY HH:mm") + " s/d " + moment(data.Sampai).format("DD MMMM YYYY HH:mm") + " )");
            if (data.Id != "00000000-0000-0000-0000-000000000000") {
                //if (isGuid(data.Id)) {
                //    $("#aanwijzingPId").val(data.Id);
                //}
            }
        },
        error: function (errormessage) {
            alert("gagal");
        }
    });
}

function rubahPenilaian() {
    var obj = {};
    obj.Mulai = moment($("#jadwal_penilaian_kandidat").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");//"dd/MM/yyyy"
    obj.Sampai = moment($("#jadwal_penilaian_kandidat_sampai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    obj.PengadaanId = $("#pengadaanId").val();
    if (cekPerubahanJadwal("#jadwal_penilaian_kandidat", "#jadwal_penilaian_kandidat_sampai") == 0) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Jadwal ini Tidak Boleh Lebih dari Jadwal Berikutnya!',
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        getPenilaian();
        return false;
    }
    if (cekPerubahanJadwal("#jadwal_penilaian_kandidat_sampai", "#jadwal_pelaksanaan_klarifikasi") == 0) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Jadwal ini Tidak Boleh Lebih dari Jadwal Berikutnya!',
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        getPenilaian();
        return false;
    }
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/updatePenilaian",
        dataType: "json",
        data: JSON.stringify(obj),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            //$("#aanwijzing_pelaksanaan").val(moment(data.Mulai).format("DD/MM/YYYY"));
            getPenilaian();
            waitingDialog.hideloading();
            if (data.Id == "00000000-0000-0000-0000-000000000000") {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Anda Tidak Memiliki Akses!',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();

                        }
                    }]
                });
            }
        },
        error: function (errormessage) {
            waitingDialog.hideloading();
            BootstrapDialog.show({
                title: 'Error',
                message: 'Error!',
                buttons: [{
                    label: errormessage,
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
        }
    });
}

function getPenilaian() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetPenilaian?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $("#jadwal_penilaian_kandidat").val(moment(data.Mulai).format("DD MMMM YYYY HH:mm"));
            $("#jadwal_penilaian_kandidat_sampai").val(moment(data.Sampai).format("DD MMMM YYYY HH:mm"));
            $("#penilaian_aktual").html("( " + moment(data.Mulai).format("DD MMMM YYYY HH:mm") + " s/d " + moment(data.Sampai).format("DD MMMM YYYY HH:mm") + " )");
            if (data.Id != "00000000-0000-0000-0000-000000000000") {
                //if (isGuid(data.Id)) {
                //    $("#aanwijzingPId").val(data.Id);
                //}
            }
        },
        error: function (errormessage) {
            alert("gagal");
        }
    });
}

function getKlarifikasi() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetKlarifikasi?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $("#jadwal_pelaksanaan_klarifikasi").val(moment(data.Mulai).format("DD MMMM YYYY HH:mm"));
            $("#jadwal_pelaksanaan_klarifikasi_sampai").val(moment(data.Sampai).format("DD MMMM YYYY HH:mm"));
            $("#klarifikasi_aktual").html("( " + moment(data.Mulai).format("DD MMMM YYYY HH:mm") + " s/d " + moment(data.Sampai).format("DD MMMM YYYY HH:mm") + " )");
            if (data.Id != "00000000-0000-0000-0000-000000000000") {
                //if (isGuid(data.Id)) {
                //    $("#aanwijzingPId").val(data.Id);
                //}
            }
            generateUndanganKlarifikasi();
        },
        error: function (errormessage) {
            alert("gagal");
        }
    });
}

function rubahKlarifikasi() {
    var obj = {};
    obj.Mulai = moment($("#jadwal_pelaksanaan_klarifikasi").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");//"dd/MM/yyyy"
    obj.Sampai = moment($("#jadwal_pelaksanaan_klarifikasi_sampai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    obj.PengadaanId = $("#pengadaanId").val();
    
    if (cekPerubahanJadwal("#jadwal_pelaksanaan_klarifikasi", "#jadwal_pelaksanaan_klarifikasi_sampai") == 0) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Jadwal ini Tidak Boleh Lebih dari Jadwal Berikutnya!',
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        getKlarifikasi();
        return false;
    }
    if (cekPerubahanJadwal("#jadwal_pelaksanaan_klarifikasi_sampai", "#jadwal_pelaksanaan_pemenang") == 0) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Jadwal ini Tidak Boleh Lebih dari Jadwal Berikutnya!',
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        getKlarifikasi();
        return false;
    }
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/updateKlarifikasi",
        dataType: "json",
        data: JSON.stringify(obj),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            waitingDialog.hideloading();
            getKlarifikasi();
            if (data.Id == "00000000-0000-0000-0000-000000000000") {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Anda Tidak Memiliki Akses!',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
            }
        },
        error: function (errormessage) {
            waitingDialog.hideloading();
            BootstrapDialog.show({
                title: 'Error',
                message: errormessage,
                buttons: [{
                    label: errormessage,
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
        }
    });
}

function getPemenang() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetPemenang?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $("#jadwal_pelaksanaan_pemenang").val(moment(data.Mulai).format("DD MMMM YYYY HH:mm"));
            $("#pemenang_aktual").html("( " + moment(data.Mulai).format("DD MMMM YYYY HH:mm") + " )");            
        },
        error: function (errormessage) {
            alert("gagal");
        }
    });
}

function rubahPemenang() {
    var obj = {};
    obj.Mulai = moment($("#jadwal_pelaksanaan_pemenang").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
    obj.PengadaanId = $("#pengadaanId").val();   
    if (cekPerubahanJadwal( "#jadwal_pelaksanaan_klarifikasi_sampai","#jadwal_pelaksanaan_pemenang") == 0) {
        BootstrapDialog.show({
            title: 'Konfirmasi',
            message: 'Jadwal ini Tidak Boleh Kecil Lebih dari Jadwal Sebelumnya!',
            buttons: [{
                label: 'Close',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
        getPemenang();
        return false;
    }
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/updatePemenang",
        dataType: "json",
        data: JSON.stringify(obj),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            getPemenang();
            waitingDialog.hideloading();
            if (data.Id == "00000000-0000-0000-0000-000000000000") {
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: 'Anda Tidak Memiliki Akses!',
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
            }
        },
        error: function (errormessage) {
            waitingDialog.hideloading();
            BootstrapDialog.show({
                title: 'Error',
                message: errormessage,
                buttons: [{
                    label: errormessage,
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
        }
    });
}

function getListKandidatPelaksanaan() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/getKehadiranAanwjzing?PengadaanId=" + $("#pengadaanId").val(),
        success: function (data) {            
            $.each(data, function (index, value) {
                var html = '<div class="col-md-3">' +
                      '<div class="box box-primary">' +
                          '<div class="box-tools pull-right vendor-check-box">';
                if (value.hadir == 1)
                    html = html+'<input type="checkbox" class="absen-kandidat" checked VId="' + value.Id + '"/>';
                else html = html+ '<input type="checkbox" class="absen-kandidat"  VId="' + value.Id + '"/>';
                html=html+'</div>'+
                       '<div class="box-body box-profile">'+
                            '<p class="profile-username title-header">' + value.NamaVendor +
                            '<p class="text-muted text-center deskripsi">' + value.Telp +
                        '</div>'+
                    '</div>'+
                    '</div>';
              $(".kehadiran-kandidat").append(html);
            });
        }
    });
}

function getListSubmitRekanan() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetRekananSubmit?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $.each(data, function (index, value) {
                var html = '<div class="col-md-4">' +
                    '<div class="box box-folder-vendor vendor-detail" attrId="' + value.VendorId + '">' +
                        '<div class="box-header with-border">';
                if (value.status == 0) {
                    html = html + '<span class="glyphicon glyphicon glyphicon-folder-open folder-vendor folder-kosong" ></span> <h3 class="box-title text-blue-mtf box-riwayat">' + value.NamaVendor + '</h3>';
                }
                else {
                    html = html + '<span class="glyphicon glyphicon glyphicon-folder-open folder-vendor folder-isi" ></span><h3 class="box-title text-blue-mtf box-riwayat">' + value.NamaVendor + '</h3>';
                }
                html=html+'</div>' +
                    '</div>' +
                '</div> ';
                $(".list-submit-rekanan").append(html);
            });
        }
    });
}

function getPersetujuanBukaAmplop() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/getPersetujuanBukaAmplop?PengadaanId=" + $("#pengadaanId").val(),
        success: function (data) {
            $.each(data, function (index, value) {
                $('.persetujuan-buka-amplop[attr1="' + value.tipe + '"]').addClass("btn-success");
                $('.persetujuan-buka-amplop[attr1="' + value.tipe + '"]').removeClass("btn-danger");
                $('.persetujuan-buka-amplop[attr1="' + value.tipe + '"]').children().addClass("glyphicon-ok");
                $('.persetujuan-buka-amplop[attr1="' + value.tipe + '"]').children().removeClass("glyphicon-pushpin");
            });
            $(".persetujuan-buka-amplop").removeAttr("disabled");
        }
    });
}

function getListPenilainRekanan() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetRekananPenilaian?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $.each(data, function (index, value) {
                var html = '<div class="col-md-3">' +
                            '<div class="box box-primary" data-toggle="tooltip" title="' + value.NamaVendor + '">' +
                                '<div class="box-tools pull-right vendor-check-box">';
                if (value.terpilih == 0)
                    html = html + '<input class="s-checkbox checkbox-pilih-kandidat"  vendorId="' + value.VendorId + '" data-idx="0" type="checkbox" value="" />';
                else html = html + '<input class="s-checkbox checkbox-pilih-kandidat" checked vendorId="' + value.VendorId + '" data-idx="0" type="checkbox" value="" />';
                html = html+'</div>' +
                                '<div class="box-body box-profile box-folder-vendor box-klarifikasi-rekanan" vendorId="' + value.VendorId + '">' +
                                    '<p class="profile-username title-header title-header-vendor">' + value.NamaVendor + '</p>' +
                                    '<p class="text-muted text-center deskripsi">' + accounting.formatNumber(value.total, { thousand: "." }) + '</p>' +
                                    '<p class="text-muted text-center deskripsi">Nilai Kriteria ' + (value.NilaiKriteria == null ? 0 : value.NilaiKriteria) + '</p>' +
                                '</div>' +
                            '</div>' +
                        '</div>';
                $(".list-rekanan-penilaian").append(html);
            });
        }
    });
}

function generateUndangan() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetPelaksanaanAanwijzings?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            var html = "Panitia " + $("#judul").text() + " Untuk " + $("#UnitKerjaPemohon").text() +
            " Menerangkan bahwa pada tanggal " + moment(data.Mulai).format("DD MMMM YYYY") + " pukul " + moment(data.Mulai).format("HH:mm") + " menyelenggarakan Rapat Pemberian Penjelasan dan Peninjauan Lokasi mengenai " +
            $("#judul").text() + " Untuk " + $("#UnitKerjaPemohon").text();
            var div = $("#undangan").val(html);
        },
        error: function (errormessage) {
            alert("gagal");
        }
    });
}

function generateUndanganKlarifikasi() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetKlarifikasi?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            var html = "Panitia " + $("#judul").text() + " Untuk " + $("#UnitKerjaPemohon").text() +
            ". Mohon untuk Klarifikasi harga, penawaran kami tunggu paling lambat " + moment(data.Sampai).format("DD MMMM YYYY") + " sebelum pukul " + moment(data.Sampai).format("HH:mm") + "\n" +
            "Demikian kami sampaikan. Terimakasi atas perhatiannya serta kerjasamanya.";
            var div = $("#mKlarifikasi").val(html);
        },
        error: function (errormessage) {
            alert("gagal");
        }
    });
}

function getListSubmitKlarifikasiRekanan() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetRekananKlarifikasiSubmit?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $.each(data, function (index, value) {
                var html = '<div class="col-md-4 box-klarifikasi-rekanan" vendorId="' + value.VendorId + '">' +
                    '<div class="box box-folder-vendor vendor-detail" attrId="' + value.VendorId + '">' +
                        '<div class="box-header with-border">';
                if (value.status == 0) {
                    html = html + '<span class="glyphicon glyphicon glyphicon-folder-open folder-vendor folder-kosong" ></span> <h3 class="box-title text-blue-mtf box-riwayat">' + value.NamaVendor + '</h3>';
                }
                else {
                    html = html + '<span class="glyphicon glyphicon glyphicon-folder-open folder-vendor folder-isi" ></span><h3 class="box-title text-blue-mtf box-riwayat">' + value.NamaVendor + '</h3>';
                }
                html = html + '</div>' +
                    '</div>' +
                '</div> ';
                $(".list-submit-klarifikasi-rekanan").append(html);
            });
        }
    });
}

function getListKlarifikasiRekanan() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetRekananKlarifikasiPenilaian?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $.each(data, function (index, value) {
                var html = '<div class="col-md-3 "  vendorId="' + value.VendorId + '">' +
                            '<div class="box box-primary">' +
                                '<div class="box-tools pull-right vendor-check-box" data-toggle="tooltip" title="' + value.NamaVendor + '">';
                       //'<button class="delete-klarifikasi-kandidat btn btn-box-tool" vendorId="' + value.VendorId + '"><i class="fa fa-times"></i></button>' +
                       
                if (value.terpilih == 0) {
                    if ($("#isPIC").val() == "1")
                        html = html + '<input class="s-checkbox checkbox-pilih-pemenang"  vendorId="' + value.VendorId + '" data-idx="0" type="checkbox" value="" />';
                    else html = html + '<input class="s-checkbox checkbox-pilih-pemenang" disabled="disabled"  vendorId="' + value.VendorId + '" data-idx="0" type="checkbox" value="" />';
                }
                else {
                    if ($("#isPIC").val() == "1")
                         html = html + '<input class="s-checkbox checkbox-pilih-pemenang" checked vendorId="' + value.VendorId + '" data-idx="0" type="checkbox" value="" />';
                    else html = html + '<input class="s-checkbox checkbox-pilih-pemenang" checked disabled="disabled"  vendorId="' + value.VendorId + '" data-idx="0" type="checkbox" value="" />';
                }
                html = html + '</div>' +
                                '<div class="box-body box-profile">' +
                                    '<p class="profile-username title-header title-header-vendor">' + value.NamaVendor + '</p>' +
                                    '<p class="text-muted text-center deskripsi">Rp. ' + accounting.formatNumber(value.total, { thousand: "." }) + '</p>' +
                                     '<p class="text-muted text-center deskripsi">Nilai Kriteria ' + value.NilaiKriteria + '</p>' +
                                '</div>' +
                            '</div>' +
                        '</div>';
                $(".list-rekanan-klarifikasi-penilaian").append(html);
            });
        }
    });
}

function getKandidatPemenang() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetPemenangPengadaan?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $.each(data, function (index, value) {
                var html = '<div class="col-md-3  box-klarifikasi-rekanan"  vendorId="' + value.VendorId + '">' +
                            '<div class="box box-primary box-folder-vendor" data-toggle="tooltip" title="' + value.NamaVendor + '">' +
                                '<div class="box-tools pull-right vendor-check-box">' +
                       //'<button class="delete-klarifikasi-kandidat btn btn-box-tool" vendorId="' + value.VendorId + '"><i class="fa fa-times"></i></button>' +
                     '</div>' +
                                '<div class="box-body box-profile">' +
                                    '<p class="profile-username title-header title-header-vendor">' + value.NamaVendor + '</p>' +
                                    '<p class="text-muted text-center deskripsi">Rp. ' + accounting.formatNumber(value.total, { thousand: "." }) + '</p>' +
                                    '<p class="text-muted text-center deskripsi">Nilai: ' + value.NilaiKriteria + '</p>' +
                                '</div>' +
                            '</div>' +
                        '</div>';
                $(".list-rekanan-pemenang").append(html);
            });
        }
    });
}

function getAllKandidatPengadaan() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetAllKandidatPengadaan?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $.each(data, function (index, value) {
                var html = '<div class="col-md-3  box-klarifikasi-rekanan"  vendorId="' + value.VendorId + '">' +
                            '<div class="box box-primary box-folder-vendor">' +
                                '<div class="box-tools pull-right vendor-check-box">' +
                       //'<button class="delete-klarifikasi-kandidat btn btn-box-tool" vendorId="' + value.VendorId + '"><i class="fa fa-times"></i></button>' +
                     '</div>' +
                                '<div class="box-body box-profile">' +
                                    '<p class="profile-username title-header">' + value.NamaVendor + '</p>' +
                                    '<p class="text-muted text-center deskripsi">Rp. ' + accounting.formatNumber(value.total, { thousand: "." }) + '</p>' +
                                    '<p class="text-muted text-center deskripsi">Bobot Nilai ' + value.NilaiKriteria+ '</p>' +
                                '</div>' +
                            '</div>' +
                        '</div>';
                $(".list-berkas-info-kandidat").append(html);
            });
        }
    });
}

function nextState(state) {
        $.ajax({
            method: "POST",
            url: "Api/PengadaanE/nextToState?Id=" + $("#pengadaanId").val() + "&tipe="+state,
            success: function (data) {
                if (data == 1) {
                    window.location.reload();
                }
            },
            error: function (errormessage) {
                alert("gagal");
            }
        });
}

function isSpkUploaded() {
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








































//", dengan penjelasan sebagai berikut:";
//    "\n" +
//    "1. Rapat dihadiri oleh:" +
//    "\n" +
//    "     1.1. Unsur Paniti terdiri dari:" +
//     "\n";
//var personilhtml = "         ";
//var i = 1;
//$.each(personils, function (index, value) {

//    if (value.tipe == "pic" || value.tipe == "tim") {
//        if(i==1)
//            personilhtml = "        " + i + ". " + value.Nama ;
//        else 
//            personilhtml = personilhtml + "\n        " + i + ". " + value.Nama + "\n";
//        i++;
//    }
//});

//html =html+ personilhtml + "\n";

//html = html + "     1.2. Unsur Rekanan terdiri dari:"
//var kandidat = "         ";
//var i = 1;
//$.each(Kandidats, function (index, value) {

//    if (value.tipe == "pic" || value.tipe == "tim") {
//        if (i == 1)
//            Kandidats = "        " + i + ". " + value.Nama;
//        else
//            Kandidats = Kandidats + "\n        " + i + ". " + value.Nama + "\n";
//        i++;
//    }
//});
//html = html + Kandidats + "\n";

//html = html + "Demikian Berita Acara ini dibuat";