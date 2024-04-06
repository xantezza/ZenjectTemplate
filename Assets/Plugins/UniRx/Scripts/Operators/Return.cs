using System;

namespace UniRx.Operators
{
    internal class ReturnObservable<T> : OperatorObservableBase<T>
    {
        private readonly T value;
        private readonly IScheduler scheduler;

        public ReturnObservable(T value, IScheduler scheduler)
            : base(scheduler == Scheduler.CurrentThread)
        {
            this.value = value;
            this.scheduler = scheduler;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            observer = new Return(observer, cancel);

            if (scheduler == Scheduler.Immediate)
            {
                observer.OnNext(value);
                observer.OnCompleted();
                return Disposable.Empty;
            }

            return scheduler.Schedule(() =>
            {
                observer.OnNext(value);
                observer.OnCompleted();
            });
        }

        private class Return : OperatorObserverBase<T, T>
        {
            public Return(IObserver<T> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public override void OnNext(T value)
            {
                try
                {
                    observer.OnNext(value);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public override void OnError(Exception error)
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

            public override void OnCompleted()
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
        }
    }

    internal class ImmediateReturnObservable<T> : IObservable<T>, IOptimizedObservable<T>
    {
        private readonly T value;

        public ImmediateReturnObservable(T value)
        {
            this.value = value;
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnNext(value);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }

    internal class ImmutableReturnUnitObservable : IObservable<Unit>, IOptimizedObservable<Unit>
    {
        internal static ImmutableReturnUnitObservable Instance = new();

        private ImmutableReturnUnitObservable()
        {
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            observer.OnNext(Unit.Default);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }

    internal class ImmutableReturnTrueObservable : IObservable<bool>, IOptimizedObservable<bool>
    {
        internal static ImmutableReturnTrueObservable Instance = new();

        private ImmutableReturnTrueObservable()
        {
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            observer.OnNext(true);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }

    internal class ImmutableReturnFalseObservable : IObservable<bool>, IOptimizedObservable<bool>
    {
        internal static ImmutableReturnFalseObservable Instance = new();

        private ImmutableReturnFalseObservable()
        {
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            observer.OnNext(false);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }

    internal class ImmutableReturnInt32Observable : IObservable<int>, IOptimizedObservable<int>
    {
        private static readonly ImmutableReturnInt32Observable[] Caches =
        {
            new(-1),
            new(0),
            new(1),
            new(2),
            new(3),
            new(4),
            new(5),
            new(6),
            new(7),
            new(8),
            new(9)
        };

        public static IObservable<int> GetInt32Observable(int x)
        {
            if (-1 <= x && x <= 9)
            {
                return Caches[x + 1];
            }

            return new ImmediateReturnObservable<int>(x);
        }

        private readonly int x;

        private ImmutableReturnInt32Observable(int x)
        {
            this.x = x;
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            observer.OnNext(x);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}