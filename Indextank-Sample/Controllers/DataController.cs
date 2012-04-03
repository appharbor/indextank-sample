using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using IndexTankDotNet;

namespace Indextank_Sample.Controllers
{
	public class DataController : Controller
	{
		private const int _indexTankMaximumStringLength = 100000;
		private readonly IEnumerable<KeyValuePair<string, string>> _documentsToIndex;

		public DataController()
		{
			_documentsToIndex = new DirectoryInfo(string.Format("{0}/{1}", HostingEnvironment.ApplicationPhysicalPath,"data"))
				.GetFiles()
				.Select(x => new KeyValuePair<string, string>(x.Name, x.FullName));
		}

		public ActionResult Create()
		{
			var documents = _documentsToIndex.AsParallel().Select(
				x => new { name = x.Key, text = System.IO.File.ReadAllText(x.Value) });

			var index = MvcApplication.GetIndexTankIndex();

			index.AddDocuments(documents.Select(x =>
				new Document(x.name, x.text.Substring(0, Math.Min(x.text.Length, _indexTankMaximumStringLength)))));

			TempData["SuccessMessage"] = "Data loaded";
			return RedirectToAction("Index", "Home");
		}

		public ActionResult Destroy()
		{
			var index = MvcApplication.GetIndexTankIndex();
			index.DeleteDocuments(_documentsToIndex.Select(x => x.Key));
			TempData["SuccessMessage"] = "Documents deleted";
			return RedirectToAction("Index", "Home");
		}
	}
}
