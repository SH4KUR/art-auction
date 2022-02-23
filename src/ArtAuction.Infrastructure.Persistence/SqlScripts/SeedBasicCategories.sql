USE [ArtAuction]

BEGIN TRY
	BEGIN TRANSACTION CategoriesInsertTransaction

		INSERT INTO [dbo].[category] ([name]) VALUES ('ABSTRACT')
		INSERT INTO [dbo].[category] ([name]) VALUES ('BAUHAUS')
		INSERT INTO [dbo].[category] ([name]) VALUES ('EXPRESSIONISM')
		INSERT INTO [dbo].[category] ([name]) VALUES ('IMPRESSIONISM')
		INSERT INTO [dbo].[category] ([name]) VALUES ('HISTORY PAINTING')
		INSERT INTO [dbo].[category] ([name]) VALUES ('POP ART')
		INSERT INTO [dbo].[category] ([name]) VALUES ('PORTRAIT')
		INSERT INTO [dbo].[category] ([name]) VALUES ('REALISM')
		INSERT INTO [dbo].[category] ([name]) VALUES ('RENAISSANCE')
		INSERT INTO [dbo].[category] ([name]) VALUES ('ROCOCO')
		INSERT INTO [dbo].[category] ([name]) VALUES ('ROMANTICISM')
		INSERT INTO [dbo].[category] ([name]) VALUES ('SURREALISM')
		INSERT INTO [dbo].[category] ([name]) VALUES ('LANDSCAPE')

	COMMIT TRANSACTION CategoriesInsertTransaction
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION CategoriesInsertTransaction
END CATCH