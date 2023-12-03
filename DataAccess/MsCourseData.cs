using fs_12_team_1_BE.DTO.MsCourse;
using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess
{
    public class MsCourseData
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public MsCourseData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //public List<MsCourseResponseDTO> GetAll()
        //{
        //    List<MsCourseResponseDTO> msCourse = new List<MsCourseResponseDTO>();

        //    string query = "SELECT cs.Id, cs.Name, cs.Description,cs.Image, cs.Price, ct.Name AS CategoryName FROM MsCourse AS cs JOIN (SELECT Id FROM MsCourse ORDER BY RAND() LIMIT 6) as t2 ON cs.Id=t2.Id JOIN MsCategory ct ON cs.CategoryId = ct.Id";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand(query, connection))
        //        {
        //            connection.Open();

        //            using (MySqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    msCourse.Add(new MsCourseResponseDTO
        //                    {
        //                        Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
        //                        Name = reader["Name"].ToString() ?? string.Empty,
        //                        //Description = reader["Description"].ToString() ?? string.Empty,
        //                        Image = reader["Image"].ToString() ?? string.Empty,
        //                        Price = Convert.ToDouble(reader["Price"]),
        //                        CategoryName = reader["CategoryName"].ToString() ?? string.Empty
        //                    });
        //                }
        //            }

        //            connection.Close();
        //        }
        //    }

        //    return msCourse;
        //}

        public List<MsCourseGetFavoriteListResDTO> GetFavoriteList()
        {
            List<MsCourseGetFavoriteListResDTO> msCourse = new List<MsCourseGetFavoriteListResDTO>();

            string query = "SELECT cs.Id, cs.Name, cs.Description,cs.Image, cs.Price, ct.Name AS CategoryName FROM MsCourse AS cs JOIN (SELECT Id FROM MsCourse ORDER BY RAND() LIMIT 3) as t2 ON cs.Id=t2.Id JOIN MsCategory ct ON cs.CategoryId = ct.Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            msCourse.Add(new MsCourseGetFavoriteListResDTO
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty,
                                Price = Convert.ToDouble(reader["Price"]),
                                CategoryName = reader["CategoryName"].ToString() ?? string.Empty
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return msCourse;
        }

        public MsCourseGetDetailResDTO? GetDetail(Guid id)
        {
            MsCourseGetDetailResDTO? msCourse = null;

            string query = $"SELECT cs.Id, cs.Name, cs.Description,cs.Image, cs.Price, ct.Id As CategoryId, ct.Name AS CategoryName FROM MsCourse cs JOIN MsCategory ct ON cs.CategoryId = ct.Id WHERE cs.Id = @Id";

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
                            msCourse = new MsCourseGetDetailResDTO
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty,
                                Price = Convert.ToDouble(reader["Price"]),
                                CategoryId = Guid.Parse(reader["CategoryId"].ToString() ?? string.Empty),
                                CategoryName = reader["CategoryName"].ToString() ?? string.Empty
                                
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return msCourse;
        }

        //public MsCourseResponseDTO? GetByName(string name)
        //{
        //    MsCourseResponseDTO? msCourse = null;
        //    string filteredName = name.Replace("-", " ");

        //    string query = $"SELECT cs.Id, cs.Name, cs.Description,cs.Image, cs.Price, ct.Name AS CategoryName FROM MsCourse cs JOIN MsCategory ct ON cs.CategoryId = ct.Id WHERE cs.Name = @Name";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand(query, connection))
        //        {
        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@Name", filteredName);

        //            connection.Open();

        //            using (MySqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    msCourse = new MsCourseResponseDTO
        //                    {
        //                        Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
        //                        Name = reader["Name"].ToString() ?? string.Empty,
        //                        //Description = reader["Description"].ToString() ?? string.Empty,
        //                        Image = reader["Image"].ToString() ?? string.Empty,
        //                        Price = Convert.ToDouble(reader["Price"]),
        //                        CategoryName = reader["CategoryName"].ToString() ?? string.Empty
        //                    };
        //                }
        //            }

        //            connection.Close();
        //        }
        //    }

        //    return msCourse;
        //}

        public List<MsCourseGetByCategoryListResDTO> GetByCategoryList(Guid Id)
        {
            List<MsCourseGetByCategoryListResDTO> msCourse = new List<MsCourseGetByCategoryListResDTO>();

            string query = $"SELECT cs.Id, cs.Name, cs.Description,cs.Image, cs.Price, ct.Name AS CategoryName FROM MsCourse cs JOIN MsCategory ct ON cs.CategoryId = ct.Id WHERE ct.Id = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", Id);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            msCourse.Add(new MsCourseGetByCategoryListResDTO
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty,
                                Price = Convert.ToDouble(reader["Price"]),
                                CategoryName = reader["CategoryName"].ToString() ?? string.Empty,
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return msCourse;
        }

        public List<MsCourseGetOtherListRes> GetOtherList(Guid categoryid, Guid courseid)
        {
            List<MsCourseGetOtherListRes> msCourse = new List<MsCourseGetOtherListRes>();

            string query = $"SELECT cs.Id, cs.Name, cs.Description,cs.Image, cs.Price, ct.Id AS CategoryId, ct.Name AS CategoryName FROM MsCourse cs " +
                $"JOIN MsCategory ct ON cs.CategoryId = ct.Id WHERE ct.Id = @CategoryId AND NOT cs.Id = @CourseId ";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@CourseId", courseid);
                    command.Parameters.AddWithValue("@CategoryId", categoryid);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            msCourse.Add(new MsCourseGetOtherListRes
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty,
                                Price = Convert.ToDouble(reader["Price"]),
                                CategoryId = Guid.Parse(reader["CategoryId"].ToString() ?? string.Empty),
                                CategoryName = reader["CategoryName"].ToString() ?? string.Empty,
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return msCourse;
        }

        //public bool Insert(MsCourse mscourse)
        //{
        //    bool result = false;

        //    string query = $"INSERT INTO MsCourse(Id, Name, Description, Image, Price, CategoryId) " +
        //        $"VALUES (DEFAULT, @Name, @Description, @Image, @Price, @CategoryId)";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand())
        //        {
        //            command.Parameters.Clear();

        //            command.Parameters.AddWithValue("@Name", mscourse.Name);
        //            command.Parameters.AddWithValue("@Description", mscourse.Description);
        //            command.Parameters.AddWithValue("@Image", mscourse.Image);
        //            command.Parameters.AddWithValue("@Price", mscourse.Price);
        //            command.Parameters.AddWithValue("@CategoryId", mscourse.CategoryId);

        //            command.Connection = connection;
        //            command.CommandText = query;

        //            connection.Open();

        //            result = command.ExecuteNonQuery() > 0 ? true : false;

        //            connection.Close();
        //        }
        //    }

        //    return result;
        //}

        //public bool Update(Guid id, MsCourse mscourse)
        //{
        //    bool result = false;

        //    string query = $"UPDATE MsCourse SET Name = @Name, Description = @Description, Image = @Image, Price = @Price, CategoryId = @CategoryId " +
        //        $"WHERE Id = @Id";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand())
        //        {
        //            command.Parameters.Clear();

        //            command.Parameters.AddWithValue("@Name", mscourse.Name);
        //            command.Parameters.AddWithValue("@Description", mscourse.Description);
        //            command.Parameters.AddWithValue("@Image", mscourse.Image);
        //            command.Parameters.AddWithValue("@Price", mscourse.Price);
        //            command.Parameters.AddWithValue("@CategoryId", mscourse.CategoryId);
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

        //public bool Delete(Guid id)
        //{
        //    bool result = false;

        //    string query = $"DELETE FROM MsCourse WHERE Id = @Id";

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
