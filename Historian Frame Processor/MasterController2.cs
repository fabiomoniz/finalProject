using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;

namespace Historian_Frame
{
    class MasterController2 : IHostedService, IDisposable
    {
        private bool ShutdownCommandGiven = false;
        private int CaseCount = 1;
        ExeptionHandelCases FailedHandel = new ExeptionHandelCases();
        object RawMessage;
        object parsedMessage;
        public Configuration cfg;

        //Forever loop to infinity and beyond!!

        public void Run()
        {
            // Load config settings

            // Validate QueueManager
            try
            {
                // Create QueueManager 
                QueueManager InfluxLoginManager = new QueueManager(cfg.InfluxPWN, cfg.InfluxHostname, cfg.InfluxUsername); // set pwd, login, name from the loaded Config file 
                QueueManager RabbitMQLoginManager = new QueueManager(cfg.RabbitMQPassword, cfg.RabbitMQHostName, cfg.RabbitMQUserName);
            }
            catch (Exception)
            {
                // close the program
                throw;
            }

            // Validate HistorianManager
            try
            {
                // Create HistorianManager
                HistorianManager InfluxHManager = new HistorianManager(cfg.InfluxPWN, cfg.InfluxHostname, cfg.InfluxUsername); // set pwd, login, name from the loaded Config file 
                HistorianManager RabbitMQHManager = new HistorianManager(cfg.RabbitMQPassword, cfg.RabbitMQHostName, cfg.RabbitMQUserName);

            }
            catch (Exception)
            {
                // Close the program
                throw;
            }

            while (ShutdownCommandGiven == false) 
            {
                try
                {
                    RawMessage = PollQueue();
                    if (RawMessage == null)
                    {
                        Delay();
                    }
                }
                catch (Exception)
                {
                    RawMessage = null;
                    Delay();
                    throw;
                }

                if (RawMessage != null && ShutdownCommandGiven == false)
                {
                    try
                    {
                        parsedMessage = ProcessMessage();
                    }
                    catch (Exception)
                    {
                        FailedHandel.SafeCourruptFile();
                        parsedMessage = null;
                        throw;
                    }
                }

                if (parsedMessage != null && ShutdownCommandGiven == false)
                {
                    try
                    {
                        PublishHistorien();
                        FailedHandel.ReleaseAndDelete();
                    }
                    catch (Exception)
                    {
                        FailedHandel.SafeCourruptFile();
                        throw;
                    }
                }

                // Clean-up
                RawMessage = null;
                parsedMessage = null;


            } // End while loop
        } // End of Run()

        private void PublishHistorien()
        {
            throw new NotImplementedException();
        }

        private object ProcessMessage()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Shutdown() { ShutdownCommandGiven = true; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private void Delay()
        {
            Thread.Sleep(5000);
        }

        private object PollQueue()
        {
            return null;
        }
    }
}
