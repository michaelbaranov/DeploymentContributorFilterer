﻿CREATE TABLE [dbo].[Employees]
(
	[EmployeeId] INT NOT NULL PRIMARY KEY,
	[Name] VARCHAR(25) NULL default  'aaaaa', 
    [UniqueId] UNIQUEIDENTIFIER NULL DEFAULT newid()
)
