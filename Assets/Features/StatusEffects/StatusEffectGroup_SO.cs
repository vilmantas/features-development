using UnityEngine;

namespace Features.Conditions
{
    [CreateAssetMenu(fileName = "RENAME ME", menuName = "Status Effects/Status Effect Group", order = 0)]
    public class StatusEffectGroup_SO : ScriptableObject
    {
        public StatusEffect_SO[] StatusEffects;
    }
}