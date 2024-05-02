

using Infraestructure.Helpers;

namespace Infraestructure.Utils
{
    public  class QueuesSqlQueries
    {
       
        private static string _sqlQuery= string.Empty;

        public static string GetTotalsQuery(FiltersParams filters= null)
        {
            
            if(filters != null)
            {
                _sqlQuery = @" 
                    SELECT 
                    (SELECT COUNT(*) 
                        FROM SupportCall  
                        WHERE YEAR(OpenDate) >= 2020 AND OpenDate >= ? AND OpenDate <= ?) AS Total,    
                    (SELECT COUNT(*) 
                        FROM SupportCall 
                        WHERE YEAR(OpenDate) >= 2020 AND Closed = 1 AND OpenDate >= ? AND OpenDate <= ?) AS TotalClosed,    
                    (SELECT COUNT(*) 
                        FROM SupportCall 
                        WHERE YEAR(OpenDate) >= 2020 AND Closed = 0 AND OpenDate >= ? AND OpenDate <= ?) AS TotalOpen;
                     ";
            }
            else
            {              
                _sqlQuery = @"
                SELECT 
                (SELECT COUNT(*) 
	            FROM SupportCall  WHERE YEAR(OpenDate) >=2020) AS Total ,
                (SELECT COUNT(*) 
                FROM SupportCall 
                WHERE YEAR(OpenDate) >= 2020 AND Closed = 1) AS TotalClosed ,
               (SELECT COUNT(*) 
                FROM SupportCall 
                WHERE YEAR(OpenDate) >= 2020 AND Closed = 0) AS TotalOpen;
               ";
            }
            return _sqlQuery;
        }

        public static string GetTotalByResponsable(FiltersParams filters= null)
        {
            if (filters != null)
            {
               _sqlQuery= @"
                        SELECT
                        CASE
                        WHEN Q.Name IS NULL THEN 'Asignadas a sistemas'
                        ELSE
                        REPLACE(Q.Name,'_',' ')
                        END AS [Queue],
                        COUNT(*) AS Total
                        FROM SupportCall AS SC
                        LEFT JOIN Queue AS Q  ON Q.QueueID = SC.AssignToQueueID
                        WHERE Closed =0 AND YEAR(OpenDate) >= 2020 AND OpenDate >= ? AND OpenDate <= ?
                        GROUP BY  Q.Name ORDER BY TOTAL DESC
                        ";
            }
            else
            {
                _sqlQuery = @"
                        SELECT
                        CASE
                        WHEN Q.Name IS NULL THEN 'Asignadas a sistemas'
                        ELSE
                        REPLACE(Q.Name,'_',' ')
                        END AS [Queue],
                        COUNT(*) AS Total
                        FROM SupportCall AS SC
                        LEFT JOIN Queue AS Q  ON Q.QueueID = SC.AssignToQueueID
                        WHERE Closed =0 AND YEAR(OpenDate) >= 2020
                        GROUP BY  Q.Name ORDER BY TOTAL DESC
                        ";
            }

            return _sqlQuery;
        }

        public static string GetTotalByRangeDays(FiltersParams filters = null)
        {
            if (filters != null)
            {

                _sqlQuery = @"
                         SELECT
                           ISNULL(Q.Name,'ASIGNADA A USUARIOS') AS [Queue],
                           SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 0 AND 2 THEN 1 ELSE 0 END) AS [RangeOne],
                           SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 3 AND 7 THEN 1 ELSE 0 END) AS [RangeTwo],
                           SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 8 AND 15 THEN 1 ELSE 0 END) AS [RangeThree],
                           SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 16 AND 20 THEN 1 ELSE 0 END) AS [RangeFour],
                           SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) >= 21 THEN 1 ELSE 0 END) AS [RangeFive],
                           COUNT(*) AS [Total]  
                         FROM SupportCall AS Sc
                         LEFT JOIN Queue AS  Q ON Q.QueueID = Sc.AssignToQueueID
                         WHERE YEAR(Sc.OpenDate) >= 2020 AND SC.Closed=0 
                         AND  Sc.OpenDate >= ? AND Sc.OpenDate <= ?
                         GROUP BY
                           Q.Name
                         ORDER BY Q.Name;
                        ";
            }
            else
            {
                _sqlQuery = @"
                    SELECT
                      ISNULL(Q.Name,'ASIGNADA A USUARIOS') AS [Queue],
                      SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 0 AND 2 THEN 1 ELSE 0 END) AS [RangeOne],
                      SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 3 AND 7 THEN 1 ELSE 0 END) AS [RangeTwo],
                      SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 8 AND 15 THEN 1 ELSE 0 END) AS [RangeThree],
                      SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) BETWEEN 16 AND 20 THEN 1 ELSE 0 END) AS [RangeFour],
                      SUM(CASE WHEN DATEDIFF(DAY, OpenDate, GETDATE()) >= 21 THEN 1 ELSE 0 END) AS [RangeFive],
                      COUNT(*) AS [Total]  
                    FROM SupportCall AS Sc
                    LEFT JOIN Queue AS  Q ON Q.QueueID = Sc.AssignToQueueID
                    WHERE YEAR(Sc.OpenDate) >= 2020 AND SC.Closed=0
                    GROUP BY
                      Q.Name
                    ORDER BY Q.Name;
                     ";

            }
            return _sqlQuery;
        }

    }
}
