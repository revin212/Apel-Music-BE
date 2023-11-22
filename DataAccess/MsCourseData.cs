using fs_12_team_1_BE.Model;
using MySql.Data.MySqlClient;

namespace fs_12_team_1_BE.DataAccess
{
    public class MsCourseData
    {
        private readonly string connectionString = "server=localhost;port=3306;database=fs12apelmusic;user=root;password=";

        public List<MsCourse> GetAll()
        {
            List<MsCourse> msCourse = new List<MsCourse>();

            string query = "SELECT * FROM MsCourse";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            msCourse.Add(new MsCourse
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty,
                                Price = Convert.ToDouble(reader["Price"]),
                                CategoryId = Guid.Parse(reader["CategoryId"].ToString() ?? string.Empty)
                            });
                        }
                    }

                    connection.Close();
                }
            }

            return msCourse;
        }

        public MsCourse? GetById(Guid id)
        {
            MsCourse? msCourse = null;

            string query = $"SELECT * FROM MsCourse WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            msCourse = new MsCourse
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty,
                                Image = reader["Image"].ToString() ?? string.Empty,
                                Price = Convert.ToDouble(reader["Price"]),
                                CategoryId = Guid.Parse(reader["CategoryId"].ToString() ?? string.Empty)
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return msCourse;
        }

        public bool Insert(MsCourse mscourse)
        {
            bool result = false;

            string query = $"INSERT INTO MsCourse(Id, Name, Description, Image, Price, CategoryId) " +
                $"VALUES (DEFAULT,'{mscourse.Name}', '{mscourse.Description}', '{mscourse.Image}', {mscourse.Price}, '{mscourse.CategoryId}')";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool Update(Guid id, MsCourse mscourse)
        {
            bool result = false;

            string query = $"UPDATE MsCourse SET Name = '{mscourse.Name}', Description = '{mscourse.Description}', Image = '{mscourse.Image}', Price = '{mscourse.Price}', CategoryId = '{mscourse.CategoryId}' " +
                $"WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool Delete(Guid id)
        {
            bool result = false;

            string query = $"DELETE FROM MsCourse WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
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
