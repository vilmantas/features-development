using Feature.Combat;

namespace Features.Combat
{
    public class AttackResult
    {
        public readonly CombatController Defender;

        public readonly AttackMetadataBase AttackMetadataBase;
        
        public readonly HitMetadataBase HitMetadataBase;

        internal AttackResult(CombatController defender, AttackMetadataBase attackMetadataBase, HitMetadataBase hitMetadataBase)
        {
            Defender = defender;
            AttackMetadataBase = attackMetadataBase;
            HitMetadataBase = hitMetadataBase;
        }
    }
}