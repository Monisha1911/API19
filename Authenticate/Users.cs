using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API9.Authenticate
{
    public class Users
    {
        
        public string UserName { get; set; }
      
        public string Email { get; set; }
    
        public string RoleType { get; set; }

        public static SqlConnection con;
        public static SqlCommand cmd;

        public static void getcon()
        {
            con = new SqlConnection("Data Source=MONISHA-LAPTOP\\SQLSERVER2019;Initial Catalog=learningportaldb;Integrated Security =true");
            con.Open();
        }

        public static List<Users> GetAllUsers()
        {
            List<Users> useer = new List<Users>();
            Users.getcon();
            cmd = new SqlCommand("select UserName,Email,roleType from Aspnetusers where roleType like 'U%'");
            cmd.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Users u = new Users();

                u.UserName = dr[0].ToString();
                u.Email = dr[1].ToString();
                u.RoleType = dr[2].ToString();


                useer.Add(u);
            }

            return useer;
        }


      
    }
}
