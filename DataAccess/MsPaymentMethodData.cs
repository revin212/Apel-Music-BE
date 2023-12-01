using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess
{
    public class MsPaymentMethodData
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public MsPaymentMethodData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<MsPaymentMethod> GetAll()
        {
            List<MsPaymentMethod> msPaymentMethod = new List<MsPaymentMethod>();

            string query = "SELECT * FROM MsPaymentMethod";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            msPaymentMethod.Add(new MsPaymentMethod
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return msPaymentMethod;
        }

        //public MsPaymentMethod? GetById(Guid id)
        //{
        //    MsPaymentMethod? msPaymentMethod = null;

        //    string query = "SELECT * FROM MsPaymentMethod WHERE Id = @Id";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand(query, connection))
        //        {
        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@Id", id);

        //            connection.Open();

        //            using (MySqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    msPaymentMethod = new MsPaymentMethod
        //                    {
        //                        Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
        //                        Name = reader["Name"].ToString() ?? string.Empty,
        //                        Image = reader["Image"].ToString() ?? string.Empty
        //                    };
        //                }
        //            }

        //            connection.Close();
        //        }
        //    }

        //    return msPaymentMethod;
        //}

        //public MsPaymentMethod? GetByName(string Name)
        //{
        //    MsPaymentMethod? msPaymentMethod = null;

        //    string query = "SELECT * FROM MsPaymentMethod WHERE Name = @Name";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand(query, connection))
        //        {
        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@Name", Name);

        //            connection.Open();

        //            using (MySqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    msPaymentMethod = new MsPaymentMethod
        //                    {
        //                        Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
        //                        Name = reader["Name"].ToString() ?? string.Empty,
        //                        Image = reader["Image"].ToString() ?? string.Empty
        //                    };
        //                }
        //            }

        //            connection.Close();
        //        }
        //    }

        //    return msPaymentMethod;
        //}
    }
}
