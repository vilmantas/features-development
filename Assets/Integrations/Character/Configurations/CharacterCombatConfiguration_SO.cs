using Features.Actions;
using Features.Conditions;
using UnityEngine;

namespace Features.Character.Configurations
{
    [CreateAssetMenu(fileName = "RENAME ME", menuName = "Configurations/Character Combat Configuration", order = 0)]
    public class CharacterCombatConfiguration_SO : ScriptableObject
    {
        public ActionGroup_SO StrikeInterruptingActions;

        public StatusEffectGroup_SO StrikeInterruptingSEs;
    }
}