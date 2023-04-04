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
        private ItemPrefabsData _itemPrefabsData;

        public void Initialize()
        {
            _itemPrefabsData = Resources.Load<ItemPrefabsData>("FactoryData/ItemPrefabs");
        }

        public ItemObject SpawnItem(ItemData item)
        {
            // TODO pooling is a good idea
            if (_itemPrefabsData.Prefabs.TryGetValue(item, out ItemObject[] prefabVariants))
            {
                ItemObject randomVariantPrefab = prefabVariants.GetRandom();
                ItemObject newItem = GameObject.Instantiate(randomVariantPrefab);
                return newItem;
            }

            Debug.LogError($"Item prefab not found for {item.itemName}", item);
            return null;
        }
    }
}