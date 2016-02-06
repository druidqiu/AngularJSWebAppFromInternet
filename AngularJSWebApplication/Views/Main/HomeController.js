"use strict";

define(['application-configuration'], function (app) {

    app.register.controller('homeController', ['$scope', '$rootScope', function ($scope, $rootScope) {

        $rootScope.applicationModule = "Main";

    }]);

});
