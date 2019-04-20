/*!
functions.js
(c) 2017-2019 IG PROG, www.igprog.hr
*/
angular.module('functions', [])

.factory('functions', ['$http', function ($http) {
    return {
        isNullOrEmpty: function (x) {
            var res = false;
            if (x === '' || x == undefined || x == null) {
                res = true;
            }
            return res;
        },
    }
}]);

;
