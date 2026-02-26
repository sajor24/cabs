CREATE PROCEDURE [dbo].[UpdatePOS]
	@ProductID INT,  
    @ProductName NVARCHAR(50) = NULL,            
    @Price NVARCHAR(50) = NULL,                      
    @Stock INT                      
         
AS
	BEGIN
    UPDATE [dbo].[POS]
	SET 
		[ProductName] = @ProductName,
		[Price] = @Price,
		[Stock] = @Stock
		
	WHERE [ProductID] = @ProductID;
    END
