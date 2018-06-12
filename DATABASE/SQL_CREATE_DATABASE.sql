 CREATE TABLE fanatic(
	fanatic_login		varchar(8) PRIMARY KEY NOT NULL,
	fanatic_id			INT IDENTITY(1,1) NOT NULL,
	fanatic_name		varchar(30) NOT NULL, 
	fanatic_last_name	varchar(30) NOT NULL,
	fanatic_email		varchar(255) NOT NULL,
	fanatic_phone		int NOT NULL,
	fanatic_birth		DATE NOT NULL,
	fanatic_date_create	DATETIME NOT NULL,
	fanatic_password	varchar(344) NOT NULL,
	fanatic_private_key	varchar(1616) NOT NULL,
	fanatic_active		BIT NOT NULL DEFAULT (1),
	fanatic_photo		IMAGE,
	fanatic_description	varchar(300)
);

CREATE TABLE user_type(
	user_type_id INT PRIMARY KEY NOT NULL,
	user_type_name VARCHAR(255) NOT NULL
);

CREATE TABLE country(
	country_id INT PRIMARY KEY NOT NULL,
	country_name VARCHAR(255) NOT NULL,
);

CREATE TABLE player(
	player_id INT PRIMARY KEY NOT NULL,
	player_name VARCHAR(30) NOT NULL,
	player_last_name VARCHAR(30) NOT NULL,
	player_birth DATE NOT NULL,
	player_height INT NOT NULL,
	player_weight INT NOT NULL,
	player_team VARCHAR(30) NOT NULL,
	player_price INT NOT NULL,
	player_active BIT NOT NULL,
	player_photo IMAGE NOT NULL
);

CREATE TABLE playerxposition(
	playerxposition_id INT PRIMARY KEY NOT NULL,
	playerxposition_name VARCHAR(30) NOT NULL,
);

CREATE TABLE stats(
	stats_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	stats_played INT NOT NULL DEFAULT(0),
	stats_winner INT NOT NULL DEFAULT(0),
	stats_tie INT NOT NULL DEFAULT(0),
	stats_lost INT NOT NULL DEFAULT(0),
	stats_minutes INT NOT NULL DEFAULT(0),
	stats_goals INT NOT NULL DEFAULT(0),
	stats_tiro_marco INT NOT NULL DEFAULT(0),
	stats_assistance INT NOT NULL DEFAULT(0),
	stats_ball_back INT NOT NULL DEFAULT(0),
	stats_yellow_card INT NOT NULL DEFAULT(0),
	stats_expulsion INT NOT NULL DEFAULT(0),
	stats_penalty_stop INT NOT NULL DEFAULT(0),
	stats_penalty_cause INT NOT NULL DEFAULT(0),
	stats_remate_save INT NOT NULL DEFAULT(0)
);

CREATE TABLE live(
	live_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	live_start DATETIME NOT NULL,
	live_end DATETIME NOT NULL
);

CREATE TABLE admin(
	admin_id int identity(1,1) not null,
	admin_username varchar(8) primary key not null,
	admin_name varchar(30) not null,
	admin_last_name varchar(30) not null,
	admin_email varchar(255) not null,
	admin_date_create DATETIME not null,
	admin_password varchar(344) not null,
	admin_private_key varchar(1616) not null
);

CREATE TABLE grouptournament(
	grouptournament_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	grouptournament_winner INT NOT NULL
);

CREATE TABLE match(
	match_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	match_date DATETIME NOT NULL,
	match_location varchar(255) NOT NULL,
	match_enable BIT NOT NULL DEFAULT(1)
);

CREATE TABLE powerup_type(
	powerup_type_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	powerup_type_name varchar(30) NOT NULL
);

CREATE TABLE sponsor(
	sponsor_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	sponsor_name VARCHAR(30) NOT NULL,
	sponsor_status BIT NOT NULL,
	sponsor_photo IMAGE NOT NULL
);

CREATE TABLE userxinfo(
	userxinfo_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	user_type_id INT FOREIGN KEY REFERENCES user_type(user_type_id),
	country_id INT FOREIGN KEY REFERENCES country(country_id),
	fanatic_login VARCHAR(8) FOREIGN KEY REFERENCES fanatic(fanatic_login)
);

