<!DOCTYPE html>
<html ng-app="app">
  <head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Mandiri Tunas Finance</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <!-- Bootstrap 3.3.5 -->
    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="font-awesome/font-awesome.min.css">
    <!-- Ionicons -->
    <link rel="stylesheet" href="ionicons/ionicons.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css">
    <!-- AdminLTE Skins. Choose a skin from the css/skins
         folder instead of downloading all of them to reduce the load. -->
    <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css">
	<link rel="stylesheet" href="plugins/select2/select2.min.css">
	
	<link rel="stylesheet" href="plugins/datatables/dataTables.bootstrap.css">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
        <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
	
	
	<link href="plugins/jQueryUItotop/ui.totop.css" rel="stylesheet" />
	<link rel="stylesheet" href="plugins/JQueryValidation/validationEngine.jquery.css">
	<link href="chatjs/css/jquery.chatjs.css" rel="stylesheet" /> 
	<link href="plugins/datepicker/datepicker3.css" rel="stylesheet" />
    <link rel='stylesheet' href="datetimepicker/bootstrap-datetimepicker.css" />
	<link href="dist/site.css" rel="stylesheet" />
	<link rel='stylesheet' href='plugins/fullcalendar/fullcalendar.min.css' />
	<link rel="stylesheet" href="dist/app.css">
	<link rel="stylesheet" href="dropzone.css">
		<style type="text/css">
			.dz-error-mark{
			display:none !important;
		}
		.dropzone{
			min-height: 50px;
		}
		.dropzone .dz-message {
			margin: 0em 0;
			text-align: center;
		}
			form.jp-navbar-form {
				padding-top:3px;
				padding-left:50px;
				margin:0;
			}
			.clear-margin{
				margin:0 !important;
				padding-top:3px;
				padding-left:250px;
			}
            .bootstrap-datetimepicker-widget{
                z-index: 100000000000000000000000000000!important;
            }
		</style>
  </head>
 <body class="hold-transition skin-blue sidebar-collapse sidebar-mini fixed"  data-spy="scroll" data-target="#myScrollspy">
    <div class="wrapper" id="section-1">

      
     <div id="header">
        <header class="main-header">
        <!-- Logo -->
        <a  class="logo">
            <!-- mini logo for sidebar mini 50x50 pixels -->
        <span class="logo-mini" style="line-height:60px;"><img src="dist/img/mtf-bg-biru.png" width="50px" /></span> 
            <!-- logo for regular state and mobile devices -->
        <span class="logo-lg"><img src="dist/img/mtf-bg-biru.png" width="100px" /></span> 
        </a>
        <!-- Header Navbar: style can be found in header.less -->
        <nav class="navbar navbar-static-top" role="navigation">
            <!-- Sidebar toggle button-->
            <a href="#" class="sidebar-toggle" data-toggle="offcanvas" role="button">
            <span class="sr-only">Toggle navigation</span>
            </a>
	        <div class="navbar-custom-menu pull-left"> 
	        <ul class="nav navbar-nav">
		       <li>	<a id="judul-page" href="#">Detail Pengadaan </a></li> 
	        </ul>
	        </div>
            <div class="navbar-custom-menu">            
            <ul id="menu-top" class="nav navbar-nav">
                <li><a  id="arsipkan" title="Arisipkan" href="#" class="arsipkan" style="display:none" ><i class="fa fa-fw fa-save " style="font-size:20px;"></i>Arsipkan</a></li>
                <li><a  id="ajukan" title="Ajukan" href="#" class="SimpanAjukan" style="display:none" ><i class="fa fa-fw fa-save " style="font-size:20px;"></i>Ajukan</a></li>
                <li><a  id="edit" title="Add Item" href="#" style="display:none"><i class="fa fa-fw fa-pencil-square-o " style="font-size:20px;"></i>Edit</a></li>
                <li><a  id="setujui" title="Setujui" href="#" class="Setujui" style="display:none" ><i class="fa fa-fw " style="font-size:20px;"></i>Setujui</a></li>
                <li><a  id="tolak" title="Tolak" href="#" class="Tolak" style="display:none" ><i class="fa fa-fw " style="font-size:20px;"></i>Tolak</a></li>
                <li><a  id="dibatalkan" title="Batalkan" href="#" class="dibatalkan" style="display:none" ><i class="fa fa-fw " style="font-size:20px;"></i>Batalkan</a></li>
            </ul>
            </div>
        </nav>
        </header>
    </div>
          <!-- Left side column. contains the logo and sidebar -->
     <div id="menu"  >
          <aside class="main-sidebar" >
            <!-- sidebar: style can be found in sidebar.less -->
             <section class="sidebar" >
			   <!-- <div class="user-panel" ng-controller="user">				
				    <div class="pull-left info">
					    <p class="title" ng-bind="user.nama"></p>
					    <p class="deskripsi" ng-bind="user.divisi"></p>
				    </div>
				    <div class="pull-right image image-profile">
					    <img alt="User Image" class="img-circle" src="{{user.logo}}" >
				    </div>
			    </div>-->
              <!-- sidebar menu: : style can be found in sidebar.less -->
              <ul class="sidebar-menu"  ng-controller="side-menu">
               <!-- <li class="header">MAIN NAVIGATION</li>-->
             
			    <li class="treeview" ng-repeat="menu in menus">
				    <a href="{{menu.url}}">
				        <i class="{{menu.css}}"></i> <span ng-bind="menu.menu"></span>
				    </a>				
			    </li>
              </ul>
            </section>
            <!-- /.sidebar -->
          </aside>
     </div>
    
      <!-- Content Wrapper. Contains page content -->
      <div class="content-wrapper">
        <input type="hidden" id="pengadaanId" />
          <input type="hidden" id="isPIC" />
          <input type="hidden" id="isTEAM" />
          <input type="hidden" id="isPersonil" />
		 <section class="content">
			<!-- Default box -->				
				<div class="row">
				    <div class="col-md-10">
                            <div class="box box-primary box-white" >
				                <div class="box-body ">						
							        <div id="notif">
							        </div>
							            <form role="form" class="jp-form">		
										    <div class="form-group judul-pengadaan">
											    <label id="judul"></label>											
										    </div>
										    <div class="form-group">
											    <label id="deskripsi"> </label>											
										    </div>										
										    <div class="form-group">
											    <label id="keterangan"></label>
										    </div>	
                                             <div class="form-group">
											    <label id="Status" class="bg-green " style="padding:5px"></label>
										    </div>	
                                            <div class="form-group">
											    <label id="AlasanPenolakan" style="display:none;padding:5px" class="bg-red " style="padding:5px"></label>
										    </div>									                
										    <div class="nav-tabs-custom">
											    <ul class="nav nav-tabs" id="myTab">
											        <li class="active"><a href="#rencana_kerja" id="tab-rk" >Rencana Kerja</a></li>
											        <li><a href="#perlaksanaan" id="tab-pelakasanaan"  >Pelaksanaan</a></li>
											        <li><a href="#berkas" id="tab-berkas" >Berkas-Berkas</a></li>
											    </ul>
											    <div class="tab-content" style="overflow:visible">
												    <div id="rencana_kerja" class=" tab-pane active"  style="overflow:visible">
														    <div class="row">
															    <div class="col-md-12">
																    <div class="panel-header-white">
																	    <span class="title-header" id="section-2">Anggaran</span>
																    </div>
															    </div>
														    </div>	
                                                            <div class="row">	
                                                                <div class="form-group col-md-6">
												                    <label class="title">Mata Uang:</label>
												                    <label class="deskripsi" id="MataUang"></label>
											                    </div>	
															    <div class="form-group col-md-6">
																    <label class="title">Harga Perkiraan Sendiri:</label>
																    <label class="deskripsi" id="hps"></label>
                                                                    <a class="deskripsi" id="lihatHps" target="_blank">Lihat HPS</a>
															    </div>
															    <div class="form-group col-md-6">
																    <label class="title">Priode Anggaran:</label>
																    <label class="deskripsi" id="PeriodeAnggaran"></label>
															    </div>	
															    <div class="form-group col-md-6">
																    <label class="title">Jenis Pembelanjaan:</label>
																    <label class="deskripsi" id="JenisPembelanjaan"></label>
															    </div>	
														    </div>	
														    <div class="row">
															    <div class="col-md-12">
																    <div class="panel-header-white">
																	    <span class="title-header" id="section-3">Korespondensi Internal </span>
																    </div>
															    </div>
														    </div>
														    <div class="row">	
															    <div class="form-group col-md-6">
																    <label class="title">Nota Internal:</label>
																    <label class="deskripsi" id="TitleDokumenNotaInternal"> </label>
															        <div id="DokumenNotaInternal" attr="NOTA" action="/api/PengadaanE/UploadFile?tipe=NOTA" class="dropzone"></div>		
                                                                </div>
															    <div class="form-group col-md-6">
																    <label class="title">Unit Kerja Pemohon:</label>
																    <label class="deskripsi" id="UnitKerjaPemohon"></label>
															    </div>	
                                                                <div class="form-group col-md-6">
																    <label class="title">Dokumen Lainnya:</label>
																    <label class="deskripsi" id="TitleDokumenLain"></label>
															        <div id="DokumenLain" attr="DOKUMENLAIN" action="/api/PengadaanE/UploadFile?tipe=DOKUMENLAIN" class="dropzone"></div>	
                                                                </div>	
														    </div>
														    <div class="row">
															    <div class="col-md-12">
																    <div class=" panel-header-white">
																	    <span class="title-header" id="section-4">Spesifikasi</span>
																    </div>
															    </div>
														    </div>
														    <div class="row">	
															    <div class="form-group col-md-6">
																    <label class="title">Wilayah Kerja/Region:</label>
																    <label class="deskripsi" id="Region"></label>
															    </div>		
															    <div class="form-group col-md-6">
																    <label class="title">Provinsi:</label>
																    <label class="deskripsi" id="Provinsi"></label>
															    </div>	
															    <div class="form-group col-md-6">
																    <label class="title">Kualifikasi Rekanan:</label>
																    <!--<label class="deskripsi" id="KualifikasiRekan"></label>-->
                                                                    <div class="checkbox">
                                                                        <label><input type="checkbox" disabled id="ukm" attrId="" class="checkbox-kualifikasi" value="Usaha Kecil Menengah">Usaha Kecil Menengah</label>
                                                                    </div>
                                                                    <div class="checkbox">
                                                                        <label><input type="checkbox" disabled id="bukan-ukm" attrId=""  class="checkbox-kualifikasi" value="Bukan Usaha Kecil Menengah">Bukan Usaha Kecil Menengah</label>
                                                                    </div>
                                                                    <div class="checkbox">
                                                                        <label><input type="checkbox" disabled id="perorangan" attrId="" class="checkbox-kualifikasi" value="Perorangan">Perorangan</label>
                                                                    </div>
															    </div>	
															    <div class="form-group col-md-6">
																    <label class="title">Jenis Pekerjaan:</label>
																    <label class="deskripsi" id="JenisPekerjaan"> </label>
															    </div>
															    <div class="form-group col-md-6">
																    <label class="title">Berkas Rujukan:</label>
                                                                    <label class="deskripsi" id="TitleBerkasRujukanLain"> </label>
																    <div id="BerkasRujukanLain" attr="BerkasRujukanLain" action="/api/PengadaanE/UploadFile?tipe=BerkasRujukanLain" class="dropzone"></div>	
															    </div>
														    </div>
                                                            <div class="row" >
											                    <div class="col-md-12">
												                    <div class=" panel-header-white">
													                    <span class="title-header" id="section-5">Kandidat</span>
												                    </div>
											                    </div>
											                    <div class="listkandidat col-md-12" >	
                                                                   
											                    </div>	
										                    </div>
														    <div class="row">
															    <div class="col-md-12">
																    <div class=" panel-header-white">
																	    <span class="title-header" id="section-6">Jadwal</span>
																    </div>
															    </div>
														    </div>
														    <div class="row">	
															    <div class="col-md-4">
																    <div class="form-group">
																	    <label class="title">Aanwijzing:</label>
																	    <label class="deskripsi jadwal" id="Aanwijzing"></label>
																    </div>
																    <div class="form-group">
																	    <label class="title">Pengisian Harga:</label>
																	    <label class="deskripsi jadwal" id="PengisianHarga"></label>
																    </div>
																    <div class="form-group">
																	    <label class="title">Buka Amplop:</label>
																	    <label class="deskripsi jadwal" id="BukaAmplop"</label>
																    </div>
                                                                    <div class="form-group">
																	    <label class="title">Penilaian Kandidat:</label>
																	    <label class="deskripsi jadwal" id="penilaian"></label>
																    </div>
																    <div class="form-group">
																	    <label class="title">Klarifikasi & Negosiasi:</label>
																	    <label class="deskripsi jadwal" id="Klarifikasi"></label>
																    </div>
                                                                   <!-- <div class="form-group">
																	    <label class="title">Klarifikasi Lanjut</label>
																	    <label class="deskripsi">22/02/2016</label>
																    </div>-->
																    <div class="form-group">
																	    <label class="title">Penentuan Pemenang:</label>
																	    <label class="deskripsi jadwal" id="PenentuanPemenang"></label>																	
																    </div>																
															    </div>
															    <!--<div class="col-md-8">
																    <div style="margin:10px;">
																	    <div id="calendar"></div>
																    </div>
															    </div>
															    -->
														    </div>
														    <div class="row">
															    <div class="col-md-12">
																    <div class=" panel-header-white">
																	    <span class="title-header" id="section-7">Personil  </span>
																    </div>
															    </div>
														    </div>
														    <div class="row">	
                                                                <div class="col-md-12">
											                        <label >PIC</label>		
                                                                    <a id="addPerson-pic" attr1=".listperson-pic" style="margin-top:10px;margin-bottom:10px;display:none" class="addPerson btn btn-sm btn-social btn-bitbucket">		
                                                                         <i class="fa fa-plus"></i>
                                                                        Rubah PIC
                                                                        </a>								
											                        <div class="listperson-pic">		
                                                                        																										
											                        </div>												
										                        </div>
                                                                <div class="col-md-12">
											                        <label >Tim</label>										
											                        <div class="listperson-tim">	
											                        </div>												
										                        </div>
                                                                <div class="col-md-12">
											                        <label >User</label>										
											                        <div class="listperson-staff">	
											                        </div>												
										                        </div>
                                                                 <div class="col-md-12">
											                        <label >Controller</label>										
											                        <div class="listperson-controller">
                                                                             
                                                                    </div>												
										                        </div>
                                                                <div class="col-md-12">
											                        <label >Compliance</label>										
											                        <div class="listperson-compliance">    
                                                                    </div>												
										                        </div>
														    </div>
												    </div>
											        <div id="perlaksanaan" class="tab-pane">                                                           
                                                        <div class="box-body">
                                                              <div class="box-group" id="accordion">
                                                                <!-- we are adding the .panel class so bootstrap.js collapse plugin detects it -->
                                                                <div class="panel box box-primary">
                                                                  <div class="box-header with-border">
                                                                    <h4 class="box-title text-blue-mtf">
                                                                      <a  id="tab-anwijzing" data-parent="#accordion" href="#collapseOne">
                                                                        Aanwijzing
                                                                      </a>
                                                                       <label id="aanwijzing_aktual" style="font-size:0.8em"></label>
                                                                    </h4>
                                                                        <button attr1="#aanwijzing_pelaksanaan" attr2="aanwijzing_pelaksanaan" style="float:right;margin-left:10px" class="btn btn-success ubah-jadwal jadwal-aanwijzing action-pelaksanaan"><i class="fa fa-fw fa-calendar"></i>Ubah</button>
                                                                        <input type="text" disabled="" value="12/02/2016" id="aanwijzing_pelaksanaan" style=" float:right;width:20%" class="form-control dateJadwal jadwal-aanwijzing "> 
                                                                  </div>
                                                                  <div id="collapseOne" class="panel-collapse collapse">
                                                                        <div class="box-body">
                                                                            <div class="row">
                                                                                <div class="col-md-10">
                                                                                    <div class="form-group">
                                                                                        <textarea class="form-control action-pelaksanaan" id="undangan"  rows="5">
                                                                                        </textarea>
                                                                                    </div>
                                                                                </div>
                                                                                 <div class="col-md-2">
                                                                                    <div class="form-group">
                                                                                        <button class="btn btn-success pull-left action-pelaksanaan kirim-undangan"><i class="fa fa-fw fa-envelope-o"></i>Kirim Undangan</button>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="row">
                                                                              <div class="col-md-8">
                                                                                  <div class="row">
                                                                                       <div class="col-md-8">
                                                                                            <div class="form-group">
                                                                                                <label class="title">Tanggal Berita Acara</label>
                                                                                                <input id="input-ba-aanwijzing" type="text"  class="form-control dateJadwalNoTime action-pelaksanaan" > 
                                                                                            </div>
                                                                                            <div class="form-group">
                                                                                                <button class="btn btn-success pull-left save-tgl-ba action-pelaksanaan" attr1="input-ba-aanwijzing" attr2="BeritaAcaraAanwijzing">Simpan</button>
                                                                                            </div>
                                                                                        </div>                                                                                      
                                                                                  </div>
                                                                                  <div class="row">
                                                                                      <div class="col-md-12">
                                                                                          <div class="form-group">	
                                                                                                <label class="title">Unggah Berita Acara</label>							
													                                            <div id="BeritaAcaraAanwijzing" attr="BeritaAcaraAanwijzing" action="/api/PengadaanE/UploadFile?tipe=BeritaAcaraAanwijzing" class="dropzone"></div>																												
										                                                   </div>
                                                                                       </div>
                                                                                   </div>
                                                                               </div>
                                                                                <div class="col-md-2">
                                                                                    <div class="form-group">
                                                                                        <label class="title">Unduh Berita Acara
                                                                                           <a  class="download-dokumen download-berkas" attr1="berkas-aanwijzing"><i class="fa fa-fw  fa-download"></i></a> 
                                                                                        </label>
                                                                                        <label class="title">Unduh Daftar Hadir
                                                                                            <a class="download-berkas" attr1="daftar-hadir"><i class="fa fa-fw  fa-download"></i></a> 
                                                                                        </label>
                                                                                    </div>
                                                                                </div>
                                                                           </div>
                                                                            <div class="row">
                                                                                <div class="col-md-4">
                                                                                    <div class="form-group ">
											                                            <label class="title">Kehadiran</label>																
										                                            </div>
                                                                               </div>
                                                                            </div>
                                                                            <div class="row kehadiran-kandidat">
                                                                               
                                                                            </div>
                                                                        </div>
                                                                  </div>
                                                                </div>
                                                                <div class="panel box box-primary">
                                                                  <div class="box-header with-border">
                                                                    <h4 class="box-title text-blue-mtf">
                                                                      <a id="tab-submit-penawaran" data-parent="#accordion"  href="#collapseTwo">
                                                                        Submit Penawaran
                                                                      </a>
                                                                        <label id="pengisian_harga_aktual" style="font-size:0.8em"></label>
                                                                    </h4>
                                                                       <button class="btn btn-success ubah-jadwal jadwal-submit action-pelaksanaan" style="float:right;margin-left:10px" attr2="pengisian_harga" attr3="#tgl_pengisian_harga_sampai_re" attr1="#tgl_pengisian_harga_re"><i class="fa fa-fw fa-calendar"></i>Ubah</button>
                                                                       <input type="text" style="float:right;width:20%;margin-left:10px" disabled class="form-control dateJadwal jadwal-submit" id="tgl_pengisian_harga_sampai_re" />
                                                                       <input type="text" style="float:right;width:20%" disabled class="form-control dateJadwal jadwal-submit" id="tgl_pengisian_harga_re" />
                                                                  </div>
                                                                  <div id="collapseTwo" class="panel-collapse collapse">
                                                                    <div class="box-body">
                                                                        <div class="row list-submit-rekanan">
                                                                            
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                                </div>
                                                                <div class="panel box box-primary">
                                                                  <div class="box-header with-border">
                                                                    <h4 class="box-title text-blue-mtf">
                                                                      <a id="tab-buka-amplop" data-parent="#accordion" href="#collapseThree">
                                                                            Buka Amplop
                                                                      </a>
                                                                        <label id="buka_amplop_aktual" style="font-size:0.8em"></label>
                                                                    </h4>
                                                                      <button class="btn btn-success ubah-jadwal jadwal-buka-amplop action-pelaksanaan" style="float:right;margin-left:10px" attr2="buka_amplop" attr3="#buka_amplop_sampai_re"   attr1="#buka_amplop_re"><i class="fa fa-fw fa-calendar" ></i>Ubah</button>
                                                                      <input type="text" disabled class="form-control dateJadwal jadwal-buka-amplop" style="float:right;margin-left:10px;width:20%" id="buka_amplop_sampai_re" />
                                                                      <input type="text" disabled class="form-control dateJadwal jadwal-buka-amplop" style="float:right;width:20%" id="buka_amplop_re" />
                                                                  </div>
                                                                  <div id="collapseThree" class="panel-collapse collapse">
                                                                    <div class="box-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">
                                                                                    <button class="btn btn-danger btn-block persetujuan-buka-amplop action-pelaksanaan-pic" attr1="pic"><i class="glyphicon glyphicon-pushpin"></i>PIC</button>
                                                                                </div>
                                                                            </div>                                                                       
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">
                                                                                    <button class="btn btn-block btn-danger persetujuan-buka-amplop action-pelaksanaan-compliance" attr1="staff"><i class="glyphicon glyphicon-pushpin"></i>User</button>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">
                                                                                    <button class="btn btn-danger btn-block persetujuan-buka-amplop action-pelaksanaan-compliance" attr1="compliance"><i class="glyphicon glyphicon-pushpin"></i>Complaince</button>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">
                                                                                    <button class="btn btn-danger btn-block persetujuan-buka-amplop action-pelaksanaan-controller" attr1="controller"><i class="glyphicon glyphicon-pushpin"></i>Controller</button>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-md-10">
                                                                                <div class="row">
                                                                                       <div class="col-md-8">
                                                                                            <div class="form-group">
                                                                                                <label class="title">Tanggal Berita Acara</label>
                                                                                                <input id="input-ba-bukaamplop" type="text"  class="form-control dateJadwalNoTime action-pelaksanaan" > 
                                                                                            </div>
                                                                                            <div class="form-group">
                                                                                                <button class="btn btn-success pull-left save-tgl-ba action-pelaksanaan" attr1="input-ba-bukaamplop" attr2="BeritaAcaraBukaAmplop">Simpan</button>
                                                                                            </div>
                                                                                        </div>    
                                                                                    </div>  
                                                                                <div class="row">                                                                                
                                                                                        <div class="col-md-8">
                                                                                            <div class="form-group ">
											                                                    <label class="title">Unggah Berita Acara</label>		
                                                                                                 <div id="BeritaAcaraBukaAmplop" attr="BeritaAcaraBukaAmplop" action="/api/PengadaanE/UploadFile?tipe=BeritaAcaraBukaAmplop" class="dropzone"></div>																
										                                                    </div>
                                                                                      </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-2">
                                                                                <div class="form-group">
                                                                                    <label class="title">Unduh Berita Acara
                                                                                        <a  class="download-dokumen download-berkas" attr1="berkas-buka-amplop"><i class="fa fa-fw fa-download"></i></a> 
                                                                                    </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                                </div>
                                                                 <div class="panel box box-primary">
                                                                  <div class="box-header with-border">
                                                                    <h4 class="box-title text-blue-mtf">
                                                                      <a   id="tab-penilaian-kandidat"  data-parent="#accordion" href="#collapse4">
                                                                            Penilaian Kandidat
                                                                      </a>
                                                                        <label id="penilaian_aktual" style="font-size:0.8em"></label>
                                                                    </h4>
                                                                      <button class="btn btn-success ubah-jadwal jadwal-penilaian action-pelaksanaan" style="float:right;margin-left:10px" attr2="penilaian_kandidat" attr3="#jadwal_penilaian_kandidat_sampai"   attr1="#jadwal_penilaian_kandidat"><i class="fa fa-fw fa-calendar" ></i>Ubah</button>
                                                                        <input type="text" disabled class="form-control dateJadwal jadwal-penilaian" style="float:right;margin-left:10px;width:20%" id="jadwal_penilaian_kandidat_sampai" />
                                                                      <input type="text" disabled class="form-control dateJadwal jadwal-penilaian" style="float:right;width:20%"  id="jadwal_penilaian_kandidat" />
                                                                  </div>
                                                                  <div id="collapse4" class="panel-collapse collapse">
                                                                    <div class="box-body">
                                                                        <div class="row">
                                                                            <div class="col-md-2">
                                                                                 <div class="form-group">
                                                                                    <a class="btn btn-success pull-left lihat-penilaian" href="#"><i class="fa fa-fw fa-calendar-check-o" ></i>Lihat Tawaran Kandidat</a>
                                                                                  </div>
                                                                              </div>
                                                                        </div>
                                                                        <div class="row list-rekanan-penilaian" style="margin-top:15px">
                                                                            
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-md-10">
                                                                                <div class="row">
                                                                                       <div class="col-md-12">
                                                                                            <div class="form-group">
                                                                                                <label class="title">Tanggal Berita Acara</label>
                                                                                                <input id="input-ba-penilaian" type="text"  class="form-control dateJadwalNoTime action-pelaksanaan" > 
                                                                                            </div>
                                                                                            <div class="form-group">
                                                                                                <button class="btn btn-success pull-left save-tgl-ba action-pelaksanaan" attr1="input-ba-penilaian" attr2="BeritaAcaraPenilaian">Simpan</button>
                                                                                            </div>
                                                                                        </div>    
                                                                                    </div>  
                                                                                <div class="row">                                                                                
                                                                                        <div class="col-md-12">
                                                                                            <div class="form-group ">
											                                                    <label class="title">Unggah Berita Acara</label>		
                                                                                                 <div id="BeritaAcaraPenilaian" attr="BeritaAcaraPenilaian" action="/api/PengadaanE/UploadFile?tipe=BeritaAcaraPenilaian" class="dropzone"></div>																
										                                                    </div>
                                                                                      </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-2">
                                                                                <div class="form-group">
                                                                                    <label class="title">Unduh Berita Acara
                                                                                        <a  class="download-dokumen download-berkas" attr1="berkas-penilaian"><i class="fa fa-fw fa-download"></i></a> 
                                                                                    </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>

                                                                </div>
                                                                  <div class="panel box box-primary">
                                                                  <div class="box-header with-border">
                                                                    <h4 class="box-title text-blue-mtf">
                                                                      <a  id="tab-klarifikasi" data-parent="#accordion" href="#collapse5">
                                                                            Klarifikasi & Negoisasi
                                                                      </a>
                                                                        <label id="klarifikasi_aktual" style="font-size:0.8em"></label>
                                                                    </h4>
                                                                      <button class="btn btn-success ubah-jadwal jadwal-klarifikasi action-pelaksanaan" style="float:right;margin-left:10px" attr2="pelaksanaan-klarifikasi" attr3="#jadwal_pelaksanaan_klarifikasi_sampai"   attr1="#jadwal_pelaksanaan_klarifikasi"><i class="fa fa-fw fa-calendar" ></i>Ubah</button>
                                                                      <input type="text" disabled class="form-control dateJadwal jadwal-klarifikasi" style="float:right;margin-left:10px;width:20%" id="jadwal_pelaksanaan_klarifikasi_sampai" />
                                                                      <input type="text" disabled class="form-control dateJadwal jadwal-klarifikasi" style="float:right;width:20%" id="jadwal_pelaksanaan_klarifikasi" />
                                                                  </div>
                                                                  <div id="collapse5" class="panel-collapse collapse">
                                                                    <div class="box-body">
                                                                        <div class="row">
                                                                                <div class="col-md-8">
                                                                                    <div class="form-group">
                                                                                        <textarea class="form-control action-pelaksanaan" id="mKlarifikasi"  rows="5">
                                                                                        </textarea>
                                                                                    </div>
                                                                                </div>
                                                                                 <div class="col-md-4">
                                                                                    <div class="form-group">
                                                                                        <button class="btn btn-success pull-left action-pelaksanaan kirim-undangan-klarifikasi"><i class="fa fa-fw fa-envelope-o"></i>Kirim Undangan Klarifikasi</button>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        <div class="row">
                                                                            <div class="col-md-2">
                                                                                 <div class="form-group">
                                                                                    <a class="btn btn-success pull-left lihat-klarifikasi" href="#"><i class="fa fa-fw fa-calendar-check-o" ></i>Lihat Klarifikasi Kandidat</a>
                                                                                  </div>
                                                                              </div>
                                                                        </div>
                                                                        <label>Daftar Kandidat Klarifikasi:</label>
                                                                        <div class="row list-submit-klarifikasi-rekanan">
                                                                            
                                                                        </div>
                                                                        <label>Pilih Kandidat Pemenang:</label>
                                                                        <div class="row list-rekanan-klarifikasi-penilaian" style="margin-top:15px">                                                                            
                                                                        </div>
                                                                        <div class="row">                                                                            
                                                                            <div class="col-md-10">
                                                                                <div class="row">
                                                                                       <div class="col-md-4">
                                                                                            <div class="form-group">
                                                                                                <label class="title">Tanggal Berita Acara</label>
                                                                                                <input id="input-ba-klarifikasi" type="text"  class="form-control dateJadwalNoTime action-pelaksanaan" > 
                                                                                            </div>
                                                                                            <div class="form-group">
                                                                                                <button class="btn btn-success pull-left save-tgl-ba action-pelaksanaan" attr1="input-ba-klarifikasi" attr2="BeritaAcaraKlarifikasi" >Simpan</button>
                                                                                            </div>
                                                                                        </div>    
                                                                                    </div>  
                                                                                <div class="row">                                                                                
                                                                                      <div class="col-md-12">
                                                                                            <div class="form-group ">
											                                                    <label class="title">Unggah Berita Acara</label>									
											                                                    <div id="BeritaAcaraKlarifikasi" attr="BeritaAcaraKlarifikasi" action="/api/PengadaanE/UploadFile?tipe=BeritaAcaraKlarifikasi" class="dropzone"></div>																			
										                                                    </div>
                                                                                        </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-2">
                                                                                <div class="form-group">
                                                                                    <label class="title">Unduh Berita Acara
                                                                                        <a class="download-berkas" attr1="berkas-klarifikasi"><i class="fa fa-fw  fa-download"></i></a> 
                                                                                    </label>
                                                                                </div>
                                                                            </div>
                                                                            
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                                </div>
                                                                   <div class="panel box box-primary">
                                                                  <div class="box-header with-border">
                                                                    <h4 class="box-title text-blue-mtf">
                                                                      <a  id="tab-penentu-pemenang" data-parent="#accordion" href="#collapse6">
                                                                            Penentuan Pemenang
                                                                      </a>
                                                                        <label id="pemenang_aktual" style="font-size:0.8em"></label>
                                                                    </h4>
                                                                      <button class="btn btn-success ubah-jadwal jadwal-pemenang action-pelaksanaan" style="float:right;margin-left:10px" attr2="pelaksanaan-pemenang"   attr1="#jadwal_pelaksanaan_pemenang"><i class="fa fa-fw fa-calendar" ></i>Ubah</button>
                                                                      <input type="text" disabled class="form-control dateJadwal jadwal-pemenang" style="float:right;margin-left:10px;width:20%" id="jadwal_pelaksanaan_pemenang" />
                                                                  </div>
                                                                  <div id="collapse6" class="panel-collapse collapse">
                                                                    <div class="box-body">
                                                                        <label>Kandidat Terpilih:</label>
                                                                        <div class="row list-rekanan-pemenang" style="margin-top:15px">                                                                            
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-md-10">
                                                                                <div class="row">
                                                                                       <div class="col-md-4">
                                                                                            <div class="form-group">
                                                                                                <label class="title">Tanggal Nota Pemenang</label>
                                                                                                <input id="input-ba-nota" type="text"  class="form-control dateJadwalNoTime action-pelaksanaan" > 
                                                                                            </div>
                                                                                            <div class="form-group">
                                                                                                <button  class="btn btn-success pull-left save-tgl-ba action-pelaksanaan" attr1="input-ba-nota" attr2="BeritaAcaraPenentuanPemenang">Simpan</button>
                                                                                            </div>
                                                                                        </div>    
                                                                                    </div>  
                                                                                <div class="row">                                                                                
                                                                                      <div class="col-md-12">
                                                                                            <div class="form-group ">
											                                                    <label class="title">Unggah Nota Pemenang</label>									
											                                                    <div id="BeritaAcaraPenentuanPemenang" attr="BeritaAcaraPenentuanPemenang" action="/api/PengadaanE/UploadFile?tipe=BeritaAcaraPenentuanPemenang" class="dropzone"></div>																			
										                                                    </div>
                                                                                        </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-2">
                                                                                <div class="form-group">
                                                                                    <label class="title">Unduh Nota Pemenang
                                                                                        <a class="download-berkas" attr1="berkas-pemenang"><i class="fa fa-fw  fa-download"></i></a> 
                                                                                    </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                       <!-- <div class="row">
                                                                             <div class="col-md-10">
                                                                                <div class="row">
                                                                                       <div class="col-md-4">
                                                                                            <div class="form-group">
                                                                                                <label class="title">Tanggal Berita Acara</label>
                                                                                                <input id="Text2" type="text"  class="form-control dateJadwalNoTime" > 
                                                                                            </div>
                                                                                            <div class="form-group">
                                                                                                <button class="btn btn-success pull-left save-tgl-ba" attr1="">Simpan</button>
                                                                                            </div>
                                                                                        </div>    
                                                                                    </div>  
                                                                                <div class="row">                                                                                
                                                                                      <div class="col-md-12">
                                                                                            <div class="form-group ">
											                                                    <label class="title">Upload Lembar Disposisi</label>									
											                                                    <div id="LembarDisposisi" attr="LembarDisposisi" action="/api/PengadaanE/UploadFile?tipe=LembarDisposisi" class="dropzone"></div>																			
										                                                    </div>
                                                                                        </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-2">
                                                                                <div class="form-group">
                                                                                    <label class="title">Unduh Lembar Disposisi
                                                                                        <a class="download-berkas" attr1="lembar-disposisi"><i class="fa fa-fw  fa-download"></i></a> 
                                                                                    </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>-->
                                                                        <div class="row">
                                                                            <div class="col-md-10">
                                                                                <div class="row">
                                                                                       <div class="col-md-4">
                                                                                            <div class="form-group">
                                                                                                <label class="title">Tanggal SPK</label>
                                                                                                <input id="input-ba-spk" type="text"  class="form-control dateJadwalNoTime action-pelaksanaan" > 
                                                                                            </div>
                                                                                            <div class="form-group">
                                                                                                <button class="btn btn-success pull-left save-tgl-ba action-pelaksanaan" attr1="input-ba-spk" attr2="SuratPerintahKerja" >Simpan</button>
                                                                                            </div>
                                                                                        </div>    
                                                                                    </div>  
                                                                                <div class="row">                                                                                
                                                                                      <div class="col-md-12">
                                                                                             <div class="form-group ">
											                                                    <label class="title">Unggah Surat Perintah Kerja (SPK)</label>									
											                                                    <div id="SuratPerintahKerja" attr="SuratPerintahKerja" action="/api/PengadaanE/UploadFile?tipe=SuratPerintahKerja" class="dropzone"></div>																			
										                                                    </div>
                                                                                        </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-2">
                                                                                <div class="form-group">
                                                                                    <label class="title">Unduh Template Surat Perintah Kerja
                                                                                        <a class="download-berkas" attr1="lembar-spk"><i class="fa fa-fw  fa-download"></i></a> 
                                                                                    </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                                </div>
                                                                  <!-- <div class="panel box box-primary">
                                                                  <div class="box-header with-border">
                                                                    <h4 class="box-title text-blue-mtf">
                                                                      <a data-toggle="collapse" data-parent="#accordion" href="#collapse7">
                                                                            Personil
                                                                      </a>
                                                                    </h4>
                                                                  </div>
                                                                  <div id="collapse7" class="panel-collapse collapse">
                                                                    <div class="box-body">
                                                                        <div class="row">	
                                                                <div class="col-md-12">

											                        <!--<label > PIC</label>
                                                                    <a id="addPerson-pic" attr1=".listperson-pic" style="margin-top:10px;margin-bottom:10px" class="addPerson btn btn-sm btn-social btn-bitbucket">		
                                                                         <i class="fa fa-plus"></i>
                                                                        Rubah PIC
                                                                        </a>							
											                        <div class="listperson-pic">																												
											                        </div>												
										                        </div>
                                                              <div class="col-md-12">
											                        <label >Tim</label>										
											                        <div class="listperson-tim">																												
											                        </div>												
										                        </div>
                                                                <div class="col-md-12">
											                        <label >User</label>										
											                        <div class="listperson-staff">	
                                                                        
											                        </div>												
										                        </div>
                                                                 <div class="col-md-12">
											                        <label >Controller</label>										
											                        <div class="listperson-controller">
                                                                             
                                                                    </div>												
										                        </div>
                                                                <div class="col-md-12">
											                        <label >Compliance</label>										
											                        <div class="listperson-compliance">  
                                                                    </div>												
										                        </div>
														    </div>
                                                                    </div>
                                                                  </div>
                                                                </div>-->
                                                              </div>
                                                            </div><!-- /.box-body -->
                                                    </div>
                                                    <div id="berkas" class="tab-pane"> 
                                                          <div class="row">
															    <div class="col-md-12">
																    <div class=" panel-header-white">
																	    <span class="title-header" id="Span1">Berkas Rencana Kerja  </span>
																    </div>
															    </div>
														    </div>
                                                        <div class="row">
															<div class="col-md-12">
                                                                <div class="form-group col-md-6">
																    <label class="title">Nota Internal</label>
																    <label class="deskripsi" id="Label1"> </label>
															        <div id="BerkasDokumenNotaInternal" attr="NOTA" class="dropzone"></div>		
                                                                </div>
                                                                    <div class="form-group col-md-6">
																    <label class="title">Dokumen Lainnya </label>
																    <label class="deskripsi" id="Label2"></label>
															        <div id="BerkasDokumenLain" attr="DOKUMENLAIN" class="dropzone"></div>	
                                                                </div>	
                                                                <div class="form-group col-md-6">
																    <label class="title">Berkas Rujukan </label>
                                                                    <label class="deskripsi" id="Label3"> </label>
																    <div id="BerkasBerkasRujukanLain" attr="BerkasRujukanLain"   class="dropzone"></div>	
															    </div>                                                                    
                                                            </div>                                                               
                                                         </div>   
                                                         <div class="row">
															<div class="col-md-12">
																<div class=" panel-header-white">
																	<span class="title-header" id="Span2">Berkas Pelaksanaan </span>
																</div>
															</div>
                                                             <div class="col-md-12">
                                                                <div class="col-md-6">
                                                                    <label class="title">Berita Acara Aanwijzing</label>
                                                                    <div class="form-group ">									
													                    <div id="BerkasBeritaAcaraAanwijzing" attr="BeritaAcaraAanwijzing" action="/api/PengadaanE/UploadFile?tipe=BeritaAcaraAanwijzing" class="dropzone"></div>																												
										                                
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group ">
											                            <label class="title">Berita Acara Buka Amplop</label>		
                                                                            <div id="BerkasBeritaAcaraBukaAmplop" attr="BeritaAcaraBukaAmplop" action="/api/PengadaanE/UploadFile?tipe=BeritaAcaraBukaAmplop" class="dropzone"></div>																
										                            </div>
                                                                </div>	
                                                                <div class="col-md-6">
                                                                    <div class="form-group ">
											                            <label class="title">Berita Acara Klarifikasi</label>									
											                            <div id="BerkasBeritaAcaraKlarifikasi" attr="BeritaAcaraPenentuanPemenang" action="/api/PengadaanE/UploadFile?tipe=BeritaAcaraKlarifikasi" class="dropzone"></div>																			
										                            </div>
                                                                </div>   
                                                                 <div class="col-md-6">
                                                                    <div class="form-group ">
											                            <label class="title">Berita Acara Penentuan Pemenang</label>									
											                            <div id="BerkasBeritaAcaraPenentuanPemenang" action="/api/PengadaanE/UploadFile?tipe=PenentuanPemenang" class="dropzone"></div>																			
										                            </div>
                                                                </div>    
                                                                 <!--<div class="col-md-6">
                                                                    <div class="form-group ">
											                            <label class="title">Berkas Lembar Disposisi</label>									
											                            <div id="BerkasLembarDisposisi" attr="LembarDisposisi" action="/api/PengadaanE/UploadFile?tipe=LembarDisposisi" class="dropzone"></div>																			
										                            </div>
                                                                </div>-->
                                                                  
                                                                <div class="col-md-6">
                                                                    <div class="form-group ">
											                            <label class="title">Upload Surat Perintah Kerja (SPK)</label>									
											                            <div id="BerkasSuratPerintahKerja" attr="SuratPerintahKerja" action="/api/PengadaanE/UploadFile?tipe=SuratPerintahKerja" class="dropzone"></div>																			
										                            </div>
                                                                </div>                                                 
                                                            </div>   
														</div>   
                                                        <div class="row">
															<div class="col-md-12">
																<div class=" panel-header-white">
																	<span class="title-header" id="Span3">Daftar Kandidat </span>
																</div>
															</div>
														</div> 
                                                        <div class="row list-berkas-info-kandidat">
                                                                            
                                                        </div>
                                                    </div>
                                                   
                                                </div>
										    </div>										
								        </form>
				                </div>
                            </div>
				    </div>
				</div>	
			
			
			
		</section><!-- /.content -->
		</div>
	    
        <aside class="control-sidebar control-sidebar-light control-sidebar-open" id="side-kanan">
        <!-- Create the tabs -->
        
            <ul class="nav nav-tabs nav-stacked"   >
				<li><a href="#section-1">Atas</a></li>
				<li class="rk"><a href="#section-2">Anggaran</a></li>
				<li class="rk"><a href="#section-3">Korespondensi Internal</a></li>
                <li class="rk"><a href="#section-4">Spesifikasi</a></li>
				<li class="rk"><a href="#section-5">Kandidat</a></li>
				<li class="rk"><a href="#section-6">Jadwal</a></li>
				<li class="rk"><a href="#section-7">Personil</a></li>
                <li class="pl"><a href="#accordion">Aanwijzing </a></li>
                <li class="pl"><a href="#accordion">Submit Penawaran </a></li>
                <li class="pl"><a href="#accordion">Buka Amplop </a></li>
                <li class="pl"><a href="#accordion">Penilaian Kandidat </a></li>
                <li class="pl"><a href="#accordion">Klarifikasi</a></li>
                <li class="pl"><a href="#accordion">Penentuan Pemenang</a></li>
			</ul>
            <!--<div class="box box-white">
			    <div class="box-header with-border">
				    <h3 class="box-title text-blue-mtf box-riwayat"><a href="riwayat-pengadaan.html">Riwayat</a></h3>
			    </div>
			    <div class="box-body" ng-controller="history" >
				    <ul class="products-list product-list-in-box" >
                            <li class="item" ng-repeat="histori in history" style="padding-top:1px">						
						    <div class="riwayat-info">
							    <span class="title" ><a href="purchase-requests-detail.html" ng-bind="histori.nama"></a></span>
							    <span class="pegadaan-description" ng-bind="histori.tanggal">
							    </span>
							    <span class="pegadaan-item" ng-bind="histori.deskripsi">
							    </span>
						    </div>	
					    </li>
				    </ul>
			    </div>
			</div>-->
        <!-- Tab panes -->
      
      </aside><!-- /.control-sidebar -->
      <!-- Add the sidebar's background. This div must be placed
           immediately after the control sidebar -->
      <div class="control-sidebar-bg"></div>	
       

    </div><!-- ./wrapper -->
	
	
