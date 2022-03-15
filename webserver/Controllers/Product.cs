using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using webserver.Models;

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Product : ControllerBase
    {
        Database db = new Database();
        public class ProductData
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int price;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? name;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? imagefile;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? category;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? description;

            public ProductData()
            {
                id = 0;
                price = 0;
                name = "";
                imagefile = "";
                category = "";
                description = "";
            }
        }

        public class ProductClass
        {
            public int id { get; set; }
            public int price { get; set; }
            public string? name { get; set; }
            public string? imagefile { get; set; }
            public string? category { get; set; }
            public string? description { get; set; }
        }

        // Get product description
        [HttpGet("name/{productid}")]
        public ActionResult<string> GetName(int productid)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();
            var productlist = new List<ProductData>();

            using (con)
            {
                int count = 0;
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT name FROM `web_products` WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", productid);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    count++;
                    return Ok(Convert.ToString(reader["name"]));
                }

                if (count == 0)
                {
                    JsonError error = new JsonError();
                    error.message = "Aucune donnée";

                    return BadRequest(JsonConvert.SerializeObject(error, Formatting.Indented));
                }

                reader.Close();
            }
            con.Close();

            return Ok(JsonConvert.SerializeObject(productlist, Formatting.Indented));
        }

        // Get product description
        [HttpGet("description/{productid}")]
        public ActionResult<string> GetDescription(int productid)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();
            var productlist = new List<ProductData>();

            using (con)
            {
                int count = 0;
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT description FROM `web_products` WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", productid);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    count++;
                    return Ok(Convert.ToString(reader["description"]));
                }

                if (count == 0)
                {
                    JsonError error = new JsonError();
                    error.message = "Aucune donnée";

                    return BadRequest(JsonConvert.SerializeObject(error, Formatting.Indented));
                }

                reader.Close();
            }
            con.Close();

            return Ok(JsonConvert.SerializeObject(productlist, Formatting.Indented));
        }

        // Get product description
        [HttpGet("price/{productid}")]
        public ActionResult<string> GetPrice(int productid)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();
            var productlist = new List<ProductData>();

            using (con)
            {
                int count = 0;
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT price FROM `web_products` WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", productid);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    count++;
                    return Ok(Convert.ToString(reader["price"]));
                }

                if (count == 0)
                {
                    JsonError error = new JsonError();
                    error.message = "Aucune donnée";

                    return BadRequest(JsonConvert.SerializeObject(error, Formatting.Indented));
                }

                reader.Close();
            }
            con.Close();

            return Ok(JsonConvert.SerializeObject(productlist, Formatting.Indented));
        }

        // Get product description
        [HttpGet("image/{productid}")]
        public ActionResult<string> GetImage(int productid)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();
            var productlist = new List<ProductData>();

            using (con)
            {
                int count = 0;
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT imagefile FROM `web_products` WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", productid);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    count++;
                    return Ok(Convert.ToString(reader["imagefile"]));
                }

                if (count == 0)
                {
                    JsonError error = new JsonError();
                    error.message = "Aucune donnée";

                    return BadRequest(JsonConvert.SerializeObject(error, Formatting.Indented));
                }

                reader.Close();
            }
            con.Close();

            return Ok(JsonConvert.SerializeObject(productlist, Formatting.Indented));
        }

        // Get product description
        [HttpGet("category/{productid}")]
        public ActionResult<string> GetCategory(int productid)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();
            var productlist = new List<ProductData>();

            using (con)
            {
                int count = 0;
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT category FROM `web_products` WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", productid);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    count++;
                    return Ok(Convert.ToString(reader["category"]));
                }

                if (count == 0)
                {
                    JsonError error = new JsonError();
                    error.message = "Aucune donnée";

                    return BadRequest(JsonConvert.SerializeObject(error, Formatting.Indented));
                }

                reader.Close();
            }
            con.Close();

            return Ok(JsonConvert.SerializeObject(productlist, Formatting.Indented));
        }

        // Get product data
        [HttpGet("{id}")]
        public ActionResult<string> GetProduct(int id)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();
            var productlist = new List<ProductData>();

            using (con)
            {
                int count = 0;
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `web_products` WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    count++;
                    ProductData product = new ProductData();
                    product.id = Convert.ToInt32(reader["id"]);
                    product.price = Convert.ToInt32(reader["price"]);
                    product.name = Convert.ToString(reader["name"]);
                    product.imagefile = Convert.ToString(reader["imagefile"]);
                    product.category = Convert.ToString(reader["category"]);
                    product.description = Convert.ToString(reader["description"]);

                    productlist.Add(product);
                }

                if (count == 0)
                {
                    JsonError error = new JsonError();
                    error.message = "Aucune donnée";

                    return BadRequest(JsonConvert.SerializeObject(error, Formatting.Indented));
                }

                reader.Close();
            }
            con.Close();

            return JsonConvert.SerializeObject(productlist, Formatting.Indented);
        }

        // Get all products
        [HttpGet]
        public ActionResult<string> GetProducts()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();
            var productlist = new List<ProductData>();

            using (con)
            {
                int count = 0;
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `web_products`", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    count++;
                    ProductData product = new ProductData();
                    product.id = Convert.ToInt32(reader["id"]);
                    product.name = Convert.ToString(reader["name"]);
                    product.price = Convert.ToInt32(reader["price"]);
                    product.imagefile = Convert.ToString(reader["imagefile"]);
                    product.category = Convert.ToString(reader["category"]);
                    product.description = Convert.ToString(reader["description"]);

                    productlist.Add(product);
                }

                if (count == 0)
                {
                    JsonError error = new JsonError();
                    error.message = "Aucune donnée";

                    return BadRequest(JsonConvert.SerializeObject(error, Formatting.Indented));
                }

                reader.Close();
            }
            con.Close();

            return Ok(JsonConvert.SerializeObject(productlist, Formatting.Indented));
        }

        // Get all products
        [HttpGet("bycategory/{category}")]
        public ActionResult<string> GetProductsByCategory(string? category)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();
            var productlist = new List<ProductData>();

            using (con)
            {
                int count = 0;
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `web_products` WHERE category = @category", con);
                cmd.Parameters.AddWithValue("@category", category);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    count++;
                    ProductData product = new ProductData();
                    product.id = Convert.ToInt32(reader["id"]);
                    product.name = Convert.ToString(reader["name"]);
                    product.price = Convert.ToInt32(reader["price"]);
                    product.imagefile = Convert.ToString(reader["imagefile"]);
                    product.category = Convert.ToString(reader["category"]);
                    product.description = Convert.ToString(reader["description"]);

                    productlist.Add(product);
                }

                if (count == 0)
                {
                    JsonError error = new JsonError();
                    error.message = "Aucune donnée";

                    return BadRequest(JsonConvert.SerializeObject(error, Formatting.Indented));
                }

                reader.Close();
            }
            con.Close();

            return Ok(JsonConvert.SerializeObject(productlist, Formatting.Indented));
        }

        // Update product price
        [HttpPost("price")]
        public void UpdatePrice([FromBody] ProductClass data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE `web_products` SET `price` = @price WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@price", data.price);
                cmd.Parameters.AddWithValue("@id", data.id);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        // Update product description
        [HttpPost("description")]
        public void UpdateDescription([FromBody] ProductClass data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE `web_products` SET `description` = @description WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@description", data.description);
                cmd.Parameters.AddWithValue("@id", data.id);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        // Update product name
        [HttpPost("name")]
        public void UpdateName([FromBody] ProductClass data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE `web_products` SET `name` = @name WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@name", data.name);
                cmd.Parameters.AddWithValue("@id", data.id);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        // Update product name
        [HttpPost("image")]
        public void UpdateImage([FromBody] ProductClass data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE `web_products` SET `imagefile` = @imagefile WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@imagefile", data.imagefile);
                cmd.Parameters.AddWithValue("@id", data.id);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }


        // Update product name
        [HttpPost("category")]
        public void UpdateCategory([FromBody] ProductClass data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE `web_products` SET `category` = @category WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@category", data.category);
                cmd.Parameters.AddWithValue("@id", data.id);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
    }
}
