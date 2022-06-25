using Feature.Combat;

namespace Features.Combat
{
    public class AttackResult
    {
        public readonly bool HitSuccess;
        
        public readonly int Damage;

        public readonly CombatController Defender;

        protected AttackResult(bool hitSuccess, int damage, CombatController defender)
        {
            HitSuccess = hitSuccess;
            Damage = damage;
            Defender = defender;
        }

        public override string ToString()
        {
            return $"Success: {HitSuccess}. Damage: {Damage}";
        }
    }
    
    public class FailedAttackResult : AttackResult
    {
        public FailedAttackResult(CombatController defender) : base(false, 0, defender) {}
    }
    
    public class SuccessfulAttackResult : AttackResult
    {
        public SuccessfulAttackResult(CombatController defender, int damage) : base(true, damage, defender) {}
    }
}