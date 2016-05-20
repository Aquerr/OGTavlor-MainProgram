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

                MessageBox.Show("Konstverket har nu sparats", "Statusmeddelande", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Någonting gick fel när tavlan skulle sparas /r /r" + ex, "Felmeddelande", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Replaces the existing information with new information about the artwork.
        public async Task ReplaceArtwork(string artist, string title, string imagepath, string place, string description, string oldArtworkTitle, string room, int width, int height, bool? signed)
        {
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

                    if (artwork.RowKey == title && artwork.PartitionKey == artist)
                    {
                        // Update Entity
                        artwork.Description = description;
                        artwork.Place = place;
                        artwork.Room = room;
                        artwork.Height = height;
                        artwork.Width = width;
                        artwork.Signed = signed;

                        if (artwork.ImagePath != imagepath)
                        {
                            var blob = await ReplaceBlob(title, imagepath);
                            artwork.ImagePath = imagepath;
                            artwork.Blob = blob;
                        }

                        // Create the Replace TableOperation.
                        var updateOperation = TableOperation.Replace(artwork);

                        // Execute the operation.
                        cloudTable.Execute(updateOperation);
                    }
                    else if (artwork.RowKey != title || artwork.PartitionKey != artist)
                    {
                        await DeleteBlob(artwork.Title);

                        var deleteOperation = TableOperation.Delete(artwork);

                        cloudTable.Execute(deleteOperation);

                        Artwork replacedArtwork = new Artwork()
                        {
                            RowKey = title,
                            PartitionKey = artist,
                            Description = description,
                            Place = place,
                            Room = room,
                            Height = height,
                            Width = width,
                            Signed = signed,
                            ImagePath = imagepath
                        };

                        await SaveBlob(replacedArtwork);

                        var insertTableOperation = TableOperation.Insert(replacedArtwork);

                        cloudTable.Execute(insertTableOperation);
                    }

                    MessageBox.Show("Tavlan har uppdaterats.", "Statusmeddelande", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else

                    MessageBox.Show("Kunde inte hitta tavlan.", "Felmeddelande", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Gets a list with existing artworks.
        public async Task<List<Artwork>> GetArtworks()
        {

            var table = GetCloudTable();

            var entities = table.ExecuteQuery(new TableQuery<Artwork>()).ToList();

            return entities;
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
                await DeleteBlob(deleteArtwork.Title);

                var deleteOperation = TableOperation.Delete(deleteArtwork);

                // Execute the operation.
                cloudTable.Execute(deleteOperation);

                MessageBox.Show("Tavlan har nu tagits bort.", "Statusmeddelande", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            else
            {
                MessageBox.Show("Det gick inte att hitta tavlan.", "Felmeddelande", MessageBoxButton.OK, MessageBoxImage.Error);
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

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(title);

            using (var fileStream = System.IO.File.OpenRead(imagepath))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
                string blob = container.GetBlockBlobReference(title).Uri.AbsoluteUri;
                return blob;
            }
        }

        private async Task DeleteBlob(string title)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            var blobClient = cloudStorageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("ogblob");

            var blockBlob = container.GetBlockBlobReference(title);

            blockBlob.Delete();
        }

        private CloudTable GetCloudTable()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            var cloudTable = cloudTableClient.GetTableReference("ogtavlor");

            return cloudTable;
        }
    }
}
