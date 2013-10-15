create table [dbo].[Surveys]
(
    [SectionID] int identity(1, 1) not null, 
    [SurveyName] nvarchar(64) not null,
    [SectionName] nvarchar(128) not null,
    [SectionIndex] int not null,
    
    constraint [PK_SectionID] primary key ([SectionID]),

    constraint [UNIQUE_Name]
    unique ([SurveyName], [SectionName]),

    constraint [UNIQUE_SectionIndex]
    unique ([SurveyName], [SectionIndex])
)
