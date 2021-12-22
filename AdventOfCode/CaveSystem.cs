using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class CaveSystem
    {
        static void Mains(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"E:\Projects\AdventOfCode\Day1\AdventOfCode\adventOfCode1.txt");
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            // Create all the nodes needed
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split('-');
                AddNode(nodes, line[0]);
                AddNode(nodes, line[1]);
            }
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split('-');
                ConnectNodes(nodes, line[0], line[1]);
            }
            List<string> smallCaves = new List<string>();
            foreach (string item in nodes.Keys)
            {
                if (!nodes[item].isBig && nodes[item].name != "start" && nodes[item].name != "end")
                {
                    smallCaves.Add(item);
                    Console.Write(item + " : ");
                }
            }
            Console.WriteLine();
            WalkDownCave(nodes["start"], smallCaves.ToArray());

            foreach (string item in nodes.Keys)
            {
                Console.Write(item + " : ");
                for (int i = 0; i < nodes[item].links.Count; i++)
                {
                    Console.Write(nodes[item].links[i].name + ", ");
                }
                Console.WriteLine();
            }

            ResetVisitedNodes(nodes);

            Console.ReadKey();
        }

        private static void WalkDownCave(Node node, string[] smallCaves)
        {
            string nodesVisited = "";
            List<string> smallNodes = new List<string>();
            Node currentNode = node;
            nodesVisited += currentNode.name;
            Console.WriteLine(WalkDownCave(currentNode, "", 0, smallCaves));
        }

        private static int WalkDownCave(Node currentNode, string nodesVisited, int numberOfBranches, string[] smallCaves)
        {
            nodesVisited += currentNode.name + ",";
            if (currentNode.name != "end")
            {
                for (int i = 0; i < currentNode.links.Count; i++)
                {
                    if (!currentNode.links[i].isBig && nodesVisited.Contains(currentNode.links[i].name))
                    {
                        if (currentNode.links[i].name == "start" || currentNode.links[i].name == "start")
                        {
                            continue;
                        }
                        bool canVisit = false;
                        for (int j = 0; j < smallCaves.Length; j++)
                        {
                            if (nodesVisited.Contains(smallCaves[j]))
                            {
                                string source = nodesVisited;
                                int count = source.Length - source.Replace(smallCaves[j], "").Length;
                                if (count == 2 * smallCaves[j].Length)
                                {
                                    canVisit = true;
                                    break;
                                }
                            }
                        }
                        if (canVisit)
                        {
                            continue;
                        }
                    }
                    numberOfBranches = WalkDownCave(currentNode.links[i], nodesVisited, numberOfBranches, smallCaves);
                }
            }
            else
            {
                Console.WriteLine(nodesVisited);
                numberOfBranches++;
            }
            return numberOfBranches;
        }

        private static void ConnectNodes(Dictionary<string, Node> nodes, string node1, string node2)
        {
            nodes[node1].AddLink(nodes[node2]);
            nodes[node2].AddLink(nodes[node1]);
        }

        private static void ResetVisitedNodes(Dictionary<string, Node> nodes)
        {
            foreach (string item in nodes.Keys)
            {
                for (int i = 0; i < nodes[item].links.Count; i++)
                {
                    nodes[item].hasBeenVisited = false;
                }
            }
        }

        private static void AddNode(Dictionary<string, Node> nodes, string line)
        {
            bool isBig = true;
            for (int k = 0; k < line.Length; k++)
            {
                if (Char.IsLower(line[k]))
                {
                    isBig = false;
                    break;
                }
            }
            if (!nodes.ContainsKey(line))
            {
                nodes.Add(line, new Node(line, isBig));
            }
        }

        class Node
        {
            public string name;
            public bool isBig;
            public List<Node> links = new List<Node>();
            public bool hasBeenVisited = false;

            public Node(string name, bool isBig)
            {
                this.name = name;
                this.isBig = isBig;
            }

            public void AddLink(Node linkedTo)
            {
                if (!links.Contains(linkedTo))
                {
                    links.Add(linkedTo);
                }
            }

            public override bool Equals(object obj)
            {
                Node node = (Node)obj;

                return this.name.Equals(node.name);
            }
        }
    }
}
