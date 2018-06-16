var UserName = "";
var Host = "http://localhost:65049";

var login = angular.module("loginApp", ["ngRoute"]);
login.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "User/LoginTemplate.html",
            controller: "userLogin"
        })
        .when("/register", {
            templateUrl: "User/RegisterUser.html",
            controller: "register"
        })
        .when("/userCalendar", {
            templateUrl: "User/UserCalendar.html",
            controller: "userCalendar"
        })
        .when("/adminCalendar", {
            templateUrl: "Admin/AdminCalendar.html",
            controller: "adminCalendar"
        });
});

//Root controller
login.controller("rootCont", function ($scope, $location) {
    $scope.goLogout = function () {
        UserToken = 0;
        UserName = "";
        $location.path("/");
    }
});

//Login verification controller
login.controller("userLogin", function ($scope, $rootScope, $location, $http) {
    $rootScope.showItem = false;
    $rootScope.showItemAdmin = false;
    $scope.goRegister = function () {
        $location.path("/register");
    }
    $scope.goLogin = function () {
        
        $location.path("/userCalendar");

       /*$http.post(Host + "/api/user/login", { username: $scope.usr, password: $scope.pswrd }).
            then((promise) => {
                if (promise.data.success === "true") {
                    UserName = $scope.usr;
                    if (promise.data.detail === "1") { 
                        $location.path("/adminCalendar");
                    }
                    else {
                        $location.path("/userCalendar");
                    }
                }
                else {
                    $scope.usr = ""
                    $scope.pswrd = ""
                    alert(promise.data.detail)
                }

            });*/
    }
});

login.controller("register", function ($scope, $rootScope, $location, $http) {
    $rootScope.showItem = false;
    $rootScope.showItemAdmin = false;

    $scope.country = [];
    $http.get(Host + "/api/Country/countries").
        then((promise) => {
            let mydata = promise.data;
            $scope.country = mydata;

        });


    $scope.subData = function () {
        console.log($scope.date);
        if ($scope.pass1 === $scope.pass2) {
            $http.post(Host + "/api/user/register", {
                firstName: $scope.fname,
                lastName: $scope.lname,
                email: $scope.uemail,
                phone: $scope.phone,
                username: $scope.username,
                password: $scope.pass1,
                country: $scope.prov.Id,
                image: $scope.image,
                birthday: $scope.date,
                
                description: $scope.description
            }).
                then((promise) => {
                    if (promise.data.success === "true") {
                        $location.path("/");
                    }
                });
        }
    }
});

login.controller("userCalendar", function ($scope, $rootScope, $location, $http) {
    $rootScope.showItem = true;
});

login.controller("adminCalendar", function ($scope, $rootScope, $location, $http) {
    $rootScope.showItemAdmin = true;
});