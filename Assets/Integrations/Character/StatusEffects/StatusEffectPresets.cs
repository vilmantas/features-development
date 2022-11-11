using System;
using System.Collections.Generic;
using Features.Actions;
using Features.Character;
using Features.Combat;
using UnityEngine;

namespace Integrations.Character.StatusEffects
{
    public static class StatusEffectPresets
    {
        private static Dictionary<string, Delegate> Handlers = new Dictionary<string, Delegate>();

        public static void DisableActivity(ActionsController actionsController, string condition)
        {
            Action<ActionActivation> handler = payload => BlockAction(payload, actionsController); 
            
            Handlers.Add(GetName(actionsController) + condition, handler);

            actionsController.OnBeforeAction += handler;
        }
        
        public static void EnableActivity(ActionsController actionsController, string condition)
        {
            var handler = (Action<ActionActivation>)Handlers[actionsController.transform.root.name + condition];

            Handlers.Remove(GetName(actionsController) + condition);
            
            actionsController.OnBeforeAction -= handler;
        }

        private static void BlockAction(ActionActivation obj, ActionsController actionsController)
        {
            if (obj.Payload.Data?["passive"] is true) return;
            
            if (obj.Payload.Source == actionsController.transform.root.gameObject)
            {
                obj.PreventDefault = true;
            }
        }

        private static string GetName(ActionsController controller) =>
            controller.transform.root.name;
    }
}