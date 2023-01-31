using UnityEngine;
using UnityEngine.Serialization;

namespace Features.CharacterModel
{
    public class CharacterModelController : MonoBehaviour
    {
        public GameObject Hitbox;

        public GameObject Avatar;

        public Transform CameraFollow;

        public Transform CameraTarget;

        public Transform WeaponHitboxSpawn;

        public Transform HeadLocation;
    }
}