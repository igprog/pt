/*!
functions.js
(c) 2017-2020 IG PROG, www.igprog.hr
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
                } else if (lang == 'en' && !this.isNullOrEmpty(x.shortdesc_en)) {
                    return (x.shortdesc_en).replace('amp;', '');
                } else {
                    return x.shortdesc_en;
                }
            }
        },
        longdesc: function (x, lang) {
            if (x !== undefined) {
                if (lang == 'hr' && !this.isNullOrEmpty(x.longdesc_hr)) {
                    return (x.longdesc_hr);
                } else if (lang == 'en' && !this.isNullOrEmpty(x.longdesc_en)) {
                    return (x.longdesc_en);
                } else {
                    return (x.longdesc_hr);
                }
            }
        },
        titleseo: function (x) {
            return x.toLowerCase().replace(/\s+/g, '-').replace('ž', 'z').replace('š', 's').replace('č', 'c').replace('ć', 'c').replace('đ', 'd').replace('®', '').replace('é', 'e').replace('™', '');
        }
    }
}]);

;
