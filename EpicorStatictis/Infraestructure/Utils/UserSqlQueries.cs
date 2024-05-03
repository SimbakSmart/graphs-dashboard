

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

        public static string GetTotalByStatus(FiltersParams filters = null)
        {
            if (filters != null)
            {
                _sqlQuery = @"
                            SELECT 
                        Scs.Name As Status,
                        COUNT(*) AS Total
                        FROM SupportCall AS Sc
                        LEFT JOIN SupportCallStatus AS Scs ON Scs.SupportCallStatusID=Sc.StatusID
                        WHERE YEAR(Sc.OpenDate) >= 2020 AND Sc.Closed=0 AND Sc.AssignToUserID IS NOT NULL
                        AND OpenDate >= ? AND OpenDate <= ?
                        GROUP BY Scs.Name
                            ";
            }
            else
            {
                _sqlQuery = @"
                           SELECT 
                            Scs.Name As Status,
                            COUNT(*) AS Total
                            FROM SupportCall AS Sc
                            LEFT JOIN SupportCallStatus AS Scs ON Scs.SupportCallStatusID=Sc.StatusID
                            WHERE YEAR(Sc.OpenDate) >= 2020 AND Sc.Closed=0 AND Sc.AssignToUserID IS NOT NULL
                            GROUP BY Scs.Name
                            ";
            }
            return _sqlQuery;
        }
    }
}
