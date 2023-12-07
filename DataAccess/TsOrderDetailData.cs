using fs_12_team_1_BE.DTO.TsOrder;
using fs_12_team_1_BE.DTO.TsOrderDetail;
using fs_12_team_1_BE.Model;
using MailKit.Search;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess
{
    public class TsOrderDetailData
    {
        
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public TsOrderDetailData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //public List<TsOrderDetail> GetAll()
        //{
        //    List<TsOrderDetail> tsOrderDetail = new List<TsOrderDetail>();

        //    string query = "SELECT * FROM TsOrderDetail";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand(query, connection))
        //        {

        //            connection.Open();

        //            using (MySqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    tsOrderDetail.Add(new TsOrderDetail
        //                    {
        //                        Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
        //                        OrderId = int.Parse(reader["OrderId"].ToString() ?? string.Empty),
        //                        CourseId = Guid.Parse(reader["CourseId"].ToString() ?? string.Empty),
        //                        IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
        //                    });
        //                }
        //            }

        //            connection.Close();
        //        }
        //    }

        //    return tsOrderDetail;
        //}

        //public TsOrderDetail? GetById(int id) 
        //{
        //    TsOrderDetail? tsOrderDetail = null;

        //    string query = $"SELECT * FROM TsOrderDetail WHERE Id = @id";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand())
        //        {
        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@Id", id);
        //            command.Connection = connection;
        //            command.CommandText = query;
        //            connection.Open();

        //            using (MySqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    tsOrderDetail = new TsOrderDetail
        //                    {
        //                        Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
        //                        OrderId = int.Parse(reader["OrderId"].ToString() ?? string.Empty),
        //                        CourseId = Guid.Parse(reader["CourseId"].ToString() ?? string.Empty),

        //                        IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
        //                    };
        //                }
        //            }

        //            connection.Close();
        //        }
        //    }

        //    return tsOrderDetail;
        //}
        //public List<TsOrderDetail?> GetCart(Guid orderid, Guid userid)
        //{
        //    List<TsOrderDetail?> tsOrderDetail = new List<TsOrderDetail?>();

        //    string query = $"SELECT * FROM TsOrderDetail LEFT JOIN TsOrder ON TsOrderDetail.OrderId = TsOrder.Id WHERE OrderId = @OrderId AND UserId = @UserId";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand())
        //        {
        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@OrderId", orderid);
        //            command.Parameters.AddWithValue("@UserId", userid);
        //            command.Connection = connection;
        //            command.CommandText = query;
        //            connection.Open();

        //            using (MySqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    tsOrderDetail.Add(new TsOrderDetail
        //                    {
        //                        Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
        //                        OrderId = int.Parse(reader["OrderId"].ToString() ?? string.Empty),
        //                        CourseId = Guid.Parse(reader["CourseId"].ToString() ?? string.Empty),
        //                        IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
        //                    });
        //                }
        //            }

        //            connection.Close();
        //        }
        //    }

        //    return tsOrderDetail;
        //}
        public List<TsOrderDetail> GetMyInvoiceDetailList(int orderid)
        {
            List<TsOrderDetail> tsOrderDetail = new List<TsOrderDetail>();

            string query = $"SELECT * FROM TsOrderDetail WHERE OrderId = @id AND IsActivated = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", orderid);
                    command.Connection = connection;
                    command.CommandText = query;
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tsOrderDetail.Add(new TsOrderDetail
                            {
                                Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
                                OrderId = int.Parse(reader["OrderId"].ToString() ?? string.Empty),
                                CourseId = Guid.Parse(reader["CourseId"].ToString() ?? string.Empty),
                                Jadwal = DateOnly.Parse((reader["Jadwal"].ToString() ?? string.Empty).Substring(1, 8)),
                                Harga = double.Parse(reader["Harga"].ToString() ?? string.Empty),
                                IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return tsOrderDetail;
        }

        public bool AddToCart(TsOrderDetail tsorderdetail) 
        {
            bool result = false;

            string query = $"INSERT INTO TsOrderDetail(Id, OrderId, CourseId, Jadwal, Harga, IsActivated) " +
                $"VALUES (DEFAULT, @OrderId, @CourseId, @Jadwal, DEFAULT, 0)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@OrderId", tsorderdetail.OrderId);
                    command.Parameters.AddWithValue("@CourseId", tsorderdetail.CourseId);
                    command.Parameters.AddWithValue("@Jadwal", tsorderdetail.Jadwal.ToString("yyyy-MM-dd"));


                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool Update(Guid id, TsOrderDetail tsorderdetail)
        {
            bool result = false;

            string query = $"UPDATE TsOrderDetail SET OrderId = @OrderId, CourseId = @CourseId, IsActive = @IsActivated" +
                $"WHERE Id = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@OrderId", tsorderdetail.OrderId);
                    command.Parameters.AddWithValue("@CourseId", tsorderdetail.CourseId);
                    command.Parameters.AddWithValue("@IsActivated", tsorderdetail.IsActivated);
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        

        //public bool Delete(Guid id)
        //{
        //    bool result = false;

        //    string query = $"DELETE FROM TsOrderDetail WHERE Id = @id";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand())
        //        {
        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@Id", id);
        //            command.Connection = connection;
        //            command.CommandText = query;

        //            connection.Open();

        //            result = command.ExecuteNonQuery() > 0 ? true : false;

        //            connection.Close();
        //        }
        //    }

        //    return result;
        //}

        public bool ClearCart(Guid orderid)
        {
            bool result = false;

            string query = $"DELETE FROM TsOrderDetail WHERE OrderId = @OrderId AND IsActivated=0";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@OrderId", orderid);
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }
        public bool UpdateSelectedCartItem (int cartitemid, bool isselected)
        {
            bool result = false;

            

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", cartitemid);
                    command.Parameters.AddWithValue("@IsSelected", isselected);
                    command.Connection = connection;
                    command.CommandText = $"UPDATE TsOrderDetail SET IsSelected = @IsSelected WHERE Id = @Id AND IsActivated = 0";

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool DeleteFromCart(int id)
        {
            bool result = false;

            string query = $"DELETE FROM TsOrderDetail WHERE Id = @Id AND IsActivated = 0";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", id);
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }
    }
}
