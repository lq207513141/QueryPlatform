SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

IF EXISTS (SELECT * FROM DBO.SYSOBJECTS WHERE id = object_id(N'[dbo].[vwZhouKu]') AND OBJECTPROPERTY(id, N'IsView') = 1)
DROP VIEW [dbo].[vwZhouKu]
IF OBJECT_ID(N'pbTableDefine') IS NOT NULL
    DELETE FROM dbo.pbTableDefine WHERE sTableName=N'vwZhouKu.sql'
GO

CREATE VIEW [dbo].[vwZhouKu]
AS
	SELECT 
	B.sGroup AS kuwei,
	dbo.fnpbConcatStringEx(  CASE WHEN A.saddressName='布号' THEN  A.sValue ELSE '' END,'')  AS sMaterialNo,
	dbo.fnpbConcatStringEx( CASE WHEN A.saddressName='并经米长' THEN  A.sValue ELSE '' END,'') AS nLength,
	dbo.fnpbConcatStringEx( CASE WHEN A.saddressName='日期' THEN  A.sValue ELSE '' END,'') AS tdate,
	dbo.fnpbConcatStringEx( CASE WHEN A.saddressName='类别' THEN  A.sValue ELSE '' END,'') AS sType
	FROM dbo.OpprotocolValue A (NOLOCK)
	JOIN dbo.OpprotocolMap B ON A.iOpprotocolid=B.iOpprotocolid AND A.iaddress=B.iaddress
	 WHERE A.iMachineGroupID=7
	AND A.isEnd=1
	GROUP BY B.sGroup
GO