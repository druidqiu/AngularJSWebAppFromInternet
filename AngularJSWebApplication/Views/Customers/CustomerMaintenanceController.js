"use strict";

define(['application-configuration', 'customersService', 'alertsService'], function (app) {

    app.register.controller('customerMaintenanceController', ['$scope', '$rootScope', '$routeParams', 'customersService', 'alertsService',
    function ($scope, $rootScope, $routeParams, customerService, alertsService) {

        $scope.initializeController = function () {

            var customerID = ($routeParams.id || "");

            $rootScope.applicationModule = "Customers";         
            $rootScope.alerts = [];

            $scope.CustomerID = customerID;

            if (customerID == "") {

                $scope.CustomerCode = "MICRO";
                $scope.CompanyName = "Microsoft";
                $scope.Address = "One Microsoft Way";
                $scope.City = "Redmond";
                $scope.Region = "WA";
                $scope.PostalCode = "98052-6399";
                $scope.CountryCode = "USA";
                $scope.PhoneNumber = "(425)705-1900"
                $scope.WebSiteURL = "www.microsoft.com";

                $scope.setOriginalValues();

                $scope.EditMode = true;
                $scope.DisplayMode = false;

                $scope.ShowCreateButton = true;
                $scope.ShowEditButton = false;
                $scope.ShowCancelButton = false;
                $scope.ShowUpdateButton = false;
              
            }
            else
            {
                var getCustomer = new Object();
                getCustomer.CustomerID = customerID;
                customerService.getCustomer(getCustomer, $scope.getCustomerCompleted, $scope.getCustomerError);

            }
          
        }

        $scope.getCustomerCompleted = function (response) {

            $scope.EditMode = false;
            $scope.DisplayMode = true;
            $scope.ShowCreateButton = false;
            $scope.ShowEditButton = true;
            $scope.ShowCancelButton = false;
            $scope.ShowUpdateButton = false;

            $scope.CustomerCode = response.Customer.CustomerCode;
            $scope.CompanyName = response.Customer.CompanyName;
            $scope.Address = response.Customer.Address;
            $scope.City = response.Customer.City;
            $scope.Region = response.Customer.Region;
            $scope.PostalCode = response.Customer.PostalCode;
            $scope.CountryCode = response.Customer.Country;
            $scope.PhoneNumber = response.Customer.PhoneNumber;
            $scope.WebSiteURL = response.Customer.WebSiteUrl;

            $scope.setOriginalValues();
        }

        $scope.getCustomerError = function (response) {
            alertsService.RenderErrorMessage(response.ReturnMessage);
        }

        $scope.setOriginalValues = function () {

            $scope.OriginalCustomerCode = $scope.CustomerCode;
            $scope.OriginalCompanyName = $scope.CompanyName;
            $scope.OriginalAddress = $scope.Address;
            $scope.OriginalCity = $scope.City;
            $scope.OriginalRegion = $scope.Region;
            $scope.OriginalPostalCode = $scope.PostalCode;
            $scope.OriginalCountryCode = $scope.CountryCode;
            $scope.OriginalPhoneNumber = $scope.PhoneNumber;
            $scope.OriginalWebSiteURL = $scope.WebSiteURL;

        }

        $scope.resetValues = function () {

            $scope.CustomerCode = $scope.OriginalCustomerCode;
            $scope.CompanyName = $scope.OriginalCompanyName;
            $scope.Address = $scope.OriginalAddress;
            $scope.City = $scope.OriginalCity;
            $scope.Region = $scope.OriginalRegion;
            $scope.PostalCode = $scope.OriginalPostalCode;
            $scope.CountryCode = $scope.OriginalCountryCode;
            $scope.PhoneNumber = $scope.OriginalPhoneNumber;
            $scope.WebSiteURL = $scope.OriginalWebSiteURL;

        }

        $scope.editCustomer = function () {

            $scope.ShowCreateButton = false;
            $scope.ShowEditButton = false;
            $scope.ShowCancelButton = true;
            $scope.ShowUpdateButton = true;
            $scope.EditMode = true;
            $scope.DisplayMode = false;
        }

        $scope.cancelChanges = function () {

            $scope.ShowCreateButton = false;
            $scope.ShowEditButton = true;
            $scope.ShowCancelButton = false;
            $scope.ShowUpdateButton = false;
            $scope.EditMode = false;
            $scope.DisplayMode = true;
            $rootScope.alerts = [];

            $scope.resetValues();
        }

        $scope.createCustomer = function () {
          
            var customer = $scope.createCustomerObject();
            customerService.createCustomer(customer, $scope.createCustomerCompleted, $scope.createCustomerError);
        }

        $scope.updateCustomer = function () {

            var customer = $scope.createCustomerObject();
            customer.CustomerID = $scope.CustomerID;
            customerService.updateCustomer(customer, $scope.updateCustomerCompleted, $scope.updateCustomerError);
        }

        $scope.createCustomerCompleted = function (response, status) {

            $scope.EditMode = false;
            $scope.DisplayMode = true;
            $scope.ShowCreateButton = false;
            $scope.ShowEditButton = true;
            $scope.ShowCancelButton = false;

            $scope.CustomerID = response.Customer.CustomerID;
            alertsService.RenderSuccessMessage(response.ReturnMessage);

            $scope.setOriginalValues();
        }

        $scope.createCustomerError = function (response) {
            alertsService.RenderErrorMessage(response.ReturnMessage);
            $scope.clearValidationErrors();
            alertsService.SetValidationErrors($scope, response.ValidationErrors);
        }


        $scope.updateCustomerCompleted = function (response, status) {

            $scope.EditMode = false;
            $scope.DisplayMode = true;
            $scope.ShowCreateButton = false;
            $scope.ShowEditButton = true;
            $scope.ShowCancelButton = false;
            $scope.ShowUpdateButton = false;

            alertsService.RenderSuccessMessage(response.ReturnMessage);

            $scope.setOriginalValues();
        }

        $scope.updateCustomerError = function(response) {
            alertsService.RenderErrorMessage(response.ReturnMessage);
            $scope.clearValidationErrors();
            alertsService.SetValidationErrors($scope, response.ValidationErrors);
        }


        $scope.createCustomerObject = function () {

            var customer = new Object();

            customer.CustomerCode = $scope.CustomerCode;
            customer.CompanyName = $scope.CompanyName;
            customer.Address = $scope.Address;
            customer.City = $scope.City;
            customer.Region = $scope.Region;
            customer.PostalCode = $scope.PostalCode;
            customer.Country = $scope.CountryCode;
            customer.PhoneNumber = $scope.PhoneNumber;
            customer.WebSiteUrl = $scope.WebSiteURL;

            return customer;

        }

        $scope.clearValidationErrors = function () {

            $scope.CustomerCodeInputError = false;
            $scope.CompanyNameInputError = false;          

        }


      
    }]);

});