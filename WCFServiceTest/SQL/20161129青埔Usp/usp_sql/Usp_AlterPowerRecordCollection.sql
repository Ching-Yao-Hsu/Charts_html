USE [ECO_secom1]
GO

/****** Object:  StoredProcedure [dbo].[Usp_AlterPowerRecordCollection]    Script Date: 2016/11/29 下午 04:57:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

			Create PROCEDURE [dbo].[Usp_AlterPowerRecordCollection]
               @CtrNr int,
               @MeterID int,
               @RecDate nVarchar(10),
	           @IAvg Float,
	           @IMax Float,
	           @VAvg Float,
	           @VMax Float,
	           @WAvg Float,
	           @WMax Float,

	           @DeMand        int,   /*尖峰最大需量*/
	           @DeMandHalf    int,   /*半尖峰最大需量*/
	           @DeMandSatHalf int,   /*週六半尖峰最大需量*/
	           @DeMandOff     int,	 /*離峰最大需量*/

	           @RushHour int,
	           @HalfHour int,
	           @SatHalfHour int,
	           @OffHour int,
	
               @RushHourMax     int,      /*尖峰*/
               @HalfHourMax     int,      /*半尖峰*/
               @SatHalfHourMax  int,      /*週六半尖峰*/
               @OffHourMax      int,       /*離峰*/

	           @KWh	int,

	           @MonthMark nVarchar(2),
	           @WeekMark  nVarchar(10),
	
	           @ImaxRD  nVarchar(20),
	           @VmaxRD  nVarchar(20),
	           @WmaxRD  nVarchar(20),
	           @TodayUseKWh	int,
	           @TotalKWh	int
           AS
           BEGIN

              declare @iCount  int
	          set @iCount = (Select count(*) From PowerRecordCollection
	                         Where CtrlNr=@CtrNr and MeterID=@MeterID and Recdate=@RecDate)
	
	          If @iCount < 1 
	             Begin	   
	          	  Insert into PowerRecordCollection
	          	  Select @CtrNr, @MeterID, @RecDate,
	          	         @IAvg, @IMax, @VAvg, @VMax, @WAvg, @WMax,
	          			 @DeMand, @DeMandHalf, @DeMandSatHalf, @DeMandOff,
	          	         @RushHour, @HalfHour, @SatHalfHour, @OffHour,
	          			 @RushHourMax, @HalfHourMax, @SatHalfHourMax, @OffHourMax,
						 @KWh,
	                     @MonthMark, @WeekMark,
	                     @ImaxRD, @VmaxRD, @WmaxRD,@TodayUseKWh,@TotalKWh	
	                     /*Print '新增資料PowerRecordCollection成功!! 【' + rtrim(cast(@CtrNr as char)) + '~' + rtrim(cast(@MeterID as char)) + '~' + rtrim(cast(@RecDate as char)) + '】' */
	             End
	          else
	             Begin
	          	  Update PowerRecordCollection
		  	      set    IAvg=@IAvg,  IMax=@IMax,
				         VAvg=@VAvg,  VMax=@VMax,
						 WAvg=@WAvg,  WMax=@WMax,
		                 DeMand=@DeMand,                  /*尖峰最大需量*/
				         DeMandHalf=@DeMandHalf,          /*半尖峰最大需量*/
				         DeMandSatHalf=@DeMandSatHalf,    /*週六半尖峰最大需量*/
				         DeMandOff=@DeMandOff,            /*離峰最大需量*/
				         
		                 RushHour=@RushHour,			
				         HalfHour=@HalfHour,
				         SatHalfHour=@SatHalfHour,
				         OffHour=@OffHour,
						 
				         RushHourMax=@RushHourMax,			
				         HalfHourMax=@HalfHourMax,
				         SatHalfHourMax=@SatHalfHourMax,
				         OffHourMax=@OffHourMax,
				         KWh=@KWh,                        /*用電量*/

	                     MonthMark=@MonthMark,
				         WeekMark=@WeekMark,	
	                     ImaxRD=@ImaxRD,  VmaxRD=@VmaxRD,  WmaxRD=@WmaxRD,
				         TodayUseKWh=@TodayUseKWh,
				         TotalKWh=@TotalKWh	   
		                 where CtrlNr=@CtrNr and MeterID=@MeterID and Recdate=@RecDate
	                     /*Print '更新資料PowerRecordCollection成功!! 【' + rtrim(cast(@CtrNr as char)) + '~' + rtrim(cast(@MeterID as char)) + '~' + rtrim(cast(@RecDate as char)) + '】'*/
	             End
	   	         Return 
           END

GO


