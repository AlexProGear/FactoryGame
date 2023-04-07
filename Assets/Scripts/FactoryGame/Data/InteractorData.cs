using UnityEngine;

namespace FactoryGame.Data
{
    [CreateAssetMenu(menuName = "Factory Data/" + nameof(InteractorData))]
    public class InteractorData : ScriptableObject
    {
        public float pickupRadius;
        public float itemTransferTime = 0.5f;
        public float itemTransferCooldown = 0.1f;
    }
}