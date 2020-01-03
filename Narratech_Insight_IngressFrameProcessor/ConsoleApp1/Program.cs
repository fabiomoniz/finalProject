using System;
using GetFrame;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            GetFrames getFrame = new GetFrames();

            var jsonString = @"[
            {
                ""SourceIP"": ""192.7.7.7"",
                ""SourceHostName"": ""plant-01"",
                ""TimeStampOnSent"": ""2012-03-16T00:13:12.2810521-10:00"",
                ""FrameVersion"": ""2,1231"",
                ""NumberOfPayloads"": ""3"",
                ""ProcessingTime"": ""2,3222"",
                ""Domain"": ""madeup domain"",
                ""TimeSeries"": [
                    {
                        ""SourceID"": ""source1"",
                        ""TimeSeriesPoint"": [
                                {
                                ""Value"": ""23"",
					            ""Quality"": ""Good"",
					            ""TimeStamp"": ""2012-03-16T00:13:12.2454785""
                                },
                                {
                                ""Value"": ""24"",
					            ""Quality"": ""Good"",
					            ""TimeStamp"": ""2012-03-16T00:13:13.2454785""
                                },
                                {
                                ""Value"": ""25"",
					            ""Quality"": ""Good"",
					            ""TimeStamp"": ""2012-03-16T00:13:14.2454785""
                                }
                                ]
                    },
                    {
                        ""SourceID"": ""source2"",
                        ""TimeSeriesPoint"": [
                                {
                                ""Value"": ""23"",
					            ""Quality"": ""Good"",
					            ""TimeStamp"": ""2012-03-16T00:13:12.2454785""
                                },
                                {
                                ""Value"": ""24"",
					            ""Quality"": ""Good"",
					            ""TimeStamp"": ""2012-03-16T00:13:13.2454785""
                                },
                                {
                                ""Value"": ""25"",
					            ""Quality"": ""Good"",
					            ""TimeStamp"": ""2012-03-16T00:13:14.2454785""
                                }
                                ]
                    }
                    ]
            }
            ]";


            Header header = new Header();
            header = getFrame.LogHeader(jsonString);

            string json = JsonConvert.SerializeObject(getFrame.GetTimeSeries(jsonString));

            Console.WriteLine(json);
        }
    }
}
