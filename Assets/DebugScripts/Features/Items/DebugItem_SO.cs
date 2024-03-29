using UnityEngine;

namespace DebugScripts
{
    [CreateAssetMenu(menuName = "Fake Item", fileName = "rename_me")]
    public class DebugItem_SO : ScriptableObject
    {
        public string Name;

        public Sprite Sprite;

        [Min(1)] [Range(1, 1000)] public int MaxStack = 1;

        public GameObject Model;

        public DebugItemMetadata GetMetadata => new(Name, Sprite, Model, "", "", MaxStack > 1, MaxStack);

        public DebugItemInstance GetInstance => new(GetMetadata);
    }
}