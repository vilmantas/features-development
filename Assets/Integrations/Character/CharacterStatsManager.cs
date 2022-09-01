using Features.Equipment;
using Features.Items;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Character
{
    public class CharacterStatsManager : MonoBehaviour
    {
        private StatsController m_StatsController;

        private void Awake()
        {
            var root = transform.root;

            m_StatsController = root.GetComponentInChildren<StatsController>();
        }
    }
}