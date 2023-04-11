using System.Collections.Generic;
using System.Linq;
using FactoryGame.Data;
using FactoryGame.Factory.World;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Utils.Extensions;

namespace FactoryGame.UI
{
    public class InventoryDisplayPanel : MonoBehaviour
    {
        [SerializeField] private ItemInteractor target;
        [SerializeField] private InventoryDisplayItem itemDisplayPrefab;

        // TODO preferably replace it with Dictionary<ItemData, InventoryDisplayItem>
        private List<InventoryDisplayItem> _inventoryDisplays;

        private void Start()
        {
            Assert.IsNotNull(target);
            Assert.IsNotNull(itemDisplayPrefab);

            _inventoryDisplays = new List<InventoryDisplayItem>();
            GetComponentsInChildren<InventoryDisplayItem>().ForEach(item => Destroy(item.gameObject));

            gameObject.SetActive(false);
            target.InventoryChanged.AddListener(UpdateVisuals);
        }

        private void OnDestroy()
        {
            target.InventoryChanged.RemoveListener(UpdateVisuals);
        }

        private void UpdateVisuals()
        {
            var itemsData = target.Inventory
                .GroupBy(itemObj => itemObj.data)
                .Select(group => (itemData: group.Key, count: group.Count()))
                .OrderBy(data => data.itemData.name)
                .ToArray();

            gameObject.SetActive(itemsData.Length != 0);

            SetDisplaysCount(itemsData.Length);

            for (int i = 0; i < _inventoryDisplays.Count; i++)
            {
                InventoryDisplayItem currentDisplay = _inventoryDisplays[i];
                (ItemData itemData, int count) = itemsData[i];

                currentDisplay.SetDisplayData(itemData, count);
            }
        }

        private void SetDisplaysCount(int targetCount)
        {
            int currentCount = _inventoryDisplays.Count;
            if (currentCount == targetCount)
                return;

            if (currentCount < targetCount)
            {
                for (int i = 0; i < targetCount - currentCount; i++)
                {
                    _inventoryDisplays.Add(Instantiate(itemDisplayPrefab, transform));
                }
            }
            else
            {
                for (int i = currentCount - 1; i >= targetCount; i--)
                {
                    Destroy(_inventoryDisplays[i].gameObject);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) transform);
        }
    }
}