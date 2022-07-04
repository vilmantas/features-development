using Features.Combat;

namespace DebugScripts
{
    public class HitDebug : HitMetadataBase
    {
        public readonly int Damage;
        
        public HitDebug(int damage)
        {
            Damage = damage;
        }
    }
}