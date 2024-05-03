-------------------------------------------
01 -TABLA DIMENSIONAL SUPPORT CALLS
----------------------------------------- 
SELECT 
    SC.SupportCallID,
	SC.PartyID,
    Pri.Description AS [Priority],
    SC.StatusID,
    SC.Number,
    SC.OpenDate,
	DATEADD(HH,-6,SC.OpenDate) AS OpenDate6HoursLess,--EN EPICOR SE MUESTRA 6 HORAS MENOS
    OpenUser.DisplayName AS[OpenByUser],
    SC.AssignDate,
    SC.AssignToUserID,
    SC.AssignToQueueID,
    SC.CloseDate,
    CloseUser.DisplayName AS[CloseByUser],
    SC.Closed,
    SC.Summary,
    SC.LastChangeDate,
	ChangeUser.DisplayName AS[LastChangByUser],
	Urgency.Entry   AS[UrgencyID],
	Impact.Entry   AS[ImpactID],
    SC.SupportCallType,
   OwnerUser.DisplayName AS[Owner],
    SC.OnHold
FROM [EpicorITSMApplication].[dbo].[SupportCall] AS SC WITH (NOLOCK)
LEFT JOIN [EpicorITSMApplication].[dbo].[ValueListEntry] AS Pri ON Pri.ValueListEntryID = SC.PriorityID
LEFT JOIN [EpicorITSMApplication].[dbo].[ApplicationUser] AS OpenUser    ON OpenUser.ApplicationUserId = SC.OpenByUserID
LEFT JOIN [EpicorITSMApplication].[dbo].[ApplicationUser] AS CloseUser    ON CloseUser.ApplicationUserId = SC.CloseByUserID
LEFT JOIN [EpicorITSMApplication].[dbo].[ApplicationUser] AS ChangeUser    ON ChangeUser.ApplicationUserId = SC.LastChangeByUserID
LEFT JOIN [EpicorITSMApplication].[dbo].[ValueListEntry] AS Urgency    ON Urgency.ValueListEntryID = SC.UrgencyID
LEFT JOIN [EpicorITSMApplication].[dbo].[ValueListEntry] AS Impact    ON Impact.ValueListEntryID = SC.ImpactID
LEFT JOIN [EpicorITSMApplication].[dbo].[ApplicationUser] AS OwnerUser    ON OwnerUser.ApplicationUserId = SC.OwnerID
WHERE YEAR(OpenDate) >= 2020 

-------------------------------------------
02 -TABLA DE FACTOR QUEUE
----------------------------------------- 
SELECT QueueID,
Name,
Active 
FROM [EpicorITSMApplication].[dbo].[Queue]


-------------------------------------------
03 -TABLA DE FACTOR SUPPORT CALLS STATUS
----------------------------------------
SELECT SupportCallStatusID,
Name,
Description 
FROM [EpicorITSMApplication].[dbo].[SupportCallStatus]

-------------------------------------------
04 -TABLA DE FACTOR APPLICATION USER
----------------------------------------
SELECT 
ApplicationUserID,
PartyRelationID,
DisplayName,
Active
FROM [EpicorITSMApplication].[dbo].[ApplicationUser] 


-------------------------------------------
05 -TABLA DE FACTOR SUPPORT CALLS ASSIGNT TO HISTORY
----------------------------------------
SELECT 
    SCATH.SupportCallAssignToHistoryID,
	SC.SupportCallID,
    SC.Number,
    AUAS.DisplayName AS [AssignToUser],
    Q.Name AS [AssignToQueue],
    SCATH.StartDate,
    SCATH.EndDate,
    AUA.DisplayName AS[AssignedBy],
	SCATH.AssignToUserID ,
	SCATH.AddByUserID
FROM [EpicorITSMApplication].[dbo].[SupportCallAssignToHistory] AS SCATH
LEFT JOIN [EpicorITSMApplication].[dbo].[SupportCall] AS SC ON SCATH.SupportCallID = SC.SupportCallID
LEFT JOIN [EpicorITSMApplication].[dbo].[ApplicationUser] AS AUAS ON SCATH.AssignToUserID = AUAS.ApplicationUserID
LEFT JOIN [EpicorITSMApplication].[dbo].[Queue] AS Q ON SCATH.AssignToQueueID = Q.QueueID
LEFT JOIN [EpicorITSMApplication].[dbo].[ApplicationUser] AS AUA ON SCATH.AddByUserID = AUA.ApplicationUserID
WHERE YEAR(SCATH.StartDate) >= 2020 

-------------------------------------------
06 -TABLA DE FACTOR PARTY
----------------------------------------

SELECT 
P.PartyID,
V.Entry AS TYPES,
Per.FullName AS PERSON,
G.FullName AS GROUPS,
O.Name  AS ORGANIZATION
FROM [EpicorITSMApplication].[dbo].[Party] AS P
LEFT JOIN [EpicorITSMApplication].[dbo].[ValueListEntry]  AS V ON P.PartyTypeID = V.ValueListEntryID
LEFT JOIN[EpicorITSMApplication].[dbo].[Person] AS Per  ON  Per.PersonID = P.PersonID
LEFT JOIN [EpicorITSMApplication].[dbo].[Groups] AS  G   ON G.GroupID = P.GroupID
LEFT JOIN [EpicorITSMApplication].[dbo].[Organization] AS O  ON O.OrganizationID = P.OrganizationID

-------------------------------------------
07 -TABLA DE FACTOR VALUE LIST ENTRY
----------------------------------------

SELECT 
ValueListEntryID,
ValueListID,
Sequence,
Entry,
Description,
Active
FROM [EpicorITSMApplication].[dbo].[ValueListEntry]

