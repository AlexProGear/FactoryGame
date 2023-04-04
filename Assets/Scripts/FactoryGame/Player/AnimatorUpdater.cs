using UnityEngine;

namespace FactoryGame.Player
{
    public class AnimatorUpdater : MonoBehaviour
    {
        [SerializeField] private UserInput userInput;

        private Animator _animator;
        private int _walkingLayerIndex;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _walkingLayerIndex = _animator.GetLayerIndex("Walking Layer");
        }

        private void Update()
        {
            _animator.SetLayerWeight(_walkingLayerIndex, userInput.Joystick.magnitude);
            if (userInput.Joystick != Vector2.zero)
            {
                Vector2 inputDelta = userInput.Joystick;
                Vector3 relativeDeltaPos = new Vector3(inputDelta.x, 0, inputDelta.y);
                Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
                transform.rotation = Quaternion.LookRotation(cameraRotation * relativeDeltaPos);
            }
        }
    }
}