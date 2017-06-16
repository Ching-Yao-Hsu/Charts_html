USE [ECO_N_AC_2016]
GO

/****** Object:  StoredProcedure [dbo].[Usp_AlterPowerRecordCollection]    Script Date: 2016/8/6 ¤U¤È 10:37:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_AlterPowerRecordCollection]
@CtrNr int,@MeterID int,@RecDate nVarchar(10),	@IAvg Float,@IMax Float,@VAvg Float,@VMax Float,@WAvg Float,@WMax Float,
@DeMand int,@DeMandHalf int,@DeMandSatHalf int,@DeMandOff int,@RushHour int,@HalfHour int,@SatHalfHour int,@OffHour int,
@RushHourMax int,@HalfHourMax int,@SatHalfHourMax int,@OffHourMax int,
@KWh int,
@MonthMark nVarchar(2),@WeekMark nVarchar(10),@ImaxRD nVarchar(20),@VmaxRD nVarchar(20),@WmaxRD nVarchar(20),
@TodayUseKWh int,@TotalKWh int
AS
BEGIN
declare @iCount int
set @iCount = (Select count(*) From PowerRecordCollection Where CtrlNr=@CtrNr and MeterID=@MeterID and Recdate=@RecDate)
If @iCount < 1
Begin
Insert into PowerRecordCollection
Select @CtrNr, @MeterID, @RecDate, @IAvg, @IMax, @VAvg, @VMax, @WAvg, @WMax,
@DeMand, @DeMandHalf, @DeMandSatHalf, @DeMandOff, @RushHour, @HalfHour, @SatHalfHour, @OffHour,
@RushHourMax, @HalfHourMax, @SatHalfHourMax, @OffHourMax, @KWh, @MonthMark, @WeekMark, @ImaxRD, @VmaxRD, @WmaxRD, @TodayUseKWh, @TotalKWh
End
Else
Begin
Update PowerRecordCollection
set IAvg=@IAvg, IMax=@IMax, VAvg=@VAvg, VMax=@VMax, WAvg=@WAvg, WMax=@WMax,
DeMand=@DeMand, DeMandHalf=@DeMandHalf, DeMandSatHalf=@DeMandSatHalf, DeMandOff=@DeMandOff,
RushHour=@RushHour, HalfHour=@HalfHour, SatHalfHour=@SatHalfHour, OffHour=@OffHour,
RushHourMax=@RushHourMax,HalfHourMax=@HalfHourMax,SatHalfHourMax=@SatHalfHourMax,OffHourMax=@OffHourMax, KWh=@KWh,
MonthMark=@MonthMark, WeekMark=@WeekMark, ImaxRD=@ImaxRD, VmaxRD=@VmaxRD, WmaxRD=@WmaxRD, TodayUseKWh=@TodayUseKWh, TotalKWh=@TotalKWh
Where CtrlNr=@CtrNr and MeterID=@MeterID and Recdate=@RecDate
End
Return
END

GO


