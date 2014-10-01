using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examine;
using Examine.LuceneEngine;

namespace ExamineBootcamp.BusinessLogic.Indexer
{
    public class DbIndexer : ISimpleDataService
    {
        public IEnumerable<SimpleDataSet> GetAllData(string indexType)
        {
            return GetData(indexType);
        }

        private IEnumerable<SimpleDataSet> GetData(string indexType)
        {
            var builder = new SqlConnectionStringBuilder();

            builder["server"] = ".\\sqlexpress";

            builder["database"] = "UKTowns";
            builder["user id"] = "towns";
            builder["password"] = "towns";

            using (var sqlConn = new SqlConnection(builder.ConnectionString))
            {
                sqlConn.Open();
                var objSqlCmd = new SqlCommand { Connection = sqlConn, CommandText = "select * from [UKTowns].[dbo].[uktownslist-sample]"};
                    using (var reader = objSqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int fields = reader.FieldCount;
                            var sds = new SimpleDataSet { NodeDefinition = new IndexedNode(), RowData = new Dictionary<string, string>() };

                            for (int i = 0; i < fields; i++)
                            {
                                if (i == 0)
                                {
                                    sds.NodeDefinition.NodeId = Convert.ToInt32(reader[0]);
                                    sds.NodeDefinition.Type = indexType;
                                }
                                else
                                {
                                    sds.RowData.Add(reader.GetName(i), XmlCharacterWhitelist(reader[i].ToString()));
                                }
                            }

                            yield return sds;
                        }
                    }
                
                sqlConn.Close();
            }

        }

        public static string XmlCharacterWhitelist(string inString)
        {
            if (inString == null) return null;

            StringBuilder sbOutput = new StringBuilder();
            char ch;

            for (int i = 0; i < inString.Length; i++)
            {
                ch = inString[i];
                if ((ch >= 0x0020 && ch <= 0xD7FF) ||
                        (ch >= 0xE000 && ch <= 0xFFFD) ||
                        ch == 0x0009 ||
                        ch == 0x000A ||
                        ch == 0x000D)
                {
                    sbOutput.Append(ch);
                }
            }
            return sbOutput.ToString();
        }
    }
}
