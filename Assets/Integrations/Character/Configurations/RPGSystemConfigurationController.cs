using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

namespace Features.Character.Configurations
{
    public class RPGSystemConfigurationController : SingletonManager<RPGSystemConfigurationController>
    {
        public RPGSystemConfiguration_SO Configuration;

        public static IEnumerable<string> Disables =>
            Instance.Configuration.StatusEffectConfiguration.Disables.StatusEffects.Select(x =>
                x.InternalName);
    }
}