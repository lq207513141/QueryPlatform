SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

IF EXISTS (SELECT * FROM DBO.SYSOBJECTS WHERE id = object_id(N'[dbo].[wCaiJiInfoNow]') AND OBJECTPROPERTY(id, N'IsView') = 1)
DROP VIEW [dbo].[wCaiJiInfoNow]
IF OBJECT_ID(N'pbTableDefine') IS NOT NULL
    DELETE FROM dbo.pbTableDefine WHERE sTableName=N'wCaiJiInfoNow.sql'
GO

--采集实时数据
CREATE VIEW [dbo].[vwCaiJiInfoNow]
AS
	SELECT A.iMachineID,A.iMachineGroupID,A.sSlaveNo,B.sGroup,A.saddressName
	,A.sValue,B.sUnit,A.iaddress,A.iOpprotocolid,C.sValueMean
	,sValueCaiji=CASE WHEN isnull(C.sValueMean,'')='' THEN A.sValue ELSE C.sValueMean END
	,A.tUpdateTime
	FROM dbo.OpprotocolValue A(NOLOCK) 
	LEFT JOIN dbo.OpprotocolMap B(NOLOCK) ON A.iOpprotocolid=B.iOpprotocolid and B.iaddress = A.iaddress
	LEFT JOIN dbo.OpprotocolMapDictionary C(NOLOCK) ON B.iaddress=C.iaddress AND B.iOpprotocolid=C.iOpprotocolid AND A.sValue=C.sValue
	WHERE A.isEnd=1
	--AND A.iMachineGroupID=
	--AND A.iMachineID=
	--AND A.sSlaveNo IN (SELECT sSlaveNo FROM dbo.OpprotocolSet A1(NOLOCK) WHERE A1.iMachineGroupID=)
	--AND A.iaddress IN (100,101,102,104,107,108,499,500)
GO