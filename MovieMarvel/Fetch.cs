using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieMarvel
{
    public class Fetch
    {
        public string cs = "Server=(localdb)\\mssqllocaldb;Database=MovieMarvelDB;Trusted_Connection=true";
        public HttpClient client = new HttpClient();
        public string Data { get; set; }
        public string Bios { get; set; }
        public string Images { get; set; }
        public string Search { get; set; }
        public string Videos { get; set; }
        public string Details { get; set; }
        public string Credits { get; set; }

        public async Task GrabPosterAsync(string search)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("applicationException/json"));

            // grab 20 vids
            HttpResponseMessage response = await client.GetAsync("https://api.themoviedb.org/3/search/movie?api_key=d194eb72915bc79fac2eb1a70a71ddd3&query="
                                                                + search);

            if (response.IsSuccessStatusCode)
            {
                Search = search;
                Data = await response.Content.ReadAsStringAsync();
            }
            else
            {
                Data = null; //this is so that if the search field is empty it does not show what was last searched
            }
        }

        public async Task GrabMovieAsync(string intent)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("applicationException/json"));


            // grab vids
            HttpResponseMessage response = await client.GetAsync("https://api.themoviedb.org/3/movie/" + intent
                                            + "/videos?api_key=d194eb72915bc79fac2eb1a70a71ddd3&language=en-US");
            //grab vid details
            HttpResponseMessage response2 = await client.GetAsync("https://api.themoviedb.org/3/movie/" + intent +
                                                "?api_key=d194eb72915bc79fac2eb1a70a71ddd3&language=en-US");

            HttpResponseMessage response3 = await client.GetAsync("https://api.themoviedb.org/3/movie/" + intent +
                "/credits?api_key=d194eb72915bc79fac2eb1a70a71ddd3");

            if (response.IsSuccessStatusCode || response2.IsSuccessStatusCode || response3.IsSuccessStatusCode)
            {
                Videos = await response.Content.ReadAsStringAsync();
                Details = await response2.Content.ReadAsStringAsync();
                Credits = await response3.Content.ReadAsStringAsync();
            }
        }

        public async Task GrabCastBioAsync(string id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue
                ("applicationException/json"));


            // grab Bios
            HttpResponseMessage response = 
                await client.GetAsync("https://api.themoviedb.org/3/person/" + id +
                                                "?api_key=d194eb72915bc79fac2eb1a70a71ddd3&language=en-US");

            if (response.IsSuccessStatusCode)
            {
                Bios = await response.Content.ReadAsStringAsync();
            }
        }

        public async Task GrabCastImagesAsync(string id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue
                ("applicationException/json"));

            // grab Bios
            HttpResponseMessage response =
                await client.GetAsync("https://api.themoviedb.org/3/person/" + id +
                                                "/images?api_key=d194eb72915bc79fac2eb1a70a71ddd3&language=en-US");

            if (response.IsSuccessStatusCode)
            {
                Images = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
