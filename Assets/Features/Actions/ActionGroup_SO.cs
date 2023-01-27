using UnityEngine;

namespace Features.Actions
{
    [CreateAssetMenu(fileName = "RENAME ME", menuName = "Actions/Action Group", order = 0)]
    public class ActionGroup_SO : ScriptableObject
    {
        public Action_SO[] Actions;
    }
}