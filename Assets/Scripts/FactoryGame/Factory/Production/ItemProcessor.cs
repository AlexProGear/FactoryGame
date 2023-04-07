using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FactoryGame.Data;
using FactoryGame.Factory.Logic;
using FactoryGame.Factory.World;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Extensions;

namespace FactoryGame.Factory.Production
{
    public class ItemProcessor : MonoBehaviour
    {
        [SerializeField] private RecipesListData recipes;
        [SerializeField] private ItemSlot[] inputSlots;
        [SerializeField] private ItemSlot[] outputSlots;
        [SerializeField] private float itemTransferTime = 0.5f;
        [SerializeField] private float craftingTime = 3f;

        [SerializeField] private Transform shakingBody;
        [SerializeField] private Transform inputInto;
        [SerializeField] private Transform outputFrom;

        private readonly ItemSpawner _itemSpawner = new ItemSpawner();
        private bool _isCrafting;

        private void Start()
        {
            _itemSpawner.Initialize();

            foreach (var slot in inputSlots)
            {
                slot.Mode = SlotMode.Input;
            }

            foreach (var slot in outputSlots)
            {
                slot.Mode = SlotMode.Output;
            }

            StartCoroutine(CraftPeriodically());
        }

        private IEnumerator CraftPeriodically()
        {
            _isCrafting = false;
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                if (!_isCrafting)
                {
                    _isCrafting = TryCraftAny();
                }
            }
        }

        [Button]
        public bool TryCraftAny()
        {
            (Recipe recipe, List<ItemSlot> input, List<ItemSlot> output) = FindFirstValidRecipe();
            if (recipe == null)
                return false;
            BeginCrafting(recipe, input, output);
            return true;
        }

        private void BeginCrafting(Recipe recipe, List<ItemSlot> input, List<ItemSlot> output)
        {
            var sequence = DOTween.Sequence();
            foreach (ItemSlot slot in input.GetScrambled())
            {
                ItemObject item = slot.HeldItem;
                Transform itemTransform = item.transform;
                var itemSequence = DOTween.Sequence()
                    .Join(itemTransform.DOScale(Vector3.zero, itemTransferTime))
                    .Join(itemTransform.DOJump(inputInto.position, 1f, 1, itemTransferTime))
                    .OnComplete(() => { Destroy(slot.ExtractItem().gameObject); });
                sequence.Append(itemSequence);
            }

            sequence.Append(shakingBody.DOShakePosition(craftingTime, 0.1f));

            foreach (var pair in output.Zip(recipe.outputs, (slot, item) => (slot, item)).GetScrambled())
            {
                ItemObject newItem = _itemSpawner.SpawnItem(pair.item);
                Transform itemTransform = newItem.transform;
                itemTransform.localScale = Vector3.zero;
                itemTransform.position = outputFrom.position;
                pair.slot.PreInsertItem(newItem);
                var itemSequence = DOTween.Sequence()
                    .Join(itemTransform.DOScale(Vector3.one, itemTransferTime))
                    .Join(itemTransform.DOJump(pair.slot.transform.position, 1f, 1, itemTransferTime))
                    .OnComplete(() => pair.slot.InsertItem(newItem, true));
                sequence.Append(itemSequence);
            }

            sequence.OnComplete(() => _isCrafting = false);
        }

        private (Recipe recipe, List<ItemSlot> input, List<ItemSlot> output) FindFirstValidRecipe()
        {
            foreach (Recipe recipe in recipes.recipes)
            {
                (List<ItemSlot> input, List<ItemSlot> output) = GetSlotsFor(recipe);
                if (input == null || output == null)
                    continue;
                return (recipe, input, output);
            }

            return default;
        }

        private (List<ItemSlot> input, List<ItemSlot> output) GetSlotsFor(Recipe recipe)
        {
            return (GetInputSlotsFor(recipe), GetOutputSlotsFor(recipe));
        }

        private List<ItemSlot> GetInputSlotsFor(Recipe recipe)
        {
            var remainingSlots = inputSlots.ToList();
            var result = new List<ItemSlot>();

            foreach (ItemData itemData in recipe.inputs)
            {
                var itemSlot = remainingSlots.FirstOrDefault(slot => slot.IsHoldingItem(itemData));
                if (itemSlot == null)
                    return null;
                remainingSlots.Remove(itemSlot);
                result.Add(itemSlot);
            }

            return result;
        }

        private List<ItemSlot> GetOutputSlotsFor(Recipe recipe)
        {
            // TODO rework output slots to support filters
            int requiredSlotsCount = recipe.outputs.Length;
            var emptySlots = outputSlots.Where(slot => slot.IsEmpty).ToList();
            return emptySlots.Count < requiredSlotsCount ? null : emptySlots.Take(requiredSlotsCount).ToList();
        }
    }
}