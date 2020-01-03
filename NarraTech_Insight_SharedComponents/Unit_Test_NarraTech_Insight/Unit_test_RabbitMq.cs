using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using static NarraTech_Insight.CommoUntil.RabbitMqAPI.RabbitMqConnection;

namespace Unit_Test_NarraTech_Insight
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Write_With_Incorrect_Hostname()
        {
            RabbitMQProducer rmq = new RabbitMQProducer("165.22.199.15", "admin", "2MmV2KjEhX36DaZeyAqhbifbp86YDxHG7yJq2R7JPJwJiXvN7jXpqhxjUr4ZCkGL");

            Assert.ThrowsException<RabbitMQ.Client.Exceptions.BrokerUnreachableException>(() => rmq.Send("Test_Queue", "Test"));
        }

        [TestMethod]
        public void Test_Write_With_Incorrect_UserName()
        {
            RabbitMQProducer rmq = new RabbitMQProducer("165.22.199.153", "dmin", "2MmV2KjEhX36DaZeyAqhbifbp86YDxHG7yJq2R7JPJwJiXvN7jXpqhxjUr4ZCkGL");

            Assert.ThrowsException<RabbitMQ.Client.Exceptions.BrokerUnreachableException>(() => rmq.Send("Test_Queue", "Test"));
        }

        [TestMethod]
        public void Test_Write_With_Incorrect_Password()
        {
            RabbitMQProducer rmq = new RabbitMQProducer("165.22.199.153", "admin", "MmV2KjEhX36DaZeyAqhbifbp86YDxHG7yJq2R7JPJwJiXvN7jXpqhxjUr4ZCkGL");

            Assert.ThrowsException<RabbitMQ.Client.Exceptions.BrokerUnreachableException>(() => rmq.Send("Test_Queue", "Test"));
        }

        [TestMethod]
        public void Test_Read_With_Incorrect_Password()
        {
            RabbitMQConsumer rmq = new RabbitMQConsumer("165.22.199.153", "admin", "MmV2KjEhX36DaZeyAqhbifbp86YDxHG7yJq2R7JPJwJiXvN7jXpqhxjUr4ZCkGL");

            Assert.ThrowsException<RabbitMQ.Client.Exceptions.BrokerUnreachableException>(() => rmq.Receive("Test_Queue"));
        }
        [TestMethod]
        public void Test_Read_With_Incorrect_UserName()
        {
            RabbitMQConsumer rmq = new RabbitMQConsumer("165.22.199.153", "dmin", "2MmV2KjEhX36DaZeyAqhbifbp86YDxHG7yJq2R7JPJwJiXvN7jXpqhxjUr4ZCkGL");

            Assert.ThrowsException<RabbitMQ.Client.Exceptions.BrokerUnreachableException>(() => rmq.Receive("Test_Queue"));
        }
        [TestMethod]
        public void Test_Read_With_Incorrect_HostName()
        {
            RabbitMQConsumer rmq = new RabbitMQConsumer("165.22.199.15", "admin", "2MmV2KjEhX36DaZeyAqhbifbp86YDxHG7yJq2R7JPJwJiXvN7jXpqhxjUr4ZCkGL");

            Assert.ThrowsException<RabbitMQ.Client.Exceptions.BrokerUnreachableException>(() => rmq.Receive("Test_Queue"));
        }

        [TestMethod]
        public void TestIfWeReceiveTheMessageSent()
        {

            RabbitMQProducer rmp = new RabbitMQProducer("165.22.199.153", "admin", "2MmV2KjEhX36DaZeyAqhbifbp86YDxHG7yJq2R7JPJwJiXvN7jXpqhxjUr4ZCkGL");

            rmp.Send("test_queue" , "testing");

            RabbitMQConsumer rmc = new RabbitMQConsumer("165.22.199.153", "admin", "2MmV2KjEhX36DaZeyAqhbifbp86YDxHG7yJq2R7JPJwJiXvN7jXpqhxjUr4ZCkGL");

            Assert.AreEqual("testing", rmc.Receive("test_queue"));

        }
    }
}
