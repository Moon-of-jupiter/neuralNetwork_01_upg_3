using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Textures
{
    public static class DrawHelper
    {
        private static float RadToDegrees => 180f / MathF.PI;


        public static void DrawLine(SpriteBatch sb, Texture2D texture, Rectangle sourceRect, Color color, Vector2 A, Vector2 B, int thickness)
        {
            var temp = Vector2.Lerp(A, B, 0.5f);

            Rectangle dest = new Rectangle((int)temp.X,(int)temp.Y, (int)Vector2.Distance(A,B), thickness);

            sb.Draw(texture, dest, sourceRect, color, AngleOfPoints(A,B), Vector2.One * 0.5f, SpriteEffects.None, 0);
        }
        
        public static float AngleOfPoints(Vector2 A, Vector2 B)
        {
            
            float angle = 0;
            Vector2 direction = B - A;

            if (Vector2.Dot(direction, new Vector2(0, 1)) < 0)
            {
                angle = AngleBetweenVectors(direction, -Vector2.UnitX);
            }
            else
            {
                angle = AngleBetweenVectors(direction, Vector2.UnitX);
            }

            return angle;
        
        }

        public static float AngleBetweenVectors(Vector2 A, Vector2 B)
        {
            float angle = (float)Math.Acos((Vector2.Dot(A, B)) / (A.Length() * B.Length()));

            float angleD = angle * RadToDegrees;

            return angle;
        }

        
        


    }
}
