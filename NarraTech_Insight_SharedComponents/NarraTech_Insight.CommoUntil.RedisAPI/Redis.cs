using NarraTech_Insight.SharedObjects;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace NarraTech_Insight.CommoUntil.RedisAPI
{
    public abstract class RedisConnection
    {
        private readonly string Host_endPoint;
        private readonly int Port;
        private readonly string Password;
        private readonly int ConnectTimeout;
        private readonly int SyncTimeOut;

        protected ConnectionMultiplexer connection = null;
        protected IDatabase redisDatabase = null;
        public RedisConnection
            (
                string _hostname,
                string _password,
                int _syncTimeout = 1000,
                int _connectTimeout = 1000,
                int _port = 6379
            )
        {
            this.Password = _password;
            this.Host_endPoint = _hostname;
            this.ConnectTimeout = _connectTimeout;
            this.SyncTimeOut = _syncTimeout;
            this.Port = _port;
        }

        /// <summary>
        /// Connects to redis 
        /// </summary>
        /// <returns>return state of connection sucessestion</returns>
        protected bool Connect()
        {
            var options = new ConfigurationOptions
            {
                AbortOnConnectFail = true,
                ConnectRetry = 3,
                ConnectTimeout = ConnectTimeout,
                SyncTimeout = SyncTimeOut,
                EndPoints = { Host_endPoint }, //future refence for using ports { "167.71.73.16" , 6379}
                Password = Password
            };


            //check connection is not null
            if (connection != null)
            {
                //check is connection is already connected
                if (!connection.IsConnected)
                {
                    //dispose current connection to avoid conflits
                    connection.Dispose();
                    try
                    {
                        connection = ConnectionMultiplexer.Connect(options);
                        return true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            else
            {
                try
                {
                    connection = ConnectionMultiplexer.Connect(options);
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// returns state of database connection
        /// </summary>
        /// <returns>returns true or false if connection is on or off respectively</returns>
        protected bool IsDatabaseSet()
        {
            //check database connection is null
            if (redisDatabase == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// establish connection to redis database of your choice
        /// </summary>
        /// <param name="index"></param>
        /// <returns>returns "IDatabase" redis database instance with the provided index</returns>
        protected IDatabase GetDatabase(int index)
        {
            // Making sure we're connected to an open connection.
            if (!IsConnected())
            {
                try
                {
                    Connect();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            // Get database from valid connection.
            if (!IsDatabaseSet())
                try
                {
                    redisDatabase = connection.GetDatabase(index);
                    return redisDatabase;
                }
                catch (Exception)
                {
                    throw;
                }

            return redisDatabase;
        }

        /// <summary>
        /// checks if the is a connection
        /// </summary>
        /// <returns>returns state of connection</returns>
        protected bool IsConnected()
        {
            //checks if connection is not null
            if (connection != null)
            {
                //checks if connection is connected 
                if (connection.IsConnected)
                {
                    return true;
                }
            }
            return false;
        }

        //this method was created to avoid repetive code
        /// <summary>
        /// connect to redis and get database(int index)
        /// </summary>
        /// <param name="index">index of desired database, 0 = destinationid, 1 = measuringmetadata, 2 = compressionObject, 3 = transformationobject, 4= currentvalues</param>
        protected void ConnectRedisAndConnectDatabase(int index)
        {
            // check if connected
            if (!IsConnected())
            {
                try
                {
                    // Connect to Redis.
                    Connect();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            if (!IsDatabaseSet())
            {
                try
                {
                    GetDatabase(index);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// disconnects redis
        /// </summary>
        protected void Disconnect()
        {
            //check if connection is not null to prevent a close() on a null object
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                redisDatabase = null;
                connection = null;
            }
        }

        /// <summary>
        /// disconnects database
        /// </summary>
        protected void DisconnectDatabase()
        {
            redisDatabase = null;
        }

        /// <summary>
        /// get the list of destinationID/s for the specified sourceiD
        /// </summary>
        /// <param name="sourceID"></param>
        /// <returns>List<string></returns>
        protected List<string> getDestinationID(string sourceID)
        {
            try
            {
                ConnectRedisAndConnectDatabase(0);
            }
            catch (RedisConnectionException)
            {
                Console.WriteLine("unable to connect enter corrent hostname and/or password");
                throw;
            }

            //create local variable to return
            List<string> destinationIDList;

            try
            {
                destinationIDList =
                JsonConvert.DeserializeObject<List<string>>(
                    connection.GetDatabase().
                    StringGet(sourceID));
            }
            catch (Exception)
            {

                throw;
            }

            DisconnectDatabase();
            return destinationIDList;
        }

        /// <summary>
        /// sets a list of destination IDs
        /// </summary>
        /// <param name="sourceID"></param>
        /// <param name="list"></param>
        protected void setDestinationID(string sourceID, List<string> list)
        {
            try
            {
                ConnectRedisAndConnectDatabase(0);
            }
            catch (RedisConnectionException)
            {
                Console.WriteLine("unable to connect enter corrent hostname and/or password");
                throw;
            }

            redisDatabase.StringSet(sourceID, JsonConvert.SerializeObject(list));
            DisconnectDatabase();
        }

        /// <summary>
        /// sets measuring point meta data
        /// </summary>
        /// <param name="destinationID"></param>
        /// <param name="obj"></param>
        protected void setMeasuringPointMetaData(string destinationID, MeasuringPointMetaData obj)
        {
            try
            {
                ConnectRedisAndConnectDatabase(1);
            }
            catch (RedisConnectionException)
            {
                Console.WriteLine("unable to connect enter corrent hostname and/or password");
                throw;
            }

            redisDatabase.StringSet(destinationID, JsonConvert.SerializeObject(obj));
            DisconnectDatabase();
        }

        /// <summary>
        /// gets the measuring point meta data
        /// </summary>
        /// <param name="destinationID"></param>
        /// <returns></returns>
        protected MeasuringPointMetaData getMeasuringPointMetaData(string destinationID)
        {
            try
            {
                ConnectRedisAndConnectDatabase(1);
            }
            catch (RedisConnectionException)
            {
                Console.WriteLine("unable to connect enter corrent hostname and/or password");
                throw;
            }

            var data = redisDatabase.StringGet(destinationID);

            DisconnectDatabase();
            Disconnect();
            return JsonConvert.DeserializeObject<MeasuringPointMetaData>(data);
            
        }

        /// <summary>
        /// sets a compression object 
        /// </summary>
        /// <param name="destinationID"></param>
        /// <param name="obj"></param>
        protected void setCompressionObject(string destinationID, CompressionObject obj)
        {
            try
            {
                ConnectRedisAndConnectDatabase(2);
            }
            catch (RedisConnectionException)
            {
                Console.WriteLine("unable to connect enter corrent hostname and/or password");
                throw;
            }

            redisDatabase.StringSet(destinationID, JsonConvert.SerializeObject(obj));
            DisconnectDatabase();
        }

        /// <summary>
        /// gets a compression object
        /// </summary>
        /// <param name="destinationID"></param>
        /// <returns></returns>
        protected CompressionObject getCompressionObject(string destinationID)
        {
            try
            {
                ConnectRedisAndConnectDatabase(2);
            }
            catch (RedisConnectionException)
            {
                Console.WriteLine("unable to connect enter corrent hostname and/or password");
                throw;
            }

            var data = redisDatabase.StringGet(destinationID);

            //terminate redis connection
            Disconnect();

            DisconnectDatabase();
            return JsonConvert.DeserializeObject<CompressionObject>(data);
        }

        /// <summary>
        /// set a transformation object
        /// </summary>
        /// <param name="destinationID"></param>
        /// <param name="obj"></param>
        protected void setTransformationObject(string destinationID, TransformationObject obj)
        {
            try
            {
                ConnectRedisAndConnectDatabase(3);
            }
            catch (RedisConnectionException)
            {
                Console.WriteLine("unable to connect enter corrent hostname and/or password");
                throw;
            }

            redisDatabase.StringSet(destinationID, JsonConvert.SerializeObject(obj));
            DisconnectDatabase();
        }

        /// <summary>
        /// gets transformation object
        /// </summary>
        /// <param name="destinationID"></param>
        /// <returns></returns>
        protected TransformationObject getTransformationObject(string destinationID)
        {
            try
            {
                ConnectRedisAndConnectDatabase(3);
            }
            catch (RedisConnectionException)
            {
                Console.WriteLine("unable to connect enter corrent hostname and/or password");
                throw;
            }

            var data = redisDatabase.StringGet(destinationID);

            DisconnectDatabase();
            return JsonConvert.DeserializeObject<TransformationObject>(data);
        }

        /// <summary>
        /// get the current value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destinationID"></param>
        /// <returns></returns>
        protected TimeSeriesPoint getCurrentValue(string destinationID)
        {
            try
            {
                ConnectRedisAndConnectDatabase(4);
            }
            catch (RedisConnectionException)
            {
                Console.WriteLine("unable to connect enter corrent hostname and/or password");
                throw;
            }

            var data = redisDatabase.StringGet(destinationID);

            DisconnectDatabase();
            return JsonConvert.DeserializeObject<TimeSeriesPoint>(data);
        }

        /// <summary>
        /// set a new current value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newPoint"></param>
        /// <param name="destinationID"></param>
        protected void setCurrentValue(string destinationID, TimeSeriesPoint newPoint)
        {
            try
            {
                ConnectRedisAndConnectDatabase(4);
            }
            catch (RedisConnectionException)
            {
                Console.WriteLine("unable to connect enter corrent hostname and/or password");
                throw;
            }

            redisDatabase.StringSet(destinationID, JsonConvert.SerializeObject(newPoint));
            DisconnectDatabase();

        }



    }

    public class RedisConnect : RedisConnection
    {
        public RedisConnect(string hostname, string password) : base(hostname, password)
        {

        }

        /// <summary>
        /// Sets a destinationId 
        /// </summary>
        /// <param name="sourceID"></param>
        /// <param name="list"></param>
        public void SetDestinationId(string sourceID, List<string> destinationIdList)
        {
            setDestinationID(sourceID, destinationIdList);
            Disconnect();

        }

        /// <summary>
        /// returns a list of destinaton ids
        /// </summary>
        /// <param name="sourceID"></param>
        /// <returns></returns>
        public List<string> GetDestinationId(String sourceID)
        {
            Disconnect();
            return getDestinationID(sourceID);
        }

        /// <summary>
        /// sets a measureing point
        /// </summary>
        /// <param name="destinationId"></param>
        /// <param name="obj"></param>
        public void SetMeasurementPoint(string destinationId, MeasuringPointMetaData obj)
        {
            setMeasuringPointMetaData(destinationId, obj);
            Disconnect();
        }

        /// <summary>
        /// cycles through the list of given destinations id and returns a list of measurement points
        /// </summary>
        /// <param name="destinationIdList"></param>
        /// <returns></returns>
        public List<MeasuringPointMetaData> GetMeasurementPoints(List<string> destinationIdList)
        {
            List<MeasuringPointMetaData> MetaDataList = new List<MeasuringPointMetaData>();

            foreach (string destinationID in destinationIdList)
            {
                MetaDataList.Add(getMeasuringPointMetaData(destinationID));
            }
            Disconnect();
            return MetaDataList;
        }

        /// <summary>
        /// sets a compression object
        /// </summary>
        /// <param name="destinationId"></param>
        /// <param name="obj"></param>
        public void SetCompressionObject(string destinationId, CompressionObject obj)
        {
            setCompressionObject(destinationId, obj);
            Disconnect();
        }

        /// <summary>
        /// cycles through the list of given destinations id and returns a list of compression objects
        /// </summary>
        /// <param name="destinationIdList"></param>
        /// <returns></returns>
        public List<CompressionObject> GetCompressionobjects(List<string> destinationIdList)
        {
            List<CompressionObject> CompressionList = new List<CompressionObject>();

            foreach (string destinationID in destinationIdList)
            {
                CompressionList.Add(getCompressionObject(destinationID));
            }

            Disconnect();
            return CompressionList;
        }

        /// <summary>
        /// set a transformation object
        /// </summary>
        /// <param name="destinationId"></param>
        /// <param name="obj"></param>
        public void SetTransformationObject(string destinationId, TransformationObject obj)
        {
            setTransformationObject(destinationId, obj);
            Disconnect();
        }

        /// <summary>
        /// cycles through the list of given destinations id and returns a list of transformation object
        /// </summary>
        /// <param name="destinationIdList"></param>
        /// <returns></returns>
        public List<TransformationObject> GetTransformationObjects(List<string> destinationIdList)
        {
            List<TransformationObject> TransformationList = new List<TransformationObject>();

            foreach (string destinationID in destinationIdList)
            {
                TransformationList.Add(getTransformationObject(destinationID));
            }
            Disconnect();
            return TransformationList;
        }

        /// <summary>
        /// set a current value object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destinationId"></param>
        /// <param name="obj"></param>
        public void SetCurrentValue(string destinationId, TimeSeriesPoint obj)
        {
            setCurrentValue(destinationId, obj);
            Disconnect();
        }

        /// <summary>
        /// cycles through the list of given destinations id and returns a list of current values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destinationIdList"></param>
        /// <returns></returns>
        public List<TimeSeriesPoint> GetCurrentValue(List<string> destinationIdList)
        {
            List<TimeSeriesPoint> CurrentValueList = new List<TimeSeriesPoint>();

            foreach (string destinationID in destinationIdList)
            {
                CurrentValueList.Add(getCurrentValue(destinationID));
            }
            Disconnect();
            return CurrentValueList;
        }
    }
}
