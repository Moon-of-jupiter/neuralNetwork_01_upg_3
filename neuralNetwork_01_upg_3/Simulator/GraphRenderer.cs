using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using neuralNetwork_01_upg_3.Simulator.Game.Snake;
using neuralNetwork_01_upg_3.Textures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator
{
    public class GraphRenderer
    {   
        

        public List<float> values;

        public RenderTarget2D renderTarget { get; private set; }

        public GraphRenderer_Data data;

        public GraphicsDevice gd;

        private Rectangle rect;

        private Vector2 reusedVec;
        private Vector2 reusedVec2;

        public GraphRenderer(GraphicsDevice gd,GraphRenderer_Data data, List<float> values)
        {
            this.data = data;
            this.values = values;

            this.gd = gd;
            reusedVec2 = new Vector2();
            reusedVec = new Vector2();
        }

        public void UpdateRT()
        {
            if(renderTarget != null)
            {
                renderTarget.Dispose();
                renderTarget = null;
            }

            renderTarget = new RenderTarget2D(gd, data.Bounds.X, data.Bounds.Y);
            
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


            float baseline = minValue;

            if(baseline < 0)
            {
                maxValue -= minValue;
            }

            gd.SetRenderTarget(renderTarget);
            
            gd.Clear(Color.Transparent);
            sb.Begin();

            for (int i = 0; i < values.Count; i++)
            {
                lastPoint = nextPoint;

                nextPoint.X = i / (values.Count - 1f);
                nextPoint.Y = 1 - (values[i] - baseline) / (maxValue);

                if(i == 0) continue;

                DrawSegment(sb,ref lastPoint,ref nextPoint);
            }

            sb.End();

            gd.SetRenderTarget(null);
            
        }

        protected void DrawSegment(SpriteBatch sb,ref Vector2 lastValue,ref Vector2 nextValue)
        {
            ValueToPoint(ref lastValue, ref reusedVec);
            ValueToPoint(ref nextValue, ref reusedVec2);

            DrawHelper.DrawLine(sb, data.tex, data.tex.Bounds, data.color, reusedVec, reusedVec2, data.lineThickness);
            DrawPoint(sb,ref lastValue);
        }

        protected void DrawPoint(SpriteBatch sb,ref Vector2 point)
        {
            ValueToPoint(ref point, ref reusedVec);
            rect.Width = data.lineThickness;
            rect.Height = data.lineThickness;
            rect.X = (int)(reusedVec.X);
            rect.Y = (int)(reusedVec.Y);
            sb.Draw(data.connection_tex, rect,data.connection_tex.Bounds,data.color,0, data.connection_tex.Bounds.Size.ToVector2() * 0.5f, SpriteEffects.None, 0);

        }


        protected void ValueToPoint(ref Vector2 value, ref Vector2 outVal)
        {
            outVal.X = value.X * data.Bounds.X;
            outVal.Y = value.Y * data.Bounds.Y - data.lineThickness * 0.5f;
                
        }

    }

    public struct GraphRenderer_Data
    {
        public Point Bounds;

        public int lineThickness;

        public Texture2D tex;
        public Texture2D connection_tex;
        public Color color;

        
        public Color background_color;

        
    }
}
