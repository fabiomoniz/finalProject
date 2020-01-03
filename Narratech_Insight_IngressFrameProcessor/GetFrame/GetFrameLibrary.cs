using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using MapFrame;
using NarraTech_Insight.SharedObjects;
using Newtonsoft.Json;
namespace GetFrame
{
    public abstract class GetFrameLibrary
    {
        protected void ConnectToIngressQueue()
        {

        }

        protected void DisconnectFromIngressQueue()
        {

        }

        protected void GetFrame()
        {
            //connect()

            //try
            //{
            //    try to get the payload;

            //}
            //catch (Exception)
            //{
            //    if no payload avaliable set a timer or something then

            //    GetPayload() call method inside itself
            //}   
        }

    }

    public class GetFrames : GetFrameLibrary
    {
        public GetFrames() : base()
        {
            
        }


        public Header LogHeader(String jsonString) //so many things could go wrong here
        {
                Header header = new Header();

                JArray jsonVal = JArray.Parse(jsonString);

            try
            {
                foreach (JObject jObject in jsonVal)
                {
                    foreach (JProperty jProperty in jObject.Properties())
                    {
                        switch (jProperty.Name.ToString())
                        {
                            case "SourceIP":
                                header.SourceIP = jProperty.Value.ToString();
                                break;
                            case "SourceHostName":
                                header.SourceHostName = jProperty.Value.ToString();
                                break;
                            case "TimeStampOnSent":
                                header.TimeStampOnSent = Convert.ToDateTime(jProperty.Value.ToString());
                                break;
                            case "FrameVersion":
                                header.FrameVersion = Convert.ToDouble(jProperty.Value.ToString());
                                break;
                            case "NumberOfPayloads":
                                header.NumberOfPayloads = Int32.Parse(jProperty.Value.ToString());
                                break;
                            case "ProcessingTime":
                                header.ProcessingTime = double.Parse(jProperty.Value.ToString());
                                break;
                            case "Domain":
                                header.Domain = jProperty.Value.ToString();
                                break;
                            case "TimeSeries":                                
                                return header;
                        }
                    }
                }
            }
            catch(JsonReaderException)
            {
                throw;
            }
            return null;
        }

        public List<TimeSeries> GetTimeSeries(String jsonString)
        {
            JArray jsonVal = JArray.Parse(jsonString);

            foreach (JObject jObject in jsonVal)
            {
                foreach (JProperty jProperty in jObject.Properties())
                {
                    switch (jProperty.Name.ToString())
                    {                        
                        case "TimeSeries":
                            MapFrames mapframe = new MapFrames();
                            return mapframe.MapTimeSeries(jProperty);
                    }
                }
            }
            return null;
        }
    }
}
