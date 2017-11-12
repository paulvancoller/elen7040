using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CSharpWebService.Models;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Diagnostics;

namespace CSharpWebService.Controllers
{
    public class HomeController : ApiController
    {
        // GET: api/Home
        // GET: api/Home
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public ReturnModel TestPerformance(int recordLimit, int threads)
        {
            ReturnModel retModel = new ReturnModel();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ConcurrentBag<List<Record>> fileContentsList = new ConcurrentBag<List<Record>>();
            Parallel.For(0, threads,
                index =>
                {
                    fileContentsList.Add(LoadCSVFile(recordLimit));
                });


            retModel.processedValue = fileContentsList;

            sw.Stop();
            retModel.ProcessingTime = Math.Round(sw.Elapsed.TotalMilliseconds, 7);

            return retModel;
        }

        private List<Record> LoadCSVFile(int recordLimit)
        {
            // increase recordLimit to ignore header
            recordLimit++;

            List<Record> returnVal = new List<Record>();

            int rowCount = 0;
            foreach (string fileContents in File.ReadLines(@"C:\Data\Data.csv"))
            {
                rowCount++;

                // Skip header
                if (rowCount == 1)
                    continue;

                if (rowCount > recordLimit)
                    break;

                string[] lineContents = fileContents.Split(',');

                Record record = new Record()
                {
                    playerID = lineContents[0],
                    yearID = int.Parse(lineContents[1]),
                    stint = int.Parse(lineContents[2]),
                    teamID = lineContents[3],
                    lgID = lineContents[4],
                    G = int.Parse(lineContents[5]),
                    AB = int.Parse(lineContents[6]),
                    R = int.Parse(lineContents[7]),
                    H = int.Parse(lineContents[8])
                };

                returnVal.Add(record);
            }

            return returnVal;
        }
    }
}
