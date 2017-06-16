
Use ECOSMART

Insert into AdminCom
select 'admin', Account, '1' as Enabled, getdate() from [dbo].[AdminSetup]

Update [dbo].[AdminCom] set Enabled = 0 where Com='admin'

Insert into  AdminCom
Select A.Account, B.Com , 0 as Enabled, getdate() from [dbo].[AdminSetup] A, AdminCom B  
where A.Account<>'admin' and B.Com<>'admin'
order by Account, Com

Update [dbo].[AdminCom] set Enabled = 1 where account=Com and Com<>'admin'






