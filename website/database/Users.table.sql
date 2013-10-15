create table [dbo].[Users]
(
    [UserID] bigint identity(1, 1) not null, 
    [EmailAddress] nvarchar(256) not null,
    [RegisteredAt] datetime2 null,
    
    constraint [PK_UserID] primary key ([UserID]),

    constraint [UNIQUE_EmailAddress]
    unique ([EmailAddress])
)
