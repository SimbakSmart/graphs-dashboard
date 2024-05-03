

-------------------------------------------
01 -TOTAL REGISTROS DESDE EL 2020 HAS HOY
-----------------------------------------  
SELECT COUNT(*) AS [TOTAL REGISTRO DESDE EL 2020 HAS HOY]
FROM SupportCall  
WHERE YEAR(OpenDate) >= 2020




-------------------------------------------
02 - TOTAL REGISTROS ACTIVOS DESDE EL 2020 HAS HOY
-----------------------------------------  
SELECT COUNT(*) AS [TOTAL REGISTRO ACTIVOS DESDE EL 2020 HAS HOY]
FROM SupportCall 
WHERE YEAR(OpenDate) >= 2020 AND Closed =0


-------------------------------------------
03 -TOTAL REGISTROS CERRADOS DESDE EL 2020 HASTA HOY
-----------------------------------------  
SELECT COUNT(*)  AS [TOTAL REGISTRO CERRADOS DESDE EL 2020 HAS HOY]
FROM SupportCall 
WHERE YEAR(OpenDate) >= 2020 AND Closed=1

-------------------------------------------
04 -COMPARACION DE ABIETOS VS CERRADOS DESDE EL 2020 HASTA HOY
-----------------------------------------  
SELECT TOP 1
    (SELECT COUNT(*) 
	FROM SupportCall  WHERE YEAR(OpenDate) >=2020) AS [GRAN TOTAL],
    (SELECT COUNT(*) 
     FROM SupportCall 
     WHERE YEAR(OpenDate) >= 2020 AND Closed = 1) AS[TOTAL CERRADOS],
    (SELECT COUNT(*) 
     FROM SupportCall 
     WHERE YEAR(OpenDate) >= 2020 AND Closed = 0) AS [TOTAL ABIERTOS];

	  -------------------------------------------
05 -TOTAL PENDIENTES POR QUEUE Y ASIGNADOS A USUARIOS
----------------------------------------- 
SELECT
CASE
WHEN Q.Name IS NULL THEN 'ASIGNADAS A USUARIOS'
ELSE
REPLACE(Q.Name,'_',' ')
END AS [QUEUES],
COUNT(*) AS TOTAL
FROM SupportCall AS SC
LEFT JOIN Queue AS Q  ON Q.QueueID = SC.AssignToQueueID
WHERE Closed =0 AND YEAR(OpenDate) >= 2020
GROUP BY  Q.Name ORDER BY TOTAL DESC


 -------------------------------------------
06 -TOTAL PENDIENTES POR AÑO
----------------------------------------- 
SELECT 
YEAR(Sc.OpenDate) AS [AÑO],
COUNT(*) AS [TOTAL ACTIVOS]
FROM SupportCall As Sc
WHERE YEAR(Sc.OpenDate) >=2020  AND Sc.Closed=0
GROUP BY YEAR(Sc.OpenDate) 
ORDER BY YEAR(Sc.OpenDate) DESC

 -------------------------------------------
07 -TOTAL PENDIENTES QUEUE Y STATUS
----------------------------------------- 

SELECT
CASE
WHEN Q.Name IS NULL THEN 'ASIGNADAS A USUARIOS'
ELSE
REPLACE(Q.Name,'_',' ')
END AS [QUEUES],
Scs.Name AS [ESTATUS],
COUNT(*) AS [TOTAL POR ESTATUS]
FROM SupportCall AS Sc
LEFT JOIN Queue AS Q  ON Q.QueueID = Sc.AssignToQueueID
LEFT JOIN SupportCallStatus AS Scs ON Scs.SupportCallStatusID = Sc.StatusID
WHERE Closed =0 AND YEAR(OpenDate) >= 2020
GROUP BY  Q.Name,Scs.Name
ORDER BY Q.Name,[TOTAL POR ESTATUS] DESC

 -------------------------------------------
08 -TOTAL ACTIVOS EN QUEUS POR ESTATUS
----------------------------------------- 
SELECT 
Scs.Name,
COUNT(*) AS TOTAL
FROM SupportCall AS Sc
LEFT JOIN SupportCallStatus AS Scs ON Scs.SupportCallStatusID=Sc.StatusID
WHERE YEAR(Sc.OpenDate) >= 2020 AND Sc.Closed=0 AND Sc.AssignToQueueID IS NOT NULL
GROUP BY Scs.Name



 -------------------------------------------
