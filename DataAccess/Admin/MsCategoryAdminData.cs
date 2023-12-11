using fs_12_team_1_BE.DTO.Admin;
using fs_12_team_1_BE.DTO.Admin.MsCategoryAdmin;
using fs_12_team_1_BE.DTO.MsCategory;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess.Admin
{
    public class MsCategoryAdminData
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public MsCategoryAdminData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public List<MsCategoryAdminDTO> GetCategoryAll()
        {
            List<MsCategoryAdminDTO> msCategory = new List<MsCategoryAdminDTO>();

            string query = $"SELECT * FROM MsCategory";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Clear();
                    

                    connection.Open();

                    try
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                msCategory.Add(new MsCategoryAdminDTO
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Title = reader["Title"].ToString() ?? string.Empty,
                                    Description = reader["Description"].ToString() ?? string.Empty,
                                    Image = reader["Image"].ToString() ?? string.Empty,
                                    HeaderImage = reader["HeaderImage"].ToString() ?? string.Empty,
                                    IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)

                                }
                                );
                            }
                        }
                    }
                    catch (MySqlException e)
                    {

                        Console.WriteLine(e);
                    }

                    connection.Close();
                }
            }

            return msCategory;
        }
        public MsCategoryAdminDTO GetCategoryById(Guid id)
        {
            MsCategoryAdminDTO msCategory = new MsCategoryAdminDTO();

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
                            msCategory = new MsCategoryAdminDTO
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Title = reader["Title"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty,
                                HeaderImage = reader["HeaderImage"].ToString() ?? string.Empty,
                                IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return msCategory;
        }
        public bool CreateCategory(MsCategoryAdminCreateDTO msCategory)
        {
            bool result = false;

            string query = $"INSERT INTO MsCategory(Id, Name, Title, Description, Image, HeaderImage, IsActivated) " +
                $"VALUES (@Id, @Name, @Title, @Description, @Image, @HeaderImage, @IsActivated)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", msCategory.Id);
                    command.Parameters.AddWithValue("@Name", msCategory.Name);
                    command.Parameters.AddWithValue("@Title", msCategory.Title);
                    command.Parameters.AddWithValue("@Description", msCategory.Description);
                    command.Parameters.AddWithValue("@Image", msCategory.Image);
                    command.Parameters.AddWithValue("@HeaderImage", msCategory.HeaderImage);
                    command.Parameters.AddWithValue("@IsActivated", msCategory.IsActivated);

                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    
                    result = command.ExecuteNonQuery() > 0 ? true : false;
                    connection.Close();
                }
            }

            return result;
        }

        public bool Update(Guid id, MsCategoryAdminDTO msCategory)
        {
            bool result = false;

            string query = $"UPDATE MsCategory SET Name = @Name, Title = @Title, Description = @Description, Image = @Image, HeaderImage = @HeaderImage, IsActivated = @IsActivated " +
                $"WHERE Id = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@Name", msCategory.Name);
                    command.Parameters.AddWithValue("@Title", msCategory.Title);
                    command.Parameters.AddWithValue("@Description", msCategory.Description);
                    command.Parameters.AddWithValue("@Image", msCategory.Image);
                    command.Parameters.AddWithValue("@HeaderImage", msCategory.HeaderImage);
                    command.Parameters.AddWithValue("@IsActivated", msCategory.IsActivated);
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

        public bool ToggleActiveStatus(Guid id, ToggleActiveStatusDTO msCategory)
        {
            bool result = false;

            string query = $"UPDATE MsCategory SET IsActivated = @IsActivated " +
                $"WHERE Id = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@IsActivated", msCategory.IsActivated);
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
