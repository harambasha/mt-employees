'use strict';

angular.module('employeesApp').controller('EmployeeDeleteCtrl', function($scope, $state, $modalInstance, employee, employeeService) {
    $scope.employee = employee;

    $scope.confirm = function() {
        employeeService.delete({ id: $scope.employee.id }, function() {
            $modalInstance.close($scope.employee);
            $state.go('employees.list', {}, { reload: true });
        }, function() {
            $scope.message = 'Unable to delete employee';
        });
    };

    $scope.cancel = function() {
        $modalInstance.dismiss('cancel');
        $state.go('employees.list');
    };
});
