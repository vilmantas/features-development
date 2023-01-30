using System.Collections.Generic;
using System.Linq;
using Features.Buffs.UI;
using Features.Character;
using Features.Conditions;
using Features.Equipment.UI;
using Features.Health;
using Features.Inventory.UI;
using Features.Skills;
using Features.Stats.Base;
using Features.Targeting;
using Integrations.Items;
using Integrations.Skills.Actions;
using Integrations.Skills.UI;
using UnityEngine;
using Utilities.ItemsContainer;

namespace DebugScripts.Character
{
    public class PlayerDebug : MonoBehaviour
    {
        public LayerMask GroundLayer;

        public Player PlayerInstance;
        
        public GameObject RootGameObject;
        
        private void Start()
        {
            RootGameObject = transform.root.gameObject;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) PlayerInstance.m_MovementController.SetRunning(true);

            if (Input.GetKeyUp(KeyCode.LeftShift)) PlayerInstance.m_MovementController.SetRunning(false);
        }
    }
}