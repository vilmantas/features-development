using System;
using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    public class HitboxPlayer : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            print("WTF");
        }
    }
}