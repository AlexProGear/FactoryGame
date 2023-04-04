using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FactoryGame.Data;
using FactoryGame.Factory.Logic;
using FactoryGame.Factory.Production;
using UnityEngine;

namespace FactoryGame.Factory.World
{
    public class ItemInteractor : MonoBehaviour
    {
        [SerializeField] private InteractorData interactorData;
        [SerializeField] private float interactionDelay;
        [SerializeField] ItemFilter filter;

        public IReadOnlyList<ItemObject> Inventory => _inventory;

        private readonly List<ItemObject> _inventory = new List<ItemObject>();
        private Vector3 _lastPos;
        private Vector3 _deltaPos;
        private float _standingTime;

        private bool CanInteract => _standingTime > interactionDelay;

        private void Start()
        {
            GetComponent<SphereCollider>().radius = interactorData.pickupRadius;
            _lastPos = transform.position;
        }

        private void Update()
        {
            UpdateStandingTime();
        }

        private void UpdateStandingTime()
        {
            Vector3 currentPos = transform.position;
            _deltaPos = currentPos - _lastPos;
            _lastPos = currentPos;
            if (_deltaPos == Vector3.zero)
            {
                _standingTime += Time.deltaTime;
            }
            else
            {
                _standingTime = 0;
            }
        }

        private bool TryPickItem(ItemObject item)
        {
            if (!CanPickItem(item))
                return false;

            item.transform.SetParent(transform, true);
            DOTween.Sequence()
                .Join(item.transform.DOLocalJump(Vector3.up, 0, 1, interactorData.itemTransferTime))
                .Join(item.transform.DOScale(Vector3.zero, interactorData.itemTransferTime))
                .OnComplete(() => _inventory.Add(item));

            item.TogglePhysics(false);
            return true;
        }

        private bool CanPickItem(ItemObject item)
        {
            return filter.IsItemAllowed(item.data);
        }

        private ItemObject ExtractItem(ItemData itemType)
        {
            ItemObject result = _inventory.FirstOrDefault(item => item.data == itemType);
            if (result == null)
                return null;

            return ExtractItem(result);
        }

        private ItemObject ExtractItem(ItemObject item)
        {
            _inventory.Remove(item);
            item.transform.SetParent(null, true);
            item.TogglePhysics(true);
            return item;
        }

        private void OnTriggerStay(Collider other)
        {
            if (TryItemInteraction(other))
                return;
            if (TryResourceInteraction(other))
                return;
            if (TrySlotInteraction(other))
                return;
        }

        private bool TryItemInteraction(Collider other)
        {
            var item = other.GetComponentInParent<ItemObject>();
            if (item != null)
            {
                if (item.CanPickup)
                    item.CanPickup = !TryPickItem(item);
                return true;
            }

            return false;
        }

        private bool TryResourceInteraction(Collider other)
        {
            var resource = other.GetComponent<ItemResource>();
            if (resource != null)
            {
                if (CanInteract)
                    resource.TryHit();

                return true;
            }

            return false;
        }

        private bool TrySlotInteraction(Collider other)
        {
            var slot = other.GetComponent<ItemSlot>();
            if (slot != null)
            {
                switch (slot.Mode)
                {
                    case SlotMode.Input:
                        TryInsertItem(slot);
                        break;
                    case SlotMode.Output:
                        if (slot.HasItem && CanPickItem(slot.HeldItem))
                        {
                            ItemObject item = slot.ExtractItem();
                            TryPickItem(item);
                        }

                        break;
                }

                return true;
            }

            return false;
        }

        private void TryInsertItem(ItemSlot slot)
        {
            foreach (var itemObject in _inventory)
            {
                if (!slot.CanHoldItem(itemObject.data))
                    continue;
                ExtractItem(itemObject);
                var itemTransform = itemObject.transform;
                slot.PreInsertItem(itemObject);
                DOTween.Sequence()
                    .Join(itemTransform.DOScale(Vector3.one, interactorData.itemTransferTime))
                    .Join(itemTransform.DOJump(slot.transform.position, 1, 1, interactorData.itemTransferTime))
                    .OnComplete(() =>
                    {
                        if (!slot.InsertItem(itemObject))
                        {
                            Debug.LogError("[ItemInteractor] Cannot insert item into slot. What? How?", this);
                        }
                    });
                break;
            }
        }
    }
}