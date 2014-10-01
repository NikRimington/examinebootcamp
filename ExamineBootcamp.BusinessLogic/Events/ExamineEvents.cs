using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examine;
using Lucene.Net.Documents;
using Umbraco.Core;
using Umbraco.Core.Logging;
using UmbracoExamine;

namespace ExamineBootcamp.BusinessLogic.Events
{
    public class ExamineEvents : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
 
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
             ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"].GatheringNodeData +=
                ExternalIndexerGatheringNodeData;

             var indexer = (UmbracoContentIndexer)ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"];

             indexer.DocumentWriting += indexer_DocumentWriting;
        }

        void indexer_DocumentWriting(object sender, Examine.LuceneEngine.DocumentWritingEventArgs e)
        {
            const string dateField = "reviewDate";

                DateTime articleDate;
                if (e.Fields.ContainsKey(dateField))
                {
                    articleDate = DateTime.Parse(e.Fields[dateField]);
                }
                else
                {
                    articleDate = DateTime.Parse(e.Fields["updateDate"]);
                }
                // in __Sort_articleDate the __ implies is not analysed therefore can be used to sorting. sorted means its retrievable
                //see page 43 lucene in action 2nd edition for full explanation of options
                var field = new Field("__Sort_" + dateField, articleDate.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED);

                e.Document.Add(field);
            
        }

        private void ExternalIndexerGatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            if (e.IndexType == IndexTypes.Content)
            {             
                try
                {
                    e.Fields.Add("SearchablePath", e.Fields["path"].Replace(",", " ")); //we can then search using languge root node
                    var fields = e.Fields;
                    var combinedFields = new StringBuilder();
                    foreach (var keyValuePair in fields)
                    {
                        combinedFields.AppendLine(keyValuePair.Value);
                    }
                    e.Fields.Add("contents", combinedFields.ToString());             
                }
                catch (Exception ex)
                {
                    LogHelper.Error<Exception>("error munging fields for " + e.NodeId,ex);
                }
            }
        }
    }
}