09 -TOTAL PENDIENTES POR URGENCIAS
----------------------------------------- 
SELECT 
CASE
WHEN U.Entry IS NULL THEN 'NO DEFINITOS'
ELSE
 SUBSTRING(U.Entry, 4, LEN(U.Entry)) 
END
AS [URGENCIA],
COUNT(*) AS [TOTAL]
FROM SupportCall AS Sc
LEFT JOIN [EpicorITSMApplication].[dbo].[ValueListEntry] AS U   ON U.ValueListEntryID = Sc.UrgencyID
WHERE YEAR(Sc.OpenDate) >=2020 AND Sc.Closed=0
GROUP BY U.Entry  ORDER BY URGENCIA DESC


 -------------------------------------------
10 -TOTAL PENDIENTES POR URGENCIAS Y QUEUE
----------------------------------------- 

SELECT 
CASE
WHEN Q.Name IS NULL THEN 'ASIGNADA A USURIOS'
ELSE
Q.Name
END  AS [QUEUE],
CASE
WHEN U.Entry IS NULL THEN 'NO DEFINITOS'
ELSE
 SUBSTRING(U.Entry, 4, LEN(U.Entry)) 
END
AS [URGENCIA],
COUNT(*) AS [TOTAL]
FROM SupportCall AS Sc
LEFT JOIN [EpicorITSMApplication].[dbo].[ValueListEntry] AS U   ON U.ValueListEntryID = Sc.UrgencyID
LEFT JOIN Queue AS Q ON Q.QueueID = Sc.AssignToQueueID
WHERE YEAR(Sc.OpenDate) >=2020 AND Sc.Closed=0
GROUP BY U.Entry,Q.Name
ORDER BY Q.Name


 -------------------------------------------
11 -TOTAL PENDIENTES POR IMPACTO
----------------------------------------- 
SELECT 
CASE
WHEN U.Entry IS NULL THEN 'NO DEFINITOS'
ELSE
 SUBSTRING(U.Entry, 4, LEN(U.Entry)) 
END
AS [IMPACTO],
COUNT(*) AS [TOTAL]
FROM SupportCall AS Sc
LEFT JOIN [EpicorITSMApplication].[dbo].[ValueListEntry] AS U   ON U.ValueListEntryID = Sc.ImpactID
WHERE YEAR(Sc.OpenDate) >=2020 AND Sc.Closed=0
GROUP BY U.Entry   ORDER BY [IMPACTO] DESC

 -------------------------------------------
12 -TOTAL PENDIENTES POR IMPACTO Y QUEUE
----------------------------------------- 
SELECT 
CASE
WHEN Q.Name IS NULL THEN 'ASIGNADA A USURIOS'
ELSE
Q.Name
END  AS [QUEUE],
CASE
WHEN U.Entry IS NULL THEN 'NO DEFINITOS'
ELSE
 SUBSTRING(U.Entry, 4, LEN(U.Entry)) 
END
AS [IMPACTO],
COUNT(*) AS [TOTAL]
FROM SupportCall AS Sc
LEFT JOIN [EpicorITSMApplication].[dbo].[ValueListEntry] AS U   ON U.ValueListEntryID = Sc.ImpactID
LEFT JOIN Queue AS Q ON Q.QueueID = Sc.AssignToQueueID
WHERE YEAR(Sc.OpenDate) >=2020 AND Sc.Closed=0
GROUP BY U.Entry,Q.Name
ORDER BY Q.Name



 -------------------------------------------
13 -TOTAL PENDIENTES POR TIPO
----------------------------------------- 

SELECT 
CASE 
WHEN Sc.SupportCallType ='I' THEN 'Incident'
WHEN Sc.SupportCallType ='R' THEN 'Request Fo Change'
WHEN Sc.SupportCallType ='S' THEN 'Service Request'
ELSE
'NO DEFINIDO'
END AS [TIPO],
COUNT(*) AS [TOTAL]
FROM SupportCall AS Sc
WHERE YEAR(Sc.OpenDate) >=2020 AND Sc.Closed=0
GROUP BY Sc.SupportCallType

 -------------------------------------------
14 -TOTAL PENDIENTES POR TIPO Y QUEUE
----------------------------------------- 

