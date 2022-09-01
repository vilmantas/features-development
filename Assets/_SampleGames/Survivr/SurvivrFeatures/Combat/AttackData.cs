using Features.Combat;
using Features.Items;
using UnityEngine;

namespace _SampleGames.Survivr.SurvivrFeatures.Combat
{
    public class AttackData : AttackMetadataBase
    {
        public readonly int Damage;

        public readonly ItemInstance Weapon;

        public readonly GameObject Source;

        public AttackData(GameObject source, ItemInstance weapon, int damage)
        {
            Damage = damage;
            Source = source;
            Weapon = weapon;
        }
    }
}