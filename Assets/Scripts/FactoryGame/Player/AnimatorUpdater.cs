using UnityEngine;

namespace FactoryGame.Player
{
    public class AnimatorUpdater : MonoBehaviour
    {
        [SerializeField] private UserInput userInput;

        private Animator _animator;
        private int _walkingLayerIndex;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        private float _walkingSpeed;
        private float _walkingSmoothVelocity;
        private const float SMOOTH_TIME = 0.1f;

        private Vector3 _directionTarget;
        private Vector3 _lookDirection;
        private Vector3 _lookDirectionVelocity;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _walkingLayerIndex = _animator.GetLayerIndex("Walking Layer");
            _walkingSpeed = 0;
            _lookDirection = _directionTarget = transform.forward;
        }

        private void Update()
        {
            Vector2 deltaInput = userInput.Joystick;
            Vector3 localDeltaPos = new Vector3(deltaInput.x, 0, deltaInput.y);
            Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

            Vector3 deltaPos = cameraRotation * localDeltaPos;
            _walkingSpeed = Mathf.SmoothDamp(_walkingSpeed, deltaPos.magnitude, ref _walkingSmoothVelocity, SMOOTH_TIME);

            UpdateAnimator();
            if (deltaInput != Vector2.zero)
            {
                _directionTarget = deltaPos;
            }
            _lookDirection = Vector3.SmoothDamp(_lookDirection, _directionTarget, ref _lookDirectionVelocity, SMOOTH_TIME);
            transform.rotation = Quaternion.LookRotation(_lookDirection);
        }

        private void UpdateAnimator()
        {
            _animator.SetLayerWeight(_walkingLayerIndex, _walkingSpeed);
            _animator.SetBool(IsWalking, _walkingSpeed > 0.01f);
        }
    }
}