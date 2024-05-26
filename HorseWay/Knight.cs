using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseWay
{
    internal class Knight
    {
        //List<Point> way;
        static short[,] CheckMatrix = new short[8, 2] { { 2, 1 }, { 2, -1 }, { 1, -2 }, { -1, -2 }, { -2, -1 }, { -2, 1 }, { -1, 2 }, { 1, 2 } };
        int boardSizeX;
        int boardSizeY;
        public Knight()
        {
            //way = new List<Point>();
            boardSizeX = 8; //изначально максимально длинный 
            boardSizeY = 8; //изначально максимально длинный 
        }
        public Knight(int boardSizeX, int boardSizeY)
        {
           //way = new List<Point>();
            this.boardSizeX = boardSizeX; 
            this.boardSizeY = boardSizeY; 
        }
        /*public Knight(List<Point> prevWay, int foundedWaySize)
        {
            way = new List<Point>();
            foreach (var step in prevWay)
            {
                way.Add(step);
            }
            this.foundedWaySize = foundedWaySize;
        }*/
        public List<Point> getNextSteps(Point p, CheckMateStepChecker checker)
        {
            List<Point> nextStepsVariants = new List<Point>();
            //рассматривается 8 варинтов хода
            //ходы вправо
            for (int i = 0; i < 8; i++)
            {
                Point temp = new Point((short)(p.X + CheckMatrix[i, 0]), (short)(p.Y + CheckMatrix[i, 1]));
                if (checker.checkPosition(temp)) nextStepsVariants.Add(temp);
            }
            return nextStepsVariants;
        }

        /*решение задачи поиска пути без использования дерева
         * 
         * public List<Point> getWayFromTo(Point start, Point finish, CheckMateStepChecker checker)
        {
            List<Point> nextSteps = getNextSteps(start, checker);
            way.Add(start);
            if (way.Count == foundedWaySize) return way; // ограничим поиск в 10 ходов  --- за 10 не нашел= хорош искать

            //Если среди соедующих ходов есть финишеая клетка --> вернем путь
            foreach (var p in nextSteps)
            {
                if (p.X == finish.X && p.Y == finish.Y)
                {
                    way.Add(p);
                    return way;
                }
            }

            //в следующем ходе нет целевой клетки --- создадим нового коня в каждой клетке следующего возможного хода -- и проверим для него
            List<List<Point>> ways = new List<List<Point>>();
            foreach (var p in nextSteps)
            {
                Knight horseTemp = new Knight(way, foundedWaySize);
                ways.Add(horseTemp.getWayFromTo(p, finish, checker));
                //Console.WriteLine("here");
            }
            List<Point> shortestWay = ways[0];
            //Console.WriteLine("-----------------------there");
            // Нужно теперь сравнить пути в этих вариантах и вернуть кратчайший
            foreach (var way in ways)
            {
                if (way.Count < shortestWay.Count) shortestWay = way;
            }
            return shortestWay;
        }*/

        public List<Point>? getWayFromTo(Point start, Point finish, CheckMateStepChecker checker, Node? parent=null)
        {
            Node node = new Node(start, parent);
            //if (parent != null) parent.Children.Add(node);


            List<Point> nextSteps = getNextSteps(start, checker);
            //главный якорь  если целевая точка в следующем ходе - верни путь до корня древа
            if (nextSteps.Contains(finish)) return GetListOfParents(node, finish);
                     
            List<Point> parents = GetListOfParents(node);
            //Второй якорь резерв переполнения  -- ограничение поиска в 8 ходов
            if (parents.Count>8)
            {
                return parents;
            }
            //иначе создай возможных детей и запусти для них ту же функцию
            List<List<Point>?> ways = new List<List<Point>?>();
            foreach (var step in nextSteps) 
            {
                if (parents.Contains(step))
                {
                    //Console.WriteLine("Contains parent");
                    continue;
                }
                List<Point>? currentWay = getWayFromTo(step, finish, checker, node);
                ways.Add(currentWay);
            }
            //Если все возможные пути ведут в клетку в которой ты уже "был"  --- верни заглушку
            if (ways.Count==0) { return null; }
            
            List<Point>? shortestWay = null;
            //Console.WriteLine("-----------------------there");
            // Нужно теперь сравнить пути в этих вариантах и вернуть кратчайший
            foreach (var way in ways)
            {
                if (shortestWay == null) shortestWay = way;
                else if (way == null) continue;
                else if (way.Count < shortestWay.Count) shortestWay = way;
            }
            return shortestWay;
        }

        private List<Point> GetListOfParents(Node child, Point finish)
        {
            List<Point> parentsPositions = new List<Point>();
            parentsPositions.Add(finish);
            Node pointer = new Node(finish, child); 
            while (pointer.Parent!=null)
            {
                parentsPositions.Add(pointer.Parent.Position);
                pointer = pointer.Parent;
            }
            parentsPositions.Reverse();
            return parentsPositions;
        }
        private List<Point> GetListOfParents(Node child)
        {
            List<Point> parentsPositions = new List<Point>();
            Node? pointer = child;
            while (pointer.Parent!=null)
            {
                parentsPositions.Add(pointer.Position);
                pointer = pointer.Parent;
            }
            parentsPositions.Reverse();
            return parentsPositions;
        }

        public List<Point>? getAllCellsWay(Point start, CheckMateStepChecker checker, Node parent)
        {
            Node node = new Node(start, parent);
            //if (parent != null) parent.Children.Add(node);
            List<Point> nextSteps = getNextSteps(start, checker);
            List<Point> parents = GetListOfParents(node); // родители и ты сам

            //создадим контейнер для вариантов
            List<List<Point>?> ways = new List<List<Point>?>();
            //Набросаем в него уникальные пути 
            foreach (var step in nextSteps)
            {
                if (parents.Contains(step))
                {
                    //Console.WriteLine("Contains parent");
                    continue;
                }
                List<Point>? currentWay = getAllCellsWay(step, checker, node);
                if (currentWay != null && currentWay.Count== boardSizeX * boardSizeY) { return currentWay;}
                ways.Add(currentWay);
            }
            //Если все возможные пути ведут в клетку в которой ты уже "был"  --- верни заглушку
            if (ways.Count == 0) 
            {
                if (parents.Count < boardSizeX* boardSizeY) return null;
                return parents;
            }

            List<Point>? longestWay = null;
            //Console.WriteLine("-----------------------there");
            // Нужно теперь сравнить пути в этих вариантах и вернуть наибольший
            foreach (var way in ways)
            {
                if (longestWay == null) longestWay = way;
                else if (way == null) continue;
                else if (way.Count > longestWay.Count) longestWay = way;
            }
            return longestWay;
        }
        public List<Point>? getLongestWay(Point start, CheckMateStepChecker checker, Node parent)
        {
            Node node = new Node(start, parent);
            //if (parent != null) parent.Children.Add(node);
            List<Point> nextSteps = getNextSteps(start, checker);
            List<Point> parents = GetListOfParents(node); // родители и ты сам

            //создадим контейнер для вариантов
            List<List<Point>?> ways = new List<List<Point>?>();
            //Набросаем в него уникальные пути 
            foreach (var step in nextSteps)
            {
                if (parents.Contains(step))
                {
                    //Console.WriteLine("Contains parent");
                    continue;
                }
                List<Point>? currentWay = getLongestWay(step, checker, node);
                if (currentWay != null && currentWay.Count == boardSizeX * boardSizeY) { return currentWay; }
                ways.Add(currentWay);
            }
            //Если все возможные пути ведут в клетку в которой ты уже "был"  --- верни заглушку
            if (ways.Count == 0)
            {
                if (parents.Count < boardSizeX * boardSizeY-5) return null;
                return parents;
            }

            List<Point>? longestWay = null;
            //Console.WriteLine("-----------------------there");
            // Нужно теперь сравнить пути в этих вариантах и вернуть наибольший
            foreach (var way in ways)
            {
                if (longestWay == null) longestWay = way;
                else if (way == null) continue;
                else if (way.Count > longestWay.Count) longestWay = way;
            }
            return longestWay;
        }
    }
}
