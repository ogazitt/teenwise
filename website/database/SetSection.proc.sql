create procedure [dbo].[SetSection]
    @SectionID int,
    @SurveyName nvarchar(64),
    @SectionName nvarchar(128),
    @SectionIndex int
as
begin
    set transaction isolation level serializable;    
    begin transaction;

        if (@SectionID = 0)
        begin
            -- insert new section
            insert into [dbo].[Surveys]([SurveyName], [SectionName], [SectionIndex])
            values (@SurveyName, @SectionName, @SectionIndex );
        end
        else
        begin
            -- update existing section
            update [dbo].[Surveys]
            set [SurveyName] = @SurveyName, [SectionName] = @SectionName, [SectionIndex] = @SectionIndex
            from [dbo].[Surveys]
            where [SectionID] = @SectionID;         
        end

    commit;	
end