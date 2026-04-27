using UnityEngine;

namespace AoV.Gameplay
{
    public enum CollectableType
	{ 
		Points, 
		Health, 
		Mana, 
		Major 
	}

    public interface ICollectable
	{
		CollectableType CType { get; }
		float CValue { get; }
		string CollectorTag { get; }

		void OnTriggerEnter2D(Collider2D collision);
		void CollectObject();
	}

}