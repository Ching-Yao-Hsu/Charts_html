USE [ECO_N_AC_2016]
GO

/****** Object:  StoredProcedure [dbo].[Usp_LostDayCheck]    Script Date: 2016/8/6 ¤U¤È 10:39:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_LostDayCheck]
@date1 varchar(10),@date2 varchar(10)
AS
BEGIN
SET NOCOUNT ON;
Create Table #tmp1 (CtrlNr int not null, MeterID int not null, RecDate varchar(10) not null)
Create Table #tmp2 (CtrlNr int not null, MeterID int not null, RecDate varchar(10) not null)
Create Table #tmp3 (CtrlNr int not null, MeterID int not null, RecDate varchar(10) not null)
insert into #tmp1 select CtrlNr,MeterID,RecDate from PowerRecord where RecDate between @date1 and @date2 Group by CtrlNr, MeterID, RecDate
insert into #tmp2 select CtrlNr,MeterID,RecDate from PowerRecordCollection where RecDate between @date1 and @date2 Group by CtrlNr, MeterID, RecDate
insert into #tmp3 select #tmp1.CtrlNr,#tmp1.MeterID,#tmp1.RecDate  from #tmp1 left join #tmp2 ON #tmp1.CtrlNr=#tmp2.CtrlNr and #tmp1.MeterID=#tmp2.MeterID and #tmp1.RecDate = #tmp2.RecDate where #tmp2.CtrlNr is null
DECLARE @CtrlNr int; DECLARE @MeterID int; DECLARE @RecDate varchar(10);
declare addr_cursor scroll cursor for select CtrlNr,MeterID,RecDate from #tmp3 
OPEN addr_cursor
FETCH NEXT FROM addr_cursor INTO @CtrlNr, @MeterID, @RecDate 
WHILE @@FETCH_STATUS = 0
BEGIN
exec Usp_Day_Record @CtrlNr,@MeterID, @RecDate
FETCH NEXT FROM addr_cursor INTO @CtrlNr, @MeterID, @RecDate
END
CLOSE addr_cursor
DEALLOCATE addr_cursor 
END

GO


