using System;
using UnityEngine;
using AoV.Player;

namespace AoV.System
{
    public class CheckpointSystem : MonoBehaviour
    {
        private Checkpoint[] _checkpointList;
        public Checkpoint[] CheckpointList { get { return _checkpointList; } }

        private PlayerCore _playerCore;

        private void Start()
        {
            _checkpointList = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
            Array.Sort(_checkpointList);
        }
    }
}