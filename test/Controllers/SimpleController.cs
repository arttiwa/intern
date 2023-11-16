using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using MySqlConnector;
//using test.Models;


namespace test.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SimpleController : ControllerBase
    {
        private string? yourConnectionString = "Server=localhost;User ID=root;Password=tester;Database=user";
        MySqlConnection connection;

        public SimpleController()
        {
            connection = new MySqlConnection(yourConnectionString);
        }

        // [HttpGet]
        // [Route("api/my-route")]
        // public ActionResult<IEnumerable<string>> GetTModels()
        // {
        //     //connection.Conn
        //     return new string[] { "value1" , "value2" };
        // }

        [HttpGet]
        [Route("get-All")]
        public ActionResult<dynamic> GetAllUsers()
        {
            string query = @"SELECT * FROM users";

            MySqlDataReader myReader;
            DataTable table = new DataTable();
            connection.Open();
 
            MySqlCommand myCommand = new MySqlCommand(query, connection); 
            myReader = myCommand.ExecuteReader();
            table.Load(myReader);

            myReader.Close();
            connection.Close();

            List<object> allUser = new List<object>();
            for (int i = 0; i < table.Rows.Count; i++) {
                Dictionary<string, object> dict = new Dictionary<string, object>        //แปลงjson
                {
                    { "id", table.Rows[i]["id"] },
                    { "username", table.Rows[i]["username"] },
                    { "password", table.Rows[i]["password"] },
                    { "admin", table.Rows[i]["admin"] },
                    { "tel", table.Rows[i]["tel"] },
                    { "email", table.Rows[i]["email"] },
                    { "name", table.Rows[i]["name"] }
                };
                allUser.Add(dict);
            }
            return allUser;
        }


        [HttpGet]
        [Route("getUser-id")]
        public ActionResult<dynamic> GetUserbyID(int id)
        {
            string query = @"SELECT * FROM users  WHERE id = @userID";
            
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            connection.Open();

            MySqlCommand myCommand = new MySqlCommand(query, connection); 
            myCommand.Parameters.AddWithValue("@userID", id);

            myReader = myCommand.ExecuteReader();
            table.Load(myReader);

            myReader.Close();
            connection.Close();
            
            if (table.Rows.Count == 0)
            {
                return BadRequest();
            }
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "id", table.Rows[0]["id"] },
                { "username", table.Rows[0]["username"] },
                { "password", table.Rows[0]["password"] },
                { "admin", table.Rows[0]["admin"] },
                { "tel", table.Rows[0]["tel"] },
                { "email", table.Rows[0]["email"] },
                { "name", table.Rows[0]["name"] }
            };
            return dict;

        }

        [HttpGet]
        [Route("getUser-UserName")]
        public ActionResult<dynamic> GetUserbyUserName(string uname)
        {
            string query = @"SELECT * FROM users  WHERE username = @userName";
            
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            connection.Open();

            MySqlCommand myCommand = new MySqlCommand(query, connection); 
            myCommand.Parameters.AddWithValue("@userName", uname);

            myReader = myCommand.ExecuteReader();
            table.Load(myReader);

            myReader.Close();
            connection.Close();
            
            if (table.Rows.Count == 0)
            {
                return BadRequest();
            }
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "id", table.Rows[0]["id"] },
                { "username", table.Rows[0]["username"] },
                { "password", table.Rows[0]["password"] },
                { "admin", table.Rows[0]["admin"] },
                { "tel", table.Rows[0]["tel"] },
                { "email", table.Rows[0]["email"] },
                { "name", table.Rows[0]["name"] }
            };
            return dict;

        }

        // [HttpPost("api/my-route")]      
        // public ActionResult<string> PostTModel(TModel model)
        // {
        //     return null;
        // }

        // [HttpPut("{id}")]
        // public IActionResult PutTModel(int id, TModel model)
        // {
        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public ActionResult<TModel> DeleteTModelById(int id)
        // {
        //     return null;
        // }
    }

    public class TModel
    {
        public int id { get; set;}

        public string uname { get; set;}

    }
}