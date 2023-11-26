using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;
using NuGet.Protocol.Plugins;

namespace fs_12_team_1_BE.DataAccess
{
    public class TsOrderData
    {
        private readonly string connectionString = "server=localhost;port=3306;database=fs12apelmusic;user=root;password=";

        public List<TsOrder> GetAll()
        {
            try
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
                                Guid paymentid;
                                if (Convert.IsDBNull(reader["PaymentId"]))
                                {
                                    paymentid = Guid.Empty;

                                }
                                else 
                                {
                                    paymentid = Guid.Parse(reader["PaymentId"].ToString() ?? string.Empty);
                                }
                                
                                tsOrder.Add(new TsOrder
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    UserId = Guid.Parse(reader["UserId"].ToString() ?? string.Empty),
                                    PaymentId = paymentid,
                                    InvoiceNo = reader["InvoiceNo"].ToString() ?? string.Empty,
                                    OrderDate = DateTime.Parse(reader["OrderDate"].ToString() ?? string.Empty),
                                    IsPaid = bool.Parse(reader["IsPaid"].ToString() ?? string.Empty)
                                });
                            }
                        }

                        connection.Close();
                    }
                }

                return tsOrder;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TsOrder> GetAllPaidByUserId(Guid userid)
        {
            try
            {
                List<TsOrder> tsOrder = new List<TsOrder>();

                string query = "SELECT * FROM TsOrder WHERE UserId = @UserId AND IsPaid = 1";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@UserId", userid);
                        command.Connection = connection;
                        command.CommandText = query;
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
                                    OrderDate = DateTime.Parse(reader["OrderDate"].ToString() ?? string.Empty),
                                    IsPaid = bool.Parse(reader["IsPaid"].ToString() ?? string.Empty)
                                });
                            }
                        }

                        connection.Close();
                    }
                }

                return tsOrder;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TsOrder? GetById(Guid id)
        {
            TsOrder? tsOrder = null;

            string query = $"SELECT * FROM TsOrder WHERE Id = @Id";

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

        public TsOrder? GetCartInfo(Guid userid)
        {
            TsOrder? tsOrder = null;

            string query = $"SELECT * FROM TsOrder WHERE UserId = @UserId AND IsPaid = 0 LIMIT 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@UserId", userid);
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0))
                            {
                                break;
                            }
                            tsOrder = new TsOrder
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                UserId = Guid.Parse(reader["UserId"].ToString() ?? string.Empty),
                                
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

        public List<TsOrderDetail?> GetCart(Guid userid) //NOT COMPLETE
        {
            try
            {
                
                List<TsOrderDetail?> tsOrderDetail = new List<TsOrderDetail?>();
                string query = "SELECT * FROM TsOrderDetail LEFT JOIN TsOrder ON TsOrderDetail.OrderId = TsOrder.Id" +
                    " WHERE IsPaid = 0 AND UserId = @id AND IsActivated = 0";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", userid);
                        command.Connection = connection;
                        command.CommandText = query;
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                tsOrderDetail.Add( new TsOrderDetail
                                {
                                    Id = Guid.Parse(reader["TsOrderDetail.Id"].ToString() ?? string.Empty),
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
            catch (Exception)
            {

                throw;
            }
        }

        public bool Insert(TsOrder tsorder)
        {
            bool result = false;

            string query = $"INSERT INTO TsOrder(Id, UserId, PaymentId, InvoiceNo, OrderDate, IsPaid) " +
                $"VALUES (@Id, @UserId, @PaymentId, @InvoiceNo, @OrderDate, @IsPaid)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", tsorder.Id);
                    command.Parameters.AddWithValue("@UserId", tsorder.UserId);
                    command.Parameters.AddWithValue("@PaymentId", tsorder.PaymentId);
                    command.Parameters.AddWithValue("@InvoiceNo", tsorder.InvoiceNo);
                    command.Parameters.AddWithValue("@OrderDate", tsorder.OrderDate);
                    command.Parameters.AddWithValue("@IsPaid", tsorder.IsPaid);

                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }
        
        public bool Update(TsOrder tsorder)
        {
            bool result = false;

            string query = $"UPDATE TsOrder SET UserId = @UserId, PaymentId = @PaymentId, InvoiceNo = @InvoiceNo, OrderDate = @OrderDate, IsPaid = @IsPaid " +
                $"WHERE Id = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", tsorder.Id);
                    command.Parameters.AddWithValue("@UserId", tsorder.UserId);
                    command.Parameters.AddWithValue("@PaymentId", tsorder.PaymentId);
                    command.Parameters.AddWithValue("@InvoiceNo", tsorder.InvoiceNo);
                    command.Parameters.AddWithValue("@OrderDate", tsorder.OrderDate);
                    command.Parameters.AddWithValue("@IsPaid", tsorder.IsPaid);
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }
        public bool Checkout(TsOrder tsorder)
        {
            //use transaction
            
            bool result = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    MySqlCommand command1 = new MySqlCommand();
                    command1.Connection = connection;
                    command1.Transaction = transaction;
                    command1.Parameters.Clear();
                    command1.CommandText = $"UPDATE TsOrder SET UserId = @UserId, PaymentId = @PaymentId, InvoiceNo = @InvoiceNo, OrderDate = @OrderDate, IsPaid = @IsPaid " +
                    $"WHERE Id = @Id";
                    command1.Parameters.AddWithValue("@Id", tsorder.Id);
                    command1.Parameters.AddWithValue("@UserId", tsorder.UserId);
                    command1.Parameters.AddWithValue("@PaymentId", tsorder.PaymentId);
                    command1.Parameters.AddWithValue("@InvoiceNo", tsorder.InvoiceNo);
                    command1.Parameters.AddWithValue("@OrderDate", tsorder.OrderDate);
                    command1.Parameters.AddWithValue("@IsPaid", tsorder.IsPaid);


                    MySqlCommand command2 = new MySqlCommand();
                    command2.Connection = connection;
                    command2.Transaction = transaction;
                    command2.Parameters.Clear();
                    command2.CommandText = $"UPDATE TsOrderDetail SET IsActivated = 1 " +
                    $"WHERE OrderId = @Id";
                    command2.Parameters.AddWithValue("@Id", tsorder.Id);
                    var result1 = command1.ExecuteNonQuery();
                    var result2 = command2.ExecuteNonQuery();

                    transaction.Commit();

                    result = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }


            }

            return result;
        }

        public bool Delete(Guid id)
        {
            bool result = false;

            string query = $"DELETE FROM TsOrder WHERE Id = @id";

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
