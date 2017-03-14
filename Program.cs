
/*
 * Maryam Fadaei
 * October 31, 2016
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Map
{
    internal class Program
    {
        /// <summary>
        /// main program:
        /// get input file as an argument
        /// initial colors
        /// Loead graph with the vertex as per input
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            int color;
            List<int> colors;
            if (args.Length == 1)
            {
                var graph = LoadGraph(args[0], out color);
                colors = GetColorsList(color);
                Map m = new Map();
                m.Graph = graph.OrderByDescending(pair => pair.Value.Count).ToDictionary(d => d.Key, d => d.Value);
                m.Color(colors);
                m.Print();
            }
            else
                Console.WriteLine("Program takes a file path as input");
        }
        /// <summary>
        /// LoadGraph function  creates a dictionary object
        /// read the input file and tokenize the file
        /// read total of node numbers
        /// read number of colors
        /// read vertice and adjacent nodes
        /// </summary>
        /// <param name="path"></param>
        /// <param name="numbersColors"></param>
        /// <returns></returns>
        private static Dictionary<int, List<int>> LoadGraph(string path, out int numbersColors)
        {
            Dictionary<int, List<int>> result = new Dictionary<int, List<int>>();
            int colors = 0;
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                int lineNumber = 0;
                int totalNodes = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace("(", string.Empty);
                    line = line.Replace(")", string.Empty);
                    line = line.Trim();
                    string[] nodes = line.Split(' ');

                    if (lineNumber == 0)
                    {
                        totalNodes = int.Parse(nodes[0]);
                        colors = int.Parse(nodes[1]);
                    }
                    if (lineNumber > 0)
                    {
                        if (nodes.Length > 0 && lineNumber <= totalNodes)
                        {
                            if (!nodes.Contains(string.Empty))
                                result.Add(int.Parse(nodes[0]), nodes.Skip(1).Select(n => int.Parse(n)).ToList());
                            else
                                result.Add(lineNumber, new List<int>());
                        }
                    }
                    lineNumber++;
                }
            }

            numbersColors = colors;
            return result;
        }
        /// <summary>
        /// create a list of the colors
        /// </summary>
        /// <param name="numberOfcolors"></param>
        /// <returns></returns>
        private static List<int> GetColorsList(int numberOfcolors)
        {
            List<int> colors = new List<int>();
            for (int i = 1; i <= numberOfcolors; i++)
                colors.Add(i);

            return colors;
        }
    }
}
