using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Actions
{
    public class ActionsController : MonoBehaviour
    {
        public Action<ActionActivation> OnBeforeAction;
        public Action<ActionActivation, ActionActivationResult> OnAfterAction;

        public ActionActivationResult DoAction(ActionActivationPayload payload)
        {
            var activation = ActionActivationHelper.GetActivation(payload);

            OnBeforeAction?.Invoke(activation);

            if (activation.PreventDefault) return ActionActivationResult.PreventedActivation;

            var result = activation.Activate();

            OnAfterAction?.Invoke(activation, result);

            return result;
        }
    }
}