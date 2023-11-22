using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess
{
    public class TsOrderData
    {
        private readonly string connectionString = "server=localhost;port=3306;database=fs12apelmusic;user=root;password=";

        public List<TsOrder> GetAll()
        {
            List<TsOrder> tsOrder = new List<TsOrder>();

            string query = "SELECT * FROM TsOrder";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tsOrder.Add(new TsOrder
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                UserId = Guid.Parse(reader["UserId"].ToString() ?? string.Empty),
                                PaymentId = Guid.Parse(reader["PaymentId"].ToString() ?? string.Empty),
                                InvoiceNo = reader["InvoiceNo"].ToString() ?? string.Empty,
                                OrderDate = DateTime.Parse(reader["OrderDate"].ToString()?? string.Empty),
                                IsPaid = bool.Parse(reader["IsPaid"].ToString() ?? string.Empty)
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return tsOrder;
        }

        public TsOrder? GetById(Guid id)
        {
            TsOrder? tsOrder = null;

            string query = $"SELECT * FROM TsOrder WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tsOrder = new TsOrder
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                UserId = Guid.Parse(reader["UserId"].ToString() ?? string.Empty),
                                PaymentId = Guid.Parse(reader["PaymentId"].ToString() ?? string.Empty),
                                InvoiceNo = reader["InvoiceNo"].ToString() ?? string.Empty,
                                OrderDate = DateTime.Parse(reader["OrderDate"].ToString() ?? string.Empty),
                                IsPaid = bool.Parse(reader["IsPaid"].ToString() ?? string.Empty)
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return tsOrder;
        }

        public bool Insert(TsOrder tsorder)
        {
            bool result = false;

            string query = $"INSERT INTO TsOrder(Id, UserId, PaymentId, InvoiceNo, OrderDate, IsPaid) " +
                $"VALUES (DEFAULT,'{tsorder.UserId}', '{tsorder.PaymentId}', '{tsorder.InvoiceNo}', {tsorder.OrderDate}, '{tsorder.IsPaid}')";

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

        public bool Update(Guid id, TsOrder tsorder)
        {
            bool result = false;

            string query = $"UPDATE TsOrder SET UserId = '{tsorder.UserId}', PaymentId = '{tsorder.PaymentId}', InvoiceNo = '{tsorder.InvoiceNo}', OrderDate = '{tsorder.OrderDate}', IsPaid = '{tsorder.IsPaid}' " +
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

            string query = $"DELETE FROM TsOrder WHERE Id = '{id}'";

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
