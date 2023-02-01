using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Features.CharacterModel;
using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    public class InterruptResult
    {
        public readonly IEnumerable<Guid> ActiveAnimationsInterrupted;

        public readonly IEnumerable<Guid> NotStartedAnimationsInterrupted;

        public InterruptResult(IEnumerable<Guid> activeAnimationsInterrupted,
            IEnumerable<Guid> notStartedAnimationsInterrupted)

        {
            ActiveAnimationsInterrupted = activeAnimationsInterrupted;
            NotStartedAnimationsInterrupted = notStartedAnimationsInterrupted;
        }
    }
    
    public class PlayHitboxAnimationPayload
    {
        public readonly Guid AnimationId;
        
        public readonly AnimationConfigurationDTO Configuration;

        public PlayHitboxAnimationPayload(Guid animationId, AnimationConfigurationDTO configuration)
        {
            AnimationId = animationId;
            Configuration = configuration;
        }
    }
    
    public class HitboxAnimationController : MonoBehaviour
    {
        private readonly ConcurrentDictionary<Guid, HitboxPlayer> ActiveHitboxes = new();

        private readonly ConcurrentDictionary<Guid, (PlayHitboxAnimationPayload, Coroutine, bool)> RunningRoutines = new();
        private Transform m_spawn;

        public Action<Collider, List<Collider>> OnAnimationCollision;

        public Action<Guid> OnHitboxActivated;

        public Action OnHitboxFinished;

        private void Start()
        {
            var modelData = transform.root.GetComponentInChildren<CharacterModelController>();

            m_spawn = modelData == null ? transform.root : modelData.WeaponHitboxSpawn;
        }

        public InterruptResult Interrupt()
        {
            var players = ActiveHitboxes.Select(keyValuePair => keyValuePair.Value).ToList();

            ActiveHitboxes.Clear();

            foreach (var hbp in players)
            {
                Destroy(hbp.gameObject);
            }

            var activeInterrupts = new List<Guid>();

            var notStartedInterrupts = new List<Guid>();

            foreach (var tuple in RunningRoutines)
            {
                var animationId = tuple.Value.Item1.AnimationId;
                
                if (tuple.Value.Item3)
                {
                    activeInterrupts.Add(animationId);
                }
                else
                {
                    notStartedInterrupts.Add(animationId);
                }
            }

            RunningRoutines.Clear();

            return new InterruptResult(activeInterrupts, notStartedInterrupts);
        }

        public void Play(PlayHitboxAnimationPayload payload)
        {
            var id = Guid.NewGuid();

            var routine = StartCoroutine(PlayHitbox(payload, id));

            RunningRoutines.TryAdd(id, (payload, routine, false));
        }

        private IEnumerator PlayHitbox(PlayHitboxAnimationPayload payload, Guid id)
        {
            var configurationSo = payload.Configuration;
            
            yield return new WaitForSeconds(configurationSo.DelayBeforeHitboxSpawn);

            if (!RunningRoutines.ContainsKey(id)) yield break;

            RunningRoutines[id] = (RunningRoutines[id].Item1, RunningRoutines[id].Item2, true);
            
            OnHitboxActivated?.Invoke(payload.AnimationId);

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