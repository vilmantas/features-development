using System.Collections.Generic;
using System.Linq;
using Features.Buffs.Events;
using Features.Buffs.UI;
using UnityEngine;

namespace Features.Buffs
{
    public class BuffController : MonoBehaviour
    {
        [HideInInspector] public BuffAddedEvent OnBuffAdded = new();

        [HideInInspector] public BuffRemovedEvent OnBuffRemoved = new();

        [HideInInspector] public BuffStackAddedEvent OnBuffStackAdded = new();

        [HideInInspector] public BuffStackRemovedEvent OnBuffStackRemoved = new();

        [HideInInspector] public BuffTickOccuredEvent OnTickOccured = new();

        private BuffContainer Container;

        private BuffUIManager UIManager;

        public IReadOnlyList<ActiveBuff> ActiveBuffs => Container.Buffs.Where(x => !x.IsDepleted).ToList();

        private void Awake()
        {
            Container = new BuffContainer()
                .RegisterCallbacks(
                    OnBuffRemoved.Invoke, OnBuffAdded.Invoke,
                    OnBuffStackRemoved.Invoke,
                    OnBuffStackAdded.Invoke
                );
        }

        private void Update()
        {
            Container.Tick(Time.deltaTime);

            OnTickOccured?.Invoke(Time.deltaTime);
        }

        public void WithUI(IBuffUI prefab, Transform container)
        {
            UIManager = new BuffUIManager();

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

        public void Receive(BuffBase buff, GameObject source)
        {
            Container.Receive(buff, source);
        }
    }
}