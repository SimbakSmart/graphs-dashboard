
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
                   _query= QueuesSqlQueries.GetTotalsQuery(filters);
                }
                else
                {
   
                  _query=  QueuesSqlQueries.GetTotalsQuery();
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

        public async Task<List<Queues>> GetTotalsByResponsableAsync(FiltersParams filters = null)
        {
            List<Queues> _list = null;
            string _query = string.Empty;
            try
            {

                if (filters != null)
                {
                    _query = QueuesSqlQueries.GetTotalByResponsable(filters);
                }
                else
                {
                    _query = QueuesSqlQueries.GetTotalByResponsable();
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

                            _list = new List<Queues>();
                            while (reader.Read())
                            {
                                _list.Add(new Queues.QueuesBuilder()
                                                .WithName(reader["Queue"].ToString())
                                                .WithTotal(Convert.ToInt32(reader["Total"]))
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

        public async Task<List<Queues>> GetTotalsByRangeDayseAsync(FiltersParams filters = null)
        {
            List<Queues> _list = null;
            string _query = string.Empty;
            try
            {

                if (filters != null)
                {
                    _query = QueuesSqlQueries.GetTotalByRangeDays(filters);
                }
                else
                {
                    _query = QueuesSqlQueries.GetTotalByRangeDays();
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

                            _list = new List<Queues>();
                            while (reader.Read())
                            {
                                _list.Add(new Queues.QueuesBuilder()
                                                .WithName(reader["Queue"].ToString())
                                                .WithRangeOne(Convert.ToInt32(reader["RangeOne"]))
                                                .WithRangeTwo(Convert.ToInt32(reader["RangeTwo"]))
                                                .WithRangeThree(Convert.ToInt32(reader["RangeThree"]))
                                                .WithRangeFour(Convert.ToInt32(reader["RangeFour"]))
                                                .WithRangeFive(Convert.ToInt32(reader["RangeFive"]))
                                                .WithTotal(Convert.ToInt32(reader["Total"]))
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

        public async Task<List<Queues>> GetTotalsByStatuseAsync(FiltersParams filters = null)
        {
            List<Queues> _list = null;
            string _query = string.Empty;
            try
            {

                if (filters != null)
                {
                    _query = QueuesSqlQueries.GetTotalByStatus(filters);
                }
                else
                {
                    _query = QueuesSqlQueries.GetTotalByStatus();
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

                            _list = new List<Queues>();
                            while (reader.Read())
                            {
                                _list.Add(new Queues.QueuesBuilder()
                                                .WithStatus(reader["Status"].ToString())
                                                .WithTotal(Convert.ToInt32(reader["Total"]))
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

        public async  Task<List<Queues>> GetTotalsByUrgencyAsync(FiltersParams filters = null)
        {
            List<Queues> _list = null;
            string _query = string.Empty;
            try
            {

                if (filters != null)
                {
                    _query = QueuesSqlQueries.GetTotalByUrgency(filters);
                }
                else
                {
                    _query = QueuesSqlQueries.GetTotalByUrgency();
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

                            _list = new List<Queues>();
                            while (reader.Read())
                            {
                                _list.Add(new Queues.QueuesBuilder()
                                                .WithUrgency(reader["Urgency"].ToString())
                                                .WithTotal(Convert.ToInt32(reader["Total"]))
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

        public async Task<List<Queues>> GetTotalsByPriorityAsync(FiltersParams filters = null)
        {
            List<Queues> _list = null;
            string _query = string.Empty;
            try
            {

                if (filters != null)
                {
                    _query = QueuesSqlQueries.GetTotalByPriority(filters);  
                }
                else
                {
                    _query = QueuesSqlQueries.GetTotalByPriority();
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

                            _list = new List<Queues>();
                            while (reader.Read())
                            {
                                _list.Add(new Queues.QueuesBuilder()
                                                .WithPriority(reader["Priority"].ToString())
                                                .WithTotal(Convert.ToInt32(reader["Total"]))
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

        public async Task<List<Queues>> GetTotalsByImpactAsync(FiltersParams filters = null)
        {
            List<Queues> _list = null;
            string _query = string.Empty;
            try
            {

                if (filters != null)
                {
                    _query = QueuesSqlQueries.GetTotalByImpact(filters);
                }
                else
                {
                    _query = QueuesSqlQueries.GetTotalByImpact();
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

                            _list = new List<Queues>();
                            while (reader.Read())
                            {
                                _list.Add(new Queues.QueuesBuilder()
                                                .WithImpact(reader["Impact"].ToString())
                                                .WithTotal(Convert.ToInt32(reader["Total"]))
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

        public async Task<List<Queues>> GetTotalsByServiceAsync(FiltersParams filters = null)
        {
            List<Queues> _list = null;
            string _query = string.Empty;
            try
            {

                if (filters != null)
                {
                    _query = QueuesSqlQueries.GetTotalByService(filters);
                }
                else
                {
                    _query = QueuesSqlQueries.GetTotalByService();
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

                            _list = new List<Queues>();
                            while (reader.Read())
                            {
                                _list.Add(new Queues.QueuesBuilder()
                                                .WithService(reader["Service"].ToString())
                                                .WithTotal(Convert.ToInt32(reader["Total"]))
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
