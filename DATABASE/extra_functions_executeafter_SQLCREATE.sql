use xfifafantasycup
go
alter procedure insertfanatic
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
	print @f_date_create;
	set @inst = 'create asymmetric key f_' +@f_login +' with algorithm = RSA_2048;';
	begin try
		EXEC (@inst);
	end try
	begin catch
		SET @variable =-1;
	end catch;
	set @inst = 'f_'+@f_login;
	set @password = ENCRYPTBYASYMKEY(ASYMKEY_ID(@inst),@f_password);
	insert into fanatic(fanatic_login,fanatic_name,fanatic_last_name,fanatic_email,fanatic_phone,fanatic_birth,fanatic_date_create,fanatic_password,fanatic_active,fanatic_photo,fanatic_description) values(@f_login,@f_name,@f_last_name,@f_email,@f_phone,@f_birth,@f_date_create,@password,@f_active,@f_photo,@f_about);
	insert into userxinfo(user_type_id,country_id,fanatic_login) values(2,@f_country,@f_login)
end;
Go;
create or alter function insertadmin(
	@a_username varchar(8),
	@a_name varchar(30),
	@a_last_name varchar(30),
	@a_email varchar(255),
	@a_password varchar(344),
	@a_privatekey varchar(1616)
)
returns int
as begin
	declare @variable int;
	declare @f_date_create_utc DATETIME;
	declare @f_date_create DATETIME;
	set @f_date_create = dateadd(hour,-8,@f_date_create_utc);
	exec sp_executesql N'insert into admin(admin_username,admin_name,admin_last_name,admin_email,admin_date_create,admin_password,admin_private_key) values(@a_username,@a_name,@a_last_name,@a_email,@f_date_create,@a_password,@a_privatekey);';
	exec sp_executesql N'insert into adminxinfo(admin_username,user_type_id) values(@a_username,1);';
	return @variable;
end;
go;
create or alter function insertplayer(
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
)
returns int
as begin
	declare @variable int;
	exec sp_executesql N'insert into player values(@p_passport, @p_name,@p_lastname,@p_birthdate,@p_height,@p_weight,@p_team,@p_price,@p_active,@p_photo);';
	exec sp_executesql N'insert into playerxinfo(country_id,playerxposition_id,player_id) values(@p_country,@p_position,@p_passport);';
	return @variable;
end;
go;

alter procedure checkuser
	@user_username varchar(8),
	@user_password varchar(8)
as begin
	set nocount on;
	declare @result int;
	declare @admin_name varchar(10);
	declare @fanatic_name varchar(10);
	declare @password varchar(8);
	declare @pass_encrypt varbinary(max);
	set @admin_name = 'a_'+ @user_username;
	set @fanatic_name = 'f_'+ @user_username;
	if (select ASYMKEY_ID(@admin_name))>=0
		begin
			select @pass_encrypt = admin_password from admin where admin_username = @user_username;
			set @password = CONVERT(varchar(8),DECRYPTBYASYMKEY(ASYMKEY_ID(@admin_name),@pass_encrypt),0);
			if (@password = @user_password)
			begin
				set @result = 1;
			end else begin
				set @result = -3;
			end
		end
	else if (select ASYMKEY_ID(@fanatic_name))>=0
		begin
			select @pass_encrypt = fanatic_password from fanatic where fanatic_login = @user_username;
			set @password = CONVERT(varchar(8),DECRYPTBYASYMKEY(ASYMKEY_ID(@fanatic_name),@pass_encrypt),0);
			if (@password = @user_password)
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

create or alter function createdinkey(
	@keyname varchar(8)
)
returns varchar(max)
as begin
	declare @inst varchar(MAX);
	set @inst = 'create asymmetric key casa with algorithm = RSA_2048;';
	return @inst;
end;

create procedure createdinkey
	@keyname varchar(8)
AS begin	
	set nocount on;
	declare @inst varchar(max);
	exec sp_executesql N'create asymmetric key @keyname with algorithm = RSA_2048;';
end
go

createdinkey('juanpabl');