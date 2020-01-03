using System;
using System.Collections.Generic;

namespace NarraTech_Insight.SharedObjects
{
    public class TimeSeries
    {
        List<TimeSeriesPoint> myPoints;
        public string TagName { get; set; }
        public string RetensionPolicy { get; set; }
        public string DatabaseName { get; set; }

        public TimeSeries(string tagname, string ret, string databasename, List<TimeSeriesPoint> points)
        {
            TagName = tagname;
            RetensionPolicy = ret;
            DatabaseName = databasename;
            myPoints = points;
        }
        public List<TimeSeriesPoint> TimeSeriesPoints
        {
            get { return this.myPoints; }
        }
    }
}