using Features.Conditions;
using UnityEngine;

namespace Features.Character.Configurations
{
    [CreateAssetMenu(fileName = "RENAME ME", menuName = "Configurations/Character Status Effect Configuration", order = 0)]
    public class CharacterStatusEffectConfiguration_SO : ScriptableObject
    {
        public StatusEffectGroup_SO Disables;
    }
}