create table [dbo].[Answers]
(
    [ID] bigint identity(1, 1) not null, 
    [UserID] bigint not null,
    [QuestionID] bigint not null,
    [AnswerText] nvarchar(2048) null,
    [OtherText] nvarchar(512) null,
    [AnsweredAt] datetime2 null,
    
    constraint [PK_AnswerID] primary key ([ID]),

    constraint [FK_Answers_User] foreign key ([UserID])
    references [dbo].[Users]([UserID])
    on delete cascade,

    constraint [FK_Answers_Question] foreign key ([QuestionID])
    references [dbo].[Questions]([ID])
)
