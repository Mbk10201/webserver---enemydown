using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using webserver.Models;
using Newtonsoft.Json;

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Forum : ControllerBase
    {
        Database db = new Database();

        public class NodeRow
        {
            public int id { get; set; }
            public int role { get; set; }
            public string? name { get; set; }
            public string? image { get; set; }

            public NodeRow()
            {
                id = 0;
                role = 0;
                name = "";
                image = "";
            }
        }

        public class CategoryRow
        {
            public int id { get; set; }
            public int node { get; set; }
            public string? name { get; set; }
            public string? description { get; set; }

            public CategoryRow()
            {
                id = 0;
                node = 0;
                name = "";
                description = "";
            }
        }

        public class PostRow
        {
            public int id { get; set; }
            public string? content { get; set; }
            public string? date { get; set; }
            public int? topic { get; set; }
            public int? by { get; set; }

            public PostRow()
            {
                id = 0;
                content = "";
                date = "";
                topic = 0;
                by = 0;
            }
        }

        public class TopicRow
        {
            public int id { get; set; }
            public string? subject { get; set; }
            public string? content { get; set; }
            public string? date { get; set; }
            public int? category { get; set; }
            public int? by { get; set; }

            public TopicRow()
            {
                id = 0;
                subject = "";
                content = "";
                date = "";
                category = 0;
                by = 0;
            }
        }

        [HttpGet("nodes")]
        public string GetNodes()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var list = new List<NodeRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `web_forum_nodes`", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    NodeRow node = new NodeRow();
                    // loop on columns
                    node.id = Convert.ToInt32(reader["node_id"]);
                    node.role = Convert.ToInt32(reader["node_role"]);
                    node.name = Convert.ToString(reader["node_name"]);
                    node.image = Convert.ToString(reader["node_image"]);

                    list.Add(node);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        [HttpPost("addnode")]
        public void AddNode([FromBody] NodeRow node)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO `web_forum_nodes` (node_name, node_role, node_image) VALUES(@node_name, @node_role, @node_image)", con);
                cmd.Parameters.AddWithValue("@node_name", node.name);
                cmd.Parameters.AddWithValue("@node_role", node.role);
                cmd.Parameters.AddWithValue("@node_image", node.image);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpDelete("deletenode/{nodeid}")]
        public void DeleteNode(int nodeid)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_forum_nodes` WHERE node_id = @node_id", con);
                cmd.Parameters.AddWithValue("@node_id", nodeid);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpDelete("deletenodes")]
        public void DeleteNodes()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_forum_nodes`", con);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpGet("categories")]
        public string GetCategories()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var list = new List<CategoryRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `web_forum_category`", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CategoryRow cat = new CategoryRow();
                    // loop on columns
                    cat.id = Convert.ToInt32(reader["cat_id"]);
                    cat.node = Convert.ToInt32(reader["cat_node"]);
                    cat.name = Convert.ToString(reader["cat_name"]);
                    cat.description = Convert.ToString(reader["cat_description"]);

                    list.Add(cat);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        [HttpPost("addcategory")]
        public void AddCategory([FromBody] CategoryRow cat)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO `web_forum_category` (cat_node, cat_name, cat_description) VALUES(@cat_node, @cat_name, @cat_description)", con);
                cmd.Parameters.AddWithValue("@cat_node", cat.node);
                cmd.Parameters.AddWithValue("@cat_name", cat.name);
                cmd.Parameters.AddWithValue("@cat_description", cat.description);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpDelete("deletecategory/{catid}")]
        public void DeleteCategory(int catid)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_forum_category` WHERE cat_id = @cat_id", con);
                cmd.Parameters.AddWithValue("@cat_id", catid);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpDelete("deletecategories")]
        public void DeleteCategories()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_forum_category`", con);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpGet("posts")]
        public string GetPost()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var list = new List<PostRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `web_forum_posts`", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PostRow post = new PostRow();
                    // loop on columns
                    post.id = Convert.ToInt32(reader["post_id"]);
                    post.content = Convert.ToString(reader["post_content"]);
                    post.date = Convert.ToString(reader["post_date"]);
                    post.topic = Convert.ToInt32(reader["post_topic"]);
                    post.by = Convert.ToInt32(reader["post_by"]);

                    list.Add(post);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        [HttpGet("postsbytopic/{topic}")]
        public string GetPostByTopic(int topic)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var list = new List<PostRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `web_forum_posts` WHERE post_topic = @post_topic", con);
                cmd.Parameters.AddWithValue("@post_topic", topic);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PostRow post = new PostRow();
                    // loop on columns
                    post.id = Convert.ToInt32(reader["post_id"]);
                    post.content = Convert.ToString(reader["post_content"]);
                    post.date = Convert.ToString(reader["post_date"]);
                    post.topic = Convert.ToInt32(reader["post_topic"]);
                    post.by = Convert.ToInt32(reader["post_by"]);

                    list.Add(post);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        [HttpGet("postbyid/{id}")]
        public string GetPostById(int id)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var list = new List<PostRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `web_forum_posts` WHERE post_id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PostRow post = new PostRow();
                    // loop on columns
                    post.id = Convert.ToInt32(reader["post_id"]);
                    post.content = Convert.ToString(reader["post_content"]);
                    post.date = Convert.ToString(reader["post_date"]);
                    post.topic = Convert.ToInt32(reader["post_topic"]);
                    post.by = Convert.ToInt32(reader["post_by"]);

                    list.Add(post);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        [HttpPost("addpost")]
        public void AddPost([FromBody] PostRow post)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO `web_forum_posts` (post_content, post_topic, post_by) VALUES(@post_content, @post_topic, @post_by)", con);
                cmd.Parameters.AddWithValue("@post_content", post.content);
                cmd.Parameters.AddWithValue("@post_topic", post.topic);
                cmd.Parameters.AddWithValue("@post_by", post.by);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpDelete("deletepost/{posttid}")]
        public void DeletePost(int posttid)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_forum_posts` WHERE post_id = @posttid", con);
                cmd.Parameters.AddWithValue("@posttid", posttid);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpDelete("deleteposts")]
        public void DeletePosts()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_forum_posts`", con);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpGet("topics")]
        public string GetTopics()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var list = new List<TopicRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `web_forum_topics`", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TopicRow topic = new TopicRow();
                    // loop on columns
                    topic.id = Convert.ToInt32(reader["topic_id"]);
                    topic.subject = Convert.ToString(reader["topic_subject"]);
                    topic.content = Convert.ToString(reader["topic_content"]);
                    topic.date = Convert.ToString(reader["topic_date"]);
                    topic.category = Convert.ToInt32(reader["topic_cat"]);
                    topic.by = Convert.ToInt32(reader["topic_by"]);

                    list.Add(topic);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        [HttpGet("topicsbycategory/{category}")]
        public string GetTopicsByCat(int category)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var list = new List<TopicRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `web_forum_topics` WHERE topic_cat = @category", con);
                cmd.Parameters.AddWithValue("@category", category);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TopicRow topic = new TopicRow();
                    // loop on columns
                    topic.id = Convert.ToInt32(reader["topic_id"]);
                    topic.subject = Convert.ToString(reader["topic_subject"]);
                    topic.content = Convert.ToString(reader["topic_content"]);
                    topic.date = Convert.ToString(reader["topic_date"]);
                    topic.category = Convert.ToInt32(reader["topic_cat"]);
                    topic.by = Convert.ToInt32(reader["topic_by"]);

                    list.Add(topic);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        [HttpGet("topicbyid/{id}")]
        public string GetTopicById(int id)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            var list = new List<TopicRow>();

            using (con)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from `web_forum_topics` WHERE topic_id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TopicRow topic = new TopicRow();
                    // loop on columns
                    topic.id = Convert.ToInt32(reader["topic_id"]);
                    topic.subject = Convert.ToString(reader["topic_subject"]);
                    topic.content = Convert.ToString(reader["topic_content"]);
                    topic.date = Convert.ToString(reader["topic_date"]);
                    topic.category = Convert.ToInt32(reader["topic_cat"]);
                    topic.by = Convert.ToInt32(reader["topic_by"]);

                    list.Add(topic);
                }

                reader.Close();
            }

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        [HttpPost("addtopic")]
        public void AddTopic([FromBody] TopicRow topic)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO `web_forum_topics` (topic_subject, topic_cat, topic_by) VALUES(@topic_subject, @topic_cat, @topic_by)", con);
                cmd.Parameters.AddWithValue("@topic_subject", topic.subject);
                cmd.Parameters.AddWithValue("@topic_cat", topic.category);
                cmd.Parameters.AddWithValue("@topic_by", topic.by);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpDelete("deletetopic/{topicid}")]
        public void DeleteTopic(int topicid)
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_forum_topics` WHERE topic_id = @topicid", con);
                cmd.Parameters.AddWithValue("@topicid", topicid);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        [HttpDelete("deletetopics")]
        public void DeleteTopics()
        {
            db = new Database();
            MySqlConnection con = db.GetDatabase();

            using (con)
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `web_forum_topics`", con);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
    }
}
