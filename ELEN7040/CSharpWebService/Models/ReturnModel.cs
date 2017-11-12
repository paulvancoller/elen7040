using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSharpWebService.Models
{
    public class ReturnModel
    {
        public DateTime ProcessingStartTime { get; set; }
        public DateTime ProcessingEndTime { get; set; }
        public ConcurrentBag<long>  processedValue { get; set; }
    }
}