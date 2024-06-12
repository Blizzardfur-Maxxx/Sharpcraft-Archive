using SharpCraft.Core.World.GameLevel.Biomes;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;

namespace SharpCraft.Core.World.GameLevel
{
    public interface ILevelSource
    {
        int GetTile(int i1, int i2, int i3);
        TileEntity GetTileEntity(int i1, int i2, int i3);
        float GetBrightness(int i1, int i2, int i3, int i4);
        float GetBrightness(int i1, int i2, int i3);
        int GetData(int i1, int i2, int i3);
        Material GetMaterial(int i1, int i2, int i3);
        bool IsSolidRenderTile(int i1, int i2, int i3);
        bool IsSolidBlockingTile(int i1, int i2, int i3);
        BiomeSource GetBiomeSource();
    }
}