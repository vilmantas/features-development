using UnityEngine;

namespace Features.Buffs
{
    [CreateAssetMenu(menuName = "Buffs/Buff", fileName = "RENAME_ME")]
    public class Buff_SO : ScriptableObject
    {
        public string Name;

        [Min(0.1f)] public float Duration;

        [Min(1)] public int MaxStacks;

        public Sprite Sprite;

        public BuffBase GetBase() => new BuffBase(Name, Duration, Sprite, MaxStacks);
    }
}