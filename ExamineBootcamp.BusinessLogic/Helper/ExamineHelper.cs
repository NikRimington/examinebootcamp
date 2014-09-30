using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
    }
}
