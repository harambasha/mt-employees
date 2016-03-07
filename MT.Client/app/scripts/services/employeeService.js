'use strict';

angular.module('employeesApp').factory('employeeService', function ($resource) {
    return $resource('http://localhost:9000/api/employees/:id', null, {
        'update': {
            method: 'PUT'
        },
        'save': {
            method: 'POST'
        }
    });
}).factory('employeeActiveEmployeesService', function ($resource) {
    return $resource('http://localhost:9000/api/employees/:isActive', null, {});
});