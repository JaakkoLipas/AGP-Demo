using UnityEngine;
using System.Collections.Generic;

namespace AoV.Gameplay
{
	public enum DamageType
	{
		Melee,
		Ranged,
		Charge,
		Enemy
	}

	public interface IDamageable
	{
		List<DamageType> EffectiveDamageTypes { get; set; }
		float MaxHealth { get; set; }
		float CurrentHealth { get; set; }
		float InvincibilityFrames { get; set; }
		bool IsInvincible { get; set; }

		void TakeDamage(float damage, bool useIFrames);
		void Kill(bool isInstant);
	}
}