<!-- Modal -->
<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
        <h4 class="modal-title">Add Item</h4>
      </div>

	<div class="modal-body ">
        <iframe id="info" class="iframe" name="info" seamless="" height="100%" width="100%"></iframe>
      </div>

    </div>
  </div>
</div> <!-- /#myModal -->

     <div class="modal" id="personilModal" role="dialog"  aria-hidden="true">
  <div class="modal-dialog">
	<div class="modal-content">
	  <div class="modal-header">
		<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
		<h4 class="modal-title">Daftar Personil</h4>
	  </div>
	  <div class="modal-body">
          <input type="hidden" id="tipe-person-list" />
		 <div class="box box-primary" ng-controller="PersonCtrl">
                <div class="box-header with-border">                  
                     <h3 class="box-title">
                         <div class="input-group">
                            <input type="text"  class="form-control" placeholder="cari personil" ng-model="search" id="search" />
                            <div class="input-group-addon"  >
                              <a href="#search"><i class="glyphicon glyphicon-search" ng-click="searchPerson()"></i></a>
                            </div>
                          </div>
                      </h3>
                </div>
                <div class="box-body">
                  <ul class="products-list pop-personil-list product-list-in-box">
                    <li class="item" ng-repeat="person in persons" ng-click="getPerson($event,person)">   
	                  <!--  <div class="product-img">
		                    <img  src="{{person.logo}}">
	                    </div>-->
	                    <div class="product-info-collaps">
		                    <input class="id-personil" type="hidden" value="{{person.Jabatan}}" />
		                    <a class="product-title nama" ng-bind="person.Nama" ></a>
		                    <span class="pegadaan-description devisi" ng-bind="person.Jabatan"></span>
	                    </div>
                    </li>
                  </ul>
                </div><!-- /.box-body -->
                <div class="box-footer text-center">
                  <a class="uppercase" id="pop-personil-next" ng-click="nextPerson($event,person)">Berikutnya</a>
                </div><!-- /.box-footer -->
              </div>
	  </div>
	  <div class="modal-footer">
		<button type="button" class="btn btn-outline " data-dismiss="modal">Batal</button>		
	  </div>
	</div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->

