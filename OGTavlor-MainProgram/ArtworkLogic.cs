using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Documents;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace OGTavlor_MainProgram
{
    internal class ArtworkLogic : IArtworkLogic
    {
        private readonly IArtworkService _artworkService;
        private static List<Artwork> _artworkList;

        public ArtworkLogic() 
        {
            _artworkList = new List<Artwork>();
        }

        public async Task<List<Artwork>> GetArtworksAsync()
        {
            lock (_artworkList)
            {
                if (_artworkList.Any())
                {
                    return _artworkList;
                }
            }

            var response = await UpdateArtworkListAsyn();
            return response;
        }

        private async Task<List<Artwork>> UpdateArtworkListAsyn()
        {
            var artworkList = await _artworkService.GetArtworks();

            lock (_artworkList)
            {
                _artworkList.Clear();
                _artworkList.AddRange(artworkList);
            }

            return _artworkList;
        }

        public async Task<Artwork> GetArtworkAsync(int artworkId)
        {
            return (await _artworkService.GetArtworks()).SingleOrDefault(x => x.ArtworkId == artworkId);
        }

        public async Task<int?> SaveArtworkAsync(Artwork artwork)
        {
            var isNew = artwork.ArtworkId == null;

            var artworkId = await _artworkService.SaveArtwork(artwork);

            await UpdateArtworkListAsyn();

            if (artwork.ArtworkId != null) artwork.ArtworkId = artworkId;

            if (isNew)
            {
                
            }
            else
            {
                
            }

            return artworkId;
        }

        public async Task DeleteArtworkAsync(int artworkId)
        {
            await _artworkService.DeleteArtwork(artworkId);
        }
    }
}
