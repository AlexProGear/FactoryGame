using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Utils.Helpers;

namespace FactoryGame.SaveSystem
{
    public class SceneSaver : SerializedMonoBehaviour
    {
        [OdinSerialize] private ISavable[] savableObjects;

        private JsonSaveFile _jsonSaveFile;

        private void Start()
        {
            InitSaveFile();
            if (_jsonSaveFile.HasData)
            {
                LoadData();
            }
            else
            {
                foreach (var savable in savableObjects)
                {
                    savable.LoadSaveData(null);
                }
            }

            foreach (ISavable savableObject in savableObjects)
            {
                savableObject.ForceSave = SaveData;
            }
        }

        private void InitSaveFile()
        {
            _jsonSaveFile = new JsonSaveFile(gameObject.scene.name);
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            SaveData();
        }

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
            if (_jsonSaveFile == null)
            {
                Debug.LogWarning("[SceneSaver] SaveData called before initialization");
                return;
            }

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

        [Button]
        public void CollectSceneSavableObjects()
        {
            savableObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ISavable>().ToArray();
        }

        [Button]
        public void DeleteSaveFile()
        {
            if (_jsonSaveFile == null)
                InitSaveFile();
            _jsonSaveFile.Delete();
            Debug.Log("[SceneSaver] Save file deleted");
        }
    }
}