define(['application-configuration', 'ajaxService'], function (app) {

    app.register.service('productsService', ['ajaxService', function (ajaxService) {

        this.importProducts = function (successFunction, errorFunction) {
            ajaxService.AjaxGet("/api/products/ImportProducts", successFunction, errorFunction);
        };

        this.getProducts = function (product, successFunction, errorFunction) {
            ajaxService.AjaxGetWithData(product, "/api/products/GetProducts", successFunction, errorFunction);
        };
             
        this.getProductsWithNoBlock = function (product, successFunction, errorFunction) {
            ajaxService.AjaxGetWithNoBlock(product, "/api/products/GetProducts", successFunction, errorFunction);
        };

        this.createProduct = function (product, successFunction, errorFunction) {
            ajaxService.AjaxPost(product, "/api/products/CreateProduct", successFunction, errorFunction);
        };

        this.updateProduct = function (product, successFunction, errorFunction) {
            ajaxService.AjaxPost(product, "/api/products/UpdateProduct", successFunction, errorFunction);
        };

        this.getProduct = function (productID, successFunction, errorFunction) {
            ajaxService.AjaxGetWithData(productID, "/api/products/GetProduct", successFunction, errorFunction);
        };

    }]);
});