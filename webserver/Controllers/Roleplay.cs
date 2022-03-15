using Microsoft.AspNetCore.Mvc;
using webserver.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using System;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Roleplay : ControllerBase
    {
        Database db = new Database();
        public class UserRow
        {
            public int playerid;
            public string? name;
            public string? steamid32;
            public string? steamid64;
            public string? model;
            public string? tag;
            public string? country;
            public string? ip;
            public int admin;
            public int tutorial;
            public int nationality;
            public int sexe;
            public int jobid;
            public int gradeid;
            public int level;
            public int xp;
            public int money;
            public int bank;
            public int playtime;
            public int viptime;

            public UserRow()
            {
                playerid = 0;
                name = "";
                steamid32 = "";
                steamid64 = "";
                model = "";
                tag = "";
                country = "";
                ip = "";
                admin = 0;
                tutorial = 0;
                nationality = 0;
                sexe = 0;
                jobid = 0;
                gradeid = 0;
                level = 0;
                xp = 0;
                money = 0;
                bank = 0;
                playtime = 0;
                viptime = 0;
            }
        }

        [HttpGet("getusers")]
        public string GetUsers()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var list = new List<UserRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `rp_api`", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UserRow row = new UserRow();
                    // loop on columns
                    row.playerid = Convert.ToInt32(reader["playerid"]);
                    row.name = Convert.ToString(reader["name"]);
                    row.steamid32 = Convert.ToString(reader["steamid_32"]);
                    row.steamid64 = Convert.ToString(reader["steamid_64"]);
                    row.model = "N/A";
                    row.tag = Convert.ToString(reader["tag"]);
                    row.country = Convert.ToString(reader["country"]);
                    row.ip = Convert.ToString(reader["ip"]);
                    row.admin = Convert.ToInt32(reader["admin"]);
                    row.tutorial = Convert.ToInt32(reader["tutorial"]);
                    row.nationality = Convert.ToInt32(reader["nationality"]);
                    row.sexe = Convert.ToInt32(reader["sexe"]);
                    row.jobid = Convert.ToInt32(reader["jobid"]);
                    row.gradeid = Convert.ToInt32(reader["gradeid"]);
                    row.level = Convert.ToInt32(reader["level"]);
                    row.xp = Convert.ToInt32(reader["xp"]);
                    row.money = Convert.ToInt32(reader["money"]);
                    row.bank = Convert.ToInt32(reader["bank"]);
                    row.playtime = Convert.ToInt32(reader["playtime"]);
                    row.viptime = Convert.ToInt32(reader["viptime"]);

                    list.Add(row);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        [HttpGet("getuser/{steamid}")]
        public string GetUser(string steamid)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var list = new List<UserRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `rp_api` WHERE steamid_64 = @steamid_64", con);
                cmd.Parameters.AddWithValue("@steamid_64", steamid);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    UserRow row = new UserRow();
                    // loop on columns
                    row.playerid = Convert.ToInt32(reader["playerid"]);
                    row.name = Convert.ToString(reader["name"]);
                    row.steamid32 = Convert.ToString(reader["steamid_32"]);
                    row.steamid64 = Convert.ToString(reader["steamid_64"]);
                    row.model = "N/A";
                    row.tag = Convert.ToString(reader["tag"]);
                    row.country = Convert.ToString(reader["country"]);
                    row.ip = Convert.ToString(reader["ip"]);
                    row.admin = Convert.ToInt32(reader["admin"]);
                    row.tutorial = Convert.ToInt32(reader["tutorial"]);
                    row.nationality = Convert.ToInt32(reader["nationality"]);
                    row.sexe = Convert.ToInt32(reader["sexe"]);
                    row.jobid = Convert.ToInt32(reader["jobid"]);
                    row.gradeid = Convert.ToInt32(reader["gradeid"]);
                    row.level = Convert.ToInt32(reader["level"]);
                    row.xp = Convert.ToInt32(reader["xp"]);
                    row.money = Convert.ToInt32(reader["money"]);
                    row.bank = Convert.ToInt32(reader["bank"]);
                    row.playtime = Convert.ToInt32(reader["playtime"]);
                    row.viptime = Convert.ToInt32(reader["viptime"]);

                    list.Add(row);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        [HttpGet("jobs")]
        public string GetJobs()
        {
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader("rp_json/jobs.json"))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                JsonError error = new JsonError();
                error.message = e.Message;

                return JsonConvert.SerializeObject(error, Formatting.Indented);
            }
        }

        [HttpPost("jobs/capital&{jobid}/{ip}")]
        public void UpdateCapital(int jobid, string ip, [FromBody] int capital)
        {
            using (var sr = new StreamReader("rp_json/jobs.json"))
            {
                JObject rss = JObject.Parse(sr.ReadToEnd());

                JObject row = (JObject)rss[Convert.ToString(jobid)];
                row.Property("capital").Replace(capital);
            }
        }

        [HttpGet("items")]
        public string GetItems()
        {
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader("rp_json/items.json"))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                JsonError error = new JsonError();
                error.message = e.Message;

                return JsonConvert.SerializeObject(error, Formatting.Indented);
            }
        }
    }
}
