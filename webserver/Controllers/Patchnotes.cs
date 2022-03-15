using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using webserver.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Patchnotes : ControllerBase
    {
        Database db = new Database();
        public class PatchRow
        {
            public int id;
            public string? title;
            public string? content;
            public string? owner;
            public string? status;
            public string? date;

            public PatchRow()
            {
                id = 0;
                title = "";
                content = "";
                owner = "";
                status = "";
                date = "";
            }
        }
        
        public class PatchPost
        {
            public int id { get; set; }
            public string? title { get; set; }
            public string? content { get; set; }
            public string? owner { get; set; }
            public string? status { get; set; }
            public string? date { get; set; }
        }

        // GET <Patchnotes>
        [HttpGet()]
        public string Get()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var patchList = new List<PatchRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `web_patchnotes`", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PatchRow patch = new PatchRow();
                    // loop on columns
                    patch.id = Convert.ToInt32(reader["id"]);
                    patch.title = Convert.ToString(reader["title"]);
                    patch.content = Convert.ToString(reader["content"]);
                    patch.owner = Convert.ToString(reader["owner"]);
                    patch.status = Convert.ToString(reader["status"]);
                    patch.date = Convert.ToString(reader["date"]);

                    patchList.Add(patch);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(patchList, Formatting.Indented);
        }

        // POST <Patchnotes>
        [HttpPost]
        public void Post([FromBody] PatchPost data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO `web_patchnotes` (title, content, owner, status) VALUES(@title, @content, @owner, @status)", con);
                cmd.Parameters.AddWithValue("@title", data.title);
                cmd.Parameters.AddWithValue("@content", data.content);
                cmd.Parameters.AddWithValue("@owner", data.owner);
                cmd.Parameters.AddWithValue("@status", data.status);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        // PUT <Patchnotes>/{id}
        [HttpPut]
        public void Put([FromBody] PatchPost data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE `web_patchnotes` SET `title` = @title, `content` = @content, `owner` = @owner, `status` = @status WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", data.id);
                cmd.Parameters.AddWithValue("@title", data.title);
                cmd.Parameters.AddWithValue("@content", data.content);
                cmd.Parameters.AddWithValue("@owner", data.owner);
                cmd.Parameters.AddWithValue("@status", data.status);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        // DELETE <Patchnotes>/{id}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_patchnotes` WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
    }
}
