using fs_12_team_1_BE.DTO.MsUser;
using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess
{
    public class MsUserData
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public MsUserData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }


        public List<MsUserDTO> GetAll()
        {
            List<MsUserDTO> msUser = new List<MsUserDTO>();

            string query = "SELECT * FROM MsUser WHERE IsDeleted = 0";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            msUser.Add(new MsUserDTO
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return msUser;
        }

        public MsUserDTO? GetById(Guid id)
        {
            MsUserDTO? msUser = null;

            string query = $"SELECT * FROM MsUser WHERE Id = @Id AND IsDeleted = 0";

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
                            msUser = new MsUserDTO
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return msUser;
        }

        public bool Register(MsUserRegisterDTO msUser)
        {
            bool result = false;

            string query = $"INSERT INTO MsUser(Id, Name, Email, Password, IsDeleted, IsActivated, CreatedAt) " +
                $"VALUES (DEFAULT, @Name, @Email, @Password, 0, 0, @CreatedAt)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@Name", msUser.Name);
                    command.Parameters.AddWithValue("@Email", msUser.Email);
                    command.Parameters.AddWithValue("@Password", msUser.Password);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool Update(Guid id, MsUserRegisterDTO msUser)
        {
            bool result = false;

            string query = $"UPDATE MsUser SET Name = @Name, Email = @Email, Password = @Password " +
                $"WHERE Id = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@Name", msUser.Name);
                    command.Parameters.AddWithValue("@Email", msUser.Email);
                    command.Parameters.AddWithValue("@Password", msUser.Password);
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

        public bool SoftDelete(Guid id)
        {
            bool result = false;

            string query = $"UPDATE MsUser SET IsDeleted = 1 WHERE Id = @Id";

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
