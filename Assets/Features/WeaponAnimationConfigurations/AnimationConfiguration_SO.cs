using System;
using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    [CreateAssetMenu(fileName = "Rename Me", menuName = "Items/Animation Configuration")]
    public class AnimationConfiguration_SO : ScriptableObject
    {
        public string AnimationName;

        public float DelayBeforeHitboxSpawn;

        public float HitboxDuration;
    }

    [Serializable]
    public class WeaponAnimation
    {
        public string Type;

        public AnimationConfiguration_SO Animation;
    }

    [CreateAssetMenu(fileName = "Rename Me", menuName = "Items/Weapon Animations")]
    public class WeaponAnimations_SO : ScriptableObject
    {
        public string Type;

        public WeaponAnimation[] Animations = Array.Empty<WeaponAnimation>();
    }
}