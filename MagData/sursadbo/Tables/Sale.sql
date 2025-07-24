CREATE TABLE [dbo].[Sale]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[CashierId] nvarchar(50) NOT NULL,
	[SaleDate] datetime2(7) NOT NULL, 
    [SubTotal] MONEY NOT NULL, 
    [Tax] MONEY NOT NULL, 
    [Total] MONEY NOT NULL,
)
