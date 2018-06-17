use xfifafantasycup
go
create procedure insertfanatic
	@f_login		VARCHAR(8),
	@f_name			VARCHAR(30),
	@f_last_name	VARCHAR(30),
	@f_email		VARCHAR(255),
	@f_phone		INT,
	@f_birth		DATE,
	@f_password		VARCHAR(8),
	@f_active		BIT = 1,
	@f_photo		IMAGE = NULL,
	@f_about		VARCHAR(300) = '',
	@f_country		INT
as begin
	set nocount on;
	declare @variable int;
	declare @password varbinary(max);
	declare @f_date_create_utc DATETIME;
	declare @f_date_create DATETIME;
	declare @inst varchar(max);
	set @f_date_create_utc = GETDATE();
	set @f_date_create = dateadd(hour,-8,@f_date_create_utc);
	set @inst = 'create asymmetric key ' +@f_login +' with algorithm = RSA_2048;';
	begin try
		EXEC (@inst);
		set @variable = 1;
		set @inst = @f_login;
		set @password = ENCRYPTBYASYMKEY(ASYMKEY_ID(@inst),@f_password);
		insert into fanatic(fanatic_login,fanatic_name,fanatic_last_name,fanatic_email,fanatic_phone,fanatic_birth,fanatic_date_create,fanatic_password,fanatic_active,fanatic_photo,fanatic_description) values(@f_login,@f_name,@f_last_name,@f_email,@f_phone,@f_birth,@f_date_create,@password,@f_active,@f_photo,@f_about);
		insert into userxinfo(user_type_id,country_id,fanatic_login) values(2,@f_country,@f_login);
	end try
	begin catch
		SET @variable =-1;
	end catch;
	return @variable;
end;
Go;
create procedure insertadmin
	@a_username varchar(8),
	@a_name varchar(30),
	@a_last_name varchar(30),
	@a_email varchar(255),
	@a_password varchar(8)
as begin
	declare @variable int;
	declare @f_date_create_utc DATETIME;
	declare @f_date_create DATETIME;
	declare @inst varchar(max);
	declare @password varbinary(max);
	
	set @f_date_create_utc = GETDATE();
	set @f_date_create = dateadd(hour,-8,@f_date_create_utc);
	set @inst = 'create asymmetric key ' +@a_username +' with algorithm = RSA_2048;';
	begin try
		EXEC (@inst);
		set @inst = @a_username;
		set @password = ENCRYPTBYASYMKEY(ASYMKEY_ID(@inst),@a_password);
		insert into admin(admin_username,admin_name,admin_last_name,admin_email,admin_date_create,admin_password) values(@a_username,@a_name,@a_last_name,@a_email,@f_date_create,@password);
		insert into adminxinfo(admin_username,user_type_id) values(@a_username,1);
		set @variable = 1;
	end try
	begin catch
		SET @variable =-1;
	end catch;
	return @variable;
end;
go;
create procedure insertplayer
	@p_passport int,
	@p_name varchar(30),
	@p_lastname varchar(30),
	@p_birthdate date,
	@p_height int,
	@p_weight int,
	@p_team	 varchar(30),
	@p_price int,
	@p_active bit,
	@p_photo image,
	@p_position int,
	@p_country int
as begin
	declare @variable int;
	begin try
		insert into player values(@p_passport, @p_name,@p_lastname,@p_birthdate,@p_height,@p_weight,@p_team,@p_price,@p_active,@p_photo);
		insert into playerxinfo(country_id,playerxposition_id,player_id) values(@p_country,@p_position,@p_passport);
		set @variable = 1
	end try
	begin catch
		set @variable = -4;
	end catch
	return @variable;
end;
go;

-- New version of the procedure, it retrieves a table because the FK of userxinfo_id is needed, not just the type.
create procedure checkuser
	@user_username varchar(8),
	@user_password varchar(8)
as begin
	set nocount on;
	declare @result int;
	--declare @userinfo table (usertype int,userid int);
	declare @admin_pass varchar(8);
	declare @fanatic_pass varchar(8);
	declare @passwordadmin varchar(8);
	declare @passworduser varchar(8);
	declare @passadmin_encrypt varbinary(max);
	declare @passuser_encrypt varbinary(max);
	set @result = 0;
	if (select ASYMKEY_ID(@user_username))>=0
		begin
			select @passadmin_encrypt = admin_password from admin where admin_username = @user_username;
			select @passuser_encrypt = fanatic_password from fanatic where fanatic_login = @user_username;
			set @passwordadmin = CONVERT(varchar(8),DECRYPTBYASYMKEY(ASYMKEY_ID(@user_username),@passadmin_encrypt),0);
			set @passworduser = CONVERT(varchar(8),DECRYPTBYASYMKEY(ASYMKEY_ID(@user_username),@passuser_encrypt),0);
			print @passwordadmin;
			print @passworduser;
			if (@passwordadmin = @user_password)
			begin
				select axi.user_type_id,axi.adminxinfo_id from admin as ad left outer join adminxinfo as axi on (ad.admin_username = axi.admin_username) where ad.admin_username = @user_username;
			end else if @passworduser = @user_password
			begin
				select distinct uxi.user_type_id,uxi.userxinfo_id,fan.fanatic_active from fanatic as fan left outer join userxinfo as uxi on (fan.fanatic_login = uxi.fanatic_login) where fan.fanatic_login = @user_username;
			end else begin
				set @result = -3;
			end
		end
	else begin
		set @result = -2;
		end
