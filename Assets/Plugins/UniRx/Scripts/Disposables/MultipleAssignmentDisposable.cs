using System;
using System.Collections;

namespace UniRx
{
    public sealed class MultipleAssignmentDisposable : IDisposable, ICancelable
    {
        private static readonly BooleanDisposable True = new(true);

        private readonly object gate = new();
        private IDisposable current;

        public bool IsDisposed
        {
            get
            {
                lock (gate)
                {
                    return current == True;
                }
            }
        }

        public IDisposable Disposable
        {
            get
            {
                lock (gate)
                {
                    return (current == True)
                        ? UniRx.Disposable.Empty
                        : current;
                }
            }
            set
            {
                var shouldDispose = false;
                lock (gate)
                {
                    shouldDispose = (current == True);
                    if (!shouldDispose)
                    {
                        current = value;
                    }
                }

                if (shouldDispose && value != null)
                {
                    value.Dispose();
                }
            }
        }

        public void Dispose()
        {
            IDisposable old = null;

            lock (gate)
            {
                if (current != True)
                {
                    old = current;
                    current = True;
                }
            }

            if (old != null) old.Dispose();
        }
    }
}