<!--<div id="view-siup" class="modal fade" role="dialog">
	<div class="modal-dialog modal-lg">
	<input type="hidden" id="row_index" />
	
	<div class="modal-content">
		<div class="modal-header">
		<button type="button" class="close" data-dismiss="modal">&times;</button>
		</div>
		<div class="modal-body">
		<div>
            <iframe width="100%" style="min-height: 500px" src="/ViewerJS/#../contoh.pdf"  allowfullscreen webkitallowfullscreen></iframe>
		</div>
		</div>
	</div>
	</div>
</div>-->

<!--kilk persetujuan-->
     <!-- Modal -->
<div class="modal" id="modal-persetujuan" role="dialog" aria-hidden="true">
  <div class="modal-dialog">
	<div class="modal-content">
	  <div class="modal-header">
		<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
		<h4 class="modal-title">Konfirmasi</h4>
          <div class="modal-body">              
                Dengan menekan tombal "lanjut" dokumen pengadaan ini akan diajukan pada Departemen Head Pengadaaan untuk dimintai persetujuan
          </div>
	  </div>
	  <div class="modal-footer">
        <button type="button" id="anjukan-lanjutkan" class="btn btn-default">Lanjut</button>	
        <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>		
	  </div>
	</div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->

     <!--kilk persetujuan-->
     <!-- Modal -->
