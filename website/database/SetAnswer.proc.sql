create procedure [dbo].[SetAnswer]
    @ID bigint,
    @EmailAddress nvarchar(256),
    @QuestionID bigint,
    @AnswerText nvarchar(2048),
    @OtherText nvarchar(512)
as
begin
    set transaction isolation level serializable;    
    begin transaction;

        declare @CurrentTime as datetime2 = getutcdate();
        declare @userID as bigint;
        exec @userID = [dbo].[LookupUserID] @EmailAddress;

        if (@ID = 0)
        begin
            -- insert new answer
            insert into [dbo].[Answers]([UserID], [QuestionID], [AnswerText], [OtherText], [AnsweredAt])
            values (@userID, @QuestionID, @AnswerText, @OtherText, @CurrentTime);
        end
        else
        begin
            -- update existing answer
            update [dbo].[Answers]
            set [AnswerText] = @AnswerText, [OtherText] = @OtherText, [AnsweredAt] = @CurrentTime
            from [dbo].[Answers]
            where [ID] = @ID and [UserID] = @userID and [QuestionID] = @QuestionID;         
        end

    commit;	
end