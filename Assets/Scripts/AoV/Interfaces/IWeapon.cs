using UnityEngine;

namespace AoV.Gameplay
{
	public interface IWeapon
	{
		void LaunchProjectile(Vector2 launchVector);
	}
}