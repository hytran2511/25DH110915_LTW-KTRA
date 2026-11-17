USE [master]
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'MyStore')
BEGIN
    CREATE DATABASE [MyStore]
END
GO
USE [MyStore]
GO
GO
/****** Object:  Table [dbo].[Category]    Script Date: 11/17/2026 11:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 11/17/2026 11:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](max) NOT NULL,
	[CustomerPhone] [nvarchar](15) NOT NULL,
	[CustomerEmail] [nvarchar](max) NULL,
	[CustomerAddress] [nvarchar](max) NULL,
	[Username] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 11/17/2026 11:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[OrderDate] [date] NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[PaymentStatus] [nvarchar](max) NULL,
	[ShippingAddress] [nvarchar](max) NOT NULL,
	[DeliveryMethod] [nvarchar](50) NULL,
	[PaymentMethod] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 11/17/2026 11:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[OrderID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[TotalPrice]  AS ([Quantity]*[UnitPrice]) PERSISTED,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/17/2026 11:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[ProductName] [nvarchar](max) NOT NULL,
	[ProductDecription] [nvarchar](max) NOT NULL,
	[ProductPrice] [decimal](18, 2) NOT NULL,
	[ProductImage] [nvarchar](max) NULL,
	[Quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 11/17/2026 11:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Username] [nvarchar](255) NOT NULL,
	[Password] [nchar](50) NOT NULL,
	[UserRole] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__User__536C85E5035BEFCE] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (1, N'Kinh tế')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2, N'Kỹ năng sống')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (3, N'Thiếu nhi')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (4, N'Truyện tranh')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (5, N'Nghệ thuật')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (6, N'Tâm lý')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (7, N'Thường thức')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (8, N'Sách văn học Việt Nam cổ điển')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([CustomerID], [CustomerName], [CustomerPhone], [CustomerEmail], [CustomerAddress], [Username]) VALUES (1, N'Trần Gia Hy', N'0909123456', N'giahy@gmail.com', N'828 Sư Vạn Hạnh, Quận 10, TP.HCM', N'khachhang1')
INSERT [dbo].[Customer] ([CustomerID], [CustomerName], [CustomerPhone], [CustomerEmail], [CustomerAddress], [Username]) VALUES (2, N'Nguyễn Văn Tèo', N'0912345678', N'teonv@gmail.com', N'123 Nguyễn Trãi, Quận 5, TP.HCM', N'teonv')
INSERT [dbo].[Customer] ([CustomerID], [CustomerName], [CustomerPhone], [CustomerEmail], [CustomerAddress], [Username]) VALUES (3, N'Phạm Thị Hòa', N'0987654321', N'hoapt@gmail.com', N'45 Lê Lợi, Quận 1, TP.HCM', N'hoapt')
INSERT [dbo].[Customer] ([CustomerID], [CustomerName], [CustomerPhone], [CustomerEmail], [CustomerAddress], [Username]) VALUES (4, N'Trần Gia Hy', N'1234567', N'giahy@gmail.com', N'abc', N'Nghia123')
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [ShippingAddress], [DeliveryMethod], [PaymentMethod]) VALUES (1, 1, CAST(N'2026-10-20' AS Date), CAST(412000.00 AS Decimal(18, 2)), N'Đã giao', N'828 Sư Vạn Hạnh, Quận 10, TP.HCM', N'Giao hàng nhanh', N'Tiền mặt')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [ShippingAddress], [DeliveryMethod], [PaymentMethod]) VALUES (2, 2, CAST(N'2026-10-24' AS Date), CAST(184400.00 AS Decimal(18, 2)), N'Chưa thanh toán', N'123 Nguyễn Trãi, Quận 5, TP.HCM', N'Giao hàng tiết kiệm', N'Tiền mặt')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [ShippingAddress], [DeliveryMethod], [PaymentMethod]) VALUES (3, 1, CAST(N'2026-11-06' AS Date), CAST(24200.00 AS Decimal(18, 2)), N'Đã hủy', N'828 Sư Vạn Hạnh, Quận 10, TP.HCM', N'Giao hàng tiết kiệm', N'Tiền mặt')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [ShippingAddress], [DeliveryMethod], [PaymentMethod]) VALUES (4, 4, CAST(N'2026-11-06' AS Date), CAST(240000.00 AS Decimal(18, 2)), N'Chờ thanh toán', N'828 Sư Vạn Hạnh, Quận 10, TP.HCM', N'Giao hàng nhanh', N'Tiền mặt')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [ShippingAddress], [DeliveryMethod], [PaymentMethod]) VALUES (5, 4, CAST(N'2026-11-09' AS Date), CAST(728000.00 AS Decimal(18, 2)), N'Đã giao', N'828 Sư Vạn Hạnh, Quận 10, TP.HCM', N'Giao hàng nhanh', N'Tiền mặt')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [ShippingAddress], [DeliveryMethod], [PaymentMethod]) VALUES (6, 4, CAST(N'2026-11-09' AS Date), CAST(112000.00 AS Decimal(18, 2)), N'Đang vận chuyển', N'828 Sư Vạn Hạnh, Quận 10, TP.HCM', N'Giao hàng nhanh', N'Tiền mặt')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [ShippingAddress], [DeliveryMethod], [PaymentMethod]) VALUES (7, 4, CAST(N'2026-11-09' AS Date), CAST(180000.00 AS Decimal(18, 2)), N'Đã hủy', N'828 Sư Vạn Hạnh, Quận 10, TP.HCM', N'Giao hàng tiết kiệm', N'Tiền mặt')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [ShippingAddress], [DeliveryMethod], [PaymentMethod]) VALUES (8, 4, CAST(N'2026-11-09' AS Date), CAST(112000.00 AS Decimal(18, 2)), N'Thanh toán tiền mặt', N'828 Sư Vạn Hạnh, Quận 10, TP.HCM', N'Giao hàng nhanh', N'Tiền mặt')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [ShippingAddress], [DeliveryMethod], [PaymentMethod]) VALUES (9, 4, CAST(N'2026-11-09' AS Date), CAST(24200.00 AS Decimal(18, 2)), N'Thanh toán tiền mặt', N'828 Sư Vạn Hạnh, Quận 10, TP.HCM', N'Giao hàng nhanh', N'Tiền mặt')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [ShippingAddress], [DeliveryMethod], [PaymentMethod]) VALUES (10, 4, CAST(N'2026-11-09' AS Date), CAST(95000.00 AS Decimal(18, 2)), N'Thanh toán tiền mặt', N'828 Sư Vạn Hạnh, Quận 10, TP.HCM', N'Giao hàng nhanh', N'Tiền mặt')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [ShippingAddress], [DeliveryMethod], [PaymentMethod]) VALUES (11, 4, CAST(N'2026-11-09' AS Date), CAST(115000.00 AS Decimal(18, 2)), N'Thanh toán paypal', N'828 Sư Vạn Hạnh, Quận 10, TP.HCM', N'Giao hàng nhanh', N'Paypal')
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetail] ON 

INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (1, 1, 1, 1, CAST(112000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (2, 2, 1, 1, CAST(180000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (3, 3, 1, 1, CAST(120000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (4, 4, 2, 1, CAST(136000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (5, 5, 2, 2, CAST(24200.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (6, 5, 3, 1, CAST(24200.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (7, 3, 4, 2, CAST(120000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (8, 2, 5, 2, CAST(180000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (9, 1, 5, 1, CAST(112000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (10, 1, 6, 1, CAST(112000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (11, 2, 7, 1, CAST(180000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (12, 1, 8, 1, CAST(112000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (13, 5, 9, 1, CAST(24200.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (14, 6, 10, 1, CAST(95000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (15, 8, 11, 1, CAST(115000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (16, 3, 5, 1, CAST(120000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (17, 4, 5, 1, CAST(136000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[OrderDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDecription], [ProductPrice], [ProductImage], [Quantity]) VALUES (1, 6, N'Tâm Lý Học Đám Đông (Tái Bản 2022)', N'Tác phẩm kinh điển của Gustave Le Bon phân tích sâu sắc tâm lý và hành vi của đám đông trong xã hội.', CAST(112000.00 AS Decimal(18, 2)), N'tam-ly.jpg', 97)
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDecription], [ProductPrice], [ProductImage], [Quantity]) VALUES (2, 1, N'Người Giàu Có Nhất Thành Babylon', N'Cuốn sách kinh điển dạy làm giàu và quản lý tài chính cá nhân thông qua những câu chuyện ngụ ngôn tại thành Babylon cổ đại.', CAST(180000.00 AS Decimal(18, 2)), N'babylon.jpg', 97)
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDecription], [ProductPrice], [ProductImage], [Quantity]) VALUES (3, 8, N'Khởi Nghiệp Bán Lẻ - Bí Quyết Thành Công Và Giàu Có', N'Công thức kinh doanh và quản lý cửa hàng bán lẻ hiệu quả dành cho người khởi nghiệp hiện đại.', CAST(120000.00 AS Decimal(18, 2)), N'khoi-nghiep.png', 100)
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDecription], [ProductPrice], [ProductImage], [Quantity]) VALUES (4, 2, N'Nguyên Lý 80/20 - Bí Quyết Làm Ít Được Nhiều', N'Khám phá quy luật Pareto 80/20 giúp tối ưu hóa thời gian, năng suất làm việc và đạt hiệu quả vượt trội.', CAST(136000.00 AS Decimal(18, 2)), N'80-20.jpg', 100)
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDecription], [ProductPrice], [ProductImage], [Quantity]) VALUES (5, 4, N'Thám Tử Lừng Danh Conan (Tái Bản 2023) - Tập 100', N'Hành trình phá án đầy kịch tính của chàng thám tử trung học Shinichi Kudo trong hình hài cậu bé Conan.', CAST(24200.00 AS Decimal(18, 2)), N'conan-tap-100.jpg', 99)
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDecription], [ProductPrice], [ProductImage], [Quantity]) VALUES (6, 2, N'Đắc Nhân Tâm (Khổ Lớn)', N'Nghệ thuật thu phục lòng người và giao tiếp ứng xử thành công của tác giả Dale Carnegie.', CAST(95000.00 AS Decimal(18, 2)), N'dac-nhan-tam.jpg', 99)
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDecription], [ProductPrice], [ProductImage], [Quantity]) VALUES (7, 3, N'Dế Mèn Phiêu Lưu Ký', N'Tác phẩm văn học thiếu nhi bất hủ của nhà văn Tô Hoài về cuộc phiêu lưu đầy ý nghĩa của chú Dế Mèn.', CAST(68000.00 AS Decimal(18, 2)), N'de-men.jpg', 100)
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDecription], [ProductPrice], [ProductImage], [Quantity]) VALUES (8, 5, N'Nghệ Thuật Tư Duy Rõ Ràng', N'99 sai lầm tư duy phổ biến mà con người thường mắc phải và cách khắc phục để ra quyết định sáng suốt.', CAST(115000.00 AS Decimal(18, 2)), N'nghe-thuat.jpg', 99)
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDecription], [ProductPrice], [ProductImage], [Quantity]) VALUES (9, 4, N'Doraemon Truyện Ngắn - Tập 1', N'Những câu chuyện hài hước, cảm động về tình bạn giữa Nobita và chú mèo máy Doraemon đến từ thế kỷ 22.', CAST(22000.00 AS Decimal(18, 2)), N'doraemon-tap-1.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDecription], [ProductPrice], [ProductImage], [Quantity]) VALUES (10, 7, N'Nhà Giả Kim (The Alchemist)', N'Hành trình theo đuổi vận mệnh và ước mơ của chàng chăn cừu Santiago, một trong những cuốn sách bán chạy nhất mọi thời đại.', CAST(89000.00 AS Decimal(18, 2)), N'gia-kim.jpg', 100)
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
INSERT [dbo].[User] ([Username], [Password], [UserRole]) VALUES (N'admin', N'123456                                            ', N'Admin')
INSERT [dbo].[User] ([Username], [Password], [UserRole]) VALUES (N'hoapt', N'123456                                            ', N'C')
INSERT [dbo].[User] ([Username], [Password], [UserRole]) VALUES (N'khachhang1', N'123456                                            ', N'C')
INSERT [dbo].[User] ([Username], [Password], [UserRole]) VALUES (N'Nghia123', N'12345                                             ', N'C')
INSERT [dbo].[User] ([Username], [Password], [UserRole]) VALUES (N'teonv', N'123456                                            ', N'C')
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_User_Customer] FOREIGN KEY([Username])
REFERENCES [dbo].[User] ([Username])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_User_Customer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Pro_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Pro_Category]
GO
USE [master]
GO
ALTER DATABASE [MyStore] SET  READ_WRITE 
GO

