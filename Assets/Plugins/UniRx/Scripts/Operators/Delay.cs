using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniRx.Operators
{
    internal class DelayObservable<T> : OperatorObservableBase<T>
    {
        private readonly IObservable<T> source;
        private readonly TimeSpan dueTime;
        private readonly IScheduler scheduler;

        public DelayObservable(IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
            : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread())
        {
            this.source = source;
            this.dueTime = dueTime;
            this.scheduler = scheduler;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            return new Delay(this, observer, cancel).Run();
        }

        private class Delay : OperatorObserverBase<T, T>
        {
            private readonly DelayObservable<T> parent;
            private readonly object gate = new();
            private bool hasFailed;
            private bool running;
            private bool active;
            private Exception exception;
            private Queue<Timestamped<T>> queue;
            private bool onCompleted;
            private DateTimeOffset completeAt;
            private IDisposable sourceSubscription;
            private TimeSpan delay;
            private bool ready;
            private SerialDisposable cancelable;

            public Delay(DelayObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
            {
                this.parent = parent;
            }

            public IDisposable Run()
            {
                cancelable = new SerialDisposable();

                active = false;
                running = false;
                queue = new Queue<Timestamped<T>>();
                onCompleted = false;
                completeAt = default;
                hasFailed = false;
                exception = default;
                ready = true;
                delay = Scheduler.Normalize(parent.dueTime);

                var _sourceSubscription = new SingleAssignmentDisposable();
                sourceSubscription = _sourceSubscription; // assign to field
                _sourceSubscription.Disposable = parent.source.Subscribe(this);

                return StableCompositeDisposable.Create(sourceSubscription, cancelable);
            }

            public override void OnNext(T value)
            {
                var next = parent.scheduler.Now.Add(delay);
                var shouldRun = false;

                lock (gate)
                {
                    queue.Enqueue(new Timestamped<T>(value, next));

                    shouldRun = ready && !active;
                    active = true;
                }

                if (shouldRun)
                {
                    cancelable.Disposable = parent.scheduler.Schedule(delay, DrainQueue);
                }
            }

            public override void OnError(Exception error)
            {
                sourceSubscription.Dispose();

                var shouldRun = false;

                lock (gate)
                {
                    queue.Clear();

                    exception = error;
                    hasFailed = true;

                    shouldRun = !running;
                }

                if (shouldRun)
                {
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
                sourceSubscription.Dispose();

                var next = parent.scheduler.Now.Add(delay);
                var shouldRun = false;

                lock (gate)
                {
                    completeAt = next;
                    onCompleted = true;

                    shouldRun = ready && !active;
                    active = true;
                }

                if (shouldRun)
                {
                    cancelable.Disposable = parent.scheduler.Schedule(delay, DrainQueue);
                }
            }

            private void DrainQueue(Action<TimeSpan> recurse)
            {
                lock (gate)
                {
                    if (hasFailed) return;
                    running = true;
                }

                var shouldYield = false;

                while (true)
                {
                    var hasFailed = false;
                    var error = default(Exception);

                    var hasValue = false;
                    var value = default(T);
                    var hasCompleted = false;

                    var shouldRecurse = false;
                    var recurseDueTime = default(TimeSpan);

                    lock (gate)
                    {
                        if (hasFailed)
                        {
                            error = exception;
                            hasFailed = true;
                            running = false;
                        }
                        else
                        {
                            if (queue.Count > 0)
                            {
                                var nextDue = queue.Peek().Timestamp;

                                if (nextDue.CompareTo(parent.scheduler.Now) <= 0 && !shouldYield)
                                {
                                    value = queue.Dequeue().Value;
                                    hasValue = true;
                                }
                                else
                                {
                                    shouldRecurse = true;
                                    recurseDueTime = Scheduler.Normalize(nextDue.Subtract(parent.scheduler.Now));
                                    running = false;
                                }
                            }
                            else if (onCompleted)
                            {
                                if (completeAt.CompareTo(parent.scheduler.Now) <= 0 && !shouldYield)
                                {
                                    hasCompleted = true;
                                }
                                else
                                {
                                    shouldRecurse = true;
                                    recurseDueTime = Scheduler.Normalize(completeAt.Subtract(parent.scheduler.Now));
                                    running = false;
                                }
                            }
                            else
                            {
                                running = false;
                                active = false;
                            }
                        }
                    }

                    if (hasValue)
                    {
                        observer.OnNext(value);
                        shouldYield = true;
                    }
                    else
                    {
                        if (hasCompleted)
                        {
                            try
                            {
                                observer.OnCompleted();
                            }
                            finally
                            {
                                Dispose();
                            }
                        }
                        else if (hasFailed)
                        {
                            try
                            {
                                observer.OnError(error);
                            }
                            finally
                            {
                                Dispose();
                            }
                        }
                        else if (shouldRecurse)
                        {
                            recurse(recurseDueTime);
                        }

                        return;
                    }
                }
            }
        }
    }
}