using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UniRx
{
#if UniRxLibrary
    public static partial class SchedulerUnity
    {
#else
    public static partial class Scheduler
    {
        public static void SetDefaultForUnity()
        {
            DefaultSchedulers.ConstantTimeOperations = Immediate;
            DefaultSchedulers.TailRecursion = Immediate;
            DefaultSchedulers.Iteration = CurrentThread;
            DefaultSchedulers.TimeBasedOperations = MainThread;
            DefaultSchedulers.AsyncConversions = ThreadPool;
        }
#endif
        private static IScheduler mainThread;

        /// <summary>
        ///     Unity native MainThread Queue Scheduler. Run on mainthread and delayed on coroutine update loop, elapsed time is calculated based on Time.time.
        /// </summary>
        public static IScheduler MainThread => mainThread ?? (mainThread = new MainThreadScheduler());

        private static IScheduler mainThreadIgnoreTimeScale;

        /// <summary>
        ///     Another MainThread scheduler, delay elapsed time is calculated based on Time.unscaledDeltaTime.
        /// </summary>
        public static IScheduler MainThreadIgnoreTimeScale => mainThreadIgnoreTimeScale ?? (mainThreadIgnoreTimeScale = new IgnoreTimeScaleMainThreadScheduler());

        private static IScheduler mainThreadFixedUpdate;

        /// <summary>
        ///     Run on fixed update mainthread, delay elapsed time is calculated based on Time.fixedTime.
        /// </summary>
        public static IScheduler MainThreadFixedUpdate => mainThreadFixedUpdate ?? (mainThreadFixedUpdate = new FixedUpdateMainThreadScheduler());

        private static IScheduler mainThreadEndOfFrame;

        /// <summary>
        ///     Run on end of frame mainthread, delay elapsed time is calculated based on Time.deltaTime.
        /// </summary>
        public static IScheduler MainThreadEndOfFrame => mainThreadEndOfFrame ?? (mainThreadEndOfFrame = new EndOfFrameMainThreadScheduler());

        private class MainThreadScheduler : IScheduler, ISchedulerPeriodic, ISchedulerQueueing
        {
            private readonly Action<object> scheduleAction;

            public MainThreadScheduler()
            {
                MainThreadDispatcher.Initialize();
                scheduleAction = Schedule;
            }

            // delay action is run in StartCoroutine
            // Okay to action run synchronous and guaranteed run on MainThread
            private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
            {
                // zero == every frame
                if (dueTime == TimeSpan.Zero)
                {
                    yield return null; // not immediately, run next frame
                }
                else
                {
                    yield return new WaitForSeconds((float) dueTime.TotalSeconds);
                }

                if (cancellation.IsDisposed) yield break;
                MainThreadDispatcher.UnsafeSend(action);
            }

            private IEnumerator PeriodicAction(TimeSpan period, Action action, ICancelable cancellation)
            {
                // zero == every frame
                if (period == TimeSpan.Zero)
                {
                    while (true)
                    {
                        yield return null; // not immediately, run next frame
                        if (cancellation.IsDisposed) yield break;

                        MainThreadDispatcher.UnsafeSend(action);
                    }
                }

                var seconds = (float) (period.TotalMilliseconds / 1000.0);
                var yieldInstruction = new WaitForSeconds(seconds); // cache single instruction object

                while (true)
                {
                    yield return yieldInstruction;
                    if (cancellation.IsDisposed) yield break;

                    MainThreadDispatcher.UnsafeSend(action);
                }
            }

            public DateTimeOffset Now => Scheduler.Now;

            private void Schedule(object state)
            {
                var t = (Tuple<BooleanDisposable, Action>) state;
                if (!t.Item1.IsDisposed)
                {
                    t.Item2();
                }
            }

            public IDisposable Schedule(Action action)
            {
                var d = new BooleanDisposable();
                MainThreadDispatcher.Post(scheduleAction, Tuple.Create(d, action));
                return d;
            }

            public IDisposable Schedule(DateTimeOffset dueTime, Action action)
            {
                return Schedule(dueTime - Now, action);
            }

            public IDisposable Schedule(TimeSpan dueTime, Action action)
            {
                var d = new BooleanDisposable();
                var time = Normalize(dueTime);

                MainThreadDispatcher.SendStartCoroutine(DelayAction(time, action, d));

                return d;
            }

            public IDisposable SchedulePeriodic(TimeSpan period, Action action)
            {
                var d = new BooleanDisposable();
                var time = Normalize(period);

                MainThreadDispatcher.SendStartCoroutine(PeriodicAction(time, action, d));

                return d;
            }

            private void ScheduleQueueing<T>(object state)
            {
                var t = (Tuple<ICancelable, T, Action<T>>) state;
                if (!t.Item1.IsDisposed)
                {
                    t.Item3(t.Item2);
                }
            }

            public void ScheduleQueueing<T>(ICancelable cancel, T state, Action<T> action)
            {
                MainThreadDispatcher.Post(QueuedAction<T>.Instance, Tuple.Create(cancel, state, action));
            }

            private static class QueuedAction<T>
            {
                public static readonly Action<object> Instance = Invoke;

                public static void Invoke(object state)
                {
                    var t = (Tuple<ICancelable, T, Action<T>>) state;

                    if (!t.Item1.IsDisposed)
                    {
                        t.Item3(t.Item2);
                    }
                }
            }
        }

        private class IgnoreTimeScaleMainThreadScheduler : IScheduler, ISchedulerPeriodic, ISchedulerQueueing
        {
            private readonly Action<object> scheduleAction;

            public IgnoreTimeScaleMainThreadScheduler()
            {
                MainThreadDispatcher.Initialize();
                scheduleAction = Schedule;
            }

            private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
            {
                if (dueTime == TimeSpan.Zero)
                {
                    yield return null;
                    if (cancellation.IsDisposed) yield break;

                    MainThreadDispatcher.UnsafeSend(action);
                }
                else
                {
                    var elapsed = 0f;
                    var dt = (float) dueTime.TotalSeconds;
                    while (true)
                    {
                        yield return null;
                        if (cancellation.IsDisposed) break;

                        elapsed += Time.unscaledDeltaTime;
                        if (elapsed >= dt)
                        {
                            MainThreadDispatcher.UnsafeSend(action);
                            break;
                        }
                    }
                }
            }

            private IEnumerator PeriodicAction(TimeSpan period, Action action, ICancelable cancellation)
            {
                // zero == every frame
                if (period == TimeSpan.Zero)
                {
                    while (true)
                    {
                        yield return null; // not immediately, run next frame
                        if (cancellation.IsDisposed) yield break;

                        MainThreadDispatcher.UnsafeSend(action);
                    }
                }

                var elapsed = 0f;
                var dt = (float) period.TotalSeconds;
                while (true)
                {
                    yield return null;
                    if (cancellation.IsDisposed) break;

                    elapsed += Time.unscaledDeltaTime;
                    if (elapsed >= dt)
                    {
                        MainThreadDispatcher.UnsafeSend(action);
                        elapsed = 0;
                    }
                }
            }

            public DateTimeOffset Now => Scheduler.Now;

            private void Schedule(object state)
            {
                var t = (Tuple<BooleanDisposable, Action>) state;
                if (!t.Item1.IsDisposed)
                {
                    t.Item2();
                }
            }

            public IDisposable Schedule(Action action)
            {
                var d = new BooleanDisposable();
                MainThreadDispatcher.Post(scheduleAction, Tuple.Create(d, action));
                return d;
            }

            public IDisposable Schedule(DateTimeOffset dueTime, Action action)
            {
                return Schedule(dueTime - Now, action);
            }

            public IDisposable Schedule(TimeSpan dueTime, Action action)
            {
                var d = new BooleanDisposable();
                var time = Normalize(dueTime);

                MainThreadDispatcher.SendStartCoroutine(DelayAction(time, action, d));

                return d;
            }

            public IDisposable SchedulePeriodic(TimeSpan period, Action action)
            {
                var d = new BooleanDisposable();
                var time = Normalize(period);

                MainThreadDispatcher.SendStartCoroutine(PeriodicAction(time, action, d));

                return d;
            }

            public void ScheduleQueueing<T>(ICancelable cancel, T state, Action<T> action)
            {
                MainThreadDispatcher.Post(QueuedAction<T>.Instance, Tuple.Create(cancel, state, action));
            }

            private static class QueuedAction<T>
            {
                public static readonly Action<object> Instance = Invoke;

                public static void Invoke(object state)
                {
                    var t = (Tuple<ICancelable, T, Action<T>>) state;

                    if (!t.Item1.IsDisposed)
                    {
                        t.Item3(t.Item2);
                    }
                }
            }
        }

        private class FixedUpdateMainThreadScheduler : IScheduler, ISchedulerPeriodic, ISchedulerQueueing
        {
            public FixedUpdateMainThreadScheduler()
            {
                MainThreadDispatcher.Initialize();
            }

            private IEnumerator ImmediateAction<T>(T state, Action<T> action, ICancelable cancellation)
            {
                yield return null;
                if (cancellation.IsDisposed) yield break;

                MainThreadDispatcher.UnsafeSend(action, state);
            }

            private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
            {
                if (dueTime == TimeSpan.Zero)
                {
                    yield return null;
                    if (cancellation.IsDisposed) yield break;

                    MainThreadDispatcher.UnsafeSend(action);
                }
                else
                {
                    var startTime = Time.fixedTime;
                    var dt = (float) dueTime.TotalSeconds;
                    while (true)
                    {
                        yield return null;
                        if (cancellation.IsDisposed) break;

                        var elapsed = Time.fixedTime - startTime;
                        if (elapsed >= dt)
                        {
                            MainThreadDispatcher.UnsafeSend(action);
                            break;
                        }
                    }
                }
            }

            private IEnumerator PeriodicAction(TimeSpan period, Action action, ICancelable cancellation)
            {
                // zero == every frame
                if (period == TimeSpan.Zero)
                {
                    while (true)
                    {
                        yield return null;
                        if (cancellation.IsDisposed) yield break;

                        MainThreadDispatcher.UnsafeSend(action);
                    }
                }

                var startTime = Time.fixedTime;
                var dt = (float) period.TotalSeconds;
                while (true)
                {
                    yield return null;
                    if (cancellation.IsDisposed) break;

                    var ft = Time.fixedTime;
                    var elapsed = ft - startTime;
                    if (elapsed >= dt)
                    {
                        MainThreadDispatcher.UnsafeSend(action);
                        startTime = ft;
                    }
                }
            }

            public DateTimeOffset Now => Scheduler.Now;

            public IDisposable Schedule(Action action)
            {
                return Schedule(TimeSpan.Zero, action);
            }

            public IDisposable Schedule(DateTimeOffset dueTime, Action action)
            {
                return Schedule(dueTime - Now, action);
            }

            public IDisposable Schedule(TimeSpan dueTime, Action action)
            {
                var d = new BooleanDisposable();
                var time = Normalize(dueTime);

                MainThreadDispatcher.StartFixedUpdateMicroCoroutine(DelayAction(time, action, d));

                return d;
            }

            public IDisposable SchedulePeriodic(TimeSpan period, Action action)
            {
                var d = new BooleanDisposable();
                var time = Normalize(period);

                MainThreadDispatcher.StartFixedUpdateMicroCoroutine(PeriodicAction(time, action, d));

                return d;
            }

            public void ScheduleQueueing<T>(ICancelable cancel, T state, Action<T> action)
            {
                MainThreadDispatcher.StartFixedUpdateMicroCoroutine(ImmediateAction(state, action, cancel));
            }
        }

        private class EndOfFrameMainThreadScheduler : IScheduler, ISchedulerPeriodic, ISchedulerQueueing
        {
            public EndOfFrameMainThreadScheduler()
            {
                MainThreadDispatcher.Initialize();
            }

            private IEnumerator ImmediateAction<T>(T state, Action<T> action, ICancelable cancellation)
            {
                yield return null;
                if (cancellation.IsDisposed) yield break;

                MainThreadDispatcher.UnsafeSend(action, state);
            }

            private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
            {
                if (dueTime == TimeSpan.Zero)
                {
                    yield return null;
                    if (cancellation.IsDisposed) yield break;

                    MainThreadDispatcher.UnsafeSend(action);
                }
                else
                {
                    var elapsed = 0f;
                    var dt = (float) dueTime.TotalSeconds;
                    while (true)
                    {
                        yield return null;
                        if (cancellation.IsDisposed) break;

                        elapsed += Time.deltaTime;
                        if (elapsed >= dt)
                        {
                            MainThreadDispatcher.UnsafeSend(action);
                            break;
                        }
                    }
                }
            }

            private IEnumerator PeriodicAction(TimeSpan period, Action action, ICancelable cancellation)
            {
                // zero == every frame
                if (period == TimeSpan.Zero)
                {
                    while (true)
                    {
                        yield return null;
                        if (cancellation.IsDisposed) yield break;

                        MainThreadDispatcher.UnsafeSend(action);
                    }
                }

                var elapsed = 0f;
                var dt = (float) period.TotalSeconds;
                while (true)
                {
                    yield return null;
                    if (cancellation.IsDisposed) break;

                    elapsed += Time.deltaTime;
                    if (elapsed >= dt)
                    {
                        MainThreadDispatcher.UnsafeSend(action);
                        elapsed = 0;
                    }
                }
            }

            public DateTimeOffset Now => Scheduler.Now;

            public IDisposable Schedule(Action action)
            {
                return Schedule(TimeSpan.Zero, action);
            }

            public IDisposable Schedule(DateTimeOffset dueTime, Action action)
            {
                return Schedule(dueTime - Now, action);
            }

            public IDisposable Schedule(TimeSpan dueTime, Action action)
            {
                var d = new BooleanDisposable();
                var time = Normalize(dueTime);

                MainThreadDispatcher.StartEndOfFrameMicroCoroutine(DelayAction(time, action, d));

                return d;
            }

            public IDisposable SchedulePeriodic(TimeSpan period, Action action)
            {
                var d = new BooleanDisposable();
                var time = Normalize(period);

                MainThreadDispatcher.StartEndOfFrameMicroCoroutine(PeriodicAction(time, action, d));

                return d;
            }

            public void ScheduleQueueing<T>(ICancelable cancel, T state, Action<T> action)
            {
                MainThreadDispatcher.StartEndOfFrameMicroCoroutine(ImmediateAction(state, action, cancel));
            }
        }
    }
}