using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    class Program
    {

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"E:\Projects\AdventOfCode\Day1\AdventOfCode\adventOfCode1.txt");
            while(true)
            {
                string line = Console.ReadLine();
                //[9,[8,7]]
                Pair pair1 = GeneratePair(line);
                //line = Console.ReadLine();
                //Pair pair2 = GeneratePair(line);
                //Pair res = Add(pair1, pair2);
                pair1 = Reduce(pair1, 0);
                Console.WriteLine(pair1.ToString());
            }
        }

        private static Pair GeneratePair(string line)
        {
            Pair currentPair = null;
            for (int k = 0; k < line.Length; k++)
            {
                if (line[k] == '[')
                {
                    if (currentPair == null)
                    {
                        currentPair = new Pair();
                    }
                    else
                    {
                        if (!currentPair.hasSeenComma)
                        {
                            currentPair.pairLeft = new Pair();
                            currentPair.pairLeft.parent = currentPair;
                            currentPair = currentPair.pairLeft;

                        }
                        else
                        {
                            currentPair.pairRight = new Pair();
                            currentPair.pairRight.parent = currentPair;
                            currentPair = currentPair.pairRight;
                        }
                    }
                }
                else if (line[k] == ']')
                {
                    if (currentPair.parent != null)
                    {
                        currentPair = currentPair.parent;
                    }
                }
                else if (line[k] == ',')
                {
                    currentPair.hasSeenComma = true;
                }
                else
                {
                    if (!currentPair.hasSeenComma)
                    {
                        currentPair.valueLeft = Int32.Parse(line[k].ToString());
                    }
                    else
                    {
                        currentPair.valueRight = Int32.Parse(line[k].ToString());
                    }
                }
            }

            return currentPair;
        }

        static Pair Add(Pair pair1, Pair pair2)
        {
            Pair newPair = new Pair();
            newPair.pairLeft = pair1;
            pair1.parent = newPair;
            newPair.pairRight = pair2;
            pair2.parent = newPair;
            return newPair;
        }

        static Pair Reduce(Pair pair, int depth)
        {
            if (depth >= 4)
            {
                Pair currentPair = pair;
                Pair parent = currentPair.parent;
                //Check left side
                bool hasFoundRegularLeft = parent.pairLeft == null;
                while (parent != null)
                {
                    if (parent.pairLeft == null)
                    {
                        parent.valueLeft += currentPair.valueLeft;
                        break;
                    }
                    parent = parent.parent;
                }
                parent = currentPair.parent;
                //Check right side
                bool hasFoundRegularRight = parent.pairRight == null;
                while (parent != null)
                {
                    if(parent.pairRight == null)
                    {
                        parent.valueRight += currentPair.valueRight;
                        break;
                    }
                    parent = parent.parent;
                }
                parent = currentPair.parent;
                if (!hasFoundRegularLeft)
                {
                    currentPair.parent.pairLeft = null;
                    currentPair.parent.valueLeft = 0;

                }
                if (!hasFoundRegularRight)
                {
                    currentPair.parent.pairRight = null;
                    currentPair.parent.valueRight = 0;

                }

                Console.WriteLine(parent.ToString());
                return null;
            }
            if (pair.pairLeft != null)
            {
                pair.pairLeft = Reduce(pair.pairLeft, depth + 1) ;
            }
            if(pair.pairRight != null)
            {
                pair.pairRight = Reduce(pair.pairRight, depth + 1);
            }
            return pair;
        }

        class Pair
        {
            public Pair parent = null;
            public bool hasFilledLeft = false;
            public bool hasSeenComma = false;
            public Pair pairLeft;
            public Pair pairRight;
            public int valueLeft = -1;
            public int valueRight = -1;

            public Pair()
            {

            }

            public Pair(Pair left, Pair right)
            {
                pairLeft = left;
                pairRight = right;
            }

            public override string ToString()
            {
                string res = "[" + (pairLeft != null ? pairLeft.ToString() : valueLeft) + "," + (pairRight != null ? pairRight.ToString() : valueRight) + "]";
                return res;
            }
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
