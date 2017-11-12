using System;
using RestSharp;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string phpService = Properties.Settings.Default.PHPWebservice;
            string csService = Properties.Settings.Default.CSharpWebService;
            int smallLimit = Properties.Settings.Default.SmallLimit;
            int bigLimit = Properties.Settings.Default.BigLimit;



            LogMessage("\r\n\r\nPaul van Coller ELEN7040 Test starting.\r\n");

            //Confirm connection to service:
            PerformTest(phpService, smallLimit, 1, true);
            PerformTest(csService, smallLimit, 1, true);

            LogMessage("\r\nPreparing to perform cold start test. Perform IISRESET and press any key to continue...");
            Console.ReadKey();
            LogMessage("PHP test - cold start\r\n=========");
            PerformTest(phpService, smallLimit, 1);

            LogMessage("\r\n\r\n5Preparing to perform cold start test. Perform IISRESET and press any key to continue...");
            Console.ReadKey();
            LogMessage("C# test - cold start\r\n=========");
            PerformTest(csService, smallLimit, 1);

            LogMessage("\r\nPHP small payload test\r\n=========");
            for (int i = 0; i < 6; i++)
                PerformTest(phpService, smallLimit, 1);

            LogMessage("\r\nC# small payload tests\r\n=========");
            for (int i = 0; i < 6; i++)
                PerformTest(csService, smallLimit, 1);

            LogMessage("\r\nPHP large payload test\r\n=========");
            for (int i = 0; i < 6; i++)
                PerformTest(phpService, bigLimit, 1);

            LogMessage("\r\nC# large payload tests\r\n=========");
            for (int i = 0; i < 6; i++)
                PerformTest(csService, bigLimit, 1);

            LogMessage("\r\nTest complete.");
            Console.ReadKey();
        }

        private static void LogMessage(string logText)
        {
            string logFile = String.Concat(Path.GetTempPath() + "ELEN7040.log");

            File.AppendAllText(logFile, logText + "\r\n");
            Console.WriteLine(logText);
        }

        private static void PerformTest(string baseUrl, int recordLimit, int threads, bool quiet = false)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var client = new RestClient();
            client.BaseUrl = new Uri(baseUrl);

            var request = new RestRequest();
            request.Resource = "TestPerformance/" + recordLimit + "/" + threads;
            request.AddHeader("Accept", "application/xml");

            IRestResponse response = client.Execute(request);

            sw.Stop();
            double ClientProcessingTime = Math.Round(sw.Elapsed.TotalMilliseconds, 7);


            // Process Results
            double serviceProcessingTime;
            if (!response.IsSuccessful)
            {
                LogMessage("Error in connection: " + response.ErrorMessage);
                return;
            }
            else
            {
                using (Stream xmlStream = GenerateStreamFromString(response.Content))
                {
                    XDocument xmlDoc = XDocument.Load(xmlStream);

                    XNamespace nsModels = "http://schemas.datacontract.org/2004/07/CSharpWebService.Models";
                    XElement resultElement = xmlDoc.Element(nsModels + "ReturnModel");
                    if (resultElement == null)
                    {
                        LogMessage("Error in connection: " + response.ErrorMessage);
                        return;
                    }
                    else
                    {
                        serviceProcessingTime = double.Parse(resultElement.Element(nsModels+ "ProcessingTime").Value);
                        //LogMessage(string.Format("ProcessingTime: {0}", resultElement.Element(nsModels + "ProcessingTime").Value));
                    }
                }
            }

            double latency = ClientProcessingTime - serviceProcessingTime;

            if (!quiet)
            {
                System.Threading.Thread.Sleep(250);
                LogMessage(String.Format("RecordLimit: {0}\tThreads {1}:\tLatency: {2}ms\tProcessing: {3}ms", recordLimit, threads, latency, serviceProcessingTime));
                //LogMessage(String.Format("\tClient Overall: {0}ms\tService Overall: {1}ms", ClientProcessingTime, serviceProcessingTime));
            }
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
