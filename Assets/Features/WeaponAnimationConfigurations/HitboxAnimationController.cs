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
        
        private readonly ConcurrentDictionary<Guid, HitboxPlayer> ActiveHitboxes = new();

        private readonly ConcurrentDictionary<Guid, Coroutine> RunningRoutines = new();

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
            
            RunningRoutines.Clear();
        }
        
        public void Play(AnimationConfigurationDTO configurationSo)
        {
            var id = Guid.NewGuid();
            
            var routine = StartCoroutine(PlayHitbox(configurationSo, id));
            
            RunningRoutines.TryAdd(id, routine);
        }

        private IEnumerator PlayHitbox(AnimationConfigurationDTO configurationSo, Guid id)
        {
            yield return new WaitForSeconds(configurationSo.DelayBeforeHitboxSpawn);

            if (!RunningRoutines.ContainsKey(id)) yield break;

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