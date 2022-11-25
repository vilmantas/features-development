using Features.Buffs;
using UnityEngine;

namespace DebugScripts.Buffs
{
    public class BuffDebugHelper : MonoBehaviour
    {
        private BuffController BuffController;

        private void Start()
        {
            BuffController = GetComponentInChildren<BuffController>();

            BuffController.OnBuffTickOccurred += OnBuffTickOccurred;
        }

        private void OnBuffTickOccurred(ActiveBuff obj)
        {
            Debug.Log(obj.Metadata.Name + " Tick");
        }

        public void GiveTicker()
        {
            BuffController.Add(new BuffAddOptions(new BuffMetadata("Simple", 5f), gameObject, 1) {Stacks = 1});
        }

        public void GiveTacker()
        {
            BuffController.Add(new(new BuffMetadata("Stackable", 5f, 5), gameObject, 1) {Stacks = 1});
        }

        public void GiveTicking()
        {
            var buffInterval = new BuffMetadata("Intervaling", 5f).WithInterval(0.5f);

            BuffController.Add(new(buffInterval, gameObject, 1) {Stacks = 1});
        }
    }
}