using Features.Actions;
using Features.Character;
using Features.Combat;
using UnityEngine;

namespace Integrations.StatusEffects
{
    public static class StatusEffectPresets
    {
        public static void PreventCharacterActivity(Modules.Character character)
        {
            character.m_CombatController.OnBeforeStrike += BlockCombat;
            character.m_CombatController.OnBeforeBlock += BlockCombat;
        }

        private static void BlockCombat(CombatActionPayload obj)
        {
            obj.PreventDefault = true;
        }
    }
}