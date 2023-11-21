using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess
{
    public class MsCategoryData
    {
        private readonly string connectionString = "server=localhost;port=3306;database=fs12apelmusic;user=root;password=";

        public List<MsCategory> GetAll()
        {
            List<MsCategory> msCategory = new List<MsCategory>();

            string query = "SELECT * FROM MsCategory";

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
                                Title = reader["Title"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty,
                                HeaderImage = reader["HeaderImage"].ToString() ?? string.Empty,
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return msCategory;
        }

        public MsCategory? GetById(Guid id)
        {
            MsCategory? msCategory = null;

            string query = $"SELECT * FROM MsCategory WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            msCategory = new MsCategory
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
    }
}
