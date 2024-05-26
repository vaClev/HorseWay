namespace HorseWay
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int SIZEX = 6;
            const int SIZEY = 6;
            testWayFromTo(SIZEX, SIZEY);
            testAllCellsWay(SIZEX, SIZEY);
                      
        }
        static void testWayFromTo(int sizeX, int sizeY)
        {
            int SIZEX = sizeX;
            int SIZEY = sizeY;
            Point StartPoint = new Point(0, 0);
            Point FinishPoint = new Point(0, 3);
            Console.WriteLine($"Board size:{SIZEX}x{SIZEY}");

            Console.WriteLine($"Knight start :{StartPoint.GetCheckmatePos()}");
            Console.WriteLine($"Knight mustFinish :{FinishPoint.GetCheckmatePos()}");
            CheckMateStepChecker checker = new CheckMateStepChecker(SIZEX, SIZEY);

            Knight horse = new Knight(SIZEX, SIZEY);
            List<Point>? steps = horse.getWayFromTo(StartPoint, FinishPoint, checker);

            //вывод результатов
            foreach (var p in steps)
            {
                Console.Write($"{p.GetCheckmatePos()} -> ");
            }
            Console.WriteLine();
        }
        static void testAllCellsWay(int sizeX, int sizeY)
        {
            int SIZEX = sizeX;
            int SIZEY = sizeY;
            Point StartPoint = new Point(0, 0);
            CheckMateStepChecker checker = new CheckMateStepChecker(SIZEX, SIZEY);
            Knight horse = new Knight(SIZEX, SIZEY);
            Node root = new Node(new Point(0, 0), null); // корень дерева для дальнейшего просмотра
            List<Point>? longUnicWay = horse.getAllCellsWay(StartPoint, checker, root);


            //вывод результатов
            if (longUnicWay == null)
            {
                Console.WriteLine($"\nПуть через все клетки на доске {SIZEX}x{SIZEX} начиная с {StartPoint.GetCheckmatePos()} невозможен");
                Console.WriteLine($"\nищем наибольший возможный путь:");
                longUnicWay = horse.getLongestWay(StartPoint, checker, root);
            }
            Console.WriteLine($"\nMax unic way size = {longUnicWay?.Count - 1}");
            foreach (var p in longUnicWay)
            {
                Console.Write($"{p.GetCheckmatePos()} -> ");
            }

            // визуализация в каких клетках был  в каких не был
            bool[,] cells = new bool[SIZEX, SIZEY];
            foreach (var p in longUnicWay)
            {
                cells[p.X, p.Y] = true;
            }
            Console.WriteLine();
            for (int i = SIZEY - 1; i >= 0; i--)
            {
                for (int j = 0; j < SIZEX; j++)
                {
                    Console.Write($"{cells[j, i]} ");
                }
                Console.WriteLine();
            }
        }

        /*static void showChildren(Node node)
        {
            Console.Write($"Childrens {node.Position.GetCheckmatePos()}:");
            foreach (var child in node.Children)
            {
                Console.Write(child.Position.GetCheckmatePos()+" -- ");
            }
            foreach (var child in node.Children)
            {
                showChildren(child);
            }
            Console.WriteLine();
            Console.WriteLine();
        }*/

    }
}
