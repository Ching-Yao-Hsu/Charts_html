USE [master]
GO

/****** Object:  StoredProcedure [dbo].[Usp_CreateDB]    Script Date: 2016/8/6 ¤U¤È 10:42:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



create PROCEDURE [dbo].[Usp_CreateDB]
	@DBName nVarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	set @DBName = 'ECO_' +@DBName;

	Exec ('Create Database ' + @DBName);
	
	Exec ('CREATE TABLE [' + @DBName +'].[dbo].[PowerRecord](
	         [CtrlNr] [int] NOT NULL,
	         [MeterID] [int] NOT NULL,
	         [RecDate] [varchar](20) NOT NULL,
	         [I1] [float] NULL,
	         [I2] [float] NULL,
	         [I3] [float] NULL,
	         [Iavg] [float] NULL,
	         [V1] [float] NULL,
	         [V2] [float] NULL,
	         [V3] [float] NULL,
	         [Vavg] [float] NULL,
	         [W] [float] NULL,
	         [V_ar] [float] NULL,
	         [VA] [float] NULL,
	         [PF] [float] NULL,
	         [KWh] [int] NULL,
	         [Mode1] [float] NULL,
	         [Mode2] [float] NULL,
	         [Mode3] [float] NULL,
	         [Mode4] [float] NULL,
	         [DeMand] [int] NULL,
	         [DeMandHalf] [int] NULL,
	         [DeMandSatHalf] [int] NULL,
	         [DeMandOff] [int] NULL,
	         [RushHour] [int] NULL,
	         [HalfHour] [int] NULL,
	         [SatHalfHour] [int] NULL,
	         [OffHour] [int] NULL,
	         [State] [varchar](4) NULL,
           CONSTRAINT [PK_PowerRecord] PRIMARY KEY CLUSTERED ([CtrlNr] ASC,  [MeterID] ASC,  [RecDate] ASC  )
		   WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
		   ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY]');

		   
	Exec ('CREATE TABLE [' + @DBName +'].[dbo].[PowerRecordCollection](
	          [CtrlNr] [int] NOT NULL,
	          [MeterID] [int] NOT NULL,
	          [RecDate] [varchar](10) NOT NULL,
	          [Iavg] [float] NULL,
	          [Imax] [float] NULL,
	          [Vavg] [float] NULL,
	          [Vmax] [float] NULL,
	          [Wavg] [float] NULL,
	          [Wmax] [float] NULL,
	          [DeMand] [int] NULL,
	          [DeMandHalf] [int] NULL,
	          [DeMandSatHalf] [int] NULL,
	          [DeMandOff] [int] NULL,
	          [RushHour] [int] NULL,
	          [HalfHour] [int] NULL,
	          [SatHalfHour] [int] NULL,
	          [OffHour] [int] NULL,
	          [RushHourMax] [int] NULL,
	          [HalfHourMax] [int] NULL,
	          [SatHalfHourMax] [int] NULL,
	          [OffHourMax] [int] NULL,
	          [KWh] [int] NULL,
	          [MonthMark] [char](2) NULL,
	          [WeekMark] [char](10) NULL,
	          [ImaxRD] [varchar](20) NULL,
	          [VmaxRD] [varchar](20) NULL,
	          [WmaxRD] [varchar](20) NULL,
	          [TodayUseKWh] [int] NULL,
	          [TotalKWh] [int] NULL,
           CONSTRAINT [PK_PowerRecordCollection] PRIMARY KEY CLUSTERED ([CtrlNr] ASC,[MeterID] ASC,[RecDate] ASC)
		   WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
		   ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]');

END

GO


