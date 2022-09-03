using UnityEngine;

namespace Features.Stats.Base
{
    public class StartingStats : MonoBehaviour
    {
        public Stats_SO Stats;

        private void Start()
        {
            var statsController = transform.root.GetComponentInChildren<StatsController>();

            statsController.ApplyStatModifiers(new StatGroup(Stats.Stats));
        }
    }
}