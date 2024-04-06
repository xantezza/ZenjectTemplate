using System;

namespace UniRx.Operators
{
    internal class ContinueWithObservable<TSource, TResult> : OperatorObservableBase<TResult>
    {
        private readonly IObservable<TSource> source;
        private readonly Func<TSource, IObservable<TResult>> selector;

        public ContinueWithObservable(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
            : base(source.IsRequiredSubscribeOnCurrentThread())
        {
            this.source = source;
            this.selector = selector;
        }

        protected override IDisposable SubscribeCore(IObserver<TResult> observer, IDisposable cancel)
        {
            return new ContinueWith(this, observer, cancel).Run();
        }

        private class ContinueWith : OperatorObserverBase<TSource, TResult>
        {
            private readonly ContinueWithObservable<TSource, TResult> parent;
            private readonly SerialDisposable serialDisposable = new();

            private bool seenValue;
            private TSource lastValue;

            public ContinueWith(ContinueWithObservable<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
            {
                this.parent = parent;
            }

            public IDisposable Run()
            {
                var sourceDisposable = new SingleAssignmentDisposable();
                serialDisposable.Disposable = sourceDisposable;

                sourceDisposable.Disposable = parent.source.Subscribe(this);
                return serialDisposable;
            }

            public override void OnNext(TSource value)
            {
                seenValue = true;
                lastValue = value;
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

                ;
            }

            public override void OnCompleted()
            {
                if (seenValue)
                {
                    try
                    {
                        var v = parent.selector(lastValue);
                        // dispose source subscription
                        serialDisposable.Disposable = v.Subscribe(observer);
                    }
                    catch (Exception error)
                    {
                        OnError(error);
                    }
                }
                else
                {
                    try
                    {
                        observer.OnCompleted();
                    }
                    finally
                    {
                        Dispose();
                    }

                    ;
                }
            }
        }
    }
}