USE [ProductDatabase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteProduct]
	@id int
AS
BEGIN

	SET NOCOUNT ON;

	delete Product		
	WHERE Id = @id
	 
END

USE [ProductDatabase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetProductById]
	@id int
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		p.Id,
		p.Name,
		p.Category,
		p.Description,
		p.Producer,
		p.Supplier,
		p.Price
	FROM Product P
	WHERE p.Id = @id
END
USE [ProductDatabase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetProducts]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		p.Id,
		p.Name,
		p.Category,
		p.Description,
		p.Producer,
		p.Supplier,
		p.Price
		
	FROM Product p

END

USE [ProductDatabase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertProduct]
	@name nvarchar(50),
	@description nvarchar(50),
	@category nvarchar(50),
	@producer nvarchar(50),
	@supplier nvarchar(50),
	@price float,
	@id int output
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO Product(Name, Description, Category, Producer, Supplier, Price)
	VALUES (@name, @description, @category, @producer,@supplier,@price)
	
	SELECT @id = SCOPE_IDENTITY()
	 
END

USE [ProductDatabase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateProduct]
	@id int,
	@name nvarchar(50),
	@description nvarchar(50),
	@category nvarchar(50),
	@producer nvarchar(50),
	@supplier nvarchar(50),
	@price float
	
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE Product
	SET 
		Name = @name,
		Description= @description,
		Category = @category,
		Producer= @producer,
		Supplier= @supplier,
		Price= @price
		
	WHERE Id = @id
	 
END