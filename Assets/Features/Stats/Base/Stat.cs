using System;

namespace Features.Stats.Base
{
    [Serializable]
    public class Stat
    {
        public string Name;

        public int Value;

        public Stat(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public Stat Copy => new(Name, Value);

        public Stat Combine(Stat other)
        {
            return other.Name != Name ? this : new Stat(Name, Value + other.Value);
        }

        public Stat Combine(int value)
        {
            return new(Name, Value + value);
        }
    }
}