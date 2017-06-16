		Create PROCEDURE [dbo].[Usp_TriggerDayRecordCollection] 
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
               	      SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax,AVG(W) AS Wavg,MAX(W) AS Wmax 
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
           
		           	  /*最大需量*/
			          Select @iDeMand = MAX(DeMand),@iDeMandHalf = MAX(DeMandHalf),@iDeMandSatHalf = MAX(DeMandSatHalf),@iDeMandOff = MAX(DeMandOff)
		           	       From PowerRecord with (Nolock)
		           	       Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID
    	                   And Convert(nvarchar(10),RecDate,120)=@sRecDate

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
					  
					  declare @TM_日期      nVarChar(10)
					  declare @TM_RushHour     int
					  declare @TM_HalfHour     int
					  declare @TM_SatHalfHour  int
					  declare @TM_OffHour      int
					  declare @TM_KWh          int


    	              /*取得前一天的日期*/
    	              select @Last_Day= cast(dateadd(day,-1,cast(@sRecDate as date)) as nvarchar(10));
			          set @Last_Day = Replace(@Last_Day,'-','/')
			  
			          /*今天最後一筆的用電量*/
    	              Select Top 1 @T1_日期=SUBSTRING(RecDate,1,10), @T1_RushHour = RushHour, @T1_HalfHour = HalfHour, @T1_SatHalfHour = SatHalfHour, @T1_OffHour = OffHour, @T1_KWh = KWh
    	                   From PowerRecord with (Nolock)
    	                   Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate 
    	                   Order By RecDate DESC

    	              /*前一天最後一筆的用電量*/
					  Set @ICOUNT = (Select Count(*) From PowerRecord With (Nolock) 
					                 Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@Last_Day)
					  if @ICOUNT > 0
					     /*前一天有資料, 讀取前一天資料的最後一筆*/
					     Begin
    	                    Select Top 1 @T2_日期=SUBSTRING(RecDate,1,10), @T2_RushHour = RushHour, @T2_HalfHour = HalfHour, @T2_SatHalfHour = SatHalfHour, @T2_OffHour = OffHour, @T2_KWh = KWh
		           	        From PowerRecord with (Nolock) 
		           	        Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@Last_Day 
		           	        Order By RecDate DESC
						 End
					  Else	/*前一天沒資料, 讀取當天資料的第一筆*/  
					     Begin
    	                    Select Top 1 @T2_日期=SUBSTRING(RecDate,1,10), @T2_RushHour = RushHour, @T2_HalfHour = HalfHour, @T2_SatHalfHour = SatHalfHour, @T2_OffHour = OffHour, @T2_KWh = KWh
		           	        From PowerRecord with (Nolock) 
		           	        Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate 
		           	        Order By RecDate ASC
						 End 

			          /*以[前一天最後一筆]及[今日最後一筆]之電量計算出，一天的用電量*/
					 if @T1_RushHour > @T2_RushHour
					    Begin
						    set @iRush = @T1_RushHour - @T2_RushHour
						End
					 Else
					    Begin
						    Select @TM_RushHour = Max(RushHour) From PowerRecord with (Nolock) 
		           	        Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate
						    /*判斷今天最大值是否等於今天最後一筆*/
							if @TM_RushHour = @T1_RushHour
							   Begin 
							      set @iRush = @T1_RushHour - @T2_RushHour
							   End
							else
							   /*判斷今天最大值是否等於昨天最後一筆*/
							   if @TM_RushHour = @T2_RushHour
							      Begin 
							         set @iRush = @T1_RushHour
							      End
							   else
							      Begin 
							         set @iRush = @TM_RushHour - @T2_RushHour + @T1_RushHour
							      End
						End
						
					 if @T1_HalfHour > @T2_HalfHour
					    Begin
						    set @iHalf = @T1_HalfHour - @T2_HalfHour
						End
					 Else
					    Begin
						    Select @TM_HalfHour = Max(HalfHour) From PowerRecord with (Nolock) 
		           	        Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate
							if @TM_HalfHour = @T1_HalfHour
							   Begin 
							      set @iHalf = @T1_HalfHour - @T2_HalfHour
							   End
							else
							   /*判斷今天最大值是否等於昨天最後一筆*/
							   if @TM_HalfHour = @T2_HalfHour
							      Begin 
							         set @iHalf = @T1_HalfHour
							      End
							   else
							      Begin 
							         set @iHalf = @TM_HalfHour - @T2_HalfHour + @T1_HalfHour
							      End
						End
						
					 if @T1_SatHalfHour > @T2_SatHalfHour
					    Begin
						    set @iSatHalf = @T1_SatHalfHour - @T2_SatHalfHour
						End
					 Else
					    Begin
						    Select @TM_SatHalfHour = Max(SatHalfHour) From PowerRecord with (Nolock) 
		           	        Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate
							if @TM_SatHalfHour = @T1_SatHalfHour
							   Begin 
							      set @iSatHalf = @T1_SatHalfHour - @T2_SatHalfHour
							   End
							else
							   /*判斷今天最大值是否等於昨天最後一筆*/
							   if @TM_SatHalfHour = @T2_SatHalfHour
							      Begin 
							         set @iSatHalf = @T1_SatHalfHour
							      End
							   else
							      Begin 
							         set @iSatHalf = @TM_SatHalfHour - @T2_SatHalfHour + @T1_SatHalfHour
							      End
						End
						
					 if @T1_OffHour > @T2_OffHour
					    Begin
						    set @iOff = @T1_OffHour - @T2_OffHour
						End
					 Else
					    Begin
						    Select @TM_OffHour = Max(OffHour) From PowerRecord with (Nolock) 
		           	        Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate
							if @TM_OffHour = @T1_OffHour
							   Begin 
							      set @iOff = @T1_OffHour - @T2_OffHour
							   End
							else
							   /*判斷今天最大值是否等於昨天最後一筆*/
							   if @TM_OffHour = @T2_OffHour
							      Begin 
							         set @iOff = @T1_OffHour
							      End
							   else
							      Begin 
							         set @iOff = @TM_OffHour - @T2_OffHour + @T1_OffHour
							      End
						End
						
					 if @T1_KWh > @T2_KWh
					    Begin
						    set @iTodayUseKWh = @T1_KWh - @T2_KWh
						End
					 Else
					    Begin
						    Select @TM_KWh = Max(KWh) From PowerRecord with (Nolock) 
		           	        Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate
							
							if @TM_KWh = @T1_KWh
							   Begin 
							      set @iTodayUseKWh = @T1_KWh - @T2_KWh
							   End
							else
							   /*判斷今天最大值是否等於昨天最後一筆*/
							   if @TM_KWh = @T2_KWh
							      Begin 
							         set @iTodayUseKWh = @T1_KWh
							      End
							   else
							      Begin 
							         set @iTodayUseKWh = @TM_KWh - @T2_KWh + @T1_KWh
							      End
						End

			          Select Top 1 @iMaxRush = RushHour,@iMaxHalf = HalfHour,@iMaxSatHalf = SatHalfHour,@iMaxOff = OffHour, @iKWh = KWh
                           From PowerRecord with (Nolock)
				           Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID	And SUBSTRING(RecDate,1,10)=@sRecDate
				           Order By RecDate DESC

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

			          /*新增PowerRecordCollection資料*/
    	              Exec Usp_AlterPowerRecordCollection @sCtrlNr, @sMeterID, @sRecDate, @iavgI, @imaxI, @iavgV, @iMaxV, @iAvgW, @imaxW, @iDeMand, @iDeMandHalf, @iDeMandSatHalf, @iDeMandOff, @iRush, @iHalf, @iSatHalf, @iOff, @iMaxRush, @iMaxHalf, @iMaxSatHalf, @iMaxOff,@iKWh, @MonthMark, @WeekMark, @dmaxIRD, @dmaxVRD, @dmaxWRD,@iTodayUseKWh,@iTotalKWh
                  end
               Else
                  begin 
    	              print '沒電量資料：' + @sRecDate
                  end
               END


