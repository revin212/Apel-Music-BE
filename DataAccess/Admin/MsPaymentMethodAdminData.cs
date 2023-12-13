using fs_12_team_1_BE.DTO.Admin;
using fs_12_team_1_BE.DTO.Admin.MsCategoryAdmin;
using fs_12_team_1_BE.DTO.Admin.MsPaymentMethod;
using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess.Admin
{
    public class MsPaymentMethodAdminData
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public MsPaymentMethodAdminData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<MsPaymentMethodAdminDTO> GetAll()
        {
            List<MsPaymentMethodAdminDTO> msPaymentMethod = new List<MsPaymentMethodAdminDTO>();

            string query = "SELECT * FROM MsPaymentMethod";


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
                                msPaymentMethod.Add(new MsPaymentMethodAdminDTO
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Image = reader["Image"].ToString() ?? string.Empty,
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
            return msPaymentMethod;
        }

        public MsPaymentMethodAdminDTO? GetById(Guid id)
        {
            MsPaymentMethodAdminDTO? msPaymentMethod = null;

            string query = "SELECT * FROM MsPaymentMethod WHERE Id = @Id";


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Id", id);

                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                msPaymentMethod = new MsPaymentMethodAdminDTO
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Image = reader["Image"].ToString() ?? string.Empty,
                                    IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
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
            return msPaymentMethod;
        }
        public bool CheckPaymentMethod(string name)
        {
            bool result = true;

            string query = $"SELECT COUNT(Id) FROM MsPaymentMethod WHERE Name = @Name";


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@Name", name);



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
        public bool CreatePaymentMethod(MsPaymentMethodAdminDTO msPaymentMethod)
        {
            bool result = false;

            string query = $"INSERT INTO MsPaymentMethod(Id, Name, Image, IsActivated) " +
                $"VALUES (@Id, @Name, @Image, @IsActivated)";


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@Id", msPaymentMethod.Id);
                        command.Parameters.AddWithValue("@Name", msPaymentMethod.Name);
                        command.Parameters.AddWithValue("@Image", msPaymentMethod.Image);
                        command.Parameters.AddWithValue("@IsActivated", msPaymentMethod.IsActivated);

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

        public bool Update(Guid id, MsPaymentMethodAdminDTO msPaymentMethod)
        {
            bool result = false;

            string query = "UPDATE MsPaymentMethod SET Name = @Name, Image = @Image, IsActivated = @IsActivated WHERE Id = @Id";


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@Name", msPaymentMethod.Name);
                        command.Parameters.AddWithValue("@Image", msPaymentMethod.Image);
                        command.Parameters.AddWithValue("@IsActivated", msPaymentMethod.IsActivated);
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

        public bool ToggleActiveStatus(Guid id, ToggleActiveStatusDTO msPaymentMethod)
        {
            bool result = false;

            string query = $"UPDATE MsPaymentMethod SET IsActivated = @IsActivated " +
                $"WHERE Id = @Id";


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@IsActivated", msPaymentMethod.IsActivated);
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
