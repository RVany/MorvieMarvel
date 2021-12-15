using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMarvel
{
    public class Rental
    {
        public List<string> movieIDs = new List<string>(); // needed?

        public List<string> rentedIDs = new List<string>();
        public List<string> titles = new List<string>();
        public List<string> posters = new List<string>();

        public void AddRental(string id, string title, string poster)
        {
            rentedIDs.Add(id);
            titles.Add(title);
            posters.Add(poster);
        }

        public void GetRentals()
        {
            movieIDs.Clear();
            titles.Clear();
            posters.Clear();
            using (SqlConnection conn = new SqlConnection(Program.Fetch.cs))
            {
                string queryString = "SELECT * FROM Item WHERE CartID = " + 
                    Program.CokeDealer.cartID;
                SqlCommand getItems = new SqlCommand(queryString, conn);
                conn.Open();

                SqlDataReader reader = getItems.ExecuteReader();

                int MovieID = reader.GetOrdinal("MovieID");
                int Duration = reader.GetOrdinal("Duration");
                int Cost = reader.GetOrdinal("Cost");
                int Status = reader.GetOrdinal("Status");
                int MovieTitle = reader.GetOrdinal("MovieTitle");
                int MoviePoster = reader.GetOrdinal("MoviePoster");
   
                while (reader.Read())
                {
                    movieIDs.Add(reader.GetString(MovieID));
                    titles.Add(reader.GetString(MovieTitle));
                    posters.Add(reader.GetString(MoviePoster));
                }
                conn.Close();
            }
        }
    }
}