using UnityEngine;

namespace DebugScripts
{
    [CreateAssetMenu(menuName = "Fake Item", fileName = "rename_me")]
    public class FakeItem_SO : ScriptableObject
    {
        public string Name;

        public Sprite Sprite;

        [Min(1)] [Range(1, 1000)] public int MaxStack;

        public FakeItemMetadata GetMetadata => new(Name, Sprite, MaxStack);

        public FakeItemInstance GetInstance => new(GetMetadata);
    }
}