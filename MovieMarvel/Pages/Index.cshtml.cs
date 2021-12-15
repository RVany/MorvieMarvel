using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks; 

namespace MovieMarvel.Pages
{
    public class IndexModel : PageModel
    {
        public List<string> movies = new List<string>();
        public List<string> posterPath = new List<string>();
        public List<string> id = new List<string>();
        public List<string> vidClips = new List<string>();
        public List<string> titles = new List<string>();
        public List<string> actorImg = new List<string>();
        public List<string> castID = new List<string>();
        public List<string> filter = new List<string>();

        public string selTitle = "";
        public string selPoster = "";
        public int voteAmount;
        public string rating = "";
        public string description = "";
        public string goldStar = "";
        public string display = "none";
        public string search = "";
        public bool empty = false;
        public string backDrop = "";
        public string holder = "search";
        JsonNinja jNinja;
        JsonNinja vidNinja;


        //==========================//
        //====== GET Methods ======//
        public int GetVote()
        {
            int amount = 0;

            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand getVote = new SqlCommand
                {
                    Connection = myConn
                };
                myConn.Open();

                getVote.Parameters.AddWithValue("@MovieID", Program.History.GetSelMovieID());

                getVote.CommandText = ("[spGetVote]");
                getVote.CommandType = System.Data.CommandType.StoredProcedure;

                var result = getVote.ExecuteScalar();

                if (result != null)
                {
                    amount = Convert.ToInt32(result);
                }

                myConn.Close();
            }

            return amount;
        }

        public async Task OnGet()
        {
            string elvisStinks = Program.History.GetSelMovieID();
            if (elvisStinks != "")
            {
                await OnPostDetails(elvisStinks);
            }
        }

        //===========================//
        //====== POST Methods ======//
        public void OnPostRent()
        {
            // we need that movieID
            string selectedMovieID = Program.History.GetSelMovieID();
            string selTitle = Program.History.GetSelTitle();
            string selPoster = Program.History.GetSelPoster();

            // we need their userID
            int userID = Program.CokeDealer.UserID;

            if (userID == 0) // not logged in
            {
                Program.Rental.AddRental(selectedMovieID, selTitle, selPoster);
            }
            else // logged in
            {
                using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
                {
                    int cartID = 0;
                    // first get cartID
                    SqlCommand getCartID = new SqlCommand();
                    getCartID.Connection = myConn;
                    myConn.Open();

                    getCartID.Parameters.AddWithValue("@UserID", userID);

                    getCartID.CommandText = ("[spGetCartID]");
                    getCartID.CommandType = System.Data.CommandType.StoredProcedure;

                    var result = getCartID.ExecuteScalar();
                    if (result != null)
                    {
                        cartID = Convert.ToInt32(result);
                    }

                    SqlCommand addItem = new SqlCommand();
                    addItem.Connection = myConn;

                    addItem.Parameters.AddWithValue("@MovieID", selectedMovieID);
                    addItem.Parameters.AddWithValue("@MovieTitle", selTitle);
                    addItem.Parameters.AddWithValue("@MoviePoster", selPoster);
                    addItem.Parameters.AddWithValue("@CartID", cartID);

                    addItem.CommandText = ("[spAddItem]");
                    addItem.CommandType = System.Data.CommandType.StoredProcedure;

                    addItem.ExecuteNonQuery();

                    myConn.Close();
                }
            }
            Response.Redirect("./Index");
        }

        public void OnPostLogin(string email, string password)
        {
           Program.CokeDealer.CheckID(email, password);
           Response.Redirect("./Index");
        }

        public void OnPostLogout()
        {
            Program.CokeDealer.UserID = 0;
            Response.Redirect("./Index");
        }

        public void OnPostVote(int vote)
        {
            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand addVote = new SqlCommand();
                addVote.Connection = myConn;
                myConn.Open();

                addVote.Parameters.AddWithValue("@MovieID", Program.History.GetSelMovieID());
                addVote.Parameters.AddWithValue("@VoteRating", vote);

                addVote.CommandText = ("[spAddVote]");
                addVote.CommandType = System.Data.CommandType.StoredProcedure;

                addVote.ExecuteNonQuery();

                myConn.Close();
            }
            Response.Redirect("./Index");
        }

        public void OnPostCast(string cast)
        {
            string origIntent = cast;
            int theID = Convert.ToInt32(cast);
            Response.Redirect("./Cast?id=" + theID);
        }

        public async Task OnPostMovies(string sarch)
        {
            this.search = sarch;
            await ShowPoster(search);
        }

        public async Task OnPostDetails(string movieData)
        {
            await ShowPoster(Program.Fetch.Search);
            search = Program.Fetch.Search;

            await Program.Fetch.GrabMovieAsync(movieData);
            vidNinja = new JsonNinja(Program.Fetch.Videos);

            filter = vidNinja.GetDetails("\"results\"");

            vidNinja = new JsonNinja(filter[0]);

            List<string> vidNames = vidNinja.GetNames();
            List<string> vidVals = vidNinja.GetVals();

            jNinja = new JsonNinja(Program.Fetch.Details);
            List<string> detailNames = jNinja.GetNames();
            List<string> detailVals = jNinja.GetVals();

            // Get movie details
            display = "grid";
            titles = jNinja.GetDetails("\"title\"");
            selTitle = titles[0];
            List<string> ratings = jNinja.GetIds("\"vote_average\"");
            rating = ratings[0] + "/10.0";
            List<string> descriptions = jNinja.GetDetails("\"overview\"");
            description = descriptions[0];
            List<string> backDrops = jNinja.GetDetails("\"poster_path\"");
            backDrop = backDrops[0];
            selPoster = backDrops[0];

            // Get youtube movie keys for poster
            vidClips = vidNinja.GetDetails("\"key\"");

            // Get movie cast
            jNinja = new JsonNinja(Program.Fetch.Credits);
            List<string> creditNames = jNinja.GetNames();
            List<string> creditVals = jNinja.GetVals();

            Program.History.SetSelMovieID(creditVals[0]);
            Program.History.SetSelTitle(selTitle);
            Program.History.SetSelPoster(selPoster);

            // Get the vote for that movie
            voteAmount = GetVote();

            filter = jNinja.GetDetails("\"cast\"");

            jNinja = new JsonNinja(filter[0]);

            List<string> actorName = jNinja.GetDetails("\"name\"");
            castID = jNinja.GetIds("\"id\"");
            actorImg = jNinja.GetDetails("\"profile_path\"");

            List<string> character = jNinja.GetDetails("\"character\"");
        }

        private async Task ShowPoster(string search)
        {
            await Program.Fetch.GrabPosterAsync(search);

            if(Program.Fetch.Data == null)
            {
                empty = true;
            }
            else
            {
                jNinja = new JsonNinja(Program.Fetch.Data);

                List<string> names = jNinja.GetNames();
                List<string> vals = jNinja.GetVals();

                filter = jNinja.GetDetails("\"results\"");

                jNinja = new JsonNinja(filter[0]);

                posterPath = jNinja.GetPosters("\"poster_path\"");
                id = jNinja.GetIds("\"id\"");
            }
        }
    }
}