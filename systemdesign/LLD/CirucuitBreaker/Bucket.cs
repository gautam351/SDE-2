using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemdesign.LLD.CirucuitBreaker
{
    internal class Bucket
    {
        public Bucket() { }
        internal int SuccessCount { get; set; }=0;
        internal int FailureCount { get; set; }=0;

        internal int SlowCount { get; set; }=0;

        internal long BucketTimeUnix { get; set; }


    }

    public class MetricsSnapshot
    {
       internal  long TotalCalls;
       internal  long SuccessfulCalls;
       internal  long FailedCalls;
       internal  long SlowCalls;
       internal  double FailureRate;
       internal  double SlowCallRate;
       internal  double AvgResponseTime;
    }
}
