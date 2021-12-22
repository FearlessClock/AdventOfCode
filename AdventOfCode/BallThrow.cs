using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class BallThrow
    {
        static int versionCount = 0;
        static string oppertations = "";
        static int offset = 200;
        static void Mains(string[] args)
        {
            //string[] lines = System.IO.File.ReadAllLines(@"E:\Projects\AdventOfCode\Day1\AdventOfCode\adventOfCode1.txt");
            //x=150..171, y=-129..-70
            //Vector targetUpperLeft = new Vector(20, -5);
            //Vector targetLowerRight = new Vector(30, -10);
            Vector targetUpperLeft = new Vector(150, -70);
            Vector targetLowerRight = new Vector(171, -129);

            List<Vector> init = new List<Vector>();
            for (int j = 150; j >= -130; j--)
            {
                Console.WriteLine(j);
                for (int i = 200; i >= 0; i--)
                {
                    bool res = SimulateShot(targetUpperLeft, targetLowerRight, i, j, 500);
                    //Console.SetCursorPosition(0, Console.BufferHeight - 1);
                    if (res)
                    {
                        init.Add(new Vector(i, j));
                    }
                }
            }
            Console.WriteLine(init.Count);
        }

        private static bool SimulateShot(Vector targetUpperLeft, Vector targetLowerRight, int velX, int velY, int steps)
        {
            Console.SetBufferSize(300, 9000);
            Vector probePos = new Vector(0, 0);
            int highestY = 0;
            List<Vector> positions = new List<Vector>();
            for (int i = 0; i < steps; i++)
            {
                probePos.x = probePos.x + velX;
                if (velX > 0)
                {
                    velX -= 1;
                }
                else if (velX < 0)
                {
                    velX += 1;
                }

                probePos.y = probePos.y + velY;
                if (highestY < probePos.y)
                {
                    highestY = (int)probePos.y;
                }
                velY -= 1;
                positions.Add(new Vector(probePos.x, probePos.y));
                if (Console.BufferWidth > (int)probePos.x && Console.BufferHeight > (int)probePos.y + offset && (int)probePos.y + offset > 0)
                {
                    bool res = IsInsideTarget(targetUpperLeft, targetLowerRight, (int)probePos.x, (int)probePos.y);
                    if (res)
                    {
                        //Console.Clear();
                        //for (int k = (int)targetUpperLeft.x; k < targetLowerRight.x; k++)
                        //{
                        //    for (int j = (int)targetLowerRight.y; j < targetUpperLeft.y; j++)
                        //    {
                        //        if (k == (int)targetUpperLeft.x || k == targetLowerRight.x - 1)
                        //        {
                        //            Console.SetCursorPosition(k, Console.BufferHeight - (j + offset));
                        //            Console.Write("O");
                        //        }
                        //    }
                        //}

                        //Console.SetCursorPosition((int)probePos.x, Console.BufferHeight - ((int)probePos.y + offset));
                        //Console.Write("#");

                        //for (int j = 0; j < positions.Count; j++)
                        //{
                        //    Console.BackgroundColor = res ? ConsoleColor.Green : ConsoleColor.Red;
                        //    Console.SetCursorPosition((int)positions[j].x, Console.BufferHeight - ((int)positions[j].y + offset));
                        //    Console.Write("X");
                        //    Console.BackgroundColor = ConsoleColor.Black;
                        //}
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.Write(highestY);
                        Console.BackgroundColor = ConsoleColor.Black;
                        return res;
                    }
                }
            }
            return false;
        }

        private static bool IsInsideTarget(Vector targetUpperLeft, Vector targetLowerRight, int x, int y)
        {
            return x >= targetUpperLeft.x && x <= targetLowerRight.x &&
                    y >= targetLowerRight.y && y <= targetUpperLeft.y;
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
