using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using webserver.Models;

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Cart : ControllerBase
    {
        Database db = new Database();
        public class CartData
        {
            public int id;
            public int price;
            public int product;
            public string? name;
            public string? imagefile;
            public int quantity;

            public CartData()
            {
                id = 0;
                price = 0;
                product = 0;
                name = "";
                imagefile = "";
                quantity = 0;
            }
        }

        public class CartUpdate
        {
            public int userid { get; set; }
            public int productid { get; set; }
            public int quantity { get; set; }
        }

        // Get product list from client
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<string> GetProducts(int id)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();
            var productlist = new List<CartData>();

            using (con)
            {
                int count = 0;
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT t1.id, product, quantity, name, price, imagefile FROM web_cart t1 INNER JOIN web_products t2 ON t1.product = t2.id WHERE t1.user = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    count++;
                    CartData product = new CartData();
                    product.id = Convert.ToInt32(reader["id"]);
                    product.product = Convert.ToInt32(reader["product"]);
                    product.name = Convert.ToString(reader["name"]);
                    product.imagefile = Convert.ToString(reader["imagefile"]);
                    product.quantity = Convert.ToInt32(reader["quantity"]);
                    product.price = Convert.ToInt32(reader["price"]);

                    productlist.Add(product);
                }

                if(count == 0)
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

        // Add Client product to cart
        [HttpPost]
        public void Add([FromBody] CartUpdate data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO `web_cart` (user, product, quantity) VALUES(@user, @product, @quantity)", con);
                cmd.Parameters.AddWithValue("@user", data.userid);
                cmd.Parameters.AddWithValue("@product", data.productid);
                cmd.Parameters.AddWithValue("@quantity", data.quantity);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        // Update client product quantity
        [HttpPut]
        public void Put([FromBody] CartUpdate data)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE `web_cart` SET `quantity` = @quantity WHERE user = @userid AND product = @productid", con);
                cmd.Parameters.AddWithValue("@quantity", data.quantity);
                cmd.Parameters.AddWithValue("@userid", data.userid);
                cmd.Parameters.AddWithValue("@productid", data.productid);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        // Empty client Cart
        [HttpDelete("{userid}")]
        public void Delete(int userid)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_cart` WHERE user = @userid", con);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        // Remove from client cart
        [HttpDelete("{userid}/{id}")]
        public void Delete(int userid, int id)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_cart` WHERE user = @userid AND id = @id", con);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
    }
}
