CREATE PROCEDURE [dbo].[DeletePOS]
	@ProductID INT
AS
BEGIN
	DELETE FROM [dbo].[POD] WHERE ProductID = @ProductID
END