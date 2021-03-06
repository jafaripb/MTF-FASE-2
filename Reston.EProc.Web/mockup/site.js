var LOGIN_PAGE = 'http://localhost:7348/';
var HOME_PAGE = 'http://localhost:49559/';
var UNAUTHORIZED_CODE = '403';
var SUCCESS_CODE = '200';
var ERROR_CODE = '0';
var ADMIN = 'http://localhost:49559/master.html';
var MANAGER = 'http://localhost:49559/pengadaan-list.html';
var STAFF = 'http://localhost:49559/pengadaan-list.html';
var REKANAN = 'http://localhost:49559/rekanan-side-terdaftar.html';
var ENDUSER = 'http://localhost:49559/pengadaan-list.html';
var COMPLIANCE = 'http://localhost:49559/pengadaan-list.html';
var HEAD = 'http://localhost:49559/pengadaan-list.html';

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function ajaxCompleteProcess(xhr) {
    if (xhr.status == UNAUTHORIZED_CODE) {//Authenticated, but no access
        window.location = HOME_PAGE;
    }
    else if (xhr.status == ERROR_CODE) {//not authenticated
        //console.log(xhr);
        window.location = LOGIN_PAGE;
    }
}

function cekLandingPage() {
    $.ajax({
        url: "Api/PengadaanE/cekRole",
        method: "POST",
        complete: function (e, xhr, settings) {
            var data = e.responseJSON;
            if (data.message.indexOf("eproc_superadmin") >= 0) {
                window.location.replace(ADMIN);
            }
            else if (data.message.indexOf("procurement_staff") >= 0) {
                window.location.replace(STAFF);
            }
            if (data.message.indexOf("procurement_manager") >= 0) {
                window.location.replace(MANAGER);
            }
            if (data.message.indexOf("rekanan_terdaftar") >= 0) {
                window.location.replace(REKANAN);
            }
            if (data.message.indexOf("end_user") >= 0) {
                window.location.replace(ENDUSER);
            }
            if (data.message.indexOf("compliance") >= 0) {
                window.location.replace(COMPLIANCE);
            }
            if (data.message.indexOf("procurement_head") >= 0) {
                window.location.replace(HEAD);
            }
        }

    });
}
function cekLogin(cek) {
    $.ajax({
        url: "Api/PengadaanE/cekLogin",
        method: "POST",
        complete: function (e, xhr, settings) {
            ajaxCompleteProcess(e);
            if (e.status == 200 && cek == 1)
                cekLandingPage();
        }

    });
}

function LogOut() {
    $.ajax({
        url: "Api/PengadaanE/logOut",
        method: "POST",
        complete: function (e, xhr, settings) {
            window.location("http://" + window.location.host + "/index.html");
            
        }

    });
}


cekLogin(0);