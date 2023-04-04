using Sirenix.OdinInspector;
using UnityEngine;

namespace FactoryGame.Data
{
    [CreateAssetMenu(menuName = "Factory Data/" + nameof(ResourceData))]
    public class ResourceData : ScriptableObject
    {
        [ValidateInput("@$value >= 0")] public float regenTime;
        [ValidateInput("@$value >= 0")] public float hitCooldown;
        [ValidateInput("@$value >= 0")] public int hitCount;
        [ValidateInput("@$value >= 0")] public int itemsPerHit;
        [ValidateInput("@$value != null")] public ItemData spawnItem;
    }
}