"use strict";

define(['application-configuration', 'accountsService', 'alertsService'], function (app) {

    app.register.controller('myAccountController', ['$scope', '$rootScope', 'accountsService', 'alertsService',
        function ($scope, $rootScope, accountsService, alertsService) {

            $rootScope.closeAlert = alertsService.closeAlert;        
            $rootScope.applicationModule = "Main";
            $rootScope.alerts = [];

            $scope.initializeController = function () {

                $scope.FirstName = "";
                $scope.LastName = "";
                $scope.UserName = "";
                $scope.EmailAddress = "";
                $scope.Password = "";
                $scope.PasswordConfirmation = "";

                accountsService.getUser($scope.getUserCompleted, $scope.getUserError);

            }
        
            $scope.getUserCompleted = function (response) {

                $scope.clearValidationErrors();

                $scope.FirstName = response.User.FirstName;
                $scope.LastName = response.User.LastName;
                $scope.UserName = response.User.UserName;
                $scope.EmailAddress = response.User.EmailAddress;
                $scope.Password = response.User.Password;
                $scope.PasswordConfirmation = response.User.Password;

            }

            $scope.getUserError = function (response) {

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

            $scope.updateUser = function () {
                var user = $scope.createUser();
                accountsService.updateUser(user, $scope.updateUserCompleted, $scope.updateUserError);
            }

            $scope.updateUserCompleted = function (response) {
                $scope.clearValidationErrors();
                alertsService.RenderSuccessMessage(response.ReturnMessage);               
            }

            $scope.updateUserError = function (response) {

                alertsService.RenderErrorMessage(response.ReturnMessage);
                $scope.clearValidationErrors();              
                alertsService.SetValidationErrors($scope, response.ValidationErrors);

            }

        }]);
});