<div class="modal" id="modal-setujui" role="dialog" aria-hidden="true">
  <div class="modal-dialog">
	<div class="modal-content">
	  <div class="modal-header">
		<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
		<h4 class="modal-title">Konfirmasi</h4>
          <div class="modal-body">              
                Dengan menekan tombol "setujui" berarti pengadaan ini memiliki ketetapan akan dilaksanakan oleh sistem
          </div>
	  </div>
	  <div class="modal-footer">
        <button type="button" id="lanjut-setujui-aja" class="btn btn-default">Lanjut</button>	
        <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>		
	  </div>
	</div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->

<div class="modal" id="modal-ditolak" role="dialog" aria-hidden="true">
  <div class="modal-dialog">
	<div class="modal-content">
	  <div class="modal-header">
		<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
		<h4 class="modal-title">Konfirmasi</h4>
          <div class="modal-body">              
                <p>Anda Yakin Menolak Pengadaan Ini?</p>
                <p>Alasan Penolakan:</p>
                <textarea class="form-control" id="keterangan_penolakan"></textarea>
          </div>
	  </div>
	  <div class="modal-footer">
        <button type="button" id="lanjut-tolak" class="btn btn-default">Lanjut</button>	
        <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>		
	  </div>
	</div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->
     <!-- Modal -->
