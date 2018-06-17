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
        })
        .when("/createTournament", {
            templateUrl: "Admin/CreateTournament.html",
            controller: "createTour"
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
        
        $location.path("/adminCalendar");

       $http.post(Host + "/api/user/login", { username: $scope.usr, password: $scope.pswrd }).
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

            });
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
    $scope.goTour = function () {
        $location.path("/createTournament");
    }
});

login.controller("createTour", function ($scope, $rootScope, $location, $http) {
    $scope.teams = true;
    $scope.groups = false;
    $scope.stages = false;

    $scope.goGroups = function () {
        $scope.teams = false;
        $scope.groups = true;

    }

    $scope.country = ["Alemania", "USA", "Canada", "China"];
    $http.get(Host + "/api/Country/countries").
        then((promise) => {
            let mydata = promise.data;
            $scope.country = mydata;

        });
    
    $scope.selectPlayer = ["1111", "2222", "3333", "4444", "5555", "6666", "7777", "8888", "9999", "99991", "99992", "99993", "99994", "99995", "99969", "99799", "98999", "99999"];
    $scope.selectCountry = [];
    $scope.playerList = {};
    var temp = { "Alemania" : ["1", "2"], "USA" : ["1", "2"], "Canada" : ["1", "2"], "China": ["1", "2"] };
    $scope.showList = []

    $scope.displayList = function () {
        var temp2 = $scope.searchProv;
        $scope.showList = temp[temp2];
    }

    $scope.addCountry = function () {
        if ($scope.selectCountry.includes($scope.prov)) {

        }
        else {
            $scope.selectCountry.push($scope.prov);
            $scope.playerList[$scope.prov] = [];
        }
    }

    $scope.addPlayer = function (person) {
        $scope.playerList[$scope.searchProv].push(person);
        console.log($scope.playerList);
    }
    
   
    
});