using SharpCraft.Core.World.GameLevel.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client
{
    public class User
    {
        // Unused classic crap - vlOd
        public static readonly List<Tile> creativeTiles = new List<Tile>();
        public string sessionId;
        public string name;

        public User(string string1, string string2)
        {
            this.name = string1;
            this.sessionId = string2;
        }

        static User() 
        {
            creativeTiles.Add(Tile.rock);
            creativeTiles.Add(Tile.stoneBrick);
            creativeTiles.Add(Tile.redBrick);
            creativeTiles.Add(Tile.dirt);
            creativeTiles.Add(Tile.wood);
            creativeTiles.Add(Tile.treeTrunk);
            creativeTiles.Add(Tile.leaves);
            creativeTiles.Add(Tile.torch);
            creativeTiles.Add(Tile.stoneSlabHalf);
            creativeTiles.Add(Tile.glass);
            creativeTiles.Add(Tile.mossStone);
            creativeTiles.Add(Tile.sapling);
            creativeTiles.Add(Tile.flower);
            creativeTiles.Add(Tile.rose);
            creativeTiles.Add(Tile.mushroom1);
            creativeTiles.Add(Tile.mushroom2);
            creativeTiles.Add(Tile.sand);
            creativeTiles.Add(Tile.gravel);
            creativeTiles.Add(Tile.sponge);
            creativeTiles.Add(Tile.cloth);
            creativeTiles.Add(Tile.coalOre);
            creativeTiles.Add(Tile.ironOre);
            creativeTiles.Add(Tile.goldOre);
            creativeTiles.Add(Tile.ironBlock);
            creativeTiles.Add(Tile.goldBlock);
            creativeTiles.Add(Tile.bookshelf);
            creativeTiles.Add(Tile.tnt);
            creativeTiles.Add(Tile.obsidian);
        }
    }
}
