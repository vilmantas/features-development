using System;
using System.Collections.Generic;
using Features.Actions;
using Features.Cooldowns;
using Features.Equipment;
using Features.Health;
using Features.Health.Events;
using Features.Movement;
using Features.Skills;
using Features.Targeting;
using Integrations.Actions;
using Integrations.Items;
using UnityEngine;

namespace Features.Character
{
    public class CharacterSkillsManager : MonoBehaviour
    {
        private GameObject Root;

        private EquipmentController m_EquipmentController;

        private SkillsController m_SkillsController;

        private CooldownsController m_CooldownsController;

        private ChannelingController m_ChannelingController;
        
        private ActionsController m_ActionsController;

        private TargetProvider m_TargetProvider;

        private HealthController m_HealthController;
        
        private MovementController m_MovementController;
        
        private readonly Dictionary<string, Delegate> RunningHandlers = new ();

        private void Awake()
        {
            Root = transform.root.gameObject;

            m_ActionsController = Root.GetComponentInChildren<ActionsController>();

            m_HealthController = Root.GetComponentInChildren<HealthController>();
            
            m_MovementController = Root.GetComponentInChildren<MovementController>();
            
            m_EquipmentController = Root.GetComponentInChildren<EquipmentController>();

            m_SkillsController = Root.GetComponentInChildren<SkillsController>();

            m_ChannelingController = Root.GetComponentInChildren<ChannelingController>();
            
            m_ChannelingController.OnChannelingStarted += OnChannelingStarted;
            
            m_ChannelingController.OnChannelingCompleted += OnChannelingCompleted;

            m_CooldownsController = Root.GetComponentInChildren<CooldownsController>();

            m_TargetProvider = Root.GetComponentInChildren<TargetProvider>();
            
            m_SkillsController.OnBeforeActivation += OnBeforeActivation;
            
            if (!m_EquipmentController) return;
            
            m_EquipmentController.OnItemEquipped += EquipmentChanged;
            
            m_EquipmentController.OnItemUnequipped += EquipmentChanged;
        }
        
        static string GetHandlerTag(SkillActivationContext ctx, SkillFlags flag) => ctx.Skill + "_" +flag;

        private void OnChannelingCompleted(ChannelingItem obj)
        {
            if (!obj.Title.StartsWith("skill_")) return;

            if (!obj.Data.TryGetValue("skill", out var contextRaw)) return;

            if (contextRaw is not SkillActivationContext context) return;
            
            if (context.Metadata.HasFlag(SkillFlags.PreventMovement))
            {
                var handlerTag = GetHandlerTag(context, SkillFlags.PreventMovement);
                
                if (RunningHandlers.Remove(handlerTag, out var handler))
                {
                    m_ActionsController.OnBeforeAction -= handler as Action<ActionActivation>;
                }
            }
            if (context.Metadata.HasFlag(SkillFlags.InterruptableByDamage))
            {
                var handlerTag = GetHandlerTag(context, SkillFlags.InterruptableByDamage);
                
                if (RunningHandlers.Remove(handlerTag, out var handler))
                {
                    m_ActionsController.OnBeforeAction -= handler as Action<ActionActivation>;
                }
            }
            if (context.Metadata.HasFlag(SkillFlags.InterruptableByMovement))
            {
                var handlerTag = GetHandlerTag(context, SkillFlags.InterruptableByMovement);
                
                if (RunningHandlers.Remove(handlerTag, out var handler))
                {
                    m_ActionsController.OnBeforeAction -= handler as Action<ActionActivation>;
                }
            }
        }

        private void BlockMovement(ActionActivation obj)
        {
            if (obj.Payload.Action.Name != nameof(Move)) return;
            
            obj.PreventDefault = true;
        }

        private void StopChannelingOnMovement(ActionActivation obj, SkillActivationContext ctx)
        {
            if (obj.Payload.Action.Name != nameof(Move)) return;
            
            if (obj.PreventDefault) return;
                
            m_ChannelingController.InterruptChanneling(GetChannelingTag(ctx));
        }
        
        private void StopChannelingOnDamage(SkillActivationContext ctx)
        {
            m_ChannelingController.InterruptChanneling(GetChannelingTag(ctx));
        }
        
        private void OnChannelingStarted(ChannelingItem obj)
        {
            if (!obj.Title.StartsWith("skill_")) return;

            if (!obj.Data.TryGetValue("skill", out var contextRaw)) return;

            if (contextRaw is not SkillActivationContext context) return;

            List<Action<SkillActivationContext>> actions = new List<Action<SkillActivationContext>>();
            
            if (context.Metadata.HasFlag(SkillFlags.PreventMovement))
            {
                Action<SkillActivationContext> adel = EnableMovementPrevent;
                actions.Add(adel);
            }
            if (context.Metadata.HasFlag(SkillFlags.InterruptableByDamage))
            {
                Action<SkillActivationContext> adel = EnableDamageInterrupt;
                actions.Add(adel);
            }
            if (context.Metadata.HasFlag(SkillFlags.InterruptableByMovement))
            {
                Action<SkillActivationContext> adel = EnableMovementInterrupt;
                actions.Add(adel);
            }

            foreach (var action in actions)
            {
                action.Invoke(context);
            }
        }

        private void EnableDamageInterrupt(SkillActivationContext context)
        {
            var handlerTag = GetHandlerTag(context, SkillFlags.InterruptableByDamage);

            Action<HealthChangeEventArgs> handler = x => StopChannelingOnDamage(context);

            m_HealthController.OnDamage += handler;

            RunningHandlers.Add(handlerTag, handler);
        }

