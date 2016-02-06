"use strict";

define(['application-configuration', 'accountsService', 'alertsService'], function (app) {

    app.register.controller('registerController', ['$scope', '$rootScope', 'accountsService', 'alertsService',
        function ($scope, $rootScope, accountsService, alertsService) {

            $rootScope.closeAlert = alertsService.closeAlert;
            $rootScope.alerts = [];

            $scope.initializeController = function () {

                $scope.FirstName = "";
                $scope.LastName = "";
                $scope.UserName = "";
                $scope.EmailAddress = "";
                $scope.Password = "";
                $scope.PasswordConfirmation = "";

            }

            $scope.registerUser = function () {
                var user = $scope.createUser();
                accountsService.registerUser(user, $scope.registerUserCompleted, $scope.registerUserError);
            }

            $scope.registerUserCompleted = function (response) {
                window.location = "/applicationMasterPage.html#/Customers/CustomerInquiry";
            }

            $scope.registerUserError = function (response) {

                alertsService.RenderErrorMessage(response.ReturnMessage);
                $scope.clearValidationErrors();              
                alertsService.SetValidationErrors($scope, response.ValidationErrors);

            }

            $scope.clearValidationErrors = function () {

                $scope.FirstNameInputError = false;
                $scope.LastNameInputError = false;
                $scope.UserNameInputError = false;
                $scope.EmailAddressInputError = false;
                $scope.PasswordInputError = false;
                $scope.PasswordConfirmationInputError = false;

            }

            $scope.createUser = function () {

                var user = new Object();

                user.FirstName = $scope.FirstName;
                user.LastName = $scope.LastName;
                user.UserName = $scope.UserName;
                user.EmailAddress = $scope.EmailAddress;
                user.Password = $scope.Password;
                user.PasswordConfirmation = $scope.PasswordConfirmation;

                return user;

            }

        }]);
});
