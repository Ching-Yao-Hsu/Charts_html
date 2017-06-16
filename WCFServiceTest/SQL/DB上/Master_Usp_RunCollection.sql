USE [master]
GO

/****** Object:  StoredProcedure [dbo].[Usp_RunCollection]    Script Date: 2016/8/6 �U�� 10:43:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Usp_RunCollection]
    @sRecDate         nvarchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @strDate nVarchar(10)
	
	declare @CtrlNr  int
	declare @MeterID int
	declare @name    nVarchar(100)   /*��Ʈw�W��*/
	declare @strSQL  nVarchar(200)   /*���O*/
	
	if @sRecDate <> '' 
	   Begin	
	      set @strDate = @sRecDate
	   End 
	Else
	   Begin 
	      set @strDate = Replace(CAST(cast(DATEADD(day,-1,GetDate()) as date) AS CHAR),'-','/')
	   End
	

	DECLARE Cursor_tmp CURSOR FOR   /*�إ�Cursor*/
            (select A.name as name , C.CtrlNr as CtrlNr , C.MeterID as MeterID from [sys].[databases] A
             Left join ECOSMART.[dbo].[ControllerSetup] B on B.Account=Substring(name,5,len(name)-4)
             Left join ECOSMART.[dbo].[MeterSetup] C on C.ECO_Account=B.ECO_Account 
             where Left(name,4) = 'ECO_' and state_desc='ONLINE' and C.Enabled='1' )

	OPEN Cursor_tmp              /*�}��Cursor*/
    FETCH NEXT FROM Cursor_tmp INTO @name, @CtrlNr, @MeterID    /*�N�ȩ�J�ܼ�*/
	WHILE @@FETCH_STATUS = 0     /*���^�ǭ�*/
    BEGIN

	   /*print '['+@name+'].[dbo].[Usp_LostDayCheck]';*/
	   exec ('['+@name+'].[dbo].[Usp_LostDayCheck]');

	   set @strSQL = '[' + @name + ']' + '.[dbo].[Usp_TriggerDayRecordCollection] '+ ' ' + rtrim(cast(@CtrlNr as char)) + ',' + rtrim(cast(@MeterID as char)) + ',''' + @strDate+'''';
	   /*print @strSQL;*/

	   exec (@strSQL);

	   FETCH NEXT FROM Cursor_tmp INTO @name, @CtrlNr, @MeterID  /*�N�ȩ�J�ܼ�	*/
    END

    CLOSE Cursor_tmp         /*����Cursor*/
    DEALLOCATE Cursor_tmp    /*����Cursor*/

END


GO


