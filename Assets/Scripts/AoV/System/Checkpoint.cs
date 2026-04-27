using System;
using UnityEngine;
using Unity.Cinemachine;
using EditorAttributes;
using AoV.Player;
using UnityEngine.SceneManagement;

namespace AoV.System
{
    public class Checkpoint : MonoBehaviour, IComparable<Checkpoint>
    {
        [SerializeField, Validate("Checkpoint ID cannot be empty", nameof(CheckEmpty))] private string _checkpointID;
        public string CheckpointID { get { return _checkpointID; } }

        private bool CheckEmpty() => _checkpointID == string.Empty;

        private Vector2 _checkpointTarget;
        public Vector2 CheckpointTarget { get { return _checkpointTarget; } }

        private Scene _parentScene;
        public Scene ParentScene { get { return _parentScene; } }

        private PlayerCore _playerCore;
        [SerializeField, TagField] private string _playerTag;

        private void Start()
        {
            _checkpointTarget = transform.position;
            _playerCore = FindFirstObjectByType<PlayerCore>();
            _parentScene = this.gameObject.scene;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(_playerTag) && _playerCore.PreviousCheckpoint != this)
            {
                _playerCore.SetCheckpoint(this);
            }
        }

        public int CompareTo(Checkpoint other)
        {
            return string.Compare(this._checkpointID, other.CheckpointID);
        }
    }

}