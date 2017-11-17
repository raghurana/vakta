using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Vakta.AzureFunctions
{
    public class ArticleEntityRepo
    {
        private readonly string storageConnectionString;

        public ArticleEntityRepo(string storageConnectionString)
        {
            this.storageConnectionString = storageConnectionString;
        }

        public void SaveArticle(ArticleEntity entity)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "article" table.
            var table = tableClient.GetTableReference("article");

            table.CreateIfNotExists();

            // Create the TableOperation object that inserts the customer entity.
            var insertOperation = TableOperation.Insert(entity);

            // Execute the insert operation.
            table.Execute(insertOperation);           
        }
    }
}