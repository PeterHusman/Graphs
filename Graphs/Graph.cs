﻿using System;
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
            foreach(Vertex<T> v in Vertices)
            {
                if(v.Value.Equals(val))
                {
                    Remove(v);
                    return;
                }
            }
        }

        public void Remove(Vertex<T> v)
        {
            foreach(Vertex<T> current in Vertices)
            {
                if(current.Edges.ContainsKey(v))
                {
                    current.Edges.Remove(v);
                }
            }
            Vertices.Remove(v);
        }

        public Graph()
        {
            Vertices = new List<Vertex<T>>();
        }
    }
}