using System;
using System.Collections.Generic;
using UnityEngine;

namespace AoV.System
{
	public enum ItemProgressionFlags
	{
		ABILITY_WEAPON,
		ABILITY_MAGIC,
		ABILITY_CHARGE,
		ABILITY_WALLHANG,
		ABILITY_SPEED,
		ABILITY_ERUPTION
	}

	public enum WorldProgressionFlags
	{
		ENEMY_FINALBOSS_KILLED
	}

	[Serializable]
	public static class ProgressionDataManager
	{
		private static Dictionary<ItemProgressionFlags, bool> _itemProgressionData;
		private static Dictionary<WorldProgressionFlags, bool> _worldProgressionData;

		public static event Action FlagChangedEvent;

		static ProgressionDataManager()
		{
			_itemProgressionData = new Dictionary<ItemProgressionFlags, bool>();
			_worldProgressionData = new Dictionary<WorldProgressionFlags, bool>();
			InitializeData();
		}

		/// <summary>
		/// Clears all entries from the flag dictionaries and reforms them to all-false values.
		/// </summary>
		public static void InitializeData()
		{
			_itemProgressionData.Clear();
			_worldProgressionData.Clear();
			foreach (ItemProgressionFlags ipf in Enum.GetValues(typeof(ItemProgressionFlags)))
			{
				_itemProgressionData.Add(ipf, false);
			}
			foreach (WorldProgressionFlags wpf in Enum.GetValues(typeof(WorldProgressionFlags)))
			{
				_worldProgressionData.Add(wpf, false);
			}
		}

		public static bool GetFlag(ItemProgressionFlags flag)
		{
			return _itemProgressionData[flag];
		}

		public static bool GetFlag(WorldProgressionFlags flag)
		{
			return _worldProgressionData[flag];
		}

		public static void SetFlag(ItemProgressionFlags flag, bool value)
		{
			_itemProgressionData[flag] = value;
			FlagChangedEvent?.Invoke();
		}

		public static void SetFlag(WorldProgressionFlags flag, bool value)
		{
			_worldProgressionData[flag] = value;
			FlagChangedEvent?.Invoke();
		}

		public static float CalculateItemCompletion()
		{
			int collectedItems = 0;
			int totalItems = 0;
			foreach (ItemProgressionFlags ipf in _itemProgressionData.Keys)
			{
				if (_itemProgressionData[ipf] == true) collectedItems++;
				totalItems++;
			}
			return Mathf.Round((collectedItems / totalItems) * 100);
		}
	}
}