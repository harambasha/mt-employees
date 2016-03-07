'use strict';
angular.module('employeesApp').config(function ($stateProvider) {
    $stateProvider.state('employees', {
        url: '/employees',
        abstract: true,
        template: '<div ui-view></div>'
    });

    $stateProvider.state('employees.list', {
        url: '',
        controller: 'EmployeeListCtrl',
        templateUrl: 'views/employees/employee-list.html',
        resolve: {
            employees: ['employeeService', function (employeeService) {
                return employeeService.query();
            }],
            departments: ['departmentService', function (departmentService) {
                return departmentService.query();
            }]
        }
    });

    $stateProvider.state('employees.list.delete', {
        url: '/:id/delete',
        onEnter: ['$stateParams', '$state', '$modal',
            function ($stateParams, $state, $modal) {
                $modal.open({
                    templateUrl: 'views/employees/employee-delete.html',
                    resolve: {
                        employee: ['employeeService',
                            function (employeeService) {
                                return employeeService.get({
                                    id: $stateParams.id
                                });
                            }
                        ]
                    },
                    controller: 'EmployeeDeleteCtrl'
                });
            }
        ]
    });

    $stateProvider.state('employees.new', {
        url: '/new',
        controller: 'EmployeeEditCtrl',
        templateUrl: 'views/employees/employee-edit.html',
        resolve: {
            employee: [
                function () {
                    return {
                        id: null
                    };
                }
            ],
            departments: ['departmentService', function (departmentService) {
                return departmentService.query();
            }]
        }
    });

    $stateProvider.state('employees.edit', {
        url: '/:id',
        controller: 'EmployeeEditCtrl',
        templateUrl: 'views/employees/employee-edit.html',
        resolve: {
            employee: ['employeeService', '$stateParams',
                function (employeeService, $stateParams) {
                    return employeeService.get({
                        id: $stateParams.id
                    });
                }
            ],
            departments: ['departmentService', function (departmentService) {
                return departmentService.query();
            }]
        }
    });
});
