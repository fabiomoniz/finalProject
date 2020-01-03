using Narractech_Insight.CommoUntil.InfluxDbAPI;
using NarraTech_Insight.SharedObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    static class Program
    {
        public static async Task Main(string[] args)
        {
            InfluxLibrary influx = new InfluxLibrary("http://134.209.204.150:8086", "SimulatorUser", "2k6qzXbefhvtVsYEojztgkQ7UZqVk2UbmQmTgM2CauGfqDtTCcQLFKrcxYk3s36L");

            await influx.WriteTimeSeriesIntAsync(GetTimeSeriesInt());
            await influx.WriteTimeSeriesDoubleAsync(GetTimeSeriesDouble());
        }

        public static TimeSeries GetTimeSeriesDouble()
        {
            var Samples = new TimeSeries("Test1",
                                         "autogen",
                                         "Test",
                                         DoublePoints());

            return Samples;
        }
        public static TimeSeries GetTimeSeriesInt()
        {
            var Samples = new TimeSeries("Test2",
                                         "autogen",
                                         "Test",
                                         IntPoints());
            return Samples;
        }
        public static List<TimeSeriesPoint> DoublePoints()
        {
            var point = new List<TimeSeriesPoint>
            {
                TimeSeriesPoint.CreatePoint("2.32532",
                                            DateTime.UtcNow,
                                            "Good",
                                            "double"
                                            )
            };
            return point;
        }
        public static List<TimeSeriesPoint> IntPoints()
        {
            var point = new List<TimeSeriesPoint>
            {
                TimeSeriesPoint.CreatePoint("2",
                                            DateTime.UtcNow,
                                            "Bad",
                                            "int")
            };
            return point;
        }
    }
}
