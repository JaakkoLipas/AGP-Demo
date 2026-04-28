using UnityEngine;
using AoV.System;

namespace AoV.Gameplay
{
	public interface IProgressionTriggered
	{
		ItemProgressionFlags ItemFlag { get; }
		WorldProgressionFlags WorldFlag { get; }
	}
}