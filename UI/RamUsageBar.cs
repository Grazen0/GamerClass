using GamerClass.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace GamerClass.UI
{
    internal class RamUsageBar : UIState
    {
        private UIElement area;
        private UIImage barFrame;

        private Color barColor;

        public override void OnInitialize()
        {
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 600, 1f);
            area.Top.Set(30, 0f); // Placing it just a bit below the top of the screen.
            area.Width.Set(182, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
            area.Height.Set(30, 0f);

            barColor = Color.LightBlue;

            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.HeldItem.modItem is GamerWeapon)
            {
                base.Draw(spriteBatch);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            GamerPlayer modPlayer = Main.LocalPlayer.GetModPlayer<GamerPlayer>();
            float ramRadius = MathHelper.Clamp((float)modPlayer.usedRam / modPlayer.maxRam, 0f, 1f);

            Rectangle hitbox = area.GetInnerDimensions().ToRectangle();
            int border = 5;

            spriteBatch.Draw(
                Main.magicPixel,
                hitbox, 
                Color.Black);

            int barWidth = hitbox.Right - hitbox.Left - border * 2;

            float interpolationStart = 0.4f;
            float colorInterpolation = MathHelper.Max(0f, ramRadius - interpolationStart) * (1f / (1f - interpolationStart));
            Color color = Color.Lerp(Color.LightBlue, Color.OrangeRed, colorInterpolation);

            spriteBatch.Draw(
                Main.magicPixel,
                new Rectangle(hitbox.Left + border, hitbox.Y + border, (int)(barWidth * ramRadius), hitbox.Height - border * 2),
                color
                );
        }
    }
}
