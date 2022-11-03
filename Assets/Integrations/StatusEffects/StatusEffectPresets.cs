using System;
using System.Collections.Generic;
using Features.Actions;
using Features.Character;
using Features.Combat;
using UnityEngine;

namespace Integrations.StatusEffects
{
    public static class StatusEffectPresets
    {
        private static Dictionary<string, Delegate> Handlers = new Dictionary<string, Delegate>();

        public static void DisableActivity(Modules.Character character, string condition)
        {
            Action<ActionActivation> handler = payload => BlockAction(payload, character); 
            
            Handlers.Add(character.name + condition, handler);

            character.m_ActionsController.OnBeforeAction += handler;
        }
        
        public static void EnableActivity(Modules.Character character, string condition)
        {
            var handler = (Action<ActionActivation>)Handlers[character.name + condition];

            Handlers.Remove(character.name + condition);
            
            character.m_ActionsController.OnBeforeAction -= handler;
        }

        private static void BlockAction(ActionActivation obj, Modules.Character character)
        {
            if (obj.Payload.Source == character.gameObject)
            {
                obj.PreventDefault = true;
            }
        }
    }
}