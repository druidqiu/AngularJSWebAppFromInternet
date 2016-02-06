"use strict";

define(['application-configuration', 'mainService', 'alertsService'], function (app) {

    app.register.controller('logoutController', ['$scope', '$rootScope', 'mainService', 'alertsService', function ($scope, $rootScope, mainService, alertsService) {

        $scope.initializeController = function () {
            mainService.logout($scope.logoutComplete, $scope.logoutError);
        }

        $scope.logoutComplete = function (response) {                 
            window.location = "/index.html";
        }

        $scope.logoutError = function (response) {
            alertsService.RenderErrorMessage(response.ReturnMessage);
        }

    }]);
});