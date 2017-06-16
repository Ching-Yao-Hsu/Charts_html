USE [ECO_eco]
GO

/****** Object:  StoredProcedure [dbo].[Usp_Day_Record]    Script Date: 2016/8/6 �U�� 10:44:02 ******/
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
	    
               declare @Err int                  /*���~�T��*/

               declare @iavgW     Float          /*������*/
               declare @imaxW     Float          /*�̤j��\*/
               declare @dmaxWRD   nvarchar(20)   /*�̤j��\���ɶ�*/

               declare @imaxV     Float          /*�q���̤j��*/
               declare @iavgV     Float          /*�q��������*/
               declare @dmaxVRD   nvarchar(20)   /*�̤j�q�����ɶ�*/

               declare @imaxI     Float          /*�q�y�̤j��*/
               declare @iavgI     Float          /*�q�y������*/
               declare @dmaxIRD   nvarchar(20)   /*�̤j�q�y���ɶ�*/
	         
               declare @iDeMand         int      /*�y�p*/
               declare @iDeMandHalf     int      /*�b�y�p*/
               declare @iDeMandSatHalf  int      /*�g���b�y�p*/
               declare @iDeMandOff      int      /*���p*/

               declare @iRush     int            /*�y�p*/
               declare @iHalf     int            /*�b�y�p*/
               declare @iSatHalf  int            /*�g���b�y�p*/
               declare @iOff      int            /*���p*/
	       
               declare @iMaxRush     int         /*�y�p*/
               declare @iMaxHalf     int         /*�b�y�p*/
               declare @iMaxSatHalf  int         /*�g���b�y�p*/
               declare @iMaxOff      int         /*���p*/

	           declare @iKWh      int	
	           declare @iTodayUseKWh	int
	           declare @iTotalKWh	int

               declare @iCount    int
               declare @Last_Day  nvarchar(20)   /*�M��e�@�Ѥ����*/
                                      
	           declare @MonthMark nvarchar(2)    /*���*/
	           declare @WeekMark  nvarchar(10)   /*�P���@�����*/
	                                    
	           set @MonthMark = Right('00' + rtrim(Month(cast(@sRecDate as date))),2)
	           set @WeekMark = rtrim(replace(cast(cast(DATEADD(week, DATEDIFF(week, '', @sRecDate) , '') as date) as char),'-','/'))

               SET @ICOUNT=(SELECT COUNT(*) FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate)
    
               If @ICOUNT  > 0
                  Begin	       
            
               	      SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax,AVG(W) AS Wavg,MAX(W) AS Wmax 
               	      into #T4 FROM PowerRecord 
               	      WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate
    	      
	           		  SELECT @iavgI = Round(#T4.Iavg,2), /*AS �q�y������,*/
	           				 @imaxI = Round(#T4.Imax,2), /*AS �q�y�̤j��,*/
               	             @iavgV = Round(#T4.Vavg,2), /*AS �q��������,*/
	           				 @imaxV = Round(#T4.Vmax,2), /*AS �q���̤j��,*/
               	             @iavgW = Round(#T4.Wavg,2), /*AS �\�v������,*/
               	             @imaxW = Round(#T4.Wmax,2)  /*AS �\�v�̤j��*/
           			  FROM #T4

               	      /*�M��̤j�q�y���Ӥ���ɶ�*/
               	      SELECT @dmaxIRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4 
                           where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate 
    	                   AND PowerRecord.Iavg=#T4.Imax

		           	  /*�M��̤j�q�����Ӥ���ɶ�*/
    	              SELECT @dmaxVRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4 
                           where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate 
    	                   AND PowerRecord.Vavg=#T4.Vmax

			          /*�M��̤j��\���Ӥ���ɶ�*/
    	              SELECT @dmaxWRD = cast(RecDate as nvarchar(20)) from PowerRecord,#T4
                           where CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate 
    	                   AND PowerRecord.W=#T4.Wmax
           
		           	  /*�̤j�ݶq*/
			          Select @iDeMand = MAX(DeMand),@iDeMandHalf = MAX(DeMandHalf),@iDeMandSatHalf = MAX(DeMandSatHalf),@iDeMandOff = MAX(DeMandOff)
		           	       From PowerRecord
		           	       Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID
    	                   And Convert(nvarchar(10),RecDate,120)=@sRecDate

    	              /*���o�e�@�Ѫ����*/
    	              select @Last_Day= cast(dateadd(day,-1,cast(@sRecDate as date)) as nvarchar(10));
			          set @Last_Day = Replace(@Last_Day,'-','/')
    	      
    	              /*�e�@�ѳ̫�@�����ιq�q*/
    	              Select Top 1 SUBSTRING(RecDate,1,10) AS ���,RushHour,HalfHour,SatHalfHour,OffHour,KWh into #T1
		           	       From PowerRecord 
		           	       Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And Convert(nvarchar(10),RecDate,120)=@Last_Day 
		           	       Order By RecDate DESC

			          /*���ѳ̫�@�����ιq�q*/
    	              Select Top 1 SUBSTRING(RecDate,1,10) AS ���,RushHour,HalfHour,SatHalfHour,OffHour,KWh into #T2
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

			          /*�H�e�@�ѳ̫�@���Τ���̫�@�����q�׭p��X�A�@�Ѫ��ιq�q*/
			          Select Top 1
				           @iRush = (CASE WHEN SUBSTRING(#T2.���,9,10)='01' THEN #T2.RushHour ELSE #T2.RushHour - #T3.RushHour END),                /* AS �y�p, */
				           @iHalf = (CASE WHEN SUBSTRING(#T2.���,9,10)='01' THEN #T2.HalfHour ELSE #T2.HalfHour - #T3.HalfHour END),                /* AS �b�y�p, */
                           @iSatHalf = (CASE WHEN SUBSTRING(#T2.���,9,10)='01' THEN #T2.SatHalfHour ELSE #T2.SatHalfHour - #T3.SatHalfHour END),    /* AS �g���b�y�p, */
                           @iOff = (CASE WHEN SUBSTRING(#T2.���,9,10)='01' THEN #T2.OffHour ELSE #T2.OffHour - #T3.OffHour END),                    /* AS ���p */
				           @iTodayUseKWh = (CASE WHEN (SUBSTRING(#T2.���,9,10)='01' AND (#T3.KWh>#T2.KWh)) THEN #T2.KWh ELSE #T2.KWh - #T3.KWh END) /* [�����q����ä��|�k�s]AS �����`�ιq�q*/
                           From #T2,#T3
    	      
			          Select Top 1 @iMaxRush = RushHour,@iMaxHalf = HalfHour,@iMaxSatHalf = SatHalfHour,@iMaxOff = OffHour
                           From PowerRecord
				           Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID	And SUBSTRING(RecDate,1,10)=@sRecDate
				           Order By RecDate DESC

			          Select Top 1 @iKWh = KWh From PowerRecord
				           Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID	And SUBSTRING(RecDate,1,10)=@sRecDate
				           Order By RecDate DESC

			          /*�N�e�@�Ѫ��ιq�q Temp�ɲ���*/
    	              Drop Table #T2,#T3

			          /*��X�C�몺�̫�@��*/
			          ;WITH LastDate(CtrlNr,MeterID,[Month],LastDate) AS
				      (
                        SELECT CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111) [���],MAX(RecDate) AS LastDate
					       FROM PowerRecordCollection
					       WHERE ctrlnr = @sCtrlNr AND MeterID = @sMeterID 
						   AND RecDate < CONVERT(NVARCHAR(7),@sRecDate,111)+'/01'
				           GROUP BY CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111)
                      )

			          /*�C��̫�@�Ѫ��ιq�q�`�M+����֭p�ܤ��Ѫ��ιq�q*/
				      SELECT @iTotalKWh=(SUM(KWH)+@iKWh)
				           FROM PowerRecordCollection AS A
                           INNER JOIN LastDate AS B ON A.CtrlNr=B.CtrlNr AND A.MeterID=B.MeterID  AND A.RecDate=B.LastDate

			          /*�s�WPowerRecordCollection���*/
    	              Exec Usp_AlterPowerRecordCollection @sCtrlNr, @sMeterID, @sRecDate, @iavgI, @imaxI, @iavgV, @iMaxV, @iAvgW, @imaxW, @iDeMand, @iDeMandHalf, @iDeMandSatHalf, @iDeMandOff, @iRush, @iHalf, @iSatHalf, @iOff, @iMaxRush, @iMaxHalf, @iMaxSatHalf, @iMaxOff,@iKWh, @MonthMark, @WeekMark, @dmaxIRD, @dmaxVRD, @dmaxWRD,@iTodayUseKWh,@iTotalKWh
                  end
               Else
                  begin 
    	              print '�S�q�q��ơG' + @sRecDate
                  end
               END

GO


