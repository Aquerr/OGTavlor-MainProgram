using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace OGTavlor_MainProgram
{
    public interface IArtworkLogic
    {
        Task<Artwork> GetArtworkAsync(string artworkName);
        Task<List<Artwork>> GetArtworksAsync();
        Task<List<string>> GetBlobsAsync();
        Task DeleteArtworkAsync(string artworkName);
        Task SaveArtworkAsync(Artwork artwork);
        Task ReplaceArtwork(string artist, string title, string imagepath, string room, string description, string oldArtworkTitle, string place, int width, int height, bool? signed);
        //  void ClearLists();
    }
}