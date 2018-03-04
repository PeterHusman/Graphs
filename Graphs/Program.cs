using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    class Program
    {
        static void Main(string[] args)
        {
            string prgrmChooser = Console.ReadLine();
            if (prgrmChooser == "a*")
            {
                Console.Clear();
                Graph<Vector2> graph = new Graph<Vector2>();
                string file = File.ReadAllText("AStarEnvironment.txt");
                Dictionary<Vector2, Vertex<Vector2>> vertices = new Dictionary<Vector2, Vertex<Vector2>>();
                Vector2 pos = Vector2.Zero;
                Vector2 startPos = Vector2.Zero;
                Vector2 endPos = Vector2.Zero;
                for (int i = 0; i < file.Length; i++)
                {
                    if (file[i] == ' ' || file[i] == 'S' || file[i] == 'E')
                    {
                        graph.Add(pos);
                        vertices.Add(pos, graph.Vertices[graph.Vertices.Count - 1]);
                        if (vertices.ContainsKey(new Vector2(pos.X - 1, pos.Y)))
                        {
                            graph.AddEdge(vertices[pos], vertices[new Vector2(pos.X - 1, pos.Y)]);
                        }
                        if (vertices.ContainsKey(new Vector2(pos.X, pos.Y - 1)))
                        {
                            graph.AddEdge(vertices[pos], vertices[new Vector2(pos.X, pos.Y - 1)]);
                        }
                        if (file[i] == 'S')
                        {
                            startPos = pos;
                        }
                        else if (file[i] == 'E')
                        {
                            endPos = pos;
                        }
                    }
                    else if (file[i] == '\n')
                    {
                        pos.Y++;
                        pos.X = -1;
                    }
                    pos.X++;
                }
                Console.WriteLine(file);
                Console.ReadLine();
                Stack<Vertex<Vector2>> path = graph.Pathfind(startPos, endPos, (Vertex<Vector2> a) => { return (endPos.X - a.Value.X) * (endPos.X - a.Value.X) + (endPos.Y - a.Value.Y) * (endPos.Y - a.Value.Y); });
                while (path.Count > 0)
                {
                    Vector2 pos2 = path.Pop().Value;
                    Console.SetCursorPosition((int)pos2.X, (int)pos2.Y);
                    Console.Write('=');
                }
                Console.ReadKey();
            }
            else if (prgrmChooser == "graph")
            {
                bool beenWarned = false;
                int radius = 10;
                Graph<int> graph = new Graph<int>();
                string stuffToWrite = "";
                while (true)
                {
                    stuffToWrite = "";
                    string s = Console.ReadLine();
                    if (s[0] == 'i')
                    {
                        s = s.Remove(0, 1);
                        if (graph.Search(int.Parse(s)) == null)
                        {
                            graph.Add(int.Parse(s));
                        }
                        else if (!beenWarned)
                        {
                            Console.WriteLine("No repeats!");
                            beenWarned = true;
                            continue;
                        }
                        else
                        {
                            throw new Exception("I said no repeats!");
                        }
                    }
                    else if (s[0] == 'd')
                    {
                        s = s.Remove(0, 1);
                        graph.Remove(int.Parse(s));
                    }
                    else if (s[0] == 'e')
                    {
                        s = s.Remove(0, 1);
                        string[] vals = s.Split(';');
                        graph.AddEdge(int.Parse(vals[0]), int.Parse(vals[1]), vals.Length >= 3 ? vals[2] == "d" : false, vals.Length >= 4 ? double.Parse(vals[3]) : 1);
                    }
                    else if (s[0] == 'r')
                    {
                        s = s.Remove(0, 1);
                        string[] vals = s.Split(';');
                        graph.RemoveEdge(int.Parse(vals[0]), int.Parse(vals[1]), vals.Length >= 3 ? vals[2] == "u" : true);
                    }
                    else if (s[0] == 'R')
                    {
                        s = s.Remove(0, 1);
                        radius = int.Parse(s);
                    }
                    else if (s[0] == 's')
                    {
                        s = s.Remove(0, 1);
                        if (s[0] == 'D')
                        {
                            s = s.Remove(0, 1);
                            Vertex<int> v = graph.Search(int.Parse(s), SearchType.DepthFirst);
                            if (v != null) { v.Color = ConsoleColor.Red; }
                        }
                        else if (s[0] == 'B')
                        {
                            s = s.Remove(0, 1);
                            Vertex<int> v = graph.Search(int.Parse(s), SearchType.BreadthFirst);
                            if (v != null) { v.Color = ConsoleColor.Red; }
                        }
                    }
                    else if (s[0] == 'g')
                    {
                        s = s.Remove(0, 1);
                        string[] param = s.Split(';');
                        int[] vals = UniqueRandom(int.Parse(param[0]), int.Parse(param[1]), int.Parse(param[2]));
                        for (int i = 0; i < vals.Length; i++)
                        {
                            graph.Add(vals[i]);
                        }
                        int[] vals2 = UniqueRandom(int.Parse(param[0]), int.Parse(param[1]), int.Parse(param[3]));
                        for (int i = 1; i < vals2.Length; i++)
                        {
                            graph.AddEdge(vals2[i - 1], vals2[i]);
                        }

                    }
                    else if (s[0] == 'P')
                    {
                        s = s.Remove(0, 1);
                        Func<Vertex<int>, double> a = (Vertex<int> c) => { return 0; };
                        Func<Vertex<int>, double> heuristic = s[0] == 'D' ? a : (Vertex<int> b) => { return 0; };
                        graph.Pathfind(new Vertex<int>(), new Vertex<int>(), a);
                        s = s.Remove(0, 1);
                        string[] stringVals = s.Split(';');
                        int[] vals = { int.Parse(stringVals[0]), int.Parse(stringVals[1]) };
                        Stack<Vertex<int>> path = graph.Pathfind(vals[0], vals[1]);
                        while (path.Count > 0)
                        {
                            stuffToWrite += path.Pop().Value.ToString() + (path.Count > 0 ? ", " : "");
                        }
                    }
                    else if (s[0] == 'p')
                    {
                        s = s.Remove(0, 1);
                        int id = int.Parse(s);
                        if (id == 1)
                        {
                            graph.Vertices = new List<Vertex<int>>();
                            for (int i = 0; i < 6; i++)
                            {
                                graph.Add(i);
                            }
                            graph.AddEdge(0, 1, true, 9);
                            graph.AddEdge(1, 2, true, 2);
                            graph.AddEdge(0, 3, true, 8);
                            graph.AddEdge(3, 4, true, 2);
                            graph.AddEdge(3, 5, true, 16);
                            graph.AddEdge(4, 5, true, 4);
                        }

                    }

                    Visualize(graph, radius);
                    Console.SetCursorPosition(0, 0);
                    Console.Write(stuffToWrite);

                }
            }

        }

        static int[] UniqueRandom(int min, int max, int number)
        {
            Random rand = new Random();
            int[] nums = new int[number];

            for (int i = 0; i < number; i++)
            {
                int val = 0;
                do
                {
                    val = rand.Next(min, max);
                } while (nums.Contains(val));
                nums[i] = val;
            }
            return nums;
        }

        static void Visualize(Graph<int> graph, int radius)
        {
            int border = 2;
            Dictionary<Vertex<int>, Point> positions = new Dictionary<Vertex<int>, Point>();
            Console.Clear();
            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                double x = Math.Cos(2 * Math.PI * i / graph.Vertices.Count);
                double y = Math.Sin(2 * Math.PI * i / graph.Vertices.Count);
                int finalX = (int)(x * radius + radius + border);
                int finalY = (int)(y * radius + radius + border);
                positions.Add(graph.Vertices[i], new Point(finalX, finalY));
            }

            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                for (int j = 0; j < graph.Vertices[i].Edges.Count; j++)
                {
                    VisualizeLine(positions[graph.Vertices[i]].X, positions[graph.Vertices[i]].Y, positions[graph.Vertices[i].Edges.Keys.ToArray<Vertex<int>>()[j]].X, positions[graph.Vertices[i].Edges.Keys.ToArray<Vertex<int>>()[j]].Y, !graph.Vertices[i].Edges.Keys.ToArray()[j].Edges.ContainsKey(graph.Vertices[i]), graph.Vertices[i].Edges.Values.ToArray()[j]);
                }
            }

            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                Console.SetCursorPosition(positions[graph.Vertices[i]].X, positions[graph.Vertices[i]].Y);
                Console.ForegroundColor = graph.Vertices[i].Color;
                Console.Write(graph.Vertices[i].Value);
                graph.Vertices[i].Color = ConsoleColor.White;
            }


        }

        static void VisualizeLine(float x1, float y1, float x2, float y2, bool directed, double weight)
        {
            float x = 0;
            float y = 0;
            int iterations = 40;
            for (int i = 0; i <= iterations; i++)
            {
                x = MathHelper.Lerp(x1, x2, i / (float)iterations);
                y = MathHelper.Lerp(y1, y2, i / (float)iterations);
                Console.SetCursorPosition((int)x, (int)y);
                if (i > iterations / 2 || !directed)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                Console.Write("█");
                if (i == iterations / 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(weight.ToString());
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
