using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Buffs
{
    public class BuffAddOptions
    {
        public BuffAddOptions()
        {
        }

        public BuffAddOptions(BuffBase buff, GameObject source)
        {
            Buff = buff;
            Source = source;
        }

        public BuffAddOptions(BuffBase buff, GameObject source, int stacks)
        {
            Buff = buff;
            Source = source;
            Stacks = stacks;
        }

        public BuffBase Buff { get; set; }
        public GameObject Source { get; set; }
        public int Stacks { get; set; }
        public float Duration { get; set; }
        public bool RequestHandled { get; set; }
    }

    public class BuffRemoveOptions
    {
        public BuffRemoveOptions()
        {
        }

        public BuffRemoveOptions(BuffBase buff)
        {
            Buff = buff;
        }

        public BuffBase Buff { get; set; }
        public bool RequestHandled { get; set; }
    }

    public class BuffController : MonoBehaviour
    {
        private BuffContainer Container;
        public Action<BuffAddOptions> OnBeforeBuffAdd;

        public Action<BuffRemoveOptions> OnBeforeBuffRemoved;

        public Action<ActiveBuff> OnBuffAdded;

        public Action<ActiveBuff> OnBuffDurationReset;

        public Action<ActiveBuff> OnBuffRemoved;

        public Action<ActiveBuff> OnBuffStackAdded;

        public Action<ActiveBuff> OnBuffStackRemoved;

        public Action<ActiveBuff> OnBuffTickOccurred;

        public Action<float> OnTimerTick;

        // private BuffUIManager UIManager;

        public IReadOnlyList<ActiveBuff> ActiveBuffs => Container.Buffs.Where(x => !x.IsDepleted).ToList();

        private void Awake()
        {
            Container = new BuffContainer();

            Container.OnBuffAdded += activeBuff => OnBuffAdded?.Invoke(activeBuff);
            Container.OnBuffDurationReset += activeBuff => OnBuffDurationReset?.Invoke(activeBuff);
            Container.OnBuffRemoved += activeBuff => OnBuffRemoved?.Invoke(activeBuff);
            Container.OnBuffStackAdded += activeBuff => OnBuffStackAdded?.Invoke(activeBuff);
            Container.OnBuffStackRemoved += activeBuff => OnBuffStackRemoved?.Invoke(activeBuff);
            Container.OnBuffTickOccurred += activeBuff => OnBuffTickOccurred?.Invoke(activeBuff);
        }

        private void Update()
        {
            Container.Tick(Time.deltaTime);

            OnTimerTick?.Invoke(Time.deltaTime);
        }
        //
        // public void WithUI(IBuffUIData prefab, Transform container)
        // {
        //     UIManager = new BuffUIManager();
        //
        //     UIManager.SetSource(this,
        //         () =>
        //         {
        //             var instance = Instantiate(prefab.gameObject, container);
        //             return instance.GetComponentInChildren<IBuffUIData>();
        //         },
        //         controller => DestroyImmediate(controller.gameObject));
        // }
        //
        // public void RemoveUI()
        // {
        //     UIManager.RemoveSource();
        // }

        public void Remove(BuffRemoveOptions opt)
        {
            Container.Remove(opt.Buff);
        }

        public void Add(BuffAddOptions opt)
        {
            if (opt.RequestHandled) return;

            Container.Receive(opt.Buff, opt.Source, opt.Stacks, opt.Duration);
        }

        public void AttemptAdd(BuffAddOptions opt)
        {
            OnBeforeBuffAdd?.Invoke(opt);
            Add(opt);
        }

        public void AttemptRemove(BuffRemoveOptions opt)
        {
            OnBeforeBuffRemoved?.Invoke(opt);
            Remove(opt);
        }
    }
}