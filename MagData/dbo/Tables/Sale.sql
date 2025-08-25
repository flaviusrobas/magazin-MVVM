CREATE TABLE [dbo].[Sale]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[CashierId] nvarchar(128) NOT NULL,
	[SaleDate] datetime2(7) NOT NULL, 
    [SubTotal] MONEY NOT NULL, 
    [Tax] MONEY NOT NULL, 
    [Total] MONEY NOT NULL, 
    CONSTRAINT [FK_Sale_ToUser] FOREIGN KEY (CashierId) REFERENCES [User](Id)
)
