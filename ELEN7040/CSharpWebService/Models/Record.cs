using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSharpWebService.Models
{
    public class Record
    {
        public string playerID { get; set; }
        public int yearID { get; set; }
        public int stint { get; set; }
        public string teamID { get; set; }
        public string lgID { get; set; }
        public int G { get; set; }
        public int AB { get; set; }
        public int R { get; set; }
        public int H { get; set; }
    }
}