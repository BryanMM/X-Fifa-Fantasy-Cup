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
	fanatic_birth		varchar(255) NOT NULL,
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
	player_birth varchar(255) NOT NULL,
	player_height float NOT NULL,
	player_weight float NOT NULL,
	player_team VARCHAR(30) NOT NULL,
	player_price float NOT NULL,
	player_active BIT NOT NULL,
	player_photo varchar(max) NOT NULL,
	player_grade int default(0)
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
	live_start varchar(255) NOT NULL,
	live_end varchar(255)
);

CREATE TABLE admin(
	admin_id int identity(1,1) not null,
	admin_username varchar(8) primary key not null,
	admin_name varchar(30) not null,
	admin_last_name varchar(30) not null,
	admin_email varchar(255) not null,
	admin_date_create DATETIME not null,
	admin_password varbinary(max) not null,
	admin_active int default(1)
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
	fanatic_login VARCHAR(8) FOREIGN KEY REFERENCES fanatic(fanatic_login)
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
	sponsor_id int foreign key references sponsor(sponsor_id),
	tournament_available int default(0)
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
	tournament_id int foreign key references tournament(tournament_id),
	userxfantasy_champ_note int default(0),
	userxfantasy_fantasy_note int default(0)
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
	match_date varchar(255) NOT NULL,
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
exec @result =  dbo.insertfanatic @f_login='pedro',@f_name='juan',@f_last_name='tacos',@f_email='tacos@tacos.com',@f_phone=8288, @f_birth='1-1-1995' ,@f_password='juantaco',@f_country=1;
exec @result = dbo.insertadmin @a_username='admin',@a_name='pedro',@a_last_name='perez',@a_email='security@tacos.com',@a_password='admin';
print @result;

insert into powerup_type(powerup_type_name) values('Multiplicador'),('Sumador');

insert into sponsor(sponsor_name,sponsor_status,sponsor_photo) values('Coca Cola',1,'C:/Program Files...');
insert into tournament(tournament_name,sponsor_id) values('Rusia 2018',1);
insert into tournamentxcountry(tournament_id,country_id) values(1,1),(1,2),(1,3);

exec insertadminmatch @match_date='1-1-2018 9:00',@match_location='Costa Rica',@stage_id=1,@txc_team_1=1,@txc_team_2=2,@tournament_id=1;
exec insertadminmatch @match_date='2-1-2018 9:00',@match_location='Costa Rica',@stage_id=1,@txc_team_1=1,@txc_team_2=3,@tournament_id=1;
exec insertadminmatch @match_date='3-1-2018 9:00',@match_location='Costa Rica',@stage_id=2,@txc_team_1=1,@txc_team_2=3,@tournament_id=1;
exec insertadminmatch @match_date='4-1-2018 9:00',@match_location='Costa Rica',@stage_id=2,@txc_team_1=3,@txc_team_2=2,@tournament_id=1;

insert into player values('1234','Juan' ,'Núñez','1-1-1980',180,85,'Pollitos FC',10,1,'C:/program files',0),('3456','Carlos' ,'Núñez','1-1-1975',180,85,'Pollitos FC',10,1,'C:/program files',0);
insert into playerxposition values(1,'Goalkeeper'),(2,'Fullback'),(3,'Midfielder'),(4,'Forward');
insert into playerxinfo(country_id,playerxposition_id,player_id) values(1,2,'1234'),(1,3,'3456');
insert into eventmatch(action_name,action_value) values('Goal goalkeeper',8),
														('Goal Fullback',6),
														('Goal Midfielder',4),
														('Goal Forward',3),
														('Assistance Goalkeeper',8),
														('Assistance Fullback',6),
														('Assistance Midfielder',4),
														('Assistance Forward',3),
														('Match Headline',2),
														('Match In-Change',1),
														('Match Out-Change',-1),
														('Yellow Card',-1),
														('Expulsion',-2),
														('Penalty Stop',2),
														('Goalkeeper without goals',2),
														('Fullback wihtout goals',1);
insert into userxfantasy(userxinfo_id,tournament_id,playerxinfo_id) values(1,1,1),(1,1,2);
declare @livedate DATE;
set @livedate = GETDATE();
exec createlive @start=@livedate,@match_id=3;
insert into livexaction(action_id,live_id,playerxinfo_id) values(4,1,1),(2,1,2);

declare @res int;
exec @res = createtournament @tournament_name='Qatar 2018',@sponsor_id=1
print @res;