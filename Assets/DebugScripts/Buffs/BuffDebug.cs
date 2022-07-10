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
            BuffController.Add(new BuffBase("Simple", 5f), gameObject);
        }

        public void GiveTacker()
        {
            BuffController.Add(new BuffBase("Stackable", 5f, 5), gameObject);
        }

        public void GiveTicking()
        {
            var interval = new BuffBase("Intervaling", 5f).WithInterval(0.5f);

            BuffController.Add(interval, gameObject);
        }
    }
}