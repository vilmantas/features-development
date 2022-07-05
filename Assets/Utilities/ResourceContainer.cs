using System;

namespace Utilities
{
    public class ResourceContainer
    {
        public ResourceContainer(int max, int current = 0)
        {
            Max = max;

            Current = current > max ? max : current;
        }

        public int Max { get; private set; }

        public int Current { get; private set; }

        public bool IsEmpty => Current == 0;

        public bool HasStuff => !IsEmpty;

        public bool IsFull => Current == Max;

        public bool CanReceive => !IsFull;

        public bool Receive(int value)
        {
            return Receive(value, out var _);
        }

        public bool Receive(int value, out int leftovers)
        {
            leftovers = value;

            if (value <= 0) return false;

            var before = Current;

            Current = Math.Min(Current + value, Max);

            var change = Current - before;

            leftovers -= change;

            return value != leftovers;
        }

        public bool Reduce(int value)
        {
            return Reduce(value, out var _);
        }

        public bool Reduce(int value, out int leftovers)
        {
            leftovers = value;

            if (value <= 0) return false;

            var before = Current;

            Current = Math.Max(Current - value, 0);

            var change = before - Current;

            leftovers -= change;

            return value != leftovers;
        }
    }
}