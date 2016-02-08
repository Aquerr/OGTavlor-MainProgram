using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGTavlor_MainProgram
{
    public class Artworks
    {
        public Artworks()
        {

        }
        public static List<Artwork> Invnetory = new List<Artwork>()
         {
             new Artwork() { ArtworkId = 1,Title = "Uggla", ImagePath = @"images\bild1mini.JPG" },
             new Artwork() { ArtworkId = 2,Title = "Uggla", ImagePath = @"images\bild2mini.JPG" },
             new Artwork() { ArtworkId = 3,Title = "Exempel", ImagePath = @"images\bild3mini.JPG" },
             new Artwork() { ArtworkId = 4,Title = "Exempel", ImagePath = @"images\bild4mini.JPG" },
             new Artwork() { ArtworkId = 5,Title = "Exempel", ImagePath = @"images\bild5mini.JPG" },
             new Artwork() { ArtworkId = 6,Title = "Exempel", ImagePath = @"images\bild6mini.JPG" },
       };
    }


    public class Artwork
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string ImagePath { get; set; }
    }
}