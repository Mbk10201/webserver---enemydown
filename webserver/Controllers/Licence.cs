using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using webserver.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Licence : ControllerBase
    {
        // GET: api/<Licence>/{ip}/{token}
        [HttpGet("{ip}&{token}")]
        public IActionResult Get(string ip, string token)
        {
            String result = "";
            IActionResult hCode;

            Database db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select enabled from `web_svlicences` WHERE ip = @serverip AND token = @servertoken", con);
                cmd.Parameters.AddWithValue("@serverip", ip);
                cmd.Parameters.AddWithValue("@servertoken", token);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    if (Convert.ToBoolean(reader["enabled"]))
                    {
                        result = String.Format("Licence for {0} - {1} is available and enabled !", ip, token);
                        hCode = Ok(result);
                    }
                    else
                    {
                        result = String.Format("Licence for {0} - {1} is available but not enabled !", ip, token);
                        hCode = NotFound(result);
                    }
                }
                else
                {
                    result = String.Format("No result found for {0} - {1} !", ip, token);
                    hCode = NotFound(result);
                }

                reader.Close();
            }

            return hCode;
        }
    }
}
