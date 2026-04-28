using UnityEngine;
using EditorAttributes;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using AoV.System;
using AoV.Gameplay;
using System.Collections.Generic;

namespace AoV.Player
{
    /// <summary>
    /// Player core component. Handles everything related to the player's basic stats, including triggering failstate and respawning.
    /// </summary>
    [Serializable]
    public class PlayerCore : MonoBehaviour, IDamageable
    {
        #region Fields

        public static Action<float, bool> HealthChangeEvent;
        public static Action<float> ManaChangeEvent;
        public static Action<float> PointChangeEvent;
        public static event Action<bool> PlayerDeathEvent;
        public static event Action PlayerRespawnEvent;

        [Header("Basic Stats")]
        [field: SerializeField] public List<DamageType> EffectiveDamageTypes { get; set; }
        [field: SerializeField, Clamp(1, Single.MaxValue)] public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }

        [Tooltip("Invincibility frames (based on Fixed Update framerate)")]
        [field: SerializeField] public float InvincibilityFrames { get; set; }
        public bool IsInvincible { get; set; }
        private readonly WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

        [SerializeField, Clamp(1, Single.MaxValue)] private float _maxMana;
        public float MaxMana {  get { return _maxMana; } }
        private float _currentMana;
        public float PlayerMana { get { return _currentMana; } }

        [Tooltip("Mana regeneration speed while regen is active (per 1/50th of second)")]
        [SerializeField] private float _manaRegenSpeed = 1.0f;
        private WaitForSeconds _manaRegenWait = new WaitForSeconds(0.02f);
        [SerializeField] private float _manaRegenTime = 3.0f;
        private float _manaRegenTimer = 0.0f;
        private bool _manaRegenActive = false;
        public bool ManaRegenActive { get { return _manaRegenActive; } }

        [SerializeField, Clamp(0, Single.MaxValue)] private float _points = 0f;
        public float PlayerPoints { get { return _points; } }

        [SerializeField] private float _respawnTime = 5f;
        private float _respawnTimer;

        [Header("Progression Checks")]
        [SerializeField] private bool _setChecksByData;
        [SerializeField] private bool _hasWeapon;
        public bool HasWeapon { get { return _hasWeapon; } }

        [SerializeField] private bool _hasMagic;
        public bool HasMagic { get { return _hasMagic; } }

        [SerializeField] private bool _hasWallHang;
        public bool HasWallHang { get { return _hasWallHang; } }

        [SerializeField] private bool _hasCharge;
        public bool HasCharge { get { return _hasCharge; } }

        [SerializeField] private bool _hasVolcanicEruption;
        public bool HasVolcanicEruption { get { return _hasVolcanicEruption; } }

        [SerializeField] private bool _hasSpeedBooster;
        public bool HasSpeedBooster { get { return _hasSpeedBooster; } }

        [Header("Debugging")]
        [SerializeField] private Checkpoint _initialCheckpoint;
        [SerializeField, ReadOnly] private Checkpoint _previousCheckpoint;
        public Checkpoint PreviousCheckpoint { get { return _previousCheckpoint; } }
        
        #endregion

        private void Awake()
        {
            CurrentHealth = MaxHealth;
            IsInvincible = false;
            _currentMana = _maxMana;
            _previousCheckpoint = _initialCheckpoint;
            if (_setChecksByData) SetAbilityChecks();

            HealthChangeEvent += OnHealthChanged;
            ManaChangeEvent += OnManaChanged;
            PointChangeEvent += OnPointsChanged;
            PlayerDeathEvent += Kill;
            PlayerRespawnEvent += Respawn;
            if (_setChecksByData) ProgressionDataManager.FlagChangedEvent += SetAbilityChecks;
        }

        private void Update()
        {
            _manaRegenTimer += Time.deltaTime;
            if (_currentMana < _maxMana && !_manaRegenActive && _manaRegenTimer >= _manaRegenTime)
                { StartCoroutine(ManaRecharge()); }
        }

        private void SetAbilityChecks()
        {
            _hasWeapon = ProgressionDataManager.GetFlag(ItemProgressionFlags.ABILITY_WEAPON);
            _hasMagic = ProgressionDataManager.GetFlag(ItemProgressionFlags.ABILITY_MAGIC);
            _hasCharge = ProgressionDataManager.GetFlag(ItemProgressionFlags.ABILITY_CHARGE);
            _hasWallHang = ProgressionDataManager.GetFlag(ItemProgressionFlags.ABILITY_WALLHANG);
            _hasSpeedBooster = ProgressionDataManager.GetFlag(ItemProgressionFlags.ABILITY_SPEED);
            _hasVolcanicEruption = ProgressionDataManager.GetFlag(ItemProgressionFlags.ABILITY_ERUPTION);
        }

        public void TakeDamage(float damage, bool useIFrames)
        {
            if (!IsInvincible)
            {
                HealthChangeEvent?.Invoke(-damage, useIFrames);
            }
        }

        private void OnHealthChanged(float value, bool useIFrames)
        {
            if (CurrentHealth + value > MaxHealth) { CurrentHealth = MaxHealth; }
            else if (CurrentHealth + value <= 0)
            { 
                CurrentHealth = 0;
                PlayerDeathEvent?.Invoke(false);
            }
            else { CurrentHealth += value; }
            if (useIFrames) StartCoroutine(InvincibilityTime());
        }

        private void OnManaChanged(float value)
        {
            if (_currentMana + value > _maxMana)
            {
                _currentMana = _maxMana;
                StopRecharge();
            }
            else if (_currentMana + value < 0) { _currentMana = 0; }
            else { _currentMana += value; }
            if (value < 0) { StopRecharge(); }
        }

        private void OnPointsChanged(float value)
        {
            if (_points + value < 0) { _points = 0; }
            else { _points += value; }
        }

        private IEnumerator ManaRecharge()
        {
            _manaRegenActive = true;
            while (_currentMana < _maxMana && _manaRegenActive)
            {
                ManaChangeEvent?.Invoke(_manaRegenSpeed);
                yield return _manaRegenWait;
            }
            _manaRegenActive = false;
        }

        public void StopRecharge()
        {
            _manaRegenTimer = 0.0f;
            StopCoroutine(ManaRecharge());
            _manaRegenActive = false;
        }

        private IEnumerator InvincibilityTime()
        {
            IsInvincible = true;
            int iterator = 0;
            while (iterator < InvincibilityFrames)
            {
                iterator++;
                yield return _waitForFixedUpdate;
            }
            IsInvincible = false;
        }

        public void SetCheckpoint(Checkpoint point)
        {
            _previousCheckpoint = point;
        }

        public void Kill(bool isInstant)
        {
            PlayerController pc = this.GetComponent<PlayerController>();
            pc.enabled = false;
            SpriteRenderer[] srs = this.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in srs) sr.enabled = false;
            StartCoroutine(RespawnTimer());
        }

        private IEnumerator RespawnTimer()
        {
            while (_respawnTimer < _respawnTime)
            {
                _respawnTimer += Time.fixedDeltaTime;
                yield return _waitForFixedUpdate;
            }
            PlayerRespawnEvent?.Invoke();
            _respawnTimer = 0.0f;
        }

        private void Respawn()
        {
            SceneLoader.Instance.LoadSceneWithFade(0);
        }

        private void OnDestroy()
        {
            PointChangeEvent = null;
            HealthChangeEvent = null;
            ManaChangeEvent = null;
            PlayerDeathEvent = null;
            PlayerRespawnEvent = null;
            if (_setChecksByData) ProgressionDataManager.FlagChangedEvent -= SetAbilityChecks;
        }
    }
}