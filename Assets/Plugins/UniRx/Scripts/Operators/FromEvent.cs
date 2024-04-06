using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniRx.Operators
{
    // FromEvent, FromEventPattern

    internal class FromEventPatternObservable<TDelegate, TEventArgs> : OperatorObservableBase<EventPattern<TEventArgs>>
        where TEventArgs : EventArgs
    {
        private readonly Func<EventHandler<TEventArgs>, TDelegate> conversion;
        private readonly Action<TDelegate> addHandler;
        private readonly Action<TDelegate> removeHandler;

        public FromEventPatternObservable(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
            : base(false)
        {
            this.conversion = conversion;
            this.addHandler = addHandler;
            this.removeHandler = removeHandler;
        }

        protected override IDisposable SubscribeCore(IObserver<EventPattern<TEventArgs>> observer, IDisposable cancel)
        {
            var fe = new FromEventPattern(this, observer);
            return fe.Register() ? fe : Disposable.Empty;
        }

        private class FromEventPattern : IDisposable
        {
            private readonly FromEventPatternObservable<TDelegate, TEventArgs> parent;
            private readonly IObserver<EventPattern<TEventArgs>> observer;
            private TDelegate handler;

            public FromEventPattern(FromEventPatternObservable<TDelegate, TEventArgs> parent, IObserver<EventPattern<TEventArgs>> observer)
            {
                this.parent = parent;
                this.observer = observer;
            }

            public bool Register()
            {
                handler = parent.conversion(OnNext);
                try
                {
                    parent.addHandler(handler);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    return false;
                }

                return true;
            }

            private void OnNext(object sender, TEventArgs eventArgs)
            {
                observer.OnNext(new EventPattern<TEventArgs>(sender, eventArgs));
            }

            public void Dispose()
            {
                if (handler != null)
                {
                    parent.removeHandler(handler);
                    handler = default;
                }
            }
        }
    }

    internal class FromEventObservable<TDelegate> : OperatorObservableBase<Unit>
    {
        private readonly Func<Action, TDelegate> conversion;
        private readonly Action<TDelegate> addHandler;
        private readonly Action<TDelegate> removeHandler;

        public FromEventObservable(Func<Action, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
            : base(false)
        {
            this.conversion = conversion;
            this.addHandler = addHandler;
            this.removeHandler = removeHandler;
        }

        protected override IDisposable SubscribeCore(IObserver<Unit> observer, IDisposable cancel)
        {
            var fe = new FromEvent(this, observer);
            return fe.Register() ? fe : Disposable.Empty;
        }

        private class FromEvent : IDisposable
        {
            private readonly FromEventObservable<TDelegate> parent;
            private readonly IObserver<Unit> observer;
            private TDelegate handler;

            public FromEvent(FromEventObservable<TDelegate> parent, IObserver<Unit> observer)
            {
                this.parent = parent;
                this.observer = observer;
            }

            public bool Register()
            {
                handler = parent.conversion(OnNext);

                try
                {
                    parent.addHandler(handler);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    return false;
                }

                return true;
            }

            private void OnNext()
            {
                observer.OnNext(Unit.Default);
            }

            public void Dispose()
            {
                if (handler != null)
                {
                    parent.removeHandler(handler);
                    handler = default;
                }
            }
        }
    }

    internal class FromEventObservable<TDelegate, TEventArgs> : OperatorObservableBase<TEventArgs>
    {
        private readonly Func<Action<TEventArgs>, TDelegate> conversion;
        private readonly Action<TDelegate> addHandler;
        private readonly Action<TDelegate> removeHandler;

        public FromEventObservable(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
            : base(false)
        {
            this.conversion = conversion;
            this.addHandler = addHandler;
            this.removeHandler = removeHandler;
        }

        protected override IDisposable SubscribeCore(IObserver<TEventArgs> observer, IDisposable cancel)
        {
            var fe = new FromEvent(this, observer);
            return fe.Register() ? fe : Disposable.Empty;
        }

        private class FromEvent : IDisposable
        {
            private readonly FromEventObservable<TDelegate, TEventArgs> parent;
            private readonly IObserver<TEventArgs> observer;
            private TDelegate handler;

            public FromEvent(FromEventObservable<TDelegate, TEventArgs> parent, IObserver<TEventArgs> observer)
            {
                this.parent = parent;
                this.observer = observer;
            }

            public bool Register()
            {
                handler = parent.conversion(OnNext);

                try
                {
                    parent.addHandler(handler);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    return false;
                }

                return true;
            }

            private void OnNext(TEventArgs args)
            {
                observer.OnNext(args);
            }

            public void Dispose()
            {
                if (handler != null)
                {
                    parent.removeHandler(handler);
                    handler = default;
                }
            }
        }
    }

    internal class FromEventObservable : OperatorObservableBase<Unit>
    {
        private readonly Action<Action> addHandler;
        private readonly Action<Action> removeHandler;

        public FromEventObservable(Action<Action> addHandler, Action<Action> removeHandler)
            : base(false)
        {
            this.addHandler = addHandler;
            this.removeHandler = removeHandler;
        }

        protected override IDisposable SubscribeCore(IObserver<Unit> observer, IDisposable cancel)
        {
            var fe = new FromEvent(this, observer);
            return fe.Register() ? fe : Disposable.Empty;
        }

        private class FromEvent : IDisposable
        {
            private readonly FromEventObservable parent;
            private readonly IObserver<Unit> observer;
            private Action handler;

            public FromEvent(FromEventObservable parent, IObserver<Unit> observer)
            {
                this.parent = parent;
                this.observer = observer;
                handler = OnNext;
            }

            public bool Register()
            {
                try
                {
                    parent.addHandler(handler);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    return false;
                }

                return true;
            }

            private void OnNext()
            {
                observer.OnNext(Unit.Default);
            }

            public void Dispose()
            {
                if (handler != null)
                {
                    parent.removeHandler(handler);
                    handler = null;
                }
            }
        }
    }

    internal class FromEventObservable_<T> : OperatorObservableBase<T>
    {
        private readonly Action<Action<T>> addHandler;
        private readonly Action<Action<T>> removeHandler;

        public FromEventObservable_(Action<Action<T>> addHandler, Action<Action<T>> removeHandler)
            : base(false)
        {
            this.addHandler = addHandler;
            this.removeHandler = removeHandler;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            var fe = new FromEvent(this, observer);
            return fe.Register() ? fe : Disposable.Empty;
        }

        private class FromEvent : IDisposable
        {
            private readonly FromEventObservable_<T> parent;
            private readonly IObserver<T> observer;
            private Action<T> handler;

            public FromEvent(FromEventObservable_<T> parent, IObserver<T> observer)
            {
                this.parent = parent;
                this.observer = observer;
                handler = OnNext;
            }

            public bool Register()
            {
                try
                {
                    parent.addHandler(handler);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    return false;
                }

                return true;
            }

            private void OnNext(T value)
            {
                observer.OnNext(value);
            }

            public void Dispose()
            {
                if (handler != null)
                {
                    parent.removeHandler(handler);
                    handler = null;
                }
            }
        }
    }
}