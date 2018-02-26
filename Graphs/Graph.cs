using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    public class Graph<T>
    {
        public List<Vertex<T>> Vertices;

        public void Add(T val)
        {
            Add(val, new Dictionary<Vertex<T>, double>());
        }

        public void Add(T val, params Vertex<T>[] edges)
        {
            Dictionary<Vertex<T>, double> connections = new Dictionary<Vertex<T>, double>();
            foreach (Vertex<T> v in edges)
            {
                connections.Add(v, 1);
            }
            Add(val, connections);

        }

        public void Add(T val, Dictionary<Vertex<T>, double> edges)
        {
            Vertices.Add(new Vertex<T>(val, edges));
        }

        public void Remove(T val)
        {
            foreach (Vertex<T> v in Vertices)
            {
                if (v.Value.Equals(val))
                {
                    Remove(v);
                    return;
                }
            }
        }

        public bool ContainsValue(T val)
        {
            return Search(val) == null;
        }

        public void AddEdge(Vertex<T> a, Vertex<T> b, bool directed = false, double weight = 1)
        {
            a.Edges.Add(b, weight);
            if (directed) { return; }
            b.Edges.Add(a, weight);
        }

        public void AddEdge(T a, T b, bool directed = false, double weight = 1)
        {
            AddEdge(Search(a, SearchType.ListSearch), Search(b, SearchType.ListSearch), directed, weight);
        }

        /// <summary>
        /// A* algorithm. If heuristic returns 0, it is equivalent to Dijkstra's algorithm.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="heuristic"></param>
        /// <returns></returns>
        public Stack<Vertex<T>> Pathfind(T start, T end, Func<Vertex<T>, double> heuristic)
        {
            return Pathfind(Search(start), Search(end), heuristic);
        }


        /// <summary>
        /// Dijkstra's algorithm
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Stack<Vertex<T>> Pathfind(T start, T end)
        {
            return Pathfind(Search(start), Search(end), (Vertex<T> a) => { return 0; });
        }


        /// <summary>
        /// Dijkstra's algorithm
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Stack<Vertex<T>> Pathfind(Vertex<T> start, Vertex<T> end)
        {
            return Pathfind(start, end, (Vertex<T> a) => { return 0; } );
        }


        /// <summary>
        /// A* algorithm. If heuristic returns 0, it is equivalent to Dijkstra's algorithm.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type"></param>
        /// <param name="heuristic"></param>
        /// <returns></returns>
        public Stack<Vertex<T>> Pathfind(Vertex<T> start, Vertex<T> end, Func<Vertex<T>, double> heuristic)
        {
            InitializeCosts();
            start.Cost = 0;
            List<Vertex<T>> verticesToUse = new List<Vertex<T>> { start };
            List<Vertex<T>> visited = new List<Vertex<T>>();
            while (true)
            {
                Vertex<T> lowestCost = new Vertex<T>();
                lowestCost.Cost = double.PositiveInfinity;
                foreach (Vertex<T> v in verticesToUse)
                {
                    if (v.Cost < lowestCost.Cost)
                    {
                        lowestCost = v;
                    }
                }
                if (lowestCost.Value.Equals(end.Value))
                {
                    Vertex<T> current = end;
                    Stack<Vertex<T>> path = new Stack<Vertex<T>>();
                    do
                    {
                        path.Push(current);
                        current = current.LastVisited;
                    } while (current != start);
                    path.Push(start);
                    return path;
                }
                else
                {
                    visited.Add(lowestCost);
                    foreach (KeyValuePair<Vertex<T>, double> edge in lowestCost.Edges)
                    {
                        if (!visited.Contains(edge.Key))
                        {
                            edge.Key.Cost = lowestCost.Cost + edge.Value + heuristic(edge.Key);
                            edge.Key.LastVisited = lowestCost;
                            verticesToUse.Add(edge.Key);
                        }
                    }
                    verticesToUse.Remove(lowestCost);
                }
            }
        }

        void InitializeCosts()
        {
            foreach (Vertex<T> v in Vertices)
            {
                v.Cost = -1;
            }
        }


        public void RemoveEdge(Vertex<T> a, Vertex<T> b, bool bothDirections)
        {
            a.Edges.Remove(b);
            if (!bothDirections) { return; }
            b.Edges.Remove(a);
        }

        public void RemoveEdge(T a, T b, bool bothDirections)
        {
            RemoveEdge(Search(a, SearchType.ListSearch), Search(b, SearchType.ListSearch), bothDirections);
        }

        public void Remove(Vertex<T> v)
        {
            foreach (Vertex<T> current in Vertices)
            {
                if (current.Edges.ContainsKey(v))
                {
                    current.Edges.Remove(v);
                }
            }
            Vertices.Remove(v);
        }

        public Vertex<T> Search(T val, SearchType searchType = SearchType.BreadthFirst)
        {
            if (Vertices.Count == 0) { return null; }
            if (searchType == SearchType.BreadthFirst)
            {
                return BreadthFirst(val);
            }
            else if (searchType == SearchType.DepthFirst)
            {
                return DepthFirst(val);
            }
            else
            {
                foreach (Vertex<T> v in Vertices)
                {
                    if (v.Value.Equals(val))
                    {
                        return v;
                    }
                }
                return null;
            }
        }


        public Vertex<T> DepthFirst(Vertex<T> start, T end)
        {
            if (Vertices.Count == 0) { return null; }
            HashSet<Vertex<T>> visited = new HashSet<Vertex<T>>();
            Stack<Vertex<T>> stack = new Stack<Vertex<T>>();
            stack.Push(start);
            if (start.Value.Equals(end))
            {
                return start;
            }
            while (stack.Count > 0)
            {
                Vertex<T> vert = stack.Pop();
                visited.Add(vert);
                foreach (Vertex<T> nextToAdd in vert.Edges.Keys)
                {
                    if (nextToAdd.Value.Equals(end))
                    {
                        return nextToAdd;
                    }
                    if (!visited.Contains(nextToAdd))
                    {
                        stack.Push(nextToAdd);
                    }
                }
            }
            return null;
        }

        public Vertex<T> DepthFirst(T end)
        {
            if (Vertices.Count == 0) { return null; }
            return DepthFirst(Vertices[0], end);
        }

        public Vertex<T> BreadthFirst(Vertex<T> start, T end)
        {
            if (Vertices.Count == 0) { return null; }
            HashSet<Vertex<T>> visited = new HashSet<Vertex<T>>();
            Queue<Vertex<T>> queue = new Queue<Vertex<T>>();
            queue.Enqueue(start);
            if (start.Value.Equals(end))
            {
                return start;
            }
            while (queue.Count > 0)
            {
                Vertex<T> vert = queue.Dequeue();
                visited.Add(vert);
                foreach (Vertex<T> nextToAdd in vert.Edges.Keys)
                {
                    if (nextToAdd.Value.Equals(end))
                    {
                        return nextToAdd;
                    }
                    if (!visited.Contains(nextToAdd))
                    {
                        queue.Enqueue(nextToAdd);
                    }
                }
            }
            return null;
        }

        public Vertex<T> BreadthFirst(T end)
        {
            if (Vertices.Count == 0) { return null; }
            return BreadthFirst(Vertices[0], end);
        }

        public Graph()
        {
            Vertices = new List<Vertex<T>>();
        }
    }

    public enum SearchType
    {
        DepthFirst = 0,
        BreadthFirst = 1,
        ListSearch = 2
    }
}
