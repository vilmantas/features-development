using UnityEngine;

namespace Features.Character.Configurations
{
    [CreateAssetMenu(fileName = "RENAME ME", menuName = "Configurations/System Configuration", order = 0)]
    public class RPGSystemConfiguration_SO : ScriptableObject
    {
        public CharacterCombatConfiguration_SO CombatConfiguration;

        public CharacterBuffConfiguration_SO BuffsConfiguration;

        public CharacterStatusEffectConfiguration_SO StatusEffectConfiguration;
    }
}