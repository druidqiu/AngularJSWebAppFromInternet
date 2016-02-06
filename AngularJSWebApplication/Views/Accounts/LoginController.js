"use strict";

define(['application-configuration', 'accountsService', 'alertsService'], function (app) {

    app.register.controller('loginController', ['$scope', '$rootScope', 'accountsService', 'alertsService',
        function ($scope, $rootScope, accountsService, alertsService) {

            $rootScope.closeAlert = alertsService.closeAlert;
            $rootScope.alerts = [];

            $scope.initializeController = function () {
               
                $scope.UserName = "billgates";
                $scope.Password = "microsoft";

                alertsService.RenderSuccessMessage("Please register if you do not have an account.");

            }

            $scope.login = function () {
                $rootScope.IsloggedIn = false;
                var user = $scope.createLoginCredentials();
                accountsService.login(user, $scope.loginCompleted, $scope.loginError);
            }

            $scope.loginCompleted = function (response) {
                $rootScope.MenuItems = response.MenuItems;             
                window.location = "/applicationMasterPage.html#/Customers/CustomerInquiry";
            }

            $scope.loginError = function (response) {

                alertsService.RenderErrorMessage(response.ReturnMessage);
        
                $scope.clearValidationErrors();
                alertsService.SetValidationErrors($scope, response.ValidationErrors);              

            }

            $scope.clearValidationErrors = function () {
              
                $scope.UserNameInputError = false;               
                $scope.PasswordInputError = false;               

            }

            $scope.createLoginCredentials = function () {

                var user = new Object();
               
                user.UserName = $scope.UserName;              
                user.Password = $scope.Password;
             
                return user;

            }

        }]);
});
