using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AdysTech.InfluxDB.Client.Net;
using NarraTech_Insight.SharedObjects;

namespace Narractech_Insight.CommoUntil.InfluxDbAPI
{
    public class InfluxLibrary
    {
        private readonly string _myHostName = null;
        private readonly string _myUserName = null;
        private readonly string _myPassword = null;
        /// <summary>
        /// Is used for the unit test
        /// </summary>
        /// <param name="HostName"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="DatabaseName"></param>
        public InfluxLibrary(string HostName, string UserName, string Password)
        {
            _myHostName = HostName;
            _myUserName = UserName;
            _myPassword = Password;
        }
        public string HostName
        {
            get { return _myHostName; }
        }
        public string UserName
        {
            get { return _myUserName; }
        }
        public string Password
        {
            get { return _myPassword; }
        }

        /// <summary>
        /// Get a connection to influx.
        /// </summary>
        /// <returns>
        /// Returns it to the methode that call for a connection
        /// </returns>
        public InfluxDBClient GetConnection()
        {
            InfluxDBClient client = new InfluxDBClient(_myHostName, _myUserName, _myPassword);
            
            return client;
        }

        /// <summary>
        /// Get data that it has to write to the database.
        /// Get the connection from GetConnection
        /// </summary>
        /// <param name="samples"></param>
        public async Task WriteTimeSeriesIntAsync(TimeSeries aSeries)
        {
            var client = GetConnection();

            string tagName = aSeries.TagName;
            string retentionPolicy = aSeries.RetensionPolicy;
            string databaseName = aSeries.DatabaseName;

            foreach (TimeSeriesPoint<int> pt in aSeries.TimeSeriesPoints)
            {
                var currentPoint = new InfluxDatapoint<InfluxValueField>();
                var quality = pt.Quality;
                var value = pt.Value;
                var timeSeries = pt.TimeStamp_UTC;

                currentPoint.MeasurementName = tagName;
                currentPoint.UtcTimestamp = timeSeries.ToUniversalTime();
                currentPoint.Precision = TimePrecision.Seconds;
                
                currentPoint.Fields.Add("Values", new InfluxValueField(value));
                        
                currentPoint.Tags.Add("Qualuty", quality);
                currentPoint.Retention = new InfluxRetentionPolicy() { Name = retentionPolicy };

                using (client)
                {
                    var write = await client.PostPointAsync(databaseName, currentPoint);
                }
            }
        }

        public async Task WriteTimeSeriesDoubleAsync(TimeSeries aSeries)
        {
            var client = GetConnection();

            string tagName = aSeries.TagName;
            string retentionPolicy = aSeries.RetensionPolicy;
            string databaseName = aSeries.DatabaseName;

            foreach (TimeSeriesPoint<double> pt in aSeries.TimeSeriesPoints)
            {
                var currentPoint = new InfluxDatapoint<InfluxValueField>();
                var quality = pt.Quality;
                var value = pt.Value;
                var timeSeries = pt.TimeStamp_UTC;

                currentPoint.MeasurementName = tagName;
                currentPoint.UtcTimestamp = timeSeries.ToUniversalTime();
                currentPoint.Precision = TimePrecision.Seconds;
                
                currentPoint.Fields.Add("Values", new InfluxValueField(value));

                currentPoint.Tags.Add("Qualuty", quality);
                currentPoint.Retention = new InfluxRetentionPolicy() { Name = retentionPolicy };

                using (client)
                {
                    var write = await client.PostPointAsync(databaseName, currentPoint);
                }
            }
        }
        /// <summary>
        /// Send a request to the database to get all the databases that is in it.
        /// </summary>
        /// <returns>
        /// Return the list of databases
        /// </returns>
        public async Task<List<string>> GetDatabasesAsync()
        {
            var client = GetConnection();

            List<String> DatabaseName;
            
            DatabaseName = await client.GetInfluxDBNamesAsync();

            return DatabaseName;
        }

        /// <summary>
        /// Send a request to get the content from a specefik database.
        /// </summary>
        /// <returns>
        /// Return the content of the database.
        /// </returns>
        public async Task<Dictionary<string, List<string>>> GetDatabaseStructure(string dataBaseName)
        {
            Dictionary<String, List<string>> results = new Dictionary<string, List<string>>();

            try
            { 
                var client = GetConnection();

                var a = await client.GetInfluxDBStructureAsync(dataBaseName);

                foreach(var q in a.MeasurementHierarchy)
                {
                    List<string> tmpMP = new List<string>();

                    foreach (var z in q.Value)
                    {
                        tmpMP.Add(z.Name);
                    }

                    results.Add(q.Key.Name, tmpMP);
                }
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
