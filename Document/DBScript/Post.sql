USE [C:\ART\PROJECT\PERSONAL\MSGBOARD\MSGBOARD\APP_DATA\MSG.MDF]
GO

/****** Object:  Table [dbo].[Post]    Script Date: 2018/8/15 下午 12:37:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Post](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](2000) NOT NULL,
	[IsDel] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateUserId] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NOT NULL,
 CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Post] ADD  CONSTRAINT [DF_Post_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

ALTER TABLE [dbo].[Post] ADD  CONSTRAINT [DF_Post_UpdateTime]  DEFAULT (getdate()) FOR [UpdateTime]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'autoNumber' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Post', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章內容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Post', @level2type=N'COLUMN',@level2name=N'Content'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否刪除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Post', @level2type=N'COLUMN',@level2name=N'IsDel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Post', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Post', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Post', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Post', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主要文章' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Post'
GO


