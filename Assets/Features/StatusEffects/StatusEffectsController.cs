using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Conditions
{
    public class StatusEffectsController : MonoBehaviour
    {
        public Action<StatusEffectMetadata> OnStatusEffectAdded;

        public Action<StatusEffectMetadata> OnStatusEffectRemoved;

        public List<ActiveStatusEffect> StatusEffects = new();

        public void AddCondition(StatusEffectMetadata effectMetadata)
        {
            if (!StatusEffectImplementationRegistry.Implementations.TryGetValue(effectMetadata.InternalName,
                    out StatusEffectImplementation impl)) return;
            
            StatusEffects.Add(new ActiveStatusEffect(effectMetadata, impl));
            
            impl.OnStatusEffectApplied.Invoke();
            
            OnStatusEffectAdded?.Invoke(effectMetadata);
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
}