using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private readonly List<string> PreparedSkills = new();
        
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

        private bool RequiresTarget(SkillActivationContext obj)
        {
            if (obj.Metadata.Target == SkillTarget.None) return false;

            switch (obj.Metadata.Target)
            {
                case SkillTarget.Character:
                    if (!obj.TargetObject)
                    {
                        m_TargetProvider.GetCharacterTarget(x =>
                        {
                            obj.TargetObject = x;

                            ContinueWithTarget(obj);
                        });

                        return true;
                    }

                    break;

                case SkillTarget.Pointer:
                    if (obj.TargetLocation == Vector3.zero)
                    {
                        m_TargetProvider.PickMousePosition(x =>
                        {
                            obj.TargetLocation = x;

                            ContinueWithTarget(obj);
                        });

                        return true;
                    }

                    break;
                case SkillTarget.CharacterLocation:
                    if (!obj.TargetObject)
                    {
                        m_TargetProvider.GetCharacterTarget(x =>
                        {
                            obj.TargetLocation = x.transform.position;

                            ContinueWithTarget(obj);
                        });

                        return true;
                    }
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

            if (IsSkillPrepared(obj))
            {
                PreparedSkills.Remove(obj.Skill);

                return false;
            }

            var command = new ChannelingCommand(GetChannelingTag(obj),
                obj.Metadata.ChannelingTime).WithCallback(() => ContinueActivation(obj));
            
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
            return PreparedSkills.Any(x => x.Equals(context.Skill));
        }

        private void ContinueActivation(SkillActivationContext obj)
        {
            PreparedSkills.Add(obj.Skill);

            obj.PreventDefault = false;
            
            m_SkillsController.ActivateSkill(obj);
        }

        private void ContinueWithTarget(SkillActivationContext obj)
        {
            obj.PreventDefault = false;
            
            m_SkillsController.ActivateSkill(obj);
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
    }
}