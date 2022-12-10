using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Features.Targeting
{
    public static class LocationProvider
    {
        public static Action OnOverlayActivated;

        public static Action OnOverlayDisabled;
        
        public static void EnableTargeting()
        {
            var manager = GameObject.Find("TargetingProvider").GetComponent<TargetingManager>();
            
            manager.Initialize(ReportTarget);
            
            OnOverlayActivated?.Invoke();
        }

        private static void ReportTarget(GameObject obj)
        {
            Debug.Log(obj.name);
            
            OnOverlayDisabled?.Invoke();
        }

        public static GameObject PlayerLocationProvider(Action<GameObject> callback)
        {
            return  GameObject.Find("Player").gameObject;
        }

        public static Vector3 MousePositionProvider()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
            {
                return hit.point;
            }
            
            return Vector3.zero;
        }

        public static GameObject StartCharacterSelect(Action<GameObject> callback)
        {
            return new GameObject();
        }
    }
}