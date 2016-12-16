using Twitter.TwitterService;

namespace Twitter.Controllers
{
	public class HomeController : Controller
	{
		TwitterClient t = new TwitterClient();
		public ActionResult Index()
		{
			if (Session["id"] == null)
			{
				return RedirectToAction("Login", "Home");
			}
			ViewBag.kisi = t.GetUser(Convert.ToInt32(Session["id"]));
			return View();
		}

		public ActionResult AllTwits()
		{
			ViewBag.AllTwits = t.GetAllTwits(Convert.ToInt32(Session["id"]));
			Session["lastdate"] = DateTime.Now;
			return View();
		}

		public ActionResult Register()
		{
			if (Session["id"] != null)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}

		public ActionResult Login()
		{
			if (Session["id"] != null)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}

		public JsonResult NewTwitControl()
		{
			int count = t.NewTwitControl(Convert.ToInt32(Session["id"]), Convert.ToDateTime(Session["lastdate"]));
			if (count > 0)
			{
				var jsonModel = new
				{
					basari = 1,
					count = count
				};
				return Json(jsonModel, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var jsonModel = new
				{
					basari = 0,
					count = 0
				};
				return Json(jsonModel, JsonRequestBehavior.AllowGet);
			}
		}

		public JsonResult AddNewTweet(string content)
		{
			var jsonModel = new
			{
				mesaj = t.AddNewTwit(Convert.ToInt32(Session["id"]), content)
			};
			return Json(jsonModel, JsonRequestBehavior.AllowGet);
		}

		public JsonResult UpdateUser(string username, string email, string password, string name)
		{
			if (t.Update(Convert.ToInt32(Session["id"]), email, username, password, name))
			{
				var jsonModel = new
				{
					basari = 1
				};
				return Json(jsonModel, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var jsonModel = new
				{
					basari = 0
				};
				return Json(jsonModel, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult RegisterUser(string name, string email, string username, string password)
		{
			Session["id"] = t.Register(email, username, password, name);
			return RedirectToAction("Index", "Home");
		}

		public JsonResult LoginUser(string user, string password)
		{
			int id = t.Login(user, password);
			if (id != 0)
			{
				Session["id"] = id;
				var jsonModel = new
				{
					basari = 1
				};
				return Json(jsonModel, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var jsonModel = new
				{
					basari = 0
				};
				return Json(jsonModel, JsonRequestBehavior.AllowGet);
			}
		}

		public JsonResult TwitFav(int twitId)
		{
			var jsonModel = new
			{
				basari = t.TwitFav(Convert.ToInt32(Session["id"]), twitId)
			};
			return Json(jsonModel, JsonRequestBehavior.AllowGet);
		}
	}
}