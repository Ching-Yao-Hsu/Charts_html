		CREATE PROCEDURE [dbo].[Usp_TriggerDayRecordCollection] 
               @sCtrlNr          int,
               @sMeterID         int,
               @sRecDate         nvarchar(10)
           AS
           BEGIN
               SET NOCOUNT ON;	
	    
               declare @Err int                  /*錯誤訊息*/

               declare @iavgW     Float          /*平均值*/
               declare @imaxW     Float          /*最大實功*/
               declare @dmaxWRD   nvarchar(20)   /*最大實功之時間*/

               declare @imaxV     Float          /*電壓最大值*/
               declare @iavgV     Float          /*電壓平均值*/
               declare @dmaxVRD   nvarchar(20)   /*最大電壓之時間*/

               declare @imaxI     Float          /*電流最大值*/
               declare @iavgI     Float          /*電流平均值*/
               declare @dmaxIRD   nvarchar(20)   /*最大電流之時間*/
	         
               declare @iDeMand         int      /*尖峰*/
               declare @iDeMandHalf     int      /*半尖峰*/
               declare @iDeMandSatHalf  int      /*週六半尖峰*/
               declare @iDeMandOff      int      /*離峰*/

               declare @iRush     int            /*尖峰*/
               declare @iHalf     int            /*半尖峰*/
               declare @iSatHalf  int            /*週六半尖峰*/
               declare @iOff      int            /*離峰*/
	       
               declare @iMaxRush     int         /*尖峰*/
               declare @iMaxHalf     int         /*半尖峰*/
               declare @iMaxSatHalf  int         /*週六半尖峰*/
               declare @iMaxOff      int         /*離峰*/

	           declare @iKWh      int	
	           declare @iTodayUseKWh	int
	           declare @iTotalKWh	int

               declare @iCount    int
               declare @Last_Day  nvarchar(20)   /*尋找前一天之日期*/
                                      
	           declare @MonthMark nvarchar(2)    /*月份*/
	           declare @WeekMark  nvarchar(10)   /*星期一之日期*/
	                                    
	           set @MonthMark = Right('00' + rtrim(Month(cast(@sRecDate as date))),2)
	           set @WeekMark = rtrim(replace(cast(cast(DATEADD(week, DATEDIFF(week, '', @sRecDate) , '') as date) as char),'-','/'))

			   /*判斷當天是否有資料*/
               SET @ICOUNT=(SELECT COUNT(*) FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate)
               If @ICOUNT  > 0
                  Begin
               	      /*SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax,AVG(W) AS Wavg,MAX(W) AS Wmax 
               	      into #T4 FROM PowerRecord with (Nolock)
               	      WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate
    	      
	           		  SELECT @iavgI = Round(#T4.Iavg,2), /*AS 電流平均值,*/
	           				 @imaxI = Round(#T4.Imax,2), /*AS 電流最大值,*/
               	             @iavgV = Round(#T4.Vavg,2), /*AS 電壓平均值,*/
	           				 @imaxV = Round(#T4.Vmax,2), /*AS 電壓最大值,*/
               	             @iavgW = Round(#T4.Wavg,2), /*AS 功率平均值,*/
               	             @imaxW = Round(#T4.Wmax,2)  /*AS 功率最大值*/
           			  FROM #T4


               	      /*尋找最大電流之該日期時間*/
               	      SELECT @dmaxIRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4 
                           where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate 
    	                   AND PowerRecord.Iavg=#T4.Imax

		           	  /*尋找最大電壓之該日期時間*/
    	              SELECT @dmaxVRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4 
                           where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate 
    	                   AND PowerRecord.Vavg=#T4.Vmax

			          /*尋找最大實功之該日期時間*/
    	              SELECT @dmaxWRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4
                           where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate 
    	                   AND PowerRecord.W=#T4.Wmax
                      */

					 set @iavgI = 0 /* 電流平均值,*/
					 set @iavgV = 0 /* 電壓平均值,*/
					 set @iavgW = 0 /* 功率平均值*/
					 set @dmaxIRD = '' /* 最大電流之該日期時間*/
					 set @dmaxVRD = '' /* 最大電壓之該日期時間*/
					 set @dmaxWRD = '' /* 最大實功之該日期時間*/


		           	  /*最大需量*/
			          Select @imaxI=MAX(Iavg), @imaxV=MAX(Vavg), @imaxW=MAX(W)
					        ,@iDeMand = MAX(DeMand),@iDeMandHalf = MAX(DeMandHalf),@iDeMandSatHalf = MAX(DeMandSatHalf),@iDeMandOff = MAX(DeMandOff)
		           	       From PowerRecord with (Nolock)
		           	       Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And substring(RecDate,1,10)=@sRecDate

                      /*今天*/
					  declare @T1_日期      nVarChar(10)
					  declare @T1_RushHour     int
					  declare @T1_HalfHour     int
					  declare @T1_SatHalfHour  int
					  declare @T1_OffHour      int
					  declare @T1_KWh          int
					  
                      /*昨天*/
					  declare @T2_日期      nVarChar(10)
					  declare @T2_RushHour     int
					  declare @T2_HalfHour     int
					  declare @T2_SatHalfHour  int
					  declare @T2_OffHour      int
					  declare @T2_KWh          int

    	              /*取得前一天的日期*/
    	              select @Last_Day= cast(dateadd(day,-1,cast(@sRecDate as date)) as nvarchar(10));
			          set @Last_Day = Replace(@Last_Day,'-','/')
			  
			          /*今天最後一筆的用電量*/
    	              Select Top 1 @T1_日期=SUBSTRING(RecDate,1,10), @T1_RushHour = RushHour, @T1_HalfHour = HalfHour, @T1_SatHalfHour = SatHalfHour, @T1_OffHour = OffHour, @T1_KWh = KWh
    	                   From PowerRecord with (Nolock)
    	                   Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And substring(RecDate,1,10)=@sRecDate
    	                   Order By RecDate DESC

    	              /*前一天最後一筆的用電量*/
					  Set @ICOUNT = (Select Count(*) From PowerRecord With (Nolock) 
					                 Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And substring(RecDate,1,10)=@Last_Day)
					  if @ICOUNT > 0
					     /*前一天有資料, 讀取前一天資料的最後一筆*/
					     Begin
    	                    Select Top 1 @T2_日期=SUBSTRING(RecDate,1,10), @T2_RushHour = RushHour, @T2_HalfHour = HalfHour, @T2_SatHalfHour = SatHalfHour, @T2_OffHour = OffHour, @T2_KWh = KWh
		           	        From PowerRecord with (Nolock) 
		           	        Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And substring(RecDate,1,10)=@Last_Day
		           	        Order By RecDate DESC
						 End
					  Else	/*前一天沒資料, 讀取當天資料的第一筆*/  
					     Begin
    	                    Select Top 1 @T2_日期=SUBSTRING(RecDate,1,10), @T2_RushHour = RushHour, @T2_HalfHour = HalfHour, @T2_SatHalfHour = SatHalfHour, @T2_OffHour = OffHour, @T2_KWh = KWh
		           	        From PowerRecord with (Nolock) 
		           	        Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And substring(RecDate,1,10)=@sRecDate
		           	        Order By RecDate ASC
						 End 
			          /*以[前一天最後一筆]及[今日最後一筆]之電量計算出，一天的用電量*/

					  
					 if (@T1_RushHour - @T2_RushHour) < 0 
					    Begin
						   set @iRush = @T1_RushHour
						End
					 Else
					    Begin
						   set @iRush = @T1_RushHour - @T2_RushHour
						End
						
						
					 if (@T1_HalfHour - @T2_HalfHour) < 0 
					    Begin
						   set @iHalf = @T1_HalfHour
						End
					 Else
					    Begin
						   set @iHalf = @T1_HalfHour - @T2_HalfHour
						End

						
					 if (@T1_SatHalfHour - @T2_SatHalfHour) < 0 
					    Begin
						   set @iSatHalf = @T1_SatHalfHour
						End
					 Else
					    Begin
						   set @iSatHalf = @T1_SatHalfHour - @T2_SatHalfHour
						End
						
					 if (@T1_OffHour - @T2_OffHour) < 0 
					    Begin
						   set @iOff = @T1_OffHour
						End
					 Else
					    Begin
						   set @iOff = @T1_OffHour - @T2_OffHour
						End
						
					 if (@T1_KWh - @T2_KWh) < 0 
					    Begin
						   set @iTodayUseKWh = @T1_KWh
						End
					 Else
					    Begin
						   set @iTodayUseKWh = @T1_KWh - @T2_KWh
						End

			          /*Select Top 1 @iMaxRush = RushHour,@iMaxHalf = HalfHour,@iMaxSatHalf = SatHalfHour,@iMaxOff = OffHour, @iKWh = KWh
                           From PowerRecord with (Nolock)
				           Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID	And SUBSTRING(RecDate,1,10)=@sRecDate
				           Order By RecDate DESC  */

                       set @iMaxRush = @T1_RushHour
					   set @iMaxHalf = @T1_HalfHour
					   set @iMaxSatHalf = @T1_SatHalfHour
					   set @iMaxOff = @T1_OffHour
					   set @iKWh = @T1_KWh

					   
					  /*
			          /*找出每月的最後一天*/
			          ;WITH LastDate(CtrlNr,MeterID,[Month],LastDate) AS
				      (
                        SELECT CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111) [月份],MAX(RecDate) AS LastDate
					       FROM PowerRecordCollection with (Nolock)
					       WHERE ctrlnr = @sCtrlNr AND MeterID = @sMeterID 
						   AND RecDate < CONVERT(NVARCHAR(7),@sRecDate,111)+'/01'
				           GROUP BY CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111)
                      )

			          /*每月最後一天的用電量總和+本月累計至今天的用電量*/
				      SELECT @iTotalKWh=(SUM(KWH)+@iKWh)
				           FROM PowerRecordCollection AS A
                           INNER JOIN LastDate AS B ON A.CtrlNr=B.CtrlNr AND A.MeterID=B.MeterID  AND A.RecDate=B.LastDate
                      */
					  
					  set @iTodayUseKWh = 0   /*當日總和*/
					  set @iTotalKWh = 0      /*當月總和*/  
			          /*新增PowerRecordCollection資料*/
    	              Exec Usp_AlterPowerRecordCollection @sCtrlNr, @sMeterID, @sRecDate, @iavgI, @imaxI, @iavgV, @iMaxV, @iAvgW, @imaxW, @iDeMand, @iDeMandHalf, @iDeMandSatHalf, @iDeMandOff, @iRush, @iHalf, @iSatHalf, @iOff, @iMaxRush, @iMaxHalf, @iMaxSatHalf, @iMaxOff,@iKWh, @MonthMark, @WeekMark, @dmaxIRD, @dmaxVRD, @dmaxWRD,@iTodayUseKWh,@iTotalKWh
                  end
               Else
                  begin 
    	              print '沒電量資料：' + @sRecDate
                  end
               END


$

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
$

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

$

