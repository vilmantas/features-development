using UnityEngine;

namespace Features.Actions
{
    [CreateAssetMenu(fileName = "Empty Action", menuName = "Actions/New Action", order = 0)]
    public class Action_SO : ScriptableObject
    {
        public string Name;

        public string Alias;

        public ActionBase Base
        {
            get => string.IsNullOrEmpty(Alias) ? new ActionBase(Name) : new ActionBase(Name, Alias);
        }
    }
}