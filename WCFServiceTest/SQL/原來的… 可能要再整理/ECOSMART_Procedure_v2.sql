USE [ECOSMART]
GO

/****** Object:  StoredProcedure [dbo].[ReadMeterTree]    Script Date: 2016/1/15 下午 11:07:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


		Create PROCEDURE [dbo].[ReadMeterTree] 
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
						 and CS.ECO_Account = MS.ECO_Account 
						 Order by CtrlNr
					  end
				   else
					  begin
						 Select CS.Account as Account, CS.InstallPosition AS ECO_Position, CS.Enabled as ECO_Enabled, 
								MS.ECO_Account, MS.DrawNr, MS.CtrlNr, MS.MeterID, MS.InstallPosition, MS.LineNum, MS.Enabled, MS.MeterType, MS.RecDate, MS.Iavg, MS.Vavg, MS.W
							   ,DateDiff(n, MS.recdate , ( CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 111) + ' '  + CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 108))) as UpLoadStatus 
						 From ControllerSetup as CS, MeterSetup as MS 
						 Where (MS.LineNum is not NULL or MS.LineNum <> '' or MS.Enabled=1) and CS.Account = @sAccount  and CS.ECO_Account = MS.ECO_Account 
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
						 and CS.ECO_Account = MS.ECO_Account 
						 Order by CtrlNr
					  end
				   else
					  begin
						 Select CS.Account as Account, CS.InstallPosition AS ECO_Position, CS.Enabled as ECO_Enabled, 
								MS.ECO_Account, MS.DrawNr, MS.CtrlNr, MS.MeterID, MS.InstallPosition,(Right('00'+ cast(MS.CtrlNr as nvarchar(2)),2) + Right('00' +cast(MS.MeterID as nvarchar(2)),2)) as LineNum, MS.Enabled, MS.MeterType, MS.RecDate, MS.Iavg, MS.Vavg, MS.W
							   ,DateDiff(n, MS.recdate , ( CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 111) + ' '  + CONVERT(NVARCHAR, dateAdd(hh,ISNULL(CS.DiffTime ,0),getdate()), 108))) as UpLoadStatus 
						 From ControllerSetup as CS, MeterSetup as MS 
						 Where (MS.LineNum is NULL and MS.LineNum = '' or MS.Enabled=0) and CS.Account = @sAccount  and CS.ECO_Account = MS.ECO_Account 
						 order by LineNum
					  end  
                  End



           END



GO