CREATE TABLE userxscore(
	userxscore_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	userxscore_score1 INT DEFAULT (0),
	userxscore_score2 INT DEFAULT (0),
	userxinfo_id INT FOREIGN KEY REFERENCES userxinfo(userxinfo_id)
);

CREATE TABLE playerxinfo(
	playerxinfo_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	country_id INT FOREIGN KEY REFERENCES country(country_id),
	playerxposition_id INT FOREIGN KEY REFERENCES playerxposition(playerxposition_id),
	player_id INT FOREIGN KEY REFERENCES player(player_id)
);

create table tournament(
	tournament_id int identity(1,1) primary key not null,
	tournament_name varchar(255) not null,
	sponsor_id int foreign key references sponsor(sponsor_id)
);

create table playerxstats(
	playerxstats_id int identity(1,1) primary key not null,
	tournament_id int foreign key references tournament(tournament_id),
	playerxinfo_id int foreign key references playerxinfo(playerxinfo_id),
	stats_id  int foreign key references stats(stats_id)
);

create table userxfantasy(
	userxfantasy_id int identity(1,1) primary key not null,
	userxinfo_id int foreign key references userxinfo(userxinfo_id),
	playerxinfo_id int foreign key references playerxinfo(playerxinfo_id),
	tournament_id int foreign key references tournament(tournament_id)
);

create table powerup(
	powerup_id int identity(1,1) primary key not null,
	powerup_name varchar(30) not null,
	powerup_unique bit not null,
	powerup_selection bit not null,
	sponsor_id int foreign key references sponsor(sponsor_id),
	powerup_type_id int foreign key references powerup_type(powerup_type_id)
);

create table tournamentxcountry(
	tournamentxcountry_id int identity(1,1) primary key not null,
	tournament_id int foreign key references tournament(tournament_id),
	country_id int foreign key references country(country_id)
);

create table tournamentxgroup(
	tournamentxgroup_id int identity(1,1) primary key not null,
	grouptournament_id int foreign key references grouptournament(grouptournament_id),
	tournament_id int foreign key references tournament(tournament_id),
	country_id int foreign key references country(country_id)
);

create table tournamentxplayer(
	tournamentxplayer_id int identity(1,1) primary key not null,
	tournamentxcountry_id int foreign key references tournamentxcountry(tournamentxcountry_id),
	player_id int foreign key references player(player_id),
);

create table tournamentxmatch(
	tournamentxmatch_id int identity(1,1) primary key not null,
	tournamentxmatch_team1 varchar(255),
	tournamentxmatch_team2 varchar(255),
	tournamentxcountry_id1 int foreign key references tournamentxcountry(tournamentxcountry_id),
	tournamentxcountry_id2 int foreign key references tournamentxcountry(tournamentxcountry_id)
);

create table matchxscore(
	matchxscore_id int identity(1,1) primary key not null,
	matchxscore_score1 int default (0),
	matchxscore_score2 int default (0),
	tournamentxmatch_id int foreign key references tournamentxmatch(tournamentxmatch_id),
	match_id int foreign key references match(match_id)
);

create table livexmatch(
	livexmatch_id int identity(1,1) primary key not null,
	livexmatch_comment varchar(MAX),
	live_id int foreign key references live(live_id),
	match_id int foreign key references match(match_id),
	powerup_id int foreign key references powerup(powerup_id)
);

create table userxlive(
	userxlive_id int identity(1,1) primary key not null,
	livexmatch_id int foreign key references livexmatch(livexmatch_id),
	userxinfo_id int foreign key references userxinfo(userxinfo_id)
);

create table adminxinfo(
	adminxinfo_id int identity(1,1) primary key not null,
	admin_username varchar(8) foreign key references admin(admin_username),
	user_type_id int foreign key references user_type(user_type_id)
);

alter table userxfantasy drop constraint FK__userxfant__playe__72C60C4A;
alter table userxfantasy drop column playerxstats_id;
alter table userxfantasy add playerxinfo_id int foreign key references playerxinfo(playerxinfo_id);

insert into user_type values (1,'Administrator'),(2,'Fanatic');