using Features.Buffs;

namespace DebugScripts.Buffs
{
    public class BuffImplDebug : BuffMetadata
    {
        public BuffImplDebug(string name, float defaultDuration, int maxStack = 1) : base(name, defaultDuration, maxStack)
        {
        }
    }
}