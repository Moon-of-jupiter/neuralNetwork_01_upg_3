using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using neuralNetwork_01_upg_3.Simulator.Game.Snake;
using neuralNetwork_01_upg_3.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator
{
    public class GraphRenderer
    {   
        

        public List<float> values;

         

        public GraphRenderer_Data data;


        public GraphRenderer(GraphRenderer_Data data, List<float> values)
        {
            this.data = data;
            this.values = values;
        }

        
        public void Render(SpriteBatch sb)
        {
            if(values.Count < 1) return;

            Vector2 lastPoint = Vector2.Zero;
            Vector2 nextPoint = Vector2.Zero;

            float maxValue = values[0];
            float minValue = values[0];

            for(int i = 1; i < values.Count; i++)
            {
                if (maxValue < values[i]) maxValue = values[i];
                else if (minValue > values[i]) minValue = values[i];
            }


            float baseline = values[0];
            for (int i = 0; i < values.Count; i++)
            {
                lastPoint = nextPoint;

                nextPoint.X = i / (values.Count - 1f);
                nextPoint.Y = 1 - (values[i] - baseline) / maxValue;

                if(i == 0) continue;

                DrawPoint(sb, lastPoint, nextPoint);
            }
            
        }

        protected void DrawPoint(SpriteBatch sb,Vector2 lastValue, Vector2 nextValue)
        {
            DrawHelper.DrawLine(sb, data.tex, data.tex.Bounds, data.color, ValueToPoint(lastValue), ValueToPoint(nextValue), data.lineThickness);
        }

        protected Vector2 ValueToPoint(Vector2 value)
        {
            return value * data.Bounds.Size.ToVector2() + data.Bounds.Location.ToVector2();
        }

    }

    public struct GraphRenderer_Data
    {
        public Rectangle Bounds;

        public int lineThickness;

        public Texture2D tex;
        public Color color;

        
    }
}
