create or alter function insertfanatic(
	@f_login		VARCHAR(8),
	@f_name			VARCHAR(30),
	@f_last_name	VARCHAR(30),
	@f_email		VARCHAR(255),
	@f_phone		INT,
	@f_birth		DATE,
	@f_password		VARCHAR(344),
	@f_privatekey	VARCHAR(1616),
	@f_active		BIT = 1,
	@f_photo		IMAGE = NULL,
	@f_about		VARCHAR(300) = '',
	@f_country		INT
)
returns INT
as begin
	declare @variable int;
	declare @f_date_create_utc DATETIME;
	declare @f_date_create DATETIME;
	set @f_date_create = dateadd(hour,-8,@f_date_create_utc);
	EXEC sp_executesql N'insert into fanatic(fanatic_login,fanatic_name,fanatic_last_name,fanatic_email,fanatic_phone,fanatic_birth,fanatic_date_create,fanatic_password,fanatic_private_key,fanatic_active,fanatic_photo,fanatic_description) values (@f_login,@f_name,@f_last_name,@f_email,@f_phone,@f_birth,@f_date_create,@f_password,@f_privatekey,@f_active,@f_photo,@f_about);';
	EXEC sp_executesql N'insert into userxinfo(user_type_id,country_id,fanatic_login) values(2,@f_country,@f_login);';
	return @variable;
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
	@p_team varchar(30),
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