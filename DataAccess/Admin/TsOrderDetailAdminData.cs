using fs_12_team_1_BE.DTO.TsOrder;
using fs_12_team_1_BE.DTO.TsOrderDetail;
using fs_12_team_1_BE.Model;
using MailKit.Search;
using MySql.Data.MySqlClient;
using System.Transactions;

namespace fs_12_team_1_BE.DataAccess
{
    public class TsOrderDetailAdminData
    {
        
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public TsOrderDetailAdminData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<TsOrderDetailAdminGetAllInvoiceDetailListResDTO> GetInvoiceDetailList(int orderid)
        {
            List<TsOrderDetailAdminGetAllInvoiceDetailListResDTO> tsOrderDetail = new List<TsOrderDetailAdminGetAllInvoiceDetailListResDTO>();

            string query = $"SELECT invdetail.Id AS InvId, invdetail.OrderId AS OrderId, invdetail.CourseId AS CourseId, course.Name AS CourseName, course.CategoryId AS CourseCategoryId, cat.Name AS CourseCategoryName, Jadwal, Harga, invdetail.IsActivated FROM TsOrderDetail AS invdetail INNER JOIN MsCourse AS course ON invdetail.CourseId = course.Id INNER JOIN MsCategory AS cat ON course.CategoryId = cat.Id WHERE OrderId = @id AND invdetail.IsActivated = 1";

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
                            tsOrderDetail.Add(new TsOrderDetailAdminGetAllInvoiceDetailListResDTO
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

            return tsOrderDetail;
        }
    }
}
