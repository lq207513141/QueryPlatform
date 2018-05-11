IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[sppbGetLoomStateQuery]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.[sppbGetLoomStateQuery]
IF OBJECT_ID(N'pbTableDefine') IS NOT NULL
    DELETE FROM dbo.pbTableDefine WHERE sTableName=N'sppbGetLoomStateQuery.sql'
GO

--获取织机实时状态 
CREATE PROCEDURE dbo.sppbGetLoomStateQuery
WITH ENCRYPTION
AS
BEGIN TRY
    SET NOCOUNT ON;

	IF OBJECT_ID(N'tempdb..#list') IS NOT NULL DROP TABLE #list
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

	--按坐标点更新数据
	DECLARE @k INT=1
	DECLARE @m INT=1
	WHILE @k<=@iMaxRow
	BEGIN
		SET @m=1
		WHILE @m<=@iMaxColumn
		BEGIN
			SET @sql = 'UPDATE A SET x'+ CONVERT(NVARCHAR(50),@m) + '='
			+''''+(SELECT A.sMachineName+'<br/>状态:'+(SELECT TOP 1 C.sStatusType FROM dbo.vwMachineMap B(NOLOCK) 
													  JOIN dbo.OpStatusType C(NOLOCK) ON C.iStatusID=B.iStatusID
													  WHERE B.iMachineID=A.iMachineID) 
			FROM dbo.OpMachine A(NOLOCK) WHERE A.iRowId=@k AND A.iColumnId=@m)+''''+
			--+(SELECT sMachineName FROM dbo.OpMachine(NOLOCK) WHERE iRowId=@k AND iColumnId=@m)
			+'FROM #list A WHERE id='+CONVERT(NVARCHAR(50),@k)
			PRINT @sql
			EXEC(@sql) 
			SET @m=@m+1
		END
		SET @k=@k+1
	END

	SELECT * FROM #list
END TRY
BEGIN CATCH
	DECLARE @sErrorMessage NVARCHAR(MAX)=REPLACE(ERROR_MESSAGE(),'%','%%')+CHAR(10)
    SET @sErrorMessage+='[ERROR]:'+LTRIM(STR(ERROR_NUMBER()))+'='+ISNULL(ERROR_PROCEDURE(),0x)+'.'+CONVERT(NVARCHAR,ERROR_LINE())
    RAISERROR(@sErrorMessage,16,1)
END CATCH
GO