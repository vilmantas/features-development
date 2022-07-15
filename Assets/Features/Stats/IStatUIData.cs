using Features.Stats.Base;
using UnityEngine;

namespace Features.Stats.Base
{
    public interface IStatUIData
    {
        GameObject gameObject { get; }
        void SetData(Stat stat);
    }
}