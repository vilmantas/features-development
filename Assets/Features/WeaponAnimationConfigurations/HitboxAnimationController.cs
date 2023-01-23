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
        private readonly ConcurrentDictionary<Guid, HitboxPlayer> ActiveHitboxes = new();

        private readonly ConcurrentDictionary<Guid, Coroutine> RunningRoutines = new();
        private Transform m_spawn;

        public Action<Collider, List<Collider>> OnAnimationCollision;

        public Action OnHitboxActivated;

        public Action OnHitboxFinished;

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

            OnHitboxActivated?.Invoke();

            var hitbox = Instantiate(configurationSo.HitboxPrefab, m_spawn);

            ActiveHitboxes.TryAdd(hitbox.Id, hitbox);

            hitbox.OnCollision += coll => OnAnimationCollision?.Invoke(coll, hitbox.Collisions.ToList());

            hitbox.OnDestruction += () => RunBeforeDestroy(hitbox.Id);

            hitbox.Initialize();

            Destroy(hitbox.gameObject, configurationSo.HitboxDuration);
        }

        private void RunBeforeDestroy(Guid id)
        {
            ActiveHitboxes.TryRemove(id, out _);
            OnHitboxFinished?.Invoke();
        }
    }
}