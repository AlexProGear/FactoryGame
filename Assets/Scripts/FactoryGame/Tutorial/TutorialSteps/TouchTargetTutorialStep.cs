using System;
using UnityEngine;

namespace FactoryGame.Tutorial.TutorialSteps
{
    public class TouchTargetTutorialStep : MonoBehaviour, IGameTutorialStep
    {
        private GameTutorial _gameTutorial;
        private bool _finished;

        public event Action Finished;

        void IGameTutorialStep.Initialize(GameTutorial tutorial, bool finished)
        {
            _gameTutorial = tutorial;
            _finished = finished;
        }

        public Transform GetTarget()
        {
            return transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_finished && other.transform.root.CompareTag("Player"))
            {
                _finished = true;
                Finished?.Invoke();
            }
        }
    }
}