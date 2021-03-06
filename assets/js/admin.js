﻿/*!
admin.js
(c) 2017-2019 IG PROG, www.igprog.hr
*/
angular.module('app', [])

.controller('adminCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    var reloadPage = function () {
        if (typeof (Storage) !== 'undefined') {
            if (localStorage.version) {
                if (localStorage.version != $rootScope.config.version) {
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
              $rootScope.config = response.data;
              reloadPage();
          });
    };
    getConfig();

    $scope.islogin = false;
    var d = new Date();
    $scope.year = d.getFullYear();

    $scope.toggleTpl = function (x) {
        $scope.tpl = x;
    };
    $scope.toggleTpl('login');

    init = function () {
        $scope.user = {
            username: null,
            password: null
        }
    }
    init();

    $scope.login = function (u) {
        $http({
            url: 'Admin.asmx/Login',
            method: 'POST',
            data: {username: u.username, password: u.password }
        })
         .then(function (response) {
             $scope.islogin = JSON.parse(response.data.d);
             if ($scope.islogin == true) {
                 $scope.toggleTpl('orders');
             } else {
                 alert('error login');
             }
         },
         function (response) {
             $scope.islogin = false;
             alert(response.data.d);
         });
    }

    $scope.logout = function () {
        $scope.islogin = false;
        $scope.toggleTpl('login');
        init();
    }

}])

