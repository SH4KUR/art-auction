USE [ArtAuction]

BEGIN TRY
	BEGIN TRANSACTION UsersInsertTransaction

	-- [dbo].[user]

	INSERT [dbo].[user] ([user_id], [login], [email], [password], [role], [first_name], [last_name], [patronymic], [birth_date], [address], [is_vip], [is_blocked]) VALUES (N'f4e3bdc3-0e42-479c-a194-6b7f81f188e2', N'customer', N'customer@gmail.com', N'B97BDCB0B4D1802A2E727FBE143CC631935BCA83C60019E1733620C08916095B', 3, N'CustomerName', N'CustomerSurname', N'CustomerPatronymic', CAST(N'2003-03-03' AS Date), N'Some Customer address', 0, 0)
	INSERT [dbo].[user] ([user_id], [login], [email], [password], [role], [first_name], [last_name], [patronymic], [birth_date], [address], [is_vip], [is_blocked]) VALUES (N'8cf4ba2e-2965-4714-bf54-76b697304a3c', N'admin', N'admin@gmail.com', N'B97BDCB0B4D1802A2E727FBE143CC631935BCA83C60019E1733620C08916095B', 1, N'AdminName', N'AdminSurname', N'AdminPatronymic', CAST(N'2001-01-01' AS Date), N'Some Admin address', 0, 0)
	INSERT [dbo].[user] ([user_id], [login], [email], [password], [role], [first_name], [last_name], [patronymic], [birth_date], [address], [is_vip], [is_blocked]) VALUES (N'e61bc676-9359-44e8-a762-e1b3cd2a8628', N'seller', N'seller@gmail.com', N'B97BDCB0B4D1802A2E727FBE143CC631935BCA83C60019E1733620C08916095B', 2, N'SellerName', N'SellerSurname', N'SellerPatronymic', CAST(N'2002-02-02' AS Date), N'Some Seller address', 0, 0)

	-- [dbo].[account]

	INSERT [dbo].[account] ([account_id], [user_id], [sum], [last_update]) VALUES (N'36fd0be0-7c7d-4627-b5ff-1de4dfb7b5b2', N'8cf4ba2e-2965-4714-bf54-76b697304a3c', CAST(0.00000 AS Decimal(19, 5)), CAST(N'2022-02-20T18:36:20.590' AS DateTime))
	INSERT [dbo].[account] ([account_id], [user_id], [sum], [last_update]) VALUES (N'843ff366-d0bd-4caa-8855-d1a2d746411b', N'f4e3bdc3-0e42-479c-a194-6b7f81f188e2', CAST(0.00000 AS Decimal(19, 5)), CAST(N'2022-02-20T18:33:43.273' AS DateTime))
	INSERT [dbo].[account] ([account_id], [user_id], [sum], [last_update]) VALUES (N'2abf7437-230c-48a2-bed3-ebdaa26b1959', N'e61bc676-9359-44e8-a762-e1b3cd2a8628', CAST(0.00000 AS Decimal(19, 5)), CAST(N'2022-02-20T18:32:31.327' AS DateTime))

	COMMIT TRANSACTION UsersInsertTransaction
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION UsersInsertTransaction
END CATCH