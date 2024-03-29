using UnityEngine;

namespace Features.Buffs
{
    public class BuffAddOptions
    {
        public BuffAddOptions(BuffMetadata buff, GameObject source, int stacks)
        {
            Buff = buff;
            Source = source;
            Stacks = stacks;
        }

        public BuffMetadata Buff { get; set; }
        public GameObject Source { get; set; }
        public int Stacks { get; set; }
        public float OverrideDuration { get; set; }
        public bool RequestHandled { get; set; }
    }
}