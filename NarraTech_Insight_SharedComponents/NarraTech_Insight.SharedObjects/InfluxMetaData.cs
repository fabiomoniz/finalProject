using System;
using System.Collections.Generic;
using System.Text;

namespace NarraTech_Insight.SharedObjects
{
    public class InfluxMetaData
    {
        public string DatabaseName { get; set; }
        public string MeasuringPointName { get; set; }
        public string FieldValue { get; set; }
        public string RetentionPolicy { get; set; }
    }
}
