
$(function () {
    $(".dateJadwal").datetimepicker({
        format: "DD MMMM YYYY HH:mm",
        locale: 'id',
        useCurrent: false,
        minDate: Date.now()

    });
   
    var myDropzoneBeritaAcaraAanwijzing = new Dropzone("#BeritaAcaraAanwijzing",
            {
                url: $("#BeritaAcaraAanwijzing").attr("action") + "&id=" + $("#pengadaanId").val(),
                maxFilesize: 10,
                acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx,.docx",
                accept: function (file, done) {
                    if ($("#isPIC").val() == 1) {
                        //if ($("#State").val() == 2 || $("#State").val() == 3)
                        //    BootstrapDialog.show({
                        //        title: 'Konfirmasi',
                        //        message: 'Dengan mengklik "Lanjutkan" Berarti State akan Berlanjut Ke state Berikutnya!',
                        //        buttons: [{
                        //            label: 'Lanjutkan',
                        //            action: function (dialog) {
                        //                done();
                        //                //nextState("pengisian_harga");
                        //                dialog.close();
                        //            }
                        //        }, {
                        //            label: 'Batal',
                        //            action: function (dialog) {
                        //                myDropzoneBeritaAcaraAanwijzing.removeFile(file);
                        //                dialog.close();

                        //            }
                        //        }]
                        //    });
                        //else {
                          //  if ($("#State").val() > 3) {
                                done();
                          //  }
                       // }
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
                    this.on("success", function (file, responseText) {
                        //if ($("#State").val() == 2 || $("#State").val() == 3)
                            //nextState("pengisian_harga");
                    });
                    //this.on("removedfile", function (file, responseText) {
                    //    myDropzoneBeritaAcaraBukaAmplop.removeFile(file);
                    //});
                }
            }
        );

    renderDokumenDropzone(myDropzoneBeritaAcaraAanwijzing, "BeritaAcaraAanwijzing");
    Dropzone.options.BeritaAcaraAanwijzing = false;

    var myDropzoneBerkasBeritaAcaraAanwijzing = new Dropzone("#BerkasBeritaAcaraAanwijzing",
           {
               url: $("#BerkasBeritaAcaraAanwijzing").attr("action") + "&id=" + $("#pengadaanId").val(),
               maxFilesize: 10,
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
                maxFilesize: 10,
                acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.docx,.xlsx",
                accept: function (file, done) {
                    var jumFile = myDropzoneBeritaAcaraAanwijzing.files.length;
                    //if (jumFile > 1) {
                    //    BootstrapDialog.show({
                    //        title: 'Konfirmasi',
                    //        message: 'Berkas Sudah Adda',
                    //        buttons: [{
                    //            label: 'Close',
                    //            action: function (dialog) {
                    //                myDropzoneBeritaAcaraBukaAmplop.removeFile(file);
                    //                dialog.close();
                    //            }
                    //        }]
                    //    });
                    //}
                    //else {
                        var cekBukaAmplop = 1;
                        $(".persetujuan-buka-amplop").each(function () {
                            if ($(this).hasClass("btn-danger")) {
                                cekBukaAmplop = 0;
                                return false;
                            }
                        });
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

                        if ($("#isPIC").val() == 1 && cekBukaAmplop == 1) {
                            if ($("#State").val() == 5)
                                //BootstrapDialog.show({
                                //    title: 'Konfirmasi',
                                //    message: 'Dengan mengklik "Lanjutkan" Berarti State akan Berlanjut Ke state Berikutnya!',
                                //    buttons: [{
                                //        label: 'Lanjutkan',
                                //        action: function (dialog) {
                                           done();
                                //            //nextState("penilaian");
                                //            dialog.close();
                                //        }
                                //    }, {
                                //        label: 'Batal',
                                //        action: function (dialog) {
                                //            myDropzoneBeritaAcaraBukaAmplop.removeFile(file);
                                //            dialog.close();

                                //        }
                                //    }]
                                //});
                            else {
                                if ($("#State").val() > 5) {
                                    done();
                                }
                            }
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
                   // }
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
                    this.on("success", function (file, responseText) {
                        //if ($("#State").val() == 5)
                            //nextState("penilaian");
                    });
                    //this.on("removedfile", function (file, responseText) {
                    //    myDropzoneBeritaAcaraBukaAmplop.removeFile(file);
                    //});
                }
            }
        );

    renderDokumenDropzone(myDropzoneBeritaAcaraBukaAmplop, "BeritaAcaraBukaAmplop");
    Dropzone.options.BeritaAcaraBukaAmplop = false;
    
    var myDropzoneBerkasBeritaAcaraBukaAmplop = new Dropzone("#BerkasBeritaAcaraBukaAmplop",
          {
              url: $("#BerkasBeritaAcaraBukaAmplop").attr("action") + "&id=" + $("#pengadaanId").val(),
              maxFilesize: 10,
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
               maxFilesize: 10,
               acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
               accept: function (file, done) {
                   var jumFile = myDropzoneBeritaAcaraKlarifikasi.files.length;
                   //if (jumFile > 1) {
                   //    BootstrapDialog.show({
                   //        title: 'Konfirmasi',
                   //        message: 'Berkas Sudah Adda',
                   //        buttons: [{
                   //            label: 'Close',
                   //            action: function (dialog) {
                   //                myDropzoneBeritaAcaraKlarifikasi.removeFile(file);
                   //                dialog.close();
                   //            }
                   //        }]
                   //    });
                   //} else {
                       if ($("#isPIC").val() == 1) {
                           var cekRekananCheck = 0;
                           $(".checkbox-pilih-pemenang").each(function () {
                               if ($(this).prop('checked') == true) {
                                   cekRekananCheck = cekRekananCheck + 1;
                               }
                           });
                           if (cekRekananCheck > 0) {
                               if ($("#State").val() == 7) {
                                   //BootstrapDialog.show({
                                   //    title: 'Konfirmasi',
                                   //    message: 'Dengan mengklik "Lanjutkan" Berarti State akan Berlanjut Ke state Berikutnya!',
                                   //    buttons: [{
                                   //        label: 'Lanjutkan',
                                   //        action: function (dialog) {
                                              done();
                                   //            // nextState("penentuan_pemenang");
                                   //            dialog.close();
                                   //        }
                                   //    }, {
                                   //        label: 'Batal',
                                   //        action: function (dialog) {
                                   //            myDropzoneBeritaAcaraKlarifikasi.removeFile(file);
                                   //            dialog.close();

                                   //        }
                                   //    }]
                                   //});
                               }
                               else if ($("#State").val() > 7) {
                                   done();
                               }
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

                 //  }
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
                   this.on("success", function (file, responseText) {
                      // if($("#State").val()==7)
                         //nextState("penentuan_pemenang");
                   });
                   //this.on("removedfile", function (file, responseText) {
                   //    myDropzoneBeritaAcaraBukaAmplop.removeFile(file);
                   //});
               }
           }
       );

    renderDokumenDropzone(myDropzoneBeritaAcaraKlarifikasi, "BeritaAcaraKlarifikasi");
    Dropzone.options.BeritaAcaraKlarifikasi = false;

    var myDropzoneBeritaAcaraKlarifikasiLanjutan = new Dropzone("#BeritaAcaraKlarifikasiLanjutan",
           {
               url: $("#BeritaAcaraKlarifikasiLanjutan").attr("action") + "&id=" + $("#pengadaanId").val(),
               maxFilesize: 10,
               acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
               accept: function (file, done) {
                   var jumFile = myDropzoneBeritaAcaraKlarifikasiLanjutan.files.length;
                   if ($("#isPIC").val() == 1) {
                       var cekRekananCheck = 0;
                       $(".checkbox-pilih-pemenang").each(function () {
                           if ($(this).prop('checked') == true) {
                               cekRekananCheck = cekRekananCheck + 1;
                           }
                       });
                       if (cekRekananCheck > 0) {
                           if ($("#State").val() == 12) {
                               done();
                           }
                       }
                       else {
                           BootstrapDialog.show({
                               title: 'Konfirmasi',
                               message: 'Anda Belum Memilih Kandidat Pemenang',
                               buttons: [{
                                   label: 'Close',
                                   action: function (dialog) {
                                       myDropzoneBeritaAcaraKlarifikasiLanjutan.removeFile(file);
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
                                   myDropzoneBeritaAcaraKlarifikasiLanjutan.removeFile(file);
                                   dialog.close();
                               }
                           }]
                       });
                   }

                   //  }
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
                           $("#konfirmasiFile").attr("attr1", "BeritaAcaraKlarifikasiLanjutan");
                           $("#konfirmasiFile").attr("FileId", id);
                           $("#konfirmasiFile").modal("show");
                       });
                   });
                   this.on("success", function (file, responseText) {
                       // if($("#State").val()==7)
                       //nextState("penentuan_pemenang");
                   });
                   //this.on("removedfile", function (file, responseText) {
                   //    myDropzoneBeritaAcaraBukaAmplop.removeFile(file);
                   //});
               }
           }
       );

    renderDokumenDropzone(myDropzoneBeritaAcaraKlarifikasiLanjutan, "BeritaAcaraKlarifikasiLanjutan");
    Dropzone.options.BeritaAcaraKlarifikasiLanjutan = false;
    

    var myDropzoneBerkasBeritaAcaraKlarifikasi = new Dropzone("#BerkasBeritaAcaraKlarifikasi",
         {
             url: $("#BerkasBeritaAcaraKlarifikasi").attr("action") + "&id=" + $("#pengadaanId").val(),
             maxFilesize: 10,
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
               maxFilesize: 10,
               acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
               accept: function (file, done) {
                   var jumFile = myDropzoneBeritaAcaraPenentuanPemenang.files.length;
                   var dtPemenang = jQuery.parseJSON($("#Pemenang").val());
                   var jumPemenang = dtPemenang.length;

                   if (jumFile > jumPemenang) {
                       BootstrapDialog.show({
                           title: 'Konfirmasi',
                           message: 'Berkas Sudah Ada Untuk ' + jumPemenang + ' Pemenang',
                           buttons: [{
                               label: 'Close',
                               action: function (dialog) {
                                   myDropzoneBeritaAcaraPenentuanPemenang.removeFile(file);
                                   dialog.close();
                               }
                           }]
                       });
                   } else {
                       var that = this;
                       var html = "<select class='form-control'>";
                       for (var key in dtPemenang) {
                           html += '<option class="form-control" value="' + dtPemenang[key].VendorId + '">' + dtPemenang[key].NamaVendor + '</option>';
                       }
                       html += "</select>";
                       BootstrapDialog.show({
                           message: 'Pilih Vendor :' + html,
                           onhide: function (dialogRef) {
                               var VendorId = dialogRef.getModalBody().find('select').val();
                               that.options.url = $("#BeritaAcaraPenentuanPemenang").attr("action") + "&id=" + $("#pengadaanId").val() + "&vendorId=" + VendorId; //that.options.url + "&vendorId=" + VendorId;
                           },
                           buttons: [{
                               label: 'Simpan',
                               action: function (dialogRef) {
                                   done();
                                   dialogRef.close();
                               }
                           }, {
                               label: 'Close',
                               action: function (dialogRef) {
                                   myDropzoneBeritaAcaraPenentuanPemenang.removeFile(file);
                                   dialogRef.close();
                               }
                           }]
                       });
                   }
               },
               init: function () {
                   //console.log(this);
                   this.on("processing", function (file) {
                       //this.options.url = "";
                       //console.log(this);
                       //console.log(this.options.url);
                   });
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
                   this.on("error", function (file) {
                       myDropzoneBeritaAcaraPenentuanPemenang.removeFile(file);
                   });
                   this.on("success", function (file, responseText) {
                       if (responseText == "00000000-0000-0000-0000-000000000000" || responseText == null) {
                           myDropzoneBeritaAcaraPenentuanPemenang.removeFile(file);
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
                 maxFilesize: 10,
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

   
    var myDropzoneSuratPerintahKerja = new Dropzone("#SuratPerintahKerja",
             {
                 url: $("#SuratPerintahKerja").attr("action") + "&id=" + $("#pengadaanId").val(),
                 maxFilesize: 10,
                 acceptedFiles: ".png,.jpg,.pdf,.xls,.jpeg,.doc,.xlsx",
                 accept: function (file, done) {
                     var jumFile = myDropzoneSuratPerintahKerja.files.length;
                     //jumFile > 1 &&
                     if ($("#isPemenangApproved").val() != 1) {

                         BootstrapDialog.show({
                             title: 'Konfirmasi',
                             message: 'Dokumen Persetujan Pemenang Belum DiSetujui',
                             buttons: [{
                                 label: 'Close',
                                 action: function (dialog) {
                                     myDropzoneSuratPerintahKerja.removeFile(file);
                                     dialog.close();
                                 }
                             }]
                         });
                     } else {
                         var jumFile = myDropzoneSuratPerintahKerja.files.length;
                         var dtPemenang = jQuery.parseJSON($("#Pemenang").val());
                         var jumPemenang = dtPemenang.length;

                         if (jumFile > jumPemenang) {
                             BootstrapDialog.show({
                                 title: 'Konfirmasi',
                                 message: 'Berkas Sudah Lebih dari ' + jumPemenang + ' Pemenang',
                                 buttons: [{
                                     label: 'Close',
                                     action: function (dialog) {
                                         myDropzoneSuratPerintahKerja.removeFile(file);
                                         dialog.close();
                                     }
                                 }]
                             });
                         } else {
                             var that = this;
                             var html = "<select class='form-control'>";
                             for (var key in dtPemenang) {
                                 html += '<option class="form-control" value="' + dtPemenang[key].VendorId + '">' + dtPemenang[key].NamaVendor + '</option>';
                             }
                             html += "</select>";
                             BootstrapDialog.show({
                                 message: 'Pilih Vendor :' + html,
                                 onhide: function (dialogRef) {
                                     var VendorId = dialogRef.getModalBody().find('select').val();
                                     that.options.url = $("#SuratPerintahKerja").attr("action") + "&id=" + $("#pengadaanId").val() + "&vendorId=" + VendorId;
                                 },
                                 buttons: [{
                                     label: 'Simpan',
                                     action: function (dialogRef) {
                                         done();
                                         dialogRef.close();
                                     }
                                 }, {
                                     label: 'Close',
                                     action: function (dialogRef) {
                                         myDropzoneSuratPerintahKerja.removeFile(file);
                                         dialogRef.close();
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
                     this.on("error", function (file) {
                         myDropzoneSuratPerintahKerja.removeFile(file);
                     });
                     this.on("success", function (file, responseText) {
                         if (responseText == "00000000-0000-0000-0000-000000000000" || responseText == null) {
                             myDropzoneSuratPerintahKerja.removeFile(file);
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
              maxFilesize: 10,
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
                        var id;
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
        var StatusBtn = 1;
        if ($(this).attr("attr2") == $(this).attr("attr3")) {
            if ($($(this).attr("attr1")).attr("disabled")) StatusBtn = 0;
            else StatusBtn = 1;
        }
        else if ($($(this).attr("attr1")).attr("disabled") && $($(this).attr("attr3")).attr("disabled")) {
            StatusBtn = 0;
        }
        

        if (StatusBtn==0) {
            var now = moment();
            var TimeMulai = moment($($(this).attr("attr1")).val(), ["D MMMM YYYY HH:mm"], "id");//.format("DD/MM/YYYY HH:mm");
            //if (TimeMulai != "Invalid date") {
            //    if(TimeMulai>now)
                   $($(this).attr("attr1")).removeAttr("disabled");
            //}
            $($(this).attr("attr3")).removeAttr("disabled");
            $(this).html('<i class="fa fa-fw fa-calendar"></i>Submit');
        } else {
            $($(this).attr("attr1")).attr("disabled", "");
            $($(this).attr("attr3")).attr("disabled", "");
            $(this).html('<i class="fa fa-fw fa-calendar"></i>Ubah');
            var data = {};
            data.PengadaanId = $("#pengadaanId").val();
            if ($(this).attr("attr2") == "aanwijzing_pelaksanaan") {
                data.Mulai = moment($("#aanwijzing_pelaksanaan").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");//"dd/MM/yyyy"
                data.status = 3;
            }
            if ($(this).attr("attr2") == "pengisian_harga") {
                data.Mulai = moment($("#tgl_pengisian_harga_re").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");//"dd/MM/yyyy"
                data.Sampai = moment($("#tgl_pengisian_harga_sampai_re").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
                data.status = 4;
            }
            if ($(this).attr("attr2") == "buka_amplop") {
                data.Sampai = moment($("#buka_amplop_sampai_re").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");//"dd/MM/yyyy"
                data.Mulai = moment($("#buka_amplop_re").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
                data.status = 5;
            }
            if ($(this).attr("attr2") == "penilaian_kandidat") {
                data.Mulai = moment($("#jadwal_penilaian_kandidat").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");//"dd/MM/yyyy"
                data.Sampai = moment($("#jadwal_penilaian_kandidat_sampai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
                data.status = 6;
            }
            if ($(this).attr("attr2") == "pelaksanaan-klarifikasi") {
                data.Mulai = moment($("#jadwal_pelaksanaan_klarifikasi").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");//"dd/MM/yyyy"
                data.Sampai = moment($("#jadwal_pelaksanaan_klarifikasi_sampai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
                data.status = 7;
            }
            if ($(this).attr("attr2") == "pelaksanaan-pemenang") {
                data.Mulai = moment($("#jadwal_pelaksanaan_pemenang").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
                data.status = 8;
            }
            if ($(this).attr("attr2") == "pelaksanaan-pemenang") {
                data.Mulai = moment($("#jadwal_pelaksanaan_pemenang").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
                data.status = 8;
            }
            if ($(this).attr("attr2") == "pelaksanaan-klarifikasi-lanjutan") {
                data.Mulai = moment($("#jadwal_klarifikasi_lanjutan").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
                data.Sampai = moment($("#jadwal_klarifikasi_lanjutan_sampai").val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
                data.status = 12;
            }
            
            rubahJadwalPelaksanaan(data);            
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

    $(".lihat-penilaian-buka-amplop").on("click", function () {
        window.open("http://" + window.location.host + "/rks-penilaian-buka-amplop.html#" + $("#pengadaanId").val());
    });

    $(".lihat-klarifikasi").on("click", function () {
        window.open("http://" + window.location.host + "/rks-klarifikasi.html#" + $("#pengadaanId").val());
    });
    
    $(".lihat-klarifikasi-lanjutan").on("click", function () {
        window.open("http://" + window.location.host + "/rks-klarifikasi-lanjutan.html#" + $("#pengadaanId").val());
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

    $(".kirim-undangan-klarifikasi-lanjutan").on("click", function () {
        waitingDialog.showloading("Proses Harap Tunggu");
        $.ajax({
            //data: odata,
            method: "POST",
            url: "Api/PengadaanE/sendMailKlarifikasi",
            data: { PengadaanId: $("#pengadaanId").val(), Surat: $("#mKlarifikasilanjutan").val() },
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

    $("body").on("click", ".checkbox-pilih-pemenang", function () {        
        var elTHis = $(this);
        var objData = {};
        objData.PengadaanId = $("#pengadaanId").val();
        objData.VendorId = $(this).attr("vendorid");
        if ($("#AturanPenawaran").val() == "Price Matching") {
            

            if ($(".checkbox-pilih-pemenang:checked").length > 1) {
                $(this).prop("checked", false);
                BootstrapDialog.show({
                    title: 'Informasi',
                    message: "Pemenang Hanya Boleh Satu",
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
            }
            else {
                if ($(this).is(':checked')) {
                    addPemenang(elTHis, objData);
                }
                else {
                    deletePemenang(elTHis, objData);
                }
            }
        }
        else {
            if ($(this).is(':checked')) {
                addPemenang(elTHis, objData);
            }
            else {
                deletePemenang(elTHis, objData);
            }
        }
    });

    $(".next-step").on("click", function () {
        if ($("#StatusTahapan").val() == "0") {
            waitingDialog.hideloading();
            BootstrapDialog.show({
                title: 'Error',
                message: "Semua Personil Harus Melakukan Persetujuan Pada Tahapan ini!",
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();

                    }
                }]
            });
            return;
        }
        else {
            var elDari = $(this).attr("elDari");
            var elSampai = $(this).attr("elSampai");

            var dari = moment($(elDari).val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
            var sampai = moment($(elSampai).val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
            var pengadaanId = $("#pengadaanId").val();
            $(this).attr("disabled", "disabled");
            var thisel = $(this);
            $.ajax({
                method: "POST",
                url: "Api/PengadaanE/nextStateAndSchelud?Id=" + pengadaanId + "&dari=" + dari + "&sampai=" + sampai,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    thisel.removeAttr("disabled");
                    waitingDialog.hideloading();
                    if (data == 0) {
                        BootstrapDialog.show({
                            title: 'Error',
                            message: 'Save Gagal!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    dialog.close();
                                }
                            }]
                        });
                    }
                    else {
                        window.location.reload();
                    }
                },
                error: function (errormessage) {
                    waitingDialog.hideloading();
                    thisel.removeAttr("disabled");
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
    });
    
   

    $(".lewati-tahapan").on("click", function () {
        
            var elDari = $(this).attr("elDari");
            var elSampai = $(this).attr("elSampai");
         
            var dari = moment($(elDari).val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
            var sampai = moment($(elSampai).val(), ["D MMMM YYYY HH:mm"], "id").format("DD/MM/YYYY HH:mm");
            var pengadaanId = $("#pengadaanId").val();
            $(this).attr("disabled", "disabled");
            var thisel = $(this);
            $.ajax({
                method: "POST",
                url: "Api/PengadaanE/nextStateAndSchelud?Id=" + pengadaanId + "&dari=" + dari + "&sampai=" + sampai,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    thisel.removeAttr("disabled");
                    waitingDialog.hideloading();
                    if (data == 0) {
                        BootstrapDialog.show({
                            title: 'Error',
                            message: 'Save Gagal!',
                            buttons: [{
                                label: 'Close',
                                action: function (dialog) {
                                    dialog.close();
                                }
                            }]
                        });
                    }
                    else {
                        window.location.reload();
                    }
                },
                error: function (errormessage) {
                    waitingDialog.hideloading();
                    thisel.removeAttr("disabled");
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

    getAanwijzing();
    getJadwal();

    getListSubmitRekanan();
    getListPenilainRekanan();
   
    getKandidatPemenang();
    generateUndangan();
    getPersetujuanBukaAmplop();
    
    getListSubmitKlarifikasiRekanan();
    getListSubmitKlarifikasiRekananLanjutan();
    getListKlarifikasiRekanan();
    getListKlarifikasiRekananLanjutan();
    getAllKandidatPengadaan();
    generateUndanganKlarifikasiLanjutan();
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

$(function () {
    
    getPersetujuanTahapanAll();
    $("body").on("click", ".click-persetujuan-tahapan", function () {
        savePersetujuanTahapan($(this).attr("attrStatus"));
    });
    
});

function getPersetujuanTahapanAll() {
    getPersetujuanTahapan("AANWIJZING");
    getPersetujuanTahapan("SUBMITPENAWARAN");
    getPersetujuanTahapan("BUKAAMPLOP");
    getPersetujuanTahapan("KLARIFIKASI");
    getPersetujuanTahapan("KLARIFIKASILANJUTAN");
    getPersetujuanTahapan("PENILAIAN");
    getPersetujuanTahapan("PEMENANG");
}

function savePersetujuanTahapan(tahapan) {
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/SavePersetujuanTahapan?PengadaanId=" + $("#pengadaanId").val() + "&&status=" + tahapan,
        success: function (data) {
            waitingDialog.hideloading();
            if (data.Id == 0) {
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
            getPersetujuanTahapanAll();
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

function getPersetujuanTahapan(tahapan) {
    
    $.ajax({
        method: "GET",
        url: "Api/PengadaanE/GetPersetujuanTahapan?PengadaanId=" + $("#pengadaanId").val() + "&&status=" + tahapan,
        success: function (data) {
            renderPersetujuanPelaksanaan(data, tahapan);
            if ($("#StatusName").val() == tahapan) {
                var oData = $.grep(data, function (e) { return e.Status == 0; });
                if (oData.length == 0) {
                    $("#StatusTahapan").val(1);
                }
                else $("#StatusTahapan").val(0);
            }
        },
        error: function (errormessage) {
        }
    });

}

function renderPersetujuanPelaksanaan(data, tahapan) {
    var el = ".bingkai-" + tahapan;
    var html = "";
    if (data.length == 0) return;
    var oData = $.grep(data, function (e) { return e.StatusPengadaanName == tahapan; });
    for (var i in oData) {
        var class_status = oData[i].Status == 0 ? "btn-danger" : "btn-success";
        var class_pin = oData[i].Status == 0 ? "glyphicon-pushpin" : "glyphicon-ok";
        html += '<div class="col-md-3">' +
                     '<div class="form-group">' +
                        '<button class="btn ' + class_status +
                            ' btn-block click-persetujuan-tahapan" attrStatus="' + tahapan +
                            '"><i class="glyphicon ' + class_pin + '"></i>' + oData[i].UserName + '</button>' +
                    '</div>' +
                '</div>';
    }
    $(el).html("");
    $(el).append(html);
}

function deletePemenang(elTHis, objData) {

    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/deletePemenang",
        dataType: "json",
        data: JSON.stringify(objData),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            elTHis.prop('checked', false);
            waitingDialog.hideloading();
            if (data.Id == 0) {
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

function addPemenang(elTHis, objData) {
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
            if (data.Id == 0) {
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

function getJadwal() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/CurrentStatePengadaan?Id=" + $("#pengadaanId").val(),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {            
            //if (data > 3)  getDateSubmitPenawaran(); 
            //if (data <= 3)$("#btn-next-submit-penawaran").removeAttr("disabled");            
            //if (data >5) getBukaAmplop();
            //if (data == 4) $("#btn-next-buka-amplop").removeAttr("disabled");
            //if (data >6) getPenilaian();
            //if (data == 5)  $("#btn-next-penilaian").removeAttr("disabled");            
            //if (data >7) getKlarifikasi();
           // if (data == 6) $("#btn-next-klarifikasi").removeAttr("disabled");
            //if (data == 7) $("#btn-next-pemenang").removeAttr("disabled");
           // if (data >8) getPemenang();
            getDateSubmitPenawaran();
            getBukaAmplop();
            getKlarifikasi();
            getPenilaian();
            getPemenang();
            getKlarifikasiLanjutan();
        }
    });
   
}

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

function rubahJadwalPelaksanaan(data) {   
    var status=data.status;
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/UpdateJadwalPelaksanaan",
        dataType: "json",
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
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
            switch(status) {
                case 3:getAanwijzing();
                    break;
                case 4:getDateSubmitPenawaran();
                    break;
                case 5: getBukaAmplop();
                    break;
                case 6: getPenilaian();
                    break;
                case 7: getKlarifikasi;
                    break;
                case 8: getPemenang();
                    break;
                default: getJadwal()
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

function getKlarifikasiLanjutan() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetJadwalPelaksanaan?PId=" + $("#pengadaanId").val() + "&status=KLARIFIKASILANJUTAN",
        success: function (data) {
            $("#jadwal_klarifikasi_lanjutan").val(moment(data.Mulai).format("DD MMMM YYYY HH:mm"));
            $("#jadwal_klarifikasi_lanjutan_sampai").val(moment(data.Sampai).format("DD MMMM YYYY HH:mm"));
            $("#klarifikasi_lanjutan").html("( " + moment(data.Mulai).format("DD MMMM YYYY HH:mm") + " s/d " + moment(data.Sampai).format("DD MMMM YYYY HH:mm") + " )");
            if (data.Id != "00000000-0000-0000-0000-000000000000") {
                //if (isGuid(data.Id)) {
                //    $("#aanwijzingPId").val(data.Id);
                //}
            }
            generateUndanganKlarifikasiLanjutan();
        },
        error: function (errormessage) {
          //  alert("gagal");
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
           // alert("gagal");
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
        url: "Api/PengadaanE/GetRekananPenilaian2?PId=" + $("#pengadaanId").val(),
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
           // alert("gagal");
        }
    });
}

function generateUndanganKlarifikasiLanjutan() {
    $.ajax({
        method: "GET",
        url: "Api/PengadaanE/GetJadwalPelaksanaan?PId=" + $("#pengadaanId").val() + "&status=KLARIFIKASILANJUTAN",
        success: function (data) {
            var html = "Panitia " + $("#judul").text() + " Untuk " + $("#UnitKerjaPemohon").text() +
           ". Mohon untuk Klarifikasi Lanjutan, penawaran kami tunggu paling lambat " + moment(data.Sampai).format("DD MMMM YYYY") + " sebelum pukul " + moment(data.Sampai).format("HH:mm") + "\n" +
           "Demikian kami sampaikan. Terimakasi atas perhatiannya serta kerjasamanya.";
            var div = $("#mKlarifikasilanjutan").val(html);
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

function getListSubmitKlarifikasiRekananLanjutan() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetRekananKlarifikasiSubmitLanjutan?PId=" + $("#pengadaanId").val(),
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
                $(".list-rekanan-klarifikasi-lanjutan").append(html);
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

function getListKlarifikasiRekananLanjutan() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetRekananKlarifikasiPenilaianLanjutan?PId=" + $("#pengadaanId").val(),
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
                $(".list-rekanan-klarifikasi-lanjutan-check").append(html);
            });
        }
    });
}

function getKandidatPemenang() {
    $.ajax({
        method: "POST",
        url: "Api/PengadaanE/GetPemenangPengadaan?PId=" + $("#pengadaanId").val(),
        success: function (data) {
            $(".list-rekanan-pemenang").html("");
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
                                    '<p class="text-muted text-center deskripsi">No SPK: ' + value.NoSPK + '</p>' +
                                '</div>' +
                            '</div>' +
                        '</div>';
                $(".list-rekanan-pemenang").append(html);
            });
            $("#Pemenang").val(JSON.stringify(data));
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


function saveTahap(data,el,elpanel) {
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: "Api/PengadaanE/saveTahapan",
        method: "POST",
        data:data,
        success: function (datax) {
            if (datax.Id == "00000000-0000-0000-0000-000000000000") {
                el.prop("checked", false);
                elpanel.hide();
               
            }
            else {
                el.prop("checked", true);
                elpanel.show();
            }
            waitingDialog.hideloading();
        },
        error: function (errormessage) {

        }
    });
}

$(function () {

    $("#tambah-klarifikasi-lanjut").on("click", function () {
        var data = {};
        data.PengadaanId = $("#pengadaanId").val();
        data.Status = 12;
        if ($(this).is(':checked'))
            data.Tambah = 1;
        else data.Tambah = 0;        
        saveTahap(data, $(this), $(".panel-klarifikasi-lanjut"));
    });

    $("#tambah-penilaian").on("click", function () {
        var data = {};
        data.PengadaanId = $("#pengadaanId").val();
        data.Status = 6;
        if ($(this).is(':checked'))
            data.Tambah = 1;
        else data.Tambah = 0;
        saveTahap(data, $(this), $(".panel-penilaian"));
    });
});
var user_table;
$(function () {
    $(".add-user-terkait").on("click", function () {
        $("#users_modal").modal("show");

    });
    user_table = $("#user_table").DataTable({
        "serverSide": true,
        "searching": true,
        "ajax": {
            "url": 'api/Pengadaane/ListUsers',
            "type": 'POST',
            "data": function (d) {
                d.status = "1";
                d.more = "0";
            }
        },
        "columns": [
            { "data": null },
            { "data": "Nama" },
            { "data": "jabatan" }
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    return '<input type="checkbox" class="check-user" attrId="' + data.PersonilId + '" />';
                },

                "targets": 0,
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
    renderUserTerkait();
    $(".adduserterkai").on("click", function () {
        var note = $("#note_user").val();

        if ($("#user_table").find("input[type=checkbox]:checked").length != 1) {
            BootstrapDialog.show({
                title: 'Konfirmasi',
                message: ' Wajib Pilih satu dipilih!',
                buttons: [{
                    label: 'Close',
                    action: function (dialog) {
                        dialog.close();

                    }
                }]
            });
            return false;
        }

        var userid = $("#user_table").find("input[type=checkbox]:checked").attr("attrid");
        var pengadaanId = $("#pengadaanId").val();
        $.ajax({
            url: 'api/PengadaanE/SavePersetujuanTerkait?PengadanId=' + pengadaanId + "&UserId=" + userid,
            method: "GET",
            success: function (data) {
                var message = "Gagal Save";
                if (data.Id != "") message = "Save Sukses";
                BootstrapDialog.show({
                    title: 'Konfirmasi',
                    message: message,
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
                renderUserTerkait();

                $("#users_modal").modal("close");
            }
        });
    });

});

function renderUserTerkait2() {
    var pengadaanId = $("#pengadaanId").val();
    $.ajax({
        url: 'api/PengadaanE/UserTerkait?PengadanId=' + pengadaanId ,
        method: "GET",
        success: function (data) {
            var html = "";
            var cekStatus = 1;
            for (var i in data) {
                var class_status = data[i].setuju == 0 ? "btn-danger" : "btn-success";
                var class_pin = data[i].setuju == 0 ? "glyphicon-pushpin" : "glyphicon-ok";
                html += '<div class="col-md-3">' +
                    '<div class="form-group">' +
                       '<button class="btn ' + class_status +
                           ' btn-block click-user-terkait"><i class="glyphicon ' + class_pin + '"></i>' + data[i].Nama + '</button>' +
                   '</div>' +
               '</div>';
                if (data[i].setuju == 0) cekStatus = 0;
            };
            $(".list-user-terkait").html("");
            $(".list-user-terkait").append(html);
            if (cekStatus == 0) $(".ajukan-pemenang").hide();
        }
    });
}

function renderUserTerkait() {
    var pengadaanId = $("#pengadaanId").val();
    $.ajax({
        url: 'api/PengadaanE/UserTerkait?PengadanId=' + pengadaanId,
        method: "GET",
        success: function (data) {
            var html = "";
            var cekStatus = 1;
            var html = "<table class='table table-bordered table-striped table-responsive'><thead>";
            html += "<th>User Name</th>";
            html += "<th>Status</th>";
            html += "<th></th>";
            html += "</thead>";
            html += "<tbody>";
            var isPic = $("#isPIC").val();
            for (var i in data) {
                html += "<tr><td>" + data[i].Nama + "</td>";
                html += "<td>" + (data[i].setuju == 0 ? "Tidak Setuju" : "Setuju") + "</td>";
                html += "<td>" + (data[i].isthismine == 1 ? (data[i].setuju == 0 ? "<button class='btn btn-default click-user-terkait '>Setuju</button>" : "") : "") + (isPic == 1 ? "<button class='btn btn-default hapus-user-terkait' attrId='" + data[i].Id + "'>Hapus</button>" : "") + "</td></tr>";
               
              /*  var class_status = data[i].setuju == 0 ? "btn-danger" : "btn-success";
                var class_pin = data[i].setuju == 0 ? "glyphicon-pushpin" : "glyphicon-ok";
                html += '<div class="col-md-3">' +
                    '<div class="form-group">' +
                       '<button class="btn ' + class_status +
                           ' btn-block click-user-terkait"><i class="glyphicon ' + class_pin + '"></i>' + data[i].Nama + '</button>' +
                   '</div>' +
               '</div>';*/
                if (data[i].setuju == 0) cekStatus = 0;

            };
            html += "</tbody></table>";
            $(".list-user-terkait").html("");
            $(".list-user-terkait").append(html);
            if (cekStatus == 0) $(".ajukan-pemenang").hide();
        }
    });
}

$(function () {
    $("body").on("click", ".click-user-terkait", function () {
         TerkaitSetuju();        
    });
    // 
    $("body").on("click", ".hapus-user-terkait", function () {
        var id = $(this).attr("attrId");
        DeleteUserTerkait(id);
    });
});

function TerkaitSetuju() {
    var pengadaanId = $("#pengadaanId").val();
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: 'api/PengadaanE/TerkaitSetuju?PengadanId=' + pengadaanId,
        method: "GET",
        success: function (data) {
            renderUserTerkait();
            waitingDialog.hideloading();
        }
    });
}


function DeleteUserTerkait(id) {
    var pengadaanId = $("#pengadaanId").val();
    waitingDialog.showloading("Proses Harap Tunggu");
    $.ajax({
        url: 'api/PengadaanE/DeletePersetujuanTerkait?Id=' + id,
        method: "GET",
        success: function (data) {
            renderUserTerkait();
            waitingDialog.hideloading();
        }
    });
}