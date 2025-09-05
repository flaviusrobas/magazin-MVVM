CREATE PROCEDURE [dbo].[spInventory_GetAll]
	
AS
begin
	set nocount on;
	SELECT [Id], [ProductId], [Quantity], [PurchasePrice], [PurchaseDate]
	FROM [dbo].[Inventory];
end
