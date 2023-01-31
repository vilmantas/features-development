using System;
using System.Collections.Generic;
using Features.Actions;
using Features.Movement;
using UnityEngine;

namespace Integrations.StatusEffects
{
    public static class StatusEffectPresets
    {
        private static readonly Dictionary<string, Delegate> Handlers = new();

        public static void PreventAllActions(ActionsController actionsController, string condition)
        {
            var dictionaryKey = GetName(actionsController) + condition;

            if (Handlers.ContainsKey(dictionaryKey)) return;
            
            Action<ActionActivation> handler = BlockAllActions;
            
            Handlers.Add(GetName(actionsController) + condition, handler);

            actionsController.OnBeforeAction += handler;
        }
        
        public static void PreventMovement(ActionsController actionsController, string condition)
        {
            var dictionaryKey = GetName(actionsController) + condition;

            if (Handlers.ContainsKey(dictionaryKey)) return;
            
            Action<ActionActivation> handler = BlockMovementAction;
            
            Handlers.Add(GetName(actionsController) + condition, handler);

            actionsController.OnBeforeAction += handler;
        }

        public static void DisableActivity(ActionsController actionsController, string condition)
        {
            Action<ActionActivation> handler = BlockCharacterActions;

            var dictionaryKey = GetName(actionsController) + condition;

            if (Handlers.ContainsKey(dictionaryKey)) return;
            
            Handlers.Add(GetName(actionsController) + condition, handler);

            actionsController.OnBeforeAction += handler;
        }
        
        public static void RemoveConditionHandler(ActionsController actionsController, string condition)
        {
            var dictionaryKey = actionsController.transform.root.name + condition;

            if (!Handlers.ContainsKey(dictionaryKey)) return;
            
            var handler = (Action<ActionActivation>)Handlers[dictionaryKey];

            Handlers.Remove(GetName(actionsController) + condition);
            
            actionsController.OnBeforeAction -= handler;
        }

        private static void BlockCharacterActions(ActionActivation obj)
        {
            if (obj.Payload.IsPassive) return;

            var actionsController = obj.Payload.Source.GetComponentInChildren<ActionsController>();
            
            if (obj.Payload.Source == actionsController.transform.root.gameObject)
            {
                obj.PreventDefault = true;
            }
        }

        private static void BlockAllActions(ActionActivation obj)
        {
            obj.PreventDefault = true;
        }

        private static void BlockMovementAction(ActionActivation obj)
        {
            if (obj.Payload.Action.Name == "Move")
            {
                obj.PreventDefault = true;
            }
        }
        
        private static string GetName(ActionsController controller) =>
            controller.transform.root.name;
    }
}