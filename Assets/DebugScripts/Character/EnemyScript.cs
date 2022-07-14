using System;
using Features.Buffs;
using Features.Health;
using UnityEngine;

namespace DebugScripts.Character
{
    public class EnemyScript : MonoBehaviour
    {
        public GameObject Target;

        public Buff_SO ShifterBuff;

        public Buff_SO LifterBuff;

        public Buff_SO Reducer;

        private Action<int> x;

        private Func<int, int> fun;

        private void Start()
        {
            x?.Invoke(5);

            x += MethodA;

            x?.Invoke(10);

            x -= MethodA;

            x?.Invoke(20);

            x -= MethodA;

            x -= MethodA;

            x?.Invoke(30);

            x += MethodA;

            x += MethodA;

            x?.Invoke(40);

            x -= MethodA;

            x?.Invoke(50);

            Debug.Log(x == null);

            Debug.Log("------");

            fun += MethodB;

            fun += x => x / 2;

            foreach (Func<int, int> ff in fun.GetInvocationList())
            {
                Debug.Log(ff(5));
            }

        }

        public void MethodA(int a)
        {
            Debug.Log(a * 4);
        }

        public int MethodB(int a)
        {
            return a * a;
        }

        public void CastLifter()
        {
            var controller = Target.GetComponentInChildren<BuffController>();

            if (controller == null) return;

            controller.AttemptAdd(LifterBuff.Base, gameObject);
        }

        public void CastShifter()
        {
            var controller = Target.GetComponentInChildren<BuffController>();

            if (controller == null) return;

            controller.AttemptAdd(ShifterBuff.Base, gameObject);
        }

        public void CastHealingReducer()
        {
            var controller = Target.GetComponentInChildren<BuffController>();

            if (controller == null) return;

            controller.AttemptAdd(Reducer.Base, gameObject);
        }

        public void AttemptHeal()
        {
            var controller = Target.GetComponentInChildren<HealthController>();

            controller.AttemptHealing(10);
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