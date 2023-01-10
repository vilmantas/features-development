using System;
using Managers;
using TMPro;
using UnityEngine;

namespace Features.Targeting
{
    public class TargetingManager : SingletonManager<TargetingManager>
    {
        private GameObject OverlayPrefab;

        private GameObject m_OverlayInstance;

        private LayerMask GroundAndPlayer;

        private TextMeshProUGUI Text;

        private bool Activated = false;
        
        private Action<GameObject> m_CharacterCallback;

        private Action<Vector3> m_PositionCallback;

        private TargetingType TargetingType;

        protected override void DoSetup()
        {
            OverlayPrefab = Resources.Load<GameObject>("Prefabs/TargetingOverlay");
            
            GroundAndPlayer = LayerMask.GetMask("Ground", "PlayerHitbox");
            
            m_OverlayInstance = Instantiate(OverlayPrefab, transform);
         
            m_OverlayInstance.SetActive(false);
        }

        private void Update()
        {
            if (!Activated) return;
            
            if (!Text) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            switch (TargetingType)
            {
                case TargetingType.Character:
                    TryGetCharacter(ray);
                    break;
                
                case TargetingType.Mouse:
                    if (!Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground"))) return;
                    
                    Text.text = hit.point.ToString();

                    if (!Input.GetMouseButtonUp(0)) return;
                    
                    m_PositionCallback.Invoke(hit.point);

                    m_OverlayInstance.SetActive(false);

                    Activated = false;

                    m_PositionCallback = null;
                    break;
            }
        }

        private void TryGetCharacter(Ray ray)
        {
            if (!Physics.Raycast(ray, out RaycastHit hit, 100f, GroundAndPlayer)) return;

            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("PlayerHitbox")) return;

            Text.text = hit.collider.transform.root.name;

            if (!Input.GetMouseButtonUp(0)) return;

            m_CharacterCallback.Invoke(hit.collider.transform.root.gameObject);

            m_OverlayInstance.SetActive(false);

            Activated = false;

            m_CharacterCallback = null;
        }

        public void Initialize(Action<GameObject> Callback)
        {
            m_OverlayInstance.SetActive(true);

            m_CharacterCallback = Callback;
            
            Text = GetComponentInChildren<TextMeshProUGUI>();

            Activated = true;

            TargetingType = TargetingType.Character;
        }
        
        public void Initialize(Action<Vector3> Callback)
        {
            m_OverlayInstance.SetActive(true);

            m_PositionCallback = Callback;
            
            Text = GetComponentInChildren<TextMeshProUGUI>();

            Activated = true;

            TargetingType = TargetingType.Mouse;
        }
    }

    public enum TargetingType
    {
        Mouse,
        Character,
    }
}