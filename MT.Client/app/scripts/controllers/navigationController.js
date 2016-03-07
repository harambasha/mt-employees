'use strict';
angular.module('employeesApp').controller('NavigationCtrl', function ($scope, navigationService) {
    $scope.navigationStyle = function (pagePath) {
        //if the page is more deeply nested than the current page, it can't ever be selected.
        if (pagePath.length > navigationService.currentPath.length) {
            return '';
        }

        for (var i = 0; i < pagePath.length; i++) {
            if (navigationService.currentPath[i] !== pagePath[i]) {
                return '';
            }
        }
        return 'active';
    };
});
