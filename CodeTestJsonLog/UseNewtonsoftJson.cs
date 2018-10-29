using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

namespace CodeTestJsonLog
{
    class UseNewtonsoftJson
    {

        public static void Process(string inputFileName)
        {
            string OutputFileName = System.AppDomain.CurrentDomain.BaseDirectory + @"\UseNewtonsoftJson.txt";

            try
            {
                string inputString = File.ReadAllText(inputFileName);

                //var result = JsonConvert.DeserializeObject(inputString);

                TracingLog[] result = JsonConvert.DeserializeObject<TracingLog[]>(inputString);

                var sortedList = from item in result
                                 orderby item.trace_id, item.time
                                 select item;

                using (StreamWriter fs = new System.IO.StreamWriter(OutputFileName))
                {
                    string PrevTraceId = "";

                    foreach (var item in sortedList)
                    {
                        if (item.trace_id == PrevTraceId) fs.Write("    ");
                        //fs.Write("- " + item.trace_id + " " + item.time.ToString() + " " + item.component + " " + item.msg + "\r\n");
                        fs.Write("- " + item.time + " " + item.component + " " + item.msg + "\r\n");

                        PrevTraceId = item.trace_id;
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("UseNewtonsoftJson Exception: " + e.ToString());
            }
            finally
            { 
            }
                        
            return;
        }

    }
}
