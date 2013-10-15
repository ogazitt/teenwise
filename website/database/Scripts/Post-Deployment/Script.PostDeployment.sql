/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if (serverproperty(N'EngineEdition') <> 5) -- if not azure
begin
    -- on by default in SQL Azure, but not settable
    alter database corpweb set ALLOW_SNAPSHOT_ISOLATION on;
    alter database corpweb set READ_COMMITTED_SNAPSHOT on;
end