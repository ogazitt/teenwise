create procedure [dbo].[RegisterUser]
    @EmailAddress nvarchar(256)
as
begin
    set transaction isolation level serializable;    
    begin transaction;

        declare @CurrentTime as datetime2 = getutcdate();

        insert into [dbo].[Users]([EmailAddress], [RegisteredAt])
        values (@EmailAddress, @CurrentTime);
        
    commit;	
end