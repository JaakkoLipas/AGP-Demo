using UnityEngine;

namespace AoV.Gameplay
{
	public interface IPhysicsInteractor
	{
		bool IsOneShot { get; }

		void OnTriggerEnter2D(Collider2D other);
		void ToggleEnabled();

	}

}