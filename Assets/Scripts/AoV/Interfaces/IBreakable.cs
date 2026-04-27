using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AoV.Gameplay
{
    public enum BreakType
    {
        Melee,
        Ranged,
        Charge,
        Enemy,
        Player,
        Speed,
        Indestructible
    }

    public interface IBreakable
    {
        List<BreakType> BreakTypes { get; }

        IEnumerator Break();
    }
}