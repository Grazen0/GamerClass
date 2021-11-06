using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.ClumpOfBlocks
{
    public abstract class TetrisBlock : ModProjectile
    {
        public readonly int Unit = 32;
        public abstract int FragmentType { get; }

        public int Rotation => (int)projectile.ai[0];

        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            projectile.friendly = true;
            projectile.GamerProjectile().gamer = true;
        }

        public virtual void SafeSetDefaults() { }

        public override void AI()
        {
            if (projectile.GamerProjectile().firstTick)
            {
                if (Rotation % 2 == 1)
                {
                    (projectile.width, projectile.height) = (projectile.height, projectile.width);
                }

                projectile.rotation = Rotation * MathHelper.PiOver2;
            }

            if (projectile.velocity.Y < 25f)
                projectile.velocity.Y += 2f;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/TetrisBlockBreak").WithVolume(0.7f), projectile.Center);
            Color color = Color.White;

            switch (FragmentType)
            {
                case 0:
                    color = Color.Red;
                    break;
                case 1:
                    color = Color.LimeGreen;
                    break;
                case 2:
                    color = Color.Magenta;
                    break;
                case 3:
                    color = Color.Yellow;
                    break;
                case 4:
                    color = Color.Blue;
                    break;
                case 5:
                    color = Color.Orange;
                    break;
                case 6:
                    color = Color.Cyan;
                    break;
            }

            for (int d = 0; d < 6; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, newColor: color, Scale: 1.2f);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
            }

            if (Main.myPlayer == projectile.owner)
            {
                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(
                        projectile.Center - Vector2.UnitY * projectile.height / 4,
                        -Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(16f, 25f),
                        ModContent.ProjectileType<TetrisFragment>(),
                        projectile.damage / 4,
                        projectile.knockBack / 4,
                        projectile.owner,
                        FragmentType);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            GamerUtils.DrawCentered(this, spriteBatch, lightColor, false);
            return false;
        }
    }

    public class ZBlock : TetrisBlock
    {
        public override int FragmentType => 0;

        public override void SafeSetDefaults()
        {
            projectile.width = 48;
            projectile.height = 76;
        }
    }

    public class SBlock : TetrisBlock
    {
        public override int FragmentType => 1;

        public override void SafeSetDefaults()
        {
            projectile.width = 48;
            projectile.height = 76;
        }
    }

    public class TBlock : TetrisBlock
    {
        public override int FragmentType => 2;

        public override void SafeSetDefaults()
        {
            projectile.width = 76;
            projectile.height = 48;
        }
    }

    public class OBlock : TetrisBlock
    {
        public override int FragmentType => 3;

        public override void SafeSetDefaults()
        {
            projectile.width = projectile.height = 54;
        }
    }

    public class JBlock : TetrisBlock
    {
        public override int FragmentType => 4;

        public override void SafeSetDefaults()
        {
            projectile.width = 72;
            projectile.height = 48;
        }
    }

    public class LBlock : TetrisBlock
    {
        public override int FragmentType => 5;

        public override void SafeSetDefaults()
        {
            projectile.width = 72;
            projectile.height = 48;
        }
    }

    public class IBlock : TetrisBlock
    {
        public override int FragmentType => 6;

        public override void SafeSetDefaults()
        {
            projectile.width = 110;
            projectile.height = 20;
        }
    }
}
