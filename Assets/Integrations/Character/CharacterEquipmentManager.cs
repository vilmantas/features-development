using Features.Equipment;
using Features.Items;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Character
{
    public class CharacterEquipmentManager : MonoBehaviour
    {
        private EquipmentController m_EquipmentController;

        private StatsController m_StatsController;

        private void Awake()
        {
            var root = transform.root;

            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();

            m_StatsController = root.GetComponentInChildren<StatsController>();
        }
    }
}