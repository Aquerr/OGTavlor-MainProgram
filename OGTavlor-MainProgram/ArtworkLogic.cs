﻿using System.Collections.Generic;
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
        private static List<string> _blobList;

        public ArtworkLogic(IArtworkService artworkService)
        {
            _artworkService = artworkService;
            _artworkList = new List<Artwork>();
            _blobList = new List<string>();
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

            var response = await UpdateArtworkListAsync();
            return response;
        }

        private async Task<List<Artwork>> UpdateArtworkListAsync()
        {
            var artworkList = await _artworkService.GetArtworks();

            lock (_artworkList)
            {
                _artworkList.Clear();
                _artworkList.AddRange(artworkList);
            }

            return _artworkList;
        }

        public async Task<Artwork> GetArtworkAsync(string artworkName)
        {
            return (await _artworkService.GetArtworks()).SingleOrDefault(x => x.RowKey == artworkName);
        }

        public async Task<List<string>> GetBlobsAsync()
        {
            lock (_blobList)
            {
                if (_blobList.Any())
                {
                    return _blobList;
                }
            }

            var response = await UpdateBlobListAsync();
            return response;
        }

        private async Task<List<string>> UpdateBlobListAsync()
        {
            var bloblist = await _artworkService.GetBlobs();

            lock (_blobList)
            {
                _blobList.Clear();
                _blobList.AddRange(bloblist);
            }

            return _blobList;
        }

        public async Task SaveArtworkAsync(Artwork artwork)
        {
            await _artworkService.SaveArtwork(artwork);

            await UpdateArtworkListAsync();
        }

        public async Task ReplaceArtwork(string artist, string title, string imagepath, string place, string description, string oldArtworkTitle, string room, string size)
        {
            await _artworkService.ReplaceArtwork(artist, title, imagepath, place, description, oldArtworkTitle, room, size);

            await UpdateArtworkListAsync();
        }

        public async Task DeleteArtworkAsync(string artworkName)
        {
            await _artworkService.DeleteArtwork(artworkName);

            await UpdateArtworkListAsync();
        }
    }
}
