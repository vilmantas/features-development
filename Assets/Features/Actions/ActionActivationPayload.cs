using UnityEngine;

namespace Features.Actions
{
    public class ActionActivationPayload
    {
        public readonly ActionBase Action;

        public readonly object Source;

        public readonly GameObject Target;

        public ActionActivationPayload(ActionBase action, object source, GameObject target)
        {
            Action = action;
            Source = source;
            Target = target;
        }
    }
}