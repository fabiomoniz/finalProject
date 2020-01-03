using System;
using System.Collections.Generic;
using System.Text;

namespace GetFrame
{
    public class Header
    {
        public string SourceIP { get; set; }
        public string SourceHostName { get; set; }
        public DateTime TimeStampOnSent { get; set; }
        public double FrameVersion { get; set; }
        public int NumberOfPayloads { get; set; }
        public double ProcessingTime { get; set; }
        public string Domain { get; set; }
    }
}
