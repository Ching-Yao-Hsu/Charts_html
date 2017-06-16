USE [ECO_secom1]
GO

/****** Object:  StoredProcedure [dbo].[Usp_AlterPowerRecordCollection]    Script Date: 2016/11/29 �U�� 04:57:33 ******/
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

	           @DeMand        int,   /*�y�p�̤j�ݶq*/
	           @DeMandHalf    int,   /*�b�y�p�̤j�ݶq*/
	           @DeMandSatHalf int,   /*�g���b�y�p�̤j�ݶq*/
	           @DeMandOff     int,	 /*���p�̤j�ݶq*/

	           @RushHour int,
	           @HalfHour int,
	           @SatHalfHour int,
	           @OffHour int,
	
               @RushHourMax     int,      /*�y�p*/
               @HalfHourMax     int,      /*�b�y�p*/
               @SatHalfHourMax  int,      /*�g���b�y�p*/
               @OffHourMax      int,       /*���p*/

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
	                     /*Print '�s�W���PowerRecordCollection���\!! �i' + rtrim(cast(@CtrNr as char)) + '~' + rtrim(cast(@MeterID as char)) + '~' + rtrim(cast(@RecDate as char)) + '�j' */
	             End
	          else
	             Begin
	          	  Update PowerRecordCollection
		  	      set    IAvg=@IAvg,  IMax=@IMax,
				         VAvg=@VAvg,  VMax=@VMax,
						 WAvg=@WAvg,  WMax=@WMax,
		                 DeMand=@DeMand,                  /*�y�p�̤j�ݶq*/
				         DeMandHalf=@DeMandHalf,          /*�b�y�p�̤j�ݶq*/
				         DeMandSatHalf=@DeMandSatHalf,    /*�g���b�y�p�̤j�ݶq*/
				         DeMandOff=@DeMandOff,            /*���p�̤j�ݶq*/
				         
		                 RushHour=@RushHour,			
				         HalfHour=@HalfHour,
				         SatHalfHour=@SatHalfHour,
				         OffHour=@OffHour,
						 
				         RushHourMax=@RushHourMax,			
				         HalfHourMax=@HalfHourMax,
				         SatHalfHourMax=@SatHalfHourMax,
				         OffHourMax=@OffHourMax,
				         KWh=@KWh,                        /*�ιq�q*/

	                     MonthMark=@MonthMark,
				         WeekMark=@WeekMark,	
	                     ImaxRD=@ImaxRD,  VmaxRD=@VmaxRD,  WmaxRD=@WmaxRD,
				         TodayUseKWh=@TodayUseKWh,
				         TotalKWh=@TotalKWh	   
		                 where CtrlNr=@CtrNr and MeterID=@MeterID and Recdate=@RecDate
	                     /*Print '��s���PowerRecordCollection���\!! �i' + rtrim(cast(@CtrNr as char)) + '~' + rtrim(cast(@MeterID as char)) + '~' + rtrim(cast(@RecDate as char)) + '�j'*/
	             End
	   	         Return 
           END

GO


