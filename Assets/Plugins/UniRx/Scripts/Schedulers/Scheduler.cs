using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UniRx
{
    // Scheduler Extension
    public static partial class Scheduler
    {
        // configurable defaults
        public static class DefaultSchedulers
        {
            private static IScheduler constantTime;

            public static IScheduler ConstantTimeOperations
            {
                get => constantTime ?? (constantTime = Immediate);
                set => constantTime = value;
            }

            private static IScheduler tailRecursion;

            public static IScheduler TailRecursion
            {
                get => tailRecursion ?? (tailRecursion = Immediate);
                set => tailRecursion = value;
            }

            private static IScheduler iteration;

            public static IScheduler Iteration
            {
                get => iteration ?? (iteration = CurrentThread);
                set => iteration = value;
            }

            private static IScheduler timeBasedOperations;

            public static IScheduler TimeBasedOperations
            {
                get
                {
#if UniRxLibrary
                    return timeBasedOperations ?? (timeBasedOperations = Scheduler.ThreadPool);
#else
                    return timeBasedOperations ?? (timeBasedOperations = MainThread); // MainThread as default for TimeBased Operation
#endif
                }
                set { timeBasedOperations = value; }
            }

            private static IScheduler asyncConversions;

            public static IScheduler AsyncConversions
            {
                get
                {
#if WEB_GL
                    // WebGL does not support threadpool
                    return asyncConversions ?? (asyncConversions = Scheduler.MainThread);
#else
                    return asyncConversions ?? (asyncConversions = ThreadPool);
#endif
                }
                set { asyncConversions = value; }
            }

            public static void SetDotNetCompatible()
            {
                ConstantTimeOperations = Immediate;
                TailRecursion = Immediate;
                Iteration = CurrentThread;
                TimeBasedOperations = ThreadPool;
                AsyncConversions = ThreadPool;
            }
        }

        // utils

        public static DateTimeOffset Now => DateTimeOffset.UtcNow;

        public static TimeSpan Normalize(TimeSpan timeSpan)
        {
            return timeSpan >= TimeSpan.Zero ? timeSpan : TimeSpan.Zero;
        }

        public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action action)
        {
            return scheduler.Schedule(dueTime - scheduler.Now, action);
        }

        public static IDisposable Schedule(this IScheduler scheduler, Action<Action> action)
        {
            // InvokeRec1
            var group = new CompositeDisposable(1);
            var gate = new object();

            Action recursiveAction = null;
            recursiveAction = () => action(() =>
            {
                var isAdded = false;
                var isDone = false;
                var d = default(IDisposable);
                d = scheduler.Schedule(() =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                            group.Remove(d);
                        else
                            isDone = true;
                    }

                    recursiveAction();
                });

                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            group.Add(scheduler.Schedule(recursiveAction));

            return group;
        }

        public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action<Action<TimeSpan>> action)
        {
            // InvokeRec2

            var group = new CompositeDisposable(1);
            var gate = new object();

            Action recursiveAction = null;
            recursiveAction = () => action(dt =>
            {
                var isAdded = false;
                var isDone = false;
                var d = default(IDisposable);
                d = scheduler.Schedule(dt, () =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                            group.Remove(d);
                        else
                            isDone = true;
                    }

                    recursiveAction();
                });

                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            group.Add(scheduler.Schedule(dueTime, recursiveAction));

            return group;
        }

        public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action<Action<DateTimeOffset>> action)
        {
            // InvokeRec3

            var group = new CompositeDisposable(1);
            var gate = new object();

            Action recursiveAction = null;
            recursiveAction = () => action(dt =>
            {
                var isAdded = false;
                var isDone = false;
                var d = default(IDisposable);
                d = scheduler.Schedule(dt, () =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                            group.Remove(d);
                        else
                            isDone = true;
                    }

                    recursiveAction();
                });

                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            group.Add(scheduler.Schedule(dueTime, recursiveAction));

            return group;
        }
    }
}