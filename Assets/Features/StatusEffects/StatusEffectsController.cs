using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Conditions
{
    public class StatusEffectsController : MonoBehaviour
    {
        public Action<StatusEffectAddPayload> OnBeforeAdd;

        public Action<StatusEffectRemovePayload> OnBeforeRemove;

        public Action<StatusEffectMetadata> OnAdded;

        public Action<StatusEffectMetadata> OnRemoved;

        public List<ActiveStatusEffect> StatusEffects = new();

        public bool IsAffectedBy(string effect) =>
            StatusEffects.Any(x => x.Metadata.InternalName.Equals(effect));

        public void AddStatusEffect(StatusEffectAddPayload payload)
        {
            if (!StatusEffectImplementationRegistry.Implementations.TryGetValue(payload.Metadata.InternalName,
                    out StatusEffectImplementation impl)) return;

            if (StatusEffects.Any(
                    x => x.Metadata.InternalName.Equals(payload.Metadata.InternalName))) return;
            
            OnBeforeAdd?.Invoke(payload);

            if (payload.PreventDefault) return;
            
            StatusEffects.Add(new ActiveStatusEffect(payload.Metadata, impl));
            
            impl.Apply(new StatusEffectPayload(transform.root.gameObject));
            
            OnAdded?.Invoke(payload.Metadata);
        }

        public void RemoveStatusEffect(StatusEffectRemovePayload payload)
        {
            var activeEffect = StatusEffects.FirstOrDefault(x => x.Metadata.Equals(payload.Metadata));

            if (activeEffect == null) return;
            
            OnBeforeRemove?.Invoke(payload);

            if (payload.PreventDefault) return;
         
            activeEffect.Implementation.Remove(new StatusEffectPayload(transform.root.gameObject));

            StatusEffects.Remove(activeEffect);
            
            OnRemoved?.Invoke(payload.Metadata);
        }
    }

    public class ActiveStatusEffect
    {
        public readonly StatusEffectMetadata Metadata;

        public readonly StatusEffectImplementation Implementation;

        public ActiveStatusEffect(StatusEffectMetadata metadata, StatusEffectImplementation implementation)
        {
            Metadata = metadata;
            Implementation = implementation;
        }
    }

    public class StatusEffectAddPayload
    {
        public readonly StatusEffectMetadata Metadata;

        public bool PreventDefault;

        public StatusEffectAddPayload(StatusEffectMetadata metadata)
        {
            Metadata = metadata;
        }
    }
    
    public class StatusEffectRemovePayload
    {
        public readonly StatusEffectMetadata Metadata;

        public bool PreventDefault;

        public StatusEffectRemovePayload(StatusEffectMetadata metadata)
        {
            Metadata = metadata;
        }
    }
}