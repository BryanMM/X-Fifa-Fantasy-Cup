 create database xfifafantasycup;
 go;
 use xfifafantasycup;
 go;
 CREATE TABLE fanatic(
	fanatic_login		varchar(8) PRIMARY KEY NOT NULL,
	fanatic_id			INT IDENTITY(1,1) NOT NULL,
	fanatic_name		varchar(30) NOT NULL, 
	fanatic_last_name	varchar(30) NOT NULL,
	fanatic_email		varchar(255) NOT NULL,
	fanatic_phone		int NOT NULL,
	fanatic_birth		DATE NOT NULL,
	fanatic_date_create	DATETIME NOT NULL,
	fanatic_password	varbinary(max) NOT NULL,
	fanatic_active		BIT NOT NULL DEFAULT (1),
	fanatic_photo		varchar(max),
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
	player_id varchar(8) PRIMARY KEY NOT NULL,
	player_name VARCHAR(30) NOT NULL,
	player_last_name VARCHAR(30) NOT NULL,
	player_birth DATE NOT NULL,
	player_height INT NOT NULL,
	player_weight INT NOT NULL,
	player_team VARCHAR(30) NOT NULL,
	player_price INT NOT NULL,
	player_active BIT NOT NULL,
	player_photo varchar(max) NOT NULL
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
	live_end DATETIME
);

CREATE TABLE admin(
	admin_id int identity(1,1) not null,
	admin_username varchar(8) primary key not null,
	admin_name varchar(30) not null,
	admin_last_name varchar(30) not null,
	admin_email varchar(255) not null,
	admin_date_create DATETIME not null,
	admin_password varbinary(max) not null
);

CREATE TABLE powerup_type(
	powerup_type_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	powerup_type_name varchar(30) NOT NULL
);

CREATE TABLE sponsor(
	sponsor_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	sponsor_name VARCHAR(30) NOT NULL,
	sponsor_status BIT NOT NULL,
	sponsor_photo varchar(max) NOT NULL
);

CREATE TABLE userxinfo(
	userxinfo_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	user_type_id INT FOREIGN KEY REFERENCES user_type(user_type_id),
	country_id INT FOREIGN KEY REFERENCES country(country_id),
	fanatic_login VARCHAR(8) FOREIGN KEY REFERENCES fanatic(fanatic_login),
	uxi_champ_note int default(0),
	uxi_fantasy_note int default(0)
);

CREATE TABLE playerxinfo(
	playerxinfo_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	country_id INT FOREIGN KEY REFERENCES country(country_id),
	playerxposition_id INT FOREIGN KEY REFERENCES playerxposition(playerxposition_id),
	player_id varchar(8) FOREIGN KEY REFERENCES player(player_id)
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

CREATE TABLE match(
	match_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	match_date DATETIME NOT NULL,
	match_location varchar(255) NOT NULL,
	match_enable BIT NOT NULL DEFAULT(1),
	match_score_1 int default(0),
	match_score_2 int default(0),
	txc_team_1 int foreign key references tournamentxcountry(tournamentxcountry_id),
	txc_team_2 int foreign key references tournamentxcountry(tournamentxcountry_id)
);


create table tournamentxplayer(
	tournamentxplayer_id int identity(1,1) primary key not null,
	tournamentxcountry_id int foreign key references tournamentxcountry(tournamentxcountry_id),
	player_id varchar(8) foreign key references player(player_id),
);


create table livexcomment(
	livexcomment_id int identity(1,1) primary key not null,
	livexcomment_comment varchar(MAX),
	live_id int foreign key references live(live_id),
	powerup_id int foreign key references powerup(powerup_id)
);

create table livexmatch(
	livexmatch_id int identity(1,1) primary key not null,
	match_id int foreign key references match(match_id),
	live_id int foreign key references live(live_id)
);

create table userxlive(
	userxlive_id int identity(1,1) primary key not null,
	live_id int foreign key references live(live_id),
	userxinfo_id int foreign key references userxinfo(userxinfo_id),
	powerup_id int foreign key references powerup(powerup_id)
);

create table adminxinfo(
	adminxinfo_id int identity(1,1) primary key not null,
	admin_username varchar(8) foreign key references admin(admin_username),
	user_type_id int foreign key references user_type(user_type_id)
);

create table eventmatch(
	action_id int identity(1,1) primary key not null,
	action_name varchar(255) not null,
	action_value int not null
);

create table livexaction(
	livexaction_id int identity(1,1) primary key not null,
	action_id int foreign key references eventmatch(action_id),
	live_id int foreign key references live(live_id),
	playerxinfo_id int foreign key references playerxinfo(playerxinfo_id)
);

CREATE TABLE userxscore(
	userxscore_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	userxscore_score1 INT DEFAULT (0),
	userxscore_score2 INT DEFAULT (0),
	userxinfo_id INT FOREIGN KEY REFERENCES userxinfo(userxinfo_id),
	match_id int foreign key references match(match_id),
	txc_team1 int foreign key references tournamentxcountry(tournamentxcountry_id),
	txc_team2 int foreign key references tournamentxcountry(tournamentxcountry_id)
);

create table tournamentxstage(
	txs_id int identity(1,1) primary key not null,
	stage_id int,
	tournament_id int foreign key references tournament(tournament_id)
);

create table stagexmatch(
	sxm_id int identity(1,1) primary key not null,
	winner_1 int,
	winner_2 int,
	match_id int foreign key references match(match_id),
	txs_id  int foreign key references tournamentxstage(txs_id)
);

insert into user_type values (1,'Administrator'),(2,'Fanatic');
insert into country values(1,'China'),(2,'Korea'),(3,'Vietnam');
create master key encryption by password = 'juan1234'
declare @result int;
declare @date DATE;
declare @datet DATETIME;
select @date = CONVERT(date,GETDATE());
exec @result =  dbo.insertfanatic @f_login='pedro',@f_name='juan',@f_last_name='tacos',@f_email='tacos@tacos.com',@f_phone=8288, @f_birth=@date ,@f_password='juantaco',@f_country=1;
declare @result int;
exec @result = dbo.insertadmin @a_username='admin',@a_name='pedro',@a_last_name='perez',@a_email='security@tacos.com',@a_password='admin';
print @result;

insert into powerup_type(powerup_type_name) values('Multiplicador'),('Sumador');

alter table live drop column live_end;
alter table live add live_end DATETIME;

insert into sponsor(sponsor_name,sponsor_status,sponsor_photo) values('Coca Cola',1,'C:/Program Files...');
insert into tournament(tournament_name,sponsor_id) values('Rusia 2018',1);
insert into tournamentxcountry(tournament_id,country_id) values(1,1),(1,2),(1,3);
insert into stage(stage_name) values('Stage B');
insert into tournamentxstage(stage_id,tournament_id) values(2,1);
declare @date datetime;
set @date = GETDATE()
exec insertadminmatch @match_date=@date,@match_location='Costa Rica',@txs_id=3,@txc_team_1=2,@txc_team_2=3