end

/**********************************************************************************

******   New procedures added, remember to use: use xfifafantasycup; **************

***********************************************************************************/

-- Returns the id for the tournament just created, it is needed because
-- when you are creating the tournament, you will need that id to be able
-- to do the rest of the inserts, it is not suitable to use random numbers
-- to create that id, but it can be done.
-- This stored procedure is applied in the early stage of configuring a tournament.
create procedure createtournament
	@tournament_name varchar(255),
	@sponsor_id int
as begin
	set nocount on;
	declare @return int;
	declare @id int;
	begin try
		insert into tournament(tournament_name,sponsor_id) values(@tournament_name,@sponsor_id);
		set @return = @@IDENTITY;
	end try
	begin catch
		set @return = -1;
	end catch
	return @return;
end;

-- It behaves in the same way as createtournament.
-- Returns the recent id created necessary to add
-- a player to a country in certain tournament.
create procedure inserttourxcountry
	@tournament_id int,
	@country_id int
as begin
	set nocount on;
	declare @return int;
	begin try
		insert into tournamentxcountry(tournament_id,country_id) values(@tournament_id,@country_id);
		set @return = @@IDENTITY;
	end try
	begin catch
		set @return = -1;
	end catch
	return @return;
end

-- It assigns a certain player to a certain tournament
-- It depends on the country to be selected to the 
-- tournament to be able to assign it.
create procedure inserttourplayer
	@tourxcountry_id int,
	@player_id int
as begin
	set nocount on;
	declare @return int;
	begin try
		insert into tournamentxplayer(tournamentxcountry_id,player_id) values(@tourxcountry_id,@player_id);
		set @return = 1;
	end try
	begin catch
		set @return =-1;
	end catch
	return @return;
end

-- It creates a powerup, the type as int is needed.
create procedure createpowerup
	@pu_name varchar(30),
	@pu_unique bit,
	@pu_selection bit,
	@pu_sponsor int,
	@pu_type int
as begin
	declare @return int;
	begin try
		insert into powerup(powerup_name,powerup_unique,powerup_selection,sponsor_id,powerup_type_id) values(@pu_name,@pu_unique,@pu_selection,@pu_sponsor,@pu_type);
		set @return = 1;
	end try
	begin catch
		set @return = -1
	end catch
	return @return;
end;

-- It creates a new live, it sets the moment it starts and returns
-- the id to be used in later actions.
create procedure createlive
	@start DATETIME
as begin
	declare @return int;
	begin try
		insert into live(live_start) values(@start);
		set @return = @@IDENTITY;
	end try
	begin catch
		set @return = -1;
	end catch
	return @return;
end

-- It creates a new event, it will be used to link to each fanatic that clicks on it.
-- Returns its ID so it can be used to identify when a fanatic catches it.
create procedure insertevent
	@comment varchar(max) = '',
	@live_id int,
	@match_id int,
	@pu_id int = null
as begin
	set nocount on;
	declare @return int;
	begin try
		insert into livexmatch(livexmatch_comment,live_id,match_id,powerup_id) values(@comment,@live_id,@match_id,@pu_id);
		set @return = @@IDENTITY;
	end try
	begin catch
		set @return = -1;
	end catch
	return @return;
end
use xfifafantasycup;
go
-- Assigns to a certain fanatic a powerup.
create procedure insertuserxlive
	@live_id int,
	@userxinfo_id int,
	@powerup_id int
as begin
	set nocount on;
	declare @return int;
	begin try
		insert into userxlive(userxinfo_id,live_id,powerup_id) values(@userxinfo_id,@live_id,@powerup_id);
		set @return =1;
	end try
	begin catch
		set @return = -1;
	end catch
	return @return;
end


-- Creates a new stage, it adds a name to it. 
-- Returns its ID to be used later.
create procedure insertstage
	@stage_name varchar(255) = '',
	@tournament_id int
as begin
	set nocount on;
	declare @return int;
	begin try
		insert into stage(stage_name) values(@stage_name);
		set @return = @@IDENTITY;
		insert into tournamentxstage(stage_id, tournament_id) values(@return,@tournament_id);
		set @return = @@IDENTITY;
	end try
	begin catch
		set @return = -1;
	end catch
	return @return;
end

create procedure insertadminmatch
	@match_date DATETIME,
	@match_location varchar(255),
	@txs_id int,
	@txc_team_1 int = null,
	@txc_team_2 int = null,
	@sxm_winner1 int = null,
	@sxm_winner2 int = null,
	@match_enable bit = 1
as begin
	set nocount on;
	declare @return int;
	begin try
		insert into match(match_date,match_location,match_enable,txc_team_1,txc_team_2) values(@match_date,@match_location,@match_enable,@txc_team_1,@txc_team_2);
		set @return = @@IDENTITY;
		insert into stagexmatch(winner_1,winner_2,match_id,txs_id) values(@sxm_winner1,@sxm_winner2,@return,@txs_id);
	end try
	begin catch
		set @return = -1;
	end catch
	return @return;
end
-- livexaction no es necesario de hacerle un store procedure porque en el web page ya se tiene esa información.