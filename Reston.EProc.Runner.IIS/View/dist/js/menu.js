var app = angular.module("app", []);

app.controller('side-menu', ['$scope', '$http', function ($scope, $http) {
    $scope.menus = [];
    
    $http.get("Api/Header/GetMenu")
       .then(function (response) {
           $scope.menus = response.data;
       });
}]);

app.controller('user', ['$scope', '$http', function ($scope, $http) {
    $scope.user = {};
    $http.get("Api/Header/User")
       .then(function (response) {
           $scope.user = response.data;
       });
}]);