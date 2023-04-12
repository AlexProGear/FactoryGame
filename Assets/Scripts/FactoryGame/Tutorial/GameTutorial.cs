using System;
using System.Collections;
using System.Collections.Generic;
using FactoryGame.SaveSystem;
using FactoryGame.Tutorial.TutorialSteps;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace FactoryGame.Tutorial
{
    public class GameTutorial : SerializedMonoBehaviour, ISavable
    {
        [OdinSerialize] private List<IGameTutorialStep> tutorialSteps;
        [SerializeField] private Transform tutorialArrow;

        public Transform player;

        private bool _initialized;
        private int _lastFinishedStep = -1;

        private Coroutine _arrowCoroutine;

        private void Initialize()
        {
            if (_initialized)
                return;
            _initialized = true;

            for (int i = 0; i < tutorialSteps.Count; i++)
            {
                int currentIndex = i;
                var currentStep = tutorialSteps[currentIndex];

                currentStep.Initialize(this, currentIndex <= _lastFinishedStep);
                if (currentIndex > _lastFinishedStep)
                {
                    currentStep.Finished += () => UpdateCurrentStep(currentIndex);
                }
            }
        }

        private void UpdateCurrentStep(int current)
        {
            _lastFinishedStep = current;
            UpdateArrowTarget();
            Debug.Log($"[GameTutorial] Step {current} finished");
        }

        private void UpdateArrowTarget()
        {
            Transform arrowTarget = null;
            if (_lastFinishedStep + 1 < tutorialSteps.Count)
            {
                arrowTarget = tutorialSteps[_lastFinishedStep + 1].GetTarget();
            }

            if (_arrowCoroutine != null)
                StopCoroutine(_arrowCoroutine);
            _arrowCoroutine = StartCoroutine(ArrowLookAt(arrowTarget));
        }

        private IEnumerator ArrowLookAt(Transform target)
        {
            bool hasTarget = target != null;
            tutorialArrow.gameObject.SetActive(hasTarget);

            if (!hasTarget)
                yield break;

            bool visible = true;
            while (true)
            {
                yield return null;

                bool tooClose = Vector3.Distance(target.position, tutorialArrow.position) < 1f;
                if (visible && tooClose)
                {
                    visible = false;
                    tutorialArrow.gameObject.SetActive(visible);
                }
                else if (!visible && !tooClose)
                {
                    visible = true;
                    tutorialArrow.gameObject.SetActive(visible);
                }

                tutorialArrow.LookAt(target);
            }
        }

        Action ISavable.ForceSave { get; set; }

        string ISavable.GetSaveData()
        {
            return _lastFinishedStep.ToString();
        }

        void ISavable.LoadSaveData(string data)
        {
            if (data != null)
                _lastFinishedStep = int.Parse(data);
            Initialize();
            UpdateCurrentStep(_lastFinishedStep);
        }
    }
}