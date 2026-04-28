using System;
using UnityEngine;
using Unity.Cinemachine;
using AoV.System;

namespace AoV.Gameplay
{
    [Serializable, RequireComponent(typeof(Collider2D))]
    public class MajorCollectable : MonoBehaviour, ICollectable
    {
        private readonly CollectableType _cType = CollectableType.Major;
        public CollectableType CType { get { return _cType; } }
        [Tooltip("Select the ID flag that this item sets true on collection")]
        [SerializeField] private ItemProgressionFlags _itemIDFlag;
        [Tooltip("For a Major Item, this is a points value")]
        [SerializeField] private float _value;
        public float CValue { get { return _value; } }
        [SerializeField, TagField] private string _collectorTag;
        public string CollectorTag { get { return _collectorTag; } }

        private void Start()
        {
            if (ProgressionDataManager.GetFlag(_itemIDFlag)) Destroy(this.gameObject);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_collectorTag)) CollectObject();
            else return;
        }

        public void CollectObject()
        {
            ProgressionDataManager.SetFlag(_itemIDFlag, true);
            Player.PlayerCore.PointChangeEvent?.Invoke(_value);
            Destroy(this.gameObject);
        }
    }
}