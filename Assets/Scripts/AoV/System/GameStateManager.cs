using System;
using UnityEngine;

namespace AoV.System
{
    public enum GameState
    {
        Active,
        Paused,
        Failure,
        Complete
    }

    public class GameStateManager : MonoBehaviour
    {
        private GameState _currentState;
        public GameState CurrentState { get { return _currentState; } set { _currentState = value; GameStateChanged?.Invoke(); } }

        public static event Action GameStateChanged;
        public static Action WinGameEvent;

        [SerializeField] private WorldProgressionFlags _winConditionFlag;
        private bool _winConditionState;
        private bool _winInvoked;

        // TODO: make the game states actually do stuff

        private void Awake()
        {
            CurrentState = GameState.Active;
            ProgressionDataManager.FlagChangedEvent += CheckWinConState;
        }

        private void Update()
        {
            if (!_winInvoked && _winConditionState) 
            { 
                CurrentState = GameState.Complete;
                WinGameEvent?.Invoke(); 
                _winInvoked = true;
            }
        }

        private void CheckWinConState()
        {
            _winConditionState = ProgressionDataManager.GetFlag(_winConditionFlag);
        }

        private void OnDestroy()
        {
            GameStateChanged = null;
            WinGameEvent = null;
            ProgressionDataManager.FlagChangedEvent -= CheckWinConState;
        }
    } 
}
