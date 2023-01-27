using UnityEngine;

namespace Features.Buffs
{
    [CreateAssetMenu(fileName = "RENAME ME", menuName = "Buffs/Buff Group", order = 0)]
    public class BuffGroup_SO : ScriptableObject
    {
        public Buff_SO[] Buffs;
    }
}