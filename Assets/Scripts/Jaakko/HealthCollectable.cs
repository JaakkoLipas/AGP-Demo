using System;
using Unity.Cinemachine;
using UnityEngine;

namespace AoV.Gameplay
{
    [Serializable, RequireComponent (typeof(Collider2D))]
    public class HealthCollectable : MonoBehaviour, ICollectable
    {
        private readonly ICollectable.CollectableType _cType = ICollectable.CollectableType.Health;
        public ICollectable.CollectableType CType { get { return _cType; } }
        [SerializeField] private float _value;
        public float CValue { get { return _value; } }
        [SerializeField, TagField] private string _collectorTag;
        public string CollectorTag { get { return _collectorTag; } }

        [Tooltip("Does collecting the item invoke iframes?")]
        [SerializeField] private bool _invokesIFrames = false;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) CollectObject();
            else return;
        }

        public void CollectObject()
        {
            Player.PlayerCore.HealthChangeEvent?.Invoke(_value, _invokesIFrames);
            Destroy(this.gameObject);
        }
    }

}