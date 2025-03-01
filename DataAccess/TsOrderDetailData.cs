﻿using fs_12_team_1_BE.DTO.TsOrder;
using fs_12_team_1_BE.DTO.TsOrderDetail;
using fs_12_team_1_BE.Model;
using MailKit.Search;
using MySql.Data.MySqlClient;
using System.Transactions;

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

        public List<TsOrderDetailGetMyInvoiceDetailListResDTO> GetMyInvoiceDetailList(int orderid)
        {
            List<TsOrderDetailGetMyInvoiceDetailListResDTO> tsOrderDetail = new List<TsOrderDetailGetMyInvoiceDetailListResDTO>();

            string query = $"SELECT invdetail.Id AS InvId, invdetail.OrderId AS OrderId, invdetail.CourseId AS CourseId, course.Name AS CourseName, course.CategoryId AS CourseCategoryId, cat.Name AS CourseCategoryName, Jadwal, Harga, invdetail.IsActivated FROM TsOrderDetail AS invdetail INNER JOIN MsCourse AS course ON invdetail.CourseId = course.Id INNER JOIN MsCategory AS cat ON course.CategoryId = cat.Id WHERE OrderId = @id AND invdetail.IsActivated = 1";


            try
            {
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
                                tsOrderDetail.Add(new TsOrderDetailGetMyInvoiceDetailListResDTO
                                {
                                    Id = int.Parse(reader["InvId"].ToString() ?? string.Empty),
                                    OrderId = int.Parse(reader["OrderId"].ToString() ?? string.Empty),
                                    CourseId = Guid.Parse(reader["CourseId"].ToString() ?? string.Empty),
                                    CourseName = reader["CourseName"].ToString() ?? string.Empty,
                                    CourseCategoryId = Guid.Parse(reader["CourseCategoryId"].ToString() ?? string.Empty),
                                    CourseCategoryName = reader["CourseCategoryName"].ToString() ?? string.Empty,
                                    Jadwal = DateTime.Parse((reader["Jadwal"].ToString() ?? string.Empty)),
                                    Harga = double.Parse(reader["Harga"].ToString() ?? string.Empty),
                                    IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
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

        public bool AddToCart(TsOrderDetail tsorderdetail) 
        {
            bool result = false;

            string query = $"INSERT INTO TsOrderDetail(Id, OrderId, CourseId, Jadwal, Harga, IsActivated) " +
                $"VALUES (DEFAULT, @OrderId, @CourseId, @Jadwal, DEFAULT, 0)";


            try
            {
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
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }

        public bool CheckJadwal(Guid userid, TsOrderDetail tsorderdetail)
        {
            bool result = true;
            
            string query = $"SELECT COUNT(TsOrderDetail.Id) FROM TsOrderDetail INNER JOIN TsOrder ON TsOrderDetail.OrderId = TsOrder.Id WHERE CourseId = @CourseId AND Jadwal = @Jadwal AND UserId = @UserId";


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@UserId", userid);
                        command.Parameters.AddWithValue("@CourseId", tsorderdetail.CourseId);
                        command.Parameters.AddWithValue("@Jadwal", tsorderdetail.Jadwal.ToString("yyyy-MM-dd"));


                        command.Connection = connection;
                        command.CommandText = query;

                        connection.Open();

                        result = (long)command.ExecuteScalar() > 0 ? false : true;

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

        public bool ClearCart(Guid orderid)
        {
            bool result = false;

            string query = $"DELETE FROM TsOrderDetail WHERE OrderId = @OrderId AND IsActivated=0";


            try
            {
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
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }
        public TsOrder UpdateSelectedCartItem (int cartitemid, bool isselected, Guid userid)
        {
            

            TsOrder tsOrder = new TsOrder();


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlTransaction transaction = connection.BeginTransaction();
                    try
                    {


                        MySqlCommand command = new MySqlCommand();
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Id", cartitemid);
                        command.Parameters.AddWithValue("@IsSelected", isselected);
                        command.Connection = connection;
                        command.CommandText = $"UPDATE TsOrderDetail SET IsSelected = @IsSelected WHERE Id = @Id AND IsActivated = 0";
                        var result1 = command.ExecuteNonQuery();

                        MySqlCommand command2 = new MySqlCommand();
                        command2.Parameters.Clear();
                        command2.Parameters.AddWithValue("@UserId", userid);
                        command2.Connection = connection;
                        command2.CommandText = $"SELECT Id, UserId, InvoiceNo, " +
                                                $"(SELECT IFNULL(SUM(Price),0) FROM TsOrderDetail od INNER JOIN MsCourse ON CourseId = mscourse.Id WHERE OrderId = cart.Id AND od.IsSelected = 1 AND od.IsActivated = 0) AS TotalHarga," +
                                                $" OrderDate, IsPaid FROM TsOrder AS cart WHERE UserId = @UserId AND IsPaid = 0 LIMIT 1;";
                        using (MySqlDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.IsDBNull(0))
                                {
                                    break;
                                }
                                //var test = double.Parse(reader["TotalHarga"].ToString() ?? string.Empty);
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
                        transaction.Commit();

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
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return tsOrder;
        }

        public bool DeleteFromCart(int id)
        {
            bool result = false;

            string query = $"DELETE FROM TsOrderDetail WHERE Id = @Id AND IsActivated = 0";


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

                        result = command.ExecuteNonQuery() > 0 ? true : false;

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
