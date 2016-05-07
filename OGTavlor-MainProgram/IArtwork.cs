namespace OGTavlor_MainProgram
{
    public interface IArtwork
    {
        int ArtworkId { get; set; }
        string Title { get; set; }
        string Artist { get; set; }
        string ImagePath { get; set; }
        string Comment { get; set; }
    }
}
