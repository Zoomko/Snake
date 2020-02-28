using System;
using System.Collections.Generic;
using SnakeBattle.Api;

namespace Client
{
    class OnePiece
    {
        private float weight;
        private List<float> listOfValue;
        private BoardPoint point;
        public OnePiece(int w, BoardPoint p)
        {
            listOfValue = new List<float>();
            weight = w;
            point = p;
        }
        public OnePiece(BoardPoint p)
        {
            listOfValue = new List<float>();
            weight = 0;
            point = p;
        }
        public void UpdatePiece()
        {
            listOfValue.Clear();
            weight = 0;
        }
        public float Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }
        public void AddWeightValue(float v)
        {
            listOfValue.Add(v);
        }
        public void MeanValue()
        {
            var sum = 0.0f;
            foreach (var i in listOfValue)
            {
                sum += i;
            }
            if (listOfValue.Count == 0)
                weight = 0;
            else
                weight = sum / listOfValue.Count;
        }
        public void ClearList()
        {
            listOfValue.Clear();
        }
    }

    class StoreOfPieces
    {
        public static OnePiece[,] store;
        public static int size;
        private GameBoard gm;
        public StoreOfPieces(int s, GameBoard gm)
        {
            store = new OnePiece[s, s];
            size = s;
            this.gm = gm;
            Update();
        }
        private void Update()
        {
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    store[i, j] = new OnePiece(new BoardPoint(i, j));
                    store[i, j].ClearList();
                }
                
            }
            foreach(var i in gm.GetApples())
            {
                new Field(i, 1);
            }            
            ToMeanValue();
        }        
        private void ToMeanValue()
        {
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    store[i, j].MeanValue();
                }

            }
        }
        public void printWeights()
        {
            Console.WriteLine();
            Console.WriteLine();
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    Console.Write(store[i, j].Weight);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }

        }
        class Field
        {
            private int radius = 20;
            private float value;
            BoardPoint pointOfCenter;            
            private float max_func_arg;
            public Field(BoardPoint p, float val)
            {
                pointOfCenter = p;
                value = val;

                max_func_arg = radius * (float)Math.Sqrt(2.0);                
                CreateField();
            }
            private void CreateField()
            {
                Console.WriteLine(pointOfCenter.X);
                Console.WriteLine(pointOfCenter.Y);
                store[pointOfCenter.X, pointOfCenter.Y].Weight = value;
                for (var i = -radius + 1; i < radius; i++)
                    for (var j = -radius + 1; j < radius; j++)
                    {
                        if (pointOfCenter.X + i >= 0 && pointOfCenter.X + i < size && pointOfCenter.Y + j >= 0 && pointOfCenter.Y + j < size)
                            store[pointOfCenter.X + i, pointOfCenter.Y + j].AddWeightValue(value * optimazeFuncion(Math.Sqrt(QuadraticFunction(i) + QuadraticFunction(j))/max_func_arg*2));
                    }
            }
            private float optimazeFuncion(float v)
            {
                return (float)(1 / (1 + Math.Exp(-4 * ((2 - v) - 1))));
            }
            private float optimazeFuncion(double v)
            {
                return (float)(1 / (1 + Math.Exp(-4 * ((2- v) - 1))));
            }

            private float QuadraticFunction(float x)
            {
                return x * x;
            }
            private float QuadraticFunction(double x)
            {
                return (float)(x * x);
            }
        }                
    }
}