SELECT 
CASE
WHEN Q.Name IS NULL  THEN 'USUARIOS'
ELSE
Q.Name
END AS[QUEUE],
CASE 
WHEN Sc.SupportCallType ='I' THEN 'Incident'
WHEN Sc.SupportCallType ='R' THEN 'Request Fo Change'
WHEN Sc.SupportCallType ='S' THEN 'Service Request'
ELSE
'NO DEFINIDO'
END AS [TIPO],
COUNT(*) AS [TOTAL]
FROM SupportCall AS Sc
LEFT JOIN Queue AS Q ON Q.QueueID = Sc.AssignToQueueID
WHERE YEAR(Sc.OpenDate) >=2020 AND Sc.Closed=0
GROUP BY Sc.SupportCallType,Q.Name
ORDER BY Q.Name ASC

 -------------------------------------------
15 -TOTAL PENDIENTES POR PRIORIDAD
----------------------------------------- 
SELECT 
CASE
WHEN U.Entry IS NULL THEN 'NO DEFINITOS'
ELSE
 SUBSTRING(U.Entry, 4, LEN(U.Entry)) 
END
AS [PRIRIDAD],
COUNT(*) AS [TOTAL]
FROM SupportCall AS Sc
LEFT JOIN [EpicorITSMApplication].[dbo].[ValueListEntry] AS U   ON U.ValueListEntryID = Sc.PriorityID
WHERE YEAR(Sc.OpenDate) >=2020 AND Sc.Closed=0
GROUP BY U.Entry


 -------------------------------------------
16 -TOTAL PENDIENTES POR PRIORIDAD Y QUEUE
----------------------------------------- 
SELECT 
CASE
WHEN Q.Name IS NULL THEN 'ASIGNADA A USURIOS'
ELSE
Q.Name
END  AS [QUEUE],
CASE
WHEN U.Entry IS NULL THEN 'NO DEFINITOS'
ELSE
 SUBSTRING(U.Entry, 4, LEN(U.Entry)) 
END
AS [PRIRIDAD],
COUNT(*) AS [TOTAL]
FROM SupportCall AS Sc
LEFT JOIN [EpicorITSMApplication].[dbo].[ValueListEntry] AS U   ON U.ValueListEntryID = Sc.PriorityID
LEFT JOIN Queue AS Q ON Q.QueueID = Sc.AssignToQueueID
WHERE YEAR(Sc.OpenDate) >=2020 AND Sc.Closed=0
GROUP BY U.Entry,Q.Name


-------------------------------------------
17 -TOTAL PENDIENTES RANGOS DE FECHA
----------------------------------------- 

SELECT
  ISNULL(Q.Name,'ASIGNADA A USUARIOS') AS [QUEUE],
  SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 0 AND 2 THEN 1 ELSE 0 END) AS [0-2 días],
  SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 3 AND 7 THEN 1 ELSE 0 END) AS [3-7 días],
  SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 8 AND 15 THEN 1 ELSE 0 END) AS [8-15 días],
  SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 16 AND 20 THEN 1 ELSE 0 END) AS [16-20 días],
  SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) >= 21 THEN 1 ELSE 0 END) AS [>21 días],
  COUNT(*) AS [GRAN TOTAL]  
FROM SupportCall AS Sc
LEFT JOIN Queue AS  Q ON Q.QueueID = Sc.AssignToQueueID
WHERE YEAR(Sc.OpenDate) >= 2020 AND SC.Closed=0
GROUP BY
  Q.Name
ORDER BY Q.Name;




 -------------------------------------------
16 -EXTRAS QUERIES
-----------------------------------------

SELECT COUNT (*)  AS [TOTAL DE HOY]
FROM SupportCall 
WHERE OpenDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) 

SELECT COUNT (*)  AS [TOTAL DE ABIERTOS HOY]
FROM SupportCall 
WHERE OpenDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) AND Closed=0

SELECT COUNT (*)  AS [TOTAL DE CERRADOS HOY]
FROM SupportCall 
WHERE OpenDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) AND Closed=1

SELECT 
    ISNULL(Q.Name,'ASIGNADA A USUARIOS'),
    COUNT(*) AS [TOTAL],
    SUM(CASE WHEN Sc.Closed = 0 THEN 1 ELSE 0 END) AS [ABIERTOS],
    SUM(CASE WHEN Sc.Closed = 1 THEN 1 ELSE 0 END) AS [CERRADOS]
FROM SupportCall AS Sc
LEFT JOIN Queue AS Q ON Q.QueueID = Sc.AssignToQueueID 
WHERE Sc.OpenDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)
GROUP BY Q.Name;




