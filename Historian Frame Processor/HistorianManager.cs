using System;
using System.Collections.Generic;
using System.Text;

namespace Historian_Frame
{
    class HistorianManager
    {
        private string hostname;
        private int port;
        private string QueueName, QueueLogin, QueuePWD;

        public HistorianManager(string _password, string _Login, string _Name) 
        {
            this.QueuePWD = _password;
            this.QueueName = _Name;
            this.QueueLogin = _Login;
        }

        public bool PublishMessage()
        {
            return false; // change later
        }
    }
}