        private void EnableMovementPrevent(SkillActivationContext context)
        {
            m_MovementController.Stop();

            var handlerTag = GetHandlerTag(context, SkillFlags.PreventMovement);

            Action<ActionActivation> handler = BlockMovement;

            m_ActionsController.OnBeforeAction += handler;

            RunningHandlers.Add(handlerTag, handler);
        }
        
        private void EnableMovementInterrupt(SkillActivationContext context)
        {
            m_MovementController.Stop();
            
            var handlerTag = GetHandlerTag(context, SkillFlags.InterruptableByMovement);

            Action<ActionActivation> handler = act =>  StopChannelingOnMovement(act, context);

            m_ActionsController.OnBeforeAction += handler;

            RunningHandlers.Add(handlerTag, handler);
        }

        private void OnBeforeActivation(SkillActivationContext obj)
        {
            if (IsSkillOnCooldown(obj))
            {
                obj.PreventDefault = true;
                
                return;
            }

            if (RequiresTarget(obj))
            {
                obj.PreventDefault = true;
                
                return;
            }

            if (RequiresChanneling(obj))
            {
                obj.PreventDefault = true;
            }
        }

        private bool IsSkillWithTarget(SkillActivationContext context)
        {
            return context is ContinuedSkillActivation cont && cont.IsOfType<TargetedSkillActivationContext>();
        }
        
        private bool RequiresTarget(SkillActivationContext obj)
        {
            if (obj.Metadata.Target == SkillTarget.None) return false;

            if (IsSkillWithTarget(obj)) return false;

            switch (obj.Metadata.Target)
            {
                case SkillTarget.Character:
                    m_TargetProvider.GetCharacterTarget(x =>
                    {
                        ContinueWithTargetObject(obj, x);
                    });

                    return true;

                    break;

                case SkillTarget.Pointer:
                    m_TargetProvider.PickMousePosition(x =>
                    {
                        ContinueWithTargetPosition(obj, x);
                    });

                    return true;

                    break;
                case SkillTarget.CharacterLocation:
                    m_TargetProvider.GetCharacterTarget(x =>
                    {
                        ContinueWithTargetPosition(obj, x.transform.position);
                    });

                        return true;
                    break;
                case SkillTarget.None:
                    break;
                case SkillTarget.Self:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }

        private bool RequiresChanneling(SkillActivationContext obj)
        {
            if (!obj.Metadata.ChanneledSkill) return false;

            if (IsSkillPrepared(obj)) return false;
            
            var command = new ChannelingCommand(GetChannelingTag(obj),
                    obj.Metadata.ChannelingTime)
                .WithCallback(result => ContinueActivation(obj, result));
            
            command.Data.Add("skill", obj);

            m_ChannelingController.StartChanneling(command);

            return true;
        }

        private static string GetChannelingTag(SkillActivationContext obj)
        {
            return "skill_" + obj.Metadata.ReferenceName;
        }

        private bool IsSkillOnCooldown(SkillActivationContext obj)
        {
            return m_CooldownsController.IsOnCooldown(obj.Skill);
        }

        private bool IsSkillPrepared(SkillActivationContext context)
        {
            return context is ContinuedSkillActivation cont && cont.IsOfType<ChanneledSkillActivationContext>();
        }

        private void ContinueActivation(SkillActivationContext context, ChannelingItem result)
        {
            if (result.IsInterrupted &&
                !context.Metadata.HasFlag(SkillFlags.AllowPartialChannelingCompletion)) return;

            var channeledSkill = new ChanneledSkillActivationContext(context, result);
            
            m_SkillsController.ActivateSkill(channeledSkill);
        }

        private void ContinueWithTargetObject(SkillActivationContext context, GameObject targetObj)
        {
            var x = new TargetedSkillActivationContext(context);

            x.SetTargetObject(targetObj);
            
            m_SkillsController.ActivateSkill(x);
        }
        
        private void ContinueWithTargetPosition(SkillActivationContext context, Vector3 targetLoc)
        {
            var x = new TargetedSkillActivationContext(context);
            
            x.SetTargetLocation(targetLoc);

            m_SkillsController.ActivateSkill(x);
        }

        private void EquipmentChanged(EquipResult obj)
        {
            if (!obj.Succeeded) return;

            if (obj.UnequippedItem is ItemInstance unequippedItem)
            {
                foreach (var metadataSkill in unequippedItem.Metadata.Skills)
                {
                    m_SkillsController.Remove(metadataSkill.ReferenceName);
                }
            }

            if (obj.EquippedItem is ItemInstance item)
            {
                foreach (var metadataSkill in item.Metadata.Skills)
                {
                    m_SkillsController.Add(metadataSkill);
                }
            }
        }

        public class TargetedSkillActivationContext : ContinuedSkillActivation
        {
            public void SetTargetObject(GameObject obj) => TargetObject = obj;

            public void SetTargetLocation(Vector3 loc) => TargetLocation = loc;

            public TargetedSkillActivationContext(SkillActivationContext ctx) : base(ctx)
            {
            }
        }
        public class ChanneledSkillActivationContext : ContinuedSkillActivation
        {
            public ChannelingItem Result { get; }

            public ChanneledSkillActivationContext(SkillActivationContext ctx, ChannelingItem result) : base(ctx)
            {
                Result = result;
            }
        }

        public class ContinuedSkillActivation : SkillActivationContext
        {
            public ContinuedSkillActivation Prev { get; set; }
            
            public ContinuedSkillActivation(SkillActivationContext ctx) : base(ctx)
            {
                Prev = ctx as ContinuedSkillActivation;
            }

            public bool IsOfType<T>() where T: ContinuedSkillActivation
            {
                if (this is T t) return true;
                
                return Prev != null && Prev.IsOfType<T>();
            }
        }
    }
}