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

create procedure checkuser
	@user_username varchar(8),
	@user_password varchar(8)
as begin
	set nocount on;
	declare @result int;
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
				set @result = 1;
			end else if @passworduser = @user_password
			begin
				set @result = 2;
			end else begin
				set @result = -3;
			end
		end
	else begin
		set @result = -2;
		end
	return @result;
end