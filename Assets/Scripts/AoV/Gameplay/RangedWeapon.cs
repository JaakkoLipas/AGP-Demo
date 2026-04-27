using UnityEngine;
using AoV.Player;

namespace AoV.Gameplay
{
    public class RangedWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _cooldown;
        private float _cooldownTimer = 0.0f;
        [SerializeField] private float _launchForce;

        [SerializeField] private bool _isPlayerWeapon;
        [SerializeField] private float _manaCost;

        private void Update()
        {
            _cooldownTimer += Time.deltaTime;
        }

        public void LaunchProjectile(Vector2 launchVector)
        {
            if (_cooldownTimer < _cooldown)
            {
                if (_isPlayerWeapon)
                {
                    PlayerCore pc = FindFirstObjectByType<PlayerCore>();
                    if (pc.PlayerMana < _manaCost) return;
                    else PlayerCore.ManaChangeEvent?.Invoke(-_manaCost);
                }
                Rigidbody2D pRB = Instantiate(_projectilePrefab, launchVector, Quaternion.identity).GetComponent<Rigidbody2D>();
                pRB.linearVelocity = launchVector * _launchForce;
                _cooldownTimer = 0.0f;
            }
        }
    }

}