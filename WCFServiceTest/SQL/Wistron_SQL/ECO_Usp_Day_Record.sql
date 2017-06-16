USE [ECO_N_AC_2016]
GO

/****** Object:  StoredProcedure [dbo].[Usp_Day_Record]    Script Date: 2016/8/6 下午 10:38:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_Day_Record]
@sCtrlNr int,@sMeterID int,@sRecDate nvarchar(10)
AS
BEGIN
SET NOCOUNT ON;
declare @iavgW Float;declare @imaxW Float;declare @dmaxWRD nvarchar(20);
declare @imaxV Float;declare @iavgV Float;declare @dmaxVRD nvarchar(20);
declare @imaxI Float;declare @iavgI Float;declare @dmaxIRD nvarchar(20);
declare @iDeMand int;declare @iDeMandHalf int;declare @iDeMandSatHalf int;declare @iDeMandOff int;
declare @iRush int;declare @iHalf int;declare @iSatHalf int;declare @iOff int;
declare @iMaxRush int;declare @iMaxHalf int;declare @iMaxSatHalf int;declare @iMaxOff int;
declare @iKWh int;declare @iTodayUseKWh int;declare @iTotalKWh int;
declare @iCount int;declare @Last_Day nvarchar(20);declare @MonthMark nvarchar(2);declare @WeekMark nvarchar(10)
set @MonthMark = Right('00' + rtrim(Month(cast(@sRecDate as date))),2)
set @WeekMark = rtrim(replace(cast(cast(DATEADD(week, DATEDIFF(week, '', @sRecDate), '') as date) as char),'-','/'))
set @ICOUNT=(SELECT COUNT(*) FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND RecDate=@sRecDate)
If @ICOUNT  > 0
Begin
SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax,AVG(W) AS Wavg,MAX(W) AS Wmax
into #T4 FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND RecDate=@sRecDate
Select @iavgI = Round(#T4.Iavg,2), @imaxI = Round(#T4.Imax,2), @iavgV = Round(#T4.Vavg,2), @imaxV = Round(#T4.Vmax,2), @iavgW = Round(#T4.Wavg,2), @imaxW = Round(#T4.Wmax,2) FROM #T4
Select @dmaxIRD = cast((RecDate + ' ' + RecTime) as nvarchar(20)) from PowerRecord,#T4 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND RecDate=@sRecDate AND PowerRecord.Iavg=#T4.Imax
Select @dmaxVRD = cast((RecDate + ' ' + RecTime) as nvarchar(20)) from PowerRecord,#T4 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND RecDate=@sRecDate AND PowerRecord.Vavg=#T4.Vmax
Select @dmaxWRD = cast((RecDate + ' ' + RecTime) as nvarchar(20)) from PowerRecord,#T4 where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND RecDate=@sRecDate AND PowerRecord.W=#T4.Wmax
Select @iDeMand = MAX(DeMand),@iDeMandHalf = MAX(DeMandHalf),@iDeMandSatHalf = MAX(DeMandSatHalf),@iDeMandOff = MAX(DeMandOff) From PowerRecord Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate
Select @Last_Day= cast(dateadd(day,-1,cast(@sRecDate as date)) as nvarchar(10));set @Last_Day = Replace(@Last_Day,'-','/')
Select Top 1 RecDate AS 日期,RushHour,HalfHour,SatHalfHour,OffHour into #T1 From PowerRecord Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@Last_Day Order By RecDate, RecTime DESC
Select Top 1 RecDate AS 日期,RushHour,HalfHour,SatHalfHour,OffHour into #T2 From PowerRecord Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate Order By RecDate, RecTime DESC
SELECT (CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT RushHour FROM #T1) ELSE (SELECT TOP 1 RushHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and RecDate = @sRecDate) END) AS RushHour,
(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT HalfHour FROM #T1) ELSE (SELECT TOP 1 HalfHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and RecDate = @sRecDate) END) AS HalfHour,
(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT SatHalfHour FROM #T1) ELSE (SELECT TOP 1 SatHalfHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and RecDate = @sRecDate) END) AS SatHalfHour,
(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT OffHour FROM #T1) ELSE (SELECT TOP 1 OffHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and RecDate = @sRecDate) END) AS OffHour,
(CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT KWh FROM #T1) ELSE (SELECT TOP 1 KWh FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and RecDate = @sRecDate) END) AS KWh INTO #T3;
Select Top 1
@iRush = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.RushHour ELSE #T2.RushHour - #T3.RushHour END),
@iHalf = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.HalfHour ELSE #T2.HalfHour - #T3.HalfHour END),
@iSatHalf = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.SatHalfHour ELSE #T2.SatHalfHour - #T3.SatHalfHour END),
@iOff = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.OffHour ELSE #T2.OffHour - #T3.OffHour END),
@iTodayUseKWh = (CASE WHEN (SUBSTRING(#T2.日期,9,10)='01' AND (#T3.KWh>#T2.KWh)) THEN #T2.KWh ELSE #T2.KWh - #T3.KWh END)
From #T2,#T3
Select TOP 1 @iMaxRush = RushHour,@iMaxHalf = HalfHour,@iMaxSatHalf = SatHalfHour,@iMaxOff = OffHour
From PowerRecord
Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID	And RecDate=@sRecDate
Order By RecDate, RecTime DESC
Drop Table #T2,#T3
;WITH LastDate(CtrlNr,MeterID,[Month],LastDate)AS (
SELECT CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111) [月份],MAX(RecDate) AS LastDate
FROM PowerRecordCollection
WHERE ctrlnr = @sCtrlNr AND MeterID = @sMeterID AND RecDate < CONVERT(NVARCHAR(7),@sRecDate,111)+'/01' 
GROUP BY CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111) )
SELECT @iTotalKWh=(SUM(KWH)+@iKWh) FROM PowerRecordCollection AS A
INNER JOIN LastDate AS B ON A.CtrlNr=B.CtrlNr AND A.MeterID=B.MeterID AND A.RecDate=B.LastDate
Exec Usp_AlterPowerRecordCollection @sCtrlNr, @sMeterID, @sRecDate, @iavgI, @imaxI, @iavgV, @iMaxV, @iAvgW, @imaxW, @iDeMand, @iDeMandHalf, @iDeMandSatHalf, @iDeMandOff, @iRush, @iHalf, @iSatHalf, @iOff, @iMaxRush, @iMaxHalf, @iMaxSatHalf, @iMaxOff,@iKWh, @MonthMark, @WeekMark, @dmaxIRD, @dmaxVRD, @dmaxWRD,@iTodayUseKWh,@iTotalKWh
END
END

GO


