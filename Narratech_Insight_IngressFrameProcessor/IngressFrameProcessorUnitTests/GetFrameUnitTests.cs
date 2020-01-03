using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetFrame;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;

namespace IngressFrameProcessorUnitTests
{
    [TestClass]
    public class GetFrameUnitTests
    {
        [TestMethod]
        public void TestLoggingTheHeader()
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
                        ""SourceID"": ""Source1"",
                        ""TimeseriesPoint"": [
                                {
                                ""value"": ""23"",
					            ""quality"": ""Good"",
					            ""timeStamp"": ""2012-03-16T00:13:12.2454785""
                                },
                                {
                                ""value"": ""24"",
					            ""quality"": ""Good"",
					            ""timeStamp"": ""2012-03-16T00:13:13.2454785""
                                },
                                {
                                ""value"": ""25"",
					            ""quality"": ""Good"",
					            ""timeStamp"": ""2012-03-16T00:13:14.2454785""
                                }
                                ]
                    },
                    {
                        ""SourceID"": ""Source2"",
                        ""TimeseriesPoint"": [
                                {
                                ""value"": ""23"",
					            ""quality"": ""Good"",
					            ""timeStamp"": ""2012-03-16T00:13:12.2454785""
                                },
                                {
                                ""value"": ""24"",
					            ""quality"": ""Good"",
					            ""timeStamp"": ""2012-03-16T00:13:13.2454785""
                                },
                                {
                                ""value"": ""25"",
					            ""quality"": ""Good"",
					            ""timeStamp"": ""2012-03-16T00:13:14.2454785""
                                }
                                ]
                    }
                    ]
            }
            ]";


            Header header = getFrame.LogHeader(jsonString);


            Assert.AreEqual("192.7.7.7", header.SourceIP);
            Assert.AreEqual("plant-01", header.SourceHostName);
            Assert.AreEqual(2.1231, header.FrameVersion);
            Assert.AreEqual(3, header.NumberOfPayloads);
            Assert.AreEqual(2.3222, header.ProcessingTime);
            Assert.AreEqual("madeup domain", header.Domain);

        }

        [TestMethod]
        public void Test_If_Error_Handling_If_There_Is_A_Missing_Header()
        {
            GetFrames getFrame = new GetFrames();

            var jsonString = @"[
            {
                ""SourceIP"": ""192.7.7.7"",
                ""SourceHostName"": """",
                ""TimeSeries"": [
                    {
                        ""SourceID"": ""Source1"",
                        ""TimeseriesPoint"": [
                                {
                                ""value"": ""23"",
					            ""quality"": ""Good"",
					            ""timeStamp"": ""2012-03-16T00:13:12.2454785""
                                },
                                {
                                ""value"": ""24"",
					            ""quality"": ""Good"",
					            ""timeStamp"": ""2012-03-16T00:13:13.2454785""
                                },
                                {
                                ""value"": ""25"",
					            ""quality"": ""Good"",
					            ""timeStamp"": ""2012-03-16T00:13:14.2454785""
                                }
                                ]
                    },
                    ]
            }
            ]";


            Header header = getFrame.LogHeader(jsonString);

            Assert.ThrowsException<JsonReaderException>(() => getFrame.LogHeader(header.SourceHostName));
        }
    }
}