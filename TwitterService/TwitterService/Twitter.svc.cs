using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TwitterService.Model;

namespace TwitterService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Twitter" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Twitter.svc or Twitter.svc.cs at the Solution Explorer and start debugging.
	public class Twitter : ITwitter
    {

        TwitterEntities db = new TwitterEntities();

        public bool AddNewTwit(int id, string content)
        {
            Twit t = new Twit();
            t.UserId = id;
            t.TwitContent = content;
            t.Date = DateTime.Now;
            db.Twits.Add(t);
            db.SaveChanges();
            return true;
        }

        public bool Follow(int follower, int followed)
        {
            UserFollow uf = new UserFollow();
            uf.FollowerId = follower;
            uf.FollowedId = followed;
            db.UserFollows.Add(uf);
            db.SaveChanges();
            return true;
        }

        public List<TwitLocal> GetAllTwits(int id)
        {
            using (db = new TwitterEntities())
            {
                List<TwitLocal> twits = new List<TwitLocal>();
                foreach (var u in db.UserFollows.Where(x => x.FollowerId == id))
                {
                    foreach (var t in db.Twits.Where(x => x.UserId == u.FollowedId))
                    {
                        TwitLocal tl = new TwitLocal();
                        tl.Id = t.Id;
                        tl.Content = t.TwitContent;
                        tl.Date = t.Date;
                        tl.Fav = db.TwitFavs.Count(x => x.TwitId == t.Id && x.UserId==id);

                        User user = db.Users.FirstOrDefault(x => x.Id == t.UserId);
                        tl.Username = user.Username;
                        tl.Name = user.Name;

                        if (db.TwitFavs.Any(x => x.TwitId == t.Id && x.UserId == id))
                        {
                            tl.isFav = true;
                        }
                        else
                        {
                            tl.isFav = false;
                        }

                        twits.Add(tl);
                    }
                }

               return twits.OrderByDescending(x => x.Id).ToList();

            }
        }

        public List<TwitLocal> GetNewTwits(int id, DateTime lastDate)
        {
            List<TwitLocal> twits = new List<TwitLocal>();
            foreach (var u in db.UserFollows.Where(x=>x.FollowerId == id))
            {
                foreach (var t in db.Twits.Where(x=>x.UserId == u.FollowedId && x.Date>lastDate))
                {
                    TwitLocal tl = new TwitLocal();
                    tl.Content = t.TwitContent;
                    tl.Date = t.Date;
                    tl.Fav = db.TwitFavs.Count(x => x.TwitId == t.Id && x.UserId == id);

                    User user = db.Users.FirstOrDefault(x=>x.Id==t.UserId);
                    tl.Username = user.Username;
                    tl.Name = user.Name;

                    if(db.TwitFavs.Any(x=>x.TwitId==t.Id && x.UserId == id))
                    {
                        tl.isFav = true;
                    }
                    else
                    {
                        tl.isFav = false;
                    }

                    twits.Add(tl);
                }
            }

            twits.OrderByDescending(x => x.Date);
            return twits;
        }

        public KisiLocal GetUser(int id)
        {
            User u = db.Users.FirstOrDefault(x => x.Id == id);

            KisiLocal k = new KisiLocal();
            k.Id = u.Id;
            k.Name = u.Name;
            k.Username = u.Username;
            k.Email = u.Email;
            return k;
        }

        public int Login(string user, string password)
        {
            Helper h = new Helper();
            string pass = h.MD5Sifrele(password);
            if (db.Users.Any(x=>(x.Email == user || x.Username == user) && x.Password == pass))
            {
                return db.Users.FirstOrDefault(x => (x.Email == user || x.Username == user) && x.Password == pass).Id;
            }
            else
            {
                return 0;
            }
        }

        public int NewTwitControl(int id, DateTime lastDate)
        {
            int count = 0;
            foreach (var u in db.UserFollows.Where(x => x.FollowerId == id))
            {
                foreach (var t in db.Twits.Where(x => x.UserId == u.FollowedId && x.Date > lastDate))
                {
                    count++;
                }
            }
            return count;
        }

        public int Register(string email, string username, string password, string name)
        {
            Helper h = new Helper();
            if (!db.Users.Any(x => (x.Email == email || x.Username == username)))
            {
                User u = new User();
                u.Email = email;
                u.Username = username;
                u.Password = h.MD5Sifrele(password);
                u.Name = name;
                u.RegisterDate = DateTime.Now;
                db.Users.Add(u);
                db.SaveChanges();

                UserFollow uf = new UserFollow();
                uf.FollowerId = u.Id;
                uf.FollowerId = u.Id;
                db.UserFollows.Add(uf);
                db.SaveChanges();

                return u.Id;
            }
            else
            {
                return 0;
            }
        }

		public KisiLocal Search(string contains)
		{
			throw new NotImplementedException();
		}

		public bool TwitFav(int id, int twitId)
        {
            if(db.TwitFavs.Any(x=>x.UserId == id && x.TwitId == twitId))
            {
                TwitFav tf = db.TwitFavs.FirstOrDefault(x => x.UserId == id && x.TwitId == twitId);
                db.TwitFavs.Remove(tf);
                db.SaveChanges();
                return false;
            }
            else
            {
                TwitFav tf = new TwitFav();
                tf.TwitId = twitId;
                tf.UserId = id;
                db.TwitFavs.Add(tf);
                db.SaveChanges();
                return true;
            }
        }

        public bool Update(int id,string email, string username, string password, string name)
        {
            Helper h = new Helper();
            User u = db.Users.FirstOrDefault(x => x.Id == id);
            u.Email = email;
            u.Username = username;
            if (password != "")
            {
                u.Password = h.MD5Sifrele(password);
            }
            u.Name = name;
            db.SaveChanges();
            return true;
        }
    }
}
