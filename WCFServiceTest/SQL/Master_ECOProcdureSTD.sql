CREATE PROCEDURE [dbo].[Usp_ECOCollection]
    @sName         nvarchar(10),
    @sRecDate         nvarchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @strDate nVarchar(10)
	
	declare @CtrlNr  int
	declare @MeterID int
	declare @name    nVarchar(100)   /*��Ʈw�W��*/
	declare @strSQL  nVarchar(200)   /*���O*/
	
	set @name = Replace(@sName,'ECO_','');
	print @name;
	DECLARE Cursor_tmp1 CURSOR FOR   /*�إ�Cursor*/
            (select B.CtrlNr as CtrlNr , B.MeterID as MeterID From ECOSMART.[dbo].[ControllerSetup] A  with (Nolock) 
             Left join ECOSMART.[dbo].[MeterSetup] B  with (Nolock) on B.ECO_Account=A.ECO_Account and B.Enabled=1 Where A.Account=@name)

	OPEN Cursor_tmp1              /*�}��Cursor*/
    FETCH NEXT FROM Cursor_tmp1 INTO  @CtrlNr, @MeterID    /*�N�ȩ�J�ܼ�*/
	WHILE @@FETCH_STATUS = 0     /*���^�ǭ�*/
    BEGIN
	
	   set @strSQL = '[dbo].[Usp_TriggerDayRecordCollection] '+ ' ' + rtrim(cast(@CtrlNr as char)) + ',' + rtrim(cast(@MeterID as char)) + ',''' + @sRecDate+'''';
	   print @strSQL;
	   exec (@strSQL);  

	   FETCH NEXT FROM Cursor_tmp1 INTO  @CtrlNr, @MeterID  /*�N�ȩ�J�ܼ�	*/
    END
    CLOSE Cursor_tmp1         /*����Cursor*/
    DEALLOCATE Cursor_tmp1    /*����Cursor*/

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
	           @TotalKWh	int,
	           @starKWh	int
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
	                         @ImaxRD, @VmaxRD, @WmaxRD,@TodayUseKWh,@TotalKWh,@starKWh
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
				         TotalKWh=@TotalKWh,
						 starKWh=@starKWh 
		                 where CtrlNr=@CtrNr and MeterID=@MeterID and Recdate=@RecDate
	                     /*Print '��s���PowerRecordCollection���\!! �i' + rtrim(cast(@CtrNr as char)) + '~' + rtrim(cast(@MeterID as char)) + '~' + rtrim(cast(@RecDate as char)) + '�j'*/
	             End
	   	         Return  
           END
$
CREATE PROCEDURE [dbo].[Usp_TriggerDayRecordCollection] 
@sCtrlNr  int,
@sMeterID int,
@sRecDate nvarchar(10)
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
  SET @ICOUNT=(SELECT COUNT(*) FROM PowerRecord WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND RecDate=@sRecDate)

  If @ICOUNT  > 0
     Begin
     SELECT AVG(Iavg) AS Iavg,MAX(Iavg) AS Imax,AVG(Vavg) AS Vavg,MAX(Vavg) AS Vmax,AVG(W) AS Wavg,MAX(W) AS Wmax 
	 ,MAX(DeMand) as DeMand, MAX(DeMandHalf) as DeMandHalf, MAX(DeMandSatHalf) as DeMandSatHalf, MAX(DeMandOff) as DeMandOff
     into #T4 FROM PowerRecord with (Nolock)
     WHERE CtrlNr=@sCtrlNr AND MeterID=@sMeterID AND RecDate=@sRecDate
    	      
     SELECT @imaxI = Round(#T4.Imax,2) /*AS �q�y�̤j��*/
     ,@imaxV = Round(#T4.Vmax,2)       /*AS �q���̤j��*/
     ,@imaxW = Round(#T4.Wmax,2)       /*AS �\�v�̤j��*/
     ,@iDeMand = #T4.DeMand
     ,@iDeMandHalf = #T4.DeMandHalf
     ,@iDeMandSatHalf = #T4.DeMandSatHalf
     ,@iDeMandOff = #T4.DeMandOff
     FROM #T4
		   
     set @iavgI = 0    /* �q�y������,*/
     set @iavgV = 0    /* �q��������,*/
     set @iavgW = 0    /* �\�v������*/
     set @dmaxIRD = '' /* �̤j�q�y���Ӥ���ɶ�*/
     set @dmaxVRD = '' /* �̤j�q�����Ӥ���ɶ�*/
     set @dmaxWRD = '' /* �̤j��\���Ӥ���ɶ�*/
	 /*
     /*�̤j�ݶq*/
     Select @iDeMand = MAX(DeMand),@iDeMandHalf = MAX(DeMandHalf),@iDeMandSatHalf = MAX(DeMandSatHalf),@iDeMandOff = MAX(DeMandOff)
     From PowerRecord with (Nolock)
     Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID
     And Convert(nvarchar(10),RecDate,120)=@sRecDate
	 */

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
					  
     declare @TM_���      nVarChar(10)
     declare @TM_RushHour     int
     declare @TM_HalfHour     int
     declare @TM_SatHalfHour  int
     declare @TM_OffHour      int
     declare @TM_KWh          int
     
     /*���o�e�@�Ѫ����*/
     select @Last_Day= cast(dateadd(day,-1,cast(@sRecDate as date)) as nvarchar(10));
     set @Last_Day = Replace(@Last_Day,'-','/')
			  
     /*���ѳ̫�@�����ιq�q*/
     Select Top 1 @T1_���=RecDate, @T1_RushHour = RushHour, @T1_HalfHour = HalfHour, @T1_SatHalfHour = SatHalfHour, @T1_OffHour = OffHour, @T1_KWh = KWh
	 ,@iMaxRush = RushHour,@iMaxHalf = HalfHour,@iMaxSatHalf = SatHalfHour,@iMaxOff = OffHour, @iKWh = KWh
     From PowerRecord with (Nolock)
     Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And RecDate=@sRecDate 
     Order By RecDate DESC, RecTime DESC
	 
     /*�e�@�ѳ̫�@�����ιq�q*/
     Set @ICOUNT = (Select Count(*) From PowerRecord With (Nolock) 
     Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And RecDate=@Last_Day)
     if @ICOUNT > 0
     /*�e�@�Ѧ����, Ū���e�@�Ѹ�ƪ��̫�@��*/
        Begin
           Select Top 1 @T2_���=RecDate, @T2_RushHour = RushHour, @T2_HalfHour = HalfHour, @T2_SatHalfHour = SatHalfHour, @T2_OffHour = OffHour, @T2_KWh = KWh
           From PowerRecord with (Nolock) 
           Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And RecDate=@Last_Day 
           Order By RecDate DESC, RecTime DESC
        End
     Else	/*�e�@�ѨS���, Ū����Ѹ�ƪ��Ĥ@��*/  
        Begin
           Select Top 1 @T2_���=RecDate, @T2_RushHour = RushHour, @T2_HalfHour = HalfHour, @T2_SatHalfHour = SatHalfHour, @T2_OffHour = OffHour, @T2_KWh = KWh
           From PowerRecord with (Nolock) 
           Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And RecDate=@sRecDate 
           Order By RecDate ASC, RecTime ASC
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
	/*
     Select Top 1 @iMaxRush = RushHour,@iMaxHalf = HalfHour,@iMaxSatHalf = SatHalfHour,@iMaxOff = OffHour, @iKWh = KWh
     From PowerRecord with (Nolock)
     Where CtrlNr=@sCtrlNr AND MeterID=@sMeterID And RecDate=@sRecDate
     Order By RecDate DESC
	 */

     /*��X�C�몺�̫�@��*/
     ;WITH LastDate(CtrlNr,MeterID,[Month],LastDate) AS
     (
     SELECT CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111) [���],MAX(RecDate) AS LastDate
     FROM Master.dbo.ECO_12868358_PowerRecordCollection with (Nolock)
     WHERE ctrlnr = @sCtrlNr AND MeterID = @sMeterID 
     AND RecDate < CONVERT(NVARCHAR(7),@sRecDate,111)+'/01'
     GROUP BY CtrlNr,MeterID,CONVERT(NVARCHAR(7),RecDate,111)
     )

     /*�C��̫�@�Ѫ��ιq�q�`�M+����֭p�ܤ��Ѫ��ιq�q*/
     SELECT @iTotalKWh=(SUM(KWH)+@iKWh)
     FROM Master.dbo.ECO_12868358_PowerRecordCollection AS A
     INNER JOIN LastDate AS B ON A.CtrlNr=B.CtrlNr AND A.MeterID=B.MeterID  AND A.RecDate=B.LastDate
	 
     /*�s�WPowerRecordCollection���*/
     --Exec Usp_AlterPowerRecordCollection @sCtrlNr, @sMeterID, @sRecDate, @iavgI, @imaxI, @iavgV, @iMaxV, @iAvgW, @imaxW, @iDeMand, @iDeMandHalf, @iDeMandSatHalf, @iDeMandOff, @iRush, @iHalf, @iSatHalf, @iOff, @iMaxRush, @iMaxHalf, @iMaxSatHalf, @iMaxOff,@iKWh, @MonthMark, @WeekMark, @dmaxIRD, @dmaxVRD, @dmaxWRD,@iTodayUseKWh,@iTotalKWh
     Exec Usp_AlterPowerRecordCollection @sCtrlNr, @sMeterID, @sRecDate, @iavgI, @imaxI, @iavgV, @iMaxV, @iAvgW, @imaxW, @iDeMand, @iDeMandHalf, @iDeMandSatHalf, @iDeMandOff, @iRush, @iHalf, @iSatHalf, @iOff, @T1_RushHour, @T1_HalfHour, @T1_SatHalfHour, @iMaxOff,@iKWh, @MonthMark, @WeekMark, @dmaxIRD, @dmaxVRD, @dmaxWRD,@iTodayUseKWh,@iTotalKWh
	 
     end
     Else
     begin 
        print '�S�q�q��ơG' + @sRecDate
     end
     END
$
CREATE PROCEDURE [dbo].[Usp_LostDayCheck]
           AS
           BEGIN
	           SET NOCOUNT ON;
	
	           DECLARE @CtrlNr int
           	   DECLARE @MeterID int
		   DECLARE @RecDate varchar(10)


	           declare @strDate nVarchar(10)
                   set @strDate = Replace(CAST(cast(DATEADD(day,-1,GetDate()) as date) AS CHAR),'-','/')


		   	   DECLARE addr_cursor CURSOR FOR
		   	   (Select distinct CtrlNr, MeterID, Recdate 
		   	      from [dbo].[PowerRecord] A 
		   	      where not exists (select 1 from [dbo].[PowerRecordCollection] B 
		   	                        where B.CtrlNr=A.CtrlNr and B.MeterID=A.MeterID and B.RecDate=A.RecDate ) and RecDate < @strDate )

		   	   OPEN addr_cursor   /*�}��Cursor*/
		   	   FETCH NEXT FROM addr_cursor INTO @CtrlNr, @MeterID, @RecDate
		   	   WHILE @@FETCH_STATUS = 0
		   	     BEGIN
		   	        exec Usp_TriggerDayRecordCollection @CtrlNr,@MeterID, @RecDate
			   	    FETCH NEXT FROM addr_cursor INTO @CtrlNr, @MeterID, @RecDate
		     	 END
		  
		   	   CLOSE addr_cursor        /*����Cursor*/
		   	   DEALLOCATE addr_cursor   /*����Cursor*/
           END
$
