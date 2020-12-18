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
        shortdesc: function (x, lang) {
            if (x !== undefined) {
                if (lang == 'hr' && !this.isNullOrEmpty(x.shortdesc_hr)) {
                    return (x.shortdesc_hr).replace('amp;', '');
                } else {
                    return (x.shortdesc_en).replace('amp;', '');
                }
            }
        },
        longdesc: function (x, lang) {
            if (x !== undefined) {
                if (lang == 'hr' && !this.isNullOrEmpty(x.longdesc_hr)) {
                    return (x.longdesc_hr);
                } else {
                    return (x.longdesc_en);
                }
            }
        },
        seotitle: function (x) {
            return x.toLowerCase().replace(/\s+/g, '-').replace('ž', 'z').replace('š', 's').replace('č', 'c').replace('ć', 'c').replace('đ', 'd');
        }
    }
}]);

;
