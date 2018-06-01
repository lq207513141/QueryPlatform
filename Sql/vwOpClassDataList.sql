SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

IF EXISTS (SELECT * FROM DBO.SYSOBJECTS WHERE id = object_id(N'[dbo].[vwOpClassDataList]') AND OBJECTPROPERTY(id, N'IsView') = 1)
DROP VIEW [dbo].[vwOpClassDataList]
IF OBJECT_ID(N'pbTableDefine') IS NOT NULL
    DELETE FROM dbo.pbTableDefine WHERE sTableName=N'vwOpClassDataList.sql'
GO

CREATE VIEW [dbo].[vwOpClassDataList]
AS
	SELECT 
CONVERT(NVARCHAR(50),A.iMachineID) iMachineID,
       SUM(ISNULL(A.iRunTime,0) )iRunTime,
       SUM(ISNULL(A.iPickCount,0)) iPickCount ,
       CONVERT(INT,SUM(ISNULL(A.iAllStopTime,0))/60)  iAllStopTime,
       SUM(ISNULL(A.iAllStopCount,0))  iAllStopCount,
	   sStatusType1=(SELECT G.sStatusType FROM dbo.OpStatusType G (NOLOCK)  WHERE G.sStatusTime='iStatusTime1'),
         CONVERT(INT,SUM(ISNULL(A.iStatusTime1 ,0))/60) iStatusTime1,
       SUM(ISNULL(A.iStatusCount1,0))  iStatusCount1,
	   sStatusType2=(SELECT G.sStatusType FROM dbo.OpStatusType G (NOLOCK)  WHERE G.sStatusTime='iStatusTime2'),
         CONVERT(INT,SUM(ISNULL(A.iStatusTime2 ,0))/60) iStatusTime2,
       SUM(ISNULL(A.iStatusCount2,0))  iStatusCount2,
	    sStatusType3=(SELECT G.sStatusType FROM dbo.OpStatusType G (NOLOCK)  WHERE G.sStatusTime='iStatusTime3'),
         CONVERT(INT,SUM(ISNULL( A.iStatusTime3 ,0))/60) iStatusTime3,
       SUM(ISNULL(A.iStatusCount3 ,0))iStatusCount3 ,
	    sStatusType4=(SELECT G.sStatusType FROM dbo.OpStatusType G (NOLOCK)  WHERE G.sStatusTime='iStatusTime4'),
         CONVERT(INT,SUM(ISNULL(A.iStatusTime4 ,0))/60) iStatusTime4,
       SUM(ISNULL(A.iStatusCount4 ,0)) iStatusCount4,
	    sStatusType5=(SELECT G.sStatusType FROM dbo.OpStatusType G (NOLOCK)  WHERE G.sStatusTime='iStatusTime5'),
         CONVERT(INT,SUM(ISNULL(A.iStatusTime5 ,0))/60) iStatusTime5,
       SUM(ISNULL(A.iStatusCount5,0))  iStatusCount5,
	    sStatusType6=(SELECT G.sStatusType FROM dbo.OpStatusType G (NOLOCK)  WHERE G.sStatusTime='iStatusTime6'),
         CONVERT(INT,SUM(ISNULL(A.iStatusTime6 ,0))/60) iStatusTime6,
       SUM(ISNULL(A.iStatusCount6 ,0)) iStatusCount6,
	    sStatusType7=(SELECT G.sStatusType FROM dbo.OpStatusType G (NOLOCK)  WHERE G.sStatusTime='iStatusTime7'),
         CONVERT(INT,SUM(ISNULL(A.iStatusTime7 ,0))/60) iStatusTime7,
       SUM(ISNULL(A.iStatusCount7 ,0))iStatusCount7 ,
	    sStatusType8=(SELECT G.sStatusType FROM dbo.OpStatusType G (NOLOCK)  WHERE G.sStatusTime='iStatusTime8'),
         CONVERT(INT,SUM(ISNULL(A.iStatusTime8 ,0))/60) iStatusTime8 ,
       SUM(ISNULL(A.iStatusCount8 ,0)) iStatusCount8,
	    sStatusType9=(SELECT G.sStatusType FROM dbo.OpStatusType G (NOLOCK)  WHERE G.sStatusTime='iStatusTime9'),
         CONVERT(INT,SUM(ISNULL(A.iStatusTime9 ,0))/60) iStatusTime9 ,
       SUM(ISNULL(A.iStatusCount9,0)) iStatusCount9 ,
	    sStatusType10=(SELECT G.sStatusType FROM dbo.OpStatusType G (NOLOCK)  WHERE G.sStatusTime='iStatusTime10'),
         CONVERT(INT,SUM(ISNULL(A.iStatusTime10 ,0))/60) iStatusTime10,
       SUM(ISNULL(A.iStatusCount10,0))  iStatusCount10,
	   A.iClassListID,B.iDeptID
	   FROM dbo.OpClassDataList (NOLOCK) A 
	   JOIN dbo.OpMachine B (NOLOCK) ON A.iMachineID=B.iMachineID
	   GROUP BY A.iMachineID,A.iClassListID,B.iDeptID
GO