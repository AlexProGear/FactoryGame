using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Utils.Helpers
{
    public class JsonSaveFile
    {
        public bool HasData => File.Exists(_path);

        private readonly string _path;

        public JsonSaveFile(string fileName)
        {
            _path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        }

        public void Serialize<T>(T data)
        {
            string jsonDataString = JsonUtility.ToJson(data, true);

            SaveString(jsonDataString);
        }

        public T Deserialize<T>()
        {
            string loadedJsonDataString = LoadString();

            return JsonUtility.FromJson<T>(loadedJsonDataString);
        }
        
        public void SerializeArray<T>(T[] data)
        {
            string jsonDataString = JsonHelpers.ArrayToJson(data, true);

            SaveString(jsonDataString);
        }

        public T[] DeserializeArray<T>()
        {
            string loadedJsonDataString = LoadString();

            return JsonHelpers.ArrayFromJson<T>(loadedJsonDataString);
        }
        
        public void SerializeDictionary<TKey, TValue>(Dictionary<TKey, TValue> data)
        {
            string jsonDataString = JsonHelpers.DictionaryToJson(data, true);

            SaveString(jsonDataString);
        }

        public Dictionary<TKey, TValue> DeserializeDictionary<TKey, TValue>()
        {
            string loadedJsonDataString = LoadString();

            return JsonHelpers.DictionaryFromJson<TKey, TValue>(loadedJsonDataString);
        }

        private string LoadString()
        {
            return File.ReadAllText(_path);
        }

        private void SaveString(string jsonDataString)
        {
            File.WriteAllText(_path, jsonDataString);
        }

        public void Delete()
        {
            File.Delete(_path);
        }
    }
}