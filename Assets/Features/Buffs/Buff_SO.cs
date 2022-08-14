using UnityEngine;

namespace Features.Buffs
{
    [CreateAssetMenu(menuName = "Buffs/Buff", fileName = "RENAME_ME")]
    public class Buff_SO : ScriptableObject
    {
        public string Name;

        [Min(0.1f)] public float Duration;

        [Min(1)] public int MaxStacks;

        [Min(0.1f)] public float Interval;

        public bool ExecuteTickImmediately;

        public Sprite Sprite;

        public BuffBase Base
        {
            get => new BuffBase(Name, Duration, Sprite, MaxStacks);
        }

        public BuffBase WithTicking
        {
            get => Base.WithInterval(Interval, ExecuteTickImmediately);
        }

        public BuffImplementation Implementation
        {
            get
            {
                BuffImplementationRegistry.Implementations.TryGetValue(Name, out var impl);

                return impl;
            }
        }
    }
}