IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[sppbGetWarpingNow]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.[sppbGetWarpingNow]
IF OBJECT_ID(N'pbTableDefine') IS NOT NULL
    DELETE FROM dbo.pbTableDefine WHERE sTableName=N'sppbGetWarpingNow.sql'
GO

--获取扦经实时状态 
CREATE PROCEDURE dbo.sppbGetWarpingNow
AS
BEGIN TRY
    SET NOCOUNT ON;

	IF OBJECT_ID(N'tempdb..#list') IS NOT NULL DROP TABLE #list

	SELECT * 
	INTO #List
	FROM dbo.vwCaiJiInfoNow A
	WHERE A.iMachineGroupID=21
	AND A.sSlaveNo IN (SELECT sSlaveNo FROM dbo.OpprotocolSet A1(NOLOCK) WHERE A1.iMachineGroupID=21)
	AND A.iaddress IN (100,101,102,104,107,108,499,500)

	SELECT A.iMachineID,A.sMachineName,sWorkName=ISNULL(A.sWorkName,'未上机'),sTaskNo=ISNULL(A.sTaskNo1,'未上机')
	,nSpeed=A.nSpeed1,nEfficiency=A.nEfficiency1,sProductNo=ISNULL(A.sProductNo1,0x)
	,iState=(CASE WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=100)>0 THEN 0
			 WHEN (SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=108)>0 THEN 2
			 ELSE 1 END)
	--当前纱长，断头个数，当前条数，设定纱长，设定条数
	,iYarnLength=CONVERT(INT,(SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=102))
	,iBrokenCount=CONVERT(INT,(SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=104))
	,iBarCount=CONVERT(INT,(SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=101))
	,iYarnSet=CONVERT(INT,(SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=499))
	,iBarSet=CONVERT(INT,(SELECT A1.sValue FROM #List A1 WHERE A1.iMachineID=A.iMachineID AND A1.iaddress=500))
	FROM dbo.psYarnMachine A(NOLOCK)
	WHERE A.iMachineGroupID=21
	ORDER BY A.iMachineID

	IF OBJECT_ID(N'tempdb..#list') IS NOT NULL DROP TABLE #list
END TRY
BEGIN CATCH
	DECLARE @sErrorMessage NVARCHAR(MAX)=REPLACE(ERROR_MESSAGE(),'%','%%')+CHAR(10)
    SET @sErrorMessage+='[ERROR]:'+LTRIM(STR(ERROR_NUMBER()))+'='+ISNULL(ERROR_PROCEDURE(),0x)+'.'+CONVERT(NVARCHAR,ERROR_LINE())
    RAISERROR(@sErrorMessage,16,1)
END CATCH
GO