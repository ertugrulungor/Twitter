using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterService
{
    public class TwitLocal
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int Fav { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public bool isFav { get; set; }
    }
}