using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTestJsonLog
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFileName = Properties.Settings.Default.inputFileName;

            if (string.IsNullOrEmpty(inputFileName)) inputFileName = System.AppDomain.CurrentDomain.BaseDirectory + @"\log-data.json";

            var watch = System.Diagnostics.Stopwatch.StartNew();

            //Method 1, using NewtonsoftJson lib, 
            //Just for testing. Not full implemented.
            UseNewtonsoftJson.Process(inputFileName);


            watch.Stop();
            Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

            watch.Restart();
            //Method 2, only the standard libraries
            //The Sort is O(nlog(n)), so the runtime complexity is O(nlog(n))
            //the memory complexity is O(2N) 
            ParseTracingLog ParseTracingLog = new ParseTracingLog();
            ParseTracingLog.Process(inputFileName);

            watch.Stop();
            Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

            Console.ReadKey();
            return;
        }
    }
}
