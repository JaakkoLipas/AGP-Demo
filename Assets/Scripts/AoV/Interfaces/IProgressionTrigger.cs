using UnityEngine;
using AoV.System;

namespace AoV.Gameplay
{
	public interface IProgressionTrigger
	{
		WorldProgressionFlags FlagSet { get; }

		void CallSetFlag();
	}
}