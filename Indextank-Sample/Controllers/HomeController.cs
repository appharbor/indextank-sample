using System.Web.Mvc;

namespace Indextank_Sample.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var index = MvcApplication.GetIndexTankIndex();
			return View(index.Size);
		}
	}
}
