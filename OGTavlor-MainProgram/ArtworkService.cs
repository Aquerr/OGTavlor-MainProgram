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
        public async Task SaveArtwork(Artwork artwork)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("ogtavlor");

            TableOperation insertOperation = TableOperation.Insert(artwork);

            table.Execute(insertOperation);
        }

        public async Task ReplaceArtwork(string artist, string title,string imagepath, string oldArtworkTitle)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("ogtavlor");

            var art = (await GetArtworks()).SingleOrDefault(x => x.Title == oldArtworkTitle);

            TableOperation retrieveOperation = TableOperation.Retrieve<Artwork>(art.Artist, art.Title);

            TableResult retrievedResult = table.Execute(retrieveOperation);

            Artwork artwork = (Artwork)retrievedResult.Result;

            if (artwork != null)
            {
                //TODO: If RowKey or PartitionKey is changed, delete an entity and insert a new one with new properties.

                // Update Entity
                artwork.RowKey = title;
                artwork.ImagePath = "";
                artwork.PartitionKey = artist;

                // Create the Replace TableOperation.
                TableOperation updateOperation = TableOperation.InsertOrReplace(artwork);

                // Execute the operation.
                table.Execute(updateOperation);

                MessageBox.Show("Tavlan har uppdaterats.");
            }

            else
                MessageBox.Show("Kunde inte återfå tavlan.");
        }

        public async Task<List<Artwork>> GetArtworks()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("ogtavlor");

           // TableQuery<Artwork> query = new TableQuery<Artwork>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Brutus"));

            var entities = table.ExecuteQuery(new TableQuery<Artwork>()).ToList();

            return entities.ToList();
        }

        public async Task DeleteArtwork(string artworkName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("ogtavlor");

            var art = (await GetArtworks()).SingleOrDefault(x => x.Title == artworkName);

            TableOperation retrieveOperation = TableOperation.Retrieve<Artwork>(art.Artist, art.Title);

            TableResult retrievedResult = table.Execute(retrieveOperation);

            Artwork deleteArtwork = (Artwork)retrievedResult.Result;

            // Create the Delete TableOperation.
            if (deleteArtwork != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteArtwork);

                // Execute the operation.
                table.Execute(deleteOperation);

                MessageBox.Show("Tavlan har tagits bort.");
            }

            else
            {
                MessageBox.Show("Det gick inte att återfå tavlan.");
            }
        }
    }
}
