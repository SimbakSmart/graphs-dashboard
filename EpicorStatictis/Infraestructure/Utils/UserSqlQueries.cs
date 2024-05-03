

using Infraestructure.Helpers;

namespace Infraestructure.Utils
{
    public static  class UserSqlQueries
    {
        private static string _sqlQuery = string.Empty;


        public static string GetTotalByResponsable(FiltersParams filters = null)
        {
            if (filters != null)
            {
                //_sqlQuery = @"
                //        SELECT
                //        CASE
                //        WHEN Q.Name IS NULL THEN 'Asignadas a sistemas'
                //        ELSE
                //        REPLACE(Q.Name,'_',' ')
                //        END AS [Queue],
                //        COUNT(*) AS Total
                //        FROM SupportCall AS SC
                //        LEFT JOIN Queue AS Q  ON Q.QueueID = SC.AssignToQueueID
                //        WHERE Closed =0 AND YEAR(OpenDate) >= 2020 AND OpenDate >= ? AND OpenDate <= ?
                //        GROUP BY  Q.Name ORDER BY TOTAL DESC
                //      ";

                    _sqlQuery = @"SELECT 
                        Au.DisplayName AS Name,
                        COUNT(*) AS Total
                        FROM ApplicationUser AS Au
                        LEFT JOIN SupportCall AS Sc ON  Au.ApplicationUserID = Sc.AssignToUserID
                        WHERE YEAR(Sc.OpenDate) >=2020 AND Au.Active =1 AND Sc.Closed=0
                        AND OpenDate >= ? AND OpenDate <= ?
                        GROUP BY Au.DisplayName ORDER BY TOTAL DESC
                        ";
            }
            else
            {
                _sqlQuery = @"SELECT 
                        Au.DisplayName AS Name,
                        COUNT(*) AS Total
                        FROM ApplicationUser AS Au
                        LEFT JOIN SupportCall AS Sc ON  Au.ApplicationUserID = Sc.AssignToUserID
                        WHERE YEAR(Sc.OpenDate) >=2020 AND Au.Active =1 AND Sc.Closed=0
                        GROUP BY Au.DisplayName ORDER BY TOTAL DESC
                        ";
            }

            return _sqlQuery;
        }
    }
}
