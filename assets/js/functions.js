/*!
functions.js
(c) 2017 IG PROG, www.igprog.hr
*/
angular.module('functions', [])

.factory('functions', ['$http', function ($http) {
    return {
        //'test': function (x, callback) {
        //    var res = x.a + 'xxx' + x.b;
        //    return callback(res);
        //},
        //'post': function (x, callback) {
        //    $http({
        //        url: 'Products.asmx/GetProducts',
        //        method: 'POST',
        //        data: { url: x }
        //    })
        //   .then(function (response) {
        //       return callback(JSON.parse(response.data.d));
        //   },
        //   function (response) {
        //       return callback("err");
        //   });
        //},
        //'getProducts': function (x, callback) {
        //    $http({
        //        url: 'Products.asmx/GetProductsWithStock',
        //        method: 'POST',
        //        data:  { skip: 0, take: x }
        //    })
        //   .then(function (response) {
        //       return callback(JSON.parse(response.data.d));
        //   },
        //   function (response) {
        //       return callback("err");
        //   });
        //},
        'isNullOrEmpty': function (x) {
            var res = false;
            if (x === '' || x == undefined || x == null) {
                res = true;
            }
            return res;
        },
    }
}]);

;
