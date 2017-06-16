USE [ECO_eco]
GO

/****** Object:  StoredProcedure [dbo].[Usp_LostDayCheck]    Script Date: 2016/8/6 下午 10:44:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




		  
		  CREATE PROCEDURE [dbo].[Usp_LostDayCheck]
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
		   	        exec Usp_TriggerDayRecordCollection @CtrlNr,@MeterID, @RecDate
			   	    FETCH NEXT FROM addr_cursor INTO @CtrlNr, @MeterID, @RecDate
		     	 END
		  
		   	   CLOSE addr_cursor        /*關閉Cursor*/
		   	   DEALLOCATE addr_cursor   /*釋放Cursor*/
           END



GO


