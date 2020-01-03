using AdysTech.InfluxDB.Client.Net;
using Narractech_Insight.CommoUntil.InfluxDbAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NarraTech_Insight.SharedObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unit_Test_NarraTech_Insight
{
    [TestClass]
    public class InfluxlibraryUnitTest
    {
        public TimeSeries GetTimeSeriesDouble()
        {
            var Samples = new TimeSeries("Test1",
                                         "autogen",
                                         "Test",
                                         DoublePoints());

            return Samples;
        }
        public TimeSeries GetTimeSeriesInt()
        {
            var Samples = new TimeSeries("Test2",
                                         "autogen",
                                         "Test",
                                         IntPoints());
            return Samples;
        }
        public List<TimeSeriesPoint> DoublePoints()
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
        public List<TimeSeriesPoint> IntPoints()
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

        [TestMethod]
        public async Task InfluxDB_CanGetDataBase()
        {
            InfluxLibrary influx = new InfluxLibrary("http://134.209.204.150:8086", "SimulatorUser", "2k6qzXbefhvtVsYEojztgkQ7UZqVk2UbmQmTgM2CauGfqDtTCcQLFKrcxYk3s36L");

            List<string> database = await influx.GetDatabasesAsync();

            Assert.IsTrue(database.Count == 4);
        }

        [TestMethod]
        public async Task Check_If_dataBase_GetConnection_Error_Username_Password()
        {
            InfluxLibrary influx = new InfluxLibrary("http://134.209.204.150:8086", "SimulatorUser", "2k6qzXbefhvtVsYEojztgkQ7UZqVk2UbmQmTgM2CauGfqDtTCcQLFKrcxYk3s36");

            await Assert.ThrowsExceptionAsync<System.UnauthorizedAccessException>(() => influx.GetDatabasesAsync());
        }

        [TestMethod]
        public async Task Check_If_dataBase_GetConnection_Error_Ip_Address()
        {
            InfluxLibrary influx = new InfluxLibrary("http://134.209.204.150:808", "SimulatorUser", "2k6qzXbefhvtVsYEojztgkQ7UZqVk2UbmQmTgM2CauGfqDtTCcQLFKrcxYk3s36L");

            await Assert.ThrowsExceptionAsync<ServiceUnavailableException>(() => influx.GetDatabasesAsync());
        }

        [TestMethod]
        public async Task InfluxDB_Write_With_All_Format()
        {
            int expected = 6;

            InfluxLibrary influx = new InfluxLibrary("http://134.209.204.150:8086", "SimulatorUser", "2k6qzXbefhvtVsYEojztgkQ7UZqVk2UbmQmTgM2CauGfqDtTCcQLFKrcxYk3s36L");

            await influx.WriteTimeSeriesIntAsync(GetTimeSeriesInt());
            await influx.WriteTimeSeriesDoubleAsync(GetTimeSeriesDouble());
            var actual = await influx.GetDatabaseStructure("Test");

            Assert.AreEqual(expected, actual["autogen"].Count);
        }
    }
}
