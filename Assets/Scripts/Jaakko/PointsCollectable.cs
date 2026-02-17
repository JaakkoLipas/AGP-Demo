using System;
using Unity.Cinemachine;
using UnityEngine;

namespace AoV.Gameplay
{
    [Serializable, RequireComponent (typeof(Collider2D))]
    public class PointsCollectable : MonoBehaviour, ICollectable
    {
        private readonly ICollectable.CollectableType _cType = ICollectable.CollectableType.Points;
        public ICollectable.CollectableType CType { get { return _cType; } }
        [SerializeField] private float _value;
        public float CValue { get { return _value; } }
        [SerializeField, TagField] private string _collectorTag;
        public string CollectorTag { get { return _collectorTag; } }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) CollectObject();
            else return;
        }

        public void CollectObject()
        {
            Player.PlayerCore.PointChangeEvent?.Invoke(_value);
            Destroy(this.gameObject);
        }
    }

}