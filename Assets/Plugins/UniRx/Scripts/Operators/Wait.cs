using System;
using UniRx.InternalUtil;

namespace UniRx.Operators
{
    internal class Wait<T> : IObserver<T>
    {
        private static readonly TimeSpan InfiniteTimeSpan = new(0, 0, 0, 0, -1); // from .NET 4.5

        private readonly IObservable<T> source;
        private readonly TimeSpan timeout;

        private System.Threading.ManualResetEvent semaphore;

        private bool seenValue;
        private T value;
        private Exception ex;

        public Wait(IObservable<T> source, TimeSpan timeout)
        {
            this.source = source;
            this.timeout = timeout;
        }

        public T Run()
        {
            semaphore = new System.Threading.ManualResetEvent(false);
            using (source.Subscribe(this))
            {
                var waitComplete = (timeout == InfiniteTimeSpan)
                    ? semaphore.WaitOne()
                    : semaphore.WaitOne(timeout);

                if (!waitComplete)
                {
                    throw new TimeoutException("OnCompleted not fired.");
                }
            }

            if (ex != null) ex.Throw();
            if (!seenValue) throw new InvalidOperationException("No Elements.");

            return value;
        }

        public void OnNext(T value)
        {
            seenValue = true;
            this.value = value;
        }

        public void OnError(Exception error)
        {
            ex = error;
            semaphore.Set();
        }

        public void OnCompleted()
        {
            semaphore.Set();
        }
    }
}