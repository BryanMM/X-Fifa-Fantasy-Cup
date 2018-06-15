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
    $scope.goRegister = function () {
        $location.path("/register");
    }
    $scope.goLogin = function () {

        $http.post(Host + "/api/user/login", { username: $scope.usr, password: $scope.pswrd }).
            then((promise) => {
                if (promise.data.success) {
                    $location.path("/userCalendar");
                    UserName = $scope.usr;
                }
                else {
                    $scope.usr = ""
                    $scope.pswrd = ""
                    alert("the username and password you entered don't match")
                }

            });
    }
});

login.controller("userRegister", function ($scope, $rootScope, $location, $http) {
    $rootScope.showItem = false;

    $scope.country = [];
    $http.get(Host + "/api/user/countries").
        then((promise) => {
            let mydata = promise.data;
            $scope.country = mydata.states;

        });


    $scope.subData = function () {
        if ($scope.pass1 == $scope.pass2) {
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
                    if (promise.data.success) {
                        $location.path("/");
                    }
                });
        }
    }
});

login.controller("userCalendar", function ($scope, $rootScope, $location, $http) {
    $rootScope.showItem = true;
});