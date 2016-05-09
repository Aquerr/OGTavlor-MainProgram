using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace OGTavlor_MainProgram
{
    public interface IArtworkLogic
    {
        Task<Artwork> GetArtworkAsync(string artworkName);
        Task<List<Artwork>> GetArtworksAsync();
        Task DeleteArtworkAsync(string artworkName);
        Task SaveArtworkAsync(Artwork artwork);
        Task ReplaceArtwork(string artist, string title,string imagepath, string oldArtworkTitle);
        //  void ClearLists();
    }
}