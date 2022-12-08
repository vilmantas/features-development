using System;

namespace UnityEngine
{
    public class ChannelingItem
    {
        public readonly string Title;
        public readonly float MaxDuration;
        public float ChanneledAmount { get; private set; } = 0;

        public ChannelingItem(string title, float maxDuration, float progress)
        {
            Title = title;
            MaxDuration = maxDuration;
            ChanneledAmount = progress;
        }

        public float TimeLeft => Math.Max(MaxDuration - ChanneledAmount, 0);

        public bool IsCompleted => TimeLeft == 0;
        
        public void AddProgress(float deltaTime)
        {
            ChanneledAmount += deltaTime;
        }
    }
}