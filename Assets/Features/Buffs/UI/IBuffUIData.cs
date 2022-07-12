using UnityEngine;

namespace Features.Buffs.UI
{
    public interface IBuffUIData
    {
        GameObject gameObject { get; }
        void BuffTickCallback(ActiveBuff buff);
        void SetData(ActiveBuff buff);
    }
}