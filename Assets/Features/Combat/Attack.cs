using System;

namespace Features.Combat
{
    public class Attack
    {
        public readonly AttackInfo Info;

        public readonly Action<AttackResult> ResultCallback;

        public Attack(AttackInfo info, Action<AttackResult> resultCallback)
        {
            Info = info;
            ResultCallback = resultCallback;
        }
    }
}