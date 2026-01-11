using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.NeuralNet.Visualizer
{
    public class ANN_Visualizer
    {
        public A_NeuralNet net;

        public RenderTarget2D rt;

        public Point Resolution;

        public Point NodeSize;

        protected Color zeroColor;
        protected Color oneColor;
        protected Texture2D neuronTex;

        private Rectangle destRect;
        public ANN_Visualizer(A_NeuralNet net) 
        {
            
        }

        public void InitializeTextures(Texture2D nodeTex, Color ZeroColor, Color OneColor)
        {

        }
        public void InitializeRenderData(Point Resolution)
        {

        }

        public void SetNeuralNet(A_NeuralNet net)
        {

        }

        public void UpdatRenderData()
        {

        }


        protected void GetNodeDestRect(int layer, int index, ref Rectangle dt)
        {

        }

        protected Color LerpColor(Color colorA, Color colorB, float T)
        {
            return Color.Lerp(colorA, colorB, T);
        }

        protected void DrawNode(SpriteBatch sb,int layer, int index)
        {   
            var c = LerpColor(zeroColor,oneColor, net.neuralLayers[layer].neurons[index].output.value);
            GetNodeDestRect(layer, index, ref destRect);
            sb.Draw(neuronTex, destRect, c);
        }



        public void Render(SpriteBatch sb)
        {
            

            for(int i = 0; i < net.neuralLayers.Count; i++)
            {
                for (int j = 0; j < net.neuralLayers[i].neurons.Count; j++)
                {
                    DrawNode(sb, i, j);
                }
            }
        }
    }
}
