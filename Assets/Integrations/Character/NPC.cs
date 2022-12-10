using Features.Targeting;

namespace Features.Character
{
    public class NPC : Modules.Character
    {
        protected override void ConfigureTargetingSystem(TargetProvider provider)
        {
            provider.CurrentMousePositionProvider = () => LocationProvider.PlayerLocationProvider().transform.position;

            provider.CharacterTargetProvider = LocationProvider.PlayerLocationProvider;
        }
    }
}