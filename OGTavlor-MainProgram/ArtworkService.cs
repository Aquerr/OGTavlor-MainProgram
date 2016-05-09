using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OGTavlor_MainProgram
{
    class ArtworkService : IArtworkService
    {
        public async Task<int> SaveArtwork(Artwork artwork)
        {
            return 1;
        }

        public async Task<List<Artwork>> GetArtworks()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "ogtavlor" table.
            CloudTable table = tableClient.GetTableReference("ogtavlor");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<Artwork> query = new TableQuery<Artwork>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Brutus"));

            // Print the fields for each customer.

            var data = table.ExecuteQuery(query);



            return data.ToList();
        }

        public async Task DeleteArtwork(int artworkId)
        {

        }
    }
}
