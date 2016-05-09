using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace OGTavlor_MainProgram
{
    internal interface IArtworkService
    {
        Task<List<Artwork>> GetArtworks();
        Task SaveArtwork(Artwork artwork);
        Task DeleteArtwork(Artwork artwork);
        Task ReplaceArtwork(string artist, string title,string imagepath, string oldArtworkTitle);
    }
}