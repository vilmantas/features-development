using System;
using _SampleGames.Survivr.SurvivrFeatures.Combat;
using Features.Actions;
using Features.Combat;
using Features.Health;
using Features.Stats.Base;
using UnityEngine;

namespace _SampleGames.Survivr.SurvivrFeatures.Actions
{
    public static class Defend
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(Defend), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            var defendActionPayload = payload as DefendActionPayload;

            var attackData = defendActionPayload.Attack;

            var actualDamage = attackData.Damage;

            var targetStats = payload.Target.GetComponentInChildren<StatsController>();

            if (targetStats)
            {
                actualDamage -= targetStats.CurrentStats["Defence"].Value;
            }

            actualDamage = Math.Max(actualDamage, 1);

            var damagePayload = new ActionActivationPayload(new(nameof(Damage)), attackData.Source, payload.Target);

            var actionsController = payload.Target.GetComponentInChildren<ActionsController>();
            
            actionsController.DoAction(new DamageActionPayload(damagePayload, actualDamage));
            
            defendActionPayload.Callback.Invoke(new AttackResult(payload.Target, attackData, new HitData(actualDamage)));
        }
    }

    public class DefendActionPayload : ActionActivationPayload
    {
        public AttackData Attack { get; }
        public Action<AttackResult> Callback { get; }

        public DefendActionPayload(
            ActionActivationPayload original, 
            AttackData attack,
            Action<AttackResult> callback
            ) : base(original.Action,
            original.Source, original.Target)
        {
            Attack = attack;
            Callback = callback;
        }
    }
}