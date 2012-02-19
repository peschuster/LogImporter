SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[wpservicesLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LogFilename] [varchar](255) NULL,
	[LogRow] [int] NULL,
	[TimeStamp] [datetime] NULL,
	[cIp] [varchar](255) NULL,
	[csUsername] [varchar](255) NULL,
	[sSitename] [varchar](255) NULL,
	[sComputername] [varchar](255) NULL,
	[sIp] [varchar](255) NULL,
	[sPort] [int] NULL,
	[csMethod] [varchar](255) NULL,
	[csUriStem] [varchar](255) NULL,
	[csUriQuery] [varchar](255) NULL,
	[scStatus] [int] NULL,
	[scSubstatus] [int] NULL,
	[scWin32Status] [int] NULL,
	[scBytes] [int] NULL,
	[csBytes] [int] NULL,
	[timeTaken] [int] NULL,
	[csVersion] [varchar](255) NULL,
	[csHost] [varchar](255) NULL,
	[csUserAgent] [varchar](255) NULL,
	[csCookie] [varchar](255) NULL,
	[csReferer] [varchar](255) NULL,
	[sEvent] [varchar](255) NULL,
	[sProcessType] [varchar](255) NULL,
	[sUserTime] [real] NULL,
	[sKernelTime] [real] NULL,
	[sPageFaults] [int] NULL,
	[sTotalProcs] [int] NULL,
	[sActiveProcs] [int] NULL,
	[sStoppedProcs] [int] NULL,
	[CountryName] [nvarchar](255) NULL,
	[CountryCode] [char](2) NULL
 CONSTRAINT [PK_wpservicesLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO