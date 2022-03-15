using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using webserver.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class News : ControllerBase
    {
        Database db = new Database();
        public class NewRow
        {
            public int id;
            public string? title;
            public string? content;
            public string? date;

            public NewRow()
            {
                id = 0;
                title = "";
                content = "";
                date = "";
            }
        }

        public class NewPost
        {
            public int id { get; set; }
            public string? title { get; set; }
            public string? content { get; set; }
            public string? date { get; set; }
        }

        // GET <News>
        [HttpGet()]
        public string Get()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var newlist = new List<NewRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `web_news`", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    NewRow newrow = new NewRow();
                    // loop on columns
                    newrow.id = Convert.ToInt32(reader["id"]);
                    newrow.title = Convert.ToString(reader["title"]);
                    newrow.content = Convert.ToString(reader["content"]);
                    newrow.date = Convert.ToString(reader["date"]);

                    newlist.Add(newrow);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(newlist, Formatting.Indented);
        }

        // POST <News>
        [HttpPost]
        public void Post([FromBody] NewPost data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO `web_news` (title, content) VALUES(@title, @content)", con);
                cmd.Parameters.AddWithValue("@title", data.title);
                cmd.Parameters.AddWithValue("@content", data.content);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        // PUT <News>
        [HttpPut]
        public void Put([FromBody] NewPost data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();
            Console.WriteLine(data.id);
            Console.WriteLine(data.title);
            Console.WriteLine(data.content);

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE `web_news` SET `title` = @title, `content` = @content WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", data.id);
                cmd.Parameters.AddWithValue("@title", data.title);
                cmd.Parameters.AddWithValue("@content", data.content);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        // DELETE <News>/{id}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_news` WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
    }
}
