using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MovieMarvel.Pages
{
    public class AddUserModel : PageModel
    {
        public void OnGet()
        {
            Program.CokeDealer.isAddUser = true;
        }

        public void OnPostLogin(string email, string password)
        {
            Program.CokeDealer.CheckID(email, password);
        }

        public void OnPostNewUser(string email, string password)
        {
            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                int userID = 0;
                SqlCommand addUser = new SqlCommand {
                    Connection = myConn
                };
                myConn.Open();

                addUser.Parameters.AddWithValue("@email", email);
                addUser.Parameters.AddWithValue("@password", password);

                addUser.CommandText = ("[spAddUser]");
                addUser.CommandType = System.Data.CommandType.StoredProcedure;

                var result = addUser.ExecuteScalar();//returns the first column of the first row in the result set returned by the query.
                if (result != null)
                {
                    userID = Convert.ToInt32(result);
                }

                SqlCommand addCart = new SqlCommand
                {
                    Connection = myConn
                };

               // addCart.Parameters.AddWithValue("@UserID", userID);
                //addCart.CommandText = ("[spAddCart]");
                //addCart.CommandType = System.Data.CommandType.StoredProcedure;

               // addCart.ExecuteNonQuery();

                myConn.Close();
            }
            Response.Redirect("./Index");
        }
    }
}