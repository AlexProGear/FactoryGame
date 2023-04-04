using UnityEngine;
using UnityEngine.AI;

namespace FactoryGame.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private UserInput userInput;
        [SerializeField] private float speed;
        private NavMeshAgent _navMeshAgent;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            Vector2 inputDelta = userInput.Joystick;
            float deltaPos = speed * Time.deltaTime;
            Vector3 relativeDeltaPos = new Vector3(inputDelta.x * deltaPos, 0, inputDelta.y * deltaPos);
            Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            _navMeshAgent.Move(cameraRotation * relativeDeltaPos);
        }
    }
}
