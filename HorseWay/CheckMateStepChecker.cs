using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseWay
{
    public class Point
    {
        public short X { set; get; }
        public short Y { set; get; }
        static char[] CheckMateSymbols = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
        public Point(short x, short y)
        {
            X = x;
            Y = y;
        }
        public string GetCheckmatePos()
        {
            //работаем в координатах от 0 до 7. (Как с индексами массива)
            //X это буквы 0-A 1-B
            //Y это цифры  0-1   1-2
            return $"{CheckMateSymbols[X]}{Y + 1}";
        }


        //тут нужно додумать
        public static bool operator ==(Point o, Point b)
        {
            return o.Equals(b);
        }
        public static bool operator !=(Point o, Point b)
        {
            return !(b == o);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                Point p = (Point)obj;
                if (p.X == this.X && p.Y == this.Y) return true;
                return false;
            }
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    internal class CheckMateStepChecker
    {
        //работаем в координатах от 0 до 7. (Как с индексами массива)
        int maxX;
        int maxY;
        public CheckMateStepChecker(int sizeX, int sizeY)
        {
            maxX = sizeX;
            maxY = sizeY;
        }
        public bool checkPosition(Point p)
        {
            if (p.X < maxX && p.Y < maxY && p.X >= 0 && p.Y >= 0)
            {
                return true;
            }
            return false;
        }
    }
}
