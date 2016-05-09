using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace OGTavlor_MainProgram
{
    public interface IArtworkLogic
    {
        Task<Artwork> GetArtworkAsync(string artworkName);
        Task<List<Artwork>> GetArtworksAsync();
        Task DeleteArtworkAsync(int artworkid);
      //  void ClearLists();
    }
}