using System;

namespace Features.Cooldowns
{
    public class ActiveCooldown
    {
        internal Guid Guid;
        
        public readonly string Name;

        public readonly float StartCooldown;

        public float TimeLeft { get; private set; }

        public ActiveCooldown(string name, float startCooldown)
        {
            Name = name;
            StartCooldown = startCooldown;
            TimeLeft = startCooldown;
        }

        public void Tick(float timeDelta)
        {
            TimeLeft -= timeDelta;
        }

        public bool IsExpired => TimeLeft <= 0f;
    }
}