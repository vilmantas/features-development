using Features.Buffs;
using Features.Buffs.UI;
using UnityEngine;

namespace DebugScripts.Buffs
{
    public class BuffDebugUIContainerController : MonoBehaviour
    {
        public BuffDebugCharacter Character;

        public BaseBuffUIData UIPrefab;

        private BuffUIManager m_UIManager;

        private void Start()
        {
            m_UIManager = new BuffUIManager();

            m_UIManager.SetSource(Character.GetComponentInChildren<BuffController>(),
                () =>
                {
                    var instance = Instantiate(UIPrefab.gameObject, transform);
                    return instance.GetComponentInChildren<IBuffUIData>();
                },
                controller => DestroyImmediate(controller.gameObject));
        }
    }
}