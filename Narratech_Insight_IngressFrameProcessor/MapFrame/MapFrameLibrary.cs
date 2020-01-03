using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NarraTech_Insight.SharedObjects;
using NarraTech_Insight.CommoUntil.RedisAPI;

namespace MapFrame
{
    public abstract class MapFrameLibrary
    {

        public MapFrameLibrary()
        {

        }

        

    }

    public class MapFrames : MapFrameLibrary
    {
        public MapFrames() : base()
        {

        }

        public List<TimeSeries> MapTimeSeries(JProperty jProprety)
        {
            string DataType = "";
            string TagName = "";
            string RetentionPolicy = "";
            string DatabaseName = "";
            List<MeasuringPointMetaData> metaData = new List<MeasuringPointMetaData>();
            List<TimeSeries> mappedTimeSeries = new List<TimeSeries>();

            foreach (var timeSeriesCollection in jProprety.Children())
            {
                foreach (var timeSeries in timeSeriesCollection)
                {
                    foreach (JProperty content in timeSeries)
                    {


                        if (content.Name.ToString() == "SourceID")
                        {
                            //connect to redis
                            RedisConnect redisConnect = new RedisConnect("167.71.73.16", "e48a27fb18497063edc3c1edbc");
                            //get destinationid from sourceid
                            List<string> destinationIDList = redisConnect.GetDestinationId(content.Value.ToString());

                            //creating and retriving the metadata from the destinationid
                            metaData = redisConnect.GetMeasurementPoints(destinationIDList);
                            

                        }
                        else if (content.Name.ToString() == "TimeSeriesPoint")
                        {
                            foreach (var item in metaData)
                            {
                                DataType = item.PointMetadata.DataType;
                                if (DataType == "int")
                                {
                                    List<TimeSeriesPoint> mappedtimeSeriesPoints = new List<TimeSeriesPoint>();
                                    foreach (var timeSeriesPoint in content.Value)
                                    {
                                        mappedtimeSeriesPoints.Add(CreateTimeSeriesPoint(timeSeriesPoint, DataType));
                                    }
                                    TagName = item.InfluxMetaData.MeasuringPointName;
                                    RetentionPolicy = item.InfluxMetaData.RetentionPolicy;
                                    DatabaseName = item.InfluxMetaData.DatabaseName;
                                    mappedTimeSeries.Add(CreateTimeSeries(TagName, RetentionPolicy, DatabaseName, mappedtimeSeriesPoints));
                                }
                                else if (DataType == "double")
                                {
                                    List<TimeSeriesPoint> mappedtimeSeriesPoints = new List<TimeSeriesPoint>();
                                    foreach (var timeSeriesPoint in content.Value)
                                    {
                                        mappedtimeSeriesPoints.Add(CreateTimeSeriesPoint(timeSeriesPoint, DataType));
                                    }
                                    TagName = item.InfluxMetaData.MeasuringPointName;
                                    RetentionPolicy = item.InfluxMetaData.RetentionPolicy;
                                    DatabaseName = item.InfluxMetaData.DatabaseName;
                                    mappedTimeSeries.Add(CreateTimeSeries(TagName, RetentionPolicy, DatabaseName, mappedtimeSeriesPoints));
                                }
                            }
                        }
                    }
                }
                return mappedTimeSeries;
            }
            return null;
        }

        public TimeSeriesPoint CreateTimeSeriesPoint(JToken timeSeriesPoint, string dataType)
        {
            string tsp_Value = "";
            string tsp_Quality = "";
            DateTime tsp_TimeStamp_UTC = DateTime.Now;

            foreach (JProperty timeSeriesPointValue in timeSeriesPoint)
            {
                switch (timeSeriesPointValue.Name.ToString())
                {
                    case "Value":
                        tsp_Value = timeSeriesPointValue.Value.ToString();
                        break;
                    case "Quality":
                        tsp_Quality = timeSeriesPointValue.Value.ToString();
                        break;
                    case "TimeStamp":
                        tsp_TimeStamp_UTC = Convert.ToDateTime(timeSeriesPointValue.Value.ToString());
                        break;
                }
            }

            TimeSeriesPoint tsp = TimeSeriesPoint.CreatePoint(tsp_Value, tsp_TimeStamp_UTC, tsp_Quality, dataType);

            return tsp;
        }

        public TimeSeries CreateTimeSeries(string TagName, string RetentionPolicy, string DatabaseName, List<TimeSeriesPoint> mappedtimeSeriesPoints)
        {
            TimeSeries timeSeries = new TimeSeries(TagName, RetentionPolicy, DatabaseName, mappedtimeSeriesPoints);

            return timeSeries;
        }

    }
}
