using UnityEngine;

namespace AoV.Gameplay
{
	public interface ICollectable
	{
		enum CollectableType { Points, Health, Mana, Major }
		CollectableType CType { get; }
		float CValue { get; }
		string CollectorTag { get; }

		void OnTriggerEnter2D(Collider2D collision);
		void CollectObject();
	}

}