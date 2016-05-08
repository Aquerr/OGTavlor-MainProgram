using Microsoft.WindowsAzure.Storage.Table;

namespace OGTavlor_MainProgram
{
    public class Artwork : TableEntity
    {
        public Artwork(string artist, string title)
        {
            this.PartitionKey = artist;
            Artist = artist;
            this.RowKey = title;
            Title = title;
        }

        public Artwork() { }

        public int ArtworkId { get; set; }

        public string Title
        {
            get { return this.RowKey; }
            set { this.RowKey = value; }
        }

        public string Artist
        {
            get { return this.PartitionKey; }
            set { this.PartitionKey = value; }
        }
        public string ImagePath { get; set; }
        public string Description { get; set; }
    }
}
