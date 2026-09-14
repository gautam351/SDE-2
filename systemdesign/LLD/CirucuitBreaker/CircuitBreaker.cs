using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemdesign.LLD.CirucuitBreaker
{
    public sealed  class CircuitBreaker
    {
        private static volatile CircuitBreaker _instance ;

        private static readonly object _lock = new object();


        private readonly ConcurrentDictionary<string, CircuitBreakerState> _circuitBreakerState;

        private CircuitBreaker()
        {
            _circuitBreakerState = new ConcurrentDictionary<string, CircuitBreakerState>();
        }


        public static  CircuitBreaker GetInstance()
        {
            if (_instance == null)
            {
                lock(_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CircuitBreaker();
                    }
                }
            }

            return  _instance;

        }


        public bool IsAllowed(string serviceName)
        {
            if (_circuitBreakerState.TryGetValue(serviceName, out var state))
            {
                
                return state.isRequetsAllowed();


            }
            return false;
        }



        public void RecordSuccess(string serviceName,long unixTimestamp, int isSlowCount=0, int count=1)
        {
            if(_circuitBreakerState.TryGetValue(serviceName, out var state))
            {
               

                    state.MetricsCollection.publishResult(count, 0, isSlowCount, unixTimestamp); 
                    var metrics = state.MetricsCollection.GetMetricsSnapshot();

                    if (state.isCircuitHalfOpen() )
                    {
                        state.TransitionToClosed();
                }
                    

                
            }
        }


        public void RecordFailure(string serviceName, long unixTimestamp, int isSlowCount = 0, int count = 1)
        {
            if (_circuitBreakerState.TryGetValue(serviceName, out var state))
            {
              
                    state.MetricsCollection.publishResult(0, count, isSlowCount, unixTimestamp);
                    var metrics = state.MetricsCollection.GetMetricsSnapshot();
                    if(state.isCircuitHalfOpen())
                    {
                       state.TransitionToOpen();
                    }
                    else if (state.isCircuitClosed() && metrics.FailureRate > 0.5)
                    {
                        state.TransitionToOpen();
                    }




                

            }
        }


        public  MetricsSnapshot GetMetricSnapshot(string serviceName)
        {
            if (_circuitBreakerState.TryGetValue(serviceName, out var state))
            {
               
                    return state.MetricsCollection.GetMetricsSnapshot();
                
            }
            return null;
        }











    }
}
