using SharpCraft.Core.NBT;
using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using System;

namespace SharpCraft.Core.World.Entities.Items
{
    public class ItemEntity : Entity
    {
        public ItemInstance item;
        private int popTime;
        public int age = 0;
        public int delayBeforeCanPickup;
        private int health = 5;
        public float mth = (float)(Mth.Random() * Math.PI * 2);
        public ItemEntity(Level world1, double d2, double d4, double d6, ItemInstance itemStack8) : base(world1)
        {
            this.SetSize(0.25F, 0.25F);
            this.yOffset = this.height / 2F;
            this.SetPosition(d2, d4, d6);
            this.item = itemStack8;
            this.yaw = (float)(Mth.Random() * 360);
            this.motionX = ((float)(Mth.Random() * 0.2F - 0.1F));
            this.motionY = 0.2F;
            this.motionZ = ((float)(Mth.Random() * 0.2F - 0.1F));
        }

        protected override bool CanTriggerWalking()
        {
            return false;
        }

        public ItemEntity(Level world1) : base(world1)
        {
            this.SetSize(0.25F, 0.25F);
            this.yOffset = this.height / 2F;
        }

        protected override void EntityInit()
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (this.delayBeforeCanPickup > 0)
            {
                --this.delayBeforeCanPickup;
            }

            this.prevX = this.x;
            this.prevY = this.y;
            this.prevZ = this.z;
            this.motionY -= 0.04F;
            if (this.worldObj.GetMaterial(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z)) == Material.lava)
            {
                this.motionY = 0.2F;
                this.motionX = (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F;
                this.motionZ = (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F;
                this.worldObj.PlaySound(this, "random.fizz", 0.4F, 2F + this.rand.NextFloat() * 0.4F);
            }

            this.PushOutOfBlocks(this.x, (this.boundingBox.y0 + this.boundingBox.y1) / 2, this.z);
            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
            float f1 = 0.98F;
            if (this.onGround)
            {
                f1 = 0.58800006F;
                int i2 = this.worldObj.GetTile(Mth.Floor(this.x), Mth.Floor(this.boundingBox.y0) - 1, Mth.Floor(this.z));
                if (i2 > 0)
                {
                    f1 = Tile.tiles[i2].slipperiness * 0.98F;
                }
            }

            this.motionX *= f1;
            this.motionY *= 0.98F;
            this.motionZ *= f1;
            if (this.onGround)
            {
                this.motionY *= -0.5;
            }

            ++this.popTime;
            ++this.age;
            if (this.age >= 6000)
            {
                this.SetEntityDead();
            }
        }

        public override bool HandleWaterMovement()
        {
            return this.worldObj.CheckAndHandleWater(this.boundingBox, Material.water, this);
        }

        protected override void DealFireDamage(int i1)
        {
            this.AttackEntityFrom((Entity)null, i1);
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            this.SetBeenAttacked();
            this.health -= i2;
            if (this.health <= 0)
            {
                this.SetEntityDead();
            }

            return false;
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetShort("Health", ((byte)this.health));
            nBTTagCompound1.SetShort("Age", (short)this.age);
            nBTTagCompound1.SetCompoundTag("Item", this.item.WriteToNBT(new CompoundTag()));
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            this.health = nBTTagCompound1.GetShort("Health") & 255;
            this.age = nBTTagCompound1.GetShort("Age");
            CompoundTag nBTTagCompound2 = nBTTagCompound1.GetCompoundTag("Item");
            this.item = new ItemInstance(nBTTagCompound2);
        }

        public override void OnCollideWithPlayer(Player entityPlayer1)
        {
            if (!this.worldObj.isRemote)
            {
                int i2 = this.item.stackSize;
                if (this.delayBeforeCanPickup == 0 && entityPlayer1.inventory.AddItem(this.item))
                {
                    if (this.item.itemID == Tile.treeTrunk.id)
                    {
                        entityPlayer1.TriggerAchievement(AchievementList.mineWood);
                    }

                    if (this.item.itemID == Item.leather.id)
                    {
                        entityPlayer1.TriggerAchievement(AchievementList.killCow);
                    }

                    this.worldObj.PlaySound(this, "random.pop", 0.2F, ((this.rand.NextFloat() - this.rand.NextFloat()) * 0.7F + 1F) * 2F);
                    entityPlayer1.OnItemPickup(this, i2);
                    if (this.item.stackSize <= 0)
                    {
                        this.SetEntityDead();
                    }
                }
            }
        }
    }
}