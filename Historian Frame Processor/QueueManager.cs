using System;
using System.Collections.Generic;
using System.Text;

namespace Historian_Frame
{
    public class QueueManager
    {
        private string hostname;
        private int port;
        private string QueueName, QueueLogin, QueuePWD;

        public QueueManager(string _password, string _Login, string _Name)
        {
            this.QueuePWD = _password;
            this.QueueName = _Name;
            this.QueueLogin = _Login;
        }

        ~QueueManager() { }

        public int GetNumberOfMessages()
        {
            return 0;
        }

        public object ReserveMessage()
        {
            return null;
        }

        public bool ReleaseMessage()
        {
            return false;
        }

        private void ConnectToQueue()
        {

        }

        private void DisconnectFromQueue()
        {
            
        }

        private bool IsConnected()
        {
            return false;
        }
    }
}
