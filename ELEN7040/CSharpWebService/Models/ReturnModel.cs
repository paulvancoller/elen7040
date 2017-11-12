using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSharpWebService.Models
{
    public class ReturnModel
    {
        public double ProcessingTime { get; set; }
        public ConcurrentBag<List<Record>>  processedValue { get; set; }
    }
}