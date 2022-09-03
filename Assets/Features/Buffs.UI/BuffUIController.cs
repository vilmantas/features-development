using UnityEngine;

namespace Features.Buffs.UI
{
    public class BuffUIController : MonoBehaviour
    {
        public BaseBuffUIData Prefab;

        private BuffController m_BuffController;

        private BuffUIManager m_UIManager;

        public void Initialize(BuffController buffController)
        {
            m_BuffController = buffController;

            m_UIManager = new BuffUIManager();

            m_UIManager.SetSource(m_BuffController, CreateMethod, DestroyMethod);
        }

        private IBuffUIData CreateMethod()
        {
            var instance = Instantiate(Prefab.gameObject, transform);

            return instance.GetComponentInChildren<IBuffUIData>();
        }

        private void DestroyMethod(IBuffUIData data)
        {
            Destroy(data.gameObject);
        }
    }
}