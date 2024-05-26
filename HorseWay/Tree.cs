using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseWay
{
    public class Node
    {
        public Point Position { get;}
        public Node? Parent { get;  }
        //public List<Node> Children { get; set; }

        public Node(Point pos, Node? parent)
        {
            Position = pos;
            Parent = parent;
            //Children = new List<Node>(); // отключил т.к. после нахождения пути древо не нужно.  ссылки теряются и отдаются сборщику мусора
        }
        ~Node()
        {
            //Console.Write($"/{Position.GetCheckmatePos()}/");
        }
    }
    internal class Tree
    {

    }
}
