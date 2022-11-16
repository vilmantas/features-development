using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Buffs
{
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

        public IReadOnlyList<ActiveBuff> ActiveBuffs => Container.Buffs.Where(x => !x.IsDepleted).ToList();

        private void Awake()
        {
            Container = new BuffContainer();

            Container.OnBuffAdded += OnBuffAdded;
            Container.OnBuffDurationReset += OnBuffDurationReset;
            Container.OnBuffRemoved += OnBuffRemoved;
            Container.OnBuffStackAdded += OnBuffStackAdded;
            Container.OnBuffStackRemoved += OnBuffStackRemoved;
            Container.OnBuffTickOccurred += OnBuffTickOccurred;
        }

        private void Update()
        {
            Container.Tick(Time.deltaTime);

            OnTimerTick?.Invoke(Time.deltaTime);
        }

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