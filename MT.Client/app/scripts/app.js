'use strict';
angular.module("employeesApp", ['ngResource','ui.bootstrap', 'ui.router']).config(function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/');

        $stateProvider.state('home', {
            url: '/',
            abstract: true,
            template: '<div ui-view></div>'
        });
    });
