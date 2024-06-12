using SharpCraft.Core.NBT;
using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Monsters;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Inventory;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Entities.Players
{
    public abstract class Player : Mob
    {
        public Inventory inventory;
        public AbstractContainerMenu inventorySlots;
        public AbstractContainerMenu curCraftingInventory;
        public byte adminType = 0;
        public int score = 0;
        public float field_775;
        public float field_774;
        public bool isSwinging = false;
        public int swingProgressInt = 0;
        public string username;
        public int dimension;
        public string playerCloakUrl;
        public double field_773;
        public double field_772;
        public double field_771;
        public double field_770;
        public double field_769;
        public double field_768;
        protected bool sleeping;
        public Pos playerLocation = default;
        private int sleepTimer;
        public float field_767;
        public float field_766;
        public float field_765;
        private Pos spawnPos = default;
        private Pos minecartStartPos = default;
        public int timeUntilPortal = 20;
        protected bool inPortal = false;
        public float timeInPortal;
        public float prevTimeInPortal;
        private int damageRemainder = 0;
        public FishingHook fishEntity = null;
        public enum BedSleepingProblem
        {
            OK,
            NOT_POSSIBLE_HERE,
            NOT_POSSIBLE_NOW,
            TOO_FAR_AWAY,
            OTHER_PROBLEM
        }

        public Player(Level world1) : base(world1)
        {
            this.inventory = new Inventory(this);
            this.inventorySlots = new InventoryMenu(this.inventory, !world1.isRemote);
            this.curCraftingInventory = this.inventorySlots;
            this.yOffset = 1.62F;
            Pos chunkCoordinates2 = world1.GetSpawnPos();
            this.SetLocationAndAngles(chunkCoordinates2.x + 0.5, chunkCoordinates2.y + 1, chunkCoordinates2.z + 0.5, 0F, 0F);
            this.health = 20;
            this.entityType = "humanoid";
            this.field_B = 180F;
            this.fireResistance = 20;
            this.texture = "/mob/char.png";
        }

        protected override void EntityInit()
        {
            base.EntityInit();
            this.dataWatcher.AddObject(16, (sbyte)0);
        }

        public override void OnUpdate()
        {
            if (this.IsSleeping())
            {
                ++this.sleepTimer;
                if (this.sleepTimer > 100)
                {
                    this.sleepTimer = 100;
                }

                if (!this.worldObj.isRemote)
                {
                    if (!this.IsInBed())
                    {
                        this.WakeUpPlayer(true, true, false);
                    }
                    else if (this.worldObj.IsDaytime())
                    {
                        this.WakeUpPlayer(false, true, true);
                    }
                }
            }
            else if (this.sleepTimer > 0)
            {
                ++this.sleepTimer;
                if (this.sleepTimer >= 110)
                {
                    this.sleepTimer = 0;
                }
            }

            base.OnUpdate();
            if (!this.worldObj.isRemote && this.curCraftingInventory != null && !this.curCraftingInventory.StillValid(this))
            {
                this.CloseScreen();
                this.curCraftingInventory = this.inventorySlots;
            }

            this.field_773 = this.field_770;
            this.field_772 = this.field_769;
            this.field_771 = this.field_768;
            double d1 = this.x - this.field_770;
            double d3 = this.y - this.field_769;
            double d5 = this.z - this.field_768;
            double d7 = 10;
            if (d1 > d7)
            {
                this.field_773 = this.field_770 = this.x;
            }

            if (d5 > d7)
            {
                this.field_771 = this.field_768 = this.z;
            }

            if (d3 > d7)
            {
                this.field_772 = this.field_769 = this.y;
            }

            if (d1 < -d7)
            {
                this.field_773 = this.field_770 = this.x;
            }

            if (d5 < -d7)
            {
                this.field_771 = this.field_768 = this.z;
            }

            if (d3 < -d7)
            {
                this.field_772 = this.field_769 = this.y;
            }

            this.field_770 += d1 * 0.25;
            this.field_768 += d5 * 0.25;
            this.field_769 += d3 * 0.25;
            this.AddStat(StatList.minPlayed, 1);
            if (this.ridingEntity == null)
            {
                this.minecartStartPos = default;
            }
        }

        protected override bool IsMovementBlocked()
        {
            return this.health <= 0 || this.IsSleeping();
        }

        protected virtual void CloseScreen()
        {
            this.curCraftingInventory = this.inventorySlots;
        }

        public override void UpdateCloak()
        {
            this.playerCloakUrl = SharedConstants.CLOAK_URL + this.username + ".png";
            this.cloakUrl = this.playerCloakUrl;
        }

        public override void UpdateRidden()
        {
            double d1 = this.x;
            double d3 = this.y;
            double d5 = this.z;
            base.UpdateRidden();
            this.field_775 = this.field_774;
            this.field_774 = 0F;
            this.AddMountedMovementStat(this.x - d1, this.y - d3, this.z - d5);
        }

        public override void PreparePlayerToSpawn()
        {
            this.yOffset = 1.62F;
            this.SetSize(0.6F, 1.8F);
            base.PreparePlayerToSpawn();
            this.health = 20;
            this.deathTime = 0;
        }

        protected override void UpdatePlayerActionState()
        {
            if (this.isSwinging)
            {
                ++this.swingProgressInt;
                if (this.swingProgressInt >= 8)
                {
                    this.swingProgressInt = 0;
                    this.isSwinging = false;
                }
            }
            else
            {
                this.swingProgressInt = 0;
            }

            this.swingProgress = this.swingProgressInt / 8F;
        }

        public override void OnLivingUpdate()
        {
            if (this.worldObj.difficultySetting == 0 && this.health < 20 && this.ticksExisted % 20 * 12 == 0)
            {
                this.Heal(1);
            }

            this.inventory.DecrementAnimations();
            this.field_775 = this.field_774;
            base.OnLivingUpdate();
            float f1 = Mth.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
            float f2 = (float)Math.Atan(-this.motionY * 0.2F) * 15F;
            if (f1 > 0.1F)
            {
                f1 = 0.1F;
            }

            if (!this.onGround || this.health <= 0)
            {
                f1 = 0F;
            }

            if (this.onGround || this.health <= 0)
            {
                f2 = 0F;
            }

            this.field_774 += (f1 - this.field_774) * 0.4F;
            this.field_R += (f2 - this.field_R) * 0.8F;
            if (this.health > 0)
            {
                IList<Entity> list3 = this.worldObj.GetEntities(this, this.boundingBox.Expand(1, 0, 1));
                if (list3 != null)
                {
                    for (int i4 = 0; i4 < list3.Count; ++i4)
                    {
                        Entity entity5 = list3[i4];
                        if (!entity5.isDead)
                        {
                            this.CollideWithPlayer(entity5);
                        }
                    }
                }
            }
        }

        private void CollideWithPlayer(Entity entity1)
        {
            entity1.OnCollideWithPlayer(this);
        }

        public virtual int GetScore()
        {
            return this.score;
        }

        public override void OnDeath(Entity entity1)
        {
            base.OnDeath(entity1);
            this.SetSize(0.2F, 0.2F);
            this.SetPosition(this.x, this.y, this.z);
            this.motionY = 0.1F;
            if (this.username.Equals("Notch"))
            {
                this.DropPlayerItemWithRandomChoice(new ItemInstance(Item.appleRed, 1), true);
            }

            this.inventory.DropAllItems();
            if (entity1 != null)
            {
                this.motionX = -Mth.Cos((this.attackedAtYaw + this.yaw) * Mth.PI / 180F) * 0.1F;
                this.motionZ = -Mth.Sin((this.attackedAtYaw + this.yaw) * Mth.PI / 180F) * 0.1F;
            }
            else
            {
                this.motionX = this.motionZ = 0;
            }

            this.yOffset = 0.1F;
            this.AddStat(StatList.deaths, 1);
        }

        public override void AddToPlayerScore(Entity entity1, int i2)
        {
            this.score += i2;
            if (entity1 is Player)
            {
                this.AddStat(StatList.playerKills, 1);
            }
            else
            {
                this.AddStat(StatList.mobKills, 1);
            }
        }

        public virtual void DropCurrentItem()
        {
            this.DropPlayerItemWithRandomChoice(this.inventory.RemoveItem(this.inventory.currentItem, 1), false);
        }

        public virtual void DropPlayerItem(ItemInstance itemStack1)
        {
            this.DropPlayerItemWithRandomChoice(itemStack1, false);
        }

        public virtual void DropPlayerItemWithRandomChoice(ItemInstance itemStack1, bool z2)
        {
            if (itemStack1 != null)
            {
                ItemEntity entityItem3 = new ItemEntity(this.worldObj, this.x, this.y - 0.3F + this.GetEyeHeight(), this.z, itemStack1);
                entityItem3.delayBeforeCanPickup = 40;
                float f4 = 0.1F;
                float f5;
                if (z2)
                {
                    f5 = this.rand.NextFloat() * 0.5F;
                    float f6 = this.rand.NextFloat() * Mth.PI * 2F;
                    entityItem3.motionX = -Mth.Sin(f6) * f5;
                    entityItem3.motionZ = Mth.Cos(f6) * f5;
                    entityItem3.motionY = 0.2F;
                }
                else
                {
                    f4 = 0.3F;
                    entityItem3.motionX = -Mth.Sin(this.yaw / 180F * Mth.PI) * Mth.Cos(this.pitch / 180F * Mth.PI) * f4;
                    entityItem3.motionZ = Mth.Cos(this.yaw / 180F * Mth.PI) * Mth.Cos(this.pitch / 180F * Mth.PI) * f4;
                    entityItem3.motionY = -Mth.Sin(this.pitch / 180F * Mth.PI) * f4 + 0.1F;
                    f4 = 0.02F;
                    f5 = this.rand.NextFloat() * Mth.PI * 2F;
                    f4 *= this.rand.NextFloat();
                    entityItem3.motionX += Math.Cos(f5) * f4;
                    entityItem3.motionY += (this.rand.NextFloat() - this.rand.NextFloat()) * 0.1F;
                    entityItem3.motionZ += Math.Sin(f5) * f4;
                }

                this.JoinEntityItemWithWorld(entityItem3);
                this.AddStat(StatList.drops, 1);
            }
        }

        protected virtual void JoinEntityItemWithWorld(ItemEntity entityItem1)
        {
            this.worldObj.AddEntity(entityItem1);
        }

        public virtual float GetCurrentPlayerStrVsBlock(Tile block1)
        {
            float f2 = this.inventory.GetStrVsBlock(block1);
            if (this.IsInsideOfMaterial(Material.water))
            {
                f2 /= 5F;
            }

            if (!this.onGround)
            {
                f2 /= 5F;
            }

            return f2;
        }

        public virtual bool CanHarvestBlock(Tile block1)
        {
            return this.inventory.CanHarvestBlock(block1);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
            ListTag<CompoundTag> nBTTagList2 = nBTTagCompound1.GetTagList<CompoundTag>("Inventory");
            this.inventory.ReadFromNBT(nBTTagList2);
            this.dimension = nBTTagCompound1.GetInteger("Dimension");
            this.sleeping = nBTTagCompound1.GetBoolean("Sleeping");
            this.sleepTimer = nBTTagCompound1.GetShort("SleepTimer");
            if (this.sleeping)
            {
                this.playerLocation = new Pos(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z));
                this.WakeUpPlayer(true, true, false);
            }

            if (nBTTagCompound1.HasKey("SpawnX") && nBTTagCompound1.HasKey("SpawnY") && nBTTagCompound1.HasKey("SpawnZ"))
            {
                this.spawnPos = new Pos(nBTTagCompound1.GetInteger("SpawnX"), nBTTagCompound1.GetInteger("SpawnY"), nBTTagCompound1.GetInteger("SpawnZ"));
            }
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
            nBTTagCompound1.SetTag("Inventory", this.inventory.WriteToNBT(new ListTag<CompoundTag>()));
            nBTTagCompound1.SetInteger("Dimension", this.dimension);
            nBTTagCompound1.SetBoolean("Sleeping", this.sleeping);
            nBTTagCompound1.SetShort("SleepTimer", (short)this.sleepTimer);
            if (this.spawnPos != default)
            {
                nBTTagCompound1.SetInteger("SpawnX", this.spawnPos.x);
                nBTTagCompound1.SetInteger("SpawnY", this.spawnPos.y);
                nBTTagCompound1.SetInteger("SpawnZ", this.spawnPos.z);
            }
        }

        public virtual void DisplayGUIChest(IContainer iInventory1)
        {
        }

        public virtual void DisplayWorkbenchGUI(int i1, int i2, int i3)
        {
        }

        public virtual void OnItemPickup(Entity entity1, int i2)
        {
        }

        public override float GetEyeHeight()
        {
            return 0.12F;
        }

        protected virtual void ResetHeight()
        {
            this.yOffset = 1.62F;
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            this.age = 0;
            if (this.health <= 0)
            {
                return false;
            }
            else
            {
                if (this.IsSleeping() && !this.worldObj.isRemote)
                {
                    this.WakeUpPlayer(true, true, false);
                }

                if (entity1 is Monster || entity1 is Arrow)
                {
                    if (this.worldObj.difficultySetting == 0)
                    {
                        i2 = 0;
                    }

                    if (this.worldObj.difficultySetting == 1)
                    {
                        i2 = i2 / 3 + 1;
                    }

                    if (this.worldObj.difficultySetting == 3)
                    {
                        i2 = i2 * 3 / 2;
                    }
                }

                if (i2 == 0)
                {
                    return false;
                }
                else
                {
                    object object3 = entity1;
                    if (entity1 is Arrow && ((Arrow)entity1).owner != null)
                    {
                        object3 = ((Arrow)entity1).owner;
                    }

                    if (object3 is Mob)
                    {
                        this.AlertWolves((Mob)object3, false);
                    }

                    this.AddStat(StatList.damageTaken, i2);
                    return base.AttackEntityFrom(entity1, i2);
                }
            }
        }

        protected virtual bool IsPVPEnabled()
        {
            return false;
        }

        protected virtual void AlertWolves(Mob entityLiving1, bool z2)
        {
            if (!(entityLiving1 is Creeper) && !(entityLiving1 is Ghast))
            {
                if (entityLiving1 is Wolf)
                {
                    Wolf entityWolf3 = (Wolf)entityLiving1;
                    if (entityWolf3.IsWolfTamed() && this.username.Equals(entityWolf3.GetOwner()))
                    {
                        return;
                    }
                }

                if (!(entityLiving1 is Player) || this.IsPVPEnabled())
                {
                    IList<Wolf> list7 = this.worldObj.GetEntitiesOfClass<Wolf>(typeof(Wolf), AABB.Of(this.x, this.y, this.z, this.x + 1, this.y + 1, this.z + 1).Expand(16, 4, 16));
                    IEnumerator<Wolf> iterator4 = list7.GetEnumerator();
                    while (true)
                    {
                        Wolf entityWolf6;
                        do
                        {
                            do
                            {
                                do
                                {
                                    do
                                    {
                                        if (!iterator4.MoveNext())
                                        {
                                            return;
                                        }

                                        entityWolf6 = iterator4.Current;
                                    }
                                    while (!entityWolf6.IsWolfTamed());
                                }
                                while (entityWolf6.GetTarget() != null);
                            }
                            while (!this.username.Equals(entityWolf6.GetOwner()));
                        }
                        while (z2 && entityWolf6.IsSitting());
                        entityWolf6.SetSitting(false);
                        entityWolf6.SetTarget(entityLiving1);
                    }
                }
            }
        }

        protected override void DamageEntity(int i1)
        {
            int i2 = 25 - this.inventory.GetTotalArmorValue();
            int i3 = i1 * i2 + this.damageRemainder;
            this.inventory.DamageArmor(i1);
            i1 = i3 / 25;
            this.damageRemainder = i3 % 25;
            base.DamageEntity(i1);
        }

        public virtual void DisplayGUIFurnace(TileEntityFurnace tileEntityFurnace1)
        {
        }

        public virtual void DisplayGUIDispenser(TileEntityDispenser tileEntityDispenser1)
        {
        }

        public virtual void DisplayGUIEditSign(TileEntitySign tileEntitySign1)
        {
        }

        public virtual void UseCurrentItemOnEntity(Entity entity1)
        {
            if (!entity1.Interact(this))
            {
                ItemInstance itemStack2 = this.GetCurrentEquippedItem();
                if (itemStack2 != null && entity1 is Mob)
                {
                    itemStack2.UseItemOnEntity((Mob)entity1);
                    if (itemStack2.stackSize <= 0)
                    {
                        itemStack2.Func_1097(this);
                        this.DestroyCurrentEquippedItem();
                    }
                }
            }
        }

        public virtual ItemInstance GetCurrentEquippedItem()
        {
            return this.inventory.GetCurrentItem();
        }

        public virtual void DestroyCurrentEquippedItem()
        {
            this.inventory.SetItem(this.inventory.currentItem, (ItemInstance)null);
        }

        public override double GetYOffset()
        {
            return this.yOffset - 0.5F;
        }

        public virtual void SwingItem()
        {
            this.swingProgressInt = -1;
            this.isSwinging = true;
        }

        public virtual void AttackTargetEntityWithCurrentItem(Entity entity1)
        {
            int i2 = this.inventory.GetDamageVsEntity(entity1);
            if (i2 > 0)
            {
                if (this.motionY < 0)
                {
                    ++i2;
                }

                entity1.AttackEntityFrom(this, i2);
                ItemInstance itemStack3 = this.GetCurrentEquippedItem();
                if (itemStack3 != null && entity1 is Mob)
                {
                    itemStack3.HitEntity((Mob)entity1, this);
                    if (itemStack3.stackSize <= 0)
                    {
                        itemStack3.Func_1097(this);
                        this.DestroyCurrentEquippedItem();
                    }
                }

                if (entity1 is Mob)
                {
                    if (entity1.IsEntityAlive())
                    {
                        this.AlertWolves((Mob)entity1, true);
                    }

                    this.AddStat(StatList.damageDealt, i2);
                }
            }
        }

        public virtual void RespawnPlayer()
        {
        }

        public abstract void Fun_o();
        public virtual void OnItemStackChanged(ItemInstance itemStack1)
        {
        }

        public override void SetEntityDead()
        {
            base.SetEntityDead();
            this.inventorySlots.Removed(this);
            if (this.curCraftingInventory != null)
            {
                this.curCraftingInventory.Removed(this);
            }
        }

        public override bool IsEntityInsideOpaqueBlock()
        {
            return !this.sleeping && base.IsEntityInsideOpaqueBlock();
        }

        public virtual BedSleepingProblem SleepInBedAt(int i1, int i2, int i3)
        {
            if (!this.worldObj.isRemote)
            {
                if (!this.IsSleeping() && this.IsEntityAlive())
                {
                    if (this.worldObj.dimension.isNether)
                    {
                        return BedSleepingProblem.NOT_POSSIBLE_HERE;
                    }

                    if (this.worldObj.IsDaytime())
                    {
                        return BedSleepingProblem.NOT_POSSIBLE_NOW;
                    }

                    if (Math.Abs(this.x - i1) <= 3 && Math.Abs(this.y - i2) <= 2 && Math.Abs(this.z - i3) <= 3)
                    {
                        goto passedChecks;
                    }

                    return BedSleepingProblem.TOO_FAR_AWAY;
                }

                return BedSleepingProblem.OTHER_PROBLEM;
            }
            passedChecks:

            this.SetSize(0.2F, 0.2F);
            this.yOffset = 0.2F;
            if (this.worldObj.HasChunkAt(i1, i2, i3))
            {
                int i4 = this.worldObj.GetData(i1, i2, i3);
                int i5 = BedTile.GetDirectionFromMetadata(i4);
                float f6 = 0.5F;
                float f7 = 0.5F;
                switch (i5)
                {
                    case 0:
                        f7 = 0.9F;
                        break;
                    case 1:
                        f6 = 0.1F;
                        break;
                    case 2:
                        f7 = 0.1F;
                        break;
                    case 3:
                        f6 = 0.9F;
                        break;
                }

                this.Fun_22052(i5);
                this.SetPosition(i1 + f6, i2 + 0.9375F, i3 + f7);
            }
            else
            {
                this.SetPosition(i1 + 0.5F, i2 + 0.9375F, i3 + 0.5F);
            }

            this.sleeping = true;
            this.sleepTimer = 0;
            this.playerLocation = new Pos(i1, i2, i3);
            this.motionX = this.motionZ = this.motionY = 0;
            if (!this.worldObj.isRemote)
            {
                this.worldObj.UpdateAllPlayersSleepingFlag();
            }

            return BedSleepingProblem.OK;
        }

        private void Fun_22052(int i1)
        {
            this.field_767 = 0F;
            this.field_765 = 0F;
            switch (i1)
            {
                case 0:
                    this.field_765 = -1.8F;
                    break;
                case 1:
                    this.field_767 = 1.8F;
                    break;
                case 2:
                    this.field_765 = 1.8F;
                    break;
                case 3:
                    this.field_767 = -1.8F;
                    break;
            }
        }

        public virtual void WakeUpPlayer(bool z1, bool z2, bool z3)
        {
            this.SetSize(0.6F, 1.8F);
            this.ResetHeight();
            Pos chunkCoordinates4 = this.playerLocation;
            Pos chunkCoordinates5 = this.playerLocation;
            if (chunkCoordinates4 != default && this.worldObj.GetTile(chunkCoordinates4.x, chunkCoordinates4.y, chunkCoordinates4.z) == Tile.bed.id)
            {
                BedTile.SetBedOccupied(this.worldObj, chunkCoordinates4.x, chunkCoordinates4.y, chunkCoordinates4.z, false);
                chunkCoordinates5 = BedTile.GetNearestEmptyPos(this.worldObj, chunkCoordinates4.x, chunkCoordinates4.y, chunkCoordinates4.z, 0);
                if (chunkCoordinates5 == default)
                {
                    chunkCoordinates5 = new Pos(chunkCoordinates4.x, chunkCoordinates4.y + 1, chunkCoordinates4.z);
                }

                this.SetPosition(chunkCoordinates5.x + 0.5F, chunkCoordinates5.y + this.yOffset + 0.1F, chunkCoordinates5.z + 0.5F);
            }

            this.sleeping = false;
            if (!this.worldObj.isRemote && z2)
            {
                this.worldObj.UpdateAllPlayersSleepingFlag();
            }

            if (z1)
            {
                this.sleepTimer = 0;
            }
            else
            {
                this.sleepTimer = 100;
            }

            if (z3)
            {
                this.SetSpawn(this.playerLocation);
            }
        }

        private bool IsInBed()
        {
            return this.worldObj.GetTile(this.playerLocation.x, this.playerLocation.y, this.playerLocation.z) == Tile.bed.id;
        }

        public static Pos GetNearestBedSpawnPos(Level world0, Pos chunkCoordinates1)
        {
            IChunkSource iChunkProvider2 = world0.GetChunkSource();
            iChunkProvider2.Create(chunkCoordinates1.x - 3 >> 4, chunkCoordinates1.z - 3 >> 4);
            iChunkProvider2.Create(chunkCoordinates1.x + 3 >> 4, chunkCoordinates1.z - 3 >> 4);
            iChunkProvider2.Create(chunkCoordinates1.x - 3 >> 4, chunkCoordinates1.z + 3 >> 4);
            iChunkProvider2.Create(chunkCoordinates1.x + 3 >> 4, chunkCoordinates1.z + 3 >> 4);
            if (world0.GetTile(chunkCoordinates1.x, chunkCoordinates1.y, chunkCoordinates1.z) != Tile.bed.id)
            {
                return default;
            }
            else
            {
                Pos chunkCoordinates3 = BedTile.GetNearestEmptyPos(world0, chunkCoordinates1.x, chunkCoordinates1.y, chunkCoordinates1.z, 0);
                return chunkCoordinates3;
            }
        }

        public virtual float GetBedOrientationInDegrees()
        {
            if (this.playerLocation != default)
            {
                int i1 = this.worldObj.GetData(this.playerLocation.x, this.playerLocation.y, this.playerLocation.z);
                int i2 = BedTile.GetDirectionFromMetadata(i1);
                switch (i2)
                {
                    case 0:
                        return 90F;
                    case 1:
                        return 0F;
                    case 2:
                        return 270F;
                    case 3:
                        return 180F;
                }
            }

            return 0F;
        }

        public override bool IsSleeping()
        {
            return this.sleeping;
        }

        public virtual bool IsPlayerFullyAsleep()
        {
            return this.sleeping && this.sleepTimer >= 100;
        }

        public virtual int Func_22060_M()
        {
            return this.sleepTimer;
        }

        public virtual void AddChatMessage(string string1)
        {
        }

        public virtual Pos GetSpawnPos()
        {
            return this.spawnPos;
        }

        public virtual void SetSpawn(Pos chunkCoordinates1)
        {
            if (chunkCoordinates1 != default)
            {
                this.spawnPos = new Pos(chunkCoordinates1);
            }
            else
            {
                this.spawnPos = default;
            }
        }

        public virtual void TriggerAchievement(Stat statBase1)
        {
            this.AddStat(statBase1, 1);
        }

        public virtual void AddStat(Stat statBase1, int i2)
        {
        }

        protected override void Jump()
        {
            base.Jump();
            this.AddStat(StatList.jumps, 1);
        }

        public override void MoveEntityWithHeading(float f1, float f2)
        {
            double d3 = this.x;
            double d5 = this.y;
            double d7 = this.z;
            base.MoveEntityWithHeading(f1, f2);
            this.AddMovementStat(this.x - d3, this.y - d5, this.z - d7);
        }

        private void AddMovementStat(double d1, double d3, double d5)
        {
            if (this.ridingEntity == null)
            {
                int i7;
                if (this.IsInsideOfMaterial(Material.water))
                {
                    i7 = (int)Math.Round(Mth.Sqrt(d1 * d1 + d3 * d3 + d5 * d5) * 100F);
                    if (i7 > 0)
                    {
                        this.AddStat(StatList.distDove, i7);
                    }
                }
                else if (this.IsInWater())
                {
                    i7 = (int)Math.Round(Mth.Sqrt(d1 * d1 + d5 * d5) * 100F);
                    if (i7 > 0)
                    {
                        this.AddStat(StatList.distSwum, i7);
                    }
                }
                else if (this.IsOnLadder())
                {
                    if (d3 > 0)
                    {
                        this.AddStat(StatList.distClimbed, (int)Math.Round(d3 * 100));
                    }
                }
                else if (this.onGround)
                {
                    i7 = (int)Math.Round(Mth.Sqrt(d1 * d1 + d5 * d5) * 100F);
                    if (i7 > 0)
                    {
                        this.AddStat(StatList.distWalked, i7);
                    }
                }
                else
                {
                    i7 = (int)Math.Round(Mth.Sqrt(d1 * d1 + d5 * d5) * 100F);
                    if (i7 > 25)
                    {
                        this.AddStat(StatList.distFly, i7);
                    }
                }
            }
        }

        private void AddMountedMovementStat(double d1, double d3, double d5)
        {
            if (this.ridingEntity != null)
            {
                int i7 = (int)Math.Round(Mth.Sqrt(d1 * d1 + d3 * d3 + d5 * d5) * 100F);
                if (i7 > 0)
                {
                    if (this.ridingEntity is Minecart)
                    {
                        this.AddStat(StatList.distCart, i7);
                        if (this.minecartStartPos == default)
                        {
                            this.minecartStartPos = new Pos(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z));
                        }
                        else if (this.minecartStartPos.GetSqDistanceTo(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z)) >= 1000)
                        {
                            this.AddStat(AchievementList.onARail, 1);
                        }
                    }
                    else if (this.ridingEntity is Boat)
                    {
                        this.AddStat(StatList.distBoat, i7);
                    }
                    else if (this.ridingEntity is Pig)
                    {
                        this.AddStat(StatList.distPig, i7);
                    }
                }
            }
        }

        protected override void Fall(float f1)
        {
            if (f1 >= 2F)
            {
                this.AddStat(StatList.distFallen, (int)Math.Round(f1 * 100));
            }

            base.Fall(f1);
        }

        public override void OnKillEntity(Mob entityLiving1)
        {
            if (entityLiving1 is Monster)
            {
                this.TriggerAchievement(AchievementList.killEnemy);
            }
        }

        public override int GetItemIcon(ItemInstance itemStack1)
        {
            int i2 = base.GetItemIcon(itemStack1);
            if (itemStack1.itemID == Item.fishingRod.id && this.fishEntity != null)
            {
                i2 = itemStack1.GetIconIndex() + 16;
            }

            return i2;
        }

        public override void SetInPortal()
        {
            if (this.timeUntilPortal > 0)
            {
                this.timeUntilPortal = 10;
            }
            else
            {
                this.inPortal = true;
            }
        }
    }
}