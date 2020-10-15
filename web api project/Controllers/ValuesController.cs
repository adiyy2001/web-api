using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;

namespace web_api_project.Controllers
{
    public class ValuesController : ApiController
    {

        public class results
        {
            public string name { get; set; }
            public string password { get; set; }
            public string error { get; set; }
            public results(string name, string password, string error)
            {
                this.name = name;
                this.password = password;
                this.error = error;
            }
        }
        // GET api/values
        public List<results> Get()
        {
            MySqlConnection conn = WebApiConfig.conn();
            string query = "Select username, password FROM all_users";
            MySqlCommand selectAll = new MySqlCommand(query, conn);

            var results = new List<results>();
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new results(null, null, ex.ToString()));
                return results;
            }
            MySqlDataReader fetch_query = selectAll.ExecuteReader();

            while (fetch_query.Read())
            {
                results user = new results(
                    fetch_query["username"].ToString(), 
                    fetch_query["password"].ToString(),
                    null
                );
                results.Add(user);
            }

            return results;

        }

        // GET api/values/5
        public List<results> Get(int id)
        {
            MySqlConnection conn = WebApiConfig.conn();
            string query = $"Select username, password FROM all_users WHERE id='{id}'";

            MySqlCommand selectByIdQuery = new MySqlCommand(query, conn);

            var results = new List<results>();
            try
            {
                conn.Open();
            } catch(MySql.Data.MySqlClient.MySqlException ex) 
            {
                results user = new results( null, null, ex.ToString() );
                results.Add(user);
                return results;
            }

            MySqlDataReader fetch_query = selectByIdQuery.ExecuteReader();

            if(fetch_query.Read())
            {
                results user = new results(
                    fetch_query["username"].ToString(), 
                    fetch_query["password"].ToString(), 
                    null
                );
                results.Add(user);
            }

            return results;
        }

        // POST api/values
        public dynamic Post([FromBody] dynamic value)
        {
            MySqlConnection conn = WebApiConfig.conn();
            string query = $"UPDATE all_users " +
                $"SET username='{value.username}', password='{value.password}' " +
                $"WHERE id='{value.id}'";

            MySqlCommand update = new MySqlCommand(query, conn);

            try
            {
                conn.Open();
            } catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                return query;
            }

            update.ExecuteReader();
            return "Congrats updated";
        }

        // DELETE api/values/5
        public string Delete(int id)
        {
            MySqlConnection conn = WebApiConfig.conn();
            string query =  $"DELETE FROM all_users WHERE id={id}";
            
            MySqlCommand removeUser = new MySqlCommand(query, conn);

            try
            {
                conn.Open();
            } catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return ex.ToString();
            }
            removeUser.ExecuteReader();

            return "Deleted";
        }
    }
}
