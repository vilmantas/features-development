using System;
using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    [CreateAssetMenu(fileName = "Rename Me", menuName = "Items/Weapon Animations")]
    public class WeaponAnimations_SO : ScriptableObject
    {
        public string Type;

        public WeaponAnimation[] Animations = Array.Empty<WeaponAnimation>();
    }
}