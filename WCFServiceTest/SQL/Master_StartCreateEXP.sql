Create Database [ECOSMART]
$

CREATE PROCEDURE [dbo].[Usp_CreateDB]
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
                       CONSTRAINT [PK_PowerRecord] PRIMARY KEY CLUSTERED 
                      (
	                      [CtrlNr] ASC,
	                      [MeterID] ASC,
	                      [RecDate] DESC,
	                      [RecTime] DESC
                      )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                      ) ON [PRIMARY]'

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
        	CONSTRAINT [PK_PowerRecordCollection] PRIMARY KEY CLUSTERED 
       	        (
       		[CtrlNr] ASC,
       		[MeterID] ASC,
       		[RecDate] DESC
       	        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
       	        ) ON [PRIMARY]');
		   
       declare @Name nvarchar(50)
       set @sqlstatement = 'Declare Cursor_tmp CURSOR FOR (Select name from sys.databases where name like (''ECO_'+@sAccount+'_%''))'

       exec sp_executesql @sqlstatement     /*建立Cursor*/

	   set @sqlstatement = '';
	   set @sqlstatement01 = '';


	   Exec ('DROP VIEW [dbo].[ECO_'+@sAccount+'_PowerRecord]');
	   Exec ('DROP VIEW [dbo].[ECO_'+@sAccount+'_PowerRecordCollection]');


       OPEN Cursor_tmp
       FETCH NEXT FROM Cursor_tmp INTO @Name
       WHILE @@FETCH_STATUS = 0
       BEGIN
		  if @sqlstatement <> '' 
		     Begin
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
		     set @sqlstatement = 'CREATE VIEW [dbo].[ECO_'+@sAccount+'_PowerRecord] AS ' + @sqlstatement;
		     set @sqlstatement01 = 'CREATE VIEW [dbo].[ECO_'+@sAccount+'_PowerRecordCollection] AS ' + @sqlstatement01;
			 --print @sqlstatement;
			 exec sp_executesql @sqlstatement;
			 exec sp_executesql @sqlstatement01;
		  End

END
$



