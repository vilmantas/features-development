using UnityEngine;

namespace Features.Stats.Base
{
    public class BaseStatUIData : MonoBehaviour, IStatUIData
    {
        private void Awake()
        {
            OnAwake();
        }

        public void SetData(Stat stat)
        {
            name = stat.Name;

            OnSetData(stat);
        }

        public virtual void OnAwake()
        {
        }

        public virtual void OnSetData(Stat stat)
        {
        }
    }
}