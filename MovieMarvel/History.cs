using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMarvel
{
    public class History
    {
        public List<string> ids = new List<string>();
        public List<string> titles = new List<string>();
        public List<string> posters = new List<string>();
        
        public string SelMovieID = "";
        public string SelMovieTitle = "";
        public string SelMoviePoster = "";

        // other stuff and things:
        public string castPic;
        public List<string> castPics { get; set; }

        public void SetSelMovieID(string id)
        {
            SelMovieID = id;
        }
        public string GetSelMovieID()
        {
            return SelMovieID;
        }

        public void SetSelTitle(string title)
        {
            SelMovieTitle = title;
        }
        public string GetSelTitle()
        {
            return SelMovieTitle;
        }

        public void SetSelPoster(string poster)
        {
            SelMoviePoster = poster;
        }
        public string GetSelPoster()
        {
            return SelMoviePoster;
        }

        public void SetCastPics(List<string> duh)
        {
            castPics = duh;
        }

        public void SetCastPic(string url)
        {
            castPic = url;
        }
        public string GetCastPic()
        {
            return castPic;
        }

        public void SetPosters(List<string> posters)
        {
            this.posters = posters;
        }

        public List<string> GetPosters()
        {
            return posters;
        }

        public void SetIDs(List<string> ids)
        {
            this.ids = ids;
        }

        public List<string> GetIDs()
        {
            return ids;
        }
    }
}
