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
        })
        .when("/adminProfile", {
            templateUrl: "Admin/AdminProfile.html",
            controller: "adminProfile"
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
        
       // $location.path("/adminCalendar");
        
       $http.post(Host + "/api/user/login", { username: $scope.usr, password: $scope.pswrd }).
            then((promise) => {
                if (promise.data.success === "true") {
                    UserName = $scope.usr;
                    if (promise.data.detail_type === "1") { 
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
        console.log({
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
        });
        if ($scope.pass1 === $scope.pass2) {
            $http.post(Host + "/api/user/register", {
                fanatic_name: $scope.fname,
                fanatic_last_name: $scope.lname,
                fanatic_email: $scope.uemail,
                fanatic_phone: $scope.phone,
                fanatic_id: $scope.username,
                fanatic_password: $scope.pass1,
                fanatic_country: $scope.prov.Id,
                fanatic_photo: $scope.image,
                fanatic_birth: $scope.day.concat("/", $scope.month, "/", $scope.year).toString(),
                fanatic_description: $scope.description
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
    $scope.name = true;
    $scope.teams = false;
    $scope.matches = false;
    $scope.stages = false;
    var tourID = "";

    //Names Configuration

    $http.get(Host + "/api/tournament/sponsor").
        then((promise) => {
            let mydata = promise.data;
            $scope.sponsor = mydata;

        });

    $scope.goTeams = function () {
        
        $http.post(Host + "/api/tournament/create", {
            tournament_name: $scope.tourName,
            sponsor_id: $scope.spon.Id
        }).
            then((promise) => {
                if (promise.data.success === "true") {
                    tourID = promise.data.detail_type;
                    $scope.name = false;
                    $scope.teams = true;
                }
            });

    }
    

    //Teams configuration
    

    $scope.country = [];
    
    $http.get(Host + "/api/Country/countries").
        then((promise) => {
            let mydata = promise.data;
            $scope.country = mydata;

        });
    
    $scope.selectPlayer = [{ "Id": "1", "Name": "Germany" }, { "Id": "2", "Name": "China" }, { "Id": "3", "Name": "USA" }];
    $scope.country = $scope.selectPlayer;
    $scope.selectCountry = [];
    $scope.selectCountId = [];
    var temp = { "Germany" : ["1", "2"], "USA" : ["1", "2"], "USA" : ["1", "2"], "China": ["1", "2"] };
    $scope.showList = [];
    $scope.playerList = {};

    $scope.displayList = function () {
        $scope.showList = temp[$scope.searchProv];
    }

    $scope.addCountry = function () {
        if ($scope.selectCountry.includes($scope.prov.Name)) {

        }
        else {
            $scope.selectCountry.push($scope.prov.Name);
            $scope.selectCountId.push($scope.prov.Id);
            $scope.playerList[$scope.prov.Name] = [];
        }
    }
    

    $scope.addPlayer = function (person) {
        if ($scope.playerList[$scope.searchProv].includes(person)) {

        }
        else {
            $scope.playerList[$scope.searchProv].push(person);
        }
    }

    var xmatchId = [];

    $scope.goMatches = function () {

        $scope.sendList = [];
        var i;
        for (i = 0; i < $scope.selectCountry.length; i++) {
            var tempJson = { "country_id": $scope.selectCountId[i], "players": $scope.playerList[$scope.selectCountry[i]] };
            $scope.sendList.push(tempJson);

        }
        
        $http.post(Host + "/api/tournament/addcountry", {
            tournament_id: tourID,
            countries: $scope.sendList
        }).
            then((promise) => {
                if (promise.data.success === "true") {
                    $scope.teams = false;
                    $scope.matches = true;
                    xmatchId = promise.data.tournamentxcoutnry_id
                }
            });

    }


    //Matches configuration



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

    $scope.availableCountry = $scope.selectCountry;
    $scope.winnerList = []

    $scope.addFight = function () {

        if ($scope.selectCountry.includes($scope.team1)) {
            var it1 = 0;
            var it2 = 0;
            var check1 = "";
            var check2 = "";
            while (check1 != $scope.team1) {
                check1 = $scope.selectCountry[it1];
                it1 += 1;
            }
            while (check2 != $scope.team2) {
                check2 = $scope.selectCountry[it2];
                it2 += 1;
            }
            

            $http.post(Host + "/api/tournament/addmatch", {
                "tournament_id": tourID,
                "match_date": $scope.year.concat("-", $scope.month, "-", $scope.day),
                "match_location": $scope.location,
                "stage_id": "1",
                "txc_team1": $scope.selectCountId[it1],
                "txc_team2": $scope.selectCountId[it2],
                "sxm_winner1" : "",
                "sxm_winner2" : "",
                "match_number": matchNumb

            }).
                then((promise) => {
                    if (promise.data.success === "true") {
                        var xmatchId = promise.data.stagexmatch_id;
                        var xmatchName = promise.data.stagexmatxh_name;
                        $scope.winnerList.push({ "id": xmatchId, "name": xmatchName });
                        $scope.availableCountry.push(xmatchName);
                        $scope.showMatch.push("Match" + matchNumb.toString());
                        $scope.showMatch.push($scope.team1 + $scope.team2);
                        $scope.showMatch.push($scope.year.concat("-", $scope.month, "-", $scope.day));
                        $scope.showMatch.push($scope.location);
                        matchNumb += 1;
                    }
                });

            


        }
        else {
            var it1 = 0;
            var it2 = 0;
            var check1 = "";
            var check2 = "";
            while (check1 != $scope.team1) {
                check1 = $scope.availableCountry[it1];
                it1 += 1;
            }
            while (check2 != $scope.team2) {
                check2 = $scope.availableCountry[it2];
                it2 += 1;
            }

            $http.post(Host + "/api/tournament/addmatch", {
                "tournament_id": tourID,
                "match_date": $scope.year.concat("-", $scope.month, "-", $scope.day),
                "match_location": $scope.location,
                "stage_id": "1",
                "txc_team1": "",
                "txc_team2": "",
                "sxm_winner1": $scope.availableCountry[it1],
                "sxm_winner2": $scope.availableCountry[it2],
                "match_number": matchNumb

            }).
                then((promise) => {
                    if (promise.data.success === "true") {
                        var xmatchId = promise.data.stagexmatch_id;
                        var xmatchName = promise.data.stagexmatxh_name;
                        $scope.winnerList.push({ "id": xmatchId, "name": xmatchName });
                        $scope.availableCountry.push(xmatchName);
                        $scope.showMatch.push("Match" + matchNumb.toString());
                        $scope.showMatch.push($scope.team1 + $scope.team2);
                        $scope.showMatch.push($scope.year.concat("-", $scope.month, "-", $scope.day));
                        $scope.showMatch.push($scope.location);
                        matchNumb += 1;
                    }
                });
        }

        $scope.showMatch.push("Match" + matchNumb.toString());
        $scope.showMatch.push($scope.doneMatch);
        $scope.showMatch.push($scope.year.concat("-", $scope.month, "-", $scope.day));
        $scope.showMatch.push($scope.location);
        matchNumb += 1;
    }

    

    $scope.goStages = function () {
        $scope.matches = false;
        $scope.stages = true;

    }


    //Stages configuration
    $scope.addMatch = function () {
        tempvs = $scope.team1 + " vs " + $scope.team2;
        $scope.matchList.push(tempvs);
    }

});

login.controller("adminProfile", function ($scope, $rootScope, $location, $http) {

});