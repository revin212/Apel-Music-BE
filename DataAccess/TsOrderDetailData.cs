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
                                IsActive = bool.Parse(reader["IsActive"].ToString() ?? string.Empty)
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return tsOrderDetail;
        }

        public TsOrderDetail? GetById(Guid id)
        {
            TsOrderDetail? tsOrderDetail = null;

            string query = $"SELECT * FROM TsOrderDetail WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
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
                                IsActive = bool.Parse(reader["IsActive"].ToString() ?? string.Empty)
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

            string query = $"INSERT INTO TsOrderDetail(Id, OrderId, CourseId, IsActive) " +
                $"VALUES (DEFAULT,'{tsorderdetail.OrderId}', '{tsorderdetail.CourseId}', '{tsorderdetail.IsActive}')";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
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

            string query = $"UPDATE TsOrderDetail SET OrderId = '{tsorderdetail.OrderId}', CourseId = '{tsorderdetail.CourseId}', IsActive = '{tsorderdetail.IsActive}' " +
                $"WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
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

            string query = $"DELETE FROM TsOrderDetail WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
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
