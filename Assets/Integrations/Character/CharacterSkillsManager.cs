using System;
using System.Collections.Generic;
using System.Linq;
using Features.Actions;
using Features.Cooldowns;
using Features.Equipment;
using Features.Skills;
using Features.Targeting;
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
        
        private List<string> PreparedSkills = new();
        
        private void Awake()
        {
            Root = transform.root.gameObject;

            m_EquipmentController = Root.GetComponentInChildren<EquipmentController>();

            m_SkillsController = Root.GetComponentInChildren<SkillsController>();

            m_ChannelingController = Root.GetComponentInChildren<ChannelingController>();

            m_CooldownsController = Root.GetComponentInChildren<CooldownsController>();

            m_TargetProvider = Root.GetComponentInChildren<TargetProvider>();
            
            m_SkillsController.OnBeforeActivation += OnBeforeActivation;
            
            if (!m_EquipmentController) return;
            
            m_EquipmentController.OnItemEquipped += EquipmentChanged;
            
            m_EquipmentController.OnItemUnequipped += EquipmentChanged;
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

            var command = new ChannelingCommand("skill_" + obj.Metadata.ReferenceName,
                obj.Metadata.CastTime)
            {
                Callback = () => ContinueActivation(obj)
            };

            m_ChannelingController.StartChanneling(command);

            return true;
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