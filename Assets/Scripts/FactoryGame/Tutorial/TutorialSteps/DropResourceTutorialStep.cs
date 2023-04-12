using System;
using FactoryGame.Factory.Production;
using UnityEngine;

namespace FactoryGame.Tutorial.TutorialSteps
{
    public class DropResourceTutorialStep : MonoBehaviour, IGameTutorialStep
    {
        [SerializeField] private ItemResource target;

        public event Action Finished;

        void IGameTutorialStep.Initialize(GameTutorial tutorial, bool finished)
        {
            if (!finished)
            {
                target.ItemsSpawned += OnItemsSpawned;
            }
        }

        public Transform GetTarget()
        {
            return target.transform;
        }

        private void OnItemsSpawned()
        {
            Finished?.Invoke();
            target.ItemsSpawned -= OnItemsSpawned;
        }
    }
}