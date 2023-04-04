using System.Collections;
using DG.Tweening;
using FactoryGame.Data;
using FactoryGame.Factory.World;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FactoryGame.Factory.Production
{
    public class ItemResource : MonoBehaviour
    {
        public ResourceData data;
        [SerializeField, ValidateInput("@$value != null")]
        private Transform spawnPosition;
        [SerializeField, ValidateInput("@$value != null")]
        private Transform viewModel;
        [SerializeField, ValidateInput("@$value != null")]
        private Transform spawnedObjectsParent;

        private int _health;
        private float _hitCooldown;
        private readonly IItemSpawner _itemSpawner = new ItemSpawner();
        private Vector3 _objectScale;

        private void Start()
        {
            _health = data.hitCount;
            _objectScale = viewModel.localScale;
            _itemSpawner.Initialize();
        }

        [Button]
        public bool TryHit()
        {
            if (_health <= 0 || _hitCooldown > 0)
                return false;

            SpawnItems();

            _health--;
            StartCoroutine(_health > 0 ? HitCooldown() : Regenerate());

            return true;
        }

        private void SpawnItems()
        {
            for (int i = 0; i < data.itemsPerHit; i++)
            {
                ItemObject newItem = _itemSpawner.SpawnItem(data.spawnItem);
                Transform newItemTransform = newItem.transform;
                newItemTransform.SetParent(spawnedObjectsParent);
                newItemTransform.position = spawnPosition.position;
                newItem.OnSpawn();
            }
        }

        private IEnumerator HitCooldown()
        {
            viewModel.DOComplete();
            viewModel.DOScale(_objectScale * 0.9f, 0.1f).SetLoops(2, LoopType.Yoyo);

            _hitCooldown = data.hitCooldown;
            while (_hitCooldown > 0)
            {
                _hitCooldown -= Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator Regenerate()
        {
            viewModel.DOKill();
            viewModel.DOScale(Vector3.zero, 0.5f);
            yield return new WaitForSeconds(data.regenTime);
            viewModel.DOScale(_objectScale, 0.5f).OnComplete(() => { _health = data.hitCount; });
        }

        private void OnDrawGizmos()
        {
            if (spawnPosition != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(spawnPosition.position, 0.125f);
            }
        }
    }
}