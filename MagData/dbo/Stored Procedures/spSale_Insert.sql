CREATE PROCEDURE [dbo].[spSale_Insert]
	@Id int OUTPUT,
	@CashierId nvarchar(128),
	@SaleDate datetime2,
	@SubTotal money,
	@Tax money,
	@Total money
AS
begin
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Sale] (CashierId, SaleDate, SubTotal, Tax, Total)
	VALUES (@CashierId, @SaleDate, @SubTotal, @Tax, @Total)
	--SET @Id = @@IDENTITY;
	SET @Id = SCOPE_IDENTITY();
END