CREATE DATABASE Library
--DROP DATABASE Library
GO

USE [Library]
GO

--DROP TABLE Books
CREATE TABLE [dbo].[Books](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Category] [nvarchar](50) NULL,
	[Author] [nvarchar](50) NULL,
	[Publisher] [nvarchar](50) NULL,
	[YearPublished] [int] NULL,
	[Price] [int] NULL,
	[ISBN] [nvarchar](20) NULL,
	PRIMARY KEY (ID),
	UNIQUE (ISBN)
) 
GO

INSERT Books (ID, Name, Category, Author, Publisher, YearPublished, Price, ISBN) VALUES
(1, N'Sherlock Holmes Tập 1', N'Trinh thám', 'Conan Doyle', N'Hồng Đức', 2014, 75000, '9781281')
INSERT Books (ID, Name, Category, Author, Publisher, YearPublished, Price, ISBN) VALUES
(2, N'Sherlock Holmes Tập 2', N'Trinh thám', 'Conan Doyle', N'Hồng Đức', 2014, 75000, '9781282')
INSERT Books (ID, Name, Category, Author, Publisher, YearPublished, Price, ISBN) VALUES
(3, N'Sherlock Holmes Tập 3', N'Trinh thám', 'Conan Doyle', N'Hồng Đức', 2014, 75000, '9781283')
INSERT Books (ID, Name, Category, Author, Publisher, YearPublished, Price, ISBN) VALUES
(4, N'Lập trình Hướng đối tượng', N'Khoa học Máy tính', N'Trần Đan Thư', N'KHTN', 2015, 80000, '9721730')
INSERT Books (ID, Name, Category, Author, Publisher, YearPublished, Price, ISBN) VALUES
(5, N'Tôi Tự Học', N'Học tập', N'Nguyễn Duy Cần', N'Trẻ', 2016, 60000, '9991805')
GO

-- Stored Procedures
IF OBJECT_ID ('SP_GetAllBooks')
IS NOT NULL
DROP PROCEDURE SP_GetAllBooks
GO
CREATE PROCEDURE SP_GetAllBooks
AS
	SELECT * FROM Books
GO

--EXEC SP_GetAllBooks

IF OBJECT_ID ('SP_AddNewBook')
IS NOT NULL
DROP PROCEDURE SP_AddNewBook
GO
CREATE PROCEDURE SP_AddNewBook @ID int, @Name nvarchar(50), @Category nvarchar(50),
@Author nvarchar(50), @Publisher nvarchar(50), @YearPublished int, @Price int,
@ISBN nvarchar(15)
AS
	DECLARE @ret bit
	IF @ID IN (SELECT ID FROM BOOKS)
	BEGIN
		raiserror('New Book Object has Duplicated ID', 16, 1)
		SET @ret = 0
		RETURN @ret
	END
	IF @ISBN IN (SELECT ISBN FROM BOOKS)
	BEGIN
		raiserror('New Book Object has Duplicated ISBN', 16, 1)
		SET @ret = 0
		RETURN @ret
	END
	INSERT INTO BOOKS VALUES
	(@ID, @Name, @Category, @Author, @Publisher, @YearPublished, @Price, @ISBN)
	SET @ret = 1
	RETURN @ret
GO

--EXEC SP_AddNewBook 6, N'Tony Buổi tối', null, null, null, null, null, '9991805'

IF OBJECT_ID ('SP_DeleteBook')
IS NOT NULL 
DROP PROCEDURE SP_DeleteBook
GO
CREATE PROCEDURE SP_DeleteBook @bID int
AS
	DELETE FROM BOOKS WHERE ID = @bID
GO

--EXEC SP_DeleteBook 6