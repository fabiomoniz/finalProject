using NarraTech_Insight.SharedObjects;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NarraTech_Insight.CommoUntil.RabbitMqAPI
{
    public abstract class RabbitMqConnection
    {
        private string _myHostName;
        private string _myUserName;
        private string _myPassword;

        protected ConnectionFactory connectionFactory = null;
        protected IConnection connection = null;
        public RabbitMqConnection(string HostName, string UserName, string Password)
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

        protected IConnection Connection()
        {
            if (connectionFactory == null)
            {
                try
                {
                    connectionFactory = new ConnectionFactory
                    {
                        HostName = _myHostName,
                        UserName = _myUserName,
                        Password = _myPassword,
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (connection == null)
            {
                try
                {
                    connection = connectionFactory.CreateConnection();
                    return connection;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            connectionFactory = null;
            connection = null;
            return null;
        }

        ///<summary>
        ///Disconnect RabbitMq
        ///</summary>
        protected void Disconnect()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }

        ///<summary>
        ///Checks if there is a connection
        ///</summary>
        protected bool IsConnected()
        {
            //checks if connection is not null
            if (connection != null)
            {
                //checks if connection is connected
                if (connection.IsOpen)
                {
                    return true;
                }
            }
            return false;
        }

        public class RabbitMQProducer : RabbitMqConnection
        {
            //the idea is to send and serialized json file "stirng" as the messege 
            public RabbitMQProducer(string HostName, string UserName, string Password) : base(HostName, UserName, Password)
            {

            }

            public void Send(string queueName, string data)
            {
                try
                {
                    using (Connection())
                    {
                        if (connection != null)
                        {
                            using (IModel channel = connection.CreateModel())
                            {
                                channel.QueueDeclare(queue: queueName,
                                                     durable: false,
                                                     exclusive: false,
                                                     autoDelete: false,
                                                     arguments: null);

                                channel.BasicPublish(exchange: string.Empty,
                                                     routingKey: queueName,
                                                     basicProperties: null,
                                                     body: Encoding.UTF8.GetBytes(data));
                            }
                            //testing purposes remove later
                            Console.WriteLine(" [x] Sent {0}", data);
                        }
                    }
                    Disconnect();
                }
                catch (BrokerUnreachableException)
                {
                    Console.WriteLine("Unable to connect, enter either a correct HostName, UserName and/or Password.");
                    throw;
                }
            }
        }

        public class RabbitMQConsumer : RabbitMqConnection
        {
            public RabbitMQConsumer(string HostName, string UserName, string Password) : base(HostName, UserName, Password)
            {

            }
            public String Receive(string queueName)
            {
                try
                {
                    using (Connection())
                    {
                        if (connection != null)
                        {
                            using (IModel channel = connection.CreateModel())
                            {
                                channel.QueueDeclare(queue: queueName,
                                                     durable: false,
                                                     exclusive: false,
                                                     autoDelete: false,
                                                     arguments: null);

                                BasicGetResult result = channel.BasicGet(queueName, true);
                                if (result != null)
                                {
                                    string data = Encoding.UTF8.GetString(result.Body);
                                    return data;
                                }
                            }
                        }
                    }
                }
                catch (BrokerUnreachableException)
                {
                    Console.WriteLine("Unable to connect, enter either a correct HostName, UserName and/or Password.");
                    throw;
                }
                Disconnect();
                return null;                
            }
        }

    }
}
