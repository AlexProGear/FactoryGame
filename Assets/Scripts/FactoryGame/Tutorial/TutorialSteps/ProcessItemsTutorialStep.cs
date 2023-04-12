using System;
using FactoryGame.Factory.Production;
using UnityEngine;

namespace FactoryGame.Tutorial.TutorialSteps
{
    public class ProcessItemsTutorialStep : MonoBehaviour, IGameTutorialStep
    {
        [SerializeField] private ItemProcessor target;

        public event Action Finished;

        public void Initialize(GameTutorial tutorial, bool finished)
        {
            if (!finished)
            {
                target.ProcessingFinished += OnProcessingFinished;
            }
        }

        public Transform GetTarget()
        {
            return target.transform;
        }

        private void OnProcessingFinished()
        {
            Finished?.Invoke();
            target.ProcessingFinished -= OnProcessingFinished;
        }
    }
}