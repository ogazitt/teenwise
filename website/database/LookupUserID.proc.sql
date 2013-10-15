create procedure [dbo].[LookupUserID]
    @EmailAddress as nvarchar(256)
as
begin
    -- no transaction semantics, use within another sproc    

    declare @userID as bigint;
    set @userID = (select [UserID] from [dbo].[Users] where [EmailAddress] = @EmailAddress)

    if (@userID is null)
        return 0;

    return @userID;
end

