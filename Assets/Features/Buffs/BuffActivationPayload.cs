using UnityEngine;

namespace Features.Buffs
{
    public class BuffActivationPayload
    {
        public BuffActivationPayload(GameObject source, GameObject target, ActiveBuff buff)
        {
            Source = source;
            Target = target;
            Buff = buff;
        }

        public GameObject Source { get; }

        public GameObject Target { get; }

        public ActiveBuff Buff { get; }
    }
}