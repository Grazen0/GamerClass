using GamerClass.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;

namespace GamerClass.UI
{
    internal class RamUsageBar : UIState
    {
        internal int fireFrame = 0;
        internal int fireTimer = 0;
        internal int shakeTimer = -1;
        internal UIElement barFrame;
        internal Texture2D frameTexture;
        internal Texture2D fillTexture;

        public override void OnInitialize()
        {
            barFrame = new UIElement();
            barFrame.Left.Set(-barFrame.Width.Pixels - 500, 1f);
            barFrame.Top.Set(30, 0f);
            barFrame.Width.Set(168, 0f);
            barFrame.Height.Set(51, 0f);

            frameTexture = ModContent.GetTexture("GamerClass/UI/RamUsageFrame");
            fillTexture = ModContent.GetTexture("GamerClass/UI/RamUsageFill");

            Append(barFrame);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.HeldItem.modItem is GamerWeapon)
                base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var modPlayer = Main.LocalPlayer.GamerPlayer();

            // For some reason, the spriteBatch uses anti-aliasing by default
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Main.UIScaleMatrix);

            float ramRadius = MathHelper.Clamp((float)modPlayer.usedRam / modPlayer.maxRam2, 0f, 1f);
            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();

            // Draw frame
            int frameHeight = frameTexture.Height / 10;
            int frameIndex = (int)(ramRadius * 3);
            if (frameIndex > 2) 
                frameIndex = 2;

            if (modPlayer.gamerCooldown)
            {
                UpdateFireAnimation();
                frameIndex += 1 + fireFrame;

                if (shakeTimer == -1)
                    shakeTimer = 60;

                if (shakeTimer > 0)
                {
                    int shake = shakeTimer / 8;
                    hitbox.X += Main.rand.Next(-shake, shake);
                    hitbox.Y += Main.rand.Next(-shake, shake);

                    shakeTimer--;
                }
            }
            else
            {
                shakeTimer = -1;
            }

            spriteBatch.Draw(
                frameTexture,
                hitbox,
                new Rectangle(0, frameIndex * frameHeight, frameTexture.Width, frameHeight),
                Color.White);

            hitbox.X += 21;
            hitbox.Width -= 42;
            hitbox.Y += 30;
            hitbox.Height -= 39;

            Rectangle destinationRectangle = new Rectangle(hitbox.Left, hitbox.Y, (int)(hitbox.Width * ramRadius), hitbox.Height);
            Rectangle sourceRectangle = new Rectangle(0, 0, (int)(fillTexture.Width * ramRadius), fillTexture.Height);

            spriteBatch.Draw(
                fillTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);

            DrawHoverTooltip(modPlayer);
        }

        private void DrawHoverTooltip(GamerPlayer modPlayer)
        {
            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 12;
            hitbox.Width -= 24;
            hitbox.Y += 21;
            hitbox.Height -= 21;

            Rectangle mouse = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 1, 1);

            if (hitbox.Intersects(mouse))
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.instance.MouseText($"Used RAM: {modPlayer.usedRam}B / {modPlayer.maxRam2}B");
            }
        }

        private void UpdateFireAnimation()
        {
            if (++fireTimer > 4)
            {
                fireTimer = 0;
                if (++fireFrame >= 6)
                {
                    fireFrame = 0;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.HeldItem.modItem is GamerWeapon)
                base.Update(gameTime);
        }

        public void Unload()
        {
            fireFrame = 0;
            fireTimer = 0;
            shakeTimer = -1;
            frameTexture = null;
            fillTexture = null;
        }
    }
}
