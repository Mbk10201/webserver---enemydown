using MySql.Data.MySqlClient;
namespace webserver.Models
{
    public class Database
    {
        MySqlConnection con = new MySqlConnection("server=141.94.248.207;user=user_website;password=XLGLvPbVrQ5qpAUj;database=user_website;port=3306;");
        public MySqlConnection GetDatabase()
        {
            return con;
        }
    }
}
