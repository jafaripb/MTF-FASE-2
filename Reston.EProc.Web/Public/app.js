'use strict';

angular
	.module('app', ['ngRoute', 'ngMaterial'])
	.config(['$routeProvider', 
		function($routeProvider) {
  			$routeProvider.otherwise({redirectTo: '/dashboard'});
  		}
  	])
	.controller('mainCtrl', function($scope, $mdSidenav, $mdMedia, $location){	
		$scope.openModule = function(route){
			$location.path(route);
		};
		$scope.goto = function(route){
			$location.path(route);
		};
	})
;