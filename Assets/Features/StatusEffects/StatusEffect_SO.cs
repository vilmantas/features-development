using UnityEngine;

namespace Features.Conditions
{
    [CreateAssetMenu(fileName = "RENAME ME", menuName = "Status Effects/Status Effect", order = 0)]
    public class StatusEffect_SO : ScriptableObject
    {
        public string InternalName;

        public string DisplayName;
    }
}