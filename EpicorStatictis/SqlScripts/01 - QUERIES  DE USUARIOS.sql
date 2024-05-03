
-------------------------------------------
01 -TOTAL REGISTROS ACTIVOS ASIGNADOS A USUARIOS
-----------------------------------------  
SELECT 
Au.DisplayName AS USUARIO,
COUNT(*) AS TOTAL
FROM ApplicationUser AS Au
LEFT JOIN SupportCall AS Sc ON  Au.ApplicationUserID = Sc.AssignToUserID
WHERE YEAR(Sc.OpenDate) >=2020 AND Au.Active =1 AND Sc.Closed=0
GROUP BY Au.DisplayName ORDER BY TOTAL DESC

 -------------------------------------------
02 -TOTAL ACTIVOS POR USUARIOS  SOLO  ESTATUS
----------------------------------------- 
SELECT 
Scs.Name,
COUNT(*) AS TOTAL
FROM SupportCall AS Sc
LEFT JOIN SupportCallStatus AS Scs ON Scs.SupportCallStatusID=Sc.StatusID
WHERE YEAR(Sc.OpenDate) >= 2020 AND Sc.Closed=0 AND Sc.AssignToUserID IS NOT NULL
GROUP BY Scs.Name

-------------------------------------------
03 -TOTAL REGISTROS ACTIVOS ASIGNADOS A USUARIOS Y CUANTOS HAY EN QUEUES
-----------------------------------------  
SELECT 
CASE 
WHEN AU.DisplayName IS NULL  THEN 'EN QUEUES'
ELSE
AU.DisplayName 
END AS USUARIO,
COUNT(*) AS TOTAL
FROM SupportCall AS SC
LEFT JOIN ApplicationUser AS AU  ON AU.ApplicationUserID = SC.AssignToUserID
WHERE Closed =0 AND YEAR(OpenDate) >= 2020 
GROUP BY AU.DisplayName ORDER BY TOTAL DESC 

-------------------------------------------
04 -TOTAL REGISTROS DESDE EL 2020 HAS HOY POR USUARIO Y RANGO
----------------------------------------- 

SELECT
  ISNULL(Au.DisplayName,'ASIGNADA A QUEUE') AS [USUARIO],
  SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 0 AND 2 THEN 1 ELSE 0 END) AS [0-2 días],
  SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 3 AND 7 THEN 1 ELSE 0 END) AS [3-7 días],
  SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 8 AND 15 THEN 1 ELSE 0 END) AS [8-15 días],
  SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 16 AND 20 THEN 1 ELSE 0 END) AS [16-21 días],
  SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) >= 21 THEN 1 ELSE 0 END) AS [>21 días],
  COUNT(*) AS TOTAL
FROM SupportCall AS Sc
LEFT JOIN ApplicationUser AS Au ON Au.ApplicationUserID = Sc.AssignToUserID
WHERE YEAR(Sc.OpenDate) >= 2020 AND SC.Closed=0
GROUP BY
  Au.DisplayName
ORDER BY Au.DisplayName



-------------------------------------------
05 -TOTAL DE REPORTES QUE SE ABIERTOS POR AÑO Y SU SEMANA  DOMINGO - SABADO
----------------------------------------- 

SELECT 
DATEPART(WEEK, OpenDate) AS SEMANA, 
YEAR(OpenDate) AS AÑO, 
COUNT(*) AS [TOTAL ABIERTOS]
FROM SUpportCall
WHERE YEAR(OpenDate) >= 2020
GROUP BY DATEPART(WEEK, OpenDate), YEAR(OpenDate)
ORDER BY AÑO,SEMANA

-------------------------------------------
06 -TOTAL DE REPORTES QUE SE ABRIERTOS POR AÑO Y SU SEMANA  LUNES - VIERNES
----------------------------------------- 
SELECT 
DATEPART(WEEK, DATEADD(DAY, -1, OpenDate)) AS SEMANA, 
YEAR(OpenDate) AS Año, 
COUNT(*) AS [TOTAL ABIERTOS]
FROM SUpportCall
WHERE YEAR(OpenDate) >= 2020
GROUP BY DATEPART(WEEK, DATEADD(DAY, -1, OpenDate)), YEAR(OpenDate)
ORDER BY AÑO, SEMANA
