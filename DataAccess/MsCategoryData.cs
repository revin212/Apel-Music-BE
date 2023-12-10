using fs_12_team_1_BE.DTO.MsCategory;
using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess
{
    public class MsCategoryData
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public MsCategoryData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<MsCategory> GetShortList()
        {
            List<MsCategory> msCategory = new List<MsCategory>();

            string query = "SELECT * FROM MsCategory WHERE IsActivated = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            msCategory.Add(new MsCategory
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

            return msCategory;
        }

        public MsCategoryDetailResDTO GetCategoryDetail(Guid id)
        {
            MsCategoryDetailResDTO msCategory = new MsCategoryDetailResDTO();

            string query = $"SELECT * FROM MsCategory WHERE Id = @Id";

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
                            msCategory = new MsCategoryDetailResDTO
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Title = reader["Title"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty,
                                HeaderImage = reader["HeaderImage"].ToString() ?? string.Empty,
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return msCategory;
        }

        //public MsCategory? GetByName(string Name)
        //{
        //    MsCategory? msCategory = null;

        //    string query = $"SELECT * FROM MsCategory WHERE Name = @Name";

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
        //                    msCategory = new MsCategory
        //                    {
        //                        Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
        //                        Name = reader["Name"].ToString() ?? string.Empty,
        //                        Title = reader["Title"].ToString() ?? string.Empty,
        //                        Description = reader["Description"].ToString() ?? string.Empty,
        //                        Image = reader["Image"].ToString() ?? string.Empty,
        //                        HeaderImage = reader["HeaderImage"].ToString() ?? string.Empty,
        //                    };
        //                }
        //            }

        //            connection.Close();
        //        }
        //    }

        //    return msCategory;
        //}

        //public bool Insert(MsCategory msCategory)
        //{
        //    bool result = false;

        //    string query = $"INSERT INTO MsCategory(Id, Name, Title, Description, Image, HeaderImage) " +
        //        $"VALUES (DEFAULT, @Name, @Title, @Description, @Image, @HeaderImage)";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand())
        //        {
        //            command.Parameters.Clear();

        //            command.Parameters.AddWithValue("@Name", msCategory.Name);
        //            command.Parameters.AddWithValue("@Title", msCategory.Title);
        //            command.Parameters.AddWithValue("@Description", msCategory.Description);
        //            command.Parameters.AddWithValue("@Image", msCategory.Image);
        //            command.Parameters.AddWithValue("@HeaderImage", msCategory.HeaderImage);

        //            command.Connection = connection;
        //            command.CommandText = query;

        //            connection.Open();

        //            result = command.ExecuteNonQuery() > 0 ? true : false;

        //            connection.Close();
        //        }
        //    }

        //    return result;
        //}

        //public bool Update(Guid id, MsCategory msCategory)
        //{
        //    bool result = false;

        //    string query = $"UPDATE MsCategory SET Name = @Name, Title = @Title, Description = @Description, Image = @Image, HeaderImage = @HeaderImage " +
        //        $"WHERE Id = @Id";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand())
        //        {
        //            command.Parameters.Clear();

        //            command.Parameters.AddWithValue("@Name", msCategory.Name);
        //            command.Parameters.AddWithValue("@Title", msCategory.Title);
        //            command.Parameters.AddWithValue("@Description", msCategory.Description);
        //            command.Parameters.AddWithValue("@Image", msCategory.Image);
        //            command.Parameters.AddWithValue("@HeaderImage", msCategory.HeaderImage);
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

        //    string query = $"DELETE FROM MsCategory WHERE Id = @Id";

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
