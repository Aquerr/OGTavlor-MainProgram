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
    public class ArtworkService : IArtworkService
    {
        public async Task SaveArtwork(Artwork artwork)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            var cloudTable = cloudTableClient.GetTableReference("ogtavlor");

            var insertTableOperation = TableOperation.Insert(artwork);

            cloudTable.Execute(insertTableOperation);

            MessageBox.Show("Ny tavlan har skapats");
        }

        public async Task ReplaceArtwork(string artist, string title,string imagepath, string oldArtworkTitle)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            var cloudTable = cloudTableClient.GetTableReference("ogtavlor");

            var art = (await GetArtworks()).SingleOrDefault(x => x.Title == oldArtworkTitle);

            if (art != null)
            {
                var retrieveOperation = TableOperation.Retrieve<Artwork>(art.Artist, art.Title);

                var retrievedResult = cloudTable.Execute(retrieveOperation);

                var artwork = (Artwork)retrievedResult.Result;

                if (artwork != null)
                {
                    //TODO: If RowKey or PartitionKey is changed, delete an entity and insert a new one with new properties.

                    // Update Entity
                    artwork.RowKey = title;
                    artwork.ImagePath = "";
                    artwork.PartitionKey = artist;

                    // Create the Replace TableOperation.
                    var updateOperation = TableOperation.InsertOrReplace(artwork);

                    // Execute the operation.
                    cloudTable.Execute(updateOperation);

                    MessageBox.Show("Tavlan har uppdaterats.");
                }
                else

                    MessageBox.Show("Kunde inte återfå tavlan.");
            }
        }

        public async Task<List<Artwork>> GetArtworks()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            var table = cloudTableClient.GetTableReference("ogtavlor");

            var entities = table.ExecuteQuery(new TableQuery<Artwork>()).ToList();

            return entities.ToList();
        }

        public async Task DeleteArtwork(string artworkName)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            var cloudTable = cloudTableClient.GetTableReference("ogtavlor");

            var art = (await GetArtworks()).SingleOrDefault(x => x.Title == artworkName);

            var retrieveTableOperation = TableOperation.Retrieve<Artwork>(art.Artist, art.Title);

            var retrievedTableResult = cloudTable.Execute(retrieveTableOperation);

            var deleteArtwork = (Artwork)retrievedTableResult.Result;

            // Create the Delete TableOperation.
            if (deleteArtwork != null)
            {
                var deleteOperation = TableOperation.Delete(deleteArtwork);

                // Execute the operation.
                cloudTable.Execute(deleteOperation);

                MessageBox.Show("Tavlan har tagits bort.");
            }

            else
            {
                MessageBox.Show("Det gick inte att återfå tavlan.");
            }
        }
    }
}
