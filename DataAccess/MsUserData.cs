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

        public MsUser? CheckUser(string Email)
        {
            MsUser? user = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * From MsUser WHERE Email = @Email";

                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@Email", Email);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new MsUser
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Email = reader["Email"].ToString() ?? string.Empty,
                                Password = reader["Password"].ToString() ?? string.Empty
                            };
                        }
                    }

                    connection.Close();

                }
            }

            return user;
        }


        public bool Register(MsUserRegisterDTO msUser)
        {
            bool result = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();
                Guid Id = Guid.NewGuid();

                try
                {
                    MySqlCommand command1 = new MySqlCommand();
                    command1.Connection = connection;
                    command1.Transaction = transaction;
                    command1.Parameters.Clear();

                    command1.CommandText = "INSERT INTO MsUser(Id, Name, Email, Password, IsDeleted, IsActivated, CreatedAt)  VALUES (@Id, @Name, @Email, @Password, 0, 0, @CreatedAt)";
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.Parameters.AddWithValue("@Name", msUser.Name);
                    command1.Parameters.AddWithValue("@Email", msUser.Email);
                    command1.Parameters.AddWithValue("@Password", msUser.Password);
                    command1.Parameters.AddWithValue("@CreatedAt", DateTime.Now);


                    MySqlCommand command2 = new MySqlCommand();
                    command2.Connection = connection;
                    command2.Transaction = transaction;
                    command2.Parameters.Clear();

                    command2.CommandText = "INSERT INTO MsUserRefreshToken (Id, UserEmail) VALUES (@Id, @UserEmail)";
                    command2.Parameters.AddWithValue("@Id", Id);
                    command2.Parameters.AddWithValue("@UserEmail", msUser.Email);

                    var result1 = command1.ExecuteNonQuery();
                    var result2 = command2.ExecuteNonQuery();

                    transaction.Commit();

                    result = true;
                }
                catch
                {
                    transaction.Rollback();
                    result = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }

        public bool UpdateRefreshToken(MsUserRefreshToken refreshToken)
        {
            bool result = false;

            string query = "UPDATE MsUserRefreshToken SET RefreshToken = @RefreshToken, CreatedAt = @CreatedAt, ExpiredAt = @ExpiredAt " + "WHERE UserEmail = @UserEmail";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;

                    command.Parameters.AddWithValue("@RefreshToken", refreshToken.RefreshToken);
                    command.Parameters.AddWithValue("@CreatedAt", refreshToken.CreatedAt);
                    command.Parameters.AddWithValue("@ExpiredAt", refreshToken.ExpiredAt);
                    command.Parameters.AddWithValue("@UserEmail", refreshToken.UserEmail);

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public MsUserRefreshToken GetRefreshToken(string Email)
        {
            MsUserRefreshToken msUserRefreshToken = new MsUserRefreshToken();

            string query = $"SELECT * FROM MsUserRefreshToken WHERE UserEmail = @Email";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Email", Email);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            msUserRefreshToken = new MsUserRefreshToken
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                UserEmail = reader["UserEmail"].ToString() ?? string.Empty,
                                RefreshToken = reader["RefreshToken"].ToString() ?? string.Empty,
                                CreatedAt = reader.GetDateTime("CreatedAt"),
                                ExpiredAt = reader.GetDateTime("ExpiredAt")
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return msUserRefreshToken;
        }

        public bool ActivateUser(string Email)
        {
            bool result = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.Parameters.Clear();

                command.CommandText = "UPDATE MsUser SET IsActivated = 1 WHERE Email = @Email";
                command.Parameters.AddWithValue("@Email", Email);

                connection.Open();
                result = command.ExecuteNonQuery() > 0 ? true : false;

                connection.Close();
            }

            return result;
        }

        public bool ResetPassword(string Id, string password)
        {
            bool result = false;

            string query = "UPDATE MsUser SET Password = @Password WHERE Id = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;
                    
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Password", password);

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

        public bool HardDelete(Guid id)
        {
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

                    command1.CommandText = "DELETE FROM MsUserRefreshToken WHERE Id = @Id";
                    command1.Parameters.AddWithValue("@Id", id);


                    MySqlCommand command2 = new MySqlCommand();
                    command2.Connection = connection;
                    command2.Transaction = transaction;
                    command2.Parameters.Clear();

                    command2.CommandText = "DELETE FROM MsUser WHERE Id = @Id";
                    command2.Parameters.AddWithValue("@Id", id);

                    var result1 = command1.ExecuteNonQuery();
                    var result2 = command2.ExecuteNonQuery();

                    transaction.Commit();

                    result = true;
                }
                catch
                {
                    transaction.Rollback();
                    result = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
    }
}
