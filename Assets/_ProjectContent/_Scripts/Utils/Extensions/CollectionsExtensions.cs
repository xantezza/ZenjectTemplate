using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils.Extensions
{
    public static class CollectionsExtensions
    {
        #region Arrays

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var obj in source)
                action(obj);
            return source;
        }

        public static T GetIndexClamped<T>(this T[] items, int index, bool logErrorIfOutOfRange = false)
        {
            if (items.Length == 0) return default;

            if (logErrorIfOutOfRange && (index < 0 || index >= items.Length)) Debug.LogError("Catched out of range error.");

            return items[Mathf.Clamp(index, 0, items.Length - 1)];
        }

        public static T ElementAtOrDefault<T>(this T[] array, int index, T defaultValue)
        {
            return array.TryGetValue(index, out var outValue) ? outValue : defaultValue;
        }

        public static bool TryGetValue<T>(this T[] array, int index, out T value)
        {
            if (array == null)
            {
                value = default;
                return false;
            }

            if (array.Length == 0
                || index < 0
                || index >= array.Length)
            {
                value = default;
                return false;
            }

            value = array[index];
            return true;
        }

        public static bool TryGetValue<T>(this T[] array, Func<T, bool> predicate, out T value)
        {
            value = default;

            if (array == null ||
                predicate == null)
            {
                Debug.LogError("Failed getting value from array!");
                return false;
            }

            if (array.Length == 0) return false;

            foreach (var element in array)
            {
                if (!predicate.Invoke(element)) continue;
                value = element;
                return true;
            }

            return false;
        }


        public static int IndexOf<T>(this T[] array, T element)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var arrayElement = array[i];
                if (arrayElement.Equals(element)) return i;
            }

            return -1;
        }

        public static Dictionary<TKey, TElement> ToDictionarySafe<TKey, TElement>(this TElement[] array, Func<TElement, TKey> keySelector)
        {
            if (array == null) return null;
            if (keySelector == null) return null;
            var dictionary = new Dictionary<TKey, TElement>();
            foreach (var element in array)
            {
                var key = keySelector(element);
                if (dictionary.ContainsKey(key))
                {
                    Debug.LogError($"Dictionary already contains key: {key}!");
                    continue;
                }

                dictionary.Add(keySelector(element), element);
            }

            return dictionary;
        }

        public static Dictionary<TKey, TValue> ToDictionarySafe<TKey, TElement, TValue>(this TElement[] array,
            Func<TElement, TKey> keySelector, Func<TElement, TValue> elementSelector)
        {
            if (array == null) return null;
            if (keySelector == null) return null;
            if (elementSelector == null) return null;
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var element in array)
            {
                var key = keySelector(element);
                if (dictionary.ContainsKey(key))
                {
                    Debug.LogError($"Dictionary already contains key: {key}!");
                    continue;
                }

                dictionary.Add(keySelector(element), elementSelector(element));
            }

            return dictionary;
        }

        #endregion

        #region Lists

        public static void Shuffle<T>(this List<T> list)
        {
            if (list.Count == 0) return;

            for (var i = 0; i < list.Count; i++)
            {
                var randomIndex = Random.Range(0, list.Count);
                var tmp = list[randomIndex];
                list[randomIndex] = list[i];
                list[i] = tmp;
            }
        }

        public static T GetIndexClamped<T>(this List<T> items, int index, bool logErrorIfOutOfRange = false)
        {
            if (items.Count == 0) return default;

            if (logErrorIfOutOfRange && (index < 0 || index >= items.Count)) Debug.LogError("Catched out of range error.");

            return items[Mathf.Clamp(index, 0, items.Count - 1)];
        }

        public static T GetRandom<T>(this List<T> items)
        {
            if (items.Count == 0) return default;

            var index = Random.Range(0, items.Count);

            return items[index];
        }

        public static bool TryGetValue<T>(this List<T> list, int index, out T value)
        {
            if (list.Count == 0
                || index < 0
                || index >= list.Count)
            {
                value = default;
                return false;
            }

            value = list[index];
            return true;
        }

        public static int RandomIndexWithProbabilities(this List<float> probabilities, Func<int, bool> skipPredicate)
        {
            float total = 0;

            for (var i = 0; i < probabilities.Count; i++)
            {
                var elem = probabilities[i];
                if (skipPredicate.Invoke(i)) continue;
                total += elem;
            }

            var randomPoint = Random.value * total;

            for (var i = 0; i < probabilities.Count; i++)
            {
                if (skipPredicate.Invoke(i)) continue;

                var probability = probabilities[i];
                if (probability == 0) continue;

                if (randomPoint < probability) return i;

                randomPoint -= probability;
            }

            return probabilities.Count - 1;
        }

        public static bool TryGetValue<T>(this List<T> list, Func<T, bool> predicate, out T value)
        {
            value = default;

            if (list == null ||
                predicate == null)
            {
                Debug.LogError("Failed getting value from array!");
                return false;
            }

            if (list.Count == 0) return false;

            for (var i = 0; i < list.Count; i++)
            {
                var element = list[i];
                if (!predicate.Invoke(element)) continue;
                value = element;
                return true;
            }

            return false;
        }

        #endregion

        #region IEnumerable

        public static T GetValueCircular<T>(this IEnumerable<T> array, int index)
        {
            if (array == null) return default;

            var enumerable = array as T[] ?? array.ToArray();
            if (!enumerable.Any()) return default;

            return enumerable.ElementAt(enumerable.GetIndexCircular(index));
        }

        public static int GetIndexCircular<T>(this IEnumerable<T> array, int index)
        {
            var length = array.Count();

            return (index % length + length) % length;
        }

        public static TSource MiddleOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            var elementsCount = source.Count();

            switch (elementsCount)
            {
                case 0:
                    return default;
                case 1:
                    return source.ElementAt(1);
                default:
                {
                    var midleIndex = Mathf.RoundToInt((elementsCount - 1) / 2f);

                    return source.ElementAt(midleIndex);
                }
            }
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            var hashSet = new HashSet<T>();

            foreach (var element in source) hashSet.Add(element);

            return hashSet;
        }

        #endregion

        #region Dictionary

        public static void AddOverride<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue data)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = data;
                return;
            }

            dictionary.Add(key, data);
        }

        /// <summary>
        ///     Returns default value if failed to find in dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue TryGetDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }

        #endregion
    }
}