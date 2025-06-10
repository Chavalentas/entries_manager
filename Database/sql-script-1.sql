CREATE TABLE [dbo].[User](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[Email] [varchar](100) NOT NULL
	CONSTRAINT DBO_USER_UNIQUE_EMAIL UNIQUE(Email),
	CONSTRAINT DBO_USER_UNIQUE_USERNAME UNIQUE(UserName),
	CONSTRAINT DBO_USER_PK_USERID PRIMARY KEY CLUSTERED (UserId)
);

CREATE TABLE [dbo].[Entry](
  [EntryId] [bigint] IDENTITY(1,1) NOT NULL,
  [EntryText] [text] NULL,
  [EntryDate] [datetime] NOT NULL,
  [UserId] [bigint] NOT NULL,
  [EntryTitle] [varchar](50) NOT NULL,
  CONSTRAINT DBO_ENTRY_PK_ENTRYID PRIMARY KEY CLUSTERED (EntryId),
  CONSTRAINT DBO_ENTRY_FK_USERID_USERID FOREIGN KEY (UserId) REFERENCES [dbo].[User] (UserId)
);


--Triggers--
CREATE OR ALTER TRIGGER UserDeleteTrigger
on [dbo].[User]
Instead Of DELETE 
as
BEGIN
	BEGIN TRY 
    BEGIN TRANSACTION;
    DECLARE @deletedUserId bigint = 0;
    Select @deletedUserId = UserId from Deleted;
    DELETE FROM [dbo].[Entry] WHERE UserId = @deletedUserId;
    DELETE FROM [dbo].[User] WHERE UserId = @deletedUserId;
    COMMIT;
	END TRY  
	BEGIN CATCH  
	    PRINT 'An error occurred.'; 
	   ROLLBACK;
	   THROW
	END CATCH;
END

--Functions--
CREATE OR ALTER FUNCTION DoesUsernameExist(@userName varchar(50))
RETURNS 
bit
AS 
BEGIN 
	DECLARE @rowsCount bigint = 0
	select @rowsCount = count(*) from [User] u 
	where UserName = @userName
	
	if @rowsCount = 1
	begin
		return 1
	end
	
	return 0
END

CREATE OR ALTER FUNCTION DoesUsernameExistExceptId(@userName varchar(50), @userId bigint)
RETURNS 
bit
AS 
BEGIN 
	DECLARE @rowsCount bigint = 0
	select @rowsCount = count(*) from [User] u 
	where UserName = @userName and UserId != @userId
	
	if @rowsCount = 1
	begin
		return 1
	end
	
	return 0
END

CREATE OR ALTER FUNCTION DoesEmailExist(@email varchar(50))
RETURNS 
bit
AS 
BEGIN 
	DECLARE @rowsCount bigint = 0
	select @rowsCount = count(*) from [User] u 
	where Email = @email
	
	if @rowsCount = 1
	begin
		return 1
	end
	
	return 0
END

CREATE OR ALTER FUNCTION DoesEmailExistExceptId(@email varchar(50), @userId bigint)
RETURNS 
bit
AS 
BEGIN 
	DECLARE @rowsCount bigint = 0
	select @rowsCount = count(*) from [User] u 
	where Email = @email and UserId != @userId
	
	if @rowsCount = 1
	begin
		return 1
	end
	
	return 0
END

CREATE OR ALTER FUNCTION DoesUserIdExist(@userId bigint)
RETURNS 
bit
AS 
BEGIN 
	DECLARE @rowsCount bigint = 0
	select @rowsCount = count(*) from [User] u 
	where UserId = @userId
	
	if @rowsCount = 1
	begin
		return 1
	end
	
	return 0
END 

--Stored procedures--
create or alter PROCEDURE CreateNewEntry
@userId bigint,
@creationDateText varchar(50),
@entryTitle varchar(50),
@entryText text
AS 
BEGIN 
	BEGIN TRY 
	   BEGIN TRANSACTION;
	      DECLARE @dateConverted datetime;
	      SET @dateConverted = CAST(@creationDateText AS DATETIME);
	      INSERT INTO Entry (EntryText, EntryTitle, EntryDate, UserId) VALUES (@entryText, @entryTitle, @creationDateText, @userId)
       COMMIT;
       RETURN 0;
	END TRY  
	BEGIN CATCH  
	    PRINT 'An error occurred.'; 
	   ROLLBACK;
	   THROW
	END CATCH;
END 