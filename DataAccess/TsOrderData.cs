using fs_12_team_1_BE.DTO.TsOrder;
using fs_12_team_1_BE.DTO.TsOrderDetail;
using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;
using NuGet.Protocol.Plugins;

namespace fs_12_team_1_BE.DataAccess
{
    public class TsOrderData
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public TsOrderData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<TsOrder> GetAll()
        {
            
            List<TsOrder> tsOrder = new List<TsOrder>();

            string query = "SELECT * FROM TsOrder";


            try
            {
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
                                    Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
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
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return tsOrder;
           
        }

        public List<TsOrderGetMyInvoiceListResDTO> GetMyInvoicesList(Guid userid)
        {
            
            List<TsOrderGetMyInvoiceListResDTO> myInvoiceList = new List<TsOrderGetMyInvoiceListResDTO>();

            string query = "SELECT * FROM TsOrder WHERE UserId = @UserId AND IsPaid = 1";


            try
            {
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
                                int count = int.Parse(reader["Course_count"].ToString() ?? string.Empty);
                                myInvoiceList.Add(new TsOrderGetMyInvoiceListResDTO
                                {
                                    Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
                                    UserId = Guid.Parse(reader["UserId"].ToString() ?? string.Empty),
                                    PaymentId = Guid.Parse(reader["PaymentId"].ToString() ?? string.Empty),
                                    course_count = count,
                                    InvoiceNo = reader["InvoiceNo"].ToString() ?? string.Empty,
                                    OrderDate = DateTime.Parse(reader["OrderDate"].ToString() ?? string.Empty),
                                    TotalHarga = int.Parse(reader["TotalHarga"].ToString() ?? string.Empty),
                                    IsPaid = bool.Parse(reader["IsPaid"].ToString() ?? string.Empty)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return myInvoiceList;
            
        }

        public TsOrderGetInvoiceDetailHeaderRes GetInvoiceDetailHeader(int id)
        {
            TsOrderGetInvoiceDetailHeaderRes tsOrder = new TsOrderGetInvoiceDetailHeaderRes();

            string query = $"SELECT * FROM TsOrder WHERE Id = @Id";


            try
            {
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
                                tsOrder = new TsOrderGetInvoiceDetailHeaderRes
                                {
                                    InvoiceNo = reader["InvoiceNo"].ToString() ?? string.Empty,
                                    OrderDate = DateTime.Parse(reader["OrderDate"].ToString() ?? string.Empty),
                                    TotalHarga = double.Parse(reader["TotalHarga"].ToString() ?? string.Empty)
                                };
                            }
                        }

                        connection.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return tsOrder;
        }

        public TsOrder GetCartInfo(Guid userid)
        {
            TsOrder tsOrder = new TsOrder();

            string query = $"SELECT Id, UserId, InvoiceNo, " +
                $"(SELECT IFNULL(SUM(Price),0) FROM TsOrderDetail INNER JOIN MsCourse ON CourseId = mscourse.Id WHERE OrderId = cart.Id AND IsSelected = 1) AS TotalHarga," +
                $" OrderDate, IsPaid FROM TsOrder AS cart WHERE UserId = @UserId AND IsPaid = 0 LIMIT 1;";


            try
            {
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
                                    Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
                                    UserId = Guid.Parse(reader["UserId"].ToString() ?? string.Empty),
                                    InvoiceNo = reader["InvoiceNo"].ToString() ?? string.Empty,
                                    TotalHarga = double.Parse(reader["TotalHarga"].ToString() ?? string.Empty),
                                    OrderDate = DateTime.Parse(reader["OrderDate"].ToString() ?? string.Empty),
                                    IsPaid = bool.Parse(reader["IsPaid"].ToString() ?? string.Empty)
                                };
                            }
                        }

                        connection.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return tsOrder;
        }

        public List<TsOrderDetailGetCartListResDTO> GetCart(Guid userid)
        {
            
                
            List<TsOrderDetailGetCartListResDTO> tsOrderDetail = new List<TsOrderDetailGetCartListResDTO>();

            string query = "SELECT cartitem.Id, cartitem.OrderId, cartitem.CourseId, course.Image, cat.Name AS catname, course.Name AS coursename, cartitem.Jadwal, course.Price, cartitem.IsActivated, cartitem.IsSelected " +
                "FROM TsOrderDetail AS cartitem INNER JOIN TsOrder AS cart ON cartitem.OrderId = cart.Id " +
                "INNER JOIN mscourse AS course ON cartitem.CourseId = course.Id " +
                "INNER JOIN mscategory AS cat ON course.CategoryId = cat.ID " +
                "WHERE cart.IsPaid = 0 AND cart.UserId = @id AND cartitem.IsActivated = 0;";


            try
            {
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

                                tsOrderDetail.Add(new TsOrderDetailGetCartListResDTO
                                {
                                    Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
                                    OrderId = int.Parse(reader["OrderId"].ToString() ?? string.Empty),
                                    CourseId = Guid.Parse(reader["CourseId"].ToString() ?? string.Empty),
                                    Image = reader["Image"].ToString() ?? string.Empty,
                                    CategoryName = reader["catname"].ToString() ?? string.Empty,
                                    CourseName = reader["coursename"].ToString() ?? string.Empty,
                                    Jadwal = DateTime.Parse((reader["Jadwal"].ToString() ?? string.Empty)),
                                    Harga = double.Parse(reader["Price"].ToString() ?? string.Empty),
                                    IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty),
                                    IsSelected = bool.Parse(reader["IsSelected"].ToString() ?? string.Empty)
                                });
                            }
                        }

