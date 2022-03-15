using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using webserver.Models;
using Newtonsoft.Json;
using Okolni.Source.Query;
using Newtonsoft.Json.Linq;
using System.Runtime.Intrinsics.Arm;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Servers : ControllerBase
    {
        Database db = new Database();
        public class ServerRow
        {
            public int id;
            public int port;
            public int protocol;
            public int gameid;
            public int players;
            public int maxplayers;
            public string? ip;
            public string? hostname;
            public string? map;
            public string? folder;
            public string? game;
            public string? environment;
            public Boolean VAC;
            public Array PlayersList;

            public ServerRow()
            {
                id = 0;
                port = 27015;
                protocol = 0;
                gameid = 0;
                players = 0;
                maxplayers = 16;
                ip = "127.0.0.1";
                hostname = string.Empty;
                map = string.Empty; 
                folder = string.Empty;
                game = string.Empty;
                environment = string.Empty;
                VAC = false;
                PlayersList = new string[] {};
            }
        }
        public class PlayersRow
        {
            public int Id;
            public string? Name;

            public PlayersRow()
            {
                Id = 1;
                Name = "";
            }
        }

        // GET <Servers>
        [HttpGet()]
        public string Get()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var ServerList = new List<ServerRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select id, ip, port from `web_servers`", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String? ServerIP = Convert.ToString(reader["ip"]);
                    int Port = Convert.ToInt32(reader["port"]);
                    String? MapName = "";

                    IQueryConnection conn = new QueryConnection();
                    conn.Host = ServerIP;
                    conn.Port = Port;

                    conn.Connect();

                    var info = conn.GetInfo(); // Get the server info
                    var players = conn.GetPlayers(); // Get the player info
                    Console.WriteLine(players);

                    /*          Server Json Array        */
                    ServerRow server = new ServerRow();
                    server.id = Convert.ToInt32(reader["id"]);
                    server.port = Port;
                    server.protocol = info.Protocol;
                    server.gameid = info.ID;
                    server.players = info.Players;
                    server.maxplayers = info.MaxPlayers;
                    server.ip = ServerIP;
                    server.hostname = info.Name;


                    if (info.Map.Contains("workshop"))
                    {
                        string[] parts = info.Map.Split('/');
                        MapName = parts[2];
                    }
                    else
                        MapName = info.Map;

                    server.map = MapName;
                    server.folder = info.Folder;
                    server.game = info.Game;
                    server.environment = Convert.ToString(info.Environment);
                    server.VAC = info.VAC;
                    /*          Server Json Array        */

                    /*          SourceQuery         */

                    // On créer une liste qui se base sur la structure PlayersRow qui répose sur les informations récupéré
                    var PlayersArray = players.Players.ToArray();
                    int OldID = 0;
                    List<PlayersRow> list = new List<PlayersRow>();
                    foreach (var player in PlayersArray)
                    {
                        PlayersRow playerData = new PlayersRow();
                        OldID++;
                        playerData.Id = OldID;
                        playerData.Name = player.Name;
                        list.Add(playerData);
                    }
                    
                    // On push la liste array de chaque joueur dans l'array principale de PlayersList
                    server.PlayersList = list.ToArray();
                    /*          SourceQuery         */

                    /*-------Push Server Json Array to Global Server List--------*/
                    ServerList.Add(server);
                    /*-----------------------------------------------------------*/
                }

                reader.Close();
            }
            con.Close();

            return JsonConvert.SerializeObject(ServerList, Formatting.Indented);
        }
    }
}
