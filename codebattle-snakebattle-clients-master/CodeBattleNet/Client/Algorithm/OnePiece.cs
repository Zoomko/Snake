using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class OnePiece
    {
        public float Weight { get; set; }
        private List<float> listOfValue;
        private BoardPoint point;
        public OnePiece(int w, BoardPoint p)
        {
            listOfValue = new List<float>();
            Weight = w;
            point = p;
        }
        public OnePiece(BoardPoint p) : this(0, p) { }

        public void UpdatePiece()
        {
            listOfValue.Clear();
            Weight = 0;
        }

        public void AddWeightValue(float v)
        {
            listOfValue.Add(v);
        }
        public void MeanValue()
        {
            if (listOfValue.Count == 0)
                Weight = 0;
            else
            {
                var sum = listOfValue.Sum();
                Weight = sum / listOfValue.Count;
            }

        }
        public void ClearList()
        {
            listOfValue.Clear();
        }
    }
}
