using System;
using System.Collections.Generic;
using FactoryGame.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Extensions;

namespace FactoryGame.Factory.Logic
{
    [Serializable, InlineProperty]
    public class ItemFilter
    {
        [SerializeField] private ItemData[] filterItems;

        private HashSet<ItemData> Filter => _filter ??= new HashSet<ItemData>(filterItems);
        private HashSet<ItemData> _filter;

        public bool IsItemAllowed(ItemData item)
        {
            return filterItems.IsNullOrEmpty() || Filter.Contains(item);
        }

        public void AddItem(ItemData item)
        {
            Filter.Add(item);
        }
        
        public void RemoveItem(ItemData item)
        {
            Filter.Remove(item);
        }
    }
}