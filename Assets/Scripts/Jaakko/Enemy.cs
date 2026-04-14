using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using AoV.Player;

namespace AoV.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Header("Stats")]
        [field: SerializeField] public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public float InvincibilityFrames { get; set; }
        public bool IsInvincible { get; set; }
        [SerializeField] private float _scoreValue = 1.0f;
        [SerializeField] private bool _isBoss = false;

        [Header("Combat")]
        [SerializeField] private bool _doesContactDamage = true;
        [SerializeField, Tooltip("Field is unused if contact damage is turned off")] private float _contactDamage = 1.0f;
        [SerializeField] private bool _contactInvokesIFrames = true;

        [Tooltip("List of projectile types that can deal damage")]
        [field: SerializeField] public List<DamageType> EffectiveDamageTypes { get; set; }

        [Header("On Death")]
        [Tooltip("Delay to destroy the object, for cinematic purposes")]
        [SerializeField] private float _destroyDelay = 0.0f;
        [Tooltip("Base perccentage chance for this enemy to drop health on death")]
        [SerializeField] private int _healthDropChance = 10;
        [Tooltip("Number of times to run the health drop RNG on this enemy's death")]
        [SerializeField] private int _healthDropCount = 1;
        [SerializeField] private GameObject _healthDropPrefab;
        private HealthCollectable _healthDrop;
        [SerializeField] private GameObject _normalDeathParticles;
        [SerializeField] private GameObject _instantDeathParticles;
        private bool _isDying = false;
        public bool IsDying { get { return _isDying; } }

        [Header("Tag Compares")]
        [SerializeField, TagField] private string _speedTag;
        [SerializeField, TagField] private string _playerTag;

        private void Awake()
        {
            _healthDrop = _healthDropPrefab.GetComponent<HealthCollectable>();
            CurrentHealth = MaxHealth;
            IsInvincible = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag(_speedTag))
            {
                if (!_isBoss) Kill(true);
                else TakeDamage(MaxHealth * 0.5f, false);
            }
            if (other.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                if (damageable.EffectiveDamageTypes.Contains(DamageType.Enemy))
                {
                    damageable.TakeDamage(_contactDamage, _contactInvokesIFrames);
                }
            }
        }

        public void TakeDamage(float damage, bool useIFrames) 
        {
            if (!IsInvincible)
            {
                CurrentHealth -= damage;
                if (CurrentHealth <= 0) { Kill(false); }
                else 
                {
                    // TODO: enemy-specific damaged VFX/SFX calls
                }
            }
        }

        public void Kill(bool isInstant)
        {
            int RNGResult;
            if (isInstant)
            {
                // TODO: instant (speed) kill VFX/SFX calls
                for (int i = 0; i < _healthDropCount; i++)
                {
                    RNGResult = Random.Range(0, 100);
                    if (RNGResult < _healthDropChance)
                    {
                        PlayerCore.HealthChangeEvent?.Invoke(_healthDrop.CValue, false);
                    }
                }
            }
            else 
            {
                Vector2 pickupSpawn = new Vector2();
                // TODO: normal kill VFX/SFX calls
                for (int i = 0; i < _healthDropCount; i++)
                {
                    RNGResult = Random.Range(0, 100);
                    if (RNGResult < _healthDropChance)
                    {
                        pickupSpawn = Random.insideUnitCircle * ((transform.lossyScale.x + transform.lossyScale.y) / 2);
                        Instantiate(_healthDropPrefab, pickupSpawn, Quaternion.identity);
                    }
                }
            }
            PlayerCore.PointChangeEvent?.Invoke(_scoreValue);
            _isDying = true;
            Destroy(this.gameObject);
        }
    }
}