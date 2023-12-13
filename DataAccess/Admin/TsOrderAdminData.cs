using fs_12_team_1_BE.DTO.TsOrder;
using fs_12_team_1_BE.DTO.TsOrderDetail;
using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;
using NuGet.Protocol.Plugins;

namespace fs_12_team_1_BE.DataAccess
{
    public class TsOrderAdminData
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public TsOrderAdminData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<TsOrderAdminGetAllInvoiceListResDTO> GetAllInvoicesList()
        {
            try
            {
                List<TsOrderAdminGetAllInvoiceListResDTO> allInvoiceList = new List<TsOrderAdminGetAllInvoiceListResDTO>();

                string query = "SELECT TsOrder.Id, MsUser.Email, TsOrder.InvoiceNo, TsOrder.OrderDate, TsOrder.Course_count, TsOrder.TotalHarga, MsPaymentMethod.Name AS PaymentName FROM TsOrder JOIN MsPaymentMethod ON TsOrder.PaymentId = MsPaymentMethod.Id JOIN MsUser ON TsOrder.UserId = MsUser.Id WHERE TsOrder.IsPaid = 1";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                       
                        command.Connection = connection;
                        command.CommandText = query;
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                allInvoiceList.Add(new TsOrderAdminGetAllInvoiceListResDTO
                                {
                                    Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
                                    UserEmail = reader["Email"].ToString() ?? string.Empty,
                                    InvoiceNo = reader["InvoiceNo"].ToString() ?? string.Empty,
                                    OrderDate = DateTime.Parse(reader["OrderDate"].ToString() ?? string.Empty),
                                    course_count = int.Parse(reader["Course_count"].ToString() ?? string.Empty),
                                    TotalHarga = int.Parse(reader["TotalHarga"].ToString() ?? string.Empty),
                                    PaymentName = reader["PaymentName"].ToString() ?? string.Empty,
                                });
                            }
                        }

                        connection.Close();
                    }
                }

                return allInvoiceList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TsOrderAdminGetInvoiceDetailHeaderRes GetInvoiceDetailHeader(int id)
        {
            TsOrderAdminGetInvoiceDetailHeaderRes tsOrder = new TsOrderAdminGetInvoiceDetailHeaderRes();

            string query = $"SELECT TsOrder.Id, MsUser.Email, TsOrder.InvoiceNo, TsOrder.OrderDate, TsOrder.Course_count, TsOrder.TotalHarga, MsPaymentMethod.Name AS PaymentName FROM TsOrder JOIN MsPaymentMethod ON TsOrder.PaymentId = MsPaymentMethod.Id JOIN MsUser ON TsOrder.UserId = MsUser.Id WHERE TsOrder.Id = @Id";

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
                            tsOrder = new TsOrderAdminGetInvoiceDetailHeaderRes
                            {
                                UserEmail = reader["Email"].ToString() ?? string.Empty,
                                PaymentName = reader["PaymentName"].ToString() ?? string.Empty,
                                InvoiceNo = reader["InvoiceNo"].ToString() ?? string.Empty,
                                OrderDate = DateTime.Parse(reader["OrderDate"].ToString() ?? string.Empty),
                                TotalHarga = double.Parse(reader["TotalHarga"].ToString() ?? string.Empty)
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return tsOrder;
        }
    }
}
