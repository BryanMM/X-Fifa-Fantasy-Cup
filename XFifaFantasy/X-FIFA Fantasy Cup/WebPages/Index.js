var UserName = "";
var UserInfo = "";
var Activity = "";
var UserType = "";
var TourSub = "";
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
        })
        .when("/userProfile", {
            templateUrl: "User/UserProfile.html",
            controller: "userProfile"
        })
        .when("/subscribe", {
            templateUrl: "User/SubTournament.html",
            controller: "subTournament"
        })
        .when("/predictions", {
            templateUrl: "User/PredictTournament.html",
            controller: "predictions"
        });
});

//Root controller///////////////////////////////////////////////////
login.controller("rootCont", function ($scope, $location) {
    $scope.goLogout = function () {
        UserName = "";
        UserType = "";
        UserInfo = "";
        Activity = "";
        $location.path("/");
    }
});

//Login verification controller///////////////////////////////////////
login.controller("userLogin", function ($scope, $rootScope, $location, $http) {
    $rootScope.showItem = false;
    $rootScope.showItemAdmin = false;
    $scope.goRegister = function () {
        $location.path("/register");
    }
    $scope.goLogin = function () {
        
       //$location.path("/userCalendar");
        
       $http.post(Host + "/api/user/login", { username: $scope.usr, password: $scope.pswrd }).
            then((promise) => {
                if (promise.data.success === "true") {
                    UserName = $scope.usr;
                    if (promise.data.detail_type === "1") { 
                        $location.path("/adminCalendar");
                        UserInfo = promise.data.detail_xinfo;
                        UserType = promise.data.detail_type;
                        Activity = promise.data.detail_status;
                    }
                    else {
                        $location.path("/userCalendar");
                        UserInfo = promise.data.detail_xinfo;
                        UserType = promise.data.detail_type;
                        Activity = promise.data.detail_status;
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

//Registration controller//////////////////////////////////
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
    for (i = 2018; i >= 1900; i--) {
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
        if ($scope.month === "02") {
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

//UserCalendar controller////////////////////////////////
login.controller("userCalendar", function ($scope, $rootScope, $location, $http) {
    $rootScope.showItem = true;

    $scope.tournament;
    $http.get(Host + "/api/tournament/gettournaments").
        then((promise) => {
            let mydata = promise.data;
            $scope.tournament = mydata;

        });

    $scope.subTournament = function (theId) {
        TourSub = theId.tournament_id;
        $location.path("/subscribe");
    }

});

//AdminCalendar Controller///////////////////////////////
login.controller("adminCalendar", function ($scope, $rootScope, $location, $http) {
    $rootScope.showItemAdmin = true;

    $scope.adminTournament;
    $http.get(Host + "/api/tournament/gettournaments").
        then((promise) => {
            let mydata = promise.data;
            $scope.adminTournament = mydata;

        });

    $scope.goTour = function () {
        $location.path("/createTournament");
    }
});

//Tournament creation controller////////////////////////////
login.controller("createTour", function ($scope, $rootScope, $location, $http) {
    $scope.name = true;
    $scope.teams = false;
    $scope.matches = false;
    $scope.stages = false;
    var tourID = "";

    //Names Configuration/////////////////////////////////////////////

    $http.get(Host + "/api/tournament/sponsor").
        then((promise) => {
            let mydata = promise.data;
            $scope.sponsor = mydata;

        });

    $scope.goTeams = function () {
        
        $http.post(Host + "/api/tournament/create", {
            tournament_name: $scope.tourName,
            sponsor_id: $scope.spon.sponsor_id
        }).
            then((promise) => {
                if (promise.data.success === "true") {
                    tourID = parseInt(promise.data.detail_type, 10);
                    $scope.name = false;
                    $scope.teams = true;
                }
            });

    }
    

    //Teams configuration/////////////////////////////////////////////
    

    $scope.country = [];
    
    $http.get(Host + "/api/Country/countries").
        then((promise) => {
            let mydata = promise.data;
            $scope.country = mydata;

        });
    /*
    $scope.selectPlayer = [{ "Id": "1", "Name": "Germany" }, { "Id": "2", "Name": "China" }, { "Id": "3", "Name": "USA" }];
    $scope.country = $scope.selectPlayer;*/
    $scope.selectCountry = [];
    $scope.selectCountId = [];
    $scope.avPlayers = {};
    $scope.showList = [];
    $scope.playerList = {};
    $scope.playerId = {};

    $scope.displayList = function () {
        
        var it1 = -1;
        var check1 = "";
        while (check1 != $scope.searchProv) {
            it1 += 1;
            check1 = $scope.selectCountry[it1];
            
        }
        $scope.showList = $scope.avPlayers[check1];
    }

    $scope.addCountry = function () {
        if ($scope.selectCountry.includes($scope.prov.Name)) {

        }
        else {
            $scope.selectCountry.push($scope.prov.Name);
            $scope.selectCountId.push($scope.prov.Id);
            $scope.playerList[$scope.prov.Name] = [];
            $scope.playerId[$scope.prov.Name] = [];

            $http.post(Host + "/api/country/players", {
                "country_id": $scope.prov.Id,
                "player_active": 1

            }).
                then((promise) => {
                    $scope.avPlayers[$scope.prov.Name] = promise.data;
                    
                });
        }
    }
    

    $scope.addPlayer = function (person) {
        if ($scope.playerList[$scope.searchProv].includes(person.player_name)) {

        }
        else {
            $scope.playerList[$scope.searchProv].push(person.player_name);
            $scope.playerId[$scope.searchProv].push(person.player_id);
        }
    }

    var xmatchId = [];

    $scope.goMatches = function () {

        $scope.sendList = [];
        var i;
        for (i = 0; i < $scope.selectCountry.length; i++) {
            var tempJson = { "country_id": $scope.selectCountId[i], "players": $scope.playerId[$scope.selectCountry[i]] };
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


    //Matches configuration//////////////////////////////////////////////



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

    $scope.daylist1.push("01", "02", "03", "04", "05", "06", "07", "08", "09");

    $scope.daylist2.push("01", "02", "03", "04", "05", "06", "07", "08", "09");

    $scope.daylist3.push("01", "02", "03", "04", "05", "06", "07", "08", "09");
    for (i = 10; i <= 28; i++) {
        $scope.daylist1.push(i);
    }
    for (i = 10; i <= 30; i++) {
        $scope.daylist2.push(i);
    }
    for (i = 10; i <= 31; i++) {
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
    var listLength = $scope.selectCountry.length;
    console.log(listLength);
    $scope.winnerList = [];

    $scope.addFight = function () {
        console.log($scope.country);
        var i;
        var flag = false;
        for (i = 0; i < $scope.country.length; i++) {
            if ($scope.country[i].Name === $scope.team1) {
                flag = true;
            }

        }

        if (flag) {
            var it1 = -1;
            var it2 = -1;
            var check1 = "";
            var check2 = "";
            while (check1 != $scope.team1) {
                it1 += 1;
                check1 = $scope.selectCountry[it1];
               
            }
            while (check2 != $scope.team2) {
                it2 += 1;
                check2 = $scope.selectCountry[it2];
               
            }
            

            $http.post(Host + "/api/tournament/addmatch", {
                "tournament_id": tourID,
                "match_date": $scope.year.concat("-", $scope.month, "-", $scope.day, " 5:5:5"),
                "match_location": $scope.location,
                "stage_id": "1",
                "txc_team_1": $scope.selectCountId[it1],
                "txc_team_2": $scope.selectCountId[it2],
                "sxm_winner1" : "",
                "sxm_winner2" : "",
                "match_number": matchNumb

            }).
                then((promise) => {
                    if (promise.data.Success === "true") {
                        var xmatchId = promise.data.stagexmatch_id;
                        var xmatchName = promise.data.stagexmatch_name;
                        $scope.winnerList.push({ "id": xmatchId, "name": xmatchName, "snum": 2 });
                        $scope.availableCountry.push(xmatchName);

                        $scope.showMatch.push("Match" + matchNumb.toString());
                        $scope.showMatch.push($scope.team1 + " VS " + $scope.team2);
                        $scope.showMatch.push($scope.year.concat("-", $scope.month, "-", $scope.day, " 5:5:5"));
                        $scope.showMatch.push($scope.location);
                        matchNumb += 1;
                    }
                });

            


        }
        else {
            
            console.log($scope.winnerList);
            var it1 = -1;
            var it2 = -1;
            var check1 = "";
            var check2 = "";
            while (check1 != $scope.team1) {
                it1 += 1;
                check1 = $scope.availableCountry[it1];
                
            }
            while (check2 != $scope.team2) {
                it2 += 1;
                check2 = $scope.availableCountry[it2];
                
            }
            console.log($scope.availableCountry);
            console.log(it1);
            console.log(listLength);
            var tempid = {};
            var tempid2 = {};
            tempid = $scope.winnerList[listLength - it1];
            tempid2 = $scope.winnerList[listLength - it2];

            $http.post(Host + "/api/tournament/addmatch", {
                "tournament_id": tourID,
                "match_date": $scope.year.concat("-", $scope.month, "-", $scope.day, " 5:5:5"),
                "match_location": $scope.location,
                "stage_id": tempid["snum"] + 1,
                "txc_team1": "",
                "txc_team2": "",
                "sxm_winner1": tempid["id"],
                "sxm_winner2": tempid2["id"],
                "match_number": matchNumb

            }).
                then((promise) => {
                    if (promise.data.Success === "true") {
                        var xmatchId = promise.data.stagexmatch_id;
                        var xmatchName = promise.data.stagexmatch_name;
                        $scope.winnerList.push({ "id": xmatchId, "name": xmatchName, "snum": tempid["snum"] + 1});
                        $scope.availableCountry.push(xmatchName);

                        $scope.showMatch.push("Match" + " VS " + matchNumb.toString());
                        $scope.showMatch.push($scope.team1 + $scope.team2);
                        $scope.showMatch.push($scope.year.concat("-", $scope.month, "-", $scope.day, " 5:5:5"));
                        $scope.showMatch.push($scope.location);
                        matchNumb += 1;
                    }
                });
        }

    }

    

    $scope.goStages = function () {
        $scope.matches = false;
        $scope.stages = true;

    }


    //Stages configuration//////////////////////////////////////////////
    $scope.addMatch = function () {
        tempvs = $scope.team1 + " vs " + $scope.team2;
        $scope.matchList.push(tempvs);
    }

});

//Admin profile controller//////////////////////////////////////////
login.controller("adminProfile", function ($scope, $rootScope, $location, $http) {
    $http.post(Host + "/api/user/getinfo", {
        "detail_xinfo": UserInfo,
        "detail_type": UserType

    }).
        then((promise) => {
            let mydata = promise.data;
            $scope.fname = mydata.fanatic_name;
            $scope.lname = mydata.fanatic_last_name;
            $scope.uemail = mydata.fanatic_email;
            $scope.usernameE = mydata.fanatic_login;
        });
});

//User profile controller////////////////////////////////////////////
login.controller("userProfile", function ($scope, $rootScope, $location, $http) {
    console.log(UserInfo);
    console.log(UserType);

    $http.post(Host + "/api/user/getinfo", {
        "detail_xinfo": UserInfo,
        "detail_type": UserType

    }).
        then((promise) => {
            let mydata = promise.data;
            $scope.fname = mydata.fanatic_name;
            $scope.lname = mydata.fanatic_last_name;
            $scope.ucountry = mydata.fanatic_country;
            $scope.uemail = mydata.fanatic_email;
            $scope.uphone = mydata.fanatic_phone;
            $scope.bday = mydata.fanatic_birthdate;
            $scope.usernameu = mydata.fanatic_login;
            $scope.userID = mydata.fanatic_id;
            $scope.regdate = mydata.fanatic_date_create;
            $scope.descript = mydata.fanatic_description;
        });
});

//Tournament Subscription Fanstasy controller//////////////////////////////////////
login.controller("subTournament", function ($scope, $rootScope, $location, $http) {
    $scope.playerSearch;

    $scope.tempC = [{ "Id": "1", "Name": "Germany" }, { "Id": "2", "Name": "China" }, { "Id": "3", "Name": "USA" }];
    $scope.tempP = [{ "player_id": "1", "player_name": "Cristiano Ronaldo", "player_country": "Germany", "position": "Forward", "statistics": null, "price": 30 }, { "player_id": "2", "player_name": "Keylor Navas", "player_country": "Costa Rica", "position": "Goaly", "statistics": null, "price": 20 }];
    

    $scope.selectCountry;
    $scope.budget = 100;

    $scope.selectMidfielder = [];
    $scope.selectGoalkeeper = [];
    $scope.selectForward = [];
    $scope.selectFullback = []

    ///////////////////////////////////////
    $http.get(Host + "/api/Country/countries").
        then((promise) => {
            let mydata = promise.data;
            $scope.selectCountry = mydata;

        });

    $scope.showList = [];

    $scope.displayList = function () {
        ////////////////////////////
        $http.post(Host + "/api/country/players", {
            "country_id": $scope.searchProv.Id,
            "player_active": 1

        }).
            then((promise) => {
                let mydata = promise.data;
                $scope.showList = mydata;
            });
    }

    $scope.selectedPlayer;
    $scope.listPlayer = [];

    $scope.addPlayer = function (thePlayer) {
        $scope.playerName = thePlayer.player_name;
        $scope.playerPrice = thePlayer.price;
        $scope.playerPosition = thePlayer.position;
        $scope.playerStatistics = thePlayer.statistics;
        $scope.playerCountry = $scope.searchProv.Name;
        $scope.selectedPlayer = thePlayer;

    }

    $scope.insertPlayer = function () {

        if (($scope.budget - parseFloat($scope.selectedPlayer.price)) >= 0) {
            $scope.listPlayer.push($scope.selectedPlayer.playerxinfo_id);
            $scope.budget -= parseFloat($scope.selectedPlayer.price);
            if ($scope.selectedPlayer.position === "Goalkeeper") {
                $scope.selectGoalkeeper.push($scope.selectedPlayer.player_name);
            }
            else if ($scope.selectedPlayer.position === "Midfielder") {
                $scope.selectMidfielder.push($scope.selectedPlayer.player_name);

            }
            else if ($scope.selectedPlayer.position === "Forward") {
                $scope.selectForward.push($scope.selectedPlayer.player_name);

            }
            else {
                $scope.selectFullback.push($scope.selectedPlayer.player_name);
            }
        }
        else {
            
        }
    }

    $scope.goPrediction = function () {
        //////////////////////////////// EDITAR URL
        $http.post(Host + "/api/country/players", {
            "userxinfo_id": UserInfo,
            "tournament_id": TourSub,
            "players": $scope.listPlayer

        }).
            then((promise) => {
                if (promise.data.success === "true") {

                    $location.path("/predictions");
                }
            });
    }
});

//Tournament Subscription Championship controller//////////////////////////////////////
login.controller("predictions", function ($scope, $rootScope, $location, $http) {
    //$scope.temp = [{ "team1": "Germany", "team2": "England", "id1": "1", "id2": "2", "IDM": "1" }, { "team1": "Costa Rica", "team2": "Belguim", "id1": "3", "id2": "4", "IDM": "2" }];
    //$scope.temp2 = [{ "team1": "Italy", "team2": "USA", "id1": "1", "id2": "2", "IDM": "1" }, { "team1": "Panama", "team2": "China", "id1": "3", "id2": "4", "IDM": "2" }];

    $scope.currentStage = 1;
    ///////////////////
    $http.post(Host + "/api/tournament/getstage", {
        "tournament_id": TourSub,
        "stage_id": 1

    }).
        then((promise) => {
            let mydata = promise.data;
            $scope.matchList = mydata;
        });

    $scope.resultList = [];
    $scope.iteratorM = 0;

    $scope.matchList = $scope.temp;
    $scope.result1 = 0;
    $scope.result2 = 0;
    $scope.player1 = $scope.matchList[0].name_team_1;
    $scope.player2 = $scope.matchList[0].name_team_2;

    

    $scope.subData = function () {
        
        if ($scope.iteratorM < ($scope.matchList.length - 1)) {
            var currentJson = $scope.matchList[$scope.iteratorM];
            var winName, winId;
            if ($scope.result1 >= $scope.result2) {
                winName = currentJson.name_team_1;
                winId = currentJson.team_1;

            }
            else {
                winName = currentJson.name_team_2;
                winId = currentJson.team_2;
            }
            var tempJson = { "match_id": currentJson.match_id, "txc_team1": currentJson.team_1, "txc_team2": currentJson.team_2, "userxscore_score1": $scope.result1, "userxscore_score2": $scope.result2, "winner_name": winName, "winner_id": winId };
            $scope.resultList.push(tempJson);
            $scope.iteratorM += 1;
            $scope.result1 = 0;
            $scope.result2 = 0;
            $scope.player1 = $scope.matchList[$scope.iteratorM].team1;
            $scope.player2 = $scope.matchList[$scope.iteratorM].team2;
        }
        else {
            var currentJson = $scope.matchList[$scope.iteratorM];
            if ($scope.result1 >= $scope.result2) {
                winName = currentJson.name_team_1;
                winId = currentJson.team_1;

            }
            else {
                winName = currentJson.name_team_2;
                winId = currentJson.team_2;
            }
            var tempJson = { "match_id": currentJson.match_id, "txc_team1": currentJson.team_1, "txc_team2": currentJson.team_2, "userxscore_score1": $scope.result1, "userxscore_score2": $scope.result2, "winner_name": winName, "winner_id": winId };
            $scope.resultList.push(tempJson);
            ////////////////////
            $http.post(Host + "/api/user/sendpredictions", {
                "userxinfo_id": UserInfo,
                "matches": $scope.resultList

            }).
                then((promise) => {
                });

            if ($scope.matchList.length === 1){
                $location.path("/userCalendar");
            }
            else { 
                $scope.currentStage += 1;
                //////////////////
                $http.post(Host + "/api/tournament/getstage", {
                    "tournament_id": TourSub,
                    "stage_id": $scope.currentStage

                }).
                    then((promise) => {
                        let mydata = promise.data;
                        $scope.matchList = mydata;
                    });

                $scope.result1 = 0;
                $scope.result2 = 0;
                $scope.iteratorM = 0;
                $scope.player1 = $scope.matchList[$scope.iteratorM].team1;
                $scope.player2 = $scope.matchList[$scope.iteratorM].team2;
            }
        }

    }
});