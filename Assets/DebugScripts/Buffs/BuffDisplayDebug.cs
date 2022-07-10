using Features.Buffs;
using Features.Buffs.Utilities;
using UnityEngine;

namespace DebugScripts.Buffs
{
    public class BuffDisplayDebug : MonoBehaviour
    {
        public GameObject UIContainer;
        public BuffController BuffController;

        public SimpleUIController UIPrefab;

        private void Start()
        {
            BuffController.WithUI(UIPrefab, UIContainer.transform);
        }
    }
}