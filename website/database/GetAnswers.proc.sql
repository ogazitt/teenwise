create procedure [dbo].[GetAnswers]
    @EmailAddress nvarchar(256),
    @SectionID int
as
begin
    set transaction isolation level snapshot;
    begin transaction;
    
        select a.[ID], a.[QuestionID], a.[AnswerText], a.[OtherText]
        from [dbo].[Answers] as a
            inner join [dbo].[Users] as u on a.[UserID] = u.[UserID]
            inner join [dbo].[Questions] as q on a.[QuestionID] = q.[ID]
        where u.EmailAddress = @EmailAddress and q.[SectionID] = @SectionID
        order by q.[Index] asc;

    commit;
end

