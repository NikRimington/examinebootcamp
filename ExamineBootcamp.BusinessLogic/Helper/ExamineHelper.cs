using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Examine;
using Examine.LuceneEngine.Providers;
using Lucene.Net.Analysis.Standard;

namespace ExamineBootcamp.BusinessLogic.Helper
{
    public static class ExamineHelper
    {
        /// <summary>
        /// take from http://stackoverflow.com/questions/263081/how-to-make-the-lucene-queryparser-more-forgiving
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string MakeSearchQuerySafe(this string query)
        {
            var regex = new Regex(@"[^\w\s-]");
            return regex.Replace(query, "");
        }

        public static MultiIndexSearcher GetMultiSearcher(string[] indexes)
        {
            var directories = new List<DirectoryInfo>();
            foreach (var index in indexes)
            {
                var indexer = ExamineManager.Instance.IndexProviderCollection[index];
                var dir = new DirectoryInfo(((LuceneIndexer)indexer).LuceneIndexFolder.FullName.Replace("\\Index", ""));
                directories.Add(dir);

            }
            var i = new MultiIndexSearcher(directories, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            return i;
        }
    }
}
