using System.Collections.Generic;

namespace Features.Buffs
{
    public static class BuffDatabase
    {
        private static Dictionary<string, BuffBase> Buffs = new();

        public static void Register(BuffBase buff) => Buffs.Add(buff.Name, buff);

        public static ActiveBuff GetActiveBuff(string name)
        {
            return !Buffs.TryGetValue(name, out var stat) ? null : new ActiveBuff(stat);
        }

        public static bool Exists(string buffName)
        {
            return Buffs.ContainsKey(buffName);
        }
    }
}