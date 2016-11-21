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
