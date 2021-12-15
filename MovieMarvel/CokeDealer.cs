using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMarvel
{
    public class CokeDealer : PageModel
    {
        public int UserID { get; set; }
        public bool isAddUser { get; set; }
        public int cartID { get; set; }

        public void CheckID(string email, string password)
        {
            UserID = 0;
            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand login = new SqlCommand();
                login.Connection = myConn;
                myConn.Open();

                login.Parameters.AddWithValue("@email", email);
                login.Parameters.AddWithValue("@password", password);

                login.CommandText = ("[spLogin]");
                login.CommandType = System.Data.CommandType.StoredProcedure;

                var result = login.ExecuteScalar();

                if (result != null)
                {
                    UserID = Convert.ToInt32(result);               }


                ///////////////////////////
                // grab the users cartID
                
                SqlCommand getCartID = new SqlCommand();
                getCartID.Connection = myConn;

                getCartID.Parameters.AddWithValue("@UserID", UserID);

                getCartID.CommandText = ("[spGetCartID]");
                getCartID.CommandType = System.Data.CommandType.StoredProcedure;

                var theCartID = getCartID.ExecuteScalar();

                if (theCartID != null)
                {
                    cartID = Convert.ToInt16(theCartID);
                }

                // now check for temporary items
                List<string> tempIDs = Program.Rental.rentedIDs;
                List<string> tempTitles = Program.Rental.titles;
                List<string> tempPosters = Program.Rental.posters;

                if (tempIDs.Count > 0)
                {
                    SqlCommand addItem = new SqlCommand();
                    addItem.Connection = myConn;
                    addItem.CommandText = ("[spAddItem]");
                    addItem.CommandType = System.Data.CommandType.StoredProcedure;

                    for(int i = 0; i < tempIDs.Count; i++)
                    {
                        addItem.Parameters.AddWithValue("@MovieID", tempIDs[i]);
                        addItem.Parameters.AddWithValue("@MovieTitle", tempTitles[i]);
                        addItem.Parameters.AddWithValue("@MoviePoster", tempPosters[i]);
                        addItem.Parameters.AddWithValue("@CartID", cartID);
                        addItem.ExecuteNonQuery();
                        for (int j = 0; j < 4; j++)
                        {
                            addItem.Parameters.RemoveAt(0);
                        }
                    }
                    // now clear tempItems
                    Program.Rental.rentedIDs.Clear();
                    Program.Rental.titles.Clear();
                    Program.Rental.posters.Clear();
                }
                myConn.Close();
            }

            // grab any cart items
            Program.Rental.GetRentals();

        } // CheckID
    }
}