-------------------------------------------
08 -TABLA DE FACTOR SUPPORT CALL EVENTS
----------------------------------------
  SELECT 
Sce.SupportCallEventID,
Sce.SupportCallID,
Sce.OpenDate,
Sce.OpenByUserID,
Sce.Summary,Notes,
Sce.TypeID 
FROM 
[EpicorITSMApplication].[dbo].[SupportCallEvent] AS Sce
INNER JOIN [EpicorITSMApplication].[dbo].[SupportCall] AS Sc ON Sc.SupportCallID = Sce.SupportCallID
WHERE YEAR(Sce.OpenDate) >=2020 AND Sc.Closed=0

-------------------------------------------
xxd -TABLA DE FACTOR PARTY RELATION
----------------------------------------
--SELECT 
--Pr.PartyRelationId,
--Pr.FromPartyID,
--Pr.ToPartyID,
--Pr.SystemRelation,
--Vle.Entry,
--Vle.Description
--FROM [EpicorITSMApplication].[dbo].[PartyRelation] AS Pr
--LEFT JOIN [EpicorITSMApplication].[dbo].[ValueListEntry] AS Vle ON Pr.PartyRelationTypeID = Vle.ValueListEntryID
-------------------------------------------
xxd -TABLA DE FACTOR QUEUE APPLICATION USER
----------------------------------------

--SELECT 
--Qua.QueueApplicationUserID,
--Q.QueueID ,
--Q.Name AS [Queue],
--Au.ApplicationUserID,
--Au.DisplayName AS [Usuario]
--FROM  [EpicorITSMApplication].[dbo].[QueueApplicationUser] AS Qua
--LEFT JOIN [EpicorITSMApplication].[dbo].[Queue] AS Q ON Q.QueueID = Qua.QueueID
--LEFT JOIN [EpicorITSMApplication].[dbo].[ApplicationUser] AS Au  ON Au.ApplicationUserID = Qua.ApplicationUserID



-------------------------------------------
xxd -TABLA DE FACTOR SUPPORT CALL ASSIGN HISTORY EJEMPLOS
----------------------------------------
--VERSION 1
--SELECT 
--    SCATH.SupportCallAssignToHistoryID,
--    SC.Number,
--    AUAS.DisplayName AS [AssignToUser],
--    Q.Name AS [AssignToQueue],
--    SCATH.StartDate,
--    SCATH.EndDate,
--    AUA.DisplayName AS[Assigned By]
--FROM [EpicorITSMApplication].[dbo].[SupportCallAssignToHistory] AS SCATH
--LEFT JOIN [EpicorITSMApplication].[dbo].[SupportCall] AS SC ON SCATH.SupportCallID = SC.SupportCallID
--LEFT JOIN [EpicorITSMApplication].[dbo].[ApplicationUser] AS AUAS ON SCATH.AssignToUserID = AUAS.ApplicationUserID
--LEFT JOIN [EpicorITSMApplication].[dbo].[Queue] AS Q ON SCATH.AssignToQueueID = Q.QueueID
--LEFT JOIN [EpicorITSMApplication].[dbo].[ApplicationUser] AS AUA ON SCATH.AddByUserID = AUA.ApplicationUserID
--WHERE YEAR(SCATH.StartDate) >= 2020 

--VERSION 2
--SELECT  
--    SCATH.SupportCallAssignToHistoryID,
--    SC.Number,
--    CASE
--        WHEN Q.Name IS NOT NULL THEN Q.Name
--        ELSE AUAS.DisplayName
--    END AS AssignToQueueOrUser,
--    SCATH.StartDate,
--    SCATH.EndDate,
--    AUA.DisplayName
--FROM SupportCallAssignToHistory AS SCATH
--LEFT JOIN SupportCall AS SC ON SCATH.SupportCallID = SC.SupportCallID
--LEFT JOIN ApplicationUser AS AUAS ON SCATH.AssignToUserID = AUAS.ApplicationUserID
--LEFT JOIN Queue AS Q ON SCATH.AssignToQueueID = Q.QueueID
--LEFT JOIN ApplicationUser AS AUA ON SCATH.AddByUserID = AUA.ApplicationUserID
--WHERE YEAR(SCATH.StartDate) >=2020

--VERSION 3
--SELECT 
--    SCATH.SupportCallAssignToHistoryID,
--	SC.SupportCallID,
--    SC.Number,
--    AUAS.DisplayName AS [AssignToUser],
--    Q.Name AS [AssignToQueue],
--    SCATH.StartDate,
--    SCATH.EndDate,
--    AUA.DisplayName AS[AssignedBy],
--	SCATH.AssignToUserID ,
--	SCATH.AddByUserID
--FROM [EpicorITSMApplication].[dbo].[SupportCallAssignToHistory] AS SCATH
--LEFT JOIN [EpicorITSMApplication].[dbo].[SupportCall] AS SC ON SCATH.SupportCallID = SC.SupportCallID
--LEFT JOIN [EpicorITSMApplication].[dbo].[ApplicationUser] AS AUAS ON SCATH.AssignToUserID = AUAS.ApplicationUserID
--LEFT JOIN [EpicorITSMApplication].[dbo].[Queue] AS Q ON SCATH.AssignToQueueID = Q.QueueID
--LEFT JOIN [EpicorITSMApplication].[dbo].[ApplicationUser] AS AUA ON SCATH.AddByUserID = AUA.ApplicationUserID
--WHERE YEAR(SCATH.StartDate) >= 2020 
--ORDER BY   SC.Number ASC