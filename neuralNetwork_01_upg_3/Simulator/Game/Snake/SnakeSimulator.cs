using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using neuralNetwork_01_upg_3.RNG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace neuralNetwork_01_upg_3.Simulator.Game.Snake
{
    public class SnakeSimulator
    {
        public float score => snakeBody.Count + time * 0.001f;
        public bool gameOver { get; protected set; }
        protected ISnakeControler controler;

        protected SnakeMapData mapData;

        public enum MapElementType : byte
        {
            empty = 0,
            apple = 1,
            snake = 2,
            edge = 255,
        }


        // map
        public MapElementType[,] map; // 0 = empty, 1 = apple, 2 = snake 
        public int time = 0;

        // snake
        protected Queue<Point> snakeBody;

        public Point SnakeHead { get; protected set; }
        protected Point nextSnakePos;

        protected Queue<Point> appleTargetQueue;

        // apple data

        public Point LatestApple { get; protected set; }
        protected bool appleRngInitialized = false;
        public SnakeSimulator(ISnakeControler controler, SnakeMapData mapData)
        {
            this.controler = controler;
            this.mapData = mapData;
            InitializeSnakeMap();
        }

        public void InitializeSnakeMap() 
        {
            if (map == null)
                map = new MapElementType[mapData.size.X, mapData.size.Y];
            else 
            {
                Point p = Point.Zero;

                for (int x = 0; x < mapData.size.X; x++)
                {
                    p.X = x;
                    for (int y = 0; y < mapData.size.Y; y++)
                    {
                        p.Y = y;
                        UpdateMap(ref p, MapElementType.empty);
                    }
                }
            }
            
            if(snakeBody == null) 
                snakeBody = new Queue<Point>();
            else 
                snakeBody.Clear();

            var a = new Point(mapData.size.X / 2, mapData.size.Y / 2);
            SnakeHead = a;
            for(int i = 0; i < 2; i++)
            {
                snakeBody.Enqueue(a);
                snakeBody.Enqueue(a);
            }

            
            //UpdateMap(ref a,MapElementType.snake);
            if(!appleRngInitialized)
                InitialzieAppleRNG(mapData.seed);

            gameOver = false;

            UpdateApple();
            
        }

        protected void InitialzieAppleRNG(int seed)
        {
            appleRngInitialized = true;

            appleTargetQueue = new Queue<Point>();

            List<Point> temp = new List<Point>();

            int c = 0;

            for(int x = 0; x < mapData.size.X; x++)
            {
                for (int y = 0; y < mapData.size.Y; y++)
                {
                    c++;
                    temp.Add(new Point(x, y));
                }
            }

            int index = 0;

            for (int i = 0; i < c; i++)
            {
                index = Math.Abs(CustomRandom.ShiftRandomXOr(i + seed)) % temp.Count;

                appleTargetQueue.Enqueue(temp[index]);

                temp.RemoveAt(index);
            }
        }

        

        protected bool CycleQueue<T>(Queue<T> queue, int maxIterations, out T foundValue, Func<T,bool> condition)
        {
            int counter = 0;
            
            do
            {
                foundValue = queue.Dequeue();
                queue.Enqueue(foundValue);
                if (++counter > maxIterations) return false;

            } while (!condition(foundValue));

            return true;
        }

        public void Update()
        {
            if(gameOver) return;
            time++;
            UpdateSnake(controler.Control(this));
            
        }

        protected void UpdateSnake(Point direction)
        {
            if(snakeBody.Count == 0) return;

            nextSnakePos = snakeBody.Last() + direction;

            MapElementType value_at_next_pos = GetElementAtPos(ref nextSnakePos);

            
            // game over
            if( value_at_next_pos == MapElementType.snake ||
                value_at_next_pos == MapElementType.edge)
            {
                EndGame();
                return;
            }

            // build snake
            SnakeHead = nextSnakePos;
            UpdateMap(ref nextSnakePos, MapElementType.snake);
            snakeBody.Enqueue(nextSnakePos);

            // clear tail
            if (value_at_next_pos != MapElementType.apple)
            {
                var tail = snakeBody.Dequeue();

                UpdateMap(ref tail, MapElementType.empty);
                return;
            }


            // if apple
            UpdateApple();

        }

        protected void UpdateApple()
        {
            
            if(!CycleQueue(appleTargetQueue, map.Length, out Point foundPos, (Point p) => 
            {
                var target = GetElementAtPos(ref p);
                return target == MapElementType.empty || target == MapElementType.apple;
            })) return;

            SetNewApplePos(ref foundPos);
        }

        protected void SetNewApplePos(ref Point pos)
        {
            LatestApple = pos;
            UpdateMap(ref pos, MapElementType.apple);
        }

        public void EndGame()
        {
            gameOver = true;
        }
        
        public bool Raycast(ref Point startPos,ref Point direction, out int length, MapElementType target)
        {
            Point p = startPos;

            MapElementType currentTarget;
            bool hit = false;
            length = 0;
            do
            {
                length++;
                p += direction;
                currentTarget = GetElementAtPos(ref p);
            }
            while (!((hit = currentTarget == target) || currentTarget == MapElementType.edge));

            return hit;
        }


        public MapElementType GetElementAtPos(ref Point pos)
        {
            if (!IsWithinMap(ref pos)) return MapElementType.edge;

            return map[pos.X, pos.Y];
        }

        protected bool IsWithinMap(ref Point pos)
        {
            return  (pos.X >= 0 && pos.X < map.GetLength(0)) && 
                    (pos.Y >= 0 && pos.Y < map.GetLength(1));
        }

        protected void UpdateMap(ref Point pos, MapElementType newValue)
        {
            if(!IsWithinMap(ref pos)) return;

            map[pos.X, pos.Y] = newValue;
        }

    }

    public struct SnakeMapData
    {
        public Point size;
        public int seed;

    }

    public interface ISnakeControler
    {
        public Point Control(SnakeSimulator simulation);
    }
}
