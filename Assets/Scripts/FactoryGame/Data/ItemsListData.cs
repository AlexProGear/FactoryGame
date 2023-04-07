using System.Collections.Generic;
using FactoryGame.Factory.World;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FactoryGame.Data
{
    [CreateAssetMenu(menuName = "Factory Data/" + nameof(ItemsListData))]
    public class ItemsListData : SerializedScriptableObject
    {
        [InfoBox("\nItems and prefabs count not same!\n", InfoMessageType.Error, "@items.Count != prefabs.Count")]
        [LabelText("Items list")]
        public HashSet<ItemData> items = new HashSet<ItemData>();
        [Space(30.0f), LabelText("Items prefabs")]
        public Dictionary<ItemData, ItemObject[]> prefabs = new Dictionary<ItemData, ItemObject[]>();
    }
}