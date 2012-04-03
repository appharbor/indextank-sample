using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IndexTankDotNet;

namespace Indextank_Sample.Controllers
{
	public class DataController : Controller
	{
		private const int _indexTankMaximumStringLength = 100000;

		private readonly IEnumerable<string> _documentsToIndex = new[] {
			"http://www.gutenberg.org/cache/epub/1795/pg1795.txt",
			"http://www.gutenberg.org/cache/epub/1112/pg1112.txt",
			"http://www.gutenberg.org/cache/epub/1119/pg1119.txt",
			"http://www.gutenberg.org/cache/epub/1120/pg1120.txt",
			"http://www.gutenberg.org/cache/epub/2266/pg2266.txt",
			"http://www.gutenberg.org/cache/epub/2265/pg2265.txt",
		};

		public ActionResult Create()
		{
			var index = MvcApplication.GetIndexTankIndex();
			var documents = _documentsToIndex.AsParallel().Select(
				x => new { url = x, text = new WebClient().DownloadString(x)});
			index.AddDocuments(documents.Select(x =>
				new Document(x.url, x.text.Substring(0, Math.Min(x.text.Length, _indexTankMaximumStringLength)))));

			TempData["SuccessMessage"] = "Data loaded";
			return RedirectToAction("Index", "Home");
		}

		public ActionResult Destroy()
		{
			var index = MvcApplication.GetIndexTankIndex();
			index.DeleteDocuments(_documentsToIndex);
			TempData["SuccessMessage"] = "Documents deleted";
			return RedirectToAction("Index", "Home");
		}
	}
}
