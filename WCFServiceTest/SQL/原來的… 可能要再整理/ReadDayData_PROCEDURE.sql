USE [Water]
GO

/****** Object:  StoredProcedure [dbo].[Usp_Day_Record]    Script Date: 2015/12/27 下午 05:39:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



		Create PROCEDURE [dbo].[ReadDayData] 
               @sRecDate        nvarchar(20)
           AS
           BEGIN
               SET NOCOUNT ON;	
	    
               declare @Last_Day  nvarchar(20)   /*尋找前一天之日期*/
                                      
	           declare @MonthMark nvarchar(2)    /*月份*/
	           declare @WeekMark  nvarchar(10)   /*星期一之日期*/
	           declare @ICOUNT    int 
			   declare @sType     int            /* 1. 前一天沒值,讀取當天最早的資料  2. 前一天有值,讀取前天最晚的資料*/
					  
    	       /*取得前一天的日期*/
    	       select @Last_Day= cast(dateadd(day,-1,cast(@sRecDate as date)) as nvarchar(10));
			   set @Last_Day = Replace(@Last_Day,'-','/')
			   set @sType = 0

               SET @ICOUNT=(SELECT COUNT(*) FROM Record Where SubString(Recdate ,1,10) = @Last_Day)

			   if @ICOUNT = 0
			      Begin
				      SET @ICOUNT=(SELECT COUNT(*) FROM Record Where SubString(Recdate ,1,10) = @sRecDate)
					  if @ICOUNT= 0 
					     Begin
						    print '查無' + @sRecDate + '水表值資料'
						 End 
					  else
					     Begin
						     set @sType = 1
						 End
				  End
			   else 
			      Begin
				      set @sType = 2
				  End


			   if @sType <> 0
			      Begin
				     if @sType = 1 
					    Begin
						   Select A.ID, A.圖面編號, A.店鋪編號, A.店鋪名稱
                                  , isnull((Select B.水表值 From Record B With (NoLock) 
                                            Where B.ID=A.ID and B.RecDate = (Select Min(C.RecDate) From Record C With (NoLock) 
                                            Where C.ID=A.ID and SubString(C.RecDate,1,10) = @sRecDate) ), 0) AS 水表值
                           into #Temp1
                           from Setup A  Order by A.ID
						End 

						
				     if @sType = 2 
					    Begin
						   Select A.ID, A.圖面編號, A.店鋪編號, A.店鋪名稱
                                  , isnull((Select B.水表值 From Record B With (NoLock) 
                                            Where B.ID=A.ID and B.RecDate = (Select Max(C.RecDate) From Record C With (NoLock) 
                                            Where C.ID=A.ID and SubString(C.RecDate,1,10) = @Last_Day) ), 0) AS 水表值
                           into #Temp2
                           from Setup A  Order by A.ID
						End 

					Select A.ID, A.圖面編號, A.店鋪編號, A.店鋪名稱
                           , isnull((Select B.水表值 From Record B With (NoLock) 
                                     Where B.ID=A.ID and B.RecDate = (Select Max(C.RecDate) From Record C With (NoLock) 
                                     Where C.ID=A.ID and SubString(C.RecDate,1,10) = @sRecDate) ), 0) AS 水表值
                    Into #Temp3
                    From Setup A  Order by A.ID


					
					if @sType = 1 
					   Begin
					      Select A.ID, A.圖面編號, A.店鋪編號, A.店鋪名稱, A.水表值, B.水表值, (A.水表值-B.水表值) as 用水量
                          From #Temp3 A
                          Left Join #Temp1 B On A.ID=B.ID
                          Order by A.ID  ;
					   End

					   
					if @sType = 2 
					   Begin
					      Select A.ID, A.圖面編號, A.店鋪編號, A.店鋪名稱, A.水表值, B.水表值, (A.水表值-B.水表值) as 用水量
                          From #Temp3 A
                          Left Join #Temp2 B On A.ID=B.ID
                          Order by A.ID  ;
					   End

				  End

           END

            

GO


