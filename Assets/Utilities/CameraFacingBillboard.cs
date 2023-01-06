using UnityEngine;

namespace Utilities
{
    public class CameraFacingBillboard : MonoBehaviour
    {
        private Camera m_Camera;

        private void Start()
        {
            m_Camera = Camera.main;
        }

        void LateUpdate()
        {
            var rotation = m_Camera.transform.rotation;

            transform.LookAt(transform.position + rotation * Vector3.forward,
                rotation * Vector3.up);
        }
    }
}