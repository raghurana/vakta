using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Vakta.AzureFunctions
{
    public class ArticleEntity : TableEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string Summary { get; set; }

        public ArticleEntity()
        {}

        public ArticleEntity(Guid id, string title, string url, string summary)
        {
            Id = id;
            Title = title;
            Url = url;
            Summary = summary;
        }
    }
}