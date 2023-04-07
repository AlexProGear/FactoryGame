using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Utils.Helpers;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FactoryGame.SaveSystem
{
    public class SceneSaver : SerializedMonoBehaviour
    {
        [OdinSerialize] private ISavable[] savableObjects;
        private JsonSaveFile _jsonSaveFile;

        private void Start()
        {
            _jsonSaveFile = new JsonSaveFile(gameObject.scene.name);
            if (_jsonSaveFile.HasData)
            {
                LoadData();
            }

            foreach (ISavable savableObject in savableObjects)
            {
                savableObject.ForceSave = SaveData;
            }
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            SaveData();
        }

#if UNITY_EDITOR
        [Button, InitializeOnEnterPlayMode]
        public void CollectSceneSavableObjects()
        {
            savableObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ISavable>().ToArray();
        }
#endif

        private void LoadData()
        {
            Dictionary<string, string> saveData = _jsonSaveFile.DeserializeDictionary<string, string>();
            foreach (ISavable savable in savableObjects)
            {
                string id = GetUniqueId(savable);
                if (id != null && saveData.TryGetValue(id, out string loadedData))
                {
                    savable.LoadSaveData(loadedData);
                }
            }
        }

        private void SaveData()
        {
            Dictionary<string, string> saveData = new Dictionary<string, string>();
            foreach (ISavable savable in savableObjects)
            {
                var savableObject = (MonoBehaviour) savable;
                if (savableObject == null)
                {
                    Debug.LogError($"[SceneSaver] Abandoning save: savable object is null. Was it destroyed?");
                    return;
                }
                string id = GetUniqueId(savable);
                if (id != null)
                {
                    saveData.Add(id, savable.GetSaveData());
                }
                else
                {
                    Debug.LogWarning("[SceneSaver] Possible data corruption, abandoning save");
                    return;
                }
            }

            _jsonSaveFile.SerializeDictionary(saveData);
        }

        private static string GetUniqueId(ISavable savable)
        {
            var savableObject = (MonoBehaviour) savable;
            var componentId = savableObject.GetComponent<UniqueId>();
            if (componentId == null)
            {
                Debug.LogError($"[SceneSaver] Missing UniqueId class on {savableObject.name}", savableObject);
                return null;
            }

            return componentId.uniqueId;
        }
    }
}