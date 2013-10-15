create table [dbo].[Questions]
(
    [ID] bigint identity(1, 1) not null, 
    [SectionID] int not null,
    [Index] int not null,
    [Type] int not null,
    [QuestionText] nvarchar(1024) not null,
    [OtherText] nvarchar(256) null,
    [Options] nvarchar(1024) null,
    
    constraint [PK_QuestionID] primary key ([ID]),

    constraint [UNIQUE_QuestionIndex]
    unique ([SectionID], [Index]),

    constraint [FK_Questions_Section] foreign key ([SectionID])
    references [dbo].[Surveys]([SectionID])
)
