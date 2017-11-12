using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CSharpWebService.Models;
using System.Threading.Tasks;
using System.Collections.Concurrent;

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
        public ReturnModel TestPerformance(string inputData, int threads)
        {
            ReturnModel retModel = new ReturnModel();
            retModel.ProcessingStartTime = DateTime.Now;

            ConcurrentBag<long> primeNumberList = new ConcurrentBag<long>();
            Parallel.For(0, threads,
                index =>
                {
                    primeNumberList.Add(FindPrimeNumber(inputData.Length * 3000));
                });


            retModel.processedValue = primeNumberList;
            retModel.ProcessingEndTime = DateTime.Now;

            return retModel;
        }

        public long FindPrimeNumber(int n)
        {
            int count = 0;
            long a = 2;

            while (count < n)
            {
                long b = 2;
                int prime = 1;// to check if found a prime

                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }

                    b++;
                }

                if (prime > 0)
                {
                    count++;
                }

                a++;
            }

            return (--a);
        }


    }
}
