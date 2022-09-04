using UnityEngine;

namespace Features.Stats.Base
{
    public class StatsUIController : MonoBehaviour
    {
        public BaseStatUIData UIPrefab;

        private StatsController m_StatsController;

        private StatsUIManager m_UIManager;

        public void Initialize(StatsController stats)
        {
            m_StatsController = stats;

            m_UIManager = new StatsUIManager();

            m_UIManager.SetSource(m_StatsController, CreateMethod, DestroyMethod);
        }

        private IStatUIData CreateMethod()
        {
            var instance = Instantiate(UIPrefab.gameObject, transform);

            return instance.GetComponentInChildren<IStatUIData>();
        }

        private void DestroyMethod(IStatUIData data)
        {
            Destroy(data.gameObject);
        }
    }
}