$

CREATE TRIGGER [dbo].[AlterRecordCollection] 
   ON  [dbo].[PowerRecord] 
   AFTER INSERT
AS 

   declare @CtrlNr  int            /*ECO5編號*/
   declare @MeterID int            /*電表編號*/
   declare @RecDate NVARCHAR (10)  /*紀錄時間*/

   declare @iCount int
   declare @Last_Day nVarChar(20)

   Select @CtrlNr =CtrlNr, @MeterID = MeterID, @RecDate = Substring(RecDate, 1,10)  from inserted

   set @iCount = (Select Count(*) From dbo.PowerRecord Where CtrlNr = @CtrlNr and MeterID = @MeterID and Substring(RecDate, 1, 10) = @RecDate)

   if @iCount < 2 
      Begin
	     /*--執行今天與昨天*/			
    	    /*取得前一天的日期*/
    	    select @Last_Day= cast(dateadd(day,-1,cast(@RecDate as date)) as nvarchar(10));
			set @Last_Day = Replace(@Last_Day,'-','/')
			
			exec [dbo].[Usp_TriggerDayRecordCollection] @CtrlNr, @MeterID, @Last_Day
	  End

   /*--執行今天即可*/
   exec [dbo].[Usp_TriggerDayRecordCollection] @CtrlNr, @MeterID, @RecDate


   /*以下Update ECOSMART.dbo.MeterSetup*/
   declare @sDBName  nVarChar(50)
   declare @sEco_Account nVarchar(20)

   declare @ICtrlNr  int
   declare @IMeterID int
   declare @IRecDate NVARCHAR(20)
   declare @IIavg    float
   declare @IVavg    float
   declare @IW       float

   Select @sDBName = Replace(DB_Name() , 'ECO_' , '')
   Select @ICtrlNr = CtrlNr, @IMeterID = MeterID, @IRecDate = RecDate, @IIavg = Iavg, @IVavg = Vavg, @IW= W from inserted
      
   Select @sEco_Account = ECO_Account From ECOSMART.[dbo].[ControllerSetup] Where Account = @sDBName and CtrlNr = @ICtrlNr

   UPDATE ECOSMART.[dbo].[MeterSetup] 
   SET [RecDate]=@IRecDate, [Iavg]=@IIavg, [Vavg]=@IVavg, [W]=@IW
       WHERE [ECO_Account]=@sEco_Account AND [CtrlNr]=@ICtrlNr AND [MeterID]=@IMeterID

$

			Create PROCEDURE [dbo].[Usp_AlterPowerRecordCollection]
			   @DBName nVarchar(50),
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

	          set @iCount = ('Select count(*) From ' +@DBName+'.dbo.PowerRecordCollection
	                         Where CtrlNr='''+@CtrNr+''' and MeterID='''+@MeterID+''' and Recdate='''+@RecDate);
	
	          If @iCount < 1 
	             Begin	   
	          	  exec ('Insert into  ' +@DBName+'.dbo.PowerRecordCollection
	          	  Select '+@CtrNr+', '+@MeterID+', '''+@RecDate+''',
	          	         '+@IAvg+', '+@IMax+', '+@VAvg+', '+@VMax+', '+@WAvg+', '+@WMax+', 
	          			 '+@DeMand+', '+@DeMandHalf+', '+@DeMandSatHalf+', '+@DeMandOff+',  
	          	         '+@RushHour+', '+@HalfHour+', '+@SatHalfHour+', '+@OffHour+', 
	          			 '+@RushHourMax+', '+@HalfHourMax+', '+@SatHalfHourMax+', '+@OffHourMax+', 
						 '+@KWh+', 
	                     '''+@MonthMark+''', '''+@WeekMark+''',
	                     '+@ImaxRD+', '+@VmaxRD+', '+@WmaxRD+', '+@TodayUseKWh+', '+@TotalKWh)	
	                     /*Print '新增資料PowerRecordCollection成功!! 【' + rtrim(cast(@CtrNr as char)) + '~' + rtrim(cast(@MeterID as char)) + '~' + rtrim(cast(@RecDate as char)) + '】' */
	             End
	          else
	             Begin
	          	  exec ('Update  ' +@DBName+'.dbo.PowerRecordCollection
		  	      set    IAvg='+@IAvg+', IMax='+@IMax+',
				         VAvg='+@VAvg+', VMax='+@VMax+',
						 WAvg='+@WAvg+', WMax='+@WMax+',
		                 DeMand='+@DeMand+',                  /*尖峰最大需量*/
				         DeMandHalf='+@DeMandHalf+',          /*半尖峰最大需量*/
				         DeMandSatHalf='+@DeMandSatHalf+',    /*週六半尖峰最大需量*/
				         DeMandOff='+@DeMandOff+',            /*離峰最大需量*/
				         
		                 RushHour='+@RushHour+',			
				         HalfHour='+@HalfHour+',
				         SatHalfHour='+@SatHalfHour+',
				         OffHour='+@OffHour+',
						 
				         RushHourMax='+@RushHourMax+',			
				         HalfHourMax='+@HalfHourMax+',
				         SatHalfHourMax='+@SatHalfHourMax+',
				         OffHourMax='+@OffHourMax+',
				         KWh='+@KWh+',                         /*用電量*/

	                     MonthMark='''+@MonthMark+''',
				         WeekMark='''+@WeekMark+''',	
	                     ImaxRD='+@ImaxRD+',  VmaxRD='+@VmaxRD+',  WmaxRD='+@WmaxRD+',
				         TodayUseKWh='+@TodayUseKWh+',
				         TotalKWh='+@TotalKWh+' 	   
		                 where CtrlNr='+@CtrNr+' and MeterID='+@MeterID+' and Recdate='''+@RecDate+'''')
	                     /*Print '更新資料PowerRecordCollection成功!! 【' + rtrim(cast(@CtrNr as char)) + '~' + rtrim(cast(@MeterID as char)) + '~' + rtrim(cast(@RecDate as char)) + '】'*/
	             End
	   	         Return 
           END
$

		Create PROCEDURE [dbo].[Usp_Day_Record] 
			   @DBName           nVarchar(50),
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

               SET @ICOUNT=(SELECT COUNT(*) FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate)
    
               If @ICOUNT  > 0
                  Begin	       
            
               	      SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax,AVG(W) AS Wavg,MAX(W) AS Wmax 
               	      into #T4 FROM PowerRecord 
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
           
		           	  /*最大需量*/
			          Select @iDeMand = MAX(DeMand),@iDeMandHalf = MAX(DeMandHalf),@iDeMandSatHalf = MAX(DeMandSatHalf),@iDeMandOff = MAX(DeMandOff)
		           	       From PowerRecord
		           	       Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID
    	                   And Convert(nvarchar(10),RecDate,120)=@sRecDate

    	              /*取得前一天的日期*/
    	              select @Last_Day= cast(dateadd(day,-1,cast(@sRecDate as date)) as nvarchar(10));
			          set @Last_Day = Replace(@Last_Day,'-','/')
    	      
    	              /*前一天最後一筆的用電量*/
    	              Select Top 1 SUBSTRING(RecDate,1,10) AS 日期,RushHour,HalfHour,SatHalfHour,OffHour,KWh into #T1
		           	       From PowerRecord 
		           	       Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@Last_Day 
		           	       Order By RecDate DESC

			          /*今天最後一筆的用電量*/
    	              Select Top 1 SUBSTRING(RecDate,1,10) AS 日期,RushHour,HalfHour,SatHalfHour,OffHour,KWh into #T2
    	                   From PowerRecord 
    	                   Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@sRecDate 
    	                   Order By RecDate DESC

			          SELECT 
    	                   (CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT RushHour FROM #T1) 
				           ELSE (SELECT TOP 1 RushHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS RushHour,
				           (CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT HalfHour FROM #T1) 
				           ELSE (SELECT TOP 1 HalfHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS HalfHour,
				           (CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT SatHalfHour FROM #T1) 
				           ELSE (SELECT TOP 1 SatHalfHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS SatHalfHour,
				           (CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT OffHour FROM #T1) 
				           ELSE (SELECT TOP 1 OffHour FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS OffHour,
				           (CASE WHEN EXISTS(SELECT * FROM #T1) THEN (SELECT KWh FROM #T1) 
				           ELSE (SELECT TOP 1 KWh FROM PowerRecord WHERE CtrlNr = @sCtrlNr AND MeterID = @sMeterID and SUBSTRING(RecDate,1,10) = @sRecDate) END) AS KWh INTO #T3

			          /*以前一天最後一筆及今日最後一筆之電度計算出，一天的用電量*/
			          Select Top 1
				           @iRush = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.RushHour ELSE #T2.RushHour - #T3.RushHour END),                /* AS 尖峰, */
				           @iHalf = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.HalfHour ELSE #T2.HalfHour - #T3.HalfHour END),                /* AS 半尖峰, */
                           @iSatHalf = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.SatHalfHour ELSE #T2.SatHalfHour - #T3.SatHalfHour END),    /* AS 週六半尖峰, */
                           @iOff = (CASE WHEN SUBSTRING(#T2.日期,9,10)='01' THEN #T2.OffHour ELSE #T2.OffHour - #T3.OffHour END),                    /* AS 離峰 */
				           @iTodayUseKWh = (CASE WHEN (SUBSTRING(#T2.日期,9,10)='01' AND (#T3.KWh>#T2.KWh)) THEN #T2.KWh ELSE #T2.KWh - #T3.KWh END) /* [有的電表月初並不會歸零]AS 今日總用電量*/
                           From #T2,#T3
    	      
			          Select Top 1 @iMaxRush = RushHour,@iMaxHalf = HalfHour,@iMaxSatHalf = SatHalfHour,@iMaxOff = OffHour
                           From PowerRecord
				           Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID	And SUBSTRING(RecDate,1,10)=@sRecDate
				           Order By RecDate DESC

			          Select Top 1 @iKWh = KWh From PowerRecord
				           Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID	And SUBSTRING(RecDate,1,10)=@sRecDate
				           Order By RecDate DESC

			          /*將前一天的用電量 Temp檔移除*/
    	              Drop Table #T2,#T3

			          /*找出每月的最後一天*/
			          ;WITH LastDate(CtrlNr,MeterID,[Month],LastDate) AS
				      (
                        SELECT CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111) [月份],MAX(RecDate) AS LastDate
					       FROM PowerRecordCollection
					       WHERE ctrlnr = @sCtrlNr AND MeterID = @sMeterID 
						   AND RecDate < CONVERT(NVARCHAR(7),@sRecDate,111)+'/01'
				           GROUP BY CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111)
                      )

			          /*每月最後一天的用電量總和+本月累計至今天的用電量*/
				      SELECT @iTotalKWh=(SUM(KWH)+@iKWh)
				           FROM PowerRecordCollection AS A
                           INNER JOIN LastDate AS B ON A.CtrlNr=B.CtrlNr AND A.MeterID=B.MeterID  AND A.RecDate=B.LastDate

			          /*新增PowerRecordCollection資料*/
    	              Exec Usp_AlterPowerRecordCollection @sCtrlNr, @sMeterID, @sRecDate, @iavgI, @imaxI, @iavgV, @iMaxV, @iAvgW, @imaxW, @iDeMand, @iDeMandHalf, @iDeMandSatHalf, @iDeMandOff, @iRush, @iHalf, @iSatHalf, @iOff, @iMaxRush, @iMaxHalf, @iMaxSatHalf, @iMaxOff,@iKWh, @MonthMark, @WeekMark, @dmaxIRD, @dmaxVRD, @dmaxWRD,@iTodayUseKWh,@iTotalKWh
                  end
               Else
                  begin 
    	              print '沒電量資料：' + @sRecDate
                  end
               END
$
		  
		  Create PROCEDURE [dbo].[Usp_LostDayCheck]
           AS
           BEGIN
	           SET NOCOUNT ON;
	
	           DECLARE @CtrlNr int
           	   DECLARE @MeterID int
		   	   DECLARE @RecDate varchar(10)

		   	   DECLARE addr_cursor CURSOR FOR
		   	   (Select distinct CtrlNr, MeterID, (Left(Recdate,10)) as Recdate 
		   	      from [dbo].[PowerRecord] A 
		   	      where not exists (select 1 from [dbo].[PowerRecordCollection] B 
		   	                        where B.CtrlNr=A.CtrlNr and B.MeterID=A.MeterID and B.RecDate=left(A.RecDate,10) ) )

		   	   OPEN addr_cursor   /*開啟Cursor*/
		   	   FETCH NEXT FROM addr_cursor INTO @CtrlNr, @MeterID, @RecDate
		   	   WHILE @@FETCH_STATUS = 0
		   	     BEGIN
		   	        exec Usp_Day_Record @CtrlNr,@MeterID, @RecDate
			   	    FETCH NEXT FROM addr_cursor INTO @CtrlNr, @MeterID, @RecDate
		     	 END
		  
		   	   CLOSE addr_cursor        /*關閉Cursor*/
		   	   DEALLOCATE addr_cursor   /*釋放Cursor*/
           END
$