.controller('ordersCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {
    var getConfig = function () {
        $http.get('./config/config.json')
          .then(function (response) {
              $scope.config = response.data;
          });
    };
    getConfig();

    var clear = function () {
        $scope.stockRequest = null;
        $scope.sendOrderRequest = null;
        $scope.stockResponse = null;
        $scope.orderResponse = null;
    }
    clear();

    var load = function () {
        $http({
            url: 'Orders.asmx/Load',
            method: 'POST',
            data: ''
        })
         .then(function (response) {
             $scope.d = JSON.parse(response.data.d);
         },
         function (response) {
             alert(response.data.d);
         });
        }
    load();

    var getOrderOptions = function () {
        $http({
            url: 'Orders.asmx/GetOrderOptionsJson',
            method: 'POST',
            data: ''
        })
         .then(function (response) {
             $scope.orderOptions = JSON.parse(response.data.d);
         },
         function (response) {
             alert(response.data.d);
         });
    }
    getOrderOptions();

    $scope.update = function (x) {
        $http({
            url: 'Orders.asmx/Update',
            method: 'POST',
            data: { order: x }
        })
         .then(function (response) {
             load();
         },
         function (response) {
             alert(response.data.d);
         });
    }

    $scope.remove = function (x) {
        $http({
            url: 'Orders.asmx/Delete',
            method: 'POST',
            data: {id: x.orderId}
        })
         .then(function (response) {
             load();
         },
         function (response) {
             alert(response.data.d);
         });
    }

    //TODO: Backend method: GetItemsDatas for one order
    var getOrderByOrderId = function (x) {
        $http({
            url: 'Orders.asmx/GetOrderByOrderId',
            method: 'POST',
            data: { orderId: x }
        })
        .then(function (response) {
            $scope.o = JSON.parse(response.data.d);

            //$scope.o.number = res.number;
            //$scope.o.invoice = res.invoice;
            //$scope.o.invoiceId = res.invoiceId;
        },
        function (response) {
            alert(response.data.d);
        });
    }

    $scope.isdetails = false;
    $scope.showDetails = function (x, show) {
        clear();
        if (x != null) {
            getOrderByOrderId(x.orderId);
            if (x != null) {
                $scope.items = GetItems(x.items);
                $scope.items_utt = GetItems_utt($scope.items);
            }
        } else {
            load();
        }
        $scope.isdetails = show;
    }

    var GetItems = function (x) {
        var items = [];
        angular.forEach(x, function (value, key) {
            var item = { sku: '', qty: 0, isoderable: '' }
            item.sku = value.sku;
            item.qty = value.quantity;
            item.color = value.color;
            item.size = value.size;
            item.supplier = value.supplier;
            items.push(item);
        });
        return items;
    }

    var GetItems_utt = function (x) {
        var items = [];
        angular.forEach(x, function (value, key) {
            var item = { sku: '', qty: 0 }
            item.sku = value.sku;
            item.qty = value.qty;
            items.push(item);
        });
        return items;
    }

    var uttPost = function (request) {
        $http({
            url: 'Orders.asmx/PostUtt',
            method: 'POST',
            data: { data: JSON.stringify(request) }
        })
         .then(function (response) {
             $scope.stockResponse = JSON.parse(response.data.d);
             checkOderable($scope.items);
         },
         function (response) {
             alert(response.data.d);
         });
    }

    $scope.checkStock = function () {
        $scope.items_utt = GetItems_utt($scope.items);
        var request = {
            "request":
            {
                "header": {
                    "auth": {
                        "key": $scope.config.apikey
                    }
                },
                "body": {
                    "items": $scope.items_utt
                }
            }
        };
        $scope.stockRequest = request;
        uttPost(request);
    }

    var checkOderable = function (items) {
        if ($scope.stockResponse != null) {
            angular.forEach(items, function (value, key) {
                value.isoderable = $scope.stockResponse.response.body.items[key].status.message;
            });
        }
    }

    $scope.sendOrder = function (x) {
        var request = {
            "request":
                {
                    "header": {
                        "auth": {
                            "key": $scope.config.apikey
                        },
                        "testMode": true
                    },
                    "body": {
                        "order": {
                            "ordernumber": "",
                            "shipmethod": "courier",
                            "shiptype": "complete",
                            "plainorder": true,
                            "deliverydate": "",
                            "comment": "api test",
                            "shipto": {
                                "company": $scope.o.deliveryCompanyName, // "My Customer",
                                "address": $scope.o.deliveryAddress, // "Customer street building A nr.99",
                                "city": $scope.o.deliveryCity, // "City",
                                "zip": $scope.o.deliveryPostalCode, // "ZIP",
                                "countrycode": $scope.o.countryCode // "HR"
                            },
                            "contact": {
                                "name": $scope.config.company, //  "My Customer",
                                "phone": $scope.config.phone // "+99-9999-9999"
                            }
                        },
                        "orderlines": $scope.items_utt
                    }
                }
        }
        $scope.sendOrderRequest = request;
        uttPost(request);  //<<TODO;
    }

    $scope.createOfferPdf = function (o, isForeign) {
        $scope.loading = true;
        $scope.pdfLink = null;
        var tempFileName = null;
        $http({
            url: 'PrintPdf.asmx/OfferPdf',
            method: 'POST',
            data: { order: o, isForeign: isForeign, lang: 'hr' }
        })
     .then(function (response) {
         $scope.loading = false;
         var tempFileName = response.data.d;
         $scope.pdfLink = 'upload/offer/temp/' + tempFileName + '.pdf';
     },
     function (response) {
         $scope.loading = false;
         alert(response.data.d);
     });
    }

    $scope.removePdfLink = function () {
        $scope.pdfLink = null;
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

    $scope.createInvoicePdf = function (o, isForeign) {
        $scope.loading = true;
        $scope.pdfLink = null;
        var tempFileName = null;
        $http({
            url: 'PrintPdf.asmx/InvoicePdf',
            method: 'POST',
            data: { order: o, isForeign: isForeign, lang: 'hr' }
        })
     .then(function (response) {
         $scope.loading = false;
         var tempFileName = response.data.d;
         $scope.pdfLink = 'upload/invoice/temp/' + tempFileName + '.pdf';
         getOrderByOrderId(o.orderId);
     },
     function (response) {
         $scope.loading = false;
         alert(response.data.d);
     });
    }

}])

