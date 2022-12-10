using System;
using TMPro;
using UnityEngine;

namespace Features.Targeting
{
    public class TargetingManager : MonoBehaviour
    {
        public GameObject OverlayPrefab;

        private GameObject m_OverlayInstance;

        public LayerMask RaycastStuff;

        public TextMeshProUGUI Text;

        private Action<GameObject> m_Callback;

        private void Awake()
        {
            m_OverlayInstance = Instantiate(OverlayPrefab, transform);
         
            m_OverlayInstance.SetActive(false);
        }

        private void Update()
        {
            if (!Text) return;
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit, 100f, RaycastStuff)) return;

            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("PlayerHitbox")) return;
            
            Text.text = hit.collider.transform.root.name;

            if (!Input.GetMouseButtonUp(0)) return;
            
            m_Callback.Invoke(hit.collider.transform.root.gameObject);
            
            m_OverlayInstance.SetActive(false);
        }

        public void Initialize(Action<GameObject> Callback)
        {
            m_OverlayInstance.SetActive(true);

            m_Callback = Callback;
            
            Text = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}