using System.Linq;
using UnityEngine;

namespace Stats.Unity
{
    [CreateAssetMenu(menuName = "Stats/Stat Group", fileName = "Base Group")]
    public class Preset : ScriptableObject
    {
        [SerializeField] private Stat[] SetupStats;

        public Stat[] Stats()
        {
            return SetupStats.Select(setupStat => setupStat?.Copy).ToArray();
        }
    }
}