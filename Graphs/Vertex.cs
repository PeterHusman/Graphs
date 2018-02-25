using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    public class Vertex<T>
    {
        public T Value;

        public Dictionary<Vertex<T>, double> Edges;

        public Vertex()
        {
            Color = ConsoleColor.White;
        }
        public Vertex(T value)
        {
            Value = value;
            Color = ConsoleColor.White;
        }
        public Vertex(T value, Dictionary<Vertex<T>,double> edges)
        {
            Value = value;
            Edges = edges;
        }

        public ConsoleColor Color = ConsoleColor.White;

        public double Cost = -1;

        public Vertex<T> LastVisited;
    }
}