<div class="modal" id="konfirmasiFile" role="dialog" FileId="" attr1=""  aria-hidden="true">
  <div class="modal-dialog">
	<div class="modal-content">
	  <div class="modal-header">
		<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
		<h4 class="modal-title">Konfirmasi</h4>
	  </div>
	  <div class="modal-footer">
        <button type="button" id="downloadFile" class="btn btn-default">Download</button>
        <button type="button" id="HapusFile" class="btn btn-default">Hapus</button>	
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>		
	  </div>
	</div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->
<div class="modal" id="KofirmasiBeritaAcaraAanwjizing" role="dialog" FileId="" attr1=""  aria-hidden="true">
  <div class="modal-dialog">
	<div class="modal-content">
	  <div class="modal-header">
		<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
		<h4 class="modal-title">Konfirmasi</h4>
          <div class="modal-body">              
               Dengan diuploadnya Berita Acara ini maka Pengadaan akan dilanjutkan ke tahap selanjutnya
          </div>
	  </div>
	  <div class="modal-footer">
        <button type="button" id="lanjutkanBeritaAanwjzing" class="btn btn-default">Lanjutkan</button>
        <button type="button" id="batalkanBeritaAanwjzing" class="btn btn-default">Batal</button>			
	  </div>
	</div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->
<div id="dialog-confirm" title="Konfirmasi!" hidden>
  <p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>
      Dengan Mengunggah Berkas Bearti akan berlanjut ke state berikutnya?</p>
