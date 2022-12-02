using Features.Combat;
using Features.Movement;
using Features.Skills;
using UnityEngine;

namespace Integrations.Skills
{
    public static class BasicAttackSkill
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            SkillImplementation implementation = new(OnReceive, OnActivation, OnRemove);
            SkillImplementationRegistry.Register(nameof(BasicAttackSkill), implementation);
        }
        
        private static void OnActivation(SkillActivationContext context)
        {
            Debug.Log("Doing basic attack");
            
            var movementController = context.Source.GetComponentInChildren<MovementController>();
            
            movementController.Stop();
            
            var combatController = context.Source.GetComponentInChildren<CombatController>();

            combatController.Strike();
        }

        private static void OnReceive(SkillActivationContext obj)
        {
            Debug.Log(obj.Source.name + " Was given BasicAttack");
        }

        private static void OnRemove(SkillActivationContext obj)
        {
            Debug.Log(obj.Source.name + " Lost BasicAttack!");
        }
    }
}