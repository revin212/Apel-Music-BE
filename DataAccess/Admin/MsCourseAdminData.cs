using fs_12_team_1_BE.DTO.Admin;
using fs_12_team_1_BE.DTO.Admin.MsCategoryAdmin;
using fs_12_team_1_BE.DTO.Admin.MsCourseAdmin;
using fs_12_team_1_BE.DTO.Admin.MsUserAdmin;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess.Admin
{
    public class MsCourseAdminData
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        public MsCourseAdminData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<MsCourseAdminDTO> GetAll()
        {
            List<MsCourseAdminDTO> msCourse = new List<MsCourseAdminDTO>();

            string query = "SELECT cs.Id, cs.Name, cs.Description,cs.Image, cs.Price, ct.Id AS CategoryId, ct.Name AS CategoryName FROM MsCourse AS cs JOIN MsCategory ct ON cs.CategoryId = ct.Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            msCourse.Add(new MsCourseAdminDTO
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty,
                                Price = Convert.ToDouble(reader["Price"]),
                                CategoryId = Guid.Parse(reader["CategoryId"].ToString() ?? string.Empty),
                                CategoryName = reader["CategoryName"].ToString() ?? string.Empty,
                                IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return msCourse;
        }

        public List<MsCategoryAdminDTO> GetCategories()
        {
            List<MsCategoryAdminDTO> msCategories = new List<MsCategoryAdminDTO>();

            string query = $"SELECT Id, Name FROM MsCategory WHERE IsActivated = 1";

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
                            msCategories.Add(new MsCategoryAdminDTO
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,

                            });
                        }
                    }

                    connection.Close();
                }
            }

            return msCategories;
        }
        public MsCourseAdminDTO GetById(Guid id)
        {
            MsCourseAdminDTO msCourse = new MsCourseAdminDTO();

            string query = "SELECT cs.Id, cs.Name, cs.Description,cs.Image, cs.Price, ct.Id AS CategoryId, ct.Name AS CategoryName FROM MsCourse AS cs JOIN MsCategory ct ON cs.CategoryId = ct.Id WHERE cs.Id = @Id";

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
                            msCourse = new MsCourseAdminDTO
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty,
                                Price = Convert.ToDouble(reader["Price"]),
                                CategoryId = Guid.Parse(reader["CategoryId"].ToString() ?? string.Empty),
                                CategoryName = reader["CategoryName"].ToString() ?? string.Empty,
                                IsActivated = bool.Parse(reader["IsActivated"].ToString() ?? string.Empty)
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return msCourse;
        }
        public bool Create(MsCourseAdminDTO mscourse)
        {
            bool result = false;

            string query = $"INSERT INTO MsCourse(Id, Name, Description, Image, Price, CategoryId, IsActivated) " +
                $"VALUES (DEFAULT, @Name, @Description, @Image, @Price, @CategoryId, @IsActivated)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@Name", mscourse.Name);
                    command.Parameters.AddWithValue("@Description", mscourse.Description);
                    command.Parameters.AddWithValue("@Image", mscourse.Image);
                    command.Parameters.AddWithValue("@Price", mscourse.Price);
                    command.Parameters.AddWithValue("@CategoryId", mscourse.CategoryId);
                    command.Parameters.AddWithValue("@IsActivated", mscourse.IsActivated);

                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool Update(Guid id, MsCourseAdminDTO mscourse)
        {
            bool result = false;

            string query = $"UPDATE MsCourse SET Name = @Name, Description = @Description, Image = @Image, Price = @Price, CategoryId = @CategoryId, IsActivated = @IsActivated " +
                $"WHERE Id = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@Name", mscourse.Name);
                    command.Parameters.AddWithValue("@Description", mscourse.Description);
                    command.Parameters.AddWithValue("@Image", mscourse.Image);
                    command.Parameters.AddWithValue("@Price", mscourse.Price);
                    command.Parameters.AddWithValue("@CategoryId", mscourse.CategoryId);
                    command.Parameters.AddWithValue("@IsActivated", mscourse.IsActivated);
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
        public bool ToggleActiveStatus(Guid id, ToggleActiveStatusDTO MsCourse)
        {
            bool result = false;

            string query = $"UPDATE MsCourse SET IsActivated = @IsActivated " +
                $"WHERE Id = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@IsActivated", MsCourse.IsActivated);
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
