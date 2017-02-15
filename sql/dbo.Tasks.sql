USE [taskerDB]
GO

/****** Объект: Table [dbo].[Tasks] Дата скрипта: 13.02.2017 19:51:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tasks] (
    [Id]            INT           NOT NULL,
    [Name]          VARCHAR (50)  NOT NULL,
    [ExpectedStart] DATETIME      NOT NULL,
    [Params]        VARCHAR (500) NULL,
    [Status]        tinyint NOT NULL DEFAULT 0,
    [Type]          VARCHAR (50)  NOT NULL
);


