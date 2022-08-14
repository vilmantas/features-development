using UnityEngine;

namespace Features.Buffs
{
    public class BuffAddOptions
    {
        public BuffAddOptions()
        {
        }

        public BuffAddOptions(BuffBase buff, GameObject source)
        {
            Buff = buff;
            Source = source;
        }

        public BuffAddOptions(BuffBase buff, GameObject source, int stacks)
        {
            Buff = buff;
            Source = source;
            Stacks = stacks;
        }

        public BuffBase Buff { get; set; }
        public GameObject Source { get; set; }
        public int Stacks { get; set; }
        public float Duration { get; set; }
        public bool RequestHandled { get; set; }
    }
}