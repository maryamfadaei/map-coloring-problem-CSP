
/*
 * Maryam Fadaei
 * October 31, 2016
 */
 
using System;
using System.Collections.Generic;
using System.Linq;

namespace Map
{
    /// <summary>
    /// Class Map
    /// class memebrs
    /// </summary>
    public class Map
    {
        private List<int> _Visited = new List<int>();

        public Dictionary<int, List<int>> Graph { get; set; }

        public int[] NodeDegrees { get; set; }

        public int[] ColoredMap { get; set; }

        private int _NumberOfColors;
        /// <summary>
        /// Color method
        /// create an array with size of graph count + 1 (diffrent use for array[0])
        /// sort array in order high degree to low
        /// until there is node in the graph  and node is not visited call DFSsearch
        /// intitialize nodedegree
        /// </summary>
        /// <param name="colors"></param>
        public void Color(IEnumerable<int> colors)
        {
            ColoredMap = new int[Graph.Count + 1];
            _NumberOfColors = colors.Count();

            NodeDegrees = Graph.OrderBy(k => k.Key).Select(s => s.Value.Count).ToArray();

            foreach (int key in Graph.Keys)
            {
                if (!_Visited.Contains(key))
                    DFSsearch(key, colors);
            }
        }
        /// <summary>
        /// print method
        /// print the result on console
        /// </summary>
        public void Print()
        {
            int Notcolored = ColoredMap.Where(c => c == 0).Count();

            if (Notcolored > 1)
            {
                Console.WriteLine("Cannot Color");
                return;
            }

            for (int item = 1; item < ColoredMap.Length; item++)
            {
                Console.WriteLine("Node:{0}- Color: {1}", item, ColoredMap[item]);
            }
        }
        /// <summary>
        /// DFSsearch method
        /// pick up a color for a node by calling GetLCV
        /// Usr MRV + DH to chose a next node to color
        /// MRV and DH are similar algorithm, check the degree of the node and select the node with larger degree to expand
        /// DH isusful when two nodes have same value in MRV
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="colors"></param>
        private void DFSsearch(int vertex, IEnumerable<int> colors)
        {
            _Visited.Add(vertex);

            ColoredMap[vertex] = GetLCV(vertex, colors);

            if (Graph[vertex] != null)
            {
                //MRV+Degree
                var adjList = GetMRV(vertex).OrderByDescending(v => GetDegree(v));

                foreach (int next in adjList)
                {
                    if (!_Visited.Contains(next))
                    {
                        DFSsearch(next, colors);
                    }
                }
            }
        }
        /// <summary>
        /// Cancolor method
        /// checks if the color is acceptable
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private bool CanColor(int vertex, int color)
        {
            if (Graph[vertex] == null)
                return true;

            foreach (int adjCountry in Graph[vertex])
            {
                if (ColoredMap[adjCountry] == color)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Getdegree method
		/// Degree heuristic (how many other variables are affected by this variable)
        /// returns the degree of the node. which is the number of the edges in the nodedegree
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        private int GetDegree(int vertex)
        {
            return NodeDegrees[vertex - 1];
        }
        /// <summary>
        /// GetMRV method
        /// Minimum-remaining-values (how many values are still valid for this variable)
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        private List<int> GetMRV(int vertex)
        {
            Dictionary<int, int> mrvs = new Dictionary<int, int>();

            foreach (int node in Graph[vertex])
            {
                int mrv = _NumberOfColors;
                foreach (int adj in Graph[node])
                {
                    if (ColoredMap[adj] != 0)
                        mrv--;
                }
                if (!mrvs.ContainsKey(node))
                    mrvs.Add(node, mrv);
            }

            return mrvs.OrderBy(s => s.Value)
                .ToDictionary(d => d.Key, d => d.Value)
                .Keys.ToList();
        }
        /// <summary>
        /// GetLCV method
		/// Least-constraining-value (what value will leave the most other values for other variables)
        /// for every color, initial total number to zero
        /// count the total number of usage of each color in graph
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="colors"></param>
        /// <returns></returns>
        private int GetLCV(int vertex, IEnumerable<int> colors)
        {
            Dictionary<int, int> lcvs = new Dictionary<int, int>();

            foreach (int color in colors)
            {
                int total = 0;
                bool isSafeColor = true;
                foreach (int node in Graph[vertex])
                {
                    if (ColoredMap[node] == color)
                    {
                        isSafeColor = false;
                        break;
                    }

                    foreach (int adj in Graph[node].Where(a => a != vertex))
                    {
                        if (ColoredMap[adj] == color)
                            total++;
                    }
                }
                if (isSafeColor)
                    lcvs.Add(color, total);
            }

            return lcvs.OrderByDescending(v => v.Value)
                .Select(k => k.Key)
                .FirstOrDefault();
        }
    }
}
