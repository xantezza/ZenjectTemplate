using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
    internal class DelaySubscriptionObservable<T> : OperatorObservableBase<T>
    {
        private readonly IObservable<T> source;
        private readonly IScheduler scheduler;
        private readonly TimeSpan? dueTimeT;
        private readonly DateTimeOffset? dueTimeD;

        public DelaySubscriptionObservable(IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
            : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread())
        {
            this.source = source;
            this.scheduler = scheduler;
            dueTimeT = dueTime;
        }

        public DelaySubscriptionObservable(IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler)
            : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread())
        {
            this.source = source;
            this.scheduler = scheduler;
            dueTimeD = dueTime;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            if (dueTimeT != null)
            {
                var d = new MultipleAssignmentDisposable();
                var dt = Scheduler.Normalize(dueTimeT.Value);

                d.Disposable = scheduler.Schedule(dt, () => { d.Disposable = source.Subscribe(observer); });

                return d;
            }
            else
            {
                var d = new MultipleAssignmentDisposable();

                d.Disposable = scheduler.Schedule(dueTimeD.Value, () => { d.Disposable = source.Subscribe(observer); });

                return d;
            }
        }
    }
}