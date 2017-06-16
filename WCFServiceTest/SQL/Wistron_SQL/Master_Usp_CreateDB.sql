USE [master]
GO

/****** Object:  StoredProcedure [dbo].[Usp_CreateDB]    Script Date: 2016/5/4 上午 11:15:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[Usp_CreateDB]
	@sAccount nVarchar(50),
	@sYear nVarchar(4)
AS
BEGIN
	SET NOCOUNT ON;
	

	DECLARE @DBName nVarchar(50);

	set @DBName = 'ECO_' +@sAccount+'_'+@sYear;
	
	Exec ('Create Database ' + @DBName);
	
       declare @sqlstatement   nvarchar(4000);	   
       declare @sqlstatement01 nvarchar(4000);

	   set @sqlstatement ='use Master; CREATE TABLE [' + @DBName +'].[dbo].[PowerRecord](
	         [CtrlNr] [int] NOT NULL,
	         [MeterID] [int] NOT NULL,
	         [RecDate] [varchar](10) NOT NULL,
	         [RecTime] [varchar](8) NOT NULL,
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
           CONSTRAINT [PK_PowerRecord] PRIMARY KEY CLUSTERED ([CtrlNr] ASC,  [MeterID] ASC,  [RecDate] DESC, [RecTime] DESC  )
		   WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
		   ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY]'

	exec sp_executesql @sqlstatement;

	Exec ('use master; CREATE TABLE [' + @DBName +'].[dbo].[PowerRecordCollection](
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
           CONSTRAINT [PK_PowerRecordCollection] PRIMARY KEY CLUSTERED ([CtrlNr] ASC,[MeterID] ASC,[RecDate] DESC)
		   WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
		   ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]');
		   
       declare @Name nvarchar(50)
       set @sqlstatement = 'Declare Cursor_tmp CURSOR FOR (Select name from sys.databases where name like (''ECO_'+@sAccount+'_%''))'

       exec sp_executesql @sqlstatement     /*建立Cursor*/

	   set @sqlstatement = '';
	   set @sqlstatement01 = '';

       OPEN Cursor_tmp
       FETCH NEXT FROM Cursor_tmp INTO @Name
       WHILE @@FETCH_STATUS = 0
       BEGIN
		  if @sqlstatement <> '' 
		     Begin
			    Exec ('DROP VIEW [dbo].[ECO_'+@sAccount+'_PowerRecord]');
			    Exec ('DROP VIEW [dbo].[ECO_'+@sAccount+'PowerRecordCollection]');
			    set @sqlstatement = @sqlstatement + ' Union '
			    set @sqlstatement01 = @sqlstatement01 + ' Union '
			 End
          
		  set @sqlstatement = @sqlstatement + 'SELECT  * FROM ['+ @Name +'].[dbo].[PowerRecord] '
		  set @sqlstatement01 = @sqlstatement01 + 'SELECT  * FROM ['+ @Name +'].[dbo].[PowerRecordCollection] '

          FETCH NEXT FROM Cursor_tmp INTO @Name
       END

       CLOSE Cursor_tmp                 /*關閉Cursor*/
       DEALLOCATE Cursor_tmp            /*釋放Cursor*/


	   if @sqlstatement <> ''
	      Begin
		     set @sqlstatement = 'CREATE VIEW [dbo].[ECO_'+@sAccount+'PowerRecord] AS ' + @sqlstatement;
		     set @sqlstatement01 = 'CREATE VIEW [dbo].[ECO_'+@sAccount+'PowerRecordCollection] AS ' + @sqlstatement01;
			 --print @sqlstatement;
			 exec sp_executesql @sqlstatement;
			 exec sp_executesql @sqlstatement01;
		  End

END


GO