                        connection.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return tsOrderDetail;
            
        }
        public bool CheckoutCart(TsOrderDTOCheckout tsorder)
        {

            bool result = false;


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        string InvoiceNo = $"APM{tsorder.Id.ToString("D5")}";

                        MySqlCommand command1 = new MySqlCommand();
                        command1.Connection = connection;
                        command1.Transaction = transaction;
                        command1.Parameters.Clear();
                        command1.CommandText = $"UPDATE TsOrder SET UserId = @UserId, PaymentId = @PaymentId, InvoiceNo = @InvoiceNo, " +
                            $"Course_count = (SELECT IFNULL(COUNT(Id),0) FROM TsOrderDetail WHERE OrderId = @Id AND TsOrderDetail.IsActivated = 1), " +
                            $"TotalHarga = (SELECT IFNULL(SUM(Harga),0) FROM TsOrderDetail WHERE OrderId = @Id AND TsOrderDetail.IsActivated = 1), " +
                            $"OrderDate = @OrderDate, IsPaid = 1 " +
                            $"WHERE Id = @Id";
                        command1.Parameters.AddWithValue("@Id", tsorder.Id);
                        command1.Parameters.AddWithValue("@UserId", tsorder.UserId);
                        command1.Parameters.AddWithValue("@PaymentId", tsorder.PaymentId);
                        command1.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                        command1.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);

                        MySqlCommand command2 = new MySqlCommand();
                        command2.Connection = connection;
                        command2.Transaction = transaction;
                        command2.Parameters.Clear();
                        command2.CommandText = $"UPDATE TsOrderDetail INNER JOIN TsOrder ON OrderId = TsOrder.Id " +
                            $"SET Harga = (SELECT IFNULL(Price,0) " +
                            $"FROM (SELECT * FROM TsOrderDetail) AS cartprice " +
                            $"INNER JOIN MsCourse ON CourseId = MsCourse.Id WHERE cartprice.Id = TsOrderDetail.Id), TsOrderDetail.IsActivated = 1 " +
                        $"WHERE UserId = @UserId AND IsSelected = 1";
                        command2.Parameters.AddWithValue("@UserId", tsorder.UserId);

                        var result2 = command2.ExecuteNonQuery();

                        var result1 = command1.ExecuteNonQuery();

                        MySqlCommand command3 = new MySqlCommand();
                        command3.Connection = connection;
                        command3.Transaction = transaction;
                        command3.Parameters.Clear();
                        command3.CommandText = $"INSERT INTO TsOrder(UserId) " +
                                                $"SELECT UserId FROM TsOrderDetail JOIN TsOrder ON OrderId = TsOrder.Id " +
                                                $"WHERE TsOrder.UserId = @UserId AND IsSelected = 0 AND TsOrderDetail.IsActivated = 0 LIMIT 1";


                        command3.Parameters.AddWithValue("@UserId", tsorder.UserId);
                        var result3 = command3.ExecuteNonQuery();


                        int cartid = int.Parse(command3.LastInsertedId.ToString() ?? string.Empty);

                        MySqlCommand command4 = new MySqlCommand();
                        command4.Connection = connection;
                        command4.Transaction = transaction;
                        command4.Parameters.Clear();
                        command4.CommandText = $"UPDATE TsOrderDetail INNER JOIN TsOrder ON TsOrderDetail.OrderId = TsOrder.Id " +
                                                $"SET TsOrderDetail.OrderId = @OrderId " +
                                                $"WHERE IsSelected = 0 AND TsOrderDetail.IsActivated = 0 AND TsOrder.UserId = @UserId";
                        command4.Parameters.AddWithValue("@OrderId", cartid);
                        command4.Parameters.AddWithValue("@UserId", tsorder.UserId);
                        var result4 = command4.ExecuteNonQuery();

                        transaction.Commit();

                        result = true;
                    }
                    catch (MySqlException ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex);
                    }
                    finally
                    {
                        connection.Close();
                    }


                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }
        public int NewCart(TsOrder tsorder)
        {
            int result = 0;

            string query = $"INSERT INTO TsOrder(Id, UserId, PaymentId, InvoiceNo, TotalHarga, OrderDate, IsPaid) " +
                $"VALUES (DEFAULT, @UserId, @PaymentId, @InvoiceNo, DEFAULT, @OrderDate, 0)";


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@UserId", tsorder.UserId);
                        command.Parameters.AddWithValue("@PaymentId", tsorder.PaymentId);
                        command.Parameters.AddWithValue("@InvoiceNo", tsorder.InvoiceNo);
                        command.Parameters.AddWithValue("@OrderDate", tsorder.OrderDate);


                        command.Connection = connection;
                        command.CommandText = query;

                        connection.Open();

                        command.ExecuteNonQuery();
                        result = int.Parse(command.LastInsertedId.ToString() ?? string.Empty);

                        connection.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }
    }
}
