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

        public List<MsCategoryShortListResDTO> GetShortList()
        {
            List<MsCategoryShortListResDTO> msCategory = new List<MsCategoryShortListResDTO>();

            string query = "SELECT * FROM MsCategory WHERE IsActivated = 1";


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
                                msCategory.Add(new MsCategoryShortListResDTO
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

            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            return msCategory;
        }

        public MsCategoryDetailResDTO GetCategoryDetail(Guid id)
        {
            MsCategoryDetailResDTO msCategory = new MsCategoryDetailResDTO();

            string query = $"SELECT * FROM MsCategory WHERE Id = @Id";


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

            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            return msCategory;
        }


    }
}
