using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Historian_Frame
{
    public class Configuration
    {
        public string RabbitMQHostName { get; set; }

        public string InfluxHostname { get; set; }

        public string RabbitMQUserName { get; set; }

        public string InfluxUsername { get; set; }

        public string RabbitMQPassword { get; set; }

        public string InfluxPWN { get; set; }

        public string RabbitMQDatabaseName { get; set; }

        public string InfluxDatabaseName { get; set; }

        public int CalculationInterval { get; set; }

        public string ModelPath { get; set; }

        public int TimeOut { get; set; }

        public static Configuration ReadFrom(string fileName)
        {
            var content = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<Configuration>(content);
        }
    }
}