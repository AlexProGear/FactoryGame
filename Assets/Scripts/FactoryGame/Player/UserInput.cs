using UnityEngine;

namespace FactoryGame.Player
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField] private Joystick joystick;

        public float Horizontal => joystick.Horizontal;
        public float Vertical => joystick.Vertical;
        public Vector2 Joystick => new Vector2(Horizontal, Vertical);
    }
}