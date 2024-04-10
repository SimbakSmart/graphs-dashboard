

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

    }
}
