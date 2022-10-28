using UnityEngine;

namespace Features.Combat
{
    public class ProjectileSpawner : MonoBehaviour
    {
        public static void SpawnProjectile(GameObject projectile, Transform position,
            Vector3 direction)
        {
            var instance = Instantiate(projectile, position);
        }
    }
}