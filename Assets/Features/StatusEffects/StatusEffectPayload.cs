using UnityEngine;

namespace Features.Conditions
{
    public class StatusEffectPayload
    {
        public GameObject Target;

        public StatusEffectPayload(GameObject target)
        {
            Target = target;
        }
    }
}