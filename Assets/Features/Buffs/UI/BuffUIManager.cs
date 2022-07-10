using System;
using System.Collections.Generic;

namespace Features.Buffs.UI
{
    public class BuffUIManager
    {
        private Dictionary<string, (IBuffUI ui, ActiveBuff buff)> m_Buffs = new();

        private Action<IBuffUI> m_DestroyAction;

        private Func<IBuffUI> m_InstantiationFunc;

        private BuffController m_Source;

        public void SetSource(BuffController controller, Func<IBuffUI> instantiationFunc, Action<IBuffUI> destroyAction)
        {
            m_Source = controller;

            m_InstantiationFunc = instantiationFunc;

            m_DestroyAction = destroyAction;

            SubscribeToSource();
        }

        public void RemoveSource()
        {
            UnsubscribeFromSource();

            m_Source = null;

            m_InstantiationFunc = null;

            foreach (var valueTuple in m_Buffs)
            {
                m_DestroyAction(valueTuple.Value.ui);
            }

            m_Buffs.Clear();

            m_DestroyAction = null;
        }

        private void HandleRemove(ActiveBuff arg0)
        {
            if (!m_Buffs.TryGetValue(arg0.Metadata.Name, out var tuple)) return;

            m_Buffs.Remove(arg0.Metadata.Name);

            m_DestroyAction(tuple.ui);
        }

        private void HandleTick(float arg0)
        {
            foreach (var valuePair in m_Buffs)
            {
                valuePair.Value.ui.BuffTickCallback(valuePair.Value.buff);
            }
        }

        private void HandleAdd(ActiveBuff arg0)
        {
            var timer = m_InstantiationFunc.Invoke();

            timer.Setup(arg0);

            m_Buffs.Add(arg0.Metadata.Name, (timer, arg0));
        }

        private void SubscribeToSource()
        {
            m_Source.OnBuffAdded.AddListener(HandleAdd);
            m_Source.OnTimerTick.AddListener(HandleTick);
            m_Source.OnBuffRemoved.AddListener(HandleRemove);
        }

        private void UnsubscribeFromSource()
        {
            m_Source.OnBuffAdded.RemoveListener(HandleAdd);
            m_Source.OnTimerTick.RemoveListener(HandleTick);
            m_Source.OnBuffRemoved.RemoveListener(HandleRemove);
        }
    }
}