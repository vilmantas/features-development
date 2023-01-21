using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Features.CharacterModel;
using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    public class HitboxAnimationController : MonoBehaviour
    {
        private Transform m_spawn;

        public Action<Collider, List<Collider>> OnAnimationCollision;
        
        public ConcurrentDictionary<Guid, HitboxPlayer> ActiveHitboxes = new();
        
        private void Start()
        {
            var modelData = transform.root.GetComponentInChildren<CharacterModelController>();

            m_spawn = modelData == null ? transform.root : modelData.WeaponHitboxSpawn;
        }

        public void Interrupt()
        {
            var players = ActiveHitboxes.Select(keyValuePair => keyValuePair.Value).ToList();

            ActiveHitboxes.Clear();

            foreach (var hbp in players)
            {
                Destroy(hbp.gameObject);
            }
        }
        
        public void Play(AnimationConfigurationDTO configurationSo)
        {
            StartCoroutine(PlayHitbox(configurationSo));
        }

        private IEnumerator PlayHitbox(AnimationConfigurationDTO configurationSo)
        {
            yield return new WaitForSeconds(configurationSo.DelayBeforeHitboxSpawn);

            var hitbox = Instantiate(configurationSo.HitboxPrefab, m_spawn);

            ActiveHitboxes.TryAdd(hitbox.Id, hitbox);
            
            hitbox.OnCollision += coll => OnAnimationCollision?.Invoke(coll, hitbox.Collisions.ToList());
            
            hitbox.OnDestruction += () => RemoveHitbox(hitbox.Id);
            
            hitbox.Initialize();

            Destroy(hitbox.gameObject, configurationSo.HitboxDuration);
        }

        private void RemoveHitbox(Guid id)
        {
            ActiveHitboxes.TryRemove(id, out _);
        }
    }
}