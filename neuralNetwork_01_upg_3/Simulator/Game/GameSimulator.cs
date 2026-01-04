using neuralNetwork_01_upg_3.Simulator.Game.Entety;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.Game
{
    public abstract class GameSimulator
    {
        

        public GameSimulator(GameEntety entetiy)
        {

        }

        public abstract void Update();

        public abstract void Render();

    }
}
