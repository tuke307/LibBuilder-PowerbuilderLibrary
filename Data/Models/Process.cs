using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Process
    {
        public string Target { get; set; }
        public string Library { get; set; }
        public string Object { get; set; }
        public string Mode { get; set; }
        public PBDotNetLib.orca.Orca.Result Result { get; set; }
    }
}