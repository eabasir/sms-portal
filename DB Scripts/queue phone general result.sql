/****** Script for SelectTopNRows command from SSMS  ******/
SELECT sp.TFId
, m.TFContent as Message
, sim.TFNumber as FromNumber
, phone.TFNumber as ToNumber
, myUser.TFUserName as UserName
      
  FROM [SMSPortalDB].[dbo].[SendBox_Phone] sp

  inner join SMSPortalDB.dbo.SendBox s

  on s.TFId = sp.TFSendBox_Id

  inner join SMSPortalDB.dbo.Message m

  on m.TFId = s.TFMessage_Id

  inner join SMSPortalDB.dbo.SIM sim
  on sim.TFId = sp.TFSim_Id

  inner join SMSPortalDB.dbo.Phone phone
  on phone.TFId = sp.TFPhone_Id

  inner join SMSPortalDB.dbo.[User]  myUser
on myUser.TFId = s.TFUser_Id

order by UserName , FromNumber



