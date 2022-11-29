using System.Collections.Generic;
using UnityEngine;

namespace Features.Actions
{
    public class ActionActivationPayload
    {
        public readonly ActionBase Action;

        public readonly GameObject Source;

        public readonly GameObject Target;

        public readonly Dictionary<string, object> Data;

        public ActionActivationPayload(ActionBase action, GameObject source, GameObject target, Dictionary<string, object> data = null)
        {
            Action = action;
            Source = source;
            Target = target;
            Data = data ?? new Dictionary<string, object>();
        }
    }
}