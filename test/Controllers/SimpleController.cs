using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using MySqlConnector;
using System.Security.Cryptography;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using Newtonsoft.Json;
using test.models;


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
        
        static string ComputeSha256Hash(string? rawData)
        {
            if (rawData == null)
            {
                return "";
            }
            string salt = "should to random";

            string saltData = rawData + "" +salt;
            //create a Sha256                
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(saltData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
                
            }

        }

        
        // [HttpGet]
        // [Route("api/my-route")]
        // public ActionResult<IEnumerable<string>> GetTModels()
        // {
        //     //connection.Conn
        //     return new string[] { "value1" , "value2" };
        // }

        [HttpGet]
        [Route("get-all")]
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
            for (int i = 0; i < table.Rows.Count; i++)
            {
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
                { "admin", table.Rows[0]["admin"] },
                { "tel", table.Rows[0]["tel"] },
                { "email", table.Rows[0]["email"] },
                { "name", table.Rows[0]["name"] }
            };
            return dict;

        }



        [HttpGet]
        [Route("getUser-NP")]
        public ActionResult<dynamic> GetUserNameAndPassword(string uname, string pass)
        {
            string query = @"SELECT * FROM users  WHERE username = @uname AND password = @pass";

            string hashedPass = ComputeSha256Hash(pass);
            
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            connection.Open();

            MySqlCommand myCommand = new MySqlCommand(query, connection);
            myCommand.Parameters.AddWithValue("@uname", uname);
            myCommand.Parameters.AddWithValue("@pass", hashedPass);

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
                { "username", table.Rows[0]["username"] },
                { "name", table.Rows[0]["name"] }
            };

            return dict;
            // Console.WriteLine("Raw data: {0}", pass);
            // string? hashedPassw = ComputeSha256Hash(pass);
            // Console.WriteLine("Hash {0}", hashedPassw);
            // Console.WriteLine(ComputeSha256Hash(pass));
            // //Console.ReadLine();

            // Dictionary<string, object> dictss = dict; // Your existing dictionary

            // var passwordObj = new { Password = dictss["password"] };

            // string jsonString = JsonConvert.SerializeObject(passwordObj);

            //return dict;
            //return jsonString + "Hashed: " + hashedPassw; //System.Collections.Generic.Dictionary`2[System.String,System.Object]
                                                          //8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92
                                                          //return (dict,hashedPassw);;
        }

        [HttpPost]
        [Route("PostUser-NP")]
        public ActionResult<dynamic> CreateUser(User user)
        {
            string? pass = user.pass;
            string hashedPass = ComputeSha256Hash(pass);


            string query = @"INSERT INTO users(`username`, `password`,`admin`, `tel`, `email`,`name`)
                        VALUES (@uname, @pass, @admin, @tel, @email, @name);";


            DataTable table = new DataTable();
            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(query, connection);

            myCommand.Parameters.AddWithValue("@uname", user.username);
            myCommand.Parameters.AddWithValue("@pass", hashedPass);
            myCommand.Parameters.AddWithValue("@admin", user.admin);
            myCommand.Parameters.AddWithValue("@tel", user.tel);
            myCommand.Parameters.AddWithValue("@email", user.email);
            myCommand.Parameters.AddWithValue("@name", user.name);

            Console.WriteLine("Records Inserted Successfully");

            int rowAff = myCommand.ExecuteNonQuery();
            if (rowAff == 0)
            {
                return BadRequest("Error Generated");
            }
            connection.Close();
            return Ok("Records Inserted Successfully");
        }


        

    }
}