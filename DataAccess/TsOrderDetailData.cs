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
                                Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
                                OrderId = int.Parse(reader["OrderId"].ToString() ?? string.Empty),
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
                                Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
                                OrderId = int.Parse(reader["OrderId"].ToString() ?? string.Empty),
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
                                Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
                                OrderId = int.Parse(reader["OrderId"].ToString() ?? string.Empty),
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
        public List<TsOrderDetail> GetMyInvoiceDetailList(Guid orderid)
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

            string query = $"INSERT INTO TsOrderDetail(Id, OrderId, CourseId, Jadwal, IsActivated) " +
                $"VALUES (DEFAULT, @OrderId, @CourseId, @Jadwal, 0)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@OrderId", tsorderdetail.OrderId);
                    command.Parameters.AddWithValue("@CourseId", tsorderdetail.CourseId);
                    command.Parameters.AddWithValue("@Jadwal", tsorderdetail.Jadwal);


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

        public bool CheckoutCart(List<TsOrderDetail> tsorderdetailchecked, List<TsOrderDetail> tsorderdetailunchecked, TsOrder tsorder)
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

                    foreach(var item in tsorderdetailchecked)
                    {
                        MySqlCommand command2 = new MySqlCommand();
                        command2.Connection = connection;
                        command2.Transaction = transaction;
                        command2.Parameters.Clear();
                        command2.CommandText = $"UPDATE TsOrderDetail SET IsActivated = 1 " +
                        $"WHERE Id = @Id";
                        command2.Parameters.AddWithValue("@Id", item.Id);
                        var result2 = command2.ExecuteNonQuery();
                    }
                    
                    

                    var result1 = command1.ExecuteNonQuery();

                    if (tsorderdetailunchecked.Count > 0)
                    {
                        

                        //Guid CartOrderId = Guid.NewGuid();

                        MySqlCommand command3 = new MySqlCommand();
                        command3.Connection = connection;
                        command3.Transaction = transaction;
                        command3.Parameters.Clear();
                        command3.CommandText = $"INSERT INTO TsOrder(Id, UserId, PaymentId, InvoiceNo, OrderDate, IsPaid) " +
                                                $"VALUES (DEFAULT, @UserId, DEFAULT, DEFAULT, DEFAULT, 0)";
                        
                        command3.Parameters.AddWithValue("@UserId", tsorder.UserId);
                        var result3 = command3.ExecuteNonQuery();

                        
                        int cartid = int.Parse(command3.LastInsertedId.ToString() ?? string.Empty);

                        foreach (var item in tsorderdetailunchecked)
                        {
                            MySqlCommand command4 = new MySqlCommand();
                            command4.Connection = connection;
                            command4.Transaction = transaction;
                            command4.Parameters.Clear();
                            command4.CommandText = $"UPDATE TsOrderDetail SET OrderId = @OrderId " +
                            $"WHERE Id = @Id";
                            command4.Parameters.AddWithValue("@Id", item.Id);
                            command4.Parameters.AddWithValue("@OrderId", cartid);
                            var result4 = command4.ExecuteNonQuery();
                        }
                        
                    }

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
        public bool DeleteOneNotActivated(int id)
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
