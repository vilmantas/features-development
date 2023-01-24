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
        public LayerMask GroundAndPlayerMask;

        public Action<Vector3> OnGameGroundClicked;

        public Action<Vector3> OnCharacterLocationClicked;

        private void Start()
        {
            GroundAndPlayerMask = LayerMask.GetMask("Ground", "PlayerHitbox");
        }

        private void Update()
        {
            if (!Input.GetMouseButtonUp(0)) return;
            
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject()) return;
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit, 100f, GroundAndPlayerMask)) return;

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
            {
                OnCharacterLocationClicked?.Invoke(hit.point);

                return;
            }

            OnGameGroundClicked?.Invoke(hit.point);
        }
    }
}