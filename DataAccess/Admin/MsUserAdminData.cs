using fs_12_team_1_BE.DTO.Admin;
using fs_12_team_1_BE.DTO.Admin.MsUserAdmin;
using fs_12_team_1_BE.DTO.MsUser;
//using fs_12_team_1_BE.DTO.MsUser;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace fs_12_team_1_BE.DataAccess
{
    [Authorize]
    public class MsUserAdminData
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public MsUserAdminData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }


        public List<MsUserAdminDTO> GetAll()
        {
            List<MsUserAdminDTO> msUser = new List<MsUserAdminDTO>();

            string query = "SELECT MsUser.Id AS UserId, MsUser.Name AS UserName, Email, Password, MsRole.Id AS RoleId, MsRole.Name AS RoleName, IsActivated, CreatedAt, RefreshToken, RefreshTokenExpires FROM MsUser INNER JOIN MsRole ON MsUser.Role = MsRole.Id";


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
                                msUser.Add(new MsUserAdminDTO
                                {
                                    Id = Guid.Parse(reader["UserId"].ToString() ?? string.Empty),
                                    Name = reader["UserName"].ToString() ?? string.Empty,
                                    Email = reader["Email"].ToString() ?? string.Empty,
                                    Password = reader["Password"].ToString() ?? string.Empty,
                                    RoleId = int.Parse(reader["RoleId"].ToString() ?? string.Empty),
                                    RoleName = reader["RoleName"].ToString() ?? string.Empty,
                                    IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty),
                                    CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? string.Empty),
                                    RefreshToken = reader["RefreshToken"].ToString() ?? string.Empty,
                                    RefreshTokenExpires = reader["RefreshTokenExpires"].ToString() ?? string.Empty,
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
            return msUser;
        }
        public MsUserAdminDTO GetById(Guid id)
        {
            MsUserAdminDTO msUser = new MsUserAdminDTO();

            string query = $"SELECT Id, Name, Email, Role, IsActivated FROM MsUser WHERE Id = @Id";


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
                                msUser = new MsUserAdminDTO
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Email = reader["Email"].ToString() ?? string.Empty,
                                    RoleId = int.Parse(reader["Role"].ToString() ?? string.Empty),
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
            return msUser;
        }
        public bool ToggleActiveStatus(Guid id, ToggleActiveStatusDTO msUser)
        {
            bool result = false;

            string query = $"UPDATE MsUser SET IsActivated = @IsActivated " +
                $"WHERE Id = @Id";


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@IsActivated", msUser.IsActivated);
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
        public List<MsRoleAdminDTO> GetRoles()
        {
            List<MsRoleAdminDTO> msRole = new List<MsRoleAdminDTO>();

            string query = $"SELECT * FROM MsRole";


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.Clear();

                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                msRole.Add(new MsRoleAdminDTO
                                {
                                    Id = int.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,

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
            return msRole;
        }

        public List<MsUserAdminGetUserClassListResDTO> GetUserClass(Guid userid)
        {
            List<MsUserAdminGetUserClassListResDTO> myclass = new List<MsUserAdminGetUserClassListResDTO>();


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Connection = connection;
                        command.Parameters.Clear();
                        command.CommandText = "SELECT CourseId, MsCourse.Name AS CourseName, MsCourse.Image AS CourseImage, Jadwal, CategoryId, MsCategory.Name AS CategoryName FROM TsOrderDetail INNER JOIN TsOrder ON TsOrderDetail.OrderId = TsOrder.Id INNER JOIN MsCourse ON TsOrderDetail.CourseId = MsCourse.Id INNER JOIN MsCategory ON MsCourse.CategoryId = MsCategory.Id WHERE TsOrder.UserId = @UserId AND IsActivated = 1";
                        command.Parameters.AddWithValue("@UserId", userid);


                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                myclass.Add(new MsUserAdminGetUserClassListResDTO
                                {
                                    CourseId = Guid.Parse(reader["CourseId"].ToString() ?? string.Empty),
                                    Name = reader["CourseName"].ToString() ?? string.Empty,
                                    Image = reader["CourseImage"].ToString() ?? string.Empty,
                                    Jadwal = DateTime.Parse(reader["Jadwal"].ToString() ?? string.Empty),
                                    CategoryId = Guid.Parse(reader["CategoryId"].ToString() ?? string.Empty),
                                    CategoryName = reader["CategoryName"].ToString() ?? string.Empty
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
            return myclass;
        }



        public Guid CheckUser(string Email)
        {
            Guid user = Guid.Empty;


            try
            {
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
                                user = Guid.Parse(reader["Id"].ToString() ?? string.Empty);
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
            return user;
        }


        public bool Create(MsUserAdminCreateDTO msUser)
        {
            bool result = false;
            string query = "INSERT INTO MsUser(Id, Name, Email, Password, Role, IsActivated, CreatedAt, RefreshToken, RefreshTokenExpires)  VALUES (@Id, @Name, @Email, @Password, @Role, @IsActivated, @CreatedAt, DEFAULT, DEFAULT)";


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Id", Guid.NewGuid());
                        command.Parameters.AddWithValue("@Name", msUser.Name);
                        command.Parameters.AddWithValue("@Email", msUser.Email);
                        command.Parameters.AddWithValue("@Password", msUser.Password);
                        command.Parameters.AddWithValue("@Role", msUser.RoleId);
                        command.Parameters.AddWithValue("@IsActivated", msUser.IsActivated);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

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

        public bool Update(Guid id, MsUserAdminCreateDTO msUser)
        {
            bool result = false;
            if (msUser.Password == "")
            {
                string query = "UPDATE MsUser SET Name = @Name, Email = @Email, Role = @RoleId,IsActivated = @IsActivated WHERE Id = @Id";


                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        using (MySqlCommand command = new MySqlCommand())
                        {
                            command.Parameters.Clear();

                            command.Parameters.AddWithValue("@Name", msUser.Name);
                            command.Parameters.AddWithValue("@Email", msUser.Email);
                            command.Parameters.AddWithValue("@RoleId", msUser.RoleId);
                            command.Parameters.AddWithValue("@IsActivated", msUser.IsActivated);
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
            } 
            else
            {
               string query = "UPDATE MsUser SET Name = @Name, Email = @Email, Role = @RoleId, Password = @Password, IsActivated = @IsActivated WHERE Id = @Id";


                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        using (MySqlCommand command = new MySqlCommand())
                        {
                            command.Parameters.Clear();

                            command.Parameters.AddWithValue("@Name", msUser.Name);
                            command.Parameters.AddWithValue("@Email", msUser.Email);
                            command.Parameters.AddWithValue("@RoleId", msUser.RoleId);
                            command.Parameters.AddWithValue("@Password", msUser.Password);
                            command.Parameters.AddWithValue("@IsActivated", msUser.IsActivated);
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
            }

            return result;
        }

      

        
    }
}
