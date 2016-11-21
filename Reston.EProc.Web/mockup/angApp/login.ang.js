///#source 1 1 /App/models.js
/// <reference path="_all.ts" />
var meetometer;
(function (meetometer) {
    'use strict';
    var authSettingsModel = (function () {
        function authSettingsModel(isAuthorized, token) {
            this.isAuthorized = isAuthorized;
            this.token = token;
        }
        return authSettingsModel;
    })();
    meetometer.authSettingsModel = authSettingsModel;
    var meetingModel = (function () {
        function meetingModel(id, date, people, avgSalary, durationSeconds) {
            this.id = id;
            this.date = date;
            this.people = people;
            this.avgSalary = avgSalary;
            this.durationSeconds = durationSeconds;
        }
        return meetingModel;
    })();
    meetometer.meetingModel = meetingModel;
})(meetometer || (meetometer = {}));
//# sourceMappingURL=models.js.map

///#source 1 1 /App/directives.js
/// <reference path="_all.ts" />
var meetometer;
(function (meetometer) {
    'use strict';

    function sliderInitDirective() {
        return {
            link: function link(scope, element, attrs) {
                var model = scope.$eval(attrs.ngModel);
                var unwatch = scope.$watch(model, function (newValue) {
                    if (newValue) {
                        element.slider('refresh');
                        unwatch();
                    }
                });
            }
        };
    }
    meetometer.sliderInitDirective = sliderInitDirective;
    ;
})(meetometer || (meetometer = {}));
//# sourceMappingURL=directives.js.map

///#source 1 1 /App/authService.js
/// <reference path="_all.ts" />
var meetometer;
(function (meetometer) {
    'use strict';
    function authService(storageService, $http, $q) {
        var _authData = storageService.getAuthSettings();

        var _authentication = {
            isAuthorized: _authData.isAuthorized,
            token: _authData.token
        };

        var _logout = function () {
            _authentication.isAuthorized = false;
            _authentication.token = "";
            storageService.saveAuthSettings(new meetometer.authSettingsModel(false, ""));
        };

        var _login = function (username, password) {
            var data = "grant_type=password&username=" + username + "&password=" + password;

            var defered = $q.defer();

            $http.post("/Token", data, { header: { 'Content-Type': 'x-www-form-urlencoded' } }).success(function (response) {
                _authentication.isAuthorized = true;
                _authentication.token = response.access_token;
                storageService.saveAuthSettings(new meetometer.authSettingsModel(true, response.access_token));
                defered.resolve(response);
            }).error(function (error) {
                defered.reject(error);
            });

            return defered.promise;
        };

        return {
            authentication: _authentication,
            login: _login,
            logout: _logout
        };
    }
    meetometer.authService = authService;
})(meetometer || (meetometer = {}));
//# sourceMappingURL=authService.js.map

///#source 1 1 /App/authInterceptor.js
/// <reference path="_all.ts" />
var meetometer;
(function (meetometer) {
    'use strict';
    function authInterceptor(storageService, $q, $injector) {
        var _request = function (config) {
            var authSettings = storageService.getAuthSettings();
            if (authSettings.isAuthorized) {
                config.headers['Authorization'] = 'Bearer ' + authSettings.token;
            }

            return config;
        };

        var _responseError = function (rejection) {
            if (rejection.status == 401) {
                var authService = $injector.get("authService");
                authService.logout();
            }

            return $q.reject(rejection);
        };

        return {
            request: _request,
            responseError: _responseError
        };
    }
    meetometer.authInterceptor = authInterceptor;
})(meetometer || (meetometer = {}));
//# sourceMappingURL=authInterceptor.js.map

///#source 1 1 /App/meetingController.js
/// <reference path="_all.ts" />
var meetometer;
(function (meetometer) {
    'use strict';

    var meetingController = (function () {
        function meetingController($scope, $window, $http, $timeout, storageService, authService) {
            var _this = this;
            this.$scope = $scope;
            this.$http = $http;
            this.$timeout = $timeout;
            this.storageService = storageService;
            this.authService = authService;
            this.$window = $window;
            $scope.authentication = authService.authentication;

            $scope.loginErrorMessage = "";
            $scope.username = "super";
            $scope.password = "123456";


            $scope.vm = this;
        }


        meetingController.prototype.logout = function () {
            this.authService.logout();
        };

        meetingController.prototype.login = function () {
            var _self = this;

            // do the login
            this.authService.login(this.$scope.username, this.$scope.password).then(function (response) {
                _self.$scope.loginErrorMessage = "";
               // console.log(_self.$http.path('/ss').replace());
                
                _self.$window.location.href = "/index.html";

            }, function (response) {
                _self.$scope.loginErrorMessage = "Failed to login";

                $(".alert").show();
            });
        };

        meetingController.$inject = ["$scope", "$window", "$http", "$timeout", "storageService", "authService"];
        return meetingController;
    })();
    meetometer.meetingController = meetingController;
})(meetometer || (meetometer = {}));
//# sourceMappingURL=meetingController.js.map

///#source 1 1 /App/storageService.js
/// <reference path="_all.ts" />
var meetometer;
(function (meetometer) {
    'use strict';

    var storageService = (function () {
        function storageService() {
            this.meetometerAuthSettingsKey = "meetometerAuthSettingsKey";
            this.meetometerSettingsKey = "meetometerSettingsKey";
            this.meetometerMeetometerKey = "meetometerMeetometerKey";
        }
        storageService.prototype.getAuthSettings = function () {
            var settings = amplify.store(this.meetometerAuthSettingsKey);
            if (!settings) {
                settings = new meetometer.authSettingsModel(false, "");
            }

            return settings;
        };

        storageService.prototype.saveAuthSettings = function (settings) {
            amplify.store(this.meetometerAuthSettingsKey, settings);
        };

        return storageService;
    })();
    meetometer.storageService = storageService;
})(meetometer || (meetometer = {}));
//# sourceMappingURL=storageService.js.map

///#source 1 1 /App/main.js
/// <reference path="_all.ts" />
var meetometer;
(function (meetometer) {
    'use strict';

    angular.module("app", []).directive("sliderInit", meetometer.sliderInitDirective).factory("authService", ["storageService", "$http", "$q", meetometer.authService]).factory("authInterceptor", ["storageService", "$q", "$injector", meetometer.authInterceptor]).service("storageService", meetometer.storageService).controller("meetingController", meetometer.meetingController).config([
        '$httpProvider', function ($httpProvider) {
            $httpProvider.interceptors.push('authInterceptor');
        }]);
})(meetometer || (meetometer = {}));
//# sourceMappingURL=main.js.map

