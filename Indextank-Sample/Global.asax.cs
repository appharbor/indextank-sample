using System.Linq;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using IndexTankDotNet;

namespace Indextank_Sample
{
	public class MvcApplication : System.Web.HttpApplication
	{
		private static readonly string _indexTankApiUrl = ConfigurationManager.AppSettings.Get("SEARCHIFY_API_URL");
		private static readonly string _indexName = "texts";

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

			CreateIndexIfNotExists();
		}
		
		private static void CreateIndexIfNotExists()
		{
			var indexTankClient = new IndexTankClient(_indexTankApiUrl);
			if (!indexTankClient.GetIndexes().Any(x => x.Name == _indexName))
			{
				indexTankClient.CreateIndex(_indexName);
			}
		}

		public static Index GetIndexTankIndex()
		{
			var indexTankClient = new IndexTankClient(_indexTankApiUrl);
			return indexTankClient.GetIndex(_indexName);
		}
	}
}
