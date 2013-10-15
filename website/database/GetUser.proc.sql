create procedure [dbo].[GetUser]
    @EmailAddress nvarchar(256)
as
begin
    set transaction isolation level snapshot;
    begin transaction;

        select [UserID], [EmailAddress], [RegisteredAt]
        from [dbo].[Users]
        where [EmailAddress] = @EmailAddress

    commit;
end
