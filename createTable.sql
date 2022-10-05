create database testDB;
go

CREATE TABLE testDB.dbo.Test1
(
    Id int PRIMARY KEY,
    FirstName varchar(20)
);

CREATE TABLE testDB.dbo.Test3
(
    Id int PRIMARY KEY,
    LastName varchar(20)
);

Insert Into testDB.dbo.Test1 values(1, 'David');
Insert Into testDB.dbo.Test3 values(1, 'He');