using LWCSGL.OpenGL;
using SharpCraft.Client.GUI;
using SharpCraft.Client.Models;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Monsters;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.Entities.Weather;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Entities
{
    public class EntityRenderDispatcher
    {
        private NullDictionary<Type, Render> renderers = new NullDictionary<Type, Render>();
        public static EntityRenderDispatcher instance = new EntityRenderDispatcher();
        private Font font;
        public static double renderPosX;
        public static double renderPosY;
        public static double renderPosZ;
        public Textures textures;
        public ItemInHandRenderer itemRenderer;
        public Level worldObj;
        public Mob livingPlayer;
        public float playerViewY;
        public float playerViewX;
        public Options options;
        public double field_1222_l;
        public double field_1221_m;
        public double field_1220_n;

        private EntityRenderDispatcher() 
        {
            this.renderers[typeof(Spider)] = new RenderSpider();
            this.renderers[typeof(Pig)] = new RenderPig(new ModelPig(), new ModelPig(0.5F), 0.7F);
            this.renderers[typeof(Sheep)] = new RenderSheep(new ModelSheep2(), new ModelSheep1(), 0.7F);
            this.renderers[typeof(Cow)] = new RenderCow(new ModelCow(), 0.7F);
            this.renderers[typeof(Wolf)] = new RenderWolf(new ModelWolf(), 0.5F);
            this.renderers[typeof(Chicken)] = new RenderChicken(new ModelChicken(), 0.3F);
            this.renderers[typeof(Creeper)] = new RenderCreeper();
            this.renderers[typeof(Skeleton)] = new RenderBiped(new ModelSkeleton(), 0.5F);
            this.renderers[typeof(Zombie)] = new RenderBiped(new ModelZombie(), 0.5F);
            this.renderers[typeof(Slime)] = new RenderSlime(new ModelSlime(16), new ModelSlime(0), 0.25F);
            this.renderers[typeof(Player)] = new RenderPlayer();
            this.renderers[typeof(Giant)] = new RenderGiantZombie(new ModelZombie(), 0.5F, 6.0F);
            this.renderers[typeof(Ghast)] = new RenderGhast();
            this.renderers[typeof(Squid)] = new RenderSquid(new ModelSquid(), 0.7F); //render quid
            this.renderers[typeof(Mob)] = new RenderLiving<Mob>(new ModelBiped(), 0.5F);
            this.renderers[typeof(Entity)] = new RenderEntity();
            this.renderers[typeof(Painting)] = new RenderPainting();
            this.renderers[typeof(Arrow)] = new RenderArrow();
            this.renderers[typeof(Snowball)] = new RenderSnowball(Item.snowball.GetIconFromDamage(0));
            this.renderers[typeof(ThrownEgg)] = new RenderSnowball(Item.egg.GetIconFromDamage(0));
            this.renderers[typeof(Fireball)] = new RenderFireball();
            this.renderers[typeof(ItemEntity)] = new RenderItem();
            this.renderers[typeof(PrimedTnt)] = new RenderTNTPrimed();
            this.renderers[typeof(FallingTile)] = new RenderFallingSand();
            this.renderers[typeof(Minecart)] = new RenderMinecart();
            this.renderers[typeof(Boat)] = new RenderBoat();
            this.renderers[typeof(FishingHook)] = new FishingHookRenderer();
            this.renderers[typeof(LightningBolt)] = new RenderLightningBolt();

            foreach (Render o in this.renderers.Values) 
            {
                //fuck you.
                //no thanks -dart
                o.SetRenderManager(this);
            }
        }

        public Render GetEntityClassRenderObject(Type class1)
        {
            this.renderers.TryGetValue(class1, out Render render);

            if (render == null && class1 != typeof(Entity))
            {
                render = this.GetEntityClassRenderObject(class1.BaseType);
                this.renderers[class1] = render;
            }

            return render;
        }

        public Render GetEntityRenderObject(Entity e) 
        {
            return GetEntityClassRenderObject(e.GetType());
        }

        public object GetEntityRenderObjectAsObject(Entity e)
        {
            return GetEntityClassRenderObject(e.GetType());
        }

        public void CacheActiveRenderInfo(Level world1, Textures renderEngine2, Font fontRenderer3, Mob entityLiving4, Options gameSettings5, float f6)
        {
            this.worldObj = world1;
            this.textures = renderEngine2;
            this.options = gameSettings5;
            this.livingPlayer = entityLiving4;
            this.font = fontRenderer3;
            if (entityLiving4.IsSleeping())
            {
                int i7 = world1.GetTile(Mth.Floor(entityLiving4.x), Mth.Floor(entityLiving4.y), Mth.Floor(entityLiving4.z));
                if (i7 == Tile.bed.id)
                {
                    int i8 = world1.GetData(Mth.Floor(entityLiving4.x), Mth.Floor(entityLiving4.y), Mth.Floor(entityLiving4.z));
                    int i9 = i8 & 3;
                    this.playerViewY = i9 * 90 + 180;
                    this.playerViewX = 0.0F;
                }
            }
            else
            {
                this.playerViewY = entityLiving4.prevYaw + (entityLiving4.yaw - entityLiving4.prevYaw) * f6;
                this.playerViewX = entityLiving4.prevPitch + (entityLiving4.pitch - entityLiving4.prevPitch) * f6;
            }

            this.field_1222_l = entityLiving4.lastTickPosX + (entityLiving4.x - entityLiving4.lastTickPosX) * f6;
            this.field_1221_m = entityLiving4.lastTickPosY + (entityLiving4.y - entityLiving4.lastTickPosY) * f6;
            this.field_1220_n = entityLiving4.lastTickPosZ + (entityLiving4.z - entityLiving4.lastTickPosZ) * f6;
        }

        public void RenderEntity(Entity entity1, float f2)
        {
            double d3 = entity1.lastTickPosX + (entity1.x - entity1.lastTickPosX) * f2;
            double d5 = entity1.lastTickPosY + (entity1.y - entity1.lastTickPosY) * f2;
            double d7 = entity1.lastTickPosZ + (entity1.z - entity1.lastTickPosZ) * f2;
            float f9 = entity1.prevYaw + (entity1.yaw - entity1.prevYaw) * f2;
            float b = entity1.GetEntityBrightness(f2);
            GL11.glColor3f(b, b, b);
            this.RenderEntityWithPosYaw(entity1, d3 - renderPosX, d5 - renderPosY, d7 - renderPosZ, f9, f2);
        }

        public void RenderEntityWithPosYaw(Entity entity1, double d2, double d4, double d6, float f8, float f9)
        {
            Render render10 = this.GetEntityRenderObject(entity1);
            if (render10 != null)
            {
                render10.DoRender(entity1, d2, d4, d6, f8, f9);
                render10.DoRenderShadowAndFire(entity1, d2, d4, d6, f8, f9);
            }
        }

        public void SetLevel(Level world1)
        {
            this.worldObj = world1;
        }

        public double Func_851_a(double d1, double d3, double d5)
        {
            double d7 = d1 - this.field_1222_l;
            double d9 = d3 - this.field_1221_m;
            double d11 = d5 - this.field_1220_n;
            return d7 * d7 + d9 * d9 + d11 * d11;
        }

        public Font GetFont()
        {
            return this.font;
        }
    }
}
