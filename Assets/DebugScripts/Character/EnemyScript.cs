using Features.Buffs;
using Features.Character.Buffs;
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
            var lifer = LifterImplementation.Implementation;

            BuffImplementationRegistry.Implementations.TryAdd(lifer.Name, lifer);

            m_BuffBase = Buff.Base;
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