.controller('usersCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    $scope.showDetails = false;

    var getOrderOptions = function () {
        $http({
            url: 'Orders.asmx/GetOrderOptionsJson',
            method: 'POST',
            data: ''
        })
         .then(function (response) {
             $scope.orderOptions = JSON.parse(response.data.d);
         },
         function (response) {
             alert(response.data.d);
         });
    }
    getOrderOptions();

    var load = function () {
        $http({
            url: 'Users.asmx/Load',
            method: 'POST',
            data: ''
        })
            .then(function (response) {
                $scope.d = JSON.parse(response.data.d);
            },
            function (response) {
                alert(response.data.d);
            });
    }
    load();

    $scope.update = function (user) {
        $http({
            url: 'Users.asmx/Update',
            method: 'POST',
            data: { x: user }
        })
            .then(function (response) {
                load();
                alert(response.data.d);
            },
            function (response) {
                alert(response.data.d);
            });
    }

    $scope.remove = function(user) {
        var r = confirm("Briši " + user.firstName + " "  + user.lastName + "?");
        if (r == true) {
            remove(user);
        } else {
        }
    }

    var remove = function (user) {
        $http({
            url: 'Users.asmx/Delete',
            method: 'POST',
            data: { x: user }
        })
            .then(function (response) {
                load();
                alert(response.data.d);
            },
            function (response) {
                alert(response.data.d);
            });
    }

    $scope.isdetails = false;
    $scope.showDetails = function (x, show) {
        $scope.u = x;
        $scope.isdetails = show;
    }

}])

