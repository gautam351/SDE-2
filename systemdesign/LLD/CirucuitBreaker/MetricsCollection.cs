using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemdesign.LLD.CirucuitBreaker
{
    internal class MetricsCollection
    {

        public  readonly Bucket[] Buckets = new Bucket[60];

        private object _lock = new object();

        public MetricsCollection() { }

        public void publishResult(int successCount,int failureCount,int slowCount, long unixTimeStamp)
        {
            int bucketIndx=(int)(unixTimeStamp % 60);
            lock (_lock)
            {

                if (Buckets[bucketIndx] == null)
                {
                    Buckets[bucketIndx] = new Bucket
                    {
                        SuccessCount = successCount,
                        FailureCount = failureCount,
                        SlowCount = slowCount,
                        BucketTimeUnix = unixTimeStamp
                    };
                }
                else
                {
                    if (Buckets[bucketIndx].BucketTimeUnix == unixTimeStamp)
                    {
                        Buckets[bucketIndx].SuccessCount += successCount;
                        Buckets[bucketIndx].FailureCount += failureCount;
                        Buckets[bucketIndx].SlowCount += slowCount;

                    }
                    else
                    {
                        Buckets[bucketIndx].SuccessCount = successCount;
                        Buckets[bucketIndx].FailureCount = failureCount;
                        Buckets[bucketIndx].SlowCount = slowCount;
                        Buckets[bucketIndx].BucketTimeUnix = unixTimeStamp;
                    }

                }


            }


        }



        public MetricsSnapshot GetMetricsSnapshot()
        {
            var result = new MetricsSnapshot();

            lock (_lock){
                long currentUnixTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                foreach (var bucket in Buckets)
                {
                    if (bucket != null && currentUnixTimeStamp - bucket.BucketTimeUnix <= 60)
                    {
                        result.TotalCalls += bucket.SuccessCount + bucket.FailureCount;
                        result.SuccessfulCalls += bucket.SuccessCount;
                        result.FailedCalls += bucket.FailureCount;
                        result.SlowCalls += bucket.SlowCount;
                    }
                }
            }

            result.FailureRate = result.TotalCalls > 0 ? (double)result.FailedCalls / result.TotalCalls : 0;
            result.SlowCallRate = result.TotalCalls > 0 ? (double)result.SlowCalls / result.TotalCalls : 0;
            result.AvgResponseTime = result.TotalCalls > 0 ? (double)(result.SuccessfulCalls + result.FailedCalls + result.SlowCalls) / result.TotalCalls : 0;

            return result;

        }




    }
}
