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

        }
        public Vertex(T value)
        {
            Value = value;
        }
        public Vertex(T value, Dictionary<Vertex<T>,double> edges)
        {
            Value = value;
            Edges = edges;
        }
    }
}
