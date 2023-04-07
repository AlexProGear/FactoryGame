using System.Linq;
using FactoryGame.Data;
using UnityEngine;
using Utils.Extensions;

namespace FactoryGame.Factory.World
{
    public interface IItemSpawner
    {
        void Initialize();
        ItemObject SpawnItem(ItemData item);
    }

    public class ItemSpawner : IItemSpawner
    {
        private ItemsListData _itemsList;

        public void Initialize()
        {
            _itemsList = Resources.Load<ItemsListData>("FactoryData/ItemList");
        }

        public ItemObject SpawnItem(string itemName)
        {
            ItemData itemData = _itemsList.items.SingleOrDefault(item => item.itemName == itemName);
            if (itemData == null)
            {
                Debug.LogError($"Item data not found for \"{itemName}\"", _itemsList);
                return null;
            }
            return SpawnItem(itemData);
        }

        public ItemObject SpawnItem(ItemData item)
        {
            // TODO pooling is a good idea
            if (_itemsList.prefabs.TryGetValue(item, out ItemObject[] prefabVariants))
            {
                ItemObject randomVariantPrefab = prefabVariants.GetRandom();
                ItemObject newItem = GameObject.Instantiate(randomVariantPrefab);
                return newItem;
            }

            Debug.LogError($"Item prefab not found for \"{item.itemName}\"", item);
            return null;
        }
    }
}