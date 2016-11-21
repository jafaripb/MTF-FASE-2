
  
function UnformatFloat(value) {
    if (value != null && value!="")
        return value.toString().replace(/\./g, "");
    else value;
}

function isGuid(value) {
    var regex = /[a-f0-9]{8}(?:-[a-f0-9]{4}){3}-[a-f0-9]{12}/i;// /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
    var match = regex.exec(value);
    return match != null;
}


function SetListRegion(el) {
    $.ajax({
        url: "/api/ReferenceData/GetAllRegion",
        success: function (data) {
            for (var i in data) {
                $(el).append("<option value='" + data[i].Name + "'>" + data[i].Name + "</option>");
            }
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

function SetListProvinsi(el) {
    $.ajax({
        url: "/api/ReferenceData/GetAllProvinsi",
        success: function (data) {
            for (var i in data) {
                $(el).append("<option value='" + data[i].Name + "'>" + data[i].Name + "</option>");
            }
        }
    });
}

function SetListPeriode(el) {
    $.ajax({
        url: "/api/ReferenceData/GetAllPeriodeAnggaran",
        success: function (data) {
            for (var i in data) {
                $(el).append("<option value='" + data[i].Name + "'>" + data[i].Name + "</option>");
            }
        }
    });
}

function SetListUnitKerja(el) {
    $.ajax({
        url: "/api/ReferenceData/GetAllUnitKerja",
        success: function (data) {
            for (var i in data) {
                $(el).append("<option value='" + data[i].Name + "'>" + data[i].Name + "</option>");
            }
        }
    });
}

function SetListJenisPekerjaan(el) {
    $.ajax({
        url: "/api/ReferenceData/GetAllJenisPekerjaan",
        success: function (data) {
            for (var i in data) {
                $(el).append("<option value='" + data[i].Name + "'>" + data[i].Name + "</option>");
            }
        }
    });
}

function SetListJenisPembelanjaan(el) {
    $.ajax({
        url: "/api/ReferenceData/GetAllJenisPembelanjaan",
        success: function (data) {
            for (var i in data) {
                $(el).append("<option value='" + data[i].Name + "'>" + data[i].Name + "</option>");
            }
        }
    });
}

/**
 * Module for displaying "Waiting for..." dialog using Bootstrap
 *
 * @author Eugene Maslovich <ehpc@em42.ru>
 */

var waitingDialog = waitingDialog || (function ($) {
    'use strict';

    // Creating modal dialog's DOM
    var $loading = $(
		'<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
		'<div class="modal-dialog modal-m">' +
		'<div class="modal-content">' +
			'<div class="modal-header"><h3 style="margin:0;"></h3></div>' +
			'<div class="modal-body">' +
				'<div class="progress progress-striped active" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>' +
			'</div>' +
		'</div></div></div>');


    var $dialogresult = $(
		'<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
		'<div class="modal-dialog modal-m">' +
		'<div class="modal-content">' +
			'<div class="modal-header"><h3 id="modalheader" style="margin:0;"></h3></div>' +
			'<div class="modal-body">' +
				'<h3 id="modalmessages" style="margin-bottom:0;"></h3>' +
			'</div>' +
		'</div></div></div>');



    return {
        /**
		 * Opens our dialog
		 * @param message Custom message
		 * @param options Custom options:
		 * 				  options.dialogSize - bootstrap postfix for dialog size, e.g. "sm", "m";
		 * 				  options.progressType - bootstrap postfix for progress bar type, e.g. "success", "warning".
		 */
        showloading: function (message, options) {
            // Assigning defaults
            if (typeof options === 'undefined') {
                options = {};
            }
            if (typeof message === 'undefined') {
                message = 'Loading';
            }
            var settings = $.extend({
                dialogSize: 'm',
                progressType: '',
                onHide: null // This callback runs after the dialog was hidden
            }, options);

            // Configuring dialog
            $loading.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
            $loading.find('.progress-bar').attr('class', 'progress-bar');
            if (settings.progressType) {
                $loading.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
            }
            $loading.find('h3').text(message);
            // Adding callbacks
            if (typeof settings.onHide === 'function') {
                $loading.off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
                    settings.onHide.call($loading);
                });
            }
            // Opening dialog
            $loading.modal();
        },



        showresult: function (messageheader, messagebody) {

            if (typeof messageheader === 'undefined') {
                messageheader = 'Message';
            }

            if (typeof messagebody === 'undefined') {
                messagebody = '..........';
            }


            $dialogresult.find('#modalheader').text(messageheader);
            $dialogresult.find('#modalmessages').text(messagebody);
            // Adding callbacks

            // Opening dialog
            $dialogresult.modal();
        },
        /**
		 * Closes dialog
		 */

        hideresult: function () {
            $dialogresult.modal('hide');
        },


        hideloading: function () {
            $loading.modal('hide');
        }
    };

})(jQuery);
