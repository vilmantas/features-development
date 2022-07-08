using System;
using System.Collections.Generic;
using System.Linq;
using Features.Buffs.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Buffs
{
    public class BuffController : MonoBehaviour
    {
        public BuffAddedEvent OnBuffAdded = new();

        public BuffRemovedEvent OnBuffRemoved = new();

        public BuffStackAddedEvent OnBuffStackAdded = new();

        public BuffStackRemovedEvent OnBuffStackRemoved = new();

        public BuffTickOccuredEvent OnTickOccured = new();

        private BuffContainer Container;

        private BuffUIManager UIManager;

        public IReadOnlyList<ActiveBuff> ActiveBuffs => Container.Buffs.Where(x => !x.IsDepleted).ToList();

        private void Start()
        {
            UIManager = new BuffUIManager();

            Container = new BuffContainer().RegisterCallbacks(OnBuffRemoved.Invoke, OnBuffAdded.Invoke,
                OnBuffStackRemoved.Invoke, OnBuffStackAdded.Invoke);
        }

        private void Update()
        {
            Container.Tick(Time.deltaTime);

            OnTickOccured?.Invoke(Time.deltaTime);
        }

        public void WithUI(IBuffUI prefab, Transform container)
        {
            UIManager.SetSource(this,
                () =>
                {
                    var instance = Instantiate(prefab.gameObject, container);
                    return instance.GetComponentInChildren<IBuffUI>();
                },
                controller => DestroyImmediate(controller.gameObject));
        }

        public void RemoveUI()
        {
            UIManager.RemoveSource();
        }

        public void Remove(BuffBase buff)
        {
            Container.Remove(buff);
        }

        public void Receive(BuffBase buff)
        {
            Container.Receive(buff);
        }
    }


    [Serializable]
    public class BuffRemovedEvent : UnityEvent<ActiveBuff>
    {
    }


    [Serializable]
    public class BuffAddedEvent : UnityEvent<ActiveBuff>
    {
    }

    [Serializable]
    public class BuffStackRemovedEvent : UnityEvent<ActiveBuff>
    {
    }


    [Serializable]
    public class BuffStackAddedEvent : UnityEvent<ActiveBuff>
    {
    }

    [Serializable]
    public class BuffTickOccuredEvent : UnityEvent<float>
    {
    }
}