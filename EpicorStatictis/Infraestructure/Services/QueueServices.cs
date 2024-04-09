﻿
using Core.Models;
using Infraestructure.Data;
using Infraestructure.Helpers;
using Infraestructure.Interfaces;
using Infraestructure.Utils;
using System.Data;
using System.Data.Odbc;

namespace Infraestructure.Services
{
    public class QueueServices : IServices<Queues>
    {
        private OdbcConnection con = null;

        public async Task DisposeAsync()
        {
            if (con != null)
                await con.CloseAsync();
        }

        public async Task<List<Queues>> GetTotalsAsync(FiltersParams filters = null)
        {
            List<Queues> _list = null;
            string _query = string.Empty;
            try
            {

                if (filters != null)
                {
                    _query = QueuesSqlQueries.GRAN_TOTAL_TOTAL_CLOSE_AND_TOTAL_OPEN_WITH_FILTERS;
                }
                else
                {
                    _query = QueuesSqlQueries.GRAN_TOTAL_TOTAL_CLOSE_AND_TOTAL_OPEN;
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
                            com.Parameters.Add("@StartDateClosed", OdbcType.DateTime).Value = filters.StartDate;
                            com.Parameters.Add("@EndDateClosed", OdbcType.DateTime).Value = filters.EndDate;
                            com.Parameters.Add("@StartDateOpen", OdbcType.DateTime).Value = filters.StartDate;
                            com.Parameters.Add("@EndDateOpen", OdbcType.DateTime).Value = filters.EndDate;
                        }

                        using (OdbcDataReader reader = com.ExecuteReader())
                        {

                            _list = new List<Queues>();
                            while (reader.Read())
                            {
                                _list.Add(new Queues.QueuesBuilder()
                                                .WithTotal(Convert.ToInt32(reader["Total"]))
                                                .WithTotalOpen(Convert.ToInt32(reader["TotalOpen"]))
                                                .WithTotalClosed(Convert.ToInt32(reader["TotalClosed"]))
                                               .Build()
                                               );
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
    }
}