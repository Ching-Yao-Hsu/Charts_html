USE [master]
GO
/****** Object:  StoredProcedure [dbo].[Usp_RunCollection01]    Script Date: 2016/11/29 下午 03:43:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE  PROCEDURE [dbo].[Usp_RunCollection01]
    @sRecDate         nvarchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @strDate nVarchar(10)
	
	declare @CtrlNr  int
	declare @MeterID int
	declare @name    nVarchar(100)   /*資料庫名稱*/
	declare @strSQL  nVarchar(200)   /*指令*/
	
	if @sRecDate <> '' 
	   Begin	
	      set @strDate = @sRecDate;
	   End 
	Else
	   Begin 
	      set @strDate = Replace(CAST(cast(DATEADD(day,-1,GetDate()) as date) AS CHAR),'-','/');
	   End
	print @strDate;

	DECLARE Cursor_tmp CURSOR FOR   /*建立Cursor*/
            (select name as name from [sys].[databases] wiht (Nolock)
             where Left(name,4) = 'ECO_' and state_desc='ONLINE' )

	OPEN Cursor_tmp              /*開啟Cursor*/
    FETCH NEXT FROM Cursor_tmp INTO @name    /*將值放入變數*/
	WHILE @@FETCH_STATUS = 0     /*有回傳值*/
    BEGIN
	
	   --print '['+@name+'].[dbo].[Usp_LostDayCheck]';
	   --exec ('['+@name+'].[dbo].[Usp_LostDayCheck]');

	   set @strSQL = '[' + @name + ']' + '.[dbo].[Usp_ECOCollection] '+ '''' + @name+''''+ ',''' + @strDate+'''';
	   print @strSQL;	   
	   exec (@strSQL); 

	   FETCH NEXT FROM Cursor_tmp INTO @name  /*將值放入變數	*/
	   print @@FETCH_STATUS;
    END

	print 'End';
    CLOSE Cursor_tmp         /*關閉Cursor*/
    DEALLOCATE Cursor_tmp    /*釋放Cursor*/

END











