'use strict';

angular.module('employeesApp').controller('EmployeeListCtrl', function ($scope, employees, departments, $state, navigationService, employeeActiveEmployeesService, employeeService, departmentEmployeeService) {
    navigationService.currentPath = ['employees'];
    $scope.employees = employees;
    $scope.departments = departments;
    $scope.isActive = null;
    $scope.showAll = null;
    $scope.selectedDepartment = null;
    $scope.message = null;
    $scope.showError = false;

    $scope.new = function () {
        $state.go('employees.new');
    };

    $scope.handleError = function (error) {
        $scope.message = error.data.message;
        $scope.showError = true;
    };

    $scope.edit = function (employee) {
        $state.go('employees.edit', {
            id: employee.id
        });
    };

    $scope.delete = function (employee) {
        $state.go('employees.list.delete', {
            id: employee.id
        });
    };

    $scope.showActiveEmployees = function () {
        $scope.showError = false;
        if ($scope.isActive === null) {
            return;
        } else {
            if ($scope.showAll != null && $scope.showAll === false) {
                employeeActiveEmployeesService.query({
                    isActive: $scope.isActive
                }).$promise.then(function (response) {
                    $scope.employees = response;
                }, function (error) {
                    $scope.handleError(error);
                });
            }
        }
    };

    $scope.showAllEmployees = function () {
        $scope.showError = false;
        if ($scope.showAll === null) {
            return;
        } else {
            if ($scope.showAll) {
                employeeService.query().$promise.then(function (response) {
                    $scope.employees = response;
                }, function (error) {
                    $scope.handleError(error);
                });
            }
        }
    };

    $scope.filterEmployeesByDepartment = function () {
        $scope.showError = false;
        if ($scope.showAll != null && $scope.showAll === true) {
            return;
        } else {
            if ($scope.showAll === null || $scope.showAll === false) {
                if ($scope.isActive == null && $scope.selectedDepartment != null) {
                    departmentEmployeeService.query({
                        isActive: true,
                        departmentId: $scope.selectedDepartment
                    }).$promise.then(function (response) {
                        $scope.employees = response;
                    }, function (error) {
                        $scope.handleError(error);
                    });
                }
                if ($scope.isActive != null && $scope.selectedDepartment != null) {
                    departmentEmployeeService.query({
                        isActive: $scope.isActive,
                        departmentId: $scope.selectedDepartment
                    }).$promise.then(function (response) {
                        $scope.employees = response;
                    }, function (error) {
                        $scope.handleError(error);
                    });
                }
            }
        }
    };
});