.controller('productsCtrl', ['$scope', '$http', '$rootScope', '$window', function ($scope, $http, $rootScope, $window) {

    $scope.csvFileName = "products";
    $scope.tranResp = null;
    $scope.translations = null;
    $scope.loading = false;
    $scope.loading_p = false;
    $scope.lacunaUrl = 'https://vp.lacuna.hr/exportxml.aspx?partner=40204';
    $scope.loading_l = false;

    $scope.upload = function () {
        var content = new FormData(document.getElementById("formUpload"));
        $http({
            url: 'UploadHandler.ashx',
            method: 'POST',
            headers: { 'Content-Type': undefined },
            data: content,
        }).then(function (response) {
            if (response.data != 'OK') {
                alert(response.data);
            } else {
                $window.location.href = 'csv.html?' + $scope.csvFileName;
            }
        },
       function (response) {
           alert(response.data);
       });
    }

    $scope.updateLacuna = function () {
        $scope.loading_l = true;
        $http({
            url: 'Products.asmx/ImportLacunaXML',
            method: 'POST',
            data: ''
        })
         .then(function (response) {
             $scope.loading_l = false;
             alert(response.data.d);
         },
         function (response) {
             $scope.loading_l = false;
             alert(response.data.d);
         });
    }

    $scope.translateProductsFromJson = function () {
        $scope.loading = true;
            $http({
                url: 'Products.asmx/TranslateProductsFromJson',
                method: 'POST',
                data: ''
            })
         .then(function (response) {
             $scope.tranResp = JSON.parse(response.data.d);
             $scope.loading = false;
         },
         function (response) {
             $scope.loading = false;
             alert(response.data.d);
         });
    }

    $scope.loadProductsTranslation = function () {
        $scope.loading_p = true;
        $http({
            url: 'Products.asmx/LoadProductsTranslation',
            method: 'POST',
            data: ''
        })
     .then(function (response) {
         $scope.translations = JSON.parse(response.data.d);
         $scope.loading_p = false;
     },
     function (response) {
         $scope.loading_p = false;
         alert(response.data.d);
     });
    }

    $scope.updateTranslation = function (x) {
        $http({
            url: 'Products.asmx/UpdateTranslation',
            method: 'POST',
            data: {translation: x}
        })
     .then(function (response) {
         alert(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }

}])

.controller('featuredCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    var init = function () {
        $http({
            url: 'Featured.asmx/Init',
            method: 'POST',
            data: ''
        })
     .then(function (response) {
         $scope.d = JSON.parse(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }
    init();

    var load = function () {
        $http({
            url: 'Featured.asmx/Load',
            method: 'POST',
            data: ''
        })
     .then(function (response) {
         $scope.products = JSON.parse(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }
    load();

    //var loadProducts = function () {
    //    $http({
    //        url: 'Featured.asmx/LoadProducts',
    //        method: 'POST',
    //        data: ''
    //    })
    // .then(function (response) {
    //     $scope.allProducts = JSON.parse(response.data.d);
    // },
    // function (response) {
    //     alert(response.data.d);
    // });
    //}
   // loadProducts();


    var save = function (p) {
       p.type = angular.fromJson(p.type);
        $http({
            url: 'Featured.asmx/Save',
            method: 'POST',
            data: {x:p}
        })
     .then(function (response) {
         alert(response.data.d);
         load();
     },
     function (response) {
         alert(response.data.d);
     });
    }

    $scope.save = function (p) {
        save(p);
    }

    var remove = function (p) {
        $http({
            url: 'Featured.asmx/Delete',
            method: 'POST',
            data: { x: p }
        })
     .then(function (response) {
         alert(response.data.d);
         load();
     },
     function (response) {
         alert(response.data.d);
     });
    }

    $scope.remove = function (p) {
        remove(p);
    }
    
}])

.controller('coeffCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    var load = function () {
        $http({
            url: 'Price.asmx/GetPriceCoeff',
            method: 'POST',
            data: { filename: 'pricecoeff' }
        })
     .then(function (response) {
         $scope.d = JSON.parse(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }
    load();

    $scope.add = function () {
        alert('todo');
    }

    $scope.remove = function (x) {
        alert('todo');
    }

    $scope.save = function (x) {
        $http({
            url: 'Price.asmx/Save',
            method: 'POST',
            data: { x: x }
        })
     .then(function (response) {
         alert(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }

}])

.controller('orderOptionsCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    var load = function () {
        $http({
            url: 'Orders.asmx/GetOrderOptionsJson',
            method: 'POST',
            data: ''
        })
     .then(function (response) {
         $scope.d = JSON.parse(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }
    load();

    $scope.add = function (x) {
        x.push({});
    }

    $scope.remove = function (x, idx) {
        x.splice(idx, 1);
    }

    $scope.save = function (x) {
        $http({
            url: 'Orders.asmx/SaveOrderOptions',
            method: 'POST',
            data: { x: x }
        })
     .then(function (response) {
         alert(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }

}])

.controller('categoriesCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    var load = function () {
        $http({
            url: 'Categories.asmx/Get',
            method: 'POST',
            data: ''
        })
     .then(function (response) {
         $scope.d = JSON.parse(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }
    load();

    $scope.add = function (x) {
        x.push({ code: '', title: '' });
    }

    $scope.remove = function (x, idx) {
        x.splice(idx, 1);
    }

    $scope.save = function (x) {
        $http({
            url: 'Categories.asmx/Save',
            method: 'POST',
            data: { x: x }
        })
     .then(function (response) {
         alert(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }

}])

.controller('countriesCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    var load = function () {
        $http({
            url: 'Users.asmx/GetCountries',
            method: 'POST',
            data: {}
        })
     .then(function (response) {
         $scope.d = JSON.parse(response.data.d);
     },
     function (response) {
         alert(JSON.parse(response.data.d));
     });
    }
    load();

    $scope.add = function (x) {
        x.push({ code: '', title: '' });
    }

    $scope.remove = function (x, idx) {
        x.splice(idx, 1);
    }

    $scope.save = function (x) {
        $http({
            url: 'Users.asmx/SaveCountries',
            method: 'POST',
            data: { x: x }
        })
     .then(function (response) {
         alert(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }

}])

.controller('companyInfoCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    var load = function () {
        $http({
            url: 'Admin.asmx/GetCompanyInfo',
            method: 'POST',
            data: {}
        })
     .then(function (response) {
         $scope.d = JSON.parse(response.data.d);
     },
     function (response) {
         alert(JSON.parse(response.data.d));
     });
    }
    load();

    $scope.save = function (x) {
        $http({
            url: 'Admin.asmx/SaveCompanyInfo',
            method: 'POST',
            data: { x: x }
        })
     .then(function (response) {
         $scope.d = JSON.parse(response.data.d);
     },
     function (response) {
         alert(response.data.d);
     });
    }

}])

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
