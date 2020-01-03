using System;

namespace NarraTech_Insight.SharedObjects
{
    public class TimeSeriesPoint
    {
        /// <summary>
        /// Represents the time the measurement was taken
        /// </summary>
        public DateTime TimeStamp_UTC { get; set; }

        /// <summary>
        /// Represents the Quality of the measurement ex.. good, bad
        /// </summary>
        public string Quality { get; set; }


        public static TimeSeriesPoint CreatePoint(string _v, DateTime _ts, string qua, string dataTpye)
        {
            TimeSeriesPoint returnedObject;

            switch (dataTpye)
            {
                case "double":
                    returnedObject = new TimeSeriesPoint<double>();

                    ((TimeSeriesPoint<double>)returnedObject).Value = Convert.ToDouble(_v);
                    break;
                case "int":
                    returnedObject = new TimeSeriesPoint<int>();
                    ((TimeSeriesPoint<int>)returnedObject).Value = Convert.ToInt32(_v);
                    break;
                default:
                    return null;
            }

            returnedObject.Quality = qua;
            returnedObject.TimeStamp_UTC = _ts;

            return returnedObject;
        }
    }

        public class TimeSeriesPoint<T> : TimeSeriesPoint where T : struct 
        {
            /// <summary>
            /// Represents the value corresponding with the timestamp
            /// </summary>
            public T Value { get; set; }

        
        }
    }
