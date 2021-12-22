using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class SevenSegment
    {
        static string middleBottomChars = "";
        static void Mains(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"E:\Projects\AdventOfCode\Day1\AdventOfCode\adventOfCode1.txt");
            int sum = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                Dictionary<int, string> results = new Dictionary<int, string>();
                List<string> segments = new List<string>();
                string[] parts = lines[i].Split(" | ");
                string[] outputs = parts[0].Split(" ");
                for (int j = 0; j < outputs.Length; j++)
                {
                    segments.Add(outputs[j]);
                }
                outputs = parts[1].Split(" ");
                for (int j = 0; j < outputs.Length; j++)
                {
                    segments.Add(outputs[j]);
                }
                segments.Sort((x, y) => { if (x == y) return 0; else if (x.Length == 2 || x.Length == 3 || x.Length == 4 || x.Length == 7) return -1; else return 1; });
                for (int j = 0; j < segments.Count; j++)
                {
                    Console.Write(DeduceSegment(segments[j], ref results) + " ");
                }
                for (int j = 0; j < segments.Count; j++)
                {
                    Console.Write(DeduceSegment(segments[j], ref results) + " ");
                }
                string res = DeduceSegment(outputs[0], ref results).ToString() + DeduceSegment(outputs[1], ref results).ToString() + DeduceSegment(outputs[2], ref results).ToString() + DeduceSegment(outputs[3], ref results).ToString();
                sum += Int32.Parse(res);
                Console.WriteLine();
                Console.WriteLine(res);
                middleBottomChars = "";
            }
            Console.WriteLine("---- " + sum);

            // Suspend the screen.  
            System.Console.ReadLine();
        }

        static int DeduceSegment(string input, ref Dictionary<int, string> results)
        {
            //0, 2, 3, 5, 6, 9
            //6, 5, 5, 5, 6, 6
            switch (input.Length)
            {
                case 2:
                    if (!results.ContainsKey(1))
                    {
                        results.Add(1, input);
                    }
                    return 1;
                case 3:
                    if (!results.ContainsKey(7))
                    {
                        results.Add(7, input);
                    }
                    return 7;
                case 4:
                    if (!results.ContainsKey(4))
                    {
                        results.Add(4, input);
                    }
                    return 4;
                case 7:
                    if (!results.ContainsKey(8))
                    {
                        results.Add(8, input);
                    }
                    return 8;
                case 6:
                    if (results.ContainsKey(6))
                    {
                        if (ContainsAllLetters(input, results[6]))
                        {
                            return 6;
                        }
                    }
                    if (results.ContainsKey(9))
                    {
                        if (ContainsAllLetters(input, results[9]))
                        {
                            return 9;
                        }
                    }
                    if (results.ContainsKey(0))
                    {
                        if (ContainsAllLetters(input, results[0]))
                        {
                            return 0;
                        }
                    }
                    string one = results[1];
                    if (!results.ContainsKey(6) && (input.Contains(one[0]) && !input.Contains(one[1])) || (!input.Contains(one[0]) && input.Contains(one[1])))
                    {
                        results.Add(6, input);
                        return 6;
                    }

                    if (middleBottomChars.Length > 0)
                    {
                        if (!results.ContainsKey(9) && input.Contains(middleBottomChars[0]) && input.Contains(middleBottomChars[1]))
                        {
                            results.Add(9, input);
                            return 9;
                        }
                        else if (!results.ContainsKey(0) && ((input.Contains(middleBottomChars[0]) && !input.Contains(middleBottomChars[1])) || (!input.Contains(middleBottomChars[0]) && input.Contains(middleBottomChars[1]))))
                        {
                            results.Add(0, input);
                            return 0;
                        }
                    }

                    if (results.ContainsKey(9))
                    {
                        if (ContainsAllLetters(input, results[9]))
                        {
                            return 9;
                        }
                        else
                        {
                            results.Add(0, input);
                            return 0;
                        }
                    }

                    return 0;
                case 5:
                    if (results.ContainsKey(3))
                    {
                        if (ContainsAllLetters(input, results[3]))
                        {
                            return 3;
                        }
                    }
                    if (results.ContainsKey(2))
                    {
                        if (ContainsAllLetters(input, results[2]))
                        {
                            return 2;
                        }
                    }
                    if (results.ContainsKey(5))
                    {
                        if (ContainsAllLetters(input, results[5]))
                        {
                            return 5;
                        }
                    }
                    one = results[1];
                    if (!results.ContainsKey(3) && input.Contains(one[0]) && input.Contains(one[1]))
                    {
                        results.Add(3, input);
                        string seven = results[7];
                        for (int i = 0; i < seven.Length; i++)
                        {
                            input = input.Replace(seven[i].ToString(), "");
                        }
                        middleBottomChars = input;
                        return 3;
                    }

                    string tempInput = input;
                    string nextSeven = results[7];
                    string four = results[4];
                    for (int i = 0; i < nextSeven.Length; i++)
                    {
                        tempInput = tempInput.Replace(nextSeven[i].ToString(), "");
                    }
                    for (int i = 0; i < four.Length; i++)
                    {
                        tempInput = tempInput.Replace(four[i].ToString(), "");
                    }

                    if (tempInput.Length == 1 && !results.ContainsKey(5))
                    {
                        results.Add(5, input);
                        return 5;
                    }
                    else if (tempInput.Length == 2 && !results.ContainsKey(2))
                    {
                        results.Add(2, input);
                        return 2;
                    }
                    return 2;
                default:
                    return -1;
            }
        }

        static private bool ContainsAllLetters(string inputString, string stringToCheck)
        {
            for (int i = 0; i < inputString.Length; i++)
            {
                if (!stringToCheck.Contains(inputString[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
