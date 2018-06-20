IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[sppbGetSteamerNow]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.[sppbGetSteamerNow]
IF OBJECT_ID(N'pbTableDefine') IS NOT NULL
    DELETE FROM dbo.pbTableDefine WHERE sTableName=N'sppbGetSteamerNow.sql'
GO

--获取蒸箱实时状态 
CREATE PROCEDURE dbo.sppbGetSteamerNow
AS
BEGIN TRY
    SET NOCOUNT ON;

	IF OBJECT_ID(N'tempdb..#list') IS NOT NULL DROP TABLE #list

	SELECT * 
	INTO #List
	FROM dbo.vwCaiJiInfoNow A
	WHERE A.iMachineGroupID=4
	AND A.sSlaveNo IN (SELECT sSlaveNo FROM dbo.OpprotocolSet A1(NOLOCK) WHERE A1.iMachineGroupID=4)
	AND A.iaddress IN (50,102,70,60,40,10,2,3,4,5,0,1,7,11,6)

	SELECT A.iMachineID,A.sMachineName,sWorkName=ISNULL(A.sWorkName,'未上机'),sTaskNo=ISNULL(A.sTaskNo1,'未上机')
	,sProductNo=ISNULL(A.sProductNo1,0x)
	,iState=(CASE WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=0)=1 THEN 0
			 WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=1)=1 THEN 1
			 WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=2)=1 THEN 2
			 WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=3)=1 THEN 3
			 WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=4)=1 THEN 4
			 WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=5)=1 THEN 5
			 WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=6)=1 THEN 6
			 WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=7)=1 THEN 7
			 WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=10)=1 THEN 10
			 WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=11)=1 THEN 11
			 WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=10)=0 THEN 99
			 ELSE 99 END
			 )
	--保温时间，温度，当前循环次数，当前恒温时间，当前设定温度
	,iHoldingTime=CONVERT(INT,ISNULL((SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=50),0))
	,nTemperature=CONVERT(DECIMAL(18,2),ISNULL((SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=102),0))
	,iRoundCount=CONVERT(INT,ISNULL((SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=70),0))
	,iConstantTime=CONVERT(INT,ISNULL((SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=60),0))
	,iTemperatureSet=CONVERT(INT,ISNULL((SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=40),0))
	FROM dbo.psYarnMachine A(NOLOCK)
	WHERE A.iMachineGroupID=4
	ORDER BY A.iMachineID

	IF OBJECT_ID(N'tempdb..#list') IS NOT NULL DROP TABLE #list
END TRY
BEGIN CATCH
	DECLARE @sErrorMessage NVARCHAR(MAX)=REPLACE(ERROR_MESSAGE(),'%','%%')+CHAR(10)
    SET @sErrorMessage+='[ERROR]:'+LTRIM(STR(ERROR_NUMBER()))+'='+ISNULL(ERROR_PROCEDURE(),0x)+'.'+CONVERT(NVARCHAR,ERROR_LINE())
    RAISERROR(@sErrorMessage,16,1)
END CATCH
GO