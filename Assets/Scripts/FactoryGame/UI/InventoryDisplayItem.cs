using DG.Tweening;
using FactoryGame.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FactoryGame.UI
{
    public class InventoryDisplayItem : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text countText;

        private ItemData _displayItem;
        private int _displayCount;

        public void SetDisplayData(ItemData item, int count)
        {
            bool isNewItem = _displayItem != item;
            bool isCountChanged = _displayCount != count;
            if (isNewItem || isCountChanged)
            {
                DoShake();
            }

            _displayItem = item;
            _displayCount = count;
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            itemIcon.sprite = _displayItem.icon;
            countText.text = _displayCount.ToString();
        }

        private void DoShake()
        {
            transform.DOKill(true);
            transform.DOShakePosition(0.5f, 10);
        }
    }
}