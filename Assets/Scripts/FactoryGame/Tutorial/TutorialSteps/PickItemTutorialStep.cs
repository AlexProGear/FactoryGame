using System;
using System.Linq;
using FactoryGame.Data;
using FactoryGame.Factory.World;
using UnityEngine;

namespace FactoryGame.Tutorial.TutorialSteps
{
    public class PickItemTutorialStep : MonoBehaviour, IGameTutorialStep
    {
        [SerializeField] private ItemData target;

        private ItemInteractor _playerItemInteractor;

        public event Action Finished;
        public void Initialize(GameTutorial tutorial, bool finished)
        {
            if (!finished)
            {
                _playerItemInteractor = tutorial.player.GetComponentInChildren<ItemInteractor>();
                _playerItemInteractor.ItemPicked += OnPlayerItemPicked;
            }
        }

        public Transform GetTarget()
        {
            return FindObjectsOfType<ItemObject>().First(obj => obj.data == target).transform;
        }

        private void OnPlayerItemPicked(ItemData item)
        {
            if (item == target)
            {
                Finished?.Invoke();
                _playerItemInteractor.ItemPicked -= OnPlayerItemPicked;
            }
        }
    }
}