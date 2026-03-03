using UnityEngine;

namespace AoV.Gameplay
{
	public class MeleeWeapon : MonoBehaviour, IWeapon
	{
		[SerializeField] private GameObject _projectilePrefab;
		[SerializeField] private float _cooldown;
		private float _cooldownTimer;

        private void Update()
        {
			_cooldownTimer += Time.deltaTime;
        }

        public void LaunchProjectile(Vector2 launchVector)
		{
			if (_cooldownTimer > _cooldown)
			{
				Instantiate(_projectilePrefab, launchVector, Quaternion.identity);
				_cooldownTimer = 0.0f;
			}
		}
	}
}