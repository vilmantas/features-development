using Features.Combat;

namespace _SampleGames.Survivr.SurvivrFeatures.Combat
{
    public class HitData : HitMetadataBase
    {
        public int DamageDealt;

        public HitData(int damageDealt)
        {
            DamageDealt = damageDealt;
        }
    }
}