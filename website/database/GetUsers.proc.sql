create procedure [dbo].[GetUsers]
as
begin
    set transaction isolation level snapshot;
    begin transaction;
    
        select [UserID], [EmailAddress], [RegisteredAt]
        from [dbo].[Users]

    commit;
end

