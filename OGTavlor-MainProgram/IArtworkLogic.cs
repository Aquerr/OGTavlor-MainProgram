using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace OGTavlor_MainProgram
{
    public interface IArtworkLogic
    {
        Task<Artwork> GetArtworkAsync(int artworkId);
        Task<List<Artwork>> GetArtworksAsync();
        Task DeleteArtworkAsync(int artworkid);
      //  void ClearLists();
    }
}