using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items
{
    public class ItemPickaxe : ItemTool
    {
        private static Tile[] blocksEffectiveAgainst = new[]
        {
            Tile.stoneBrick,
            Tile.stoneSlab,
            Tile.stoneSlabHalf,
            Tile.rock,
            Tile.sandStone,
            Tile.mossStone,
            Tile.ironOre,
            Tile.ironBlock,
            Tile.coalOre,
            Tile.goldBlock,
            Tile.goldOre,
            Tile.oreDiamond,
            Tile.blockDiamond,
            Tile.ice,
            Tile.netherrack,
            Tile.lapisOre,
            Tile.lapisBlock
        };
        public ItemPickaxe(int i1, Tier enumToolMaterial2) : base(i1, 2, enumToolMaterial2, blocksEffectiveAgainst)
        {
        }

        public override bool CanHarvestBlock(Tile block1)
        {
            return block1 == Tile.obsidian ? this.toolMaterial.GetHarvestLevel() == 3 : (block1 != Tile.blockDiamond && block1 != Tile.oreDiamond ? (block1 != Tile.goldBlock && block1 != Tile.goldOre ? (block1 != Tile.ironBlock && block1 != Tile.ironOre ? (block1 != Tile.lapisBlock && block1 != Tile.lapisOre ? (block1 != Tile.oreRedstone && block1 != Tile.oreRedstoneGlowing ? (block1.material == Material.stone ? true : block1.material == Material.metal) : this.toolMaterial.GetHarvestLevel() >= 2) : this.toolMaterial.GetHarvestLevel() >= 1) : this.toolMaterial.GetHarvestLevel() >= 1) : this.toolMaterial.GetHarvestLevel() >= 2) : this.toolMaterial.GetHarvestLevel() >= 2);
        }
    }
}