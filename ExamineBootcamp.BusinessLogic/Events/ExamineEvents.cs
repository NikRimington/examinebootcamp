using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examine;
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
        }

        private void ExternalIndexerGatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            if (e.IndexType == IndexTypes.Content)
            {             
                try
                {
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
