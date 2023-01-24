using System.Collections;
using UnityEngine;

namespace Features.DestinationPointer
{
    public class PointerController : MonoBehaviour
    {
        private MeshRenderer m_Model;

        private float m_TimeRemaining = -1f;

        private void Awake()
        {
            m_Model = GetComponentInChildren<MeshRenderer>();

            m_Model.gameObject.SetActive(false);
        }

        public void Display(Vector3 pos, float duration)
        {
            ShowPointer(pos);

            if (m_TimeRemaining > 0)
            {
                m_TimeRemaining = duration;
            }
            else
            {
                StartCoroutine(DisplayPointer(duration));
            }
        }

        private void ShowPointer(Vector3 pos)
        {
            transform.position = pos;

            m_Model.gameObject.SetActive(true);

            m_Model.gameObject.SetActive(true);
        }

        IEnumerator DisplayPointer(float duration)
        {
            m_TimeRemaining = duration;

            while (m_TimeRemaining > 0)
            {
                yield return new WaitForSeconds(0.1f);

                m_TimeRemaining -= 0.1f;
            }

            m_Model.gameObject.SetActive(false);
        }
    }
}