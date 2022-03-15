using Microsoft.AspNetCore.Mvc;
using webserver.Models;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using webserver.Services.UserService;
using Razor.Templating.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;

        public Auth(IConfiguration configuration, IUserService userService, IMailService mailService)
        {
            _configuration = configuration;
            _userService = userService;
            _mailService = mailService;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }

        // POST: auth/register
        [HttpPost("register")]
        public ActionResult Register(UserDto request)
        {
            Database db = new Database();
            MySqlConnection con = db.GetDatabase();
            using (con)
            {
                con.Open();

                if (UsernameAlreadyTaken(con, request.Username))
                    return BadRequest("Malheureusement ce nom d'utilisateur n'est pas disponible");
                else if (EmailAlreadyTaken(con, request.Email))
                    return BadRequest("Malheureusement cette adresse mail n'est pas disponible");
                else if (SteamIDAlreadyTaken(con, request.SteamID))
                    return BadRequest("Malheureusement ce steamid n'est pas disponible");

                string hash = "";
                using (SHA256 sha256Hash = SHA256.Create())
                    hash = GetHash(sha256Hash, request.Password);

                MySqlCommand cmd = new MySqlCommand("INSERT INTO `web_users` (email, steamid, username, password) VALUES (@email, @steamid, @username, @password)", con);
                cmd.Parameters.AddWithValue("@email", request.Email);
                cmd.Parameters.AddWithValue("@steamid", request.SteamID);
                cmd.Parameters.AddWithValue("@username", request.Username);
                cmd.Parameters.AddWithValue("@password", hash);
                cmd.ExecuteNonQuery();

                string code = CreateCode(con, GetUserId(con, request.SteamID));

                var html_confirmdata = new html_confirmdata
                {
                    Username = request.Username,
                    keycontent = code
                };
                var html = RazorTemplateEngine.RenderAsync("Templates/confirmation.cshtml", html_confirmdata);

                var mailRequest = new MailRequest
                {
                    ToEmail = request.Email,
                    Subject = "Confirmation d'inscription",
                    Body = html.Result,
                };

                ConfirmationRequest confirm = new ConfirmationRequest(_mailService);
                Task delay = confirm.SendConfirmation(mailRequest);
            }
            con.Close();

            return Ok("Votre compte a été crée avec succès !");
        }

        // POST: auth/login
        [HttpPost("login")]
        public ActionResult<string> Login([FromBody]UserDto request)
        {
            Database db = new Database();
            MySqlConnection con = db.GetDatabase();
            using (con)
            {
                con.Open();

                if (!UsernameAlreadyTaken(con, request.Username) && !EmailAlreadyTaken(con, request.Email) && !SteamIDAlreadyTaken(con, request.SteamID))
                       return BadRequest("Aucun utilisateur a été trouvée !");

                MySqlCommand cmd = new MySqlCommand("select * from `web_users` WHERE email = @email OR username = @username OR steamid = @steamid", con);
                cmd.Parameters.AddWithValue("@email", request.Email);
                cmd.Parameters.AddWithValue("@username", request.Username);
                cmd.Parameters.AddWithValue("@steamid", request.SteamID);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        if (!VerifyHash(sha256Hash, request.Password, Convert.ToString(reader["password"])))
                        {
                            reader.Close();
                            return BadRequest("Mot de passe incorrect !");
                        }

                        if (UserIsBan(Convert.ToInt32(reader["id"])))
                        {
                            reader.Close();
                            return BadRequest("Votre compte a été banni de notre site !");
                        }

                        request.Id = Convert.ToInt32(reader["id"]);
                        request.Username = Convert.ToString(reader["username"]);
                        request.SteamID = Convert.ToString(reader["steamid"]);
                        request.Email = Convert.ToString(reader["email"]);
                        request.Role = Convert.ToInt16(reader["role"]);
                        request.MailConfirmed = Convert.ToInt16(reader["mail_confirmed"]);
                    }
                }
                reader.Close();
            }
            con.Close();

            string token = CreateToken(request);

            var list = new List<JObject>();
            JObject response = JObject.FromObject(new
            {
                token = token,
            });

            list.Add(response);

            return Ok(JsonConvert.SerializeObject(list, Formatting.Indented));
        }

        private string CreateToken(UserDto user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("id", Convert.ToString(user.Id)),
                new Claim("username", user.Username ?? string.Empty),
                new Claim("email", user.Email ?? string.Empty),
                new Claim("steamid", user.SteamID ?? string.Empty),
                new Claim("role", Convert.ToString(user.Role)),
                new Claim("mail_confirmed", Convert.ToString(user.MailConfirmed)),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["jwt:key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration["jwt:issuer"],
                _configuration["jwt:issuer"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string? input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string? input, string? hash)
        {
            // Hash the input.
            var hashOfInput = GetHash(hashAlgorithm, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        private bool UsernameAlreadyTaken(MySqlConnection con, string? username)
        {
            MySqlCommand cmd = new MySqlCommand("select id from `web_users` WHERE username = @username", con);
            cmd.Parameters.AddWithValue("@username", username);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }
        }

        private bool EmailAlreadyTaken(MySqlConnection con, string? email)
        {
            MySqlCommand cmd = new MySqlCommand("select id from `web_users` WHERE email = @email", con);
            cmd.Parameters.AddWithValue("@email", email);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }
        }

        private bool SteamIDAlreadyTaken(MySqlConnection con, string? steamid)
        {
            MySqlCommand cmd = new MySqlCommand("select id from `web_users` WHERE steamid = @steamid", con);
            cmd.Parameters.AddWithValue("@steamid", steamid);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }
        }

        private int GetUserId(MySqlConnection con, string? steamid)
        {
            int value = -1;
            
            MySqlCommand cmd = new MySqlCommand("select id from `web_users` WHERE steamid = @steamid", con);
            cmd.Parameters.AddWithValue("@steamid", steamid);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                value = Convert.ToInt32(reader["id"]);
            }
           
            reader.Close();
            return value;
        }

        public static bool UserIsBan(int id)
        {
            Database db = new Database();
            MySqlConnection con = db.GetDatabase();
            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("select id, time from `web_bans` WHERE user = @user", con);
                cmd.Parameters.AddWithValue("@user", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (Convert.ToInt32(reader["time"]) > DateTimeOffset.Now.ToUnixTimeSeconds() || Convert.ToInt32(reader["time"]) == -1)
                        return true;
                    else
                    {
                        Users.UnbanFunction(id);
                        return false;
                    }
                }
                else
                    return false;
            }
        }

        [HttpGet("confirm/isconfirmed/{user}")]
        public ActionResult<string> Confirmed(int user)
        {
            int response = 0;
            Database db = new Database();
            MySqlConnection con = db.GetDatabase();
            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("select confirmed from `web_confirmations` WHERE user = @user", con);
                cmd.Parameters.AddWithValue("@user", user);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    response = Convert.ToInt32(reader["confirmed"]);
                }
            }
            con.Close();

            return Ok(response);
        }

        // POST: auth/confirm/code&user
        [HttpGet("confirm/{code}")]
        public ActionResult<string> Confirm(string code)
        {
            Database db = new Database();
            MySqlConnection con = db.GetDatabase();
            using (con)
            {
                con.Open();

                if (!CodeIsValid(con, code))
                    return BadRequest("Le code de validation n'est pas valide !");

                MySqlCommand cmd = new MySqlCommand("UPDATE `web_confirmations` SET `confirmed` = 1 WHERE code = @code", con);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.ExecuteNonQuery();

                cmd = new MySqlCommand("select user from `web_confirmations` WHERE code = @code", con);
                cmd.Parameters.AddWithValue("@code", code);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string? id = Convert.ToString(reader["user"]);
                    reader.Close();
                    
                    MySqlCommand sqlFinal = new MySqlCommand("UPDATE `web_users` SET `mail_confirmed` = 1 WHERE id = @id", con);
                    sqlFinal.Parameters.AddWithValue("@id", id);
                    sqlFinal.ExecuteNonQuery();
                }
                reader.Close();
            }
            con.Close();

            return Ok();
        }

        private bool CodeIsValid(MySqlConnection con, string code)
        {
            MySqlCommand cmd = new MySqlCommand("select confirmed from `web_confirmations` WHERE code = @code", con);
            cmd.Parameters.AddWithValue("@code", code);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (Convert.ToBoolean(reader["confirmed"]))
                {
                    reader.Close();
                    return false;
                }
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }
        }

        private string CreateCode(MySqlConnection con, int user)
        {
            string code = GenerateRandomString(10);
            
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `web_confirmations` (code, confirmed, user) VALUES (@code, 0, @user)", con);
            cmd.Parameters.AddWithValue("@code", code);
            cmd.Parameters.AddWithValue("@user", user);
            cmd.ExecuteNonQuery();

            return code;
        }

        public static string GenerateRandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
    }
}
