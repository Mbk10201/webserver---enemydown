using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using webserver.Models;

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class totalvisits : ControllerBase
    {
        Database db = new Database();
        // Get product description
        [HttpGet]
        public int Get()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();
            int total = 0;
            
            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT totalvisits FROM `web_stats`", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    total = Convert.ToInt32(reader[0]);
                }

                reader.Close();
            }
            con.Close();

            return total;
        }

        [HttpGet("increase")]
        public void Set()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE `web_stats` SET totalvisits = totalvisits + 1", con);
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }
    }
}
