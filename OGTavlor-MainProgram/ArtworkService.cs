using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAzure.Storage.Blob;

namespace OGTavlor_MainProgram
{
    internal class ArtworkService : IArtworkService
    {
        public async Task SaveArtwork(Artwork artwork)
        {
            try
             {
                await SaveBlob(artwork);

                var cloudTable = GetCloudTable();

                var insertTableOperation = TableOperation.Insert(artwork);

                cloudTable.Execute(insertTableOperation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Någonting gick fel när tavlan skulle sparas");
                MessageBox.Show(ex.ToString());
            }
        }

        //Replaces the existing information with new information about the artwork.
        public async Task ReplaceArtwork(string artist, string title, string imagepath, string place, string description, string oldArtworkTitle, string room, int width, int height, bool? signed)
        {

            var blob = await ReplaceBlob(title,imagepath);

            var cloudTable = GetCloudTable();

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
                    artwork.ImagePath = imagepath;
                    artwork.PartitionKey = artist;
                    artwork.Description = description;
                    artwork.Place = place;
                    artwork.Room = room;
                    artwork.Height = height;
                    artwork.Width = width;
                    artwork.Signed = signed;
                    artwork.Blob = blob;

                    // Create the Replace TableOperation.
                    var updateOperation = TableOperation.InsertOrReplace(artwork);

                    // Execute the operation.
                    cloudTable.Execute(updateOperation);

                    MessageBox.Show("Tavlan har uppdaterats.");
                }
                else

                    MessageBox.Show("Kunde inte hitta tavlan.");
            }
        }

        //Gets a list with existing artworks.
        public async Task<List<Artwork>> GetArtworks()
        {

            var table = GetCloudTable();

            var entities = table.ExecuteQuery(new TableQuery<Artwork>()).ToList();

            return entities;
        }

        public async Task<List<string>> GetBlobs()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            var blobClient = cloudStorageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("ogblob");

            List<string> blobls = new List<string>();

            foreach (var blobItem in container.ListBlobs())
            {
                blobls.Add(blobItem.Uri.ToString());

            }

            return blobls;
        }

        //Deletes artwork from Database.
        public async Task DeleteArtwork(string artworkName)
        {

            var cloudTable = GetCloudTable();

            var art = (await GetArtworks()).SingleOrDefault(x => x.RowKey == artworkName);

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

        private async Task SaveBlob(Artwork artwork)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            var blobClient = cloudStorageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("ogblob");

            container.CreateIfNotExists();

            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(artwork.Title);

            using (var fileStream = System.IO.File.OpenRead(artwork.ImagePath))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
                artwork.Blob = container.GetBlockBlobReference(artwork.Title).Uri.AbsoluteUri;
            }
        }

       private async Task<string> ReplaceBlob(string title, string imagepath)
       {
            var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            var blobClient = cloudStorageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("ogblob");

            container.CreateIfNotExists();

            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(title);

            using (var fileStream = System.IO.File.OpenRead(imagepath))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
                string blob = container.GetBlockBlobReference(title).Uri.AbsoluteUri;
                return blob;
            }


       }

        private CloudTable GetCloudTable()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings ["StorageConnectionString"]);

            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            var cloudTable = cloudTableClient.GetTableReference("ogtavlor");

            return cloudTable;
        }
    }
}
