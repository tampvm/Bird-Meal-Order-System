USE [BMOS]
GO
/****** Object:  Table [dbo].[Tbl_Address]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Address](
	[address_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[address] [nvarchar](250) NULL,
	[city_province] [nvarchar](100) NULL,
	[district] [nvarchar](100) NULL,
	[block_village] [nvarchar](100) NULL,
	[is_default] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[address_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Blog]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Blog](
	[blog_id] [varchar](50) NOT NULL,
	[name] [nvarchar](200) NULL,
	[description] [nvarchar](max) NULL,
	[date] [datetime] NULL,
	[status] [bit] NULL,
 CONSTRAINT [PK_Tbl_Blog] PRIMARY KEY CLUSTERED 
(
	[blog_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_FavouriteList]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_FavouriteList](
	[favourite_list_id] [varchar](50) NOT NULL,
	[user_id] [int] NULL,
	[product_id] [varchar](50) NULL,
 CONSTRAINT [PK_Tbl_FavouriteList] PRIMARY KEY CLUSTERED 
(
	[favourite_list_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Feedback]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Feedback](
	[feedback_id] [varchar](50) NOT NULL,
	[product_id] [varchar](50) NULL,
	[user_id] [int] NULL,
	[content] [nvarchar](200) NULL,
	[star] [int] NULL,
	[date] [datetime] NULL,
 CONSTRAINT [PK_Tbl_Feedback] PRIMARY KEY CLUSTERED 
(
	[feedback_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Image]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Image](
	[image_id] [varchar](50) NOT NULL,
	[name] [varchar](50) NULL,
	[relation_id] [nvarchar](max) NULL,
	[type] [varchar](20) NULL,
	[url] [text] NULL,
 CONSTRAINT [PK_Tbl_Image] PRIMARY KEY CLUSTERED 
(
	[image_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Notify]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Notify](
	[notify_id] [varchar](50) NOT NULL,
	[user_id] [int] NULL,
	[message] [nvarchar](200) NULL,
	[type] [varchar](50) NULL,
	[date] [datetime] NULL,
 CONSTRAINT [PK_Tbl_Notify] PRIMARY KEY CLUSTERED 
(
	[notify_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Order]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Order](
	[order_id] [varchar](50) NOT NULL,
	[user_id] [int] NULL,
	[total_price] [float] NULL,
	[date] [datetime] NULL,
	[is_confirm] [bit] NULL,
	[address] [ntext] NULL,
	[phone] [varchar](10) NULL,
	[note] [text] NULL,
	[payment_type] [varchar](50) NULL,
	[point] [float] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Tbl_Order] PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_OrderDetail]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_OrderDetail](
	[order_detail_id] [varchar](50) NOT NULL,
	[order_id] [varchar](50) NULL,
	[product_id] [varchar](50) NULL,
	[quantity] [int] NULL,
	[price] [float] NULL,
	[date] [datetime] NULL,
 CONSTRAINT [PK_Tbl_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[order_detail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Permission]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Permission](
	[permission_id] [int] IDENTITY(1,1) NOT NULL,
	[user_role_id] [int] NULL,
	[permission_name] [varchar](50) NULL,
 CONSTRAINT [PK_Tbl_Permission] PRIMARY KEY CLUSTERED 
(
	[permission_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Product]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Product](
	[product_id] [varchar](50) NOT NULL,
	[name] [nvarchar](100) NULL,
	[quantity] [int] NULL,
	[description] [ntext] NULL,
	[weight] [float] NULL,
	[sold_quantity] [int] NULL,
	[is_loved] [bit] NULL,
	[status] [bit] NULL,
	[price] [float] NULL,
	[type] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_Product] PRIMARY KEY CLUSTERED 
(
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_ProductInRouting]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_ProductInRouting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] [varchar](50) NULL,
	[routing_id] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Refund]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Refund](
	[refund_id] [varchar](50) NOT NULL,
	[user_id] [int] NULL,
	[order_id] [varchar](50) NULL,
	[description] [nvarchar](200) NULL,
	[date] [datetime] NULL,
	[is_confirm] [bit] NULL,
 CONSTRAINT [PK_Tbl_Refund] PRIMARY KEY CLUSTERED 
(
	[refund_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Role]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Role](
	[user_role_id] [int] NOT NULL,
	[role_name] [nvarchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Routing]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Routing](
	[routing_id] [varchar](50) NOT NULL,
	[name] [nvarchar](100) NULL,
	[description] [nvarchar](max) NULL,
	[quantity] [int] NULL,
	[price] [float] NULL,
	[status] [bit] NULL,
 CONSTRAINT [PK_Tbl_Routing] PRIMARY KEY CLUSTERED 
(
	[routing_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_User]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_User](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](100) NULL,
	[password] [varchar](20) NULL,
	[is_confirm] [bit] NULL,
	[firstname] [nvarchar](50) NULL,
	[lastname] [nvarchar](50) NULL,
	[numberphone] [varchar](10) NULL,
	[address] [nvarchar](150) NULL,
	[date_create] [datetime] NULL,
	[last_activitty] [datetime] NULL,
	[point] [float] NULL,
	[status] [bit] NULL,
	[user_role_id] [int] NULL,
 CONSTRAINT [PK_Tbl_User] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_VoucherCode]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_VoucherCode](
	[voucher_id] [varchar](50) NOT NULL,
	[voucher_code] [varchar](10) NULL,
	[value] [float] NULL,
	[quantity] [int] NULL,
	[used] [int] NULL,
	[status] [bit] NULL,
 CONSTRAINT [PK_Tbl_VoucherCode] PRIMARY KEY CLUSTERED 
(
	[voucher_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_VoucherUsed]    Script Date: 8/2/2023 12:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_VoucherUsed](
	[id] [int] NOT NULL,
	[voucher_id] [varchar](50) NULL,
	[user_id] [int] NULL,
 CONSTRAINT [PK_Tbl_VoucherUsed] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tbl_Address]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Tbl_User] ([user_id])
GO
ALTER TABLE [dbo].[Tbl_FavouriteList]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_FavouriteList_Tbl_Product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Tbl_Product] ([product_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tbl_FavouriteList] CHECK CONSTRAINT [FK_Tbl_FavouriteList_Tbl_Product]
GO
ALTER TABLE [dbo].[Tbl_FavouriteList]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_FavouriteList_Tbl_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[Tbl_User] ([user_id])
GO
ALTER TABLE [dbo].[Tbl_FavouriteList] CHECK CONSTRAINT [FK_Tbl_FavouriteList_Tbl_User]
GO
ALTER TABLE [dbo].[Tbl_Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Feedback_Tbl_Product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Tbl_Product] ([product_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tbl_Feedback] CHECK CONSTRAINT [FK_Tbl_Feedback_Tbl_Product]
GO
ALTER TABLE [dbo].[Tbl_Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Feedback_Tbl_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[Tbl_User] ([user_id])
GO
ALTER TABLE [dbo].[Tbl_Feedback] CHECK CONSTRAINT [FK_Tbl_Feedback_Tbl_User]
GO
ALTER TABLE [dbo].[Tbl_Notify]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Notify_Tbl_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[Tbl_User] ([user_id])
GO
ALTER TABLE [dbo].[Tbl_Notify] CHECK CONSTRAINT [FK_Tbl_Notify_Tbl_User]
GO
ALTER TABLE [dbo].[Tbl_OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_OrderDetail_Tbl_Order] FOREIGN KEY([order_id])
REFERENCES [dbo].[Tbl_Order] ([order_id])
GO
ALTER TABLE [dbo].[Tbl_OrderDetail] CHECK CONSTRAINT [FK_Tbl_OrderDetail_Tbl_Order]
GO
ALTER TABLE [dbo].[Tbl_OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_OrderDetail_Tbl_Product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Tbl_Product] ([product_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tbl_OrderDetail] CHECK CONSTRAINT [FK_Tbl_OrderDetail_Tbl_Product]
GO
ALTER TABLE [dbo].[Tbl_Refund]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Refund_Tbl_Order] FOREIGN KEY([order_id])
REFERENCES [dbo].[Tbl_Order] ([order_id])
GO
ALTER TABLE [dbo].[Tbl_Refund] CHECK CONSTRAINT [FK_Tbl_Refund_Tbl_Order]
GO
ALTER TABLE [dbo].[Tbl_Refund]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Refund_Tbl_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[Tbl_User] ([user_id])
GO
ALTER TABLE [dbo].[Tbl_Refund] CHECK CONSTRAINT [FK_Tbl_Refund_Tbl_User]
GO
ALTER TABLE [dbo].[Tbl_Role]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Role_Tbl_Permission] FOREIGN KEY([user_role_id])
REFERENCES [dbo].[Tbl_Permission] ([permission_id])
GO
ALTER TABLE [dbo].[Tbl_Role] CHECK CONSTRAINT [FK_Tbl_Role_Tbl_Permission]
GO
ALTER TABLE [dbo].[Tbl_Role]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Role_Tbl_User] FOREIGN KEY([user_role_id])
REFERENCES [dbo].[Tbl_User] ([user_id])
GO
ALTER TABLE [dbo].[Tbl_Role] CHECK CONSTRAINT [FK_Tbl_Role_Tbl_User]
GO
