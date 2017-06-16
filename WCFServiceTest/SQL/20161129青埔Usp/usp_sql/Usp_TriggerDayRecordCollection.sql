USE [ECO_secom1]
GO

/****** Object:  StoredProcedure [dbo].[Usp_TriggerDayRecordCollection]    Script Date: 2016/11/29 �U�� 04:58:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







		CREATE PROCEDURE [dbo].[Usp_TriggerDayRecordCollection] 
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

			   /*�P�_��ѬO�_�����*/
               SET @ICOUNT=(SELECT COUNT(*) FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND substring(RecDate,1,10)=@sRecDate)
               If @ICOUNT  > 0
                  Begin
               	      /*SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax,AVG(W) AS Wavg,MAX(W) AS Wmax 
               	      into #T4 FROM PowerRecord with (Nolock)
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
                      */

					 set @iavgI = 0 /* �q�y������,*/
					 set @iavgV = 0 /* �q��������,*/
					 set @iavgW = 0 /* �\�v������*/
					 set @dmaxIRD = '' /* �̤j�q�y���Ӥ���ɶ�*/
					 set @dmaxVRD = '' /* �̤j�q�����Ӥ���ɶ�*/
					 set @dmaxWRD = '' /* �̤j��\���Ӥ���ɶ�*/


		           	  /*�̤j�ݶq*/
			          Select @imaxI=MAX(Iavg), @imaxV=MAX(Vavg), @imaxW=MAX(W)
					        ,@iDeMand = MAX(DeMand),@iDeMandHalf = MAX(DeMandHalf),@iDeMandSatHalf = MAX(DeMandSatHalf),@iDeMandOff = MAX(DeMandOff)
		           	       From PowerRecord with (Nolock)
		           	       Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And substring(RecDate,1,10)=@sRecDate

                      /*����*/
					  declare @T1_���      nVarChar(10)
					  declare @T1_RushHour     int
					  declare @T1_HalfHour     int
					  declare @T1_SatHalfHour  int
					  declare @T1_OffHour      int
					  declare @T1_KWh          int
					  
                      /*�Q��*/
					  declare @T2_���      nVarChar(10)
					  declare @T2_RushHour     int
					  declare @T2_HalfHour     int
					  declare @T2_SatHalfHour  int
					  declare @T2_OffHour      int
					  declare @T2_KWh          int

    	              /*���o�e�@�Ѫ����*/
    	              select @Last_Day= cast(dateadd(day,-1,cast(@sRecDate as date)) as nvarchar(10));
			          set @Last_Day = Replace(@Last_Day,'-','/')
			  
			          /*���ѳ̫�@�����ιq�q*/
    	              Select Top 1 @T1_���=SUBSTRING(RecDate,1,10), @T1_RushHour = RushHour, @T1_HalfHour = HalfHour, @T1_SatHalfHour = SatHalfHour, @T1_OffHour = OffHour, @T1_KWh = KWh
    	                   From PowerRecord with (Nolock)
    	                   Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And substring(RecDate,1,10)=@sRecDate
    	                   Order By RecDate DESC

    	              /*�e�@�ѳ̫�@�����ιq�q*/
					  Set @ICOUNT = (Select Count(*) From PowerRecord With (Nolock) 
					                 Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And substring(RecDate,1,10)=@Last_Day)
					  if @ICOUNT > 0
					     /*�e�@�Ѧ����, Ū���e�@�Ѹ�ƪ��̫�@��*/
					     Begin
    	                    Select Top 1 @T2_���=SUBSTRING(RecDate,1,10), @T2_RushHour = RushHour, @T2_HalfHour = HalfHour, @T2_SatHalfHour = SatHalfHour, @T2_OffHour = OffHour, @T2_KWh = KWh
		           	        From PowerRecord with (Nolock) 
		           	        Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And substring(RecDate,1,10)=@Last_Day
		           	        Order By RecDate DESC
						 End
					  Else	/*�e�@�ѨS���, Ū����Ѹ�ƪ��Ĥ@��*/  
					     Begin
    	                    Select Top 1 @T2_���=SUBSTRING(RecDate,1,10), @T2_RushHour = RushHour, @T2_HalfHour = HalfHour, @T2_SatHalfHour = SatHalfHour, @T2_OffHour = OffHour, @T2_KWh = KWh
		           	        From PowerRecord with (Nolock) 
		           	        Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And substring(RecDate,1,10)=@sRecDate
		           	        Order By RecDate ASC
						 End 
			          /*�H[�e�@�ѳ̫�@��]��[����̫�@��]���q�q�p��X�A�@�Ѫ��ιq�q*/

					  
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
			          /*��X�C�몺�̫�@��*/
			          ;WITH LastDate(CtrlNr,MeterID,[Month],LastDate) AS
				      (
                        SELECT CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111) [���],MAX(RecDate) AS LastDate
					       FROM PowerRecordCollection with (Nolock)
					       WHERE ctrlnr = @sCtrlNr AND MeterID = @sMeterID 
						   AND RecDate < CONVERT(NVARCHAR(7),@sRecDate,111)+'/01'
				           GROUP BY CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111)
                      )

			          /*�C��̫�@�Ѫ��ιq�q�`�M+����֭p�ܤ��Ѫ��ιq�q*/
				      SELECT @iTotalKWh=(SUM(KWH)+@iKWh)
				           FROM PowerRecordCollection AS A
                           INNER JOIN LastDate AS B ON A.CtrlNr=B.CtrlNr AND A.MeterID=B.MeterID  AND A.RecDate=B.LastDate
                      */
					  
					  set @iTodayUseKWh = 0   /*����`�M*/
					  set @iTotalKWh = 0      /*����`�M*/  
			          /*�s�WPowerRecordCollection���*/
    	              Exec Usp_AlterPowerRecordCollection @sCtrlNr, @sMeterID, @sRecDate, @iavgI, @imaxI, @iavgV, @iMaxV, @iAvgW, @imaxW, @iDeMand, @iDeMandHalf, @iDeMandSatHalf, @iDeMandOff, @iRush, @iHalf, @iSatHalf, @iOff, @iMaxRush, @iMaxHalf, @iMaxSatHalf, @iMaxOff,@iKWh, @MonthMark, @WeekMark, @dmaxIRD, @dmaxVRD, @dmaxWRD,@iTodayUseKWh,@iTotalKWh
                  end
               Else
                  begin 
    	              print '�S�q�q��ơG' + @sRecDate
                  end
               END





GO


