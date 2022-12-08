using System;

namespace UnityEngine
{
    public class ChannelingCommand
    {
        public readonly float Max;

        public readonly string Title;
        
        public readonly float Current;

        public Action Callback;

        public ChannelingCommand(string title, float max, float current = 0)
        {
            Title = title;
            Max = max;
            Current = current;
        }
    }
}