using Features.Actions;
using Features.Buffs;
using Features.Equipment;
using Features.Health;
using Features.Inventory;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Character
{
    public class CharacterC
    {
        public class Character : MonoBehaviour
        {
            public bool Buffs;

            public bool Inventory;

            public bool Equipment;

            public bool Stats;

            public bool Health;

            [Range(1, 100)] public int MaxHealth = 20;

            [Range(1, 100)] public int CurrentHealth = 10;

            [Range(1, 100)] public int InventorySize = 5;

            public SlotData[] EquipmentSlots = new[] {new SlotData() {slotType = "Main"}};

            public Stats_SO BaseStats;

            private ActionsController m_ActionsController;

            private CharacterActionsManager m_ActionsManager;

            private BuffController m_BuffController;

            private CharacterBuffsManager m_BuffsManager;

            private EquipmentController m_EquipmentController;

            private CharacterEquipmentManager m_EquipmentManager;

            private HealthController m_HealthController;

            private InventoryController m_InventoryController;

            private CharacterInventoryManager m_InventoryManager;

            private StatsController m_StatsController;

            private CharacterStatsManager m_StatsManager;

            private void Awake()
            {
                var systemsParent = new GameObject("systems").transform;

                var root = transform;

                systemsParent.parent = root;

                var managersParent = new GameObject("managers").transform;

                managersParent.parent = root;

                NewMethod(systemsParent, "actions", ref m_ActionsController);

                if (Inventory)
                {
                    NewMethod(systemsParent, "inventory", ref m_InventoryController);

                    m_InventoryController.Initialize(InventorySize);
                }

                if (Equipment)
                {
                    NewMethod(systemsParent, "equipment", ref m_EquipmentController);

                    m_EquipmentController.Initialize(EquipmentSlots);
                }

                if (Buffs)
                {
                    NewMethod(systemsParent, "buffs", ref m_BuffController);
                }

                if (Stats)
                {
                    NewMethod(systemsParent, "stats", ref m_StatsController);

                    if (BaseStats)
                    {
                        m_StatsController.Initialize(BaseStats.Stats);
                    }
                }

                if (Health)
                {
                    NewMethod(systemsParent, "health", ref m_HealthController);

                    m_HealthController.Initialize(CurrentHealth, MaxHealth);
                }


                if (Inventory)
                {
                    NewMethod(managersParent, "inventory", ref m_InventoryManager);
                }

                if (Equipment)
                {
                    NewMethod(managersParent, "equipment", ref m_EquipmentManager);
                }

                if (Buffs)
                {
                    NewMethod(managersParent, "buffs", ref m_BuffsManager);
                }

                if (Stats)
                {
                    NewMethod(managersParent, "stats", ref m_StatsManager);
                }
            }

            private void NewMethod<T>(Transform parent, string componentName, ref T holder) where T : MonoBehaviour
            {
                var i = new GameObject(componentName);

                i.transform.parent = parent;

                holder = i.AddComponent<T>();
            }
        }
    }
}