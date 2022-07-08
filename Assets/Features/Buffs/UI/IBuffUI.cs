using UnityEngine;

namespace Features.Buffs.UI
{
    public interface IBuffUI
    {
        GameObject gameObject { get; }
        void BuffTickCallback(ActiveBuff buff);
    }
}