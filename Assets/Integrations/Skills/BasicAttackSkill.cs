using System;
using System.Collections.Concurrent;
using Features.Actions;
using Features.Combat;
using Features.Cooldowns;
using Features.Skills;
using Integrations.Actions;
using UnityEngine;

namespace Integrations.Skills
{
    public static class BasicAttackSkill
    {
        private static readonly ConcurrentDictionary<string, Delegate> RunningHandlers = new();

        private static readonly ConcurrentDictionary<Guid, SkillActivationContext> InitiatedAttacks = new();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SkillImplementation implementation = new(OnActivation)
            {
                OnReceive = OnReceived,
                OnRemove = OnRemoved
            };
            SkillImplementationRegistry.Register(nameof(BasicAttackSkill), implementation);
        }
        
        private static SkillActivationResult OnActivation(SkillActivationContext context)
        {
            var payload = Strike.MakePayload(context.Source);

            var a = context.Source.GetComponentInChildren<ActionsController>();

            var result = a.DoAction(payload);
            
            InitiatedAttacks.TryAdd(payload.StrikeId, context);

            return new SkillActivationResult(result.IsSuccessful.HasValue && result.IsSuccessful.Value);
        }

        private static void OnReceived(SkillActivationContext ctx)
        {
            var ctr = ctx.Source.GetComponentInChildren<CombatController>();

            Action<Guid> del = StrikeCooldownRefund;

            RunningHandlers.TryAdd(ctx.Source.gameObject.name, del);
            
            ctr.OnStrikeCancelled += del;
        }

        private static void OnRemoved(SkillActivationContext ctx)
        {
            if (!RunningHandlers.TryGetValue(ctx.Source.gameObject.name, out var del)) return;
            
            var ctr = ctx.Source.GetComponentInChildren<CombatController>();
            
            ctr.OnStrikeCancelled -= del as Action<Guid>;
        }

        private static void StrikeCooldownRefund(Guid id)
        {
            if (!InitiatedAttacks.TryGetValue(id, out var context)) return;

            var cooldowns = context.Source.GetComponentInChildren<CooldownsController>();

            if (!cooldowns) return;
            
            cooldowns.ReduceCooldown(context.Skill, float.MaxValue);
        }
    }
}