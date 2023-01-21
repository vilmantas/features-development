using System;

namespace Features.WeaponAnimationConfigurations
{
    [Serializable]
    public class WeaponAnimation_SO
    {
        public string Type;

        public AnimationConfiguration_SO Animation;

        public WeaponAnimationDTO Instance => new()
        {
            AnimationType = Type,
            Animation = Animation.Instance
        };
    }

    public class WeaponAnimationDTO
    {
        public string AnimationType;

        public AnimationConfigurationDTO Animation;
    }
}