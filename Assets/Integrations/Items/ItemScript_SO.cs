using UnityEngine;

namespace Integrations.Items
{
    [CreateAssetMenu(fileName = "New Item Script", menuName = "Items/New Script")]
    public class ItemScript_SO : ScriptableObject
    {
        public string Name;

        public ItemScriptDTO Instance => ItemScriptDTO.CreateDTO(Name);
    }
}