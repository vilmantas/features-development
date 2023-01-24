using Managers;
using UnityEngine;

namespace Features.DestinationPointer
{
    public class DestinationPointerManager : SingletonManager<DestinationPointerManager>
    {
        private PointerController m_PointerControllerInstance;
        private PointerController m_PointerControllerPrefab;

        protected override void DoSetup()
        {
            m_PointerControllerPrefab = Resources.Load<PointerController>("pointer");

            m_PointerControllerInstance = Instantiate(m_PointerControllerPrefab, transform);
        }

        public static void Show(Vector3 pos, float duration)
        {
            Instance.m_PointerControllerInstance.Display(pos, duration);
        }
    }
}