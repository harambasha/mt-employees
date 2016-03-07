'use strict';

angular.module('employeesApp').factory('departmentService', function ($resource) {
    return $resource('http://localhost:9000/api/departments');
}).
factory('departmentEmployeeService', function ($resource) {
    return $resource('http://localhost:9000/api/departments/:departmentId/employees/:isActive', null, {});
});