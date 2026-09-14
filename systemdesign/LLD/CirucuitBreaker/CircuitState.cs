using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemdesign.LLD.CirucuitBreaker
{
   

    internal class CircuitBreakerState
    {
        internal CircuitBreakerState() { }
        internal bool IsOpen { get; set; } = false;
        internal bool IsClosed { get; set; } = true;
        internal bool IsHalfOpen { get; set; } = false;
        private static readonly TimeSpan _openStateDuration = TimeSpan.FromSeconds(30); // Duration for which the circuit remains open


        internal int HalfOpenrequestInProgress { get; set; } = 0;

        private Object _lock = new Object();

        internal DateTime LastStateChangeTime { get; set; } = DateTime.Now;

        internal MetricsCollection MetricsCollection { get; set; } = new MetricsCollection();

        internal void TransitionToOpen()
        {
            lock (_lock)
            {
                IsOpen = true;
                IsClosed = false;
                IsHalfOpen = false;
                HalfOpenrequestInProgress=0;
                LastStateChangeTime = DateTime.UtcNow;
            }
        }

        internal void TransitionToClosed()
        {
            lock (_lock)
            {
                IsOpen = false;
                IsClosed = true;
                IsHalfOpen = false;
                HalfOpenrequestInProgress=0;
                LastStateChangeTime = DateTime.UtcNow;
            }
        }

        internal void TransitionToHalfOpen()
        {
            lock (_lock)
            {
                IsOpen = false;
                IsClosed = false;
                IsHalfOpen = true;
                HalfOpenrequestInProgress++;
                LastStateChangeTime = DateTime.UtcNow;
            }
        }



        internal bool isRequetsAllowed()
        {
            lock (_lock)
            {
                if (IsClosed) return true;
                if (IsHalfOpen && HalfOpenrequestInProgress >= 1) return false;

                else if (IsHalfOpen) return true;

                if (IsOpen && DateTime.UtcNow - LastStateChangeTime > _openStateDuration)
                {
                    IsOpen = false;
                    IsClosed = false;
                    IsHalfOpen = true;
                    HalfOpenrequestInProgress++;
                    LastStateChangeTime = DateTime.UtcNow;
                    return true;
                }

                return false;

            }

        }

            internal bool isCircuitOpen()
            {
                lock (_lock)
                {
                    return IsOpen;
                }
        }

        internal bool isCircuitClosed() {

            lock (_lock)
            {
                return IsClosed;
            }

        }

        internal bool isCircuitHalfOpen()
        {
            lock (_lock)
            {
                return IsHalfOpen;
            }
        }





        }
    }
}
