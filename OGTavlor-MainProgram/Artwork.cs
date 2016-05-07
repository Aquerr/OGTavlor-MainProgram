using Microsoft.WindowsAzure.Storage.Table;

namespace OGTavlor_MainProgram
{
    public class Artwork : TableEntity
    {
        public Artwork(string artist, string title)
        {
            this.PartitionKey = artist;
            this.RowKey = title;
        }

        public Artwork() { }

        public int ArtworkId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
    }
}
