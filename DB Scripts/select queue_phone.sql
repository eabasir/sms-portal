use SMSPortalDB
UPDATE Queue_Phone
SET TFIsUnderProcess = 1
OUTPUT INSERTED.TFId ,INSERTED.TFQueue_Id  , INSERTED.TFPhone_Id 
WHERE TFId IN 
(
   SELECT TOP 20 TFId
   FROM Queue_Phone WHERE TFQueue_Id in 
	(
		SELECT TFId FROM Queue where Queue.TFUser_Id in
			(
				select TFUser_Id from SIM_User where TFSIM_Id = '6a2b32a4-6fa0-415d-a242-5ae05762b0e7'
			)
			and TFScheduleType_Id = 0 
   )
   and  TFIsUnderProcess = 0
 
) 

