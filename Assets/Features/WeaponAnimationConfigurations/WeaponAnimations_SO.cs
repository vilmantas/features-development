using System;
using System.Linq;
using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    [CreateAssetMenu(fileName = "Rename Me", menuName = "Items/Weapon Animations")]
    public class WeaponAnimations_SO : ScriptableObject
    {
        public string Type;

        public WeaponAnimation_SO[] Animations = Array.Empty<WeaponAnimation_SO>();
        
        public WeaponAnimationsDTO Instance => new()
        {
            Type = Type,
            Animations = Animations.Select(x => x.Instance).ToArray(),
        };
    }
}