CREATE PROCEDURE [dbo].[Usp_RunCollection]
    @sRecDate         nvarchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @strDate nVarchar(10)
	declare @strRunYear nVarchar(4)
	
	declare @CtrlNr  int
	declare @MeterID int
	declare @name    nVarchar(100)   /*資料庫名稱*/
	declare @strSQL  nVarchar(200)   /*指令*/
	declare @Count int =0

	if @sRecDate <> '' 
	   Begin	
	      set @strDate = @sRecDate
	   End 
	Else
	   Begin 
	      set @strDate = Replace(CAST(cast(DATEADD(day,-1,GetDate()) as date) AS CHAR),'-','/')
	   End
	   
	set @strRunYear = Substring( @strDate,1,4);
	
	delete from [ECOSMART].[dbo].[DBRecord];

	insert into [ECOSMART].[dbo].[DBRecord]
	select A.name as name , C.CtrlNr as CtrlNr , C.MeterID as MeterID from [sys].[databases] A with (Nolock)
             join ECOSMART.[dbo].[ControllerSetup] B with (Nolock) on CHARINDEX(B.Account, name) > 0 and len(B.Account) <> (len(name)-4) and Substring(name, (len(name)-3), len(name))= substring(@strDate,1,4)
             join ECOSMART.[dbo].[MeterSetup] C with (Nolock) on C.ECO_Account=B.ECO_Account 
             where state_desc='ONLINE' and C.Enabled='1';
	
	DECLARE Cursor_tmp CURSOR FOR   /*建立Cursor*/
            Select name, CtrlNr, MeterID from [ECOSMART].[dbo].[DBRecord];
	OPEN Cursor_tmp              /*開啟Cursor*/

    FETCH NEXT FROM Cursor_tmp INTO @name, @CtrlNr, @MeterID    /*將值放入變數*/
	WHILE @@FETCH_STATUS = 0    --@Count < (@@CURSOR_rows+1)    /*有回傳值*/
    BEGIN
		set @strSQL = '[' + @name + ']' + '.[dbo].[Usp_TriggerDayRecordCollection] '+ ' ' + rtrim(cast(@CtrlNr as char)) + ',' + rtrim(cast(@MeterID as char)) + ',''' + @strDate+'''';
		exec (@strSQL);
	   
		/*print '['+@name+'].[dbo].[Usp_LostDayCheck]';*/
		--exec ('['+@name+'].[dbo].[Usp_LostDayCheck01]');

		FETCH NEXT FROM Cursor_tmp INTO @name, @CtrlNr, @MeterID  /*將值放入變數	*/
    END
    CLOSE Cursor_tmp         /*關閉Cursor*/
    DEALLOCATE Cursor_tmp    /*釋放Cursor*/
END
$


/*開始新增資料表*/
USE [ECOSMART]
$
CREATE TABLE [dbo].[AdminCom](
	[Account] [nvarchar](20) NOT NULL,
	[Com] [nvarchar](20) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[EnabledTime] [datetime] NOT NULL,
 CONSTRAINT [PK_AdminCom] PRIMARY KEY CLUSTERED 
(
	[Account] ASC,
	[Com] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

$

ALTER TABLE [dbo].[AdminCom] ADD  CONSTRAINT [DF_AdminCom_EnabledTime]  DEFAULT (getdate()) FOR [EnabledTime]


$
CREATE TABLE [dbo].[AdminSetup](
	[Account] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[ECO_Group] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](20) NULL,
	[Company] [nvarchar](20) NOT NULL,
	[Address] [nvarchar](50) NULL,
	[Tel] [nvarchar](20) NULL,
	[Mobile] [nvarchar](20) NULL,
	[E_Mail] [nvarchar](30) NOT NULL,
	[Rank] [int] NOT NULL,
	[GUID] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[EnabledTime] [datetime] NOT NULL,
	[CreateDB] [bit] NOT NULL,
	[SumDayMailSent] [bit] NULL,
	[SumMonthMailSent] [bit] NULL,
	[SumDayMail] [nvarchar](100) NULL,
	[SumMonthMail] [nvarchar](100) NULL,
 CONSTRAINT [PK_AdminSetup] PRIMARY KEY CLUSTERED 
(
	[Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

$

ALTER TABLE [dbo].[AdminSetup] ADD  CONSTRAINT [DF_AdminSetup_GUID]  DEFAULT (newid()) FOR [GUID]
$

ALTER TABLE [dbo].[AdminSetup] ADD  CONSTRAINT [DF_AdminSetup_EnabledTime]  DEFAULT (getdate()) FOR [EnabledTime]
$


CREATE TABLE [dbo].[ControllerSetup](
	[Account] [nvarchar](20) NOT NULL,
	[ECO_Account] [nvarchar](20) NOT NULL,
	[ECO_Password] [nvarchar](20) NOT NULL,
	[ECO_Type] [nvarchar](20) NOT NULL,
	[CtrlNr] [int] NOT NULL,
	[DrawNr] [nvarchar](30) NULL,
	[InstallPosition] [nvarchar](30) NULL,
	[IP_Address] [nvarchar](30) NULL,
	[Enabled] [bit] NOT NULL,
	[SMSSentEnabled] [bit] NOT NULL,
	[EmailSentEnabled] [bit] NOT NULL,
	[Num] [nvarchar](100) NULL,
	[Mail] [nvarchar](100) NULL,
	[DayMailSentEnabled] [bit] NOT NULL,
	[WeekMailSnetEnabled] [bit] NULL,
	[MonthMailSentEnabled] [bit] NOT NULL,
	[DayMail] [nvarchar](100) NULL,
	[WeekMail] [nvarchar](100) NULL,
	[MonthMail] [nvarchar](100) NULL,
	[DiffTime] [int] NULL,
 CONSTRAINT [PK_ControllerSetup] PRIMARY KEY CLUSTERED 
(
	[ECO_Account] ASC,
	[CtrlNr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

$

ALTER TABLE [dbo].[ControllerSetup]  WITH CHECK ADD  CONSTRAINT [FK_ControllerSetup_AdminSetup] FOREIGN KEY([Account])
REFERENCES [dbo].[AdminSetup] ([Account])
ON UPDATE CASCADE
$

ALTER TABLE [dbo].[ControllerSetup] CHECK CONSTRAINT [FK_ControllerSetup_AdminSetup]
$

CREATE TABLE [dbo].[EventRecord](
	[EventIndex] [int] IDENTITY(1,1) NOT NULL,
	[ECO_Account] [nvarchar](15) NULL,
	[MeterID] [int] NULL,
	[RecDate] [nvarchar](20) NOT NULL,
	[InstallPosition] [nvarchar](30) NULL,
	[ErrType] [int] NULL,
	[ErrContent] [nvarchar](100) NULL,
	[SMSSent] [bit] NULL,
	[EMailSent] [bit] NULL,
	[Num] [nvarchar](100) NULL,
	[Mail] [nvarchar](100) NULL,
 CONSTRAINT [PK_EventRecord] PRIMARY KEY CLUSTERED 
(
	[EventIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
$

CREATE TABLE [dbo].[MailSetup](
	[SmtpServer] [nvarchar](30) NULL,
	[MailServer_Account] [nvarchar](20) NULL,
	[MailServer_Password] [nvarchar](20) NULL,
	[MailAddress] [nvarchar](30) NULL,
	[MailName] [nvarchar](20) NULL,
	[Bcc] [nvarchar](30) NULL
) ON [PRIMARY]
$

CREATE TABLE [dbo].[MeterSetup](
	[ECO_Account] [nvarchar](20) NOT NULL,
	[CtrlNr] [int] NOT NULL,
	[MeterID] [int] NOT NULL,
	[DrawNr] [nvarchar](30) NULL,
	[InstallPosition] [nvarchar](30) NOT NULL,
	[LineNum] [nvarchar](100) NULL,
	[Enabled] [bit] NOT NULL,
	[MeterType] [int] NULL,
	[FloorArea] [float] NULL,
	[StaffNum] [int] NULL,
	[CommState] [tinyint] NULL,
	[RecDate] [varchar](20) NULL,
	[Iavg] [float] NULL,
	[Vavg] [float] NULL,
	[W] [float] NULL,
 CONSTRAINT [PK_MeterSetup] PRIMARY KEY CLUSTERED 
(
	[ECO_Account] ASC,
	[CtrlNr] ASC,
	[MeterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

$

SET ANSI_PADDING OFF
$

ALTER TABLE [dbo].[MeterSetup]  WITH CHECK ADD  CONSTRAINT [FK_MeterSetup_ControllerSetup] FOREIGN KEY([ECO_Account], [CtrlNr])
REFERENCES [dbo].[ControllerSetup] ([ECO_Account], [CtrlNr])
ON DELETE CASCADE
$

ALTER TABLE [dbo].[MeterSetup] CHECK CONSTRAINT [FK_MeterSetup_ControllerSetup]
$

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'控制器帳號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeterSetup', @level2type=N'COLUMN',@level2name=N'ECO_Account'
$

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'控制器編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeterSetup', @level2type=N'COLUMN',@level2name=N'CtrlNr'
$

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'電表ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeterSetup', @level2type=N'COLUMN',@level2name=N'MeterID'
$

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否啟用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeterSetup', @level2type=N'COLUMN',@level2name=N'Enabled'
$

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'電表通訊狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeterSetup', @level2type=N'COLUMN',@level2name=N'CommState'
$


CREATE TABLE [dbo].[PowerRecord](
	[CtrlNr] [int] NOT NULL,
	[MeterID] [int] NOT NULL,
	[RecDate] [nvarchar](20) NOT NULL,
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
	[Demand] [int] NULL,
	[DemandHalf] [int] NULL,
	[DemandSatHalf] [int] NULL,
	[DemandOff] [int] NULL,
	[RushHour] [int] NULL,
	[HalfHour] [int] NULL,
	[SatHalfHour] [int] NULL,
	[OffHour] [int] NULL,
	[State] [nvarchar](100) NULL
) ON [PRIMARY]
$

CREATE TABLE [dbo].[DBRecord](
	[Name] [varchar](30) NOT NULL,
	[CtrlNr] [int] NOT NULL,
	[MeterID] [int] NOT NULL,
 CONSTRAINT [PK_DBRecord] PRIMARY KEY CLUSTERED 
(
	[Name] ASC,
	[CtrlNr] ASC,
	[MeterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

$
CREATE TRIGGER [dbo].[InsertMeter] 
   ON  [dbo].[ControllerSetup] 
   AFTER INSERT
AS 

declare @Account NVARCHAR (20)
set @Account = (select ECO_Account from inserted)
declare @EcoType NVARCHAR (20)
set @EcoType = (select ECO_Type from inserted)
declare @ctrlnr int
set @ctrlnr = (select CtrlNr from inserted)
declare @enabled int
set @enabled = (select Enabled from inserted)

if @enabled = 1
Begin
	if @EcoType = 'ECO5'
	Begin
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,1 ,'預設位置1' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,2 ,'預設位置2' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,3 ,'預設位置3' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,4 ,'預設位置4' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,5 ,'預設位置5' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,6 ,'預設位置6' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,7 ,'預設位置7' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,8 ,'預設位置8' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,9 ,'預設位置9' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,10 ,'預設位置10' ,0 ,7)
	END
	if @EcoType = 'ECO5_Lite'
	Begin
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,1 ,'預設位置1' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,2 ,'預設位置2' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,3 ,'預設位置3' ,0 ,7)
		insert into MeterSetup (ECO_Account,CtrlNr,MeterID,InstallPosition,Enabled,MeterType)
		values(@Account, @ctrlnr ,4 ,'預設位置4' ,0 ,7)
	END
END

$

		CREATE PROCEDURE [dbo].[ReadMeterTree] 
               @sKind           int,            /*0表未啟用, 1表啟用*/
               @sAccount        nvarchar(20),
               @sCtrlNr         int,
               @sMeterID        int
           AS
           BEGIN
               SET NOCOUNT ON;	
		
               declare @sStr   nvarchar(100)
			   IF (@sKind = 1)
			      Begin
				   if (@sCtrlNr <> '' and @sMeterID <> '')
					  begin
						 Select CS.Account as Account, CS.InstallPosition AS ECO_Position, CS.Enabled as ECO_Enabled, 
						 MS.ECO_Account, MS.DrawNr, MS.CtrlNr, MS.MeterID, MS.InstallPosition, MS.LineNum, MS.Enabled, MS.MeterType, MS.RecDate, MS.Iavg, MS.Vavg, MS.W
						 ,DateDiff(n, MS.recdate , ( CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 111) + ' '  + CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 108))) as UpLoadStatus 
						 From ControllerSetup as CS, MeterSetup as MS 
						 Where (MS.LineNum is NULL or MS.LineNum <> '' or MS.Enabled=1) and CS.Account = @sAccount 
						 and MS.CtrlNr = @sCtrlNr and MS.MeterID = @sMeterID
						 and CS.ECO_Account = MS.ECO_Account and CS.CtrlNr = MS.CtrlNr 
						 Order by CtrlNr
					  end
				   else
					  begin
						 Select CS.Account as Account, CS.InstallPosition AS ECO_Position, CS.Enabled as ECO_Enabled, 
						 MS.ECO_Account, MS.DrawNr, MS.CtrlNr, MS.MeterID, MS.InstallPosition, MS.LineNum, MS.Enabled, MS.MeterType, MS.RecDate, MS.Iavg, MS.Vavg, MS.W
						 ,DateDiff(n, MS.recdate , ( CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 111) + ' '  + CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 108))) as UpLoadStatus 
						 From ControllerSetup as CS, MeterSetup as MS 
						 Where (MS.LineNum is not NULL or MS.LineNum <> '' or MS.Enabled=1) and CS.Account = @sAccount  and CS.ECO_Account = MS.ECO_Account and CS.CtrlNr = MS.CtrlNr 
						 order by LineNum
					  end 
                  End
			   Else
			      Begin
				   if (@sCtrlNr <> '' and @sMeterID <> '')
					  begin
						 Select CS.Account as Account, CS.InstallPosition AS ECO_Position, CS.Enabled as ECO_Enabled, 
						 MS.ECO_Account, MS.DrawNr, MS.CtrlNr, MS.MeterID, MS.InstallPosition,(Right('00'+ cast(MS.CtrlNr as nvarchar(2)),2) + Right('00' +cast(MS.MeterID as nvarchar(2)),2)) as LineNum, MS.Enabled, MS.MeterType, MS.RecDate, MS.Iavg, MS.Vavg, MS.W
						 ,DateDiff(n, MS.recdate , ( CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 111) + ' '  + CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 108))) as UpLoadStatus 
						 From ControllerSetup as CS, MeterSetup as MS 
						 Where (MS.LineNum is NULL or MS.LineNum = '' or MS.Enabled=0) and CS.Account = @sAccount  
						 and MS.CtrlNr = @sCtrlNr and MS.MeterID = @sMeterID
						 and CS.ECO_Account = MS.ECO_Account and CS.CtrlNr = MS.CtrlNr 
						 Order by CtrlNr
					  end
				   else
					  begin
						 Select CS.Account as Account, CS.InstallPosition AS ECO_Position, CS.Enabled as ECO_Enabled, 
						 MS.ECO_Account, MS.DrawNr, MS.CtrlNr, MS.MeterID, MS.InstallPosition,(Right('00'+ cast(MS.CtrlNr as nvarchar(2)),2) + Right('00' +cast(MS.MeterID as nvarchar(2)),2)) as LineNum, MS.Enabled, MS.MeterType, MS.RecDate, MS.Iavg, MS.Vavg, MS.W
						 ,DateDiff(n, MS.recdate , ( CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 111) + ' '  + CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 108))) as UpLoadStatus 
						 From ControllerSetup as CS, MeterSetup as MS 
						 Where (MS.LineNum is NULL and MS.LineNum = '' or MS.Enabled=0) and CS.Account = @sAccount  and CS.ECO_Account = MS.ECO_Account and CS.CtrlNr = MS.CtrlNr 
						 order by LineNum
					  end  
                  End

           END

$

INSERT INTO [dbo].[AdminSetup]
           ([Account] ,[Password] ,[ECO_Group]  ,[Name]
           ,[Company] ,[Address]  ,[Tel] ,[Mobile] ,[E_Mail]
           ,[Rank] ,[GUID] ,[Enabled]
           ,[EnabledTime] ,[CreateDB] 
		   ,[SumDayMailSent]  ,[SumMonthMailSent] ,[SumDayMail] ,[SumMonthMail])
     VALUES
           ('admin','admin','總管理','最高管理者','艾可智能科技','','03-4625590','','test@mail.com'
		   ,'2','CD89CAC7-B3F1-414E-B493-28F433AEEB2B','1','2013-03-14 13:13:03.463','1','0','0','','')

$