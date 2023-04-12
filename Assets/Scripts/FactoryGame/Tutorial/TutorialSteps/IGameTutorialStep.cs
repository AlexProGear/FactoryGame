using System;
using UnityEngine;

namespace FactoryGame.Tutorial.TutorialSteps
{
    public interface IGameTutorialStep
    {
        public event Action Finished;

        public void Initialize(GameTutorial tutorial, bool finished);

        public Transform GetTarget();
    }
}