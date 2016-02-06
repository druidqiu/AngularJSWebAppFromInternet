define(['application-configuration', 'ajaxService'], function (app) {

    app.register.service('customersService', ['ajaxService', function (ajaxService) {

        this.importCustomers = function (successFunction, errorFunction) {
            ajaxService.AjaxGet("/api/customers/ImportCustomers", successFunction, errorFunction);
        };

        this.getCustomers = function (customer, successFunction, errorFunction) {          
            ajaxService.AjaxGetWithData(customer, "/api/customers/GetCustomers", successFunction, errorFunction);
        };

        this.createCustomer = function (customer, successFunction, errorFunction) {
            ajaxService.AjaxPost(customer, "/api/customers/CreateCustomer", successFunction, errorFunction);
        };

        this.updateCustomer = function (customer, successFunction, errorFunction) {
            ajaxService.AjaxPost(customer, "/api/customers/UpdateCustomer", successFunction, errorFunction);
        };
     
        this.getCustomer = function (customerID, successFunction, errorFunction) {
            ajaxService.AjaxGetWithData(customerID, "/api/customers/GetCustomer", successFunction, errorFunction);
        };

    }]);
});