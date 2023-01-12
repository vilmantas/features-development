using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class ChannelingCommand
    {
        public readonly float Max;

        public readonly string Title;
        
        public readonly float Current;

        public Dictionary<string, object> Data = new();

        public Action<ChannelingItem> Callback { get; private set; }

        public ChannelingCommand(string title, float max, float current = 0)
        {
            Title = title;
            Max = max;
            Current = current;
        }

        public ChannelingCommand WithCallback(Action<ChannelingItem> callback)
        {
            Callback = callback;

            return this;
        }
    }
}