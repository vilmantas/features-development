using Features.Buffs.UI;
using UnityEngine;

namespace Features.Buffs
{
    public class BaseBuffUIData : MonoBehaviour, IBuffUIData
    {
        public void SetData(ActiveBuff buff)
        {
            OnSetData(buff);
        }

        public virtual void BuffTickCallback(ActiveBuff buff)
        {
        }


        public virtual void OnSetData(ActiveBuff buff)
        {
        }
    }
}