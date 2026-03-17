using UnityEngine;
using UnityEngine.InputSystem;

namespace AoV.Player
{
    /// <summary>
    /// Player control script. Required for PlayerController.
    /// </summary>
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInputs;

        public float HorizontalAxis { get; private set; }
        public float VerticalAxis { get; private set; }
        public bool Jump { get; private set; }
        public bool Fire { get; private set; }
        public bool Melee { get; private set; }
        public bool Confirm { get; private set; }
        public bool Cancel { get; private set; }

        private void Awake()
        {

        }

        private void Update()
        {
            HorizontalAxis = _playerInputs.actions["Move"].ReadValue<Vector2>().x;
            VerticalAxis = _playerInputs.actions["Move"].ReadValue<Vector2>().y;
            Jump = _playerInputs.actions["Jump"].IsPressed();
            Fire = _playerInputs.actions["Fire"].IsPressed();
            Melee = _playerInputs.actions["Melee"].IsPressed();
            Confirm = _playerInputs.actions["Confirm"].WasPressedThisFrame();
            Cancel = _playerInputs.actions["Cancel"].WasPressedThisFrame();
        }
    }
}