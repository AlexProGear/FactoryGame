using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.Helpers
{
    public static class JsonHelpers
    {
        public static T[] ArrayFromJson<T>(string json)
        {
            ArrayWrapper<T> wrapper = JsonUtility.FromJson<ArrayWrapper<T>>(json);
            return wrapper?.items;
        }

        public static string ArrayToJson<T>(T[] list, bool prettyPrint = false)
        {
            ArrayWrapper<T> wrapper = new ArrayWrapper<T> { items = list };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        public static Dictionary<TKey, TValue> DictionaryFromJson<TKey, TValue>(string json)
        {
            DictionaryWrapper<TKey, TValue> wrapper = JsonUtility.FromJson<DictionaryWrapper<TKey, TValue>>(json);
            return wrapper?.keys
                .Zip(wrapper.values, (key, value) => (key, value))
                .ToDictionary(pair => pair.key, pair => pair.value);
        }

        public static string DictionaryToJson<TKey, TValue>(Dictionary<TKey, TValue> dictionary,
            bool prettyPrint = false)
        {
            DictionaryWrapper<TKey, TValue> wrapper = new DictionaryWrapper<TKey, TValue>
            {
                keys = dictionary.Keys.ToArray(),
                values = dictionary.Values.ToArray()
            };

            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class ArrayWrapper<T>
        {
            public T[] items;
        }

        [Serializable]
        private class DictionaryWrapper<TKey, TValue>
        {
            public TKey[] keys;
            public TValue[] values;
        }
    }
}