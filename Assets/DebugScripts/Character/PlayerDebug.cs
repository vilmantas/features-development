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
            
            if (Input.GetMouseButtonDown(2)) PlayerInstance.m_CombatController.SetBlocking(true);

            if (Input.GetMouseButtonUp(2)) PlayerInstance.m_CombatController.SetBlocking(false);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!SkillMetadataRegistry.Implementations.TryGetValue("Basic Attack",
                        out var skill)) return;
                
                var payload = ActivateSkill.MakePayload(RootGameObject, skill);

                PlayerInstance.m_ActionsController.DoAction(payload);
                
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, GroundLayer))
                {
                    var x = hit.point;

                    x.y = transform.position.y;

                    transform.LookAt(x);
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                if (!SkillMetadataRegistry.Implementations.TryGetValue("Meteor Strike",
                        out var skill)) return;
                
                var payload = ActivateSkill.MakePayload(RootGameObject, skill);

                PlayerInstance.m_ActionsController.DoAction(payload);
            }
            
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (!SkillMetadataRegistry.Implementations.TryGetValue("Meteor Strike Location",
                        out var skill)) return;
                
                var payload = ActivateSkill.MakePayload(RootGameObject, skill);

                PlayerInstance.m_ActionsController.DoAction(payload);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                LocationProvider.EnableTargeting(TargetingType.Character);
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                LocationProvider.EnableTargeting(TargetingType.Mouse);
            }
        }
    }
}