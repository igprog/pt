﻿/*!
app.js
(c) 2017-2019 IG PROG, www.igprog.hr
*/
angular.module('app', ['ngStorage', 'pascalprecht.translate', 'functions'])

.config(['$translateProvider', '$translatePartialLoaderProvider', '$httpProvider', function ($translateProvider, $translatePartialLoaderProvider, $httpProvider) {

        $translateProvider.useLoader('$translatePartialLoader', {
            urlTemplate: './assets/json/translations/{lang}/{part}.json'
        });
        $translateProvider.preferredLanguage('hr');
        $translatePartialLoaderProvider.addPart('main');
        $translateProvider.useSanitizeValueStrategy('escape');

        //--------------disable catche---------------------
        if (!$httpProvider.defaults.headers.get) {
            $httpProvider.defaults.headers.get = {};
        }
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
        //-------------------------------------------------
    }])

.controller('appCtrl', ['$scope', '$http', '$rootScope', '$sessionStorage', 'functions', '$translate', '$localStorage', '$window', function ($scope, $http, $rootScope, $sessionStorage, functions, $translate, $localStorage, $window) {

    if (angular.isDefined($sessionStorage.u)) {
        $rootScope.u = JSON.parse($sessionStorage.u);
    }

    var reloadPage = function () {
        if (typeof (Storage) !== 'undefined') {
            if (localStorage.version) {
                if (localStorage.version !== $rootScope.config.version) {
                    localStorage.version = $rootScope.config.version;
                    window.location.reload(true);
                }
            } else {
                localStorage.version = $rootScope.config.version;
            }
        }
    }

    var getConfig = function () {
        $http.get('./config/config.json')
          .then(function (response) {
              $sessionStorage.config = response.data;
              $rootScope.config = response.data;
              reloadPage();
          });
    };
    if (!angular.isDefined($sessionStorage.config)) {
        getConfig();
    } else {
        $rootScope.config = $sessionStorage.config;
        reloadPage();
    }
    //getConfig();

    var getCompanyInfo = function () {
        $http({
            url: 'Admin.asmx/GetCompanyInfo',
            method: 'POST',
            data: {}
        })
         .then(function (response) {
             $scope.companyInfo = JSON.parse(response.data.d);
         },
         function (response) {
             alert(JSON.parse(response.data.d));
         });
    }
    getCompanyInfo();

    var loadCategories = function () {
        $http({
            url: 'Products.asmx/GetCategories',
            method: 'POST',
            data: { }
        })
          .then(function (response) {
             $scope.categories = JSON.parse(response.data.d);
          },
          function (response) {
              alert(JSON.parse(response.data.d));
          });
    }
    loadCategories();

    var loadFeatured = function () {
        $http({
            url: 'Featured.asmx/Load',
            method: 'POST',
            data: ''
        })
     .then(function (response) {
         $scope.featured = JSON.parse(response.data.d);  //TODO
     },
     function (response) {
         alert(response.data.d);
     });
    }
   // loadFeatured();

    var loadNewProducts = function () {
        $http({
            url: 'Featured.asmx/LoadNewProducts',
            method: 'POST',
            data: ''
        })
     .then(function (response) {
         $scope.newProducts = JSON.parse(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }
    //loadNewProducts();

    $scope.href = function (x) {
        $window.location.href = x;
    }

    var initFeatured = function () {
        $http({
            url: 'Featured.asmx/Init',
            method: 'POST',
            data: ''
        })
     .then(function (response) {
         $scope.featuredTypes = JSON.parse(response.data.d).types;
         $scope.featuredType = $scope.featuredTypes.featured;
     },
     function (response) {
         alert(response.data.d);
     });
    }
    //initFeatured();

    $scope.isShowFeatured = false;
    $scope.showFeatured = function () {
        $scope.isShowFeatured = $scope.isShowFeatured == true ? false : true;
    }

    $scope.toggleFeatureType = function (x) {
        $scope.featuredType = angular.copy(x);
        $scope.showFeatured();
    }

    $scope.toggleNew = function (code, title) {
        $scope.featuredType.code = angular.copy(code);
        $scope.featuredType.title = angular.copy(title);
        $scope.showFeatured();
    }

    $rootScope.logout = function () {
        $sessionStorage.u = null;
        $rootScope.u = null;
        $rootScope.o = null;

       // $scope.href('login.html');  //TODO
    }

    $scope.shortdesc = function (x, lang) {
        return (functions.shortdesc(x, lang));
    }

    $scope.longdesc = function (x, lang) {
        return (functions.longdesc(x, lang));
    }

}])

.controller('headerCtrl', ['$scope', '$http', '$rootScope', '$sessionStorage', 'functions', '$translate', '$translatePartialLoader', '$localStorage', '$timeout', function ($scope, $http, $rootScope, $sessionStorage, functions, $translate, $translatePartialLoader, $localStorage, $timeout) {

    var getConfig = function () {
        $http.get('./config/config.json')
          .then(function (response) {
              $sessionStorage.config = response.data;
              $rootScope.config = response.data;
          });
    };
    if (!angular.isDefined($sessionStorage.config)) {
        getConfig();
    } else {
        $rootScope.config = $sessionStorage.config;
    }

    //$rootScope.u = !angular.isDefined($sessionStorage.u) ? $scope.initUser() : JSON.parse($sessionStorage.u);


    $scope.setLanguage = function (x) {
        $rootScope.config.language = x;
        $sessionStorage.config.language = x;
        $translate.use(x.code);
        $translatePartialLoader.addPart('main');
    };
    if (angular.isDefined($sessionStorage.config)) {
        $scope.setLanguage($sessionStorage.config.language);
    }

    $scope.setCurrency = function (x) {
        $rootScope.config.currency = JSON.parse(angular.toJson(x));
        $sessionStorage.config.currency = $rootScope.config.currency;
        $timeout(function () {
            window.location.reload(true);
        }, 200);
    };

}])

//.controller('productsCtrl', ['$scope', '$http', '$rootScope', '$sessionStorage', 'functions', '$translate', function ($scope, $http, $rootScope, $sessionStorage, functions, $translate) {


//}])

.controller('shopCtrl', ['$scope', '$http', '$rootScope', '$sessionStorage', 'functions', '$translate', '$translatePartialLoader', '$localStorage', '$window', '$timeout', function ($scope, $http, $rootScope, $sessionStorage, functions, $translate, $translatePartialLoader, $localStorage, $window, $timeout) {
    $scope.isloading = false;
    $scope.group = "";
    $scope.displayFilters = false;
    var type = "";

    var queryString = location.search;
    var params = queryString.split('&');

    if (params.length > 1) {
        $scope.page = parseInt(params[1].substring(5, 6));
    } else {
        $scope.page = 1;
    }
    //$scope.page = 1;
    window.scrollTo(0, 150);

    if (params[0].substring(1, 6) === 'brand') {
        $scope.group = params[0].substring(7);
        type = "brand";
        $scope.current = params[0].substring(7);
        $scope.searchQuery = '';
        $sessionStorage.search = '';
    }
    if (params[0].substring(1, 7) === 'gender') {
        $scope.group = params[0].substring(8);
        type = "gender";
        $scope.current = params[0].substring(8);
        $scope.searchQuery = '';
        $sessionStorage.search = '';
    }
    if (params[0].substring(1, 6) === 'isnew') {
        $scope.group = params[0].substring(7);
        type = "isnew";
        $scope.current = "new models";
        $scope.searchQuery = '';
        $sessionStorage.search = '';
    }
    if (params[0].substring(1, 7) === 'outlet') {
        $scope.group = params[0].substring(8);
        type = "outlet";
        $scope.current = "outlet";
        $scope.searchQuery = '';
        $sessionStorage.search = '';
    }


    if (params[0].substring(1, 9) === 'category') {
        $scope.category = params[0].substring(10);
        $scope.current = params[0].substring(10);
    } else {
        $scope.category = "";
    }

    $scope.show = angular.isDefined($sessionStorage.config) ? $sessionStorage.config.prodctstoshow : 12;
    $scope.searchQuery = null;

    if (!functions.isNullOrEmpty(localStorage.distinct == 'null' ? null : localStorage.distinct)) {
        $scope.filters = true;
    } else {
        $scope.filters = false;
    }

    $scope.sort = 'price';
    $scope.sortOrder = 'ASC';
    $scope.colorFilter = '';

    $scope.setSortOrder = function () {
        $scope.sortOrder = $scope.sortOrder == 'ASC' ? 'DESC' : 'ASC';
        $scope.setFilter($scope.show);
    }

    var setPages = function (x) {
        $scope.pages = [];
        for (i = 1; i < (x / $scope.show) + 1; i++) {
            $scope.pages.push(i);
        }
    }

    $scope.setPage = function (x) {
        //window.scrollTo(0, 150);
        //$scope.page = x;
        //$sessionStorage.page = x;

        //TODO:
        $timeout(function () {
            //window.location.href = window.location.origin + window.location.pathname + params[0] + '&page=' + x;
            //window.location.href = window.location.origin + window.location.pathname + params[0].length == 0 ? '?category=' + $scope.category : params[0] + '&page=' + x;
            window.location.href = window.location.origin +
                (window.location.pathname === '/' ? '/index.html' : window.location.pathname) +
                (params[0].length <= 1 ? '?category=' + $scope.category : params[0]) +
                '&page=' + x;

            window.scrollTo(0, 150);
            $scope.page = x;
            $sessionStorage.page = x;

          //  searchProducts($scope.show, $scope.category);


        }, 200);

        //searchProducts($scope.show, $scope.category);
    }

    var load = function (limit, category) {
        $scope.isloading = true;
       // $scope.group = null;
        $http({
            url: 'Products.asmx/GetProductsByCategory',
            method: 'POST',
            data: { limit: limit, category: category, sort: $scope.sort, order: $scope.sortOrder }
        })
      .then(function (response) {
          $scope.isloading = false;
          $sessionStorage.d = response.data.d;
          $scope.d = JSON.parse(response.data.d);
          setPages($scope.d.response.count);
          $scope.filter = {
              price: $scope.d.response.maxPrice,
              size: '',
              brand: ''
          }
          //getDistinctFilters(category, null);
      },
      function (response) {
          $scope.isloading = false;
          alert(JSON.parse(response.data.d));
      });
    }

    $scope.getProductsByCategory = function (category) {
        $scope.searchQuery = '';
       // $scope.page = 1;
        $scope.category = category;
        load($scope.show, category);
    }
    
    var loadGroup = function () {
        $scope.isloading = true;
        $scope.searchQuery = '';
        //$scope.page = 1;
       // $scope.category = null;
        $http({
            url: 'Products.asmx/GetProductsByGroup',
            method: 'POST',
            data: { limit: $scope.show, group: $scope.group, type: type, sort: $scope.sort, order: $scope.sortOrder }
        })
      .then(function (response) {
          $scope.isloading = false;
          $sessionStorage.d = response.data.d;
          $scope.d = JSON.parse(response.data.d);
          setPages($scope.d.response.count);
          $scope.filter = {
              price: $scope.d.response.maxPrice,
              size: '',
              brand: ''
          }
          //getDistinctFilters(null, $scope.group);
      },
      function (response) {
          $scope.isloading = false;
          alert(JSON.parse(response.data.d));
      });
   } 

    $scope.href = function (x) {
        $window.location.href = x;
    }

    $scope.changeCategory = function (x) {
        $sessionStorage.page = 1;
        $sessionStorage.search = '';
        $scope.searchQuery = '';
        $scope.page = 1;
        $scope.filters = false;
        $timeout(function () {
            $scope.href(x);
        }, 200);
    }

    var searchProducts = function (limit, category) {
        $scope.isloading = true;
        $sessionStorage.search = $scope.searchQuery;
        if ($scope.searchQuery !== '') {
            $scope.group = '';
            type = '';
        }
        if (!functions.isNullOrEmpty(localStorage.distinct == 'null' ? null : localStorage.distinct)) {
            $scope.d.distinct = JSON.parse(localStorage.distinct);
        }
        $http({
            url: 'Products.asmx/SearchProducts',
            method: 'POST',
            data: { limit: limit, page: $scope.page, category: category, search: $scope.searchQuery, filter: $scope.d.distinct, group: $scope.group, type: type, sort: $scope.sort, order: $scope.sortOrder }
        })
      .then(function (response) {
          $scope.isloading = false;
          var res = JSON.parse(response.data.d);
          $scope.d.products = res.products;
          $scope.d.response = res.response;
          $sessionStorage.d = response.data.d;
          $scope.filter.price = $scope.d.response.maxPrice;
          if ($scope.searchQuery != '') { $scope.d.distinct = res.distinct; }
          setPages($scope.d.response.count);
          //getDistinctFilters(category, null);
      },
      function (response) {
          $scope.isloading = false;
          alert(JSON.parse(response.data.d));
      });
    }

    $scope.setFilter = function (show) {
        $sessionStorage.config.prodctstoshow = show;
        $scope.searchQuery = '';
        $scope.page = 1;

        localStorage.distinct = JSON.stringify($scope.d.distinct);

        searchProducts($scope.show, $scope.category);
        $scope.filters = true;
    }

    $scope.colorFilter = function (x) {
        angular.forEach($scope.d.distinct.colorGroup, function (value, key) {
            if (value.colorfamily_en == x.colorfamily_en) {
                value.isselected = true;
            } else {
                value.isselected = false;
            }
        })
        $scope.setFilter($scope.show);
    }

    $scope.setColorFilter = function(x) {
        $scope.colorFilter = x;
        searchProducts($scope.show, $scope.category);
    }

    $scope.search = function () {
        $scope.category = '';
        $scope.page = 1;
        searchProducts($scope.show, $scope.category);
    }

    $scope.clearFilters = function () {
        $scope.filters = false;
        localStorage.distinct = null;
        //$scope.href('index.html?category=' + $scope.category);
        $scope.href('index.html?category=' + $scope.category + '&page=1');
    }

    $scope.showFilters = function () {
        $scope.displayFilters = $scope.displayFilters == false ? true : false;
    }

    $scope.loading_f = false;
    var getDistinctFilters = function (category, group) {
        $scope.loading_f = true;
        $http({
            url: 'Products.asmx/GetDistinctFilters',
            method: 'POST',
            data: { category: category, group: group }
        })
      .then(function (response) {
          $scope.loading_f = false;
          $scope.d.distinct = JSON.parse(response.data.d);
      },
      function (response) {
          $scope.loading_f = false;
          alert(JSON.parse(response.data.d));
      });
    }

    $scope.getDistinctFilters = function () {
       // if (functions.isNullOrEmpty($scope.d.distinct)) {
            return getDistinctFilters($scope.category, $scope.group);
       // } return false;
    }

    if ($sessionStorage.d !== undefined && $sessionStorage.d !== 'Sequence contains no elements') {
        $scope.d = JSON.parse($sessionStorage.d);
        if ($sessionStorage.page !== undefined) {
           // $scope.page = $sessionStorage.page;
        }
        if ($sessionStorage.search !== undefined) {
            $scope.searchQuery = $sessionStorage.search;
        }
        setPages($scope.d.response.count);
        $scope.filter = {
            price: $scope.d.response.maxPrice,
            size: '',
            brand: ''
        }
    } else {
        //$scope.d = null;
        $scope.d = {
            products: null,
            distinct: null,
            response: null
        }
        $scope.filter = {
            price: 0,
            size: '',
            brand: ''
        }
    }

    if (!angular.isDefined($scope.d) || $scope.group == '') {
        if (params[0].substring(1, 9) === 'category') {
            $scope.category = params[0].substring(10);
            $scope.current = params[0].substring(10);
        } else {
            $scope.category = 'T-Shirt';
        }
        
        if (params[0].length > 1) {
            searchProducts($scope.show, $scope.category);
        } else {
            load($scope.show, $scope.category);
        }
        //load($scope.show, $scope.category);
    }

    //if (params[0].length > 1) {
    //    searchProducts($scope.show, $scope.category);
    //}

    if ($scope.group != '') {
        searchProducts($scope.show, $scope.category);
        //loadGroup();
    }


    //$scope.getDistinctFilters = function (category) {
    //    if (functions.isNullOrEmpty($scope.d.distinct)) {
    //        return getDistinctFilters(category);
    //    } return false;
    //}

}])

.controller('productCtrl', ['$scope', '$http', '$rootScope', '$sessionStorage', 'functions', '$translate', '$window', '$localStorage', function ($scope, $http, $rootScope, $sessionStorage, functions, $translate, $window, $localStorage) {
    var style = '';
    var queryString = location.search;
    var params = queryString.split('&');
    if (params[0].substring(1, 6) === 'style') {
        style = params[0].substring(7);
    }

    //var querystring = location.search.substring(7);
    //var style = querystring;
    var color = null;

    $scope.limit = 50;
    $scope.addedToCart = false;
    $scope.enableButton = false;
    var getConfig = function () {
        $http.get('./config/config.json')
          .then(function (response) {
              $sessionStorage.config = response.data;
              $rootScope.config = response.data;
          });
    };
    if (!angular.isDefined($sessionStorage.config)) {
        getConfig();
    } else {
        $rootScope.config = $sessionStorage.config;
    }

    $scope.stockGroupedByColor = [];
    $scope.loading = false;
    var getStockGroupedByColor = function (style) {
        $scope.loading = true;
        $http({
            url: 'Products.asmx/GetStockGroupedByColor',
            method: 'POST',
            data: { style: style }
        })
      .then(function (response) {
          $scope.stockGroupedByColor = JSON.parse(response.data.d);
          $scope.loading = false;
      },
      function (response) {
          alert(JSON.parse(response.data.d));
      });
    }

    $rootScope.productTitle = null;
    $rootScope.productDesc = null;
    $scope.loading_p = false;
    var load = function (style, color) {
        $scope.loading_p = true;
        $http({
            url: 'Products.asmx/GetProduct',
            method: 'POST',
            data: { style: style, color: color }
        })
       .then(function (response) {
           $scope.p = JSON.parse(response.data.d);
           if ($scope.p.shortdesc_en != null) {
               //$window.document.title = 'Promo-Tekstil - ' + $translate.instant($scope.p.shortdesc_en);
               //location.hash = '#/' + unescape($translate.instant($scope.p.shortdesc_en)).replace(/\s/g, "-");
           }
           $rootScope.productTitle = $scope.shortdesc($scope.p, $rootScope.config.language.code)
           $rootScope.productDesc = $scope.longdesc($scope.p, $rootScope.config.language.code)

           $scope.loading_p = false;

           getStockGroupedByColor(style);


           //var maxStock = 0;
           //angular.forEach($scope.p.stock, function (value, key) {
           //    if (value.uttstock + value.suppstock > maxStock) {
           //        maxStock = value.uttstock + value.suppstock;
           //    }
           //})
           //$rootScope.maxStockList = [];
           //for (i = 0; i < maxStock; i++) {
           //    $rootScope.maxStockList.push(i);
           //}
           $scope.choosen = {
               sku: '',
               imageurl: $scope.p.modelimageurl,
               style: $scope.p.style,
               shortdesc_en: '',
               quantity: 1,
               size: '',
               color: '',
               price: []
           }
       },
       function (response) {
           $scope.loading_p = false;
           alert(JSON.parse(response.data.d));
       });
    };
    load(style, color);

    $scope.setSize = function (x) {
        $scope.choosen.size = x;
    }

    $scope.setColor = function (x) {
        $scope.choosen.color = x;
    }

    $scope.plus = function (x) {
        if ($scope.choosen.quantity + x < 1) { return false; }
        $scope.choosen.quantity = $scope.choosen.quantity + x;
    }

    if (localStorage.cart != undefined && localStorage.cart != '') {
        $scope.cart = JSON.parse(localStorage.cart);
    } else {
        $scope.cart = [];
        localStorage.cart = [];
    }

    if (localStorage.groupingcart != undefined && localStorage.groupingcart != '') {
        $rootScope.groupingCart = JSON.parse(localStorage.groupingcart);
    } else {
        $rootScope.groupingCart = [];
        localStorage.groupingcart = [];
    }

    $scope.updateCart = function () {
        localStorage.groupingcart = JSON.stringify($rootScope.groupingCart);
    }

    $scope.clearAll = function () {
        localStorage.clear();
        $scope.cart = [];
        $rootScope.groupingCart = [];
    }

    $scope.remove = function (x) {
        for (var i = $scope.cart.length - 1; i >= 0; i--) {
            if ($scope.cart[i].style == x.style) {
                $scope.cart.splice(i, 1);
            }
        }
        groupingCart($scope.cart);
        localStorage.cart = JSON.stringify($scope.cart);
    }

    $scope.removeVariant = function (x) {
        angular.forEach($scope.cart, function (value, key) {
            if (value.sku == x.sku) {
                $scope.cart.splice(key, 1);
            }
        })
        groupingCart($scope.cart);
        localStorage.cart = JSON.stringify($scope.cart);
    }

    var getTotalPrice = function (x) {
        $rootScope.u = angular.isDefined($rootScope.u) ? $rootScope.u : null;
        $http({
            url: 'Orders.asmx/GetTotalPrice',
            method: 'POST',
            data: { groupingCart: x, user: $rootScope.u, course: $rootScope.config.currency.course }
        })
       .then(function (response) {
           $rootScope.price = JSON.parse(response.data.d);
       },
       function (response) {
           alert(response.data.d);
       });
    }
    if ($rootScope.groupingCart !== undefined) {
        getTotalPrice($rootScope.groupingCart);
    }

    var groupingCart = function (x) {
        $http({
            url: 'Cart.asmx/GroupingCart',
            method: 'POST',
            data: { cart: x, product: $scope.p }
        })
       .then(function (response) {
           $rootScope.groupingCart = JSON.parse(response.data.d);
           localStorage.groupingcart = response.data.d;
           priceSumTotal($rootScope.groupingCart);
           getTotalPrice($rootScope.groupingCart);
       },
       function (response) {
           alert(response.data.d);
       });
    };

    $scope.chartMsg = {
        msg: '', css: 'success', icon: 'check'
    }

    $scope.addToCart = function (x) {
        var proceede = false;
        angular.forEach(x, function (value, key) {
            angular.forEach(value.stock, function (val, key) {
                var obj = [];
                if (val.quantity > 0 && val.quantity <= val.uttstock * 1 + val.suppstock * 1) {
                    obj = val;
                    $scope.cart.push(obj);
                    proceede = true;
                }
            })
        })
        if (proceede == false) {
            $scope.chartMsg.msg = $translate.instant('choose color and size');
            $scope.chartMsg.css = 'danger';
            $scope.chartMsg.icon = 'exclamation';
            return false;
        }
        localStorage.cart = JSON.stringify($scope.cart);
        groupingCart($scope.cart);
        $scope.addedToCart = true;
        $scope.chartMsg.msg = $translate.instant('product successfully added to your cart');
        $scope.chartMsg.css = 'success';
        $scope.chartMsg.icon = 'check';
    }

    $scope.delivery = function (x) {
        if (x.quantity > 0) {
            if (x.quantity <= x.uttstock) {
                return { title: '', css: 'label label-success', css_bg: 'bg-success' };
            } else {
                return { title: 'not available', css: 'label label-danger', css_bg: 'bg-danger text-center' };
            }
        }
    }

    $scope.priceSum = function (x) {
        var total = { net: 0, gross: 0 };
        angular.forEach(x, function (value, key) {
            angular.forEach(value.stock, function (val, key) {
                if (val.quantity > 0 && val.quantity <= val.uttstock * 1 + val.suppstock * 1) {
                    total.net = total.net + (val.myprice.net * val.quantity);
                    //total.gross = total.gross + (val.myprice.gross * val.quantity);
                }
            })
        })
        total.net = total.net.toFixed(2) * $rootScope.config.currency.course;
        total.gross = (total.net * $rootScope.config.vatcoeff).toFixed(2);
        return total;
    }

    $scope.productPriceTotal = function (x) {
        var total = { net: 0, gross: 0 };
        angular.forEach(x, function (value, key) {
            total.net = total.net + value.myprice.net * value.quantity;
            total.gross = total.gross + value.myprice.gross * value.quantity;
        })
        return total;
    }

    priceSumTotal = function (x) {
        $rootScope.priceTotal = { net: 0, gross: 0 };
        angular.forEach(x, function (value, key) {
            angular.forEach(value.data, function (v, key) {
                if (v.quantity > 0) {
                    $rootScope.priceTotal.net = $rootScope.priceTotal.net + (v.myprice.net * v.quantity);
                    $rootScope.priceTotal.gross = $rootScope.priceTotal.gross + (v.myprice.gross * v.quantity);
                }
            })
        })
    }
    if (angular.isDefined(localStorage.groupingcart) && localStorage.groupingcart != '') {
        priceSumTotal(JSON.parse(localStorage.groupingcart));
        getTotalPrice(JSON.parse(localStorage.groupingcart));
    } else {
        $rootScope.priceTotal = { net: 0, gross: 0 };
    }

    $scope.priceSumTotal = function () {
        priceSumTotal($rootScope.groupingCart);
        getTotalPrice($rootScope.groupingCart);
    }

    $rootScope.multipleColorStyle = function (x, c) {
        var length = x.colorhex.split('/').length;
        return 'background-color:' + c + '; width:30px; height:' + 30/length + 'px';
    }

    $rootScope.multipleColorStyle2 = function (x, c) {
        var length = x.colorhex.split('/').length;
        return 'background-color:' + c + '; width:20px; height:' + 20 / length + 'px';
    }

    var getProductColorImg = function (x) {
        $http({
            url: 'Products.asmx/GetProductColorImg',
            method: 'POST',
            data: { style: $scope.p.style, color: x.color }
        })
       .then(function (response) {
           $scope.p.packshotimageurl = JSON.parse(response.data.d).packshotimageurl;
       },
       function (response) {
           alert(response.data.d);
       });
    }

    $scope.filterColors = [];
    $scope.filterColor = function (x, scroll) {
        if (scroll) {
            window.scrollTo(0, 0);
        }
        $scope.filterColors.push(x.colorhex);
      //  load(style, x.color);
        getProductColorImg(x);
    }

    $scope.checkColorfilter = function (filterColors, color) {
        if (filterColors.length == 0) {
            return true;
        }
        if (filterColors.indexOf(color) > -1) {
            return true;
        } else {
            return false;
        }
    }

    $scope.clearFilters = function () {
        $scope.filterColors = [];
    }

    $scope.setColorImage = function (x) {
        load(style, x.color);
    }

    $scope.mainImgIdx = 0;
    $scope.selectImg = function (idx) {
        $scope.mainImgIdx = idx;
    }

    $scope.shortdesc = function (x, lang) {
        return (functions.shortdesc(x, lang));
    }

}])

.controller('userCtrl', ['$scope', '$http', '$rootScope', '$sessionStorage', 'functions', '$translate', '$translatePartialLoader', '$localStorage', '$window', '$timeout', function ($scope, $http, $rootScope, $sessionStorage, functions, $translate, $translatePartialLoader, $localStorage, $window, $timeout) {
    $scope.currentStep = 1;
    $scope.cart = !angular.isDefined(localStorage.cart) || localStorage.cart == '' ? [] : JSON.parse(localStorage.cart);
    $rootScope.groupingCart = !angular.isDefined(localStorage.groupingcart) || localStorage.groupingcart == '' ? [] : JSON.parse(localStorage.groupingcart);
    $scope.userType = 'register';
    $scope.email = null;
    $scope.password = null;
    $scope.alertmsg = null;
    $scope.alertclass = 'info';
    $scope.personType = 0;

    var getCountries = function () {
        $http({
            url: 'Users.asmx/GetCountries',
            method: 'POST',
            data: {}
        })
     .then(function (response) {
         $scope.countries = JSON.parse(response.data.d);
     },
     function (response) {
         alert(JSON.parse(response.data.d));
     });
    }
    getCountries();

    var getCompanyInfo = function () {
        $http({
            url: 'Admin.asmx/GetCompanyInfo',
            method: 'POST',
            data: {}
        })
     .then(function (response) {
         $scope.companyInfo = JSON.parse(response.data.d);
     },
     function (response) {
         alert(JSON.parse(response.data.d));
     });
    }
    getCompanyInfo();


    $scope.setCountry = function (x) {
        $rootScope.u.country = x;
    }

    $scope.setDeliveryCountry = function (x) {
        $rootScope.u.deliveryCountry = x;
    }

    $scope.login = function (email, password, isCheckout) {
        $scope.alertmsg = null;
        $http({
            url: 'Users.asmx/Login',
            method: 'POST',
            data: { userName: email, password: password }
        })
     .then(function (response) {
         $rootScope.u = JSON.parse(response.data.d);
         $sessionStorage.u = response.data.d;
         if ($rootScope.u.userId == null) {
             $scope.loginMsg = $translate.instant("wrong username or password");
             $scope.loginMsgClass = 'message-box message-danger';
             $scope.showLoginMsg = true;
         } else {
             if (isCheckout == true) {
                 $scope.nextStep('deliveryTpl', 2);
             } else {
                 $scope.successLoginMsg = $translate.instant("you have successfully logged in into") + " " + $rootScope.config.appname;
                 $scope.loginMsgClass = 'message-box message-success';
                 $scope.showSuccessLoginMsg = true;
             }
         }
     },
     function (response) {
         alert(response.data.d);
     });
    }

    $scope.initUser = function (isguest) {
        $http({
            url: 'Users.asmx/Init',
            method: 'POST',
            data: {}
        })
     .then(function (response) {
         $rootScope.u = JSON.parse(response.data.d);
         $scope.isGuest = isguest;
         $scope.setDeliveryAddress($scope.sameDeliveryAddress);
     },
     function (response) {
         alert(JSON.parse(response.data.d));
     });
    }

    $scope.toggleTpl = function (x) {
        $scope.tpl = x;
    }

    $scope.toggleUserTpl = function (x) {
        $scope.userTpl = x;
    }

    if (!angular.isDefined($sessionStorage.u) || $sessionStorage.u == null) {
        $scope.initUser(false);
        $scope.toggleTpl('registerTpl');
    } else {
        $rootScope.u = JSON.parse($sessionStorage.u);
        $scope.toggleTpl('deliveryTpl');
    }

    var initOrder = function () {
        $http({
            url: 'Orders.asmx/Init',
            method: 'POST',
            data: {}
        })
     .then(function (response) {
         $scope.order = JSON.parse(response.data.d);
     },
     function (response) {
         alert(JSON.parse(response.data.d));
     });
    }
    initOrder();

    $scope.showSignupTpl = function () {
        $scope.signupTpl = true;
    }

 
    $scope.signup = function (u, isCheckout) {
        $scope.alertmsg = null;
        if (functions.isNullOrEmpty(u.firstName)) {
            $scope.alertmsg = $translate.instant('first name is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.lastName)) {
            $scope.alertmsg = $translate.instant('last name is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.address)) {
            $scope.alertmsg = $translate.instant('address is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.postalCode)) {
            $scope.alertmsg = $translate.instant('postal code is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.city)) {
            $scope.alertmsg = $translate.instant('city is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.country.Name)) {
            $scope.alertmsg = $translate.instant('country is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.deliveryFirstName)) {
            $scope.alertmsg = $translate.instant('delivery first name is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.deliveryLastName)) {
            $scope.alertmsg = $translate.instant('delivery last name is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.deliveryAddress)) {
            $scope.alertmsg = $translate.instant('delivery address is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.deliveryPostalCode)) {
            $scope.alertmsg = $translate.instant('delivery postal code is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.deliveryCity)) {
            $scope.alertmsg = $translate.instant('delivery city is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.deliveryCountry.Name)) {
            $scope.alertmsg = $translate.instant('delivery country is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (functions.isNullOrEmpty(u.email)) {
            $scope.alertmsg = $translate.instant('email is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (u.email != u.emailConfirm) {
            $scope.alertmsg = $translate.instant('emails are not the same');
            return false;
        }
        if (functions.isNullOrEmpty(u.password)) {
            $scope.alertmsg = $translate.instant('password is required');
            $scope.alertclass = 'danger';
            return false;
        }
        if (u.password != u.passwordConfirm) {
            $scope.alertmsg = $translate.instant('passwords are not the same');
            return false;
        }

        $http({
            url: 'Users.asmx/Signup',
            method: 'POST',
            data: { x: u }
        })
         .then(function (response) {
             $scope.alertmsg = $translate.instant(response.data.d);
             var sendto = u.email;
             var subject = $translate.instant('registration') + ' ' + $rootScope.config.appname;
             var body = '<img src="https://www.' + $rootScope.config.appname + '/assets/img/' + $rootScope.config.logo + '">' +
             '<br/>' +
             '<hr/>' +
             '<p>' + $translate.instant('dear') + ', '  + u.firstName + ' ' + u.lastName + ', ' + u.companyName + '</p>' +
             '<p>' + $translate.instant('thank you for registering on') + ' Promo-Tekstil.com</p>' +
             '<br/>' +
             '<p>' + $translate.instant('your account information is as follows') + ':</p>' +
             '<p>' + $translate.instant('user name') + ': ' + u.email + '</p>' + 
             '<p>' + $translate.instant('password') + Lozinka + ': ' + u.password + '</p>' +
             '<br/>' +
             '<p>' + $translate.instant('you can log in by following the link') + ': <a href="https://www.' + $rootScope.config.appname + '/login.html">https://www.' + $rootScope.config.appname + '/login.html</a></p>' +
             '<p>' + $translate.instant('you can edit your user profile by following the link') + ': <a href="https://www.' + $rootScope.config.appname + '/user.html">https://www.' + $rootScope.config.appname + '/user.html</a></p>' +
             '<br/>' +
             '<p>' + $translate.instant('if you have any questions feel free to contact us via e-mail') + ' <a href="mailto:' + $scope.companyInfo.email + '?Subject=Upit" target="_top">' + $scope.companyInfo.email + '</a>.</p>' +
             '<br/>' +
             '<p>' + $translate.instant('best regards') + ',</p>' +
             '<br/>' +
             '<p>' + $translate.instant('your') + ' <a href="https://wwww.' + $rootScope.config.appname + '">' + $rootScope.config.name + ' Tim</a></p>';
             sendMail(sendto, subject, body, [], null);
             if (isCheckout == true) {
                 $scope.nextStep('shippingMethodTpl', 3);
             }
         },
         function (response) {
             $scope.alertmsg = response.data.Message;
         });
    }

    $scope.checkoutSignup = function (u) {
        $scope.signup(u, true);
    }


    var sendMail = function (sendto, subject, body, cc, file) {
        $http({
            url: 'Mail.asmx/SendMail',
            method: 'POST',
            data: { sendTo: sendto, subject: subject, body: body, cc: cc, file: file }
        })
        .then(function (response) {
            //alert($translate.instant(response.data.d));
        },
        function (response) {
            alert(response.data.d);
        });
    }

    $scope.confirm = function (u, tpl, step) {
            $http({
                url: 'Users.asmx/Update',
                method: 'POST',
                data: { x: u }
            })
        .then(function (response) {
            $scope.nextStep(tpl, step);
        },
        function (response) {
        alert(response.data.d);
        });
    }

   var clearCart = function () {
        localStorage.clear();
        $scope.cart = [];
        $rootScope.groupingCart = [];
   }

   $scope.accept = false;
   $scope.acceptAlert = false;
   $scope.sendtoprint = false;
   $scope.saveOrder = function (u, accept, sendtoprint) {
       if (accept == false) {
           $scope.acceptAlert = true;
           return false;
       }
        $scope.order.userId = u.userId;
        $scope.order.orderDate = new Date().toLocaleDateString();
        $scope.order.price = $scope.price;
            $http({
                url: 'Orders.asmx/Save',
                method: 'POST',
                data: { user: u, order: $scope.order, cart: $rootScope.groupingCart, lang: $sessionStorage.config.language.code, sendToPrint: sendtoprint }
            })
        .then(function (response) {
           if (response.data.d.startsWith($translate.instant('no stock for'))) {
               alert(response.data.d);
           }

           $scope.order = JSON.parse(response.data.d);

           items = '';
           var li = '';
           angular.forEach($scope.order.items, function (value, key) {
               if (value != null) {
                   li = li + '<li>' + value.style + ' - ' + $translate.instant(value.shortdesc_en) + ', sku:' + value.sku + ', ' + $translate.instant(value.color) + ', ' + value.size + ', ' + value.quantity + ' ' + $translate.instant('piece') + '</li>';
               }
           })
           if (li != '') { items = '<ul>' + li + '</ul>'; }

           var deliveryAddress = '<br />' + 
               $scope.order.deliveryFirstName + ' ' + $scope.order.deliveryLastName + '<br />' +
               $scope.order.deliveryCompanyName + '<br />' +
               $scope.order.deliveryAddress + ', ' + $scope.order.deliveryPostalCode + ' ' + $scope.order.deliveryCity + '<br/>' +
               $translate.instant($scope.order.deliveryCountry) + '<br />';

             var sendto = u.email;
             var subject = $translate.instant('new order') + ' - Promo-Tekstil.com';
             var body = '<img src="https://www.' + $rootScope.config.appname + '/assets/img/' + $rootScope.config.logo + '">' +
             '<br/>' +
             '<hr/>' +
             '<p>' + $translate.instant('dear') + ', '  + u.firstName + ' ' + u.lastName + ', ' + u.companyName + '</p>' +
             '<p>' + $translate.instant('thank you for placing your order') +'</p>' +
             '<br/>' +
             '<p>' + $translate.instant('order details') + ': ' +
             '<p>' + $translate.instant('order number') + ': ' + $scope.order.number + '</p>' +
             '<p>' + $translate.instant('order status') + ': ' + $translate.instant($scope.order.status.title) + '</p>' +
             '<p>' + $translate.instant('ordered products') + ': ' + items + '</p>' +
             '<p>' + $translate.instant('total') + ': ' + $scope.price.total.toFixed(2) + ' ' + $rootScope.config.currency.symbol + '</p>' +
             '<p>' + $translate.instant('delivery address') + ': ' + deliveryAddress + '</p>' +
             '<br/>' +
             '<p>' + $translate.instant('payment type') + ': ' + $translate.instant($scope.order.paymentMethod.title) + '.</p>' +
             '<p>' + $translate.instant('delivery type') + ': ' + $translate.instant($scope.order.deliveryType.title) + '.</p>' +
             '<br/>' +
             '<label>' + $translate.instant('payment details') + '</label>' +
                '<hr />' +
                '<ul>' +
                    '<li>IBAN: ' + $scope.companyInfo.iban + '</li>' +
                    '<li>' + $translate.instant('bank') + ': ' + $scope.companyInfo.bank + '</li>' +
                    '<li>' + $translate.instant('company') + ': ' + $scope.companyInfo.company + '</li>' +
                    '<li>' + $translate.instant('payment model') + ': HR99</li>' +
                    '<li>' + $translate.instant('amount') + ': <strong>' + $scope.price.total.toFixed(2) + ' </strong>' + $rootScope.config.currency.symbol + '</li>' +
                    '<li>' + $translate.instant('description of payment') + ': ' + $scope.order.number + '</li>' +
                '</ul>' +
                '<hr />' +
             '<br/>' +
             '<p>' + $translate.instant('after receiving your payment we will ship your products') + '.</p>' +
             '<p>' + $translate.instant('if you have any questions feel free to contact us via e-mail') + '.</p>' +
             '<br/>' +
             '<p>' + $translate.instant('best regards') + ',</p>' +
             '<br/>' +
             '<p>' + $translate.instant('your') + ' <a href="' + $rootScope.config.appname + '">' + $rootScope.config.appname + ' ' + $translate.instant('team') + '</a></p>';

             sendMail(sendto, subject, body, [], $scope.getPdfLink($scope.order, 'offer'));

            var bodyToMe =
               '<p>' + $translate.instant('new order') + ': ' +
               '<p>' + $translate.instant('order number') + ': ' + $scope.order.number + '</p>' +
               '<p>' + $translate.instant('order status') + ': ' + $translate.instant($scope.order.status.title) + '</p>' +
               '<p>' + $translate.instant('ordered products') + ': ' + items + '</p>' +
               '<p>' + $translate.instant('total') + ': ' + $scope.price.total.toFixed(2) + ' ' + $rootScope.config.currency.symbol + '</p>' +
               '<p>' + $translate.instant('delivery address') + ': ' + deliveryAddress + '</p>' +
               '<br/>' +
               '<p>' + $translate.instant('payment type') + ': ' + $translate.instant($scope.order.paymentMethod.title) + '.</p>' +
               '<p>' + $translate.instant('delivery type') + ': ' + $translate.instant($scope.order.deliveryType.title) + '.</p>';

            $timeout(function () {
                sendMail($rootScope.config.email, subject, bodyToMe, $rootScope.config.emailcc, null);
            }, 2000);

             $scope.nextStep('orderConfirmationTpl', 6);
             clearCart();
       },
       function (response) {
           alert(response.data.d);
       });
    }

    $scope.showForgotPassword = function () {
        $scope.forgotPass = true;
    }

    $scope.forgotPassword = function (u) {
        $http({
            url: 'Users.asmx/ForgotPassword',
            method: 'POST',
            data: { email: u.email }
        })
     .then(function (response) {
         var user = JSON.parse(response.data.d);
         var sendto = user.email;
         var subject = $translate.instant('account information') + ' - Promo-Tekstil.com';
         var body = '<img src="https://www.' + $rootScope.config.appname + '/assets/img/' + $rootScope.config.logo + '">' +
         '<br/>' +
         '<hr/>' +
         '<p>' + $translate.instant('dear') + ', ' + user.firstName + ' ' + user.lastName + ', ' + user.companyName + '</p>' +
         '<br/>' +
         '<p>' + $translate.instant('your account information is as follows') + ':</p>' +
         '<p>Korisničko ime: ' + user.userName + '</p>' +
         '<p>Lozinka: ' + user.password + '</p>' +
         '<br/>' +
         '<p>Prijavite se klikom na poveznicu: <a href="https://www.' + $rootScope.config.appname + '/login.html">https://www.' + $rootScope.config.appname + '/login.html</a></p>' +
         '<p>Vaš korisnički profil možete uređivati na poveznici: <a href="https://www.' + $rootScope.config.appname + '/signup.html">https://www.' + $rootScope.config.appname + '/signup.html</a></p>' +
         '<br/>' +
         '<p>Želimo vam ugodnu kupovinu.</p>' +
         '<p>Stojimo na raspolaganju za sve vaše upite. Kontaktirati nas možete putem e-maila <a href="mailto:' + $scope.companyInfo.email + '?Subject=Upit" target="_top">' + $scope.companyInfo.email + '</a>.</p>' +
         '<br/>' +
         '<p>Srdačno,</p>' +
         '<br/>' +
         '<p>Vaš <a href="https://www.' + $rootScope.config.appname + '">' + $rootScope.config.name + ' Tim</a></p>';

         sendMail(sendto, subject, body, [], null);

         $scope.loginMsg = $translate.instant("your account information has been sent to the email address");
         $scope.loginMsgClass = 'info';
         $scope.showLoginMsg = true;
     },
     function (response) {
         $scope.loginMsg = $translate.instant(response.data.Message);
         $scope.loginMsgClass = 'warning';
         $scope.showLoginMsg = true;
     });
    }

    var gotoAnchor = function (x) {
        $window.location.href = '#' + x;
    };

    $scope.nextStep = function (tpl, step) {
        if (step == 3) {
            $http({
                url: 'Orders.asmx/GetOrderOptionsJson',
                method: 'POST',
                data: ''
            })
               .then(function (response) {
                   $rootScope.orderOptions = JSON.parse(response.data.d);
               },
               function (response) {
                   alert(response.data.d);
               });
        }
        if (step == 5) {
            getTotalPrice($rootScope.groupingCart);
        }
        $scope.toggleTpl(tpl);
        $scope.currentStep = step;
        gotoAnchor('checkout');
    }

    $scope.setProgressBarClass = function (x) {
        if ($scope.currentStep == x) {
            return 'active';
        }
        if ($scope.currentStep > x) {
            return 'visited';
        }
        if ($scope.currentStep < x) {
            return '';
        }
    }

    $scope.showPassword = function () {
        $scope.showpass = $scope.showpass == true ? false : true;
    }

    $scope.updateUser = function (u) {
        $scope.alertmsg = null;
        $http({
            url: 'Users.asmx/Update',
            method: 'POST',
            data: { x: u }
        })
     .then(function (response) {
         $sessionStorage.u = JSON.stringify($rootScope.u)
         $scope.alertmsg = response.data.d;
     },
     function (response) {
         $scope.alertmsg = response.data.d;
     });
    }

    var get = function () {
        $http({
            url: 'Orders.asmx/Get',
            method: 'POST',
            data: { userId: $rootScope.u.userId }
        })
     .then(function (response) {
         $rootScope.o = JSON.parse(response.data.d);
         if (angular.isDefined($sessionStorage.u)) {
             $scope.toggleUserTpl('editUserTpl');
         }
     },
     function (response) {
         alert(JSON.parse(response.data.d));
     });
    }
    if (angular.isDefined($rootScope.u) && $rootScope.u != null) { get(); };

    //$scope.sameDeliveryAddress = true;
    $scope.setDeliveryAddress = function (isTheSame) {
        if (isTheSame == true) {
            $rootScope.u.deliveryFirstName = $rootScope.u.firstName;
            $rootScope.u.deliveryLastName = $rootScope.u.lastName;
            $rootScope.u.deliveryCompanyName = $rootScope.u.companyName;
            $rootScope.u.deliveryAddress = $rootScope.u.address;
            $rootScope.u.deliveryPostalCode = $rootScope.u.postalCode;
            $rootScope.u.deliveryCity = $rootScope.u.city;
            $rootScope.u.deliveryCountry = $rootScope.u.country;
        } else {
            $rootScope.u.deliveryFirstName = '';
            $rootScope.u.deliveryLastName = '';
            $rootScope.u.deliveryCompanyName = '';
            $rootScope.u.deliveryAddress = '';
            $rootScope.u.deliveryPostalCode = '';
            $rootScope.u.deliveryCity = '';
            $rootScope.u.deliveryCountry = '';
        }
    }

    $scope.productPriceTotal = function (x) {
        var total = { net: 0, gross: 0 };
        angular.forEach(x, function (value, key) {
            total.net = total.net + value.myprice.net * value.quantity;
            total.gross = total.gross + value.myprice.gross * value.quantity;
        })
        return total;
    }

    $scope.priceTotal = [];
    var getTotalPrice = function (x) {
        $http({
            url: 'Orders.asmx/GetTotalPrice',
            method: 'POST',
            data: { groupingCart: x, user: $rootScope.u, course: $rootScope.config.currency.course }
        })
       .then(function (response) {
           $scope.price = JSON.parse(response.data.d);
       },
       function (response) {
           alert(response.data.d);
       });
    }

    $scope.getPdfLink = function (x, type) {
        var link = null;
        if (x !== undefined) {
            link = type == 'offer'
            ? 'upload/' + type + '/' + x.orderId + '.pdf'
            : 'upload/' + type + '/' + x.invoiceId + '.pdf';
        }
        return link;
    }


}])

.controller('contactCtrl', ['$scope', '$http', '$rootScope', '$sessionStorage', 'functions', '$translate', '$translatePartialLoader', '$localStorage', '$window', function ($scope, $http, $rootScope, $sessionStorage, functions, $translate, $translatePartialLoader, $localStorage, $window) {
    $scope.d = {
        firstName: null,
        lastName: null,
        email: null,
        message: null
    }
    $scope.alertmsg = null;
    $scope.issent = false;
    $scope.isrequired = false;

    var getCompanyInfo = function () {
        $http({
            url: 'Admin.asmx/getCompanyInfo',
            method: 'POST',
            data: {}
        })
     .then(function (response) {
         $scope.companyInfo = JSON.parse(response.data.d);
     },
     function (response) {
         alert(JSON.parse(response.data.d));
     });
    }
    getCompanyInfo();

    $scope.send = function (d) {
        if (functions.isNullOrEmpty(d.firstName)) {
            $scope.alertmsg = $translate.instant('first name is required');
            return false;
        }
        if (functions.isNullOrEmpty(d.email)) {
            $scope.alertmsg = $translate.instant('email is required');
            return false;
        }
        if (functions.isNullOrEmpty(d.message)) {
            $scope.alertmsg = $translate.instant('message is required');
            return false;
        }

        $scope.issent = false;
        var subject = $translate.instant('inquiry') + ' - ' + config.appname; // ' - Promo-Tekstil.com';
        var body = '<p>' + $translate.instant('inquiry') + ':</p>' +
        '<p>' + $translate.instant('first name') + ': ' + d.firstName + '</p>' +
        '<p>' + $translate.instant('last name') + ': ' + d.lastName + '</p>' +
        '<p>' + $translate.instant('email') + ': ' + d.email + '</p>' +
        '<br/>' +
        '<p>Upit: ' + d.message + '</p>';
        $http({
            url: 'Mail.asmx/SendMail',
            method: 'POST',
            data: { sendTo: $rootScope.config.email, subject: subject, body: body, cc: $rootScope.config.emailcc, file:null }
        })
     .then(function (response) {
         $scope.issent = true;
         $scope.alertmsg = null;
         $scope.response = $translate.instant(response.data.d);
         $window.location.href = '#msg';
     },
     function (response) {
         $scope.issent = false;
         $scope.alertmsg = null;
         alert(response.data);
     });
    }

}])

//.controller('cartCtrl', ['$scope', '$http', '$rootScope', '$sessionStorage', '$localStorage', function ($scope, $http, $rootScope, $sessionStorage, $localStorage) {
//    $rootScope.groupingCart = !angular.isDefined(localStorage.groupingcart) || localStorage.groupingcart == '' ? [] : JSON.parse(localStorage.groupingcart);
//    $rootScope.u = angular.isDefined($rootScope.u) ? $rootScope.u : null;
//    var getTotalPrice = function (x) {
//        $http({
//            url: 'Orders.asmx/GetTotalPrice',
//            method: 'POST',
//            data: { groupingCart: x, user: $rootScope.u, course: $rootScope.config.currency.course }
//        })
//       .then(function (response) {
//           $scope.price = JSON.parse(response.data.d);
//       },
//       function (response) {
//           alert(response.data.d);
//       });
//    }
//    getTotalPrice($rootScope.groupingCart);
//}])

.directive('checkImage', function ($http) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            attrs.$observe('ngSrc', function (ngSrc) {
                $http.get(ngSrc).success(function () {
                }).error(function () {
                    element.attr('src', './assets/img/default.png'); // set default image
                });
            });
        }
    };
})
.directive('checkLink', function ($http) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            attrs.$observe('href', function (href) {
                $http.get(href).success(function () {
                }).error(function () {
                    element.attr('class', 'btn btn-default');
                    element.attr('disabled', 'disabled');
                });
            });
        }
    };
})
;




;