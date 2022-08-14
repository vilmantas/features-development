using Features.Buffs;
using Features.Buffs.UI;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class PlayerBuffsUIDisplay : Manager
    {
        public BaseBuffUIData Prefab;

        private BuffController m_BuffController;

        private GameObject m_Character;

        private BuffUIManager m_UIManager;

        public override void Initialize()
        {
            m_Character = GameObject.FindGameObjectWithTag("Player");

            m_BuffController = m_Character.GetComponentInChildren<BuffController>();

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