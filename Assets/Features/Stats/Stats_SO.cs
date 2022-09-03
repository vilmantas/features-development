using UnityEngine;

namespace Features.Stats.Base
{
    [CreateAssetMenu(fileName = "Empty Stats", menuName = "Stats/Empty Stats", order = 0)]
    public class Stats_SO : ScriptableObject
    {
        public Stat[] Stats = {new("Strength", 1), new("Defence", 1), new("Constitution", 1)};
    }
}