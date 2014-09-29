using System;
using System.Collections.Generic;
using Examine;
using Examine.Providers;

namespace ExamineBootcamp.BusinessLogic.Services
{
    public interface ISearchService
    {
        ISearchResults Search(string searchTerm, IDictionary<string, string> query, IList<string> nodeTypeAliases, out string generatedQuery, string sortBy = "", bool orFilterOnQuery = false);
    }

    public class SearchService : ISearchService
    {
        private readonly BaseSearchProvider _searcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];

        public static string Contents = "contents";
        public static string HideFromNavigation = "umbracoNaviHide";

        public ISearchResults Search(string searchTerm, IDictionary<string, string> query, IList<string> nodeTypeAliases, out string generatedQuery, string sortBy = "", bool orFilterOnQuery = false)
        {
            throw new NotImplementedException();
        }
    }
}
