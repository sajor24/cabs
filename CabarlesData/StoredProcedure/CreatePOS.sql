CREATE PROCEDURE [dbo].[CreatePOS]
	@ProductID int,
	@ProductName nvarchar(255),
	@Price int,
	@Stock int
AS
	BEGIN
    INSERT INTO [dbo].[POS] ([ProductID], [ProductName], [Price], [Stock])
	VALUES (@ProductID,@ProductName,@Price,@Stock);
    END
