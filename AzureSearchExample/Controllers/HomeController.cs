using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureSearchExample.Controllers
{
    public class HomeController : Controller
    {

        #region propiedades
        private static SearchServiceClient _searchClient;
        private static ISearchIndexClient _indexClientProduct;
        private static ISearchIndexClient _indexClientUsers;

        private static string IndexNameProducts = "audiencechannelindex";
        private static string IndexNameUser = "userindex";

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private void InitSearch()
        {
            string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            string apiKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];

            // Create a reference to the NYCJobs index
            _searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));

            // Create clients
            _indexClientProduct = _searchClient.Indexes.GetClient(IndexNameProducts);            
            _indexClientUsers = _searchClient.Indexes.GetClient(IndexNameUser);
        }

        public ActionResult SuggestProducts(bool highlights, bool fuzzy, string term)
        {
            InitSearch();

            // Call suggest API and return results
            SuggestParameters sp = new SuggestParameters()
            {
                UseFuzzyMatching = fuzzy,
                Top = 5
            };

            if (highlights)
            {
                sp.HighlightPreTag = "<b>";
                sp.HighlightPostTag = "</b>";
            }

            DocumentSuggestResult suggestResult = _indexClientProduct.Documents.Suggest(term, "usernamesuggester", sp);

            // Convert the suggest query results to a list that can be displayed in the client.
            List<string> suggestions = suggestResult.Results.Select(x => x.Text).ToList();
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = suggestions
            };
        }

        public ActionResult SuggestUsers(bool highlights, bool fuzzy, string term)
        {
            InitSearch();

            // Call suggest API and return results
            SuggestParameters sp = new SuggestParameters()
            {
                UseFuzzyMatching = fuzzy,
                Top = 5
            };

            if (highlights)
            {
                sp.HighlightPreTag = "<b>";
                sp.HighlightPostTag = "</b>";
            }

            DocumentSuggestResult suggestResult = _indexClientUsers.Documents.Suggest(term, "usernamesuggester", sp);

            // Convert the suggest query results to a list that can be displayed in the client.
            List<string> suggestions = suggestResult.Results.Select(x => x.Text).ToList();
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = suggestions
            };
        }
    }
}