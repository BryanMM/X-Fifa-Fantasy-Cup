var UserName = "";
var UserInfo = "";
var Activity = 0;
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
        /*
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

            });*/
    }
});

login.controller("register", function ($scope, $rootScope, $location, $http) {
    $rootScope.showItem = false;
    $rootScope.showItemAdmin = false;

    $scope.country = [];
    /*
    $http.get(Host + "/api/Country/countries").
        then((promise) => {
            let mydata = promise.data;
            $scope.country = mydata;

        });*/

    $scope.yearlist = [];
    for (i = 2017; i >= 1900; i--) {
        $scope.yearlist.push(i);
    }
    $scope.monthlist = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"];
    $scope.daylist1 = [];
    $scope.daylist2 = [];
    $scope.daylist3 = [];
    for (i = 1; i <= 28; i++) {
        $scope.daylist1.push(i);
    }
    $scope.daylist2 = [];
    for (i = 1; i <= 30; i++) {
        $scope.daylist2.push(i);
    }
    $scope.daylist3 = [];
    for (i = 1; i <= 31; i++) {
        $scope.daylist3.push(i);
    }
    $scope.chooseDaylist = function () {
        var monthcheck = ["01", "03", "05", "07", "09", "11"];
        if ($scope.month == "02") {
            $scope.daylist = $scope.daylist1;
        } else if (monthcheck.indexOf($scope.month) > -1) {
            $scope.daylist = $scope.daylist2;
        } else {
            $scope.daylist = $scope.daylist3;
        }
    }


    $scope.subData = function () {

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
                birthday: $scope.year.concat("-", $scope.month, "-", $scope.day),
                
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
    $scope.matches = false;
    $scope.stages = false;

    //Teams configuration
    $scope.goMatches = function () {
        $scope.teams = false;
        $scope.matches = true;

    }

    $scope.country = ["Alemania", "USA", "Canada", "China"];
    /*
    $http.get(Host + "/api/Country/countries").
        then((promise) => {
            let mydata = promise.data;
            $scope.country = mydata;

        });*/
    
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

    $scope.test = [];

    $scope.addPlayer = function (person) {
        $scope.playerList[$scope.searchProv].push(person);
        $scope.test = $scope.playerList[$scope.cont];
    }

    $scope.matchList = [];

    //Stages configuration
    $scope.addMatch = function () {
        tempvs = $scope.team1 + " vs " + $scope.team2;
        $scope.matchList.push(tempvs);
    }

    //Matches configuration
    $scope.goStages = function () {
        $scope.matches = false;
        $scope.stages = true;

    }

    var matchNumb = 1;
    $scope.showMatch = [];
    
    $scope.yearlist = [];
    for (i = 2017; i >= 1900; i--) {
        $scope.yearlist.push(i);
    }
    $scope.monthlist = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"];
    $scope.daylist1 = [];
    $scope.daylist2 = [];
    $scope.daylist3 = [];
    for (i = 1; i <= 28; i++) {
        $scope.daylist1.push(i);
    }
    $scope.daylist2 = [];
    for (i = 1; i <= 30; i++) {
        $scope.daylist2.push(i);
    }
    $scope.daylist3 = [];
    for (i = 1; i <= 31; i++) {
        $scope.daylist3.push(i);
    }
    $scope.chooseDaylist = function () {
        var monthcheck = ["01", "03", "05", "07", "09", "11"];
        if ($scope.month == "02") {
            $scope.daylist = $scope.daylist1;
        } else if (monthcheck.indexOf($scope.month) > -1) {
            $scope.daylist = $scope.daylist2;
        } else {
            $scope.daylist = $scope.daylist3;
        }
    }

    $scope.addFight = function () {
        $scope.showMatch.push("Match" + matchNumb.toString());
        $scope.showMatch.push($scope.doneMatch);
        $scope.showMatch.push($scope.year.concat("-", $scope.month, "-", $scope.day));
        $scope.showMatch.push($scope.location);
        matchNumb += 1;
    }

    

});