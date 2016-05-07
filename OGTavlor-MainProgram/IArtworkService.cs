using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace OGTavlor_MainProgram
{
    internal interface IArtworkService
    {
        Task<List<Artwork>> GetArtworks();
        Task<int> SaveArtwork(Artwork artwork);
        Task DeleteArtwork(int id);
    }
}