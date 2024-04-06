﻿using System;

namespace UniRx.Operators
{
    // Optimize for .Select().Where()

    internal class SelectWhereObservable<T, TR> : OperatorObservableBase<TR>
    {
        private readonly IObservable<T> source;
        private readonly Func<T, TR> selector;
        private readonly Func<TR, bool> predicate;

        public SelectWhereObservable(IObservable<T> source, Func<T, TR> selector, Func<TR, bool> predicate)
            : base(source.IsRequiredSubscribeOnCurrentThread())
        {
            this.source = source;
            this.selector = selector;
            this.predicate = predicate;
        }

        protected override IDisposable SubscribeCore(IObserver<TR> observer, IDisposable cancel)
        {
            return source.Subscribe(new SelectWhere(this, observer, cancel));
        }

        private class SelectWhere : OperatorObserverBase<T, TR>
        {
            private readonly SelectWhereObservable<T, TR> parent;

            public SelectWhere(SelectWhereObservable<T, TR> parent, IObserver<TR> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                this.parent = parent;
            }

            public override void OnNext(T value)
            {
                var v = default(TR);
                try
                {
                    v = parent.selector(value);
                }
                catch (Exception ex)
                {
                    try
                    {
                        observer.OnError(ex);
                    }
                    finally
                    {
                        Dispose();
                    }

                    return;
                }

                var isPassed = false;
                try
                {
                    isPassed = parent.predicate(v);
                }
                catch (Exception ex)
                {
                    try
                    {
                        observer.OnError(ex);
                    }
                    finally
                    {
                        Dispose();
                    }

                    return;
                }

                if (isPassed)
                {
                    observer.OnNext(v);
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
}