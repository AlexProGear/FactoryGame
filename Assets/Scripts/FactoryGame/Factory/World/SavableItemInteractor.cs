using System;
using System.Linq;
using FactoryGame.SaveSystem;
using UnityEngine;
using Utils.Extensions;
using Utils.Helpers;

namespace FactoryGame.Factory.World
{
    [RequireComponent(typeof(UniqueId))]
    public class SavableItemInteractor : ItemInteractor, ISavable
    {
        protected override void OnDestroy()
        {
            Debug.LogWarning($"Savable interactor {name} being destroyed!", this);
        }

        Action ISavable.ForceSave { get; set; }

        string ISavable.GetSaveData()
        {
            return JsonHelpers.ArrayToJson(inventory.Select(item => item.data.itemName));
        }

        void ISavable.LoadSaveData(string data)
        {
            string[] itemNames = JsonHelpers.ArrayFromJson<string>(data);
            if (itemNames.IsNullOrEmpty())
                return;

            var itemSpawner = new ItemSpawner();
            itemSpawner.Initialize();
            inventory.Clear();
            foreach (var itemName in itemNames)
            {
                ItemObject newItem = itemSpawner.SpawnItem(itemName);
                Transform newItemTransform = newItem.transform;
                newItemTransform.SetParent(transform, true);
                newItemTransform.localPosition = Vector3.up;
                newItemTransform.localScale = Vector3.zero;
                newItem.TogglePhysics(false);
                inventory.Add(newItem);
            }

            InventoryChanged?.Invoke();
        }
    }
}