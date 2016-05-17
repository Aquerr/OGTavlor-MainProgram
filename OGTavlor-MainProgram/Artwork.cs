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
        public string Room { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public string Size { get; set; }
    }
}
