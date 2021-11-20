using System.Reflection;
using Terraria;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamerClass.Helpers
{
    public class TrailHelper
    {
        public readonly Texture2D Texture;
        public readonly Color Color;
        public readonly Effect Effect;

        public delegate float WidthFunction(float progress);

        public WidthFunction VertexWidthFunction;

        public TrailHelper(Texture2D texture, Color color, WidthFunction widthFunction, Effect specialEffect = null)
        {
            Texture = texture;
            Color = color;
            VertexWidthFunction = widthFunction;
            Effect = specialEffect ?? GamerClass.TrailEffect;
        }

        private static Matrix GetMatrix()
        {
            GraphicsDevice device = Main.graphics.GraphicsDevice;
            int width = device.Viewport.Width;
            int height = device.Viewport.Height;
            Vector2 zoom = Main.GameViewMatrix.Zoom;

            Matrix View = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2f, height / -2f, 0f) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
            Matrix Projection = Matrix.CreateOrthographic(width, height, 0f, 1000f);

            return View * Projection;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2[] points)
        {
            // Save current spriteBatch state
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);

            spriteBatch.End();

            // Draw stuff
            GraphicsDevice device = Main.instance.GraphicsDevice;

            device.Textures[0] = Texture;
            device.SamplerStates[0] = SamplerState.LinearWrap;

            device.RasterizerState = RasterizerState.CullNone;

            Effect.Parameters["WVP"].SetValue(GetMatrix());
            Effect.CurrentTechnique.Passes["Texture"].Apply();

            var (vertices, indices) = PointsToVertexData(points);
            device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3);

            // Load previous spriteBatch state
            spriteBatch.Begin(sortMode, default, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }

        private (VertexPositionColorTexture[], short[]) PointsToVertexData(Vector2[] points)
        {
            var vertices = new VertexPositionColorTexture[points.Length * 2];

            for (short i = 0; i < points.Length; i++)
            {
                Vector2 normal;

                if (i == 0)
                    normal = points[i + 1] - points[i];
                else if (i == points.Length - 1)
                    normal = points[i] - points[i - 1];
                else
                    normal = (points[i] - points[i - 1]) + (points[i + 1] - points[i]);

                float ratio = (float)i / (points.Length - 1);
                float width = VertexWidthFunction(ratio);

                normal = new Vector2(-normal.Y, normal.X).SafeNormalize(Vector2.Zero) * width; // Rotate by -90° and scale to width

                vertices[i * 2 + 0] = new VertexPositionColorTexture(new Vector3(points[i] + normal, 0), Color, new Vector2(ratio, 0f));
                vertices[i * 2 + 1] = new VertexPositionColorTexture(new Vector3(points[i] - normal, 0), Color, new Vector2(ratio, 1f));
            }

            const int IVP = 6;
            short[] indices = new short[(points.Length - 1) * IVP];

            for (int i = 0; i < points.Length - 1; i++)
            {
                indices[IVP * i + 0] = (short)(2 * i + 0);
                indices[IVP * i + 1] = (short)(2 * i + 1);
                indices[IVP * i + 2] = (short)(2 * i + 2);

                indices[IVP * i + 3] = (short)(2 * i + 1);
                indices[IVP * i + 4] = (short)(2 * i + 3);
                indices[IVP * i + 5] = (short)(2 * i + 2);
            }

            return (vertices, indices);
        }
    }
}
