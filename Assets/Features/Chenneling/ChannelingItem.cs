using System;

namespace UnityEngine
{
    public class ChannelingItem
    {
        public readonly float MaxDuration;
        public float ChanneledAmount { get; private set; } = 0;

        public ChannelingItem(float maxDuration, float progress = 0)
        {
            MaxDuration = maxDuration;
            ChanneledAmount = progress;
        }

        public float TimeLeft => Math.Max(MaxDuration - ChanneledAmount, 0);

        public bool IsCompleted => TimeLeft == 0;
        
        public void AddToChannel(float deltaTime)
        {
            ChanneledAmount += deltaTime;
        }
    }
}