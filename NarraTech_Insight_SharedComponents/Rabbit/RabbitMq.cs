using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client.Exceptions;

namespace RabbitMqClassLibrary
{
    public abstract class RabbitMqConnection
    {
        private string HostName;
        private string UserName;
        private string Password;

        protected ConnectionFactory connectionFactory = null;
        protected IConnection connection = null;

        protected RabbitMqConnection
            (
                string _hosName,
                string _userName,
                string _password
            )
        {
            this.HostName = _hosName;
            this.UserName = _userName;
            this.Password = _password;
        }


        protected IConnection Connection()
        {
            if(connectionFactory == null)
            {
                try
                {
                    connectionFactory = new ConnectionFactory
                    {
                        HostName = HostName,
                        UserName = UserName,
                        Password = Password,
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
            public RabbitMQProducer(string hostname, string username, string password) : base(hostname, username, password)
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
            public RabbitMQConsumer(string hostname, string username, string password) : base(hostname, username, password)
            {

            }

            public void Receive(string queueName)
            {
                try { 
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
                                    //testing remove later
                                    Console.WriteLine(data);
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
            }
        }
    }
}
