using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SampleGames.Survivr
{
    public class UserInputManager : MonoBehaviour
    {
        public static Action<Vector3> OnGroundClicked;
        
        public LayerMask GroundLayer;

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject()) return;
                
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, GroundLayer))
                {
                    OnGroundClicked?.Invoke(hit.point);
                }   
            }
        }
    }
}