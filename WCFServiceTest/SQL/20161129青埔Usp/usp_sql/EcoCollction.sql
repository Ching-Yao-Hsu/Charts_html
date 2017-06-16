USE [ECO_secom1]
GO

/****** Object:  StoredProcedure [dbo].[Usp_ECOCollection]    Script Date: 2016/11/29 下午 05:00:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Usp_ECOCollection]
    @sName         nvarchar(10),
    @sRecDate         nvarchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @strDate nVarchar(10)
	
	declare @CtrlNr  int
	declare @MeterID int
	declare @name    nVarchar(100)   /*資料庫名稱*/
	declare @strSQL  nVarchar(200)   /*指令*/
	
	set @name = Replace(@sName,'ECO_','');
	print @name;
	DECLARE Cursor_tmp1 CURSOR FOR   /*建立Cursor*/
            (select B.CtrlNr as CtrlNr , B.MeterID as MeterID From ECOSMART.[dbo].[ControllerSetup] A  with (Nolock) 
             Left join ECOSMART.[dbo].[MeterSetup] B  with (Nolock) on B.ECO_Account=A.ECO_Account and B.Enabled=1 Where A.Account=@name)

	OPEN Cursor_tmp1              /*開啟Cursor*/
    FETCH NEXT FROM Cursor_tmp1 INTO  @CtrlNr, @MeterID    /*將值放入變數*/
	WHILE @@FETCH_STATUS = 0     /*有回傳值*/
    BEGIN
	
	   print '['+@sName+'].[dbo].[Usp_LostDayCheck]';
	   
	   --exec ('[dbo].[Usp_LostDayCheck]');

	   set @strSQL = '[dbo].[Usp_TriggerDayRecordCollection] '+ ' ' + rtrim(cast(@CtrlNr as char)) + ',' + rtrim(cast(@MeterID as char)) + ',''' + @sRecDate+'''';
	   print @strSQL;
	   
	   exec (@strSQL);  

	   FETCH NEXT FROM Cursor_tmp1 INTO  @CtrlNr, @MeterID  /*將值放入變數	*/
	   --print @@FETCH_STATUS;
    END
    CLOSE Cursor_tmp1         /*關閉Cursor*/
    DEALLOCATE Cursor_tmp1    /*釋放Cursor*/

END
















GO


