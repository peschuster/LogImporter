﻿IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = '$TABLENAME$'))
BEGIN
CREATE TABLE [dbo].[$TABLENAME$](
	[LogFilename] [varchar](255) NOT NULL,
	[LogRow] [int] NOT NULL,
	[TimeStamp] [datetime] NULL,
	[cIp] [varchar](255) NULL,
	[csUsername] [varchar](255) NULL,
	[sSitename] [varchar](255) NULL,
	[sComputername] [varchar](255) NULL,
	[sIp] [varchar](255) NULL,
	[sPort] [int] NULL,
	[csMethod] [varchar](255) NULL,
	[csUriStem] [varchar](2048) NULL,
	[csUriQuery] [varchar](max) NULL,
	[scStatus] [int] NULL,
	[scSubstatus] [int] NULL,
	[scWin32Status] [int] NULL,
	[scBytes] [int] NULL,
	[csBytes] [int] NULL,
	[timeTaken] [int] NULL,
	[csVersion] [varchar](255) NULL,
	[csHost] [varchar](255) NULL,
	[csUserAgent] [varchar](2048) NULL,
	[csCookie] [varchar](max) NULL,
	[csReferer] [varchar](2048) NULL,
	[sEvent] [varchar](255) NULL,
	[sProcessType] [varchar](255) NULL,
	[sUserTime] [real] NULL,
	[sKernelTime] [real] NULL,
	[sPageFaults] [int] NULL,
	[sTotalProcs] [int] NULL,
	[sActiveProcs] [int] NULL,
	[sStoppedProcs] [int] NULL,
	[CountryName] [nvarchar](255) NULL,
	[CountryCode] [char](2) NULL,
	[CleanUri] [varchar](2048) NULL,
 CONSTRAINT [PK_$TABLENAME$] PRIMARY KEY CLUSTERED ([LogFilename] ASC, [LogRow] ASC) ON [PRIMARY]
) ON [PRIMARY]
END