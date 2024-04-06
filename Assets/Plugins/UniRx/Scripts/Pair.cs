using System;
using System.Collections.Generic;

namespace UniRx
{
    // Pair is used for Observable.Pairwise
    [Serializable]
    public struct Pair<T> : IEquatable<Pair<T>>
    {
        public T Previous { get; }

        public T Current { get; }

        public Pair(T previous, T current)
        {
            this.Previous = previous;
            this.Current = current;
        }

        public override int GetHashCode()
        {
            var comparer = EqualityComparer<T>.Default;

            int h0;
            h0 = comparer.GetHashCode(Previous);
            h0 = ((h0 << 5) + h0) ^ comparer.GetHashCode(Current);
            return h0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Pair<T>)) return false;

            return Equals((Pair<T>) obj);
        }

        public bool Equals(Pair<T> other)
        {
            var comparer = EqualityComparer<T>.Default;

            return comparer.Equals(Previous, other.Previous) &&
                   comparer.Equals(Current, other.Current);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", Previous, Current);
        }
    }
}