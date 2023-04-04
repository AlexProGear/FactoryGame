using System.Collections.Generic;
using FactoryGame.Factory.World;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FactoryGame.Data
{
    [CreateAssetMenu(menuName = "Factory Data/" + nameof(ItemPrefabsData))]
    public class ItemPrefabsData : SerializedScriptableObject
    {
        public Dictionary<ItemData, ItemObject[]> Prefabs;
    }
}