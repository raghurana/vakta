using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Vakta.AzureFunctions
{
    public class ArticleEntity : TableEntity
    {
        public string Url { get; set; }

        public string Summary { get; set; }

        public ArticleEntity()
        {}

        /// <summary>
        /// Creates a new Article Row
        /// </summary>
        /// <param name="id">PartitionKey</param>
        /// <param name="title">RowKey</param>
        /// <param name="url"></param>
        /// <param name="summary"></param>
        public ArticleEntity(Guid id, string title, string url, string summary)
        {
            PartitionKey = id.ToString();
            RowKey = title;
            Url = url;
            Summary = summary;
        }
    }
}