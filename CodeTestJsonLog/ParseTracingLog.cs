using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeTestJsonLog
{
    class ParseTracingLog
    {
        BTree<TracingLogLinkNode> TracingLogTree = new BTree<TracingLogLinkNode>();
        TracingLogDict TracingLogDict = new TracingLogDict();

        public string OutputFileName = System.AppDomain.CurrentDomain.BaseDirectory + @"\ParseTracingLog.txt";

        public void Process(string inputFileName)
        {
            try
            {
                //string inputString = File.ReadAllText(inputFileName);

                ParseInputFile(inputFileName);

                var sortedList = TracingLogDict.LogDict.Values.ToList();
                sortedList.Sort();

                //If do not allow to use Sort(), we can use the BTree to Sort the result by time field.

                using (StreamWriter fs = new System.IO.StreamWriter(OutputFileName))
                {

                    foreach (var node in sortedList)
                    {
                        var curNode = node;
                        var item = curNode.TracingLog;
                        fs.Write("- " + item.time + " " + item.component + " " + item.msg + "\r\n");

                        while (curNode.Next != null)
                        {
                            curNode = curNode.Next;
                            item = curNode.TracingLog;
                            fs.Write("    ");
                            fs.Write("- " + item.time + " " + item.component + " " + item.msg + "\r\n");
                        }
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("ParseTracingLog Exception: " + e.ToString());
            }
            finally
            {
            }

            return;
        }

        private void ParseInputFile(string inputFileName)
        {
            using (StreamReader fsource = new StreamReader(inputFileName))
            {
                TracingLog CurrentNode = new TracingLog();

                string line = fsource.ReadLine();

                while (line != null)
                {
                    line = line.Trim();

                    int Index = line.IndexOf("\": \"");
                    if (Index > 0)
                    {
                        string[] lineArr = new string[2];

                        lineArr[0] = line.Substring(1, Index - 1);

                        int Length = line.Length;
                        int tailLength = 1;
                        if (line[Length - 1] == ',') tailLength = 2;
                        lineArr[1] = line.Substring(Index + 4, Length - (Index + 4) - tailLength);
                        CurrentNode.SetFieldByStringArr(lineArr);
                    }
                    else if (line == "{")
                    {
                        CurrentNode = new TracingLog();
                    }
                    else if (line[0] == '}')
                    {
                        TracingLogDict.AddToDict(CurrentNode);
                    }

                    line = fsource.ReadLine();
                }

                return;
            }
        }

    }
}
