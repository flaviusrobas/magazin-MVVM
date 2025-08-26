CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id INT

AS
begin
	set nocount on;

	select Id, ProductName, [Description], RetailPrice, QuantityInStock, IsTaxable 
	from dbo.Product
	where Id = @Id;
end	
