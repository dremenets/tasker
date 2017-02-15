CREATE TABLE [dbo].[Tasks] (
    [Id]            INT           NOT NULL,       --
    [Name]          VARCHAR (50)  NOT NULL,
    [ExpectedStart] DATETIME      NOT NULL,
    [Params]        VARCHAR (500) NULL,           -- параметры
    [Status]        tinyint NOT NULL DEFAULT 0,   -- состояние: Новая(0), Запланирована(1), Выполнена успещно(2), С ошибкой(3)  
    [Type]          VARCHAR (50)  NOT NULL        -- тип задания: File, Email
);


