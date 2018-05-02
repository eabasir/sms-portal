/****** Script for SelectTopNRows command from SSMS  ******/
use SMSPortalDB
SELECT simUser.[TFId]
      ,[TFSIM_Id]
      ,[TFName] , [TFFamily]
      ,[TFMax] , [TFNumber]
  FROM 
  [SMSPortalDB].[dbo].[SIM_User] simUser

  inner join [SMSPortalDB].dbo.SIM sim

  on sim.TFId = simUser.TFSIM_Id

  inner join [SMSPortalDB].[dbo].[User] us
  on us.TFId = simUser.TFUser_Id
  
  where TFSIM_Id = '579e9e0d-c1bf-4256-81bb-c1f44f0efe2b'
  

  
