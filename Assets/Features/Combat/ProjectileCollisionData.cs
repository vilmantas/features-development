using UnityEngine;

namespace Features.Combat
{
    public class ProjectileCollisionData
    {
        public ProjectileController Projectile { get; }
        
        public readonly GameObject ProjectileParent;

        public readonly Collider OriginalCollider;

        public readonly GameObject ColliderRoot;

        public readonly object Source;

        private bool m_isConsumed;
        
        public bool IsConsumed => m_isConsumed;

        public ProjectileCollisionData(ProjectileController projectile, GameObject parent, GameObject colliderRoot, object source, Collider originalCollider)
        {
            Projectile = projectile;
            ProjectileParent = parent;
            ColliderRoot = colliderRoot;
            Source = source;
            OriginalCollider = originalCollider;
        }

        public void SetProjectileConsumed()
        {
            m_isConsumed = true;
        }
    }
}