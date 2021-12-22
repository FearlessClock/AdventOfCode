using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class aStar
    {
        static void Mains(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"E:\Projects\AdventOfCode\Day1\AdventOfCode\adventOfCode1.txt");
            Node[,] grid = new Node[lines[0].Length * 5, lines.Length * 5];
            for (int offX = 0; offX < 5; offX++)
            {
                for (int offY = 0; offY < 5; offY++)
                {
                    for (int i = offX * lines.Length; i < (offX + 1) * lines.Length; i++)
                    {
                        for (int j = offY * lines[0].Length; j < (offY + 1) * lines[0].Length; j++)
                        {
                            int value = Int32.Parse(lines[i % lines[0].Length][j % lines.Length].ToString());
                            value += 1 * offX + 1 * offY;
                            value = value % 9;
                            if (value == 0)
                            {
                                value = 9;
                            }
                            grid[i, j] = new Node(i + " " + j, false, new Vector(i, j), value);
                        }
                    }
                }
            }
            List<Node> open = new List<Node>();
            List<Node> closed = new List<Node>();
            Vector goal = new Vector(grid.GetLength(0) - 1, grid.GetLength(1) - 1);
            grid[0, 0].g = 0;
            grid[0, 0].f = CalculateDistance(new Vector(0, 0), goal);

            open.Add(grid[0, 0]);

            while (open.Count > 0)
            {
                double lowestF = open[0].f;
                int index = 0;
                for (int i = 0; i < open.Count; i++)
                {
                    if (open[i].f < lowestF)
                    {
                        index = i;
                        lowestF = open[i].f;
                    }
                }
                Node current = open[index];
                if (current.pos.x == goal.x && current.pos.y == goal.y)
                {
                    Stack<Vector> path = ReconstructPath(current, goal, grid);
                    //for (int i = 0; i < grid.GetLength(0); i++)
                    //{
                    //    for (int h = 0; h < grid.GetLength(1); h++)
                    //    {
                    //        Console.Write(grid[i, h].weight);
                    //    }
                    //    Console.WriteLine();
                    //}
                    int sum = 0;
                    while (path.Count > 0)
                    {
                        Vector step = path.Pop();
                        //Console.SetCursorPosition((int)step.x, (int)step.y);
                        //Console.BackgroundColor = ConsoleColor.Green;
                        //Console.Write(grid[(int)step.x, (int)step.y].weight);
                        sum += grid[(int)step.x, (int)step.y].weight;
                        //Console.BackgroundColor = ConsoleColor.Black;

                    }
                    Console.SetCursorPosition(grid.GetLength(0), grid.GetLength(1));
                    Console.WriteLine(sum);
                    Console.ReadKey();
                }

                open.RemoveAt(index);
                Node[] neighbors = GetGridNeighbors(current, grid);
                for (int i = 0; i < neighbors.Length; i++)
                {
                    double newGscore = current.g + neighbors[i].weight;
                    if (newGscore < neighbors[i].g)
                    {
                        neighbors[i].parent = current;
                        neighbors[i].g = newGscore;
                        neighbors[i].f = newGscore + CalculateDistance(neighbors[i].pos, goal);
                        if (!open.Contains(neighbors[i]))
                        {
                            open.Add(neighbors[i]);
                        }
                    }
                }
            }

        }

        private static double CalculateDistance(Vector current, Vector goal)
        {
            Vector vec = new Vector(goal.x - current.x, goal.y - current.y);
            return Math.Sqrt(vec.x * vec.x + vec.y * vec.y);
        }

        private static Node[] GetGridNeighbors(Node current, Node[,] grid)
        {
            Vector[] neighborsDirs = new Vector[4] { new Vector(-1, 0), new Vector(0, 1), new Vector(0, -1), new Vector(1, 0) };
            List<Node> neighbors = new List<Node>();
            for (int i = 0; i < neighborsDirs.Length; i++)
            {
                if (current.pos.x + neighborsDirs[i].x >= 0 && current.pos.y + neighborsDirs[i].y >= 0 &&
                    current.pos.x + neighborsDirs[i].x < grid.GetLength(0) && current.pos.y + neighborsDirs[i].y < grid.GetLength(1))
                {
                    neighbors.Add(grid[(int)(current.pos.x + neighborsDirs[i].x), (int)(current.pos.y + neighborsDirs[i].y)]);
                }
            }
            return neighbors.ToArray();
        }

        static private Stack<Vector> ReconstructPath(Node current, Vector goal, Node[,] grid)
        {
            Stack<Vector> path = new Stack<Vector>();
            while (current.parent != null)
            {
                path.Push(current.pos);
                current = current.parent;
            }
            return path;
        }

        class Node
        {
            public string name;
            public bool hasBeenVisited = false;
            public double g = 0;
            public double f = 0;
            public int weight = 0;
            public Vector pos;
            public Node parent;

            public Node(string name, bool hasBeenVisited, Vector pos, int weight)
            {
                this.name = name;
                this.hasBeenVisited = hasBeenVisited;
                this.pos = pos;
                g = float.MaxValue;
                f = float.MaxValue;
                this.weight = weight;
            }

            public override string ToString()
            {
                return name;
            }

            public override bool Equals(object obj)
            {
                Node node = (Node)obj;

                return this.pos.Equals(node.pos);
            }
        }

        class Vector
        {
            public double x;
            public double y;

            public Vector(double x, double y)
            {
                this.x = x;
                this.y = y;
            }

            public override bool Equals(object obj)
            {
                Vector vec = (Vector)obj;
                return vec.x == x && vec.y == y;
            }

            public override string ToString()
            {
                return x + " : " + y;
            }
        }
        class Map
        {
            public int[,] grid;

            public int Width => grid.GetLength(0);
            public int Height => grid.GetLength(1);

            public Map(int x, int y)
            {
                grid = new int[x, y];

            }

            public void FillInHeightLine(string heightLine, int y)
            {
                for (int i = 0; i < heightLine.Length; i++)
                {
                    grid[i, y] = Int32.Parse(heightLine[i].ToString());
                }
            }

            public void DrawGrid()
            {
                for (int i = 0; i < grid.GetLength(1); i++)
                {
                    for (int l = 0; l < grid.GetLength(0); l++)
                    {
                        Console.Write(grid[l, i] == 0 ? "." : "#");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine();
            }

            public int CountLowest()
            {
                int sum = 0;
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int l = 0; l < grid.GetLength(1); l++)
                    {
                        if (grid[i, l] >= 2)
                        {
                            sum++;
                        }
                    }
                    Console.WriteLine("");
                }
                return sum;
            }

            public int GetPoint(int i, int j)
            {
                return grid[i, j];
            }

            internal int GetPoint(Vector vector)
            {
                return grid[(int)vector.x, (int)vector.y];
            }

            public Vector[] GetNeighborPositions(Vector pos)
            {
                List<Vector> vecs = new List<Vector>();
                Vector[] dirs = new Vector[8] { new Vector(1,0),new Vector(-1,0),new Vector(0,1),new Vector(0,-1),
                                            new Vector(1,1),new Vector(1,-1),new Vector(-1,1),new Vector(-1,-1)};
                for (int i = 0; i < dirs.Length; i++)
                {
                    if (pos.x + dirs[i].x < this.Width && pos.x + dirs[i].x >= 0 &&
                        pos.y + dirs[i].y < this.Height && pos.y + dirs[i].y >= 0)
                    {
                        vecs.Add(new Vector(pos.x + dirs[i].x, pos.y + dirs[i].y));
                    }
                }
                return vecs.ToArray();
            }

            public void FillIn(Vector pos)
            {
                grid[(int)pos.x, (int)pos.y] = 1;
            }
        }
        class Basin
        {
            int startX;
            int startY;
            Map map;
            public int basinSize = 1;
            List<Vector> openPositions = new List<Vector>();
            List<Vector> closedPositions = new List<Vector>();

            public Basin(int x, int y, Map map, List<Vector> globalClosedList)
            {
                this.map = map;
                startX = x;
                startY = y;
                if (!globalClosedList.Contains(new Vector(x, y)))
                {
                    openPositions.Add(new Vector(x, y));
                    FillBasin();
                }
            }

            private void FillBasin()
            {
                while (openPositions.Count > 0)
                {
                    Vector[] vecs = GetNeighborPositions(openPositions[0]);
                    closedPositions.Add(openPositions[0]);
                    openPositions.RemoveAt(0);
                    for (int i = 0; i < vecs.Length; i++)
                    {
                        if (map.GetPoint(vecs[i]) != 9)
                        {
                            if (!closedPositions.Contains(vecs[i]))
                            {
                                if (!openPositions.Contains(vecs[i]))
                                {
                                    basinSize++;
                                    openPositions.Add(vecs[i]);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Result " + basinSize);
            }

            private Vector[] GetNeighborPositions(Vector pos)
            {
                List<Vector> vecs = new List<Vector>();
                if (pos.x + 1 < map.Width)
                {
                    vecs.Add(new Vector(pos.x + 1, pos.y));
                }
                if (pos.y + 1 < map.Height)
                {
                    vecs.Add(new Vector(pos.x, pos.y + 1));
                }
                if (pos.x - 1 >= 0)
                {
                    vecs.Add(new Vector(pos.x - 1, pos.y));
                }
                if (pos.y - 1 >= 0)
                {
                    vecs.Add(new Vector(pos.x, pos.y - 1));
                }
                return vecs.ToArray();
            }
        }

        class Command
        {
            public bool isVertical = false;
            public int pos = 0;

            public Command(bool isVert, int position)
            {
                isVertical = isVert;
                pos = position;
            }
        }
    }
}
