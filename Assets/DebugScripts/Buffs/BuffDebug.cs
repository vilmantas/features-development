using Features.Buffs;
using UnityEngine;

namespace DebugScripts.Buffs
{
    public class BuffDebug : MonoBehaviour
    {
        private BuffController BuffController;

        private void Start()
        {
            BuffController = GetComponentInChildren<BuffController>();
        }

        public void GiveTicker()
        {
            BuffController.Receive(new BuffBase("Simple", 5f));
        }

        public void GiveTacker()
        {
            BuffController.Receive(new BuffBase("Stackable", 5f, 5));
        }

        public void GiveTicking()
        {
            var interval = new BuffBase("Intervaling", 5f).WithInterval(0.5f, () => Debug.Log("Tick!"));

            BuffController.Receive(interval);
        }
    }
}