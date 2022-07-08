using Features.Buffs;
using Features.Buffs.UI;
using UnityEngine;

namespace DebugScripts.Buffs
{
    public class BuffDisplayDebug : MonoBehaviour
    {
        public BuffController BuffController;

        public SimpleUIController UIPrefab;

        public GameObject UIContainer;

        private void Start()
        {
            BuffController.WithUI(UIPrefab, UIContainer.transform);
        }
    }
}