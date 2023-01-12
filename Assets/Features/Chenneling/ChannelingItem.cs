using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class ChannelingItem
    {
        public readonly string Title;
        public readonly float MaxDuration;
        public float ChanneledAmount { get; private set; } = 0;

        public Action<ChannelingItem> Callback;

        public readonly Dictionary<string, object> Data;

        public ChannelingItem(string title, float maxDuration, float progress, Dictionary<string, object> data)
        {
            Title = title;
            MaxDuration = maxDuration;
            ChanneledAmount = progress;
            Data = data;
        }

        public float TimeLeft => Math.Max(MaxDuration - ChanneledAmount, 0);

        public bool IsCompleted => TimeLeft == 0;
        
        public bool IsInterrupted { get; private set; }
        
        public void AddProgress(float deltaTime)
        {
            ChanneledAmount += deltaTime;
        }

        public void Interrupted()
        {
            IsInterrupted = true;
        }
    }
}