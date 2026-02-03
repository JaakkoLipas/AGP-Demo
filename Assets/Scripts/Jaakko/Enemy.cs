using UnityEngine;
using System.Collections.Generic;

namespace AG3958
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private float _maxHealth = 1.0f;
        private float _currentHealth;
        [SerializeField] private float _scoreValue = 1.0f;
        [SerializeField] private bool _isBoss = false;

        [Header("Combat")]
        [SerializeField] private bool _doesContactDamage = true;
        [SerializeField, Tooltip("Field is unused if contact damage is turned off")] private float _contactDamage = 1.0f;
        [SerializeField] private bool _contactInvokesIFrames = true;
        [SerializeField] private bool _knockbackEnabled = true;
        [SerializeField] private float _knockbackStrength = 1.0f;
        private Vector2 _knockbackForceMultiplier;

        [Tooltip("List of projectile types that can deal damage")]
        public List<DamageType> EffectiveDamageTypes;

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

        private void Awake()
        {
            _knockbackForceMultiplier = new Vector2(_knockbackStrength, _knockbackStrength);
            _healthDrop = _healthDropPrefab.GetComponent<HealthCollectable>();
            _currentHealth = _maxHealth;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Speed"))
            {
                if (!_isBoss) Kill(true);
                else TakeDamage(_maxHealth * 0.5f);
            }
            if (other.collider.CompareTag("Player"))
            {
                PlayerController pcon = other.gameObject.GetComponent<PlayerController>();
                PlayerCore pcor = other.gameObject.GetComponentInParent<PlayerCore>();
                if (_doesContactDamage && !pcor.IsInvincible)
                {
                    if (_knockbackEnabled)
                    {
                        Vector2 kbVector = (Vector2)other.transform.position - (Vector2)transform.position;
                        kbVector.Scale(_knockbackForceMultiplier);
                        pcon.Launch(kbVector * _knockbackStrength, true);
                    }
                    PlayerCore.HealthChangeEvent?.Invoke(-_contactDamage, _contactInvokesIFrames);
                }
            }
        }

        public void TakeDamage(float damage) 
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0) { Kill(false); }
            else { } // TODO: enemy-specific damaged vfx/sfx
        }

        private void Kill(bool isInstant)
        {
            int RNGResult;
            if (isInstant)
            {
                // TODO: instant (speed) kill vfx/sfx
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
                // TODO: normal kill vfx/sfx
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