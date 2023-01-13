using System;
using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    public class WeaponAnimationsDTO
    {
        public string Type;

        public WeaponAnimation[] Animations = Array.Empty<WeaponAnimation>();
    }
}