create procedure [dbo].[SetQuestion]
    @ID bigint,
    @SectionID int,
    @Index int,
    @Type int,
    @QuestionText nvarchar(1024),
    @OtherText nvarchar(256),
    @Options nvarchar(1024)
as
begin
    set transaction isolation level serializable;    
    begin transaction;

        if (@ID = 0)
        begin
            -- insert new question
            insert into [dbo].[Questions]([SectionID], [Index], [Type], [QuestionText], [OtherText], [Options])
            values (@SectionID, @Index, @Type, @QuestionText, @OtherText, @Options);
        end
        else
        begin
            -- update existing question
            update [dbo].[Questions]
            set [SectionID] = @SectionID, [Index] = @Index, [Type] = @Type, [QuestionText] = @QuestionText, [OtherText] = @OtherText, [Options] = @Options
            from [dbo].[Questions]
            where [ID] = @ID;         
        end

    commit;	
end