</div>
    <!-- jQuery 2.1.4 -->
     
    <script src="plugins/jQuery/jQuery-2.1.4.min.js"></script>
    <!-- Bootstrap 3.3.5 -->
    <script src="bootstrap/js/bootstrap.min.js"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <!-- FastClick -->
    <script src="plugins/fastclick/fastclick.min.js"></script>
  
	<script src="plugins/datatables/jquery.dataTables.min.js"></script>
	<script src="plugins/datatables/dataTables.bootstrap.min.js"></script>
	<script src="plugins/jQueryUItotop/easing.js"></script>
	<script src="plugins/jQueryUItotop/jquery.ui.totop.js"></script>
	<script src="plugins/JQueryValidation/jquery.validate.js"></script>
     <script src="plugins/bootstrap-dialog/bootstrap-dialog.min.js"></script>

	<script src="plugins/jQueryUI/jquery-ui-autocomplete.js"></script>
	<script src="plugins/accounting/accounting.js"></script>	
	
	 <script src='plugins/fullcalendar/moment.min.js'></script>
    <script src='plugins/fullcalendar/fullcalendar.min.js'></script>
    <script src='plugins/fullcalendar/lang/id.js'></script>
     <!--<script src="plugins/datepicker/bootstrap-datepicker.js"></script>-->
     <script src="datetimepicker/bootstrap-datetimepicker.js"></script>
      <script src="datetimepicker/locales/bootstrap-datetimepicker.id.js"></script>
     <script src="dist/js/app.min.js"></script>
     <!--<script src="chatjs/chat.js"></script>-->
    <script src="plugins/angular/angular.min.js"></script>
     <script src="dropzone.js"></script>
	  <script>
	 
	  
    </script>
  </body>
</html>
