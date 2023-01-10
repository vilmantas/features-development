using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Features.Targeting
{
    public class OverlayInfo
    {
        public bool BlockMovementActions;
    }
    
    public static class LocationProvider
    {
        public static Action<OverlayInfo> OnOverlayActivated;

        public static Action OnOverlayDisabled;
        
        public static void EnableTargeting(TargetingType type)
        {
            var manager = TargetingManager.Instance;

            OverlayInfo info = new();
            
            switch (type)
            {
                case TargetingType.Character:
                    manager.Initialize(ReportCharacter);

                    info = new() {BlockMovementActions = false};
                    break;
                
                case TargetingType.Mouse:
                    manager.Initialize(ReportPosition);

                    info = new OverlayInfo() {BlockMovementActions = true};
                    break;
            }
            
            OnOverlayActivated?.Invoke(info);
        }

        private static void ReportPosition(Vector3 obj)
        {
            Debug.Log(obj);
            
            OnOverlayDisabled?.Invoke();
        }

        private static void ReportCharacter(GameObject obj)
        {
            Debug.Log(obj.name);
            
            OnOverlayDisabled?.Invoke();
        }

        public static void PlayerLocationProvider(Action<GameObject> callback)
        {
            callback.Invoke(GameObject.Find("Player").gameObject);
        }

        public static GameObject PlayerLocationProvider()
        {
            return GameObject.Find("Player").gameObject;
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

        public static void StartCharacterSelect(Action<GameObject> callback)
        {
            var manager = GameObject.Find("targeting_manager").GetComponent<TargetingManager>();
            
            manager.Initialize(x =>
            {
                callback.Invoke(x);
                
                OnOverlayDisabled?.Invoke();
            });

            OnOverlayActivated?.Invoke(new OverlayInfo() {BlockMovementActions = false});
        }

        public static void StartMousePositionSelect(Action<Vector3> callback)
        {
            var manager = GameObject.Find("targeting_manager").GetComponent<TargetingManager>();

            manager.Initialize(x =>
            {
                callback.Invoke(x);

                OnOverlayDisabled?.Invoke();
            });
            
            OnOverlayActivated?.Invoke(new OverlayInfo() {BlockMovementActions = true});
        }
    }
}