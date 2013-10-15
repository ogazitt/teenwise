create procedure [dbo].[GetSurvey]
    @SurveyName nvarchar(64)
as
begin
    set transaction isolation level snapshot;
    begin transaction;
    
        select [SectionID], [SectionName]
        from [dbo].[Surveys]
        where [SurveyName] = @SurveyName
        order by [SectionIndex] asc

    commit;
end

