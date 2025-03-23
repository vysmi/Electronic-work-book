CREATE DATABASE WorkBooksDB;
USE [WorkBooksDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataReward](
	[IDentryReward] [int] IDENTITY(1,1) NOT NULL,
	[DateOfEntryRew] [date] NULL,
	[RewardDetails] [nvarchar](150) NULL,
	[DocEntryMadeRew] [nvarchar](150) NULL,
	[IDuser] [int] NULL,
	[IDorder] [int] NULL,
 CONSTRAINT [PK_DataReward] PRIMARY KEY CLUSTERED 
(
	[IDentryReward] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataWork](
	[IDdataWork] [int] IDENTITY(1,1) NOT NULL,
	[DateOfEntry] [date] NULL,
	[JobDetails] [nvarchar](150) NULL,
	[DocEntryMade] [nvarchar](150) NULL,
	[IDuser] [int] NULL,
	[IDorder] [int] NULL,
 CONSTRAINT [PK_DataWork] PRIMARY KEY CLUSTERED 
(
	[IDdataWork] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonInformation](
	[IDpersonInf] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Education] [nvarchar](50) NULL,
	[Profession] [nvarchar](50) NULL,
	[EDSowner] [nvarchar](50) NULL,
	[EDSinput] [nvarchar](50) NULL,
	[IDUser] [int] NULL,
	[DateOfBirthday] [date] NULL,
	[DateOfInput] [date] NULL,
 CONSTRAINT [PK_PersonInformation] PRIMARY KEY CLUSTERED 
(
	[IDpersonInf] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[IDuser] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[FirstName] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Position] [nvarchar](50) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[IDuser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DataReward]  WITH CHECK ADD  CONSTRAINT [FK_DataReward_Users] FOREIGN KEY([IDuser])
REFERENCES [dbo].[Users] ([IDuser])
GO
ALTER TABLE [dbo].[DataReward] CHECK CONSTRAINT [FK_DataReward_Users]
GO
ALTER TABLE [dbo].[DataWork]  WITH CHECK ADD  CONSTRAINT [FK_DataWork_Users] FOREIGN KEY([IDuser])
REFERENCES [dbo].[Users] ([IDuser])
GO
ALTER TABLE [dbo].[DataWork] CHECK CONSTRAINT [FK_DataWork_Users]
GO
ALTER TABLE [dbo].[PersonInformation]  WITH CHECK ADD  CONSTRAINT [FK_PersonInformation_Users] FOREIGN KEY([IDUser])
REFERENCES [dbo].[Users] ([IDuser])
GO
ALTER TABLE [dbo].[PersonInformation] CHECK CONSTRAINT [FK_PersonInformation_Users]
GO
