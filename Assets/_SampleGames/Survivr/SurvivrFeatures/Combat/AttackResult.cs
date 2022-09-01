using UnityEngine;

namespace _SampleGames.Survivr.SurvivrFeatures.Combat
{
    public class AttackResult
    {
        public readonly GameObject Defender;

        public readonly AttackData AttackMetadataBase;
        
        public readonly HitData HitMetadataBase;

        internal AttackResult(GameObject defender, AttackData attack, HitData hit)
        {
            Defender = defender;
            AttackMetadataBase = attack;
            HitMetadataBase = hit;
        }
    }
}