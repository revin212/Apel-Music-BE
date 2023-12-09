﻿using fs_12_team_1_BE.DTO.MsUser;
using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

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

        public List<MsUserGetMyClassListResDTO> GetMyClass(Guid userid)
        {
            List<MsUserGetMyClassListResDTO> myclass = new List<MsUserGetMyClassListResDTO>();  
            
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
                            myclass.Add(new MsUserGetMyClassListResDTO
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

            return myclass;
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
                    command.CommandText = "SELECT MsUser.Id AS UserId, Email, Password, MsRole.Id AS RoleId, MsRole.Name AS RoleName, IsActivated, IsDeleted From MsUser JOIN MsRole ON Role = MsRole.Id WHERE Email = @Email";

                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@Email", Email);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new MsUser
                            {
                                Id = Guid.Parse(reader["UserId"].ToString() ?? string.Empty),
                                Email = reader["Email"].ToString() ?? string.Empty,
                                Password = reader["Password"].ToString() ?? string.Empty,
                                RoleId = int.Parse(reader["RoleId"].ToString() ?? string.Empty),
                                RoleName = reader["RoleName"].ToString() ?? string.Empty,
                                IsActivated = Convert.ToBoolean(reader["IsActivated"]),
                                IsDeleted = Convert.ToBoolean(reader["IsDeleted"])
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
            string query = "INSERT INTO MsUser(Id, Name, Email, Password, Role, IsDeleted, IsActivated, CreatedAt, RefreshToken, RefreshTokenExpires)  VALUES (UUID(), @Name, @Email, @Password, DEFAULT, 0, 0, @CreatedAt, DEFAULT, DEFAULT)";

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

        public bool UpdateRefreshToken(RefreshTokenDTO refreshToken)
        {
            bool result = false;

            string query = "UPDATE MsUser SET RefreshToken = @RefreshToken, RefreshTokenExpires = @RefreshTokenExpires " + "WHERE Email = @Email";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;

                    command.Parameters.AddWithValue("@RefreshToken", refreshToken.RefreshToken);
                    command.Parameters.AddWithValue("@RefreshTokenExpires", refreshToken.RefreshTokenExpires);
                    command.Parameters.AddWithValue("@Email", refreshToken.Email);

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public RefreshTokenDTO GetRefreshToken(string Email)
        {
            RefreshTokenDTO RefreshToken = new RefreshTokenDTO();

            string query = $"SELECT * FROM MsUser WHERE Email = @Email";

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
                            RefreshToken = new RefreshTokenDTO
                            {
                                Email = reader["Email"].ToString() ?? string.Empty,
                                RefreshToken = reader["RefreshToken"].ToString() ?? string.Empty,
                                RefreshTokenExpires = reader.GetDateTime("RefreshTokenExpires")
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return RefreshToken;
        }

        public bool Logout(string Email)
        {
            bool result = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.Parameters.Clear();

                command.CommandText = "UPDATE MsUser SET RefreshToken = @RefreshToken, RefreshTokenExpires = @RefreshTokenExpires WHERE Email = @Email";
                command.Parameters.AddWithValue("@RefreshToken", string.Empty);
                command.Parameters.AddWithValue("@RefreshTokenExpires", DateTime.Now.AddDays(-1));
                command.Parameters.AddWithValue("@Email", Email);

                connection.Open();
                result = command.ExecuteNonQuery() > 0 ? true : false;

                connection.Close();
            }

            return result;
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

        //public bool Update(Guid id, MsUserRegisterDTO msUser)
        //{
        //    bool result = false;

        //    string query = $"UPDATE MsUser SET Name = @Name, Email = @Email, Password = @Password " +
        //        $"WHERE Id = @Id";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand())
        //        {
        //            command.Parameters.Clear();

        //            command.Parameters.AddWithValue("@Name", msUser.Name);
        //            command.Parameters.AddWithValue("@Email", msUser.Email);
        //            command.Parameters.AddWithValue("@Password", msUser.Password);
        //            command.Parameters.AddWithValue("@Id", id);

        //            command.Connection = connection;
        //            command.CommandText = query;

        //            connection.Open();

        //            result = command.ExecuteNonQuery() > 0 ? true : false;

        //            connection.Close();
        //        }
        //    }

        //    return result;
        //}

        //public bool SoftDelete(Guid id)
        //{
        //    bool result = false;

        //    string query = $"UPDATE MsUser SET IsDeleted = 1 WHERE Id = @Id";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand())
        //        {
        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@Id", id);

        //            command.Connection = connection;
        //            command.CommandText = query;

        //            connection.Open();

        //            result = command.ExecuteNonQuery() > 0 ? true : false;

        //            connection.Close();
        //        }
        //    }

        //    return result;
        //}

        //public bool HardDelete(Guid id)
        //{
        //    bool result = false;
        //    string query = "DELETE FROM MsUser WHERE Id = @Id";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand())
        //        {
        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@Id", id);

        //            command.Connection = connection;
        //            command.CommandText = query;

        //            connection.Open();

        //            result = command.ExecuteNonQuery() > 0 ? true : false;

        //            connection.Close();
        //        }
        //    }

        //    return result;
        //}
    }
}
