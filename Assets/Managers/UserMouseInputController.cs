using System;
using Features.Character;
using Features.Movement;
using Features.Targeting;
using Integrations.Actions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Managers
{
    public class UserMouseInputController : SingletonManager<UserMouseInputController>
    {
        private Player m_Player;

        private GameObject m_PlayerRoot;
        
        public LayerMask GroundAndPlayerMask;

        public LayerMask GroundMask;
        
        public bool DisableInput = false;

        public bool DisableMovement = false;

        private void Start()
        {
            m_Player = GameplayManager.Instance.Player;

            m_PlayerRoot = m_Player.transform.root.gameObject;

            GroundAndPlayerMask = LayerMask.GetMask("Ground", "PlayerHitbox");
            
            GroundMask = LayerMask.GetMask("Ground");
            
            LocationProvider.OnOverlayActivated += OnOverlayActivated;
            
            LocationProvider.OnOverlayDisabled += OnOverlayDisabled;
        }
        
        private void OnOverlayDisabled()
        {
            DisableInput = false;
            DisableMovement = false;
        }

        private void OnOverlayActivated(OverlayInfo info)
        {
            DisableInput = true;

            if (info.BlockMovementActions)
            {
                DisableMovement = true;
            }
        }

        private void Update()
        {
            if (DisableInput)
            {
                if (DisableMovement) return;
                
                if (Input.GetMouseButtonUp(0))
                {
                    if (EventSystem.current != null &&
                        EventSystem.current.IsPointerOverGameObject()) return;
                
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (!Physics.Raycast(ray, out RaycastHit hit, 100f, GroundAndPlayerMask)) return;
                    
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
                        return;
                        
                    var movePayload =
                        Move.MakePayload(m_PlayerRoot, new MoveActionData(hit.point));

                    m_Player.m_ActionsController.DoAction(movePayload);
                }
                
                return;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject()) return;


                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, GroundMask))
                {
                    var movePayload =
                        Move.MakePayload(m_Player.transform.root.gameObject, new MoveActionData(hit.point));

                    m_Player.m_ActionsController.DoAction(movePayload);
                }
            }
        }
    }
}