using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniRx.Operators
{
    internal class ThrottleObservable<T> : OperatorObservableBase<T>
    {
        private readonly IObservable<T> source;
        private readonly TimeSpan dueTime;
        private readonly IScheduler scheduler;

        public ThrottleObservable(IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
            : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread())
        {
            this.source = source;
            this.dueTime = dueTime;
            this.scheduler = scheduler;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            return new Throttle(this, observer, cancel).Run();
        }

        private class Throttle : OperatorObserverBase<T, T>
        {
            private readonly ThrottleObservable<T> parent;
            private readonly object gate = new();
            private T latestValue;
            private bool hasValue;
            private SerialDisposable cancelable;
            private ulong id;

            public Throttle(ThrottleObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
            {
                this.parent = parent;
            }

            public IDisposable Run()
            {
                cancelable = new SerialDisposable();
                var subscription = parent.source.Subscribe(this);

                return StableCompositeDisposable.Create(cancelable, subscription);
            }

            private void OnNext(ulong currentid)
            {
                lock (gate)
                {
                    if (hasValue && id == currentid)
                    {
                        observer.OnNext(latestValue);
                    }

                    hasValue = false;
                }
            }

            public override void OnNext(T value)
            {
                ulong currentid;
                lock (gate)
                {
                    hasValue = true;
                    latestValue = value;
                    id = unchecked(id + 1);
                    currentid = id;
                }

                var d = new SingleAssignmentDisposable();
                cancelable.Disposable = d;
                d.Disposable = parent.scheduler.Schedule(parent.dueTime, () => OnNext(currentid));
            }

            public override void OnError(Exception error)
            {
                cancelable.Dispose();

                lock (gate)
                {
                    hasValue = false;
                    id = unchecked(id + 1);
                    try
                    {
                        observer.OnError(error);
                    }
                    finally
                    {
                        Dispose();
                    }
                }
            }

            public override void OnCompleted()
            {
                cancelable.Dispose();

                lock (gate)
                {
                    if (hasValue)
                    {
                        observer.OnNext(latestValue);
                    }

                    hasValue = false;
                    id = unchecked(id + 1);
                    try
                    {
                        observer.OnCompleted();
                    }
                    finally
                    {
                        Dispose();
                    }
                }
            }
        }
    }
}