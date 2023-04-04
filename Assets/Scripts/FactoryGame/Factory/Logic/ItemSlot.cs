using FactoryGame.Data;
using FactoryGame.Factory.World;
using UnityEngine;

namespace FactoryGame.Factory.Logic
{
    public enum SlotMode
    {
        None,
        Input,
        Output
    }

    public class ItemSlot : MonoBehaviour
    {
        public ItemFilter filter;

        public SlotMode Mode { get; set; }
        public bool IsEmpty => _heldItem == null;
        public bool HasItem => !IsEmpty;
        public ItemObject HeldItem => _heldItem;

        private ItemObject _heldItem;
        private ItemObject _preInsertedItem;

        public bool InsertItem(ItemObject item, bool force = false)
        {
            if (_preInsertedItem != null && _preInsertedItem != item)
            {
                return false;
            }

            if (HasItem || !filter.IsItemAllowed(item.data))
            {
                if (!force)
                    return false;
                Debug.LogWarning($"[ItemSlot] Item {item.data.itemName} was forced into slot", this);
                if (_heldItem != null)
                {
                    Destroy(_heldItem);
                    Debug.LogError("[ItemSlot] Previous item destroyed", this);
                }
            }

            _preInsertedItem = null;
            item.transform.SetParent(transform);
            _heldItem = item;
            return true;
        }

        public ItemObject ExtractItem()
        {
            ItemObject item = _heldItem;
            _heldItem = null;
            item.transform.SetParent(null);
            return item;
        }

        public bool IsHoldingItem(ItemData itemData)
        {
            return HasItem && _heldItem.data == itemData;
        }

        public bool CanHoldItem(ItemData itemData)
        {
            return IsEmpty && _preInsertedItem == null && filter.IsItemAllowed(itemData);
        }

        public void PreInsertItem(ItemObject item)
        {
            _preInsertedItem = item;
        }
    }
}