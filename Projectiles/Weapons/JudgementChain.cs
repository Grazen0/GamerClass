using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.Weapons
{
    public class JudgementChain : ModProjectile
    {
        private readonly int Size = 12;
        private readonly int Unit = 120;

        private float xScale = 0.75f;
        private bool playSound = true;

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.alpha = 180;
            projectile.GamerProjectile().gamer = true;
        }

        public int State
        {
            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float Timer
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void AI()
        {
            int timerTarget = 0;

            switch (State)
            {
                case 0: // Telegraph
                    timerTarget = 20;
                    projectile.alpha = (int)MathHelper.Min(projectile.alpha + Timer * 2, 255);
                    xScale -= Timer * 0.01f;
                    if (xScale < 0f) xScale = 0f;
                    break;
                case 1: // Open up
                    timerTarget = 2;
                    projectile.alpha = (int)MathHelper.Max(projectile.alpha - 200, 0);
                    xScale += 0.5f;

                    if (playSound)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/JudgementChain_" + Main.rand.Next(4)));
                        playSound = false;
                    }
                    break;
                case 2:
                    // Close down
                    if (projectile.timeLeft > 30)
                        projectile.timeLeft = 30;

                    xScale -= Timer * 0.008f;
                    projectile.alpha = (int)MathHelper.Min(projectile.alpha + Timer * 2, 255);
                    break;
            }

            if (++Timer >= timerTarget && State < 2)
            {
                Timer -= timerTarget;
                State++;
            }
        }

        public override bool CanDamage() => State > 0 && projectile.timeLeft > 10;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = projectile.Center;
            start.Y -= Size * Unit;

            Vector2 end = projectile.Center;
            end.Y += Size * Unit;

            float point = 0f;

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 42, ref point);
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;

            Vector2 start = projectile.Center;
            start.Y -= Size * Unit;

            Vector2 end = projectile.Center;
            end.Y += Size * Unit;

            Utils.PlotTileLine(start, end, 42, DelegateMethods.CutTiles);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Color color = projectile.GetAlpha(lightColor);
            Vector2 origin = new Vector2(texture.Width / 2, 0f);

            for (int i = -Size; i < Size; i++)
            {
                Vector2 position = projectile.Center;
                position.Y += i * Unit;

                spriteBatch.Draw(
                    texture,
                    position - Main.screenPosition,
                    null,
                    color * projectile.Opacity,
                    projectile.rotation,
                    origin,
                    new Vector2(xScale * projectile.scale, projectile.scale),
                    SpriteEffects.None,
                    0f);
            }

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
