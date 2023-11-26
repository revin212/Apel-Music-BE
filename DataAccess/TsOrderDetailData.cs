using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess
{
    public class TsOrderDetailData
    {
        private readonly string connectionString = "server=localhost;port=3306;database=fs12apelmusic;user=root;password=";

        public List<TsOrderDetail> GetAll()
        {
            List<TsOrderDetail> tsOrderDetail = new List<TsOrderDetail>();

            string query = "SELECT * FROM TsOrderDetail";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tsOrderDetail.Add(new TsOrderDetail
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                OrderId = Guid.Parse(reader["OrderId"].ToString() ?? string.Empty),
                                CourseId = Guid.Parse(reader["CourseId"].ToString() ?? string.Empty),
                                IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return tsOrderDetail;
        }

        public TsOrderDetail? GetAllById(Guid id) 
        {
            TsOrderDetail? tsOrderDetail = null;

            string query = $"SELECT * FROM TsOrderDetail WHERE Id = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", id);
                    command.Connection = connection;
                    command.CommandText = query;
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tsOrderDetail = new TsOrderDetail
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                OrderId = Guid.Parse(reader["OrderId"].ToString() ?? string.Empty),
                                CourseId = Guid.Parse(reader["CourseId"].ToString() ?? string.Empty),
                                IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return tsOrderDetail;
        }
        public List<TsOrderDetail?> GetCart(Guid orderid, Guid userid)
        {
            List<TsOrderDetail?> tsOrderDetail = new List<TsOrderDetail?>();

            string query = $"SELECT * FROM TsOrderDetail LEFT JOIN TsOrder ON TsOrderDetail.OrderId = TsOrder.Id WHERE OrderId = @OrderId AND UserId = @UserId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@OrderId", orderid);
                    command.Parameters.AddWithValue("@UserId", userid);
                    command.Connection = connection;
                    command.CommandText = query;
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tsOrderDetail.Add(new TsOrderDetail
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                OrderId = Guid.Parse(reader["OrderId"].ToString() ?? string.Empty),
                                CourseId = Guid.Parse(reader["CourseId"].ToString() ?? string.Empty),
                                IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return tsOrderDetail;
        }
        public TsOrderDetail? GetCourse(Guid orderid, bool isactivated)
        {
            TsOrderDetail? tsOrderDetail = null;

            string query = $"SELECT * FROM TsOrderDetail WHERE OrderId = @id AND IsActivated = @IsActivated";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", orderid);
                    command.Parameters.AddWithValue("@IsActivated", isactivated);
                    command.Connection = connection;
                    command.CommandText = query;
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tsOrderDetail = new TsOrderDetail
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                OrderId = Guid.Parse(reader["OrderId"].ToString() ?? string.Empty),
                                CourseId = Guid.Parse(reader["CourseId"].ToString() ?? string.Empty),
                                IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return tsOrderDetail;
        }

        public bool Insert(TsOrderDetail tsorderdetail) 
        {
            bool result = false;

            string query = $"INSERT INTO TsOrderDetail(Id, OrderId, CourseId, IsActivated) " +
                $"VALUES (DEFAULT, @OrderId, @CourseId, @IsActivated)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
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

        public bool UpdateIsActivated(Guid orderid, bool isactivated)
        {
            bool result = false;

            string query = $"UPDATE TsOrderDetail SET IsActivated = @IsActivated" +
                $"WHERE OrderId = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", orderid);
                    command.Parameters.AddWithValue("@IsActivated", isactivated);
                    //command.Parameters.AddWithValue("@OrderId", tsorderdetail.OrderId);
                    //command.Parameters.AddWithValue("@CourseId", tsorderdetail.CourseId);
                    //command.Parameters.AddWithValue("@IsActivated", tsorderdetail.IsActivated);
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool Delete(Guid id)
        {
            bool result = false;

            string query = $"DELETE FROM TsOrderDetail WHERE Id = @id";

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
        public bool DeleteOneNotActivated(Guid id, Guid orderid)
        {
            bool result = false;

            string query = $"DELETE FROM TsOrderDetail WHERE Id = @Id AND OrderId = @OrderId AND IsActivated = 0";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", id);
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
    }
}
