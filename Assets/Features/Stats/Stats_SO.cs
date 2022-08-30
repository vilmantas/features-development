using System.Collections.Generic;
using UnityEngine;

namespace Features.Stats.Base
{
    [CreateAssetMenu(fileName = "Empty Stats", menuName = "Stats/Empty Stats", order = 0)]
    public class Stats_SO : ScriptableObject
    {
        public List<Stat> Stats = new() { new Stat("Strength", 1), new Stat("Defence", 1), new Stat("Constitution", 1) };
    }
}