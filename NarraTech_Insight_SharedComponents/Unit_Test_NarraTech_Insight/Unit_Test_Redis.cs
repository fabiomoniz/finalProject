using Microsoft.VisualStudio.TestTools.UnitTesting;
using NarraTech_Insight.CommoUntil.RedisAPI;
using NarraTech_Insight.SharedObjects;
using System;
using System.Collections.Generic;

namespace Unit_Test_NarraTech_Insight
{
    [TestClass]
    public class RedisLibraryTest
    {
        [TestMethod]
        public void TestConnectionWithWrongHostname()
        {
            RedisConnect redis = new RedisConnect("167.71.73.6", "e48a27fb18497063edc3c1edbc");

            List<string> list = new List<string>
            {
                "test"
            };

            Assert.ThrowsException<StackExchange.Redis.RedisConnectionException>(() => redis.SetDestinationId("test", list));
        }

        [TestMethod]
        public void TestConnectionWithWrongPassword()
        {
            RedisConnect redis = new RedisConnect("167.71.73.16", "e48a27fb18497063edc3c1edbc1");

            List<string> list = new List<string>
            {
                "test"
            };

            Assert.ThrowsException<StackExchange.Redis.RedisConnectionException>(() => redis.SetDestinationId("test", list));
        }

        [TestMethod]
        public void TestGetDestinationIds()
        {
            RedisConnect redis = new RedisConnect("167.71.73.16", "e48a27fb18497063edc3c1edbc");

            List<string> list;

            list = redis.GetDestinationId("test");

            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void TestGetDestinationsIdwithNonExistingSourceId()
        {
            RedisConnect redis = new RedisConnect("167.71.73.16", "e48a27fb18497063edc3c1edbc");

            Assert.ThrowsException<ArgumentNullException>(() => redis.GetDestinationId("madeupsourceid"));
        }

        [TestMethod]
        public void TestGetMessuringMetaData()
        {
            RedisConnect redis = new RedisConnect("167.71.73.16", "e48a27fb18497063edc3c1edbc");

            List<string> list;

            list = redis.GetDestinationId("source1");

            List<MeasuringPointMetaData> list2 = redis.GetMeasurementPoints(list);

            Assert.AreEqual(1, list2.Count);


        }

        [TestMethod]
        public void TestGetCompression()
        {
            RedisConnect redis = new RedisConnect("167.71.73.16", "e48a27fb18497063edc3c1edbc");

            List<string> list;

            list = redis.GetDestinationId("source1");

            List<CompressionObject> list2 = redis.GetCompressionobjects(list);

            Assert.AreEqual(1, list2.Count);
        }

        [TestMethod]
        public void TestGetTransformation()
        {
            RedisConnect redis = new RedisConnect("167.71.73.16", "e48a27fb18497063edc3c1edbc");

            List<string> list;

            list = redis.GetDestinationId("source2");

            List<TransformationObject> list2 = redis.GetTransformationObjects(list);

            Assert.AreEqual(2, list2.Count);
        }

        [TestMethod]
        public void TestGetCurrentValues()
        {
            RedisConnect redis = new RedisConnect("167.71.73.16", "e48a27fb18497063edc3c1edbc");

            List<string> list;

            list = redis.GetDestinationId("source2");

            List<MeasuringPointMetaData> list2 = redis.GetMeasurementPoints(list);

            MeasuringPointMetaData obj = new MeasuringPointMetaData();

            obj = list2[0];

            string datatype = obj.PointMetadata.DataType;


            if (datatype == "int")
            {
                List<TimeSeriesPoint> list3 = redis.GetCurrentValue(list);
                Assert.AreEqual(2, list3.Count);
            }

        }
    }
}
