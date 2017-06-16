USE [ECO_N_AC_2016]
GO

/****** Object:  StoredProcedure [dbo].[Usp_BatchRun]    Script Date: 2016/8/6 下午 10:38:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_BatchRun]
AS
BEGIN
SET NOCOUNT ON;
declare @CtrlNr int;declare @MeterID int;declare @RecDate nVarchar(10);declare @RecDateS nVarchar(10);declare @RecDateE nVarchar(10);
declare @iCount int;declare @Err int;set @RecDateS = '';set @RecDateE = '';
DECLARE Cursor_tmp CURSOR FOR (SELECT DISTINCT CtrLNr , MeterID, rtrim(CAST(cast(RecDate as date) AS CHAR)) as RecDate FROM PowerRecord)
OPEN Cursor_tmp
FETCH NEXT FROM Cursor_tmp INTO @CtrlNr,@MeterID,@RecDate
WHILE @@FETCH_STATUS = 0
BEGIN
if @RecDateS > @RecDate or @RecDateS ='' Begin set @RecDateS = @RecDate end
else if @RecDateE < @RecDate Begin set @RecDateE = @RecDate end 
set @RecDate = Replace(@RecDate,'-','/')
exec Usp_Day_Record @CtrlNr,@MeterID,@RecDate
FETCH NEXT FROM Cursor_tmp INTO @CtrlNr,@MeterID,@RecDate
END
CLOSE Cursor_tmp
DEALLOCATE Cursor_tmp
Select * From PowerRecordCollection Where RecDate Between @RecDateS and @RecDateE Order by CtrlNr, MeterID, RecDate Desc
print rtrim(cast(@RecDateS as char)) + '至' + rtrim(cast(@RecDatee as char)) + '，共執行' + rtrim(cast(@@RowCount as char)) + '筆'
END

GO


