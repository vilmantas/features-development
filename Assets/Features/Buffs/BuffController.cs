using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Buffs
{
    public class BuffController : MonoBehaviour
    {
        public int TickIntervalMilliseconds = 100;

        [Min(0)] public int MaxBuffCount = 100;

        private List<ActiveBuff> Buffs = new();

        public void Receive(string buffName)
        {
            var buff = Buffs.FirstOrDefault(x => x.Metadata.Name == buffName);

            if (buff == null)
            {
                if (!BuffDatabase.Exists(buffName)) return;

                buff = BuffDatabase.GetActiveBuff(buffName);

                Buffs.Add(buff);

                buff.ResetDuration();

                StartCoroutine(Ticker(buff));
            }
            else
            {
                if (buff.Stacks.CanReceive)
                {
                    buff.Stacks.Receive(1);
                }

                buff.ResetDuration();
            }
        }

        private IEnumerator Ticker(ActiveBuff buff)
        {
            var interval = TickIntervalMilliseconds / 1000;

            while (true)
            {
                yield return new WaitForSeconds(interval);

                buff.DurationLeft -= interval;

                buff.Metadata.OnTick(interval);
            }
        }
    }
}