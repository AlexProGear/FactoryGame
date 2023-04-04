using Sirenix.OdinInspector;
using UnityEngine;

namespace FactoryGame.Data
{
    [CreateAssetMenu(menuName = "Factory Data/" + nameof(ItemData))]
    public class ItemData : ScriptableObject
    {
        [VerticalGroup("row1/left")]
        public string itemName;
        [VerticalGroup("row1/left"), TextArea(3, 10)]
        public string description;

        [HorizontalGroup("row1", 50), VerticalGroup("row1/right")]
        [HideLabel, PreviewField(ObjectFieldAlignment.Right)]
        public Sprite icon;

        [BoxGroup("Spawn Data")]
        public float verticalImpulse;
        [BoxGroup("Spawn Data")]
        [ValidateInput("@$value >= 0")] public float horizontalImpulseMin;
        [BoxGroup("Spawn Data")]
        [ValidateInput("@$value >= 0")] public float horizontalImpulseMax;
        [BoxGroup("Spawn Data")]
        [ValidateInput("@$value >= 0")] public float pickupCooldown;
    }
}