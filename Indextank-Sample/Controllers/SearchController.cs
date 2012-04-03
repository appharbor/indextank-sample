using System.Linq;
using System.Web.Mvc;

namespace Indextank_Sample.Controllers
{
	public class SearchController : Controller
	{
		public ActionResult Show(string query)
		{
			var index = MvcApplication.GetIndexTankIndex();
			var result = index.Search(query);
			return View(result.ResultDocuments.Select(x => x.DocumentId));
		}
	}
}
