using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MovieMarvel.Pages
{
    public class CastModel : PageModel
    {
        public string castId = "";
        public string bio;
        public string birthday;
        public int birthYear;
        public int age;
        public string name;
        public string birthplace;
        public string img;
        public string imdb_id;
        public string imageURLs;
        public List<string> imdbs = new List<string>();
        public List<string> bios = new List<string>();
        public List<string> birthdays = new List<string>();
        public List<string> castImg = new List<string>();
        public List<string> images = new List<string>();


        public void OnPostLogin(string email, string password)
        {
            Program.CokeDealer.CheckID(email, password);
        }

        public void OnPostLogout()
        {
            Program.CokeDealer.UserID = 0;
            Response.Redirect("./Cast");
        }

        public async Task OnGet(string id)
        {
            if (id == null)
            {
                id = TempData["castID"].ToString();
            }
            else
            {
                TempData["castID"] = id;
            }

            await Program.Fetch.GrabCastBioAsync(id);
            await Program.Fetch.GrabCastImagesAsync(id);

            string bioData = Program.Fetch.Bios;
            JsonNinja bNinja = new JsonNinja(bioData);

            imdbs = bNinja.GetDetails("\"imdb_id\"");
            imdb_id = imdbs[0];
            List<string> names = bNinja.GetDetails("\"name\"");
            name = names[0];

            List<string> birthplaces = bNinja.GetDetails("\"place_of_birth\"");
            birthplace = birthplaces[0];

            string imageData = Program.Fetch.Images;
            JsonNinja iNinja = new JsonNinja(imageData);
            images = iNinja.GetVals();

            JsonNinja pNinja = new JsonNinja(images[0]);
            pNinja.StackImages(pNinja);
            images = pNinja.GetAllImages();

            birthdays = bNinja.GetDetails("\"birthday\"");
            birthday = birthdays[0];

            birthYear = Convert.ToInt16(birthday.Substring(0, 4));
            age = System.DateTime.Now.Year - birthYear;

            bios = bNinja.GetDetails("\"biography\"");
            bio = bios[0];

            bio = bio.Replace("\\", "");
            bio = bio.Replace("\\n", "");
            bio = bio.Replace(".nn", ". ");
            bio = bio.Replace(". nn", ". ");

            castImg = bNinja.GetPosters("\"profile_path\"");
            img = castImg[0];
        }
    }
}