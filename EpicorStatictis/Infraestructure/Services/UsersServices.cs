

using Core.Models;
using Infraestructure.Data;
using Infraestructure.Helpers;
using Infraestructure.Interfaces;
using Infraestructure.Utils;
using System.Data;
using System.Data.Odbc;

namespace Infraestructure.Services
{
    public class UsersServices : IServices<Users>
    {

        private OdbcConnection con = null;

        public async Task DisposeAsync()
        {
            if (con != null)
                await con.CloseAsync();
        }


        public Task<List<Users>> GetTotalsAsync(FiltersParams filters = null)
        {
            throw new NotImplementedException();
        }

        public  async Task<List<Users>> GetTotalsByResponsableAsync(FiltersParams filters = null)
        {
            List<Users> _list = null;
            string _query = string.Empty;
            try
            {

                if (filters != null)
                {
                   _query = UserSqlQueries.GetTotalByResponsable(filters);
                }
                else
                {
                    _query = UserSqlQueries.GetTotalByResponsable();
                }


                using (OdbcConnection con = new OdbcConnection(DBContext.GetConnectionString))
                {
                    await con.OpenAsync();
                    using (OdbcCommand com = new OdbcCommand(_query, con))
                    {
                        if (filters != null)
                        {
                            com.CommandType = CommandType.Text;
                            com.Parameters.Add("@StartDate", OdbcType.DateTime).Value = filters.StartDate;
                            com.Parameters.Add("@EndDate", OdbcType.DateTime).Value = filters.EndDate;
                        }

                        using (OdbcDataReader reader = com.ExecuteReader())
                        {

                            _list = new List<Users>();
                            while (reader.Read())
                            {
                                _list.Add(new Users.UsersBuilder()
                                    .WithName(reader["Name"].ToString())
                                    .WithTotal(Convert.ToInt32(reader["Total"]))
                                    .Build());
                            }
                            reader.Close();
                        }

                    }
                }
            }

            catch
            {
                return null;
            }
            return _list;
        }

        public Task<List<Users>> GetTotalsByImpactAsync(FiltersParams filters = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Users>> GetTotalsByPriorityAsync(FiltersParams filters = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Users>> GetTotalsByRangeDayseAsync(FiltersParams filters = null)
        {
            throw new NotImplementedException();
        }

       

        public Task<List<Users>> GetTotalsByServiceAsync(FiltersParams filters = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Users>> GetTotalsByStatuseAsync(FiltersParams filters = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Users>> GetTotalsByUrgencyAsync(FiltersParams filters = null)
        {
            throw new NotImplementedException();
        }
    }
}
