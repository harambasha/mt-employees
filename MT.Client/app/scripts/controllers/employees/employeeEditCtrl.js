'use strict';

angular.module('employeesApp').controller('EmployeeEditCtrl', function ($scope, $state, $stateParams, employeeService, employee, departments) {
    $scope.employee = employee;
    $scope.departments = departments;
    $scope.message = null;
    $scope.isError = false;
    $scope.isSuccess = false;

    $scope.save = function () {
        var savePromise = null;

        if ($scope.employee.id !== null) {
            savePromise = employeeService.update({
                id: $scope.employee.id
            }, $scope.employee).$promise;
        } else {
            savePromise = employeeService.save($scope.employee).$promise;
        }

        savePromise.then(function (response) {
            $state.go('employees.edit', {
                id: response.id
            });
            $scope.isSuccess = true;
            $scope.message = "Success! Well done employee data submitted.";
        }, function (error) {
            $scope.isError = true;
            $scope.message = 'Unable to save - ' + error.data.message;
        });
    };
    $scope.cancel = function () {
        $state.go('employees.list');
    };
});