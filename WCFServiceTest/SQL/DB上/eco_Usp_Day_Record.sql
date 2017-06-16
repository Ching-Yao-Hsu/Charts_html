USE [ECO_eco]
GO

/****** Object:  StoredProcedure [dbo].[Usp_Day_Record]    Script Date: 2016/8/6 下午 10:44:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



		Create PROCEDURE [dbo].[Usp_Day_Record] 
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

GO


