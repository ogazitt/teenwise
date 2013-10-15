create procedure [dbo].[GetQuestions]
    @SectionID int
as
begin
    set transaction isolation level snapshot;
    begin transaction;
    
        select [ID], [Type], [QuestionText], [OtherText], [Options]
        from [dbo].[Questions]
        where [SectionID] = @SectionID
        order by [Index] asc

    commit;
end

