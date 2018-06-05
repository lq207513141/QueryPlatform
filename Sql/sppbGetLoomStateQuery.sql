IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[sppbGetLoomStateQuery]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.[sppbGetLoomStateQuery]
IF OBJECT_ID(N'pbTableDefine') IS NOT NULL
    DELETE FROM dbo.pbTableDefine WHERE sTableName=N'sppbGetLoomStateQuery.sql'
GO

--获取织机实时状态 
CREATE PROCEDURE dbo.sppbGetLoomStateQuery(
@iType INT=0		--参数:1_车速,2_效率,3_打纬,4_运行时间,5_停机时间
)
WITH ENCRYPTION
AS
BEGIN TRY
    SET NOCOUNT ON;

	IF OBJECT_ID(N'tempdb..#list') IS NOT NULL DROP TABLE #list
	IF OBJECT_ID(N'tempdb..#stateList') IS NOT NULL DROP TABLE #stateList
	IF OBJECT_ID(N'tempdb..#vwMachineMap') IS NOT NULL DROP TABLE #vwMachineMap

	SELECT *
	INTO #vwMachineMap
    FROM dbo.vwMachineMap(NOLOCK)

	CREATE TABLE #list(
	id INT
	)
	--取最大列数与最大行数
	DECLARE @iMaxColumn INT=0,@iMaxRow INT=0
	SELECT @iMaxColumn=MAX(iColumnId),@iMaxRow=MAX(iRowId) FROM dbo.OpMachine

	--增加临时表的列
	DECLARE @sql NVARCHAR(MAX)=0x
	DECLARE @i INT=1
	WHILE @i<=@iMaxColumn
	BEGIN
		SET @sql = 'ALTER TABLE #list ADD x' + CONVERT(NVARCHAR(50),@i) + ' NVARCHAR(MAX)'
		EXEC(@sql)
		SET @i=@i+1
	END

	--插入临时表数据
	DECLARE @j INT=1
	WHILE @j<=@iMaxRow
	BEGIN
		INSERT INTO #list(id)VALUES(@j)
		SET @j=@j+1
	END

	--查询机台状态参数
	SELECT X.iMachineID,X.iPickCount
	,nBancieff=CONVERT(DECIMAL(18, 2),CASE WHEN (X.iRunTime + X.iAllStopTime)=0 THEN 0 ELSE X.iRunTime * 100.0/ (X.iRunTime + X.iAllStopTime)END)
	,sBancieff=CONVERT(NVARCHAR(50),CONVERT(DECIMAL(18,2), CASE WHEN (X.iRunTime + X.iAllStopTime)=0 THEN 0 ELSE X.iRunTime * 100.0/ (X.iRunTime + X.iAllStopTime)END))+'%'
	,nBanciSpeed=CASE WHEN X.iRunTime =0 THEN 0 ELSE  CONVERT(DECIMAL(18,0),(X.iPickCount*60.0)/iRunTime) END
	,sRunTime=CONVERT(NVARCHAR(50),X.iRunTime/3600)+'小时'+CONVERT(NVARCHAR(50),X.iRunTime%3600/60)+'分'
	,sAllStopTime=CONVERT(NVARCHAR(50),X.iAllStopTime/3600)+'小时'+CONVERT(NVARCHAR(50),X.iAllStopTime%3600/60)+'分'
	INTO #stateList
	FROM(SELECT A.iMachineID
		 ,iRunTime=SUM(ISNULL(B.iRunTime, 0))
		 ,iAllStopTime=SUM(ISNULL(B.iAllStopTime, 0))
		 ,iPickCount=SUM(ISNULL(B.iPickCount, 0))
		 FROM dbo.OpMachine A(NOLOCK) 
		 LEFT JOIN dbo.OpClassDataList B(NOLOCK) ON B.iMachineID = A.iMachineID AND B.iClassListID = dbo.fnzzGetClassid('NOW')
		 GROUP BY A.iMachineID)X

	--按坐标点更新数据
	DECLARE @k INT=1
	DECLARE @m INT=1
	WHILE @k<=@iMaxRow
	BEGIN
		SET @m=1
		WHILE @m<=@iMaxColumn
		BEGIN
			/*0: { 'title': '运转', 'class': 'btn-success' },
            1: { 'title': '纬停', 'class': 'btn-yellow' },
            2: { 'title': '经停', 'class': 'btn-danger' },
            3: { 'title': '绞边停', 'class': 'btn-warning' },
            4: { 'title': '耳丝停', 'class': 'btn-primary' },
            5: { 'title': '断纱停', 'class': 'btn-info' },
            6: { 'title': '满匹停', 'class': 'btn-lightcyan' },
            7: { 'title': '手动停', 'class': 'btn-yellow' },
            8: { 'title': '故障', 'class': 'btn-outline-metal active' },
            9: { 'title': '离线', 'class': 'btn-outline-metal active' },
            10: { 'title': '其他停', 'class': 'btn-lightsalmon' },
			*/
			IF NOT EXISTS(SELECT TOP 1 1 FROM dbo.OpMachine A(NOLOCK) WHERE A.iRowId=@k AND A.iColumnId=@m)
			BEGIN
				SET @m=@m+1
				CONTINUE;
			END
			SET @sql = 'UPDATE A SET x'+ CONVERT(NVARCHAR(50),@m) + '='
			+''''
			+(SELECT '<button type="button" class="btn m-btn--pill '
			+(SELECT TOP 1 CASE B.iStatusID 
						   WHEN 0 THEN 'btn-success'
						   WHEN 1 THEN 'btn-yellow'
						   WHEN 2 THEN 'btn-danger'
						   WHEN 3 THEN 'btn-warning'
						   WHEN 4 THEN 'btn-primary'
						   WHEN 5 THEN 'btn-info'
						   WHEN 6 THEN 'btn-lightcyan'
						   WHEN 7 THEN 'btn-yellow'
						   WHEN 8 THEN 'btn-outline-metal active'
						   WHEN 9 THEN 'btn-outline-metal active'
						   WHEN 10 THEN 'btn-lightsalmon'
						   ELSE '' END
			  FROM #vwMachineMap B(NOLOCK) 
			  WHERE B.iMachineID=A.iMachineID)
			+' m-btn m-btn--custom">'
			+'机台:'+A.sMachineName+'<br/>状态:'+(SELECT TOP 1 B.sStatusType FROM #vwMachineMap B(NOLOCK) 
												  WHERE B.iMachineID=A.iMachineID)
			--1_车速,2_效率,3_打纬,4_运行时间,5_停机时间
			+(SELECT CASE @iType WHEN 1 THEN '<br/>车速:'+CONVERT(NVARCHAR(50),A1.nBanciSpeed)
					 WHEN 2 THEN '<br/>'+A1.sBancieff
					 WHEN 3 THEN '<br/>'+CONVERT(NVARCHAR(50),A1.iPickCount)
					 WHEN 4 THEN '<br/>'+A1.sRunTime
					 WHEN 5 THEN '<br/>'+A1.sAllStopTime
					 ELSE '' END
			  FROM #stateList A1(NOLOCK) 
			  WHERE A1.iMachineID=A.iMachineID)
			FROM dbo.OpMachine A(NOLOCK) WHERE A.iRowId=@k AND A.iColumnId=@m)
			+ '</button >'+''''
			+'FROM #list A WHERE id='+CONVERT(NVARCHAR(50),@k)
			--PRINT @sql
			EXEC(@sql)
			SET @m=@m+1
		END
		SET @k=@k+1
	END
	
	--返回数据集
	SELECT * FROM #list

	IF OBJECT_ID(N'tempdb..#list') IS NOT NULL DROP TABLE #list
	IF OBJECT_ID(N'tempdb..#stateList') IS NOT NULL DROP TABLE #stateList
	IF OBJECT_ID(N'tempdb..#vwMachineMap') IS NOT NULL DROP TABLE #vwMachineMap
END TRY
BEGIN CATCH
	DECLARE @sErrorMessage NVARCHAR(MAX)=REPLACE(ERROR_MESSAGE(),'%','%%')+CHAR(10)
    SET @sErrorMessage+='[ERROR]:'+LTRIM(STR(ERROR_NUMBER()))+'='+ISNULL(ERROR_PROCEDURE(),0x)+'.'+CONVERT(NVARCHAR,ERROR_LINE())
    RAISERROR(@sErrorMessage,16,1)
END CATCH
GO