using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Algorithm
{
    class StoreOfPieces
    {
        public static OnePiece[,] store;
        public static int size;
        private GameBoard gm;
        public StoreOfPieces(int size, GameBoard gm)
        {
            store = new OnePiece[size, size];
            StoreOfPieces.size = size;
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
            foreach (var i in gm.GetApples())
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
            printWeights(Console.Write);
        }

        public void printWeights(Action<string> write)
        {
            write("\n\n");
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    write($"{store[i, j].Weight} ");
                }
                write("\n");
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
                            store[pointOfCenter.X + i, pointOfCenter.Y + j].AddWeightValue(value * optimazeFuncion(Math.Sqrt(QuadraticFunction(i) + QuadraticFunction(j)) / max_func_arg * 2));
                    }
            }
            private float optimazeFuncion(float v)
            {
                return (float)(1 / (1 + Math.Exp(-4 * ((2 - v) - 1))));
            }
            private float optimazeFuncion(double v)
            {
                return (float)(1 / (1 + Math.Exp(-4 * ((2 - v) - 1))));
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
