using System;
using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    [CreateAssetMenu(fileName = "Rename Me", menuName = "Items/Animation Configuration")]
    public class AnimationConfiguration_SO : ScriptableObject
    {
        public string AnimationName;

        public float AnimationDuration;
        
        public float DelayBeforeHitboxSpawn;

        public float HitboxDuration;

        public HitboxPlayer HitboxPrefab;

        public AnimationConfigurationDTO Instance => new()
        {
            AnimationName = AnimationName,
            AnimationDuration = AnimationDuration,
            DelayBeforeHitboxSpawn = DelayBeforeHitboxSpawn,
            HitboxDuration = HitboxDuration,
            HitboxPrefab = HitboxPrefab,
        };
    }
}