using System;
using System.Collections;
using Features.CharacterModel;
using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    public class HitboxAnimationController : MonoBehaviour
    {
        private Transform m_spawn;

        public Action<Collider> OnColliderCollided;
        
        private void Start()
        {
            var modelData = transform.root.GetComponentInChildren<CharacterModelController>();

            m_spawn = modelData == null ? transform.root : modelData.WeaponHitboxSpawn;
        }

        public void Play(AnimationConfigurationDTO configurationSo)
        {
            StartCoroutine(PlayHitbox(configurationSo));
        }

        private IEnumerator PlayHitbox(AnimationConfigurationDTO configurationSo)
        {
            yield return new WaitForSeconds(configurationSo.DelayBeforeHitboxSpawn);
            
            var hitbox = Instantiate(configurationSo.HitboxPrefab, m_spawn);

            hitbox.OnCollision += OnColliderCollided;
            
            hitbox.Initialize();

            Destroy(hitbox.gameObject, configurationSo.HitboxDuration);
        }
    }
}