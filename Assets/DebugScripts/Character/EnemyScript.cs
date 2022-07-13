using Features.Buffs;
using UnityEngine;

namespace DebugScripts.Character
{
    public class EnemyScript : MonoBehaviour
    {
        public GameObject Target;

        public Buff_SO Buff;

        private BuffBase m_BuffBase;

        private void Start()
        {
            var i = new BuffImplementation("Lifter", OnReceive, OnRemove, null, null);

            BuffImplementationRegistry.Implementations.TryAdd(i.Name, i);

            m_BuffBase = Buff.GetBase();
        }

        public void CastDebuff()
        {
            var controller = Target.GetComponentInChildren<BuffController>();

            if (controller == null) return;

            controller.AttemptAdd(m_BuffBase, gameObject);
        }

        public void OnReceive(BuffActivationPayload payload)
        {
            payload.Target.transform.Translate(Vector3.up * 5);
        }

        public void OnRemove(BuffActivationPayload payload)
        {
            payload.Target.transform.Translate(Vector3.down * 5);
        }
    }
}