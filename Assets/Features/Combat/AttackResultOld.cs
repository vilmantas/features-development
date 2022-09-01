using Feature.Combat;

namespace Features.Combat
{
    public class AttackResultOld
    {
        public readonly CombatController Defender;

        public readonly AttackMetadataBase AttackMetadataBase;
        
        public readonly HitMetadataBase HitMetadataBase;

        internal AttackResultOld(CombatController defender, AttackMetadataBase attackMetadataBase, HitMetadataBase hitMetadataBase)
        {
            Defender = defender;
            AttackMetadataBase = attackMetadataBase;
            HitMetadataBase = hitMetadataBase;
        }
    }
}