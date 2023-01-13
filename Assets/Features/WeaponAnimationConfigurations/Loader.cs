using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    public static class Loader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void RegisterFactory()
        {
            WeaponAnimations_SO[] allItems =
                Resources.LoadAll<WeaponAnimations_SO>("");

            foreach (var item in allItems)
            {
                WeaponAnimationConfigurationRegistry.Register(item);
            }
        }
    }
}