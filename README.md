# RappidPayTest

SCRIPT TO GENERATE THE DATABASE

USE [master]
CREATE DATABASE [RapidPayDataBase]
 
USE [RapidPayDataBase]
GO

CREATE TABLE [dbo].[CardManagement](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [char](1) NULL,
	[IsDeleted] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateAt] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UpdateAt] [datetime] NULL,
	[CardNumber] [int] NULL,
	[Balance] [decimal](18, 0) NULL,
	[IDUser] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 31/10/2023 12:14:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 31/10/2023 12:14:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [char](1) NULL,
	[IsDeleted] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateAt] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UpdateAt] [datetime] NULL,
	[Email] [varchar](100) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[Password] [varchar](250) NOT NULL,
	[PasswordKey] [varchar](250) NOT NULL,
	[ContractValidation] [bit] NULL,
	[LastAccess] [datetime] NULL,
	[Gender] [bit] NULL,
	[Phone] [varchar](100) NULL,
	[Estate] [varchar](100) NULL,
	[IDRole] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CardManagement] ADD  CONSTRAINT [DF_CardManagement_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD FOREIGN KEY([IDRole])
REFERENCES [dbo].[Role] ([ID])
GO
USE [master]
GO
ALTER DATABASE [RapidPayDataBase] SET  READ_WRITE 
GO
