using System;
using UnityEngine;

namespace Features.Actions
{
    public class ActionsController : MonoBehaviour
    {
        public Action<ActionActivation> OnActionActivated;
        public Action<ActionActivation> OnBeforeActionActivation;

        public void DoAction(ActionBase action, object source)
        {
            var payload = new ActionActivationPayload(action, source, transform.root.gameObject);

            var activation = ActionActivationHelper.GetActivation(payload);

            OnBeforeActionActivation?.Invoke(activation);

            activation.Activate();

            OnActionActivated?.Invoke(activation);
        }
    }
}