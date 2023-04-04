using System.Collections;
using System.Linq;
using FactoryGame.Data;
using FactoryGame.Factory.Logic;
using FactoryGame.Factory.World;
using UnityEngine;

namespace FactoryGame.Factory.Production
{
    public class ItemGenerator : MonoBehaviour
    {
        [SerializeField] private ItemData spawnItem;
        [SerializeField] private ItemSlot[] outputSlots;
        [SerializeField] private float spawnCooldown;

        private readonly IItemSpawner _itemSpawner = new ItemSpawner();

        private void Start()
        {
            _itemSpawner.Initialize();
            StartCoroutine(SpawnItemsCoroutine());
        }

        private IEnumerator SpawnItemsCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnCooldown);
                yield return new WaitUntil(() => outputSlots.Any(slot => slot.IsEmpty));

                ItemSlot emptySlot = outputSlots.First(slot => slot.IsEmpty);
                ItemObject newItem = _itemSpawner.SpawnItem(spawnItem);
                bool success = emptySlot.InsertItem(newItem);
                if (!success)
                {
                    Debug.LogError("[ItemGenerator] Can't insert item in slot", this);
                }
            }
        }
    }
}