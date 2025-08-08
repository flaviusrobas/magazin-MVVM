CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
AS
begin
	SET NOCOUNT ON;
	SELECT Id, FirstName, LastName, EmailAddress, CreateDate
	FROM [dbo].[User]
	WHERE Id = @Id;
end
