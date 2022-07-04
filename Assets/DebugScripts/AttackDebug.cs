using Features.Combat;

namespace DebugScripts
{
    public class AttackDebug : AttackMetadataBase
    {
        public readonly string AttackType;

        public AttackDebug(string attackType)
        {
            AttackType = attackType;
        }
    }
}