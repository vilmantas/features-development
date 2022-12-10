using Features.Targeting;

namespace Features.Character
{
    public class NPC : Modules.Character
    {
        protected override void ConfigureTargetingSystem(TargetProvider provider)
        {
            provider.MousePositionProvider = () => LocationProvider.PlayerLocationProvider(null).transform.position;

            provider.CharacterTargetProvider = LocationProvider.PlayerLocationProvider;
        }
    }
}