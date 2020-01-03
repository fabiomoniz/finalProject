using System;
using System.Collections.Generic;
using System.Text;

namespace Historian_Frame
{
    class ExeptionHandelCases
    {
        public object SafeCourruptFile() // Execption from 'Parse header' , 'Use parse based on version' and 'Sobmit to Historien'(beieng called when 'ConnectionError' have failed x number of times)
        {
            // safe corrupt file and run the 'ReleaseAndDelete' class after

            ReleaseAndDelete();
            return null;
        }

        public object ReleaseAndDelete() // run when 'Sobmit to Historien' action is sucess, or after 'SafeCorruptFile' class 
        {
            return null;
        }

        public object LogErrorAndRetry() // Execption from 'Reserve message' action
        {
            return null;
        }

        public object ConnectionError() // Execption from 'Ask Queue' and 'Sobmit to Historien' action(s)
        {
            return null;
        }
    }
}
