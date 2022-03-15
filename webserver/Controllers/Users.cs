using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using webserver.Models;
using Newtonsoft.Json;
using SteamWebAPI2.Utilities;
using SteamWebAPI2.Interfaces;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        Database db = new Database();
        public class UsersRow
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int? id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int? role;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? username;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? email;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? steamid;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? joindate;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int? banned;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Array? steamdata;
        }
        public class SteamRow
        {
            public string? ProfileUrl;
            public string? Avatar;
            public string? AvatarMedium;
            public string? AvatarFull;
            public string? UserStatus;
            public string? RealName;
            public string? PrimaryGroupId;
            public string? AccountCreatedDate;
            public string? CountryCode;
            public string? PlayingGameName;
            public string? PlayingGameId;
            public string? PlayingGameServerIP;

            public SteamRow()
            {
                ProfileUrl = "";
                Avatar = "";
                AvatarMedium = "";
                AvatarFull = "";
                UserStatus = "";
                RealName = "";
                PrimaryGroupId = "";
                AccountCreatedDate = "";
                CountryCode = "";
                PlayingGameName = "";
                PlayingGameId = "";
                PlayingGameServerIP = "";
            }
        }
        public class BanPost
        {
            public int user { get; set; }
            public int admin { get; set; }
            public int time { get; set; }
            public string? raison { get; set; }
        }

        // GET <Users>
        [HttpGet()]
        public async Task<string> Get()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var UserList = new List<UsersRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select id, username, email, steamid, role, joindate from `web_users`", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // User json row
                    UsersRow user = new UsersRow();
                    user.id = Convert.ToInt32(reader["id"]);
                    user.username = Convert.ToString(reader["username"]);
                    user.steamid = Convert.ToString(reader["steamid"]);
                    user.role = Convert.ToInt32(reader["role"]);
                    user.email = Convert.ToString(reader["email"]);
                    user.joindate = Convert.ToString(reader["joindate"]);
                    user.banned = Banstatus(Convert.ToInt32(reader["id"]));

                    String? sSteamID = Convert.ToString(reader["steamid"]);

                    // SteamAPI Fetch

                    if (Regex.IsMatch(sSteamID ?? string.Empty, @"^\d+$"))
                    {
                        var webInterfaceFactory = new SteamWebInterfaceFactory("D29B43A3EDF1F47C5D5FFE76C5B9A77C");
                        var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamUser>(new HttpClient());


                        var playerSummaryResponse = await steamInterface.GetPlayerSummaryAsync(Convert.ToUInt64(sSteamID));

                        // User steamdata array constructor
                        List<SteamRow> list = new List<SteamRow>();
                        SteamRow steamrow = new SteamRow();
                        steamrow.ProfileUrl = playerSummaryResponse.Data.ProfileUrl;
                        steamrow.Avatar = playerSummaryResponse.Data.AvatarUrl;
                        steamrow.AvatarMedium = playerSummaryResponse.Data.AvatarMediumUrl;
                        steamrow.AvatarFull = playerSummaryResponse.Data.AvatarFullUrl;
                        steamrow.UserStatus = Convert.ToString(playerSummaryResponse.Data.UserStatus);
                        steamrow.RealName = playerSummaryResponse.Data.RealName;
                        steamrow.PrimaryGroupId = playerSummaryResponse.Data.PrimaryGroupId;
                        steamrow.AccountCreatedDate = Convert.ToString(playerSummaryResponse.Data.AccountCreatedDate);
                        steamrow.CountryCode = playerSummaryResponse.Data.CountryCode;
                        steamrow.PlayingGameName = playerSummaryResponse.Data.PlayingGameName;
                        steamrow.PlayingGameId = playerSummaryResponse.Data.PlayingGameId;
                        steamrow.PlayingGameServerIP = playerSummaryResponse.Data.PlayingGameServerIP;

                        list.Add(steamrow);

                        //Set user steamdata array with the precedent builded array
                        user.steamdata = list.ToArray();
                    }

                    // Finally send the user complet array to global list
                    UserList.Add(user);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(UserList, Formatting.Indented);
        }

        // GET <Users>
        [HttpGet("getbysteamid/{steamid}")]
        public async Task<IActionResult> GetBySteamId(string steamid)
        {
            if (Regex.IsMatch(steamid, @"^\d+$"))
            {
                db = new Database();
                MySqlConnection con = db.GetDatabase();

                var UserList = new List<UsersRow>();

                using (con)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("select id, username, email, steamid, role, joindate from `web_users` WHERE steamid = @steamid", con);
                    cmd.Parameters.AddWithValue("@steamid", steamid);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // User json row
                        UsersRow user = new UsersRow();
                        user.id = Convert.ToInt32(reader["id"]);
                        user.username = Convert.ToString(reader["username"]);
                        user.steamid = Convert.ToString(reader["steamid"]);
                        user.role = Convert.ToInt32(reader["role"]);
                        user.email = Convert.ToString(reader["email"]);
                        user.joindate = Convert.ToString(reader["joindate"]);
                        user.banned = Banstatus(Convert.ToInt32(reader["id"]));

                        string? sSteamID = Convert.ToString(reader["steamid"]);

                        // SteamAPI Fetch

                        if (Regex.IsMatch(sSteamID ?? string.Empty, @"^\d+$"))
                        {
                            var webInterfaceFactory = new SteamWebInterfaceFactory("D29B43A3EDF1F47C5D5FFE76C5B9A77C");
                            var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamUser>(new HttpClient());

                            var playerSummaryResponse = await steamInterface.GetPlayerSummaryAsync(Convert.ToUInt64(sSteamID));

                            // User steamdata array constructor
                            List<SteamRow> list = new List<SteamRow>();
                            SteamRow steamrow = new SteamRow();
                            steamrow.ProfileUrl = playerSummaryResponse.Data.ProfileUrl;
                            steamrow.Avatar = playerSummaryResponse.Data.AvatarUrl;
                            steamrow.AvatarMedium = playerSummaryResponse.Data.AvatarMediumUrl;
                            steamrow.AvatarFull = playerSummaryResponse.Data.AvatarFullUrl;
                            steamrow.UserStatus = Convert.ToString(playerSummaryResponse.Data.UserStatus);
                            steamrow.RealName = playerSummaryResponse.Data.RealName;
                            steamrow.PrimaryGroupId = playerSummaryResponse.Data.PrimaryGroupId;
                            steamrow.AccountCreatedDate = Convert.ToString(playerSummaryResponse.Data.AccountCreatedDate);
                            steamrow.CountryCode = playerSummaryResponse.Data.CountryCode;
                            steamrow.PlayingGameName = playerSummaryResponse.Data.PlayingGameName;
                            steamrow.PlayingGameId = playerSummaryResponse.Data.PlayingGameId;
                            steamrow.PlayingGameServerIP = playerSummaryResponse.Data.PlayingGameServerIP;

                            list.Add(steamrow);

                            //Set user steamdata array with the precedent builded array
                            user.steamdata = list.ToArray();
                        }

                        // Finally send the user complet array to global list
                        UserList.Add(user);
                    }

                    reader.Close();
                }

                return Ok(JsonConvert.SerializeObject(UserList, Formatting.Indented));
            }

            return NotFound(steamid);
        }

        // GET <Users>
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var UserList = new List<UsersRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select id, username, email, steamid, role, joindate from `web_users` WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // User json row
                    UsersRow user = new UsersRow();
                    user.id = Convert.ToInt32(reader["id"]);
                    user.username = Convert.ToString(reader["username"]);
                    user.steamid = Convert.ToString(reader["steamid"]);
                    user.role = Convert.ToInt32(reader["role"]);
                    user.email = Convert.ToString(reader["email"]);
                    user.joindate = Convert.ToString(reader["joindate"]);
                    user.banned = Banstatus(Convert.ToInt32(reader["id"]));

                    string? sSteamID = Convert.ToString(reader["steamid"]);

                    // SteamAPI Fetch

                    if (Regex.IsMatch(sSteamID ?? string.Empty, @"^\d+$"))
                    {
                        var webInterfaceFactory = new SteamWebInterfaceFactory("D29B43A3EDF1F47C5D5FFE76C5B9A77C");
                        var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamUser>(new HttpClient());

                        var playerSummaryResponse = await steamInterface.GetPlayerSummaryAsync(Convert.ToUInt64(sSteamID));

                        // User steamdata array constructor
                        List<SteamRow> list = new List<SteamRow>();
                        SteamRow steamrow = new SteamRow();
                        steamrow.ProfileUrl = playerSummaryResponse.Data.ProfileUrl;
                        steamrow.Avatar = playerSummaryResponse.Data.AvatarUrl;
                        steamrow.AvatarMedium = playerSummaryResponse.Data.AvatarMediumUrl;
                        steamrow.AvatarFull = playerSummaryResponse.Data.AvatarFullUrl;
                        steamrow.UserStatus = Convert.ToString(playerSummaryResponse.Data.UserStatus);
                        steamrow.RealName = playerSummaryResponse.Data.RealName;
                        steamrow.PrimaryGroupId = playerSummaryResponse.Data.PrimaryGroupId;
                        steamrow.AccountCreatedDate = Convert.ToString(playerSummaryResponse.Data.AccountCreatedDate);
                        steamrow.CountryCode = playerSummaryResponse.Data.CountryCode;
                        steamrow.PlayingGameName = playerSummaryResponse.Data.PlayingGameName;
                        steamrow.PlayingGameId = playerSummaryResponse.Data.PlayingGameId;
                        steamrow.PlayingGameServerIP = playerSummaryResponse.Data.PlayingGameServerIP;

                        list.Add(steamrow);

                        //Set user steamdata array with the precedent builded array
                        user.steamdata = list.ToArray();
                    }

                    // Finally send the user complet array to global list
                    UserList.Add(user);
                }

                reader.Close();
            }

            return Ok(JsonConvert.SerializeObject(UserList, Formatting.Indented));
        }

        // GET <Users>/latest
        [HttpGet("latest/user")]
        public string GetLatestUser()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var ResultLit = new List<UsersRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select username, steamid, joindate from `web_users` ORDER BY id DESC LIMIT 1", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // User json row
                    UsersRow user = new UsersRow();
                    user.username = Convert.ToString(reader["username"]);
                    user.steamid = Convert.ToString(reader["steamid"]);
                    user.joindate = Convert.ToString(reader["joindate"]);
                    ResultLit.Add(user);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(ResultLit, Formatting.Indented);
        }

        // GET <Users>/latest
        [HttpGet("latest/ban")]
        public string GetLatestBan()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var ResultLit = new List<BanPost>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select user, admin, time, raison from `web_bans` ORDER BY id DESC LIMIT 1", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // User json row
                    BanPost ban = new BanPost();
                    ban.user = Convert.ToInt32(reader["user"]);
                    ban.admin = Convert.ToInt32(reader["admin"]);
                    ban.time = Convert.ToInt32(reader["time"]);
                    ban.raison = Convert.ToString(reader["raison"]);
                    ResultLit.Add(ban);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(ResultLit, Formatting.Indented);
        }

        // DELETE <users>/{id}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_users` WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        // BAN <users>/ban/
        [HttpPost("ban")]
        public void Ban([FromBody] BanPost data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO `web_bans` (user, admin, time, raison) VALUES(@user, @admin, @time, @raison)", con);
                cmd.Parameters.AddWithValue("@user", data.user);
                cmd.Parameters.AddWithValue("@admin", data.admin);
                cmd.Parameters.AddWithValue("@time", data.time);
                cmd.Parameters.AddWithValue("@raison", data.raison);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        public static void UnbanFunction(int id)
        {
            Database db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_bans` WHERE user = @user", con);
                cmd.Parameters.AddWithValue("@user", id);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpDelete("unban/{id}")]
        public void Unban(int id)
        {
            UnbanFunction(id);
        }

        // UNBAN <users>/ban/
        [HttpGet("banstatus/{id}")]
        public int Banstatus(int id)
        {
            if (Auth.UserIsBan(id))
                return 1;
            else
                return 0;
        }
    }
}
