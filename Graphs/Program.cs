using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph<int> graph = new Graph<int>();
            graph.Add(1);
            graph.Add(2);
            graph.Add(3);
            graph.Add(4);
            Visualize(graph);


            Console.ReadKey();
        }



        static void Visualize(Graph<int> graph)
        {
            int radius = 10;
            int border = 2;
            Dictionary<Vertex<int>, Point> positions = new Dictionary<Vertex<int>, Point>();
            Console.Clear();
            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                double x = Math.Cos(2 * Math.PI * i / graph.Vertices.Count);
                double y = Math.Sin(2 * Math.PI * i / graph.Vertices.Count);
                int finalX = (int)(x * radius + radius + border);
                int finalY = (int)(y * radius + radius + border);
                positions.Add(graph.Vertices[i],new Point(finalX, finalY));
                Console.SetCursorPosition(finalX, finalY);
                Console.Write(graph.Vertices[i].Value);
            }

            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                for(int j = 0; j < graph.Vertices[i].Edges.Count;j++)
                {

                }
            }


        }

        void VisualizeLine(float x1, float y1, float x2, float y2)
        {
            
